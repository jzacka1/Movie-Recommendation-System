using SmartReturns.Models;

namespace SmartReturns.Services
{
    public interface IReturnService
    {
        Task<ReturnRequest?> GetReturnAsync(int id);
        Task<ReturnRequest> CreateReturnAsync(ReturnRequest request);
    }
}
