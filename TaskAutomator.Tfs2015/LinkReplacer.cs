using System;
using System.Text.RegularExpressions;
using TaskAutomator.Core;

namespace TaskAutomator.Tfs2015
{
    public class LinkReplacer: ITaskProcessor
    {
        private readonly ITaskService _taskService;

        public LinkReplacer(ITaskService taskService)
        {
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
        }

        public ActionResult<bool> Process(string taskId)
        {
            var task = _taskService.GetTask(taskId);

            try
            {
                task.Description = ReplaceLinks(task.Description);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ActionResult<bool>.Fail(e);
            }

            var updateResult = _taskService.UpdateTask(task);
            if (!updateResult.IsSuccess)
                return ActionResult<bool>.Fail(updateResult.Exception);

            return ActionResult<bool>.Success(true);
        }

        internal static string ReplaceLinks(string htmlText)
        {
            foreach (Match match in Regex.Matches(htmlText, "(\\d{6}(?=[^\\d]))"))  // все шестизначные числа
            {
                match.Equals(null);
            }

            throw new NotImplementedException();
        }
    }
}
