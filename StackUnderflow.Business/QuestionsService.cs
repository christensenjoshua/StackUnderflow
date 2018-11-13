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

        public List<ResponseForView> GetRelatedResponses(int qId)
        {
            List <ResponseForView> newItems = new List<ResponseForView>();
            List<Response> tmpResponses = _ctx.QuestionResponses
                .Where(x => x.QuestionId == qId)
                .Select(g => g.Response)
                .ToList();
            foreach(Response item in tmpResponses)
            {
                newItems.Add(new ResponseForView
                {
                    Id = item.Id,
                    Body = item.Body,
                    UserId = item.UserId,
                    Popularity = item.Popularity,
                    IsSolution = item.IsSolution,
                    Comments = GetRelatedComments(item.Id)
                });
            }
            return newItems;
        }

        public List<Comment> GetRelatedComments(int rId)
        {
            return _ctx.ResponseComments
                .Where(x => x.ResponseId == rId)
                .Select(y => y.Comment)
                .ToList();
        }

        public void CreateResponse(Response r, int qId)
        {
            _ctx.Responses.Add(r);
            QuestionResponses xRef = new QuestionResponses
            {
                ResponseId = r.Id,
                QuestionId = qId
            };
            _ctx.QuestionResponses.Add(xRef);
            _ctx.SaveChanges();
        }

        public void CreateComment(Comment c, int rId)
        {
            _ctx.Comments.Add(c);
            ResponseComments xRef = new ResponseComments
            {
                CommentId = c.Id,
                ResponseId = rId
            };
            _ctx.ResponseComments.Add(xRef);
            _ctx.SaveChanges();
        }
    }
}
