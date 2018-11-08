using StackUnderflow.Data;
using StackUnderflow.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StackUnderflow.Business
{
    public class QuestionsService
    {
        private readonly DataContext _ctx;

        public QuestionsService(DataContext ctx)
        {
            _ctx = ctx;
        }

        public List<Question> GetAllQuestions()
        {
            return _ctx.Questions.ToList();
        }

        public Question GetQuestionById(int id)
        {
            return _ctx.Questions.FirstOrDefault(x => x.Id == id);
        }

        public void CreateQuestion(Question q)
        {
            _ctx.Questions.Add(q);
        }
    }
}
