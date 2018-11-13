using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public string UserId { get; set; }
        public int Popularity { get; set; }
    }

    public class NewComment
    {
        public string Body { get; set; }
        public int ResponseId { get; set; }
        public int QuestionId { get; set; }
    }
}
