using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Crypto_Marketplace.Models;
using System.Text.Json;

namespace Crypto_Marketplace.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;


    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        List<Crypto> CryptoList = new List<Crypto>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://api2.binance.com/api/v3/ticker/24hr"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    CryptoList = JsonSerializer.Deserialize<List<Crypto>>(apiResponse); 
                }
            }
            ViewBag.CryptoList = CryptoList;
            return View();
    }
    [HttpPost("searchBySymbol")]
    public IActionResult searchBySymbol(Crypto searchingCrypto)
    {
        return RedirectToAction("Search", searchingCrypto.symbol );
    }
    [HttpGet("{symbol}/Search")]
    public async Task<IActionResult> Search(string symbol)
    {
        List<Crypto> CryptoList = new List<Crypto>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://api2.binance.com/api/v3/ticker/24hr"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    CryptoList = JsonSerializer.Deserialize<List<Crypto>>(apiResponse); 
                }
            }
            ViewBag.CryptoList = CryptoList.Where(c=> c.symbol == symbol).ToList();
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}