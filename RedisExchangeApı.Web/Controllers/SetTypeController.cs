using Microsoft.AspNetCore.Mvc;
using RedisExchangeApı.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApı.Web.Controllers;

public class SetTypeController : Controller
{
    private readonly RedisService redisService;
    private readonly IDatabase db;
    private const string setKey = "setKey";
    public SetTypeController(RedisService redisService)
    {
        this.redisService = redisService;
        db = redisService.GetDb(2);
        
    }
    public IActionResult Index()
    {
        HashSet<string> list = new HashSet<string>();
        foreach (var item in db.SetMembers(setKey))
        {
            list.Add(item);
        };
        return View(list);
    }
    [HttpPost]
    public IActionResult Add(string name)
    {
        db.KeyExpire(setKey,DateTime.Now.AddMinutes(5));
        db.SetAdd(setKey,name);
        return View();
    }
    public IActionResult Remove(string name)
    {
        db.SetRemove(setKey, name);
        return RedirectToAction("Index");
    }
}
