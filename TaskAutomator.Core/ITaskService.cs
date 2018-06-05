namespace TaskAutomator.Core
{
    public interface ITaskService
    {
        string Ping();

        Task GetTask(string id);

        ActionResult<bool> UpdateTask(Task task);
    }
}
