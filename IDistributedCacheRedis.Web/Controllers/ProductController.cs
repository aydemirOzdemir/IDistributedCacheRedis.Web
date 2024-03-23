using IDistributedCacheRedis.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace IDistributedCacheRedis.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IDistributedCache distributedCache;

        public ProductController(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }
        public async Task< IActionResult> Index()
        {

            DistributedCacheEntryOptions opt = new()
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(15)
            };
            Product product = new() {Id=1,Name="Kalem",Price=100 };
            string jsonProduct = JsonConvert.SerializeObject(product);
             await distributedCache.SetStringAsync("product1",jsonProduct,opt);
            return View();
        }
        public IActionResult Show()
        {
            string jsonProduct = distributedCache.GetString("product1");
            Product product = JsonConvert.DeserializeObject<Product>(jsonProduct);
           ViewBag.name= distributedCache.GetString("name");
            return View();
        }


        public IActionResult ImageCache()
        {
            DistributedCacheEntryOptions opt = new() { AbsoluteExpiration=DateTime.Now.AddMinutes(15) };
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/50.jpg");

            byte[] imageByte = System.IO.File.ReadAllBytes(path);

            distributedCache.Set("resim",imageByte,opt);

            return View();
            
        }
        public IActionResult ShowImage()
        {


            byte[] bytePath = distributedCache.Get("resim");
          

           


            return File(bytePath,"imagePath/jpg");

        }
    }
}
