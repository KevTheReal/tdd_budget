using FluentAssertions;
using NSubstitute;
using tdd_budget.Services;

namespace tdd_budget_tests;

public class BudgetServiceTests
{
    private IBudgetRepo _budgetRepo;
    private BudgetService _budgetService;

    [SetUp]
    public void Setup()
    {
        _budgetRepo = Substitute.For<IBudgetRepo>();
        _budgetService = new BudgetService(_budgetRepo);
    }
    
    [Test]
    public void QueryWholeMonth()
    {
        GivenBudgets(new List<Budget>()
        {
            new()
            {
                YearMonth = "202312",
                Amount = 3100
            }
        });
        var result = _budgetService.Query(new DateTime(2023, 12, 1), new DateTime(2023, 12, 31));
        result.Should().Be(3100);
    }

    [Test]
    public void QueryPartialMonth()
    {
        GivenBudgets(new List<Budget>()
        {
            new()
            {
                YearMonth = "202312",
                Amount = 3100
            }
        });
        var result = _budgetService.Query(new DateTime(2023, 12, 1), new DateTime(2023, 12, 10));
        result.Should().Be(1000);
    } 
    
    [Test]
    public void QueryCrossWholeMonth()
    {
        GivenBudgets(new List<Budget>()
        {
            new()
            {
                YearMonth = "202311",
                Amount = 6000
            },
            new()
            {
                YearMonth = "202312",
                Amount = 3100
            }
        });
        var result = _budgetService.Query(new DateTime(2023, 11, 1), new DateTime(2023, 12, 31));
        result.Should().Be(9100);
    }
    
    [Test]
    public void QueryCrossMonth()
    {
        GivenBudgets(new List<Budget>()
        {
            new()
            {
                YearMonth = "202311",
                Amount = 6000
            },
            new()
            {
                YearMonth = "202312",
                Amount = 3100
            }
        });
        var result = _budgetService.Query(new DateTime(2023, 11, 30), new DateTime(2023, 12, 2));
        result.Should().Be(400);
    }
    
    [Test]
    public void QueryInvalidPeriod()
    {
        GivenBudgets(new List<Budget>()
        {
            new()
            {
                YearMonth = "202311",
                Amount = 6000
            },
            new()
            {
                YearMonth = "202312",
                Amount = 3100
            }
        });
        var result = _budgetService.Query(new DateTime(2023, 12, 2), new DateTime(2023, 11, 30));
        result.Should().Be(0);
    }
    
    [Test]
    public void QueryNoDataInPeriod()
    {
        GivenBudgets(new List<Budget>()
        {
            new()
            {
                YearMonth = "202311",
                Amount = 6000
            }
        });
        var result = _budgetService.Query(new DateTime(2023, 12, 1), new DateTime(2023, 12, 31));
        result.Should().Be(0);
    }

    private void GivenBudgets(List<Budget> budgets)
    {
        _budgetRepo.GetAll().Returns(budgets);
    }
}