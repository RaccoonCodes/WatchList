using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WatchList.Models;
using WatchList.Controllers;

namespace WatchList.Controllers;

public class HomeController : Controller
{


    public IActionResult Index() => View();

    
    
}
