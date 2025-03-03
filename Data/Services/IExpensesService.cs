using FinanceApp.Models;

namespace FinanceApp.Data.Services
{
    public interface IExpensesService //anything related to database must be in a service for security purposes
    {
        Task<IEnumerable<Expense>> GetAll();
        Task<Expense> FindId(int? expenseId);
        Task Add(Expense expense);
        IQueryable GetChartData();
        Task<Expense> Update(Expense expense);
        Task<Expense> Delete(Expense expense);
    }
}
