using System.Text.Json.Serialization;

namespace PhoneBook.Domain.Dto.Api
{
    public class ResponseDto<T>
    {
        public T Data { get; set; }
        [JsonIgnore]
        public int StatusCode { get; set; }
        [JsonIgnore]
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; }
        public ResponseDto<T> Success(T data, int statusCode)
        {
            return new ResponseDto<T> { Data = data, StatusCode = statusCode, IsSuccess = true };
        }
        public ResponseDto<T> Success(int statusCode)
        {
            return new ResponseDto<T> { Data = default(T), StatusCode = statusCode, IsSuccess = true };
        }

        public ResponseDto<T> Fail(List<string> errors, int statusCode)
        {
            return new ResponseDto<T> { Errors = errors, StatusCode = statusCode, IsSuccess = false };
        }

        public ResponseDto<T> Fail(string errors, int statusCode)
        {
            return new ResponseDto<T> { Errors = new List<string>() { errors }, StatusCode = statusCode, IsSuccess = false };
        }
    }
}
