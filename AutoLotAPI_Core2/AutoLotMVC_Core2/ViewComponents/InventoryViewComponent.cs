using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AutoLotMVC_Core2.ViewComponents
{
    public class InventoryViewComponent: ViewComponent
    {
        private readonly string _baseUrl;

        public InventoryViewComponent(IConfiguration configure)
        {
            _baseUrl = configure.GetSection("ServiceAddress").Value;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(_baseUrl);
            if(response.IsSuccessStatusCode)
            {
                var items = JsonConvert.DeserializeObject<List<InventoryViewComponent>>(await response.Content.ReadAsStringAsync());
                return View("InventoryPartialView", items);
            }

            return new ContentViewComponentResult("Unable to return records.");
        }
    }
}
