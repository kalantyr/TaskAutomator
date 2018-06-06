using System;
using System.Collections.Generic;

namespace TaskAutomator.Core
{
    public interface ITaskSelector
    {
        IReadOnlyCollection<string> GetTaskIds();
    }

    public class SimpleTaskSelector : ITaskSelector
    {
        private readonly IReadOnlyCollection<string> _ids;

        public SimpleTaskSelector(IReadOnlyCollection<string> ids)
        {
            _ids = ids ?? throw new ArgumentNullException(nameof(ids));
        }

        public IReadOnlyCollection<string> GetTaskIds()
        {
            return _ids;
        }
    }
}
