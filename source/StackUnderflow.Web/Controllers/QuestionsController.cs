using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public IActionResult Index()
        {
            return View(_service.GetAllQuestions());
        }

        // GET: Questions/Details/5
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public IActionResult Edit(int id, [Bind("Id,Title,Body,UserId")] Question question)
        {

            if (id != question.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _service.UpdateQuestion(question);
                return RedirectToAction(nameof(Index));
            }
            return View(question);
        }

        // GET: Questions/Delete/5
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
        public IActionResult DeleteConfirmed(int id)
        {
            var question = _service.DeleteQuestion(id);
            return RedirectToAction(nameof(Index));
        }
        // POST: New Response on Question
        [HttpPost, ActionName("AddResponse")]
        [ValidateAntiForgeryToken]
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
