﻿using g_aideUWP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;



namespace g_aideUWP.DAO
{
    class UserConnection
    {
        public async void GetToken()
        {
            string values = "Username=admin&Password=admin12&grant_type=password";
            //var content = new FormUrlEncodedContent(values);

            StringContent content = new StringContent(values);
            var getToken = new HttpClient();
            var token = await getToken.PostAsync(new Uri("http://g-aideappweb.azurewebsites.net/Token"), content);

            token.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            token.EnsureSuccessStatusCode();
            string responseBody = await token.Content.ReadAsStringAsync();

        }

    }
}
