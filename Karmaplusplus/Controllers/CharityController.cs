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

string apiKey = ConfigurationManager.appSettings["APIKey"];

  [HttpPost, ActionName("Search")]
  public async Task<IActionResult> Search(string name, string apiKey)
  {
  
  
    if(name == null)
    {
      return RedirectToAction("Index");
    }
    List<Charity> CharityList = new List<Charity> { };
    using (var httpClient = new HttpClient())
    {
      using (var response = await httpClient.GetAsync($"https://partners.every.org/v0.2/browse/{name}?apiKey=pk_live_0010085fa96129b630dc18c80f8728f8"))
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