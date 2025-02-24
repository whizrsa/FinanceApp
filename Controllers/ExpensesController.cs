﻿using FinanceApp.Data;
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
        public async Task<IActionResult> Create(Expense expense) //it takes the expense model
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
    }
}
