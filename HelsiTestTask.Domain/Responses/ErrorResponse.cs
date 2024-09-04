using Newtonsoft.Json;

namespace HelsiTestTask.Domain.Responses
{
    public class ErrorResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonIgnore]
        public int StatusCode { get; set; }
    }
}
