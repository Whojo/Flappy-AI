using System;

namespace Flappy
{
    public class Jump : Controller
    {
        public bool ShouldJump(Bird bird, Drawer drawer, long x, Deque<Pipe> pipes)
        {
            return true;
        }
    }
}