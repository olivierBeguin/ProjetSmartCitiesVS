using g_aideUWP.Exceptions;
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
            try
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
                    ServiceDone = d["ServiceDone"].Value<bool>(),
                   // DatePublicationService = new TimeSpan(int.Parse(t["openingHour"].Value<string>().Substring(0, 2)), int.Parse(t["openingHour"].Value<string>().Substring(3, 2)), int.Parse(t["openingHour"].Value<string>().Substring(6, 2))),
                    UserNeedServiceEmail = d["UserNeedService"]["Email"].Value<string>(),
                    Category = new CategoryService()
                    {
                        Id = d["Category"]["Id"].Value<long>(),
                        Label = d["Category"]["Label"].Value<string>()
                    }
                });

                return services;
                
            }
            catch (Exception)
            {
                throw new DataNotAvailableException();
            }

        }

        public async void RemoveService(Service deleteService, string tokenAccess)
        {
            try
            {
                var client = new HttpClient();
                long id = deleteService.Id;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAccess);
                var result = await client.DeleteAsync(new Uri("http://g-aideappweb.azurewebsites.net/api/services/" + id));

                result.EnsureSuccessStatusCode();
            }
            catch(Exception)
            {
                throw new DataUpdateException();
            }

           
        }


        public async void EditService(Service serviceEdit, string tokenAccess)
        {
            try
            {
                long id = serviceEdit.Id;
                ServiceBindingModel putService = new ServiceBindingModel()
                {
                    Label = serviceEdit.NameService,
                    DescriptionService = serviceEdit.DescriptionService,
                    DatePublicationService = serviceEdit.DatePublicationService,
                    ServiceDone = serviceEdit.ServiceDone,
                    UserNeedService = serviceEdit.UserNeedServiceEmail,
                    Category = (int)serviceEdit.Category.Id
                };
                var json = JsonConvert.SerializeObject(putService);

                var buffer = Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAccess);
                var result = await client.PutAsync(new Uri("http://g-aideappweb.azurewebsites.net/api/services/" + id), byteContent);

                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                result.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                throw new DataUpdateException();
            }
            
        }

        public async Task<IEnumerable<CategoryService>> GetCategory(string tokenAccess)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAccess);
                var getCategory = await client.GetStringAsync("http://g-aideappweb.azurewebsites.net/api/CategoryServices");
                getCategory = @"{ ""data"" : " + getCategory + "}";

                var rawCategory = JObject.Parse(getCategory);

                var category = rawCategory["data"].Children().Select(d => new CategoryService()
                {
                    Id = d["Id"].Value<long>(),
                    Label = d["Label"].Value<string>()
                });

                return category;
            }
            catch(Exception)
            {
                throw new DataNotAvailableException();
            }

            }
        }

    }
