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
        var rawBudgets = budgets
            .Where(x => x.YearMonthDateTime >= ToFirstMonthDay(start) && x.YearMonthDateTime <= ToFirstMonthDay(end));
        
        return rawBudgets.Sum(x => GetBudgetInMonth(start, end, x));
    }

    private DateTime ToFirstMonthDay(DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, 1);
    }

    private static int GetBudgetInMonth(DateTime start, DateTime end, Budget x)
    {
        var mStartDay = start;
        var mEndDay = end;
        while (!(mStartDay.Month == x.YearMonthDateTime.Month && mStartDay.Year == x.YearMonthDateTime.Year))
        {
            mStartDay = mStartDay.AddDays(1);
        }
        while (!(mEndDay.Month == x.YearMonthDateTime.Month && mEndDay.Year == x.YearMonthDateTime.Year))
        {
            mEndDay = mEndDay.AddDays(-1);
        }
        return x.Amount/DateTime
            .DaysInMonth(x.YearMonthDateTime.Year, x.YearMonthDateTime.Month) * ((mEndDay - mStartDay).Days + 1);
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