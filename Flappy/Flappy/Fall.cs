using System;

namespace Flappy
{
    public class Fall : Controller
    {
        public bool ShouldJump(Bird bird, Drawer drawer, long x, Deque<Pipe> pipes)
        {
            return false;
        }
    }
}