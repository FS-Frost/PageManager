using Newtonsoft.Json;
using System.Collections.Generic;

namespace PageManager.Core {
    public class GetPostsResponse : ApiResponse {
        [JsonProperty("data")]
        public List<Post> Posts { get; set; }

        [JsonProperty("paging")]
        public Paging Paging { get; set; }
    }

    public class Paging {
        [JsonProperty("cursors")]
        public Cursors Cursors { get; set; }

        [JsonProperty("next")]
        public string Next { get; set; }
    }

    public class Cursors {
        [JsonProperty("before")]
        public string Before { get; set; }

        [JsonProperty("after")]
        public string After { get; set; }
    }
}
