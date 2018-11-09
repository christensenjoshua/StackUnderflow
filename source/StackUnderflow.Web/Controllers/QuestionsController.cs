using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            return View(question);
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

        //private bool QuestionExists(int id)
        //{
        //    return _context.Questions.Any(e => e.Id == id);
        //}
    }
}
