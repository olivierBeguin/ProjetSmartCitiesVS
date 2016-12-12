using g_aideUWP.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace g_aideUWP.DAO
{
    class ServicesDAO
    {
        public async Task<IEnumerable<Service>> GetServices(string tokenAccess)//fonctionne mais pas encore afficher dans l app
        {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAccess);
                var service = await client.GetStringAsync("http://g-aideappweb.azurewebsites.net/api/services");
                service = @"{ ""data"" : " + service + "}";

                var rawService = JObject.Parse(service);

                var services = rawService["data"].Children().Select(d => new Service()
                {
                    Id = d["Id"].Value<long>(),
                    NameService = d["Label"].Value<string>(),
                    DescriptionService = d["DescriptionService"].Value<string>(),
                    DatePublicationService = d["DatePublicationService"].Value<DateTime>(),
                    Category = new CategoryService()
                    {
                        Id = d["Category"]["Id"].Value<long>(),
                        Label = d["Category"]["Label"].Value<string>()
                    }
                });

                return services;
            
        }

        public async void RemoveService(Service deleteService, string tokenAccess)// fonctionne mais pas encore fonctionnel lors de l appuie sur le bouton
        {
            var client = new HttpClient();
            long id = deleteService.Id;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAccess);
            var result = await client.DeleteAsync(new Uri("http://g-aideappweb.azurewebsites.net/api/services/"+id));

            result.EnsureSuccessStatusCode();
        }


        public async void EditService(Service serviceEdit, string tokenAccess)// fini , pas tester mais normalement OK il faut le mettre en place lorsqu on appuie sur le bouton
        {
            long id = serviceEdit.Id;
            var json = JsonConvert.SerializeObject(serviceEdit);

            var buffer = Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAccess);
            var result = await client.PutAsync(new Uri("http://g-aideappweb.azurewebsites.net/api/services/"+1), byteContent);

            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            result.EnsureSuccessStatusCode();
        }
    }
}
