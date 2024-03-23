using Microsoft.AspNetCore.Mvc;
using RedisExchangeApı.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApı.Web.Controllers
{
    public class SortedSetController : Controller
    {
        private readonly RedisService redisService;
        private readonly IDatabase db;
        private const string sortedKey = "sortedKey";
        public SortedSetController(RedisService redisService)
        {
            this.redisService = redisService;
            db = redisService.GetDb(3);
        }
        public IActionResult Index()
        {

            List<string> list = new List<string>();
            if (db.KeyExists(sortedKey))
            {
                foreach (var item in db.SortedSetScan(sortedKey).ToList())
                {
                    list.Add(item.ToString());
                };
            }
            return View(list);
        }
        [HttpPost]
        public IActionResult Add(string name,int score)
        {
            db.SortedSetAdd(sortedKey,name, score);
            return View();
        }

        public IActionResult Remove(string name)
        {
            db.SortedSetRemove(sortedKey, name);
            return RedirectToAction("Index");
        }
    }
}
