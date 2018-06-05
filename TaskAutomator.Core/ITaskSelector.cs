using System.Collections.Generic;

namespace TaskAutomator.Core
{
    public interface ITaskSelector
    {
        IReadOnlyCollection<string> GetTaskIds();
    }
}
