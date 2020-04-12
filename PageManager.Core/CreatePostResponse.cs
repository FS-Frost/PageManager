using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PageManager.Core {
    public class CreatePostResponse : ApiResponse {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("created_time")]
        public DateTime CreatedTime { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("permalink_url")]
        public string PermalinkUrl { get; set; }

        [JsonProperty("full_picture")]
        public string FullPicture { get; set; }

        [JsonProperty("attachments")]
        private Attachments Attachments { get; set; }

        [JsonProperty("attached_link")]
        public string AttachedLink { get => GetAttachedLink(); set => SetAttachedLink(value); }

        private string GetAttachedLink() {
            if (Attachments != null && Attachments.Data.Count > 0) {
                return Attachments.Data[0].Url;
            }

            return "";
        }

        private void SetAttachedLink(string link) {
            Attachments = new Attachments {
                Data = new List<Attachment> {
                    new Attachment {
                        Url = link
                    }
                }
            };
        }

        public Post ToPost() {
            return new Post {
                Id = Id,
                CreatedTime = CreatedTime,
                Message = Message,
                PermalinkUrl = PermalinkUrl,
                FullPicture = FullPicture,
                AttachedLink = AttachedLink
            };
        }
    }
}
