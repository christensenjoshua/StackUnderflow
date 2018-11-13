using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StackUnderflow.Business;
using StackUnderflow.Data;
using StackUnderflow.Entities;

namespace StackUnderflow.Web.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly QuestionsService _service;
        private readonly UserManager<IdentityUser> _um;

        public QuestionsController(QuestionsService service, UserManager<IdentityUser> um)
        {
            _service = service;
            _um = um;
        }

        // GET: Questions
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View(_service.GetAllQuestions());
        }

        // GET: Questions/Details/5
        [Authorize]
        public IActionResult Details(int id)
        {
            var question = _service.GetQuestionById(id);
            if (question == null)
            {
                return NotFound();
            }
            var qResponses = _service.GetRelatedResponses(id);
            var viewQuestion = new QuestionForView
            {
                Id = question.Id,
                Title = question.Title,
                Body = question.Body,
                UserId = question.UserId,
                Popularity = question.Popularity,
                Responses = qResponses
            };
            return View(viewQuestion);
        }

        // GET: Questions/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create([Bind("Id,Title,Body,UserId")] Question question)
        {
            var user = _um.GetUserAsync(HttpContext.User).Result;
            question.UserId = user.Id;
            if (ModelState.IsValid)
            {
                _service.CreateQuestion(question);
                return RedirectToAction(nameof(Index));
            }
            return View(question);
        }

        // GET: Questions/Edit/5
        [Authorize]
        public IActionResult Edit(int id)
        {
            var question = _service.GetQuestionById(id);
            if (question == null)
            {
                return NotFound();
            }
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit([Bind("Id,Title,Body,UserId")] Question question)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _service.UpdateQuestion(question, _um.GetUserAsync(HttpContext.User).Result.Id);
                    return RedirectToAction(nameof(Index));
                }catch(Exception ex)
                {
                    return View("CustomError", new CustomErrorModel { Text = ex.Message });
                }
            }
            return View(question);
        }

        // GET: Questions/Delete/5
        [Authorize]
        public IActionResult Delete(int id)
        {
            var question = _service.GetQuestionById(id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult DeleteConfirmed(int id)
        {
            try {
                _service.DeleteQuestion(id, _um.GetUserAsync(HttpContext.User).Result.Id);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View("CustomError", new CustomErrorModel { Text = ex.Message});
            }
        }
        // POST: New Response on Question
        [HttpPost, ActionName("AddResponse")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult AddResponse([FromForm] NewResponse rData)
        {
            var user = _um.GetUserAsync(HttpContext.User).Result;
            Response newResponse = new Response
            {
                Body = rData.Body,
                UserId = user.Id,
                Popularity = 0,
                IsSolution = false
            };
            _service.CreateResponse(newResponse, rData.QuestionId);
            var origQ = _service.GetQuestionById(rData.QuestionId);
            var qResponses = _service.GetRelatedResponses(origQ.Id);
            var viewQuestion = new QuestionForView
            {
                Id = origQ.Id,
                Title = origQ.Title,
                Body = origQ.Body,
                UserId = origQ.UserId,
                Popularity = origQ.Popularity,
                Responses = qResponses
            };
            return View("Details", viewQuestion);
        }

        // POST: New Response on Question
        [HttpPost, ActionName("AddComment")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult AddComment([FromForm] NewComment cData)
        {
            var user = _um.GetUserAsync(HttpContext.User).Result;
            Comment newResponse = new Comment
            {
                Body = cData.Body,
                UserId = user.Id,
                Popularity = 0,
            };
            _service.CreateComment(newResponse, cData.ResponseId);
            var origQ = _service.GetQuestionById(cData.QuestionId);
            var qResponses = _service.GetRelatedResponses(origQ.Id);
            var viewQuestion = new QuestionForView
            {
                Id = origQ.Id,
                Title = origQ.Title,
                Body = origQ.Body,
                UserId = origQ.UserId,
                Popularity = origQ.Popularity,
                Responses = qResponses
            };
            return View("Details", viewQuestion);
        }

        [HttpPost, ActionName("VoteResponse")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult VoteResponse([FromForm] VoteResponse r)
        {
            _service.VoteOnResponse(r);
            var origQ = _service.GetQuestionById(r.QuestionId);
            var qResponses = _service.GetRelatedResponses(origQ.Id);
            var viewQuestion = new QuestionForView
            {
                Id = origQ.Id,
                Title = origQ.Title,
                Body = origQ.Body,
                UserId = origQ.UserId,
                Popularity = origQ.Popularity,
                Responses = qResponses
            };
            return View("Details", viewQuestion);
        }

        [HttpPost, ActionName("VoteQuestion")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult VoteQuestion([FromForm] VoteQuestion r)
        {
            _service.VoteOnQuestion(r);
            var origQ = _service.GetQuestionById(r.QuestionId);
            var qResponses = _service.GetRelatedResponses(origQ.Id);
            var viewQuestion = new QuestionForView
            {
                Id = origQ.Id,
                Title = origQ.Title,
                Body = origQ.Body,
                UserId = origQ.UserId,
                Popularity = origQ.Popularity,
                Responses = qResponses
            };
            return View("Details", viewQuestion);
        }

        [HttpPost, ActionName("MarkSolution")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult MarkSolution([FromForm] VoteResponse r)
        {
            _service.MarkSolution(r.ResponseId);
            var origQ = _service.GetQuestionById(r.QuestionId);
            var qResponses = _service.GetRelatedResponses(origQ.Id);
            var viewQuestion = new QuestionForView
            {
                Id = origQ.Id,
                Title = origQ.Title,
                Body = origQ.Body,
                UserId = origQ.UserId,
                Popularity = origQ.Popularity,
                Responses = qResponses
            };
            return View("Details", viewQuestion);
        }

        //private bool QuestionExists(int id)
        //{
        //    return _context.Questions.Any(e => e.Id == id);
        //}
    }
}
