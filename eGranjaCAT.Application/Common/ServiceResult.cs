namespace eGranjaCAT.Application.Common
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new();
        public int StatusCode { get; set; } = 200;

        public static ServiceResult<T> Ok(T data, int statusCode = 200) => new()
        {
            Success = true,
            Data = data,
            StatusCode = statusCode
        };

        public static ServiceResult<T> Fail(List<string> errors, int statusCode = 400) => new()
        {
            Success = false,
            Errors = errors,
            StatusCode = statusCode
        };

        public static ServiceResult<T> Fail(string error, int statusCode = 400) => Fail(new List<string> { error }, statusCode);

        public static ServiceResult<T> FromException(Exception ex, int statusCode = 500) => new()
        {
            Success = false,
            Errors = new List<string> { ex.Message },
            StatusCode = statusCode
        };
    }
}
