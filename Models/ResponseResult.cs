namespace UserManagementAPI.Models
{
    public class ResponseResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ResponseResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public ResponseResult(bool success, string message, T? data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }

    }
}
