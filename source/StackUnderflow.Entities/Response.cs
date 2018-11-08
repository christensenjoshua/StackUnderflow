using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace StackUnderflow.Entities
{
    public class ResponseComment
    {
        public int Id { get; set; }
        public int ResponseId { get; set; }
        public int CommentId { get; set; }

        [IgnoreDataMember]
        public Response Response { get; set; }
        [IgnoreDataMember]
        public Comment Comment { get; set; }
    }

    public class Response
    {
        public int Id { get; set; }
        [Required]
        public string Body { get; set; }
        public string UserId { get; set; }
        public int Popularity { get; set; }
        public bool IsSolution { get; set; }

        public virtual ICollection<ResponseComment> ResponseComments { get; set; }
    }
}
