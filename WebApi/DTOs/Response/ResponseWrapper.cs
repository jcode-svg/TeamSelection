namespace WebApi.DTOs.Response
{
    public class ResponseWrapper
    {
        public string Message { get; set; } = null!;
        public bool IsSuccessful { get; set; }
        public int StatusCode { get; set; }
    }

    public class ResponseWrapper<T> : ResponseWrapper where T : class
    {
        public T ResponseObject { get; set; } = null!;
        public bool ResponseObjectExists => ResponseObject != null;

        public static ResponseWrapper<T> Success(T instance, string message = "Successful") => new ResponseWrapper<T>()
        {
            ResponseObject = instance,
            Message = message,
            IsSuccessful = true
        };

        public static ResponseWrapper<T> Error(string error, string message = "", int statusCode = 400) => new ResponseWrapper<T>()
        {
            IsSuccessful = false,
            Message = string.IsNullOrWhiteSpace(message) ? error : message,
            StatusCode = statusCode
        };
    }

}
