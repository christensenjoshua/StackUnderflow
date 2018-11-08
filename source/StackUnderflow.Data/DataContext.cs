using Microsoft.EntityFrameworkCore;
using StackUnderflow.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<QuestionResponses> QuestionResponses { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ResponseComments> ResponseComments { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}
