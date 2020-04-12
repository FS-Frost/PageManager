using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PageManager.Core {
    public class Post {
        [JsonProperty("id")]
        [DisplayName("ID")]
        public string Id { get; set; }

        [JsonProperty("created_time")]
        [DisplayName("Fecha de creación")]
        public DateTime CreatedTime { get; set; }

        [JsonProperty("message")]
        [DisplayName("Mensaje")]
        public string Message { get; set; }

        [JsonProperty("permalink_url")]
        [DisplayName("URL")]
        public string PermalinkUrl { get; set; }

        [JsonProperty("full_picture")]
        [DisplayName("Imagen adjunta")]
        public string FullPicture { get; set; }

        [JsonProperty("attachments")]
        private Attachments Attachments { get; set; }

        [DisplayName("Enlace adjunto")]
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
    }

    public class Attachments {
        [JsonProperty("data")]
        public List<Attachment> Data { get; set; }
    }

    public class Attachment {
        [JsonProperty("url")]
        public string Url { get; set; }
    }

}
