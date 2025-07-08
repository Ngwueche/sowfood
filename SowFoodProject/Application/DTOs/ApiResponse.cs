using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SowFoodProject.Application.DTOs
{
    public static class BaseApiResponse
    {
        public static ApiResponse Success(object? data, string responseMessage = "Success", string responseCode = "00")
        {
            return new ApiResponse
            {
                Data = data,
                ResponseCode = responseCode,
                IsSuccessful = true,
                ResponseMessage = responseMessage
            };
        }
        public static ApiResponse Fail(string responseMessage, string responseCode)
        {
            return new ApiResponse
            {
                ResponseCode = responseCode,
                IsSuccessful = false,
                ResponseMessage = responseMessage
            };
        }
        public class ApiResponse
        {
            public string ResponseCode { get; set; }
            public string ResponseMessage { get; set; }
            public bool IsSuccessful { get; set; }
            public object? Data { get; set; }
        }
    }
}
