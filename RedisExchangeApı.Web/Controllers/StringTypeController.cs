using Microsoft.AspNetCore.Mvc;
using RedisExchangeApı.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApı.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService redisService;
        private readonly IDatabase db;

        public StringTypeController(RedisService redisService)
        {
            this.redisService = redisService;
             db = redisService.GetDb(0);
        }
        public IActionResult Index()
        {
            
            db.StringSet("name","Aydemir");
            return View();
        }
        public IActionResult Show()
        {

       var value=     db.StringGet("name");
            if (value.HasValue)
            {
                ViewBag.value = value.ToString();
            }
            return View();
        }
    }
}
