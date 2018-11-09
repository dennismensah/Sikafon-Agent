using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sikafon
{
    public class Rest
    {
        private HttpClient _client;
        private const string ApiUrl = "http://dennismensaht-001-site1.btempurl.com/api/";
        //private const string ApiUrl = "localhost:5000/api/";

        public Rest()
        {
            _client = new HttpClient
            {
                MaxResponseContentBufferSize = 256000
            };
        }

        public async Task<Response> GetAsync(string path)
        {
            var response = await _client.GetAsync(String.Concat(ApiUrl, path));
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return new Response
                {
                    Content = content,
                    IsSuccessStatusCode = response.IsSuccessStatusCode,
                    StatusCode = (int)response.StatusCode
                };
            }
            else
            {
                return new Response
                {
                    Content = content,
                    IsSuccessStatusCode = response.IsSuccessStatusCode,
                    ReasonPhrase = response.ReasonPhrase,
                    StatusCode = (int)response.StatusCode
                };
            }
        }

        public async Task<Response> PostAsync(string path, Object inputdata)
        {
            var uri = new Uri(string.Format(String.Concat(ApiUrl, path), string.Empty));
            var json = JsonConvert.SerializeObject(inputdata);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(uri, content);
            var outputdata = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new Response
                {
                    Content = outputdata,
                    IsSuccessStatusCode = response.IsSuccessStatusCode,
                    StatusCode = (int)response.StatusCode
                };
            }
            else
            {
                return new Response
                {
                    Content = outputdata,
                    IsSuccessStatusCode = response.IsSuccessStatusCode,
                    ReasonPhrase = response.ReasonPhrase,
                    StatusCode = (int)response.StatusCode
                };
            }
        }
    }
}
