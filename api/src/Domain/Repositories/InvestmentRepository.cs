public interface IInvestmentRepository
{
    Task<Investment> GetInvestmentByIdAsync(int id);
    Task<IEnumerable<Investment>> GetAllInvestmentsAsync();
    Task<Investment> AddInvestmentAsync(Investment investment);
    Task DeleteInvestmentAsync(int id);
}