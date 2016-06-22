using System.Threading.Tasks;

namespace App1.Services
{
    public interface IRestClient
    {
        string APIBasePath { get; }
        string APIId { get; }
        string LatestJsonQuery { get; }

        Task<T> Get<T>();

        Task<T> Post<T>(string url, object requestContent);
    }
}