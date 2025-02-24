using FinanceApp.Models;

namespace FinanceApp.Data.Services
{
    public interface IExpensesService //anything related to database must be in a service for security purposes
    {
        Task<IEnumerable<Expense>> GetAll();
        Task Add(Expense expense);
        IQueryable GetChartData();
    }
}
