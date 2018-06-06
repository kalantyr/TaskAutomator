using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskAutomator.Core
{
    public class Job
    {
        private readonly ITaskSelector _selector;
        private readonly ITaskProcessor _processor;

        public Job(ITaskSelector selector, ITaskProcessor processor)
        {
            _selector = selector ?? throw new ArgumentNullException(nameof(selector));
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        public ActionResult<string> Run()
        {
            var errors = new List<string>();

            foreach (var taskId in _selector.GetTaskIds())
            {
                var processorResult = _processor.Process(taskId);
                if (!processorResult.IsSuccess)
                    errors.Add(processorResult.Error);
            }

            if (!errors.Any())
                return ActionResult<string>.Success(string.Empty);
            else
                return ActionResult<string>.Fail(new Exception(string.Join(Environment.NewLine, errors)));
        }
    }
}
