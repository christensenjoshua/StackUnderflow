using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace StackUnderflow.Entities
{
    public class ResponseComments
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

        public virtual ICollection<ResponseComments> ResponseComments { get; set; }
    }

    public class ResponseForView
    {
        public int Id { get; set; }
        [Required]
        public string Body { get; set; }
        public string UserId { get; set; }
        public int Popularity { get; set; }
        public bool IsSolution { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }

    public class NewResponse
    {
        public string Body { get; set; }
        public int QuestionId { get; set; }
    }

    public class VoteResponse
    {
        public int QuestionId { get; set; }
        public int ResponseId { get; set; }
        public int Value { get; set; }
    }
}
