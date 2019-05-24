using MediatR;

namespace Bank.Application.Bank.Queries.GetBankStatistics
{
    public class GetBankStatisticsQuery : IRequest<BankStatisticsViewModel>
    {
    }
}
