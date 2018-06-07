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
            var getTaskResult = _taskService.GetTask(taskId);
            if (!getTaskResult.IsSuccess)
                return ActionResult<bool>.Fail(getTaskResult.Exception);
            var task = getTaskResult.Data;

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

        public static ActionResult<string> IdToLink(string taskId, ITaskService taskService, Uri baseUri)
        {
            var getTaskResult = taskService.GetTask(taskId);
            if (!getTaskResult.IsSuccess)
                return ActionResult<string>.Fail(getTaskResult.Exception);
            var task = getTaskResult.Data;

            try
            {
                var hint = string.Empty;
                var link = task.Link;
                var text = task.Id + " " + task.Name;
                var result = $"<a aria-label=\"{hint}\" href=\"{link}\">{text}</a>";
                return ActionResult<string>.Success(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ActionResult<string>.Fail(e);
            }
        }
    }
}
