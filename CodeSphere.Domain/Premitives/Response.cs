using System.Net;

namespace CodeSphere.Domain.Premitives
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public bool IsSuccess { get; set; } = true;
        public object? Data { get; set; }
        public string Message { get; set; } = "";
        public async static Task<Response> SuccessAsync(object data, string message)
        {
            await Task.CompletedTask;
            Response response = new Response()
            {
                IsSuccess = true,
                Message = message,
                Data = data
            };

            return response;
        }
        public async static Task<Response> SuccessAsync(object data)
        {
            await Task.CompletedTask;
            Response response = new Response()
            {
                IsSuccess = true,
                Data = data
            };

            return response;
        }
        public async static Task<Response> SuccessAsync(string message)
        {

            await Task.CompletedTask;
            Response response = new Response()
            {
                IsSuccess = true,
                Message = message
            };

            return response;
        }



        public async static Task<Response> FailureAsync(object data, string message)
        {
            await Task.CompletedTask;
            Response response = new Response()
            {
                StatusCode = HttpStatusCode.BadRequest,
                IsSuccess = false,
                Message = message,
                Data = data
            };

            return response;
        }
        public async static Task<Response> FailureAsync(string message)
        {
            await Task.CompletedTask;
            Response response = new Response()
            {
                StatusCode = HttpStatusCode.BadRequest,
                IsSuccess = false,
                Message = message
            };

            return response;
        }
        public async static Task<Response> FailureAsync(string message, HttpStatusCode statusCode)
        {
            await Task.CompletedTask;
            Response response = new Response()
            {
                StatusCode = statusCode,
                IsSuccess = false,
                Message = message
            };

            return response;
        }
    }
}
