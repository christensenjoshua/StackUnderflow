using Microsoft.EntityFrameworkCore;
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
            _ctx.SaveChanges();
        }

        public Question UpdateQuestion(Question q)
        {
            try
            {
                _ctx.Questions.Update(q);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            _ctx.SaveChanges();
            return GetQuestionById(q.Id);
        }

        public Question DeleteQuestion(int id)
        {
            var q = GetQuestionById(id);
            _ctx.Remove(q);
            _ctx.SaveChanges();
            return q;
        }

        public bool QuestionExists(int id)
        {
            return _ctx.Questions.Any(q => q.Id == id);
        }
    }
}
