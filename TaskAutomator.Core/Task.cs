using System;

namespace TaskAutomator.Core
{
    public class Task
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Uri Link { get; set; }

        public string Description { get; set; }
    }
}
