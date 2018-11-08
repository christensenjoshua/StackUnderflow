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
}
