using System.Threading.Tasks;

namespace FundaHomework
{
    public interface IRepository
    {
        public Task<FundaResponse> GetPropertiesAsync(int page);
    }
}