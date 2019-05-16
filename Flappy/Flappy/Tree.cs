using System;

namespace Flappy
{
    public class Tree
    {
        /// <summary>
        /// This Tree contains the possible path the bird can take
        /// </summary>

        public Tree jump;
        public Tree fall;
        public int survive;
        public Bird bird;
        public long x;

        public Tree(Bird bird, int survive = 0)
        {
            this.bird = bird;
            this.survive = survive;
            jump = null;
            fall = null;
        }

        public void Print()
        {
            Console.WriteLine(this.survive);
            _Print(this.jump, "  ");
            _Print(this.fall, "  ");
            Console.ReadLine();
        }

        private void _Print(Tree tree, string str)
        {
            if (tree == null)
                return;
            Console.WriteLine(str + tree.survive);
            str += "  ";
            _Print(tree.jump, str);
            _Print(tree.fall, str);
        }
    }
}