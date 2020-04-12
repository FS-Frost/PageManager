using Newtonsoft.Json;

namespace PageManager.Core {
    public class ApiError {
        public string Message { get; set; }
        public string Type { get; set; }
        public int Code { get; set; }

        [JsonProperty("fbtrace_id")]
        public string TraceId { get; set; }

        public override string ToString() {
            return $"Message: {Message}\n" +
                $"Type: {Type}\n" +
                $"Code: {Code}\n" +
                $"Trace ID: {TraceId}";
        }
    }
}
