using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flappy
{
    public class MaximaxController : Controller
    {
        /// <summary>
        /// Returns whether the bird should jump with maximax
        /// </summary>
        /// <param name="bird">The bird</param>
        /// <param name="drawer">The drawer</param>
        /// <param name="x">The x position</param>
        /// <param name="pipes">The list of pipes</param>
        /// <returns>True if the bird should jump, false otherwise</returns>

        public bool ShouldJump(Bird bird, Drawer drawer, long x, Deque<Pipe> pipes)
        {
            int anticipation = 20;
            return recShouldJump(bird.DeepCopy(new Jump()), drawer, x + 1, pipes.DeepCopy(pipe => pipe.DeepCopy()), anticipation);
        }

        private bool recShouldJump(Bird bird, Drawer drawer, long x, Deque<Pipe> pipes, int anticipation)
        {
            bird.Update(drawer, x, pipes);
                
            // Delete passed pipes
            while (pipes.PeekFront().X < x - 2)
                pipes.PopFront();
            
            Pipe first = pipes.PeekFront();
            return anticipation == 0 || (!first.Collides(x, bird.Y) && (recShouldJump(bird.DeepCopy(new Jump()), drawer,
                                                                            x + 1, pipes.DeepCopy(pipe => pipe.DeepCopy()),
                                                                            anticipation - 1) ||
                                                                        recShouldJump(bird.DeepCopy(new Fall()), drawer,
                                                                            x + 1, pipes.DeepCopy(pipe => pipe.DeepCopy()),
                                                                            anticipation - 1)));
        }
    }
}
