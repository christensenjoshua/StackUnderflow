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
            return _ctx.Questions.OrderByDescending(x=>x.Popularity).ToList();
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

        public Question UpdateQuestion(Question q, string userId)
        {
            //need to check updating user vs current user
            Question origQ = GetQuestionById(q.Id);
            if(origQ.UserId == userId)
            {
                // everything is okay!
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
            throw new Exception("You are not the owner of this question!");
        }

        public Question DeleteQuestion(int id)
        {
            //need to check deleting user vs user on question.
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
                .Select(x => x.Response)
                .OrderByDescending(x => x.Popularity)
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
                .Select(x => x.Comment)
                .OrderByDescending(x => x.Popularity)
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

        public void VoteOnResponse(VoteResponse v)
        {
            Response r = _ctx.Responses.FirstOrDefault(x => x.Id == v.ResponseId);
            r.Popularity += v.Value;
            _ctx.Responses.Update(r);
            _ctx.SaveChanges();
        }

        public void VoteOnQuestion(VoteQuestion v)
        {
            Question q = _ctx.Questions.FirstOrDefault(x => x.Id == v.QuestionId);
            q.Popularity += v.Value;
            _ctx.Questions.Update(q);
            _ctx.SaveChanges();
        }

        public void MarkSolution(int rId)
        {
            Response r = _ctx.Responses.FirstOrDefault(x => x.Id == rId);
            r.IsSolution = true;
            _ctx.Responses.Update(r);
            _ctx.SaveChanges();
        }
    }
}
