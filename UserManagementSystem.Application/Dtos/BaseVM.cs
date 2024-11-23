using Newtonsoft.Json;
using System.Net;
using System.Text.Json.Serialization;

namespace UserManagementSystem.Application.Dtos
{
    public class BaseVM
    {
        [JsonProperty("status")]
        public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;

        [JsonProperty("messages")]
        public List<ResponseMessage> Messages { get; set; } = new List<ResponseMessage>();

        public BaseVM() { }
        public BaseVM(HttpStatusCode code, params ResponseMessage[] messages)
        {
            this.Status = code;
            this.Messages = messages?.ToList() ?? new List<ResponseMessage>();
        }
    }
    public class BaseVM<T> : BaseVM
    {
        public T Data { get; set; }
        public BaseVM() : base() { }
        public BaseVM(HttpStatusCode code, params ResponseMessage[] messages) : base(code, messages) { }
    }

    public class ResponseMessage
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
