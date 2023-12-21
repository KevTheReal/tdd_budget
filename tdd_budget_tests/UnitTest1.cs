using FluentAssertions;
using NSubstitute;
using tdd_budget.Services;

namespace tdd_budget_tests;

public class BudgetServiceTests
{
    private IBudgetRepo _budgetRepo;

    [Test]
    public void QueryWholeMonth()
    {
        _budgetRepo = Substitute.For<IBudgetRepo>();
        _budgetRepo.GetAll().Returns(new List<Budget>()
        {
            new Budget
            {
                YearMonth = "202312",
                Amount = 3100
            }
        });
        var budgetService = new BudgetService(_budgetRepo);
        var result = budgetService.Query(new DateTime(2023, 12, 1), new DateTime(2023, 12, 31));
        result.Should().Be(3100);
    }

    [Test]
    public void QueryPartialMonth()
    {
        _budgetRepo = Substitute.For<IBudgetRepo>();
        _budgetRepo.GetAll().Returns(new List<Budget>()
        {
            new Budget
            {
                YearMonth = "202312",
                Amount = 3100
            }
        });
        var budgetService = new BudgetService(_budgetRepo);
        var result = budgetService.Query(new DateTime(2023, 12, 1), new DateTime(2023, 12, 10));
        result.Should().Be(1000);
    }
}