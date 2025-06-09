namespace SimpleStoreAPI.DTOs
{
    public class Result
    {
        public bool Succeeded { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public static Result Success()
        {
            return new Result { Succeeded = true };
        }

        public static Result Failed(string error)
        {
            return new Result { Succeeded = false, Errors = new List<string> { error } };
        }
        public static Result Failed(List<string> errors)
        {
            return new Result { Succeeded = false, Errors = errors };
        }
    }

    public class Result<T>
    {
        public T? Data { get; set; }
        public bool Succeeded { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public static Result<T> Success()
        {
            return new Result<T> { Succeeded = true };
        }
        public static Result<T> Success(T data)
        {
            return new Result<T>() { Succeeded = true, Data = data };
        }

        public static Result<T> Failed(string error)
        {
            return new Result<T> { Succeeded = false, Errors = new List<string> { error } };
        }
        public static Result<T> Failed(List<string> errors)
        {
            return new Result<T> { Succeeded = false, Errors = errors };
        }
    }
}
