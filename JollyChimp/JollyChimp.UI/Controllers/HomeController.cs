using System.Diagnostics;
using JollyChimp.Core.Common.Constants.Media;
using JollyChimp.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JollyChimp.UI.Controllers;

public class HomeController : Controller
{

    private readonly IConfiguration _configuration;

    public HomeController(
        IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult EndPoints()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult ServerSettings()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult WebHooks()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult HealthChecks()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        ViewData["Content-Gifs-JollyChimpAlerted"] = JollyChimpContentConstants.Gifs.JollyChimpAlerted;
        ViewData["Content-Gifs-JollyChimpInform"] = JollyChimpContentConstants.Gifs.JollyChimpInform;
        ViewData["Content-Gifs-JollyChimpStare"] = JollyChimpContentConstants.Gifs.JollyChimpStare;
        ViewData["Content-Images-JollyChimpAlerted"] = JollyChimpContentConstants.Images.JollyChimpAlerted;
        ViewData["Content-Images-JollyChimpStare"] = JollyChimpContentConstants.Images.JollyChimpStare;

        ViewData["URI-DOMAIN"] = _configuration["JollyChimp:Domain"];

        base.OnActionExecuting(context);
    }
}