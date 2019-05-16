using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flappy
{    
    class BestController : MaximaxController
    {
        private int anticipation = 15;
        private bool firstTime = true;
        private Tree FollowingActions;


        public bool ShouldJump(Bird bird, Drawer drawer, long x, Deque<Pipe> pipes)
        {
            if (firstTime)
            {
                FollowingActions = new Tree(bird);
                FollowingActions.jump = _buildTree(bird.DeepCopy(new Jump()), anticipation, drawer, x + 1, pipes.DeepCopy(pipe => pipe.DeepCopy()));
                FollowingActions.fall = _buildTree(bird.DeepCopy(new Fall()), anticipation, drawer, x + 1, pipes.DeepCopy(pipe => pipe.DeepCopy()));

                firstTime = false;
            }
            else
            {
                FollowingActions = _completeTree(FollowingActions, drawer, x, pipes.DeepCopy(pipe => pipe.DeepCopy()));
            }

            if (FollowingActions.jump.survive >= FollowingActions.fall.survive)
            {
                FollowingActions = FollowingActions.jump;
                return true;
            }
            else
            {
                FollowingActions = FollowingActions.fall;
                return false;
            }
        }

        private Tree _buildTree(Bird bird, int remaining, Drawer drawer, long x, Deque<Pipe> pipes)
        {
            // Update bird's position
            bird.Update(drawer, x, pipes);
            
            // Delete passed pipes
            while (pipes.PeekFront().X < x - 2)
                pipes.PopFront();

            // Check if the deepCopy died
            Pipe first = pipes.PeekFront();
            if (first.Collides(x + 2, bird.Y))
                return new Tree(bird, -1);
            
            // Stop generating the tree
            if (remaining <= 0)
                return new Tree(bird, 1);
            
            // Building tree
            Tree tree = new Tree(bird);
            tree.jump = _buildTree(bird.DeepCopy(new Jump()), remaining - 1, drawer, x + 1, pipes.DeepCopy(pipe => pipe.DeepCopy()));
            tree.fall = _buildTree(bird.DeepCopy(new Fall()), remaining - 1, drawer, x + 1, pipes.DeepCopy(pipe => pipe.DeepCopy()));
            tree.survive = tree.jump.survive + tree.jump.survive;

            tree.x = x;

            return tree;
        }

        private Tree _completeTree(Tree actualTree, Drawer drawer, long x, Deque<Pipe> pipes)
        {
            // Don't generate more possibilities if this one can't survive
            if (actualTree.survive == -1 && actualTree.jump == null)
                return actualTree;

            // Add one level to the tree
            if (actualTree.jump == null)
            {
                actualTree = _buildTree(actualTree.bird, 1, drawer, x, pipes.DeepCopy(pipe => pipe.DeepCopy()));
            }
            else
            {
                actualTree.jump = _completeTree(actualTree.jump, drawer, x + 1, pipes.DeepCopy(pipe => pipe.DeepCopy()));
                actualTree.fall = _completeTree(actualTree.fall, drawer, x + 1, pipes.DeepCopy(pipe => pipe.DeepCopy()));
                actualTree.survive = actualTree.jump.survive + actualTree.fall.survive;
            }
            
            return actualTree;
        }
    }
}
