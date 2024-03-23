using Microsoft.AspNetCore.Mvc;
using RedisExchangeApı.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApı.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService redisService;
        private readonly IDatabase db;
        private const string listKey = "names";
        public ListTypeController(RedisService redisService)
        {
            this.redisService = redisService;
            this.db = redisService.GetDb(1);
        }
        public IActionResult Index()
        {

            List<string> list = new List<string>();
            if (db.KeyExists(listKey))
            {
                foreach (var item in db.ListRange(listKey).ToList())
                {
                    list.Add(item);
                };
            }
            return View(list);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            db.ListRightPush(listKey,name);
            return View();
        }
       
        public IActionResult Remove(string name)
        {
            db.ListRemove(listKey,name);
            return RedirectToAction("Index");
        }
    }
}
