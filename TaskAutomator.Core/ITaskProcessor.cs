namespace TaskAutomator.Core
{
    public interface ITaskProcessor
    {
        ActionResult<bool> Process(string taskId);
    }
}
