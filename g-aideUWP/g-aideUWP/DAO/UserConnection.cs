using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;



namespace g_aideUWP.DAO
{
    class UserConnection
    {
        public async Task<string> GetToken()
        {
            string values = "Username=admin&Password=admin12&grant_type=password";
            //var content = new FormUrlEncodedContent(values);

            StringContent content = new StringContent(values);
            var getToken = new HttpClient();
            var token = await getToken.PostAsync(new Uri("http://g-aideappweb.azurewebsites.net/Token"), content);

            token.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            token.EnsureSuccessStatusCode();
            string responseBody = await token.Content.ReadAsStringAsync();

            string trueToken = jsonToToken(responseBody);

            return trueToken;


        }

        private String jsonToToken(String responseBody) //throws Exception
        {
            String token = responseBody.Remove(0, 17);
            token = token.Remove(token.Length - 2);
        return token;
    }

}
}
