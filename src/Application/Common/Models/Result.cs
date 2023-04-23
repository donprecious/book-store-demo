namespace BookStore.Application.Common.Models
{
    public class Result
    {
        public Result()
        {
            
        }
        internal Result(bool succeeded, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
        }

        public string Message { get; set; }
        public bool Succeeded { get; set; }

        public string[] Errors { get; set; }

        public static Result Success()
        {
            return new Result(true, Array.Empty<string>());
        }

        public static Result Failure(IEnumerable<string> errors)
        {
            return new Result(false, errors);
        }
        public static Result Failure( string message)
        {
          
            var  errors = new[] { message };
          
            var result = new Result(false, errors) { Message = message };
            return result;
        }
      
        public static Result Failure(IEnumerable<string> errors, string message)
        {
            return new Result(false, errors) { Message = message };
        }
    }

    public class Result <T> : Result
    {
        internal Result(bool succeeded, string?[] errors) : base(succeeded, errors)
        {
        }

        public Result()
        {
            
        }

     
        public T Data { get; set; }
        public static Result<T> Success(T data, string message = "success")
        {
            var result = new Result<T>(true, Array.Empty<string>()) { Data = data, Message = message };
            return result;
        }

        public static Result<T> Failure(string?[] errors, string message)
        {
            if (errors == null)
            {
                errors = new[] { "" };
            }
            var result = new Result<T>(false, errors) { Message = message };
            return result;
        }
      
        public static Result<T>  Failure(string?[] errors)
        {
            return new Result<T>(false, errors);
        }

        public static Result<T> Failure( string message)
        {
          
               var  errors = new[] { message };
          
            var result = new Result<T>(false, errors) { Message = message };
            return result;
        }
    }


}