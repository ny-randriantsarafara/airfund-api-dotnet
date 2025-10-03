
using Microsoft.EntityFrameworkCore;

public class InvestmentRepository : DbContext, IInvestmentRepository
{
    private DbSet<Investment> Investments => Set<Investment>();

    public InvestmentRepository(DbContextOptions<InvestmentRepository> options) : base(options)
    {
    }

    public Task<Investment> AddInvestmentAsync(Investment investment)
    {
        if (investment == null)
        {
            throw new ArgumentNullException(nameof(investment));
        }

        Investments.Add(investment);
        SaveChanges();
        return Task.FromResult(investment);
    }

    public Task DeleteInvestmentAsync(int id)
    {
        var investment = Investments.Find(id);
        if (investment == null)
        {
            throw new KeyNotFoundException($"Investment with ID {id} not found.");
        }
        Investments.Remove(investment);
        SaveChanges();
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Investment>> GetAllInvestmentsAsync()
    {
        return Task.FromResult(Investments.AsEnumerable());
    }

    public Task<Investment> GetInvestmentByIdAsync(int id)
    {
        var investment = Investments.Find(id);
        if (investment == null)
        {
            throw new KeyNotFoundException($"Investment with ID {id} not found.");
        }
        return Task.FromResult(investment);
    }

}