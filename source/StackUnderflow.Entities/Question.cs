using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace StackUnderflow.Entities
{
    public class QuestionResponses
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int ResponseId { get; set; }

        [IgnoreDataMember]
        public Question Question { get; set; }
        [IgnoreDataMember]
        public Response Response { get; set; }
    }

    public class Question
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Body { get; set; }
        public string UserId { get; set; }
        public int Popularity { get; set; }

        public virtual ICollection<QuestionResponses> QuestionResponses { get; set; }
    }

    public class QuestionForView
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Body { get; set; }
        public string UserId { get; set; }
        public int Popularity { get; set; }

        public virtual ICollection<ResponseForView> Responses { get; set; }
    }

    public class VoteQuestion
    {
        public int QuestionId { get; set; }
        public int Value { get; set; }
    }
}
