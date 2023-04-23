using System.Collections.Generic;

namespace SharedKernel.Model
{
    public class BaseResponse<T>  : BaseResponse
    {
        public new T Data { get; set; }
        public BaseResponse() 
        {
        }
        public  BaseResponse(T data, string message = null) 
        {
            Message = message;
            Data = data;
        }

        public BaseResponse(T data, string message = null, string status= "")
        {
            Message = message;
            Data = data;
            Status = status;
        }
        public BaseResponse(string message)
        {
            Message = message;
        }
     
     

        public static BaseResponse<T> Ok(T data, string message = null, string status="success")
        {
            return new BaseResponse<T>(data, message, status)
            {
                Succeeded = true
            };
        }

        public static BaseResponse Failed( List<string> errors = null, string message = null, string status = "failed")
        {
            return new BaseResponse( message)
            {
                Succeeded = false, Errors = errors, Status = status
            };
        }

   


    }

    public class BaseResponse 
    {
        public bool Succeeded;
        public string Message { get; set; }
        public string Status { get; set; }
        public string ErrorCode { get; set; }
        public List<string> Errors;
        public  object Data { get; set; }

        public BaseResponse(object data, string message = null)
        {
            Message = message;
            Data = data;
        }

        public BaseResponse(object data, string message = null, string status = "")
        {
            Message = message;
            Data = data;
            Status = status;
        }

        protected BaseResponse()
        {
            
        }

    

        public BaseResponse(string message)
        {
            Message = message;
        }



        public static BaseResponse<object> Ok(object data, string message = null, string status = "success")
        {
            return new BaseResponse<object>(data, message, status)
            {
                Succeeded = true
            };
        }

        public static BaseResponse<object> Failed(List<string> errors = null, string message = null,
            string status = "failed")
        {
            return new BaseResponse<object>(null, message, status)
            {
                Succeeded = false, Errors = errors
            };
        }

     

    }
}
