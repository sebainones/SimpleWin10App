using Caliburn.Micro;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace RateApp.Services
{
    internal class FakeRestClient : IRestClient
    {
        private readonly ILog _log;

        public string APIBasePath
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string APIId
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string LatestJsonQuery
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public FakeRestClient()
        {
            _log = LogManager.GetLog(typeof(FakeRestClient));
        }

        public async Task<T> GetAsync<T>()
        {
            T responseJson = default(T);
            try
            {
                var sf = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///Services/FakeJsonResponse.json"));
                string jsonText = await FileIO.ReadTextAsync(sf);

                responseJson = JsonConvert.DeserializeObject<T>(jsonText);
            }

            catch (JsonException exception)
            {
                _log.Error(exception);
                throw;
            }
            catch (Exception exception)
            {
                _log.Error(exception);
                throw;
            }

            return responseJson;
        }
    }
}
