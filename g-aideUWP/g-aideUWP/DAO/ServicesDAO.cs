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
        public async Task<IEnumerable<Service>> GetServices(string tokenAccess)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAccess);
            var json = await client.GetStringAsync("http://g-aideappweb.azurewebsites.net/api/services");

            var rawService = JObject.Parse(json); // pourquoi ca fonctionne PAS ....

            var service = rawService["list"].Children().Select(d => new Service()
            {
                Id = d["Id"].Value<long>(),
                NameService = d["Label"].Value<string>(),
                DescriptionService = d["DescriptionService"].Value<string>(),
                DatePublicationService = d["DatePublicationService"].Value<DateTime>(),
                ServiceDone = d["ServiceDone"].Value<Boolean>(),
                UserApplication = d["UserNeedService"].Value<UserApp>(),
                Category = d["Category"].Value<CategoryService>(),
                DoServices = d["doService"].Value<DoService>(),
                RowVersion = d["RowVersion"].Value<Byte[]>()
            });

            return null;
        }

        public async void RemoveService(Service deleteService)
        {

        }

        public async void EditService(Service editService)
        {

        }
    }
}
