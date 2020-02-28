using System;

namespace DependencyInjectionDemo.Client
{
    public class MenuItem
    {
        public string Title { get; set; }
        public Action Command { get; set; }
    }
}
