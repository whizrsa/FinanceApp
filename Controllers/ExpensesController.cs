using FinanceApp.Data;
using FinanceApp.Data.Services;
using FinanceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Text;

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
                TempData["SuccessMessage"] = "Expense added successfully!";
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

            return View(expense);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Expense expense) 
        {
            if (expense == null || expense.Id == 0)
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
                TempData["SuccessMessage"] = "Expense updated successfully!";
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

            return View(expense);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(Expense expense)
        {
            if (expense == null || expense.Id == 0)
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
                TempData["SuccessMessage"] = "Expense deleted successfully!";
                return RedirectToAction("Manage");
            }

            return View(expense);
        }

        // Export expenses to CSV
        public async Task<IActionResult> ExportCsv()
        {
            var expenses = await _expensesService.GetAll();
            var csv = new StringBuilder();
            
            csv.AppendLine("Description,Amount,Category,Date");
            
            foreach (var expense in expenses)
            {
                csv.AppendLine($"\"{expense.Description}\",{expense.Amount},\"{expense.Category}\",{expense.Date:yyyy-MM-dd}");
            }

            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", $"expenses_{DateTime.Now:yyyyMMdd}.csv");
        }
    }
}
