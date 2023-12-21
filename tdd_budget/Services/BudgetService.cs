using System.Globalization;

namespace tdd_budget.Services;

public class BudgetService
{
    private readonly IBudgetRepo _budgetRepo;

    public BudgetService(IBudgetRepo budgetRepo)
    {
        _budgetRepo = budgetRepo;
    }

    public decimal Query(DateTime start, DateTime end)
    {
        var budgets = _budgetRepo.GetAll();
        return GetTotalBudget(budgets, start, end);
    }

    private decimal GetTotalBudget(List<Budget> budgets, DateTime start, DateTime end)
    {
        var rawBudgets = budgets.Where(x => x.YearMonthDateTime >= start && x.YearMonthDateTime <= end);
        return rawBudgets.Sum(x => x.Amount);
    }
}

public interface IBudgetRepo
{
    List<Budget> GetAll();
}

public class Budget
{
    public string YearMonth { get; set; }
    public int Amount { get; set; }

    public DateTime YearMonthDateTime => DateTime.ParseExact(YearMonth, "yyyyMM", CultureInfo.InvariantCulture);
}