using Microsoft.AspNetCore.Mvc;
using RedisExchangeApı.Web.Services;
using StackExchange.Redis;
using System.Globalization;

namespace RedisExchangeApı.Web.Controllers
{
    public class HashTypeController : Controller
    {
        private readonly RedisService redisService;
        private readonly IDatabase db;
        private const string hashKey = "hashKey";
        public HashTypeController(RedisService redisService)
        {
            this.redisService = redisService;
            this.db = redisService.GetDb(4);
        }
        public IActionResult Index()
        {
            Dictionary<string,string> list = new Dictionary<string, string>();
            foreach (var item in db.HashGetAll(hashKey).ToList())
            {
                list.Add(item.Name,item.Value);
            };
            return View(list);
        }
        [HttpPost]
        public IActionResult Add(string key,string val)
        {
            db.KeyExpire(hashKey, DateTime.Now.AddMinutes(5));
            db.HashSet(hashKey, key,val);
            return View();
        }
        public IActionResult Remove(string name)
        {
            db.HashDelete(hashKey, name);
            return RedirectToAction("Index");
        }
    }
}
