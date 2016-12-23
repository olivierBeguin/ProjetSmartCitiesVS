using g_aideUWP.Exceptions;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Threading.Tasks;



namespace g_aideUWP.DAO
{
    class UserConnection
    {
        public async Task<string> GetToken(string userName, string password)
        {
            try
            {
                string values = ("Username=" + userName + "&Password=" + password + "&grant_type=password");

                StringContent content = new StringContent(values);
                var getToken = new HttpClient();
                var token = await getToken.PostAsync(new Uri("http://g-aideappweb.azurewebsites.net/Token"), content);

                token.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                string responseBody = await token.Content.ReadAsStringAsync();
                var data = JObject.Parse(responseBody);
                string trueToken = data["access_token"].Value<string>();
                return trueToken;
            }
            catch (HttpRequestException)
            {
                throw new ConnectionException(false);
            }
            catch (Exception)
            {
                throw new ConnectionException(true);
            }
        }
    }
}
