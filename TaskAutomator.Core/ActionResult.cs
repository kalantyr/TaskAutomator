using System;

namespace TaskAutomator.Core
{
    public class ActionResult<T>
    {
        public T Data { get; set; }

        public Exception Exception { get; private set; }

        public string Error { get; set; }

        public bool IsSuccess => string.IsNullOrWhiteSpace(Error);

        public static ActionResult<T> Success(T data)
        {
            return new ActionResult<T> { Data = data };
        }

        public static ActionResult<T> Fail(Exception error)
        {
            return new ActionResult<T> { Exception = error };
        }
    }
}
