using Microsoft.AspNetCore.Mvc;
using Karmaplusplus.Models;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Configuration;

namespace Karmaplusplus.Controllers;

[Authorize]
public class CharitiesController : Controller
{
  private readonly KarmaplusplusContext _db;
  private readonly UserManager<ApplicationUser> _userManager;
  public CharitiesController(UserManager<ApplicationUser> userManager, KarmaplusplusContext db)
  {
    _userManager = userManager;
    _db = db;
  }
    public IActionResult Index()
    { 
        return View();
    }


  [HttpPost, ActionName("Search")]
  public async Task<IActionResult> Search(string name, string apiKey)
  {
  
  
    if(name == null)
    {
      return RedirectToAction("Index");
    }
    
    //add secret apikey
    var config = new ConfigurationBuilder()
          .AddUserSecrets<Program>()
          .Build();
    string apikey = config["apikey"];

    List<Charity> CharityList = new List<Charity> { };
    using (var httpClient = new HttpClient())
    {
      using (var response = await httpClient.GetAsync($"https://partners.every.org/v0.2/browse/{name}?apiKey={apikey}"))
      {
        string apiResponse = await response.Content.ReadAsStringAsync();
        JObject jsonResponse = JObject.Parse(apiResponse);
        JArray charityArray = (JArray)jsonResponse["nonprofits"];
        CharityList = charityArray.ToObject<List<Charity>>();
      }
    }
    ViewBag.SearchResults = name;
    return View(CharityList);
  }


}