using System;

namespace Navicon.Common
{
    public class Result
    {
        protected Result(bool success, string error)
        {
            switch (success)
            {
                case true when error != string.Empty:
                    throw new InvalidOperationException();
                case false when error == string.Empty:
                    throw new InvalidOperationException();
                default:
                    Success = success;
                    Error = error;
                    break;
            }
        }

        public bool Success { get; }
        public string Error { get; }
        public bool IsFailure => !Success;

        public static Result Fail(string message)
        {
            return new Result(false, message);
        }

        public static Result<T> Fail<T>(string message)
        {
            return new Result<T>(default, false, message);
        }

        public static Result Ok()
        {
            return new Result(true, string.Empty);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, string.Empty);
        }
    }

    public class Result<T> : Result
    {
        protected internal Result(T value, bool success, string error)
            : base(success, error)
        {
            Value = value;
        }

        public T Value { get; set; }
    }
}