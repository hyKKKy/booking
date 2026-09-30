namespace booking.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public Error? Error { get; }

        private Result(bool isSuccess, T? value, Error? error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        public static Result<T> Success(T value) => new(true, value, null);

        public static Result<T> Failure(ErrorType type, string message) =>
            new(false, default, new Error(type, message));
    }

    public class Result
    {
        public bool IsSuccess { get; }
        public Error? Error { get; }

        private Result(bool isSuccess, Error? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, null);

        public static Result Failure(ErrorType type, string message) =>
            new(false, new Error(type, message));
    }
}
