using System.Threading.Tasks;

namespace RateApp.Services
{
    public interface IRestClient
    {
        string APIBasePath { get; }
        string APIId { get; }
        string LatestJsonQuery { get; }

        Task<T> Get<T>();
    }
}