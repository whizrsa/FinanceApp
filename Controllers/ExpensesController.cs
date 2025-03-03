using FinanceApp.Data;
using FinanceApp.Data.Services;
using FinanceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace FinanceApp.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly IExpensesService _expensesService;
        public ExpensesController(IExpensesService expensesService)
        {
            _expensesService = expensesService;
        }
        public async Task<IActionResult> Index()
        {
            var expenses = await _expensesService.GetAll();
            return View(expenses);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Expense expense)
        {
            if (ModelState.IsValid)
            {
                await _expensesService.Add(expense);

                return RedirectToAction("Index");
            }

            return View(expense);
        }

        public IActionResult GetChart()
        {
            var data = _expensesService.GetChartData();

            return Json(data);
        }

        public async Task<IActionResult> Manage()
        {
            var expenses = await _expensesService.GetAll();
            return View(expenses);
        }

        public async Task<ActionResult> Edit(int? expenseId)
        {
            if (expenseId == null)
            {
                return NotFound();
            }

            var expense = await _expensesService.FindId(expenseId);

            if (expense == null)
            {
                return NotFound();
            }

            return View(expense); // ensuring the view gets an expense with a valid Id
        }


        [HttpPost]
        public async Task<ActionResult> Edit(Expense expense) 
        {
            if (expense == null || expense.Id == 0) // Checking if Id is missing
            {
                return BadRequest("Invalid expense data");
            }

            var existingExpense = await _expensesService.FindId(expense.Id);
            if (existingExpense == null)
            {
                return NotFound("Expense not found.");
            }

            if (ModelState.IsValid)
            {
                await _expensesService.Update(expense);
                TempData["SuccessMsg"] = "Expense (" + expense.Description + ") updated successfully.";
                return RedirectToAction("Manage");
            }

            return View(expense);
        }

        public async Task<ActionResult> Delete(int? expenseId)
        {
            if (expenseId == null)
            {
                return NotFound();
            }

            var expense = await _expensesService.FindId(expenseId);

            if (expense == null)
            {
                return NotFound();
            }

            return View(expense); // This ensures the view gets an expense with a valid Id
        }

        [HttpPost]
        public async Task<ActionResult> Delete(Expense expense)
        {
            if (expense == null || expense.Id == 0) // Id exists or not
            {
                return BadRequest("Invalid expense data");
            }

            var existingExpense = await _expensesService.FindId(expense.Id);

            if (existingExpense == null)
            {
                return NotFound("Expense not found.");
            }

            if (ModelState.IsValid)
            {
                await _expensesService.Delete(expense);
                return RedirectToAction("Manage");
            }

            return View(expense);
        }

    }
}
