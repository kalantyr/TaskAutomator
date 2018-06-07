namespace TaskAutomator.Core
{
    public interface ITaskService
    {
        string Ping();

        ActionResult<Task> GetTask(string id);

        ActionResult<bool> UpdateTask(Task task);
    }
}
