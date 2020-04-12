using Flurl;
using Flurl.Http;
using System.Threading.Tasks;

namespace PageManager.Core {
    public class Connector {
        private string _PathRoot => "https://graph.facebook.com";
        private string _PostFields => "id,created_time,message,permalink_url,full_picture,attachments{url}";
        public string AccessToken { get; set; }
        public int DataLimit { get; set; }
        public bool PrettyJson { get; set; }

        public Task<GetPostsResponse> GetLatestPosts(int? limit = null) {
            var options = new {
                fields = _PostFields,
                limit = limit == null ? DataLimit : limit,
                pretty = PrettyJson == true ? 1 : 0,
                access_token = AccessToken
            };

            return _PathRoot
            .AppendPathSegment("me")
            .AppendPathSegment("feed")
            .SetQueryParams(options)
            .GetJsonAsync<GetPostsResponse>();
        }

        public Task<SuccessResponse> UpdatePost(string id, string message) {
            var options = new {
                pretty = PrettyJson == true ? 1 : 0,
                access_token = AccessToken
            };

            var payload = new {
                message
            };

            return _PathRoot
            .AppendPathSegment(id)
            .SetQueryParams(options)
            .AllowAnyHttpStatus()
            .PostJsonAsync(payload)
            .ReceiveJson<SuccessResponse>();
        }

        public Task<SuccessResponse> DeletePost(string id) {
            var options = new {
                pretty = PrettyJson == true ? 1 : 0,
                access_token = AccessToken
            };

            return _PathRoot
            .AppendPathSegment(id)
            .SetQueryParams(options)
            .DeleteAsync()
            .ReceiveJson<SuccessResponse>();
        }

        public Task<CreatePostResponse> CreatePost(string message, string link = null) {
            var options = new {
                fields = _PostFields,
                pretty = PrettyJson == true ? 1 : 0,
                access_token = AccessToken
            };

            var payload = new {
                message,
                link
            };

            return _PathRoot
            .AppendPathSegment("me")
            .AppendPathSegment("feed")
            .SetQueryParams(options)
            .PostJsonAsync(payload)
            .ReceiveJson<CreatePostResponse>();
        }
    }
}
