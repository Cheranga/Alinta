namespace Alinta.Core
{
    public class OperationResult
    {
        public bool Status { get; set; }
        public string Message { get; set; }

        public static OperationResult Failure()
        {
            return new OperationResult
            {
                Status = false
            };
        }

        public static OperationResult Failure(string message)
        {
            return new OperationResult
            {
                Status = false,
                Message = message
            };
        }

        public static OperationResult Success()
        {
            return new OperationResult
            {
                Status = true
            };
        }

    }

    public class OperationResult<T> : OperationResult
    {
        public T Data { get; set; }

        public static OperationResult<T> Success(T data)
        {
            return new OperationResult<T>
            {
                Status = true,
                Data = data
            };
        }

        public new static OperationResult<T> Failure()
        {
            return new OperationResult<T>
            {
                Status = false
            };
        }

        public new static OperationResult<T> Failure(string message)
        {
            return new OperationResult<T>
            {
                Status = false,
                Message = message
            };
        }
    }
}