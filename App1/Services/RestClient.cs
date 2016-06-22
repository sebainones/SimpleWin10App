using App1.Model;
using Caliburn.Micro;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.Storage;

namespace App1.Services
{
    public class RestClient : IRestClient
    {
        private readonly JsonSerializerSettings jsonSerializerSettings;

        private readonly ILog _log;

        private readonly static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public string LatestJsonQuery
        {
            get { return localSettings.Values["LatestJsonQuery"].ToString(); }
        }

        public string APIId
        {
            get { return localSettings.Values["APIId"].ToString(); }
        }

        public string APIBasePath
        {
            get { return localSettings.Values["APIBasePath"].ToString(); }
        }

        static RestClient()
        {
            localSettings.Values["APIBasePath"] = @"https://openexchangerates.org/api/";

            localSettings.Values["LatestJsonQuery"] = @"latest.json?app_id=";

            localSettings.Values["APIId"] = @"2f533c04fdbe4641a57bdad53800958e";
        }

        public RestClient()
        {
            jsonSerializerSettings = new JsonSerializerSettings { }; //ContractResolver = new CamelCasePropertyNamesContractResolver()

            //logger for your own code.
            _log = LogManager.GetLog(typeof(App));
        }

        private Uri CreateUri()
        {
            //Uri apiUri = new Uri(APIBasePath);

            UriBuilder baseUri = new UriBuilder("https://openexchangerates.org/api/latest.json");
            string queryToAppend = "app_id=2f533c04fdbe4641a57bdad53800958e";
            baseUri.Query = baseUri.Query + queryToAppend;

            return baseUri.Uri;
        }

        private void SetHeaders(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", LatestJsonQuery, APIId))));
        }

        public async Task<T> Get<T>()
        {
            T jsonObject = default(T);
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    SetHeaders(client);

                    //TODO: Verify this!! This workaround could be potential security threat as you are turning off the SSL certificate validation.
                    //https://social.msdn.microsoft.com/Forums/windowsapps/en-US/f5821194-4c40-48e7-976c-3dec8864ac59/servicepointmanagerservercertificatevalidationcallback?forum=winappswithcsharp
                    //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });

                    Uri uri = CreateUri();

                    using (HttpResponseMessage response = client.GetAsync(uri).Result)
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();


                        T responseJson = JsonConvert.DeserializeObject<T>(responseBody);//, jsonSerializerSettings

                        //jsonObject = Converter.ConvertValue<T>(responseJson.Schedule);
                    }
                }
            }

            catch (JsonException exception)
            {
                _log.Error(exception);
                throw new Exception(exception.Message);
            }
            catch (Exception exception)
            {
                _log.Error(exception);
            }

            return jsonObject;
        }
        
        public async Task<T> Post<T>(string url, object requestContent)
        {

            //var jsonRequestContent = JsonConvert.SerializeObject(requestContent, JsonUtilities.GetJsonSerializerSettings());

            T jsonObject = default(T);
            HttpResponseMessage response = null;
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //response = await client.PostAsync(new Uri(url), new StringContent(jsonRequestContent, Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                        _log.Warn("Resource not found in server");
                    response.EnsureSuccessStatusCode();
                }
                var responseContentString = await response.Content.ReadAsStringAsync();
                //var responseJson = JsonConvert.DeserializeObject<JsonServiceResult>(responseContentString);
                //jsonObject = JsonConvert.DeserializeObject<T>(responseJson.content.ToString(), JsonUtilities.GetJsonSerializerSettings());
            }
            catch (JsonException exception)
            {
                _log.Error(exception);
            }
            catch (Exception exception)
            {
                _log.Error(exception);
            }

            return jsonObject;
        }
    }
}

