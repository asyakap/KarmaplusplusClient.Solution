using Microsoft.AspNetCore.Mvc;
using Karmaplusplus.Models;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Karmaplusplus.Controllers;

[Authorize]
public class ServicesController : Controller
{
  private readonly KarmaplusplusContext _db;
  private readonly UserManager<ApplicationUser> _userManager;
  public ServicesController(UserManager<ApplicationUser> userManager, KarmaplusplusContext db)
  {
    _userManager = userManager;
    _db = db;
  }

  public async Task<IActionResult> Index( int page = 1, int pageSize = 6)
  {
    Service service = new Service();
    List<Service> serviceList = new List<Service> { };
    using (var httpClient = new HttpClient())
    {
      using (var response = await httpClient.GetAsync($"https://localhost:7225/api/Services?page={page}&pageSize={pageSize}"))
      {
        string apiResponse = await response.Content.ReadAsStringAsync();
        JObject jsonResponse = JObject.Parse(apiResponse);
        JArray serviceArray = (JArray)jsonResponse["data"];
        serviceList = serviceArray.ToObject<List<Service>>();
      }
    }

    List<Service> totalServices = new List<Service> { };
    using (var httpClient = new HttpClient())
    {
      using (var response = await httpClient.GetAsync($"https://localhost:7225/api/Services?page={1}&pageSize=1001"))
      {
        string apiResponse = await response.Content.ReadAsStringAsync();
        JObject jsonResponse = JObject.Parse(apiResponse);
        JArray serviceArray = (JArray)jsonResponse["data"];
        totalServices = serviceArray.ToObject<List<Service>>();
      }
    }
    ViewBag.Remainder = totalServices.Count() % 6;
    ViewBag.TotalPages = (totalServices.Count() / 6);
    ViewBag.CurrentPage = page;
    ViewBag.PageSize = pageSize;
    
    return View(serviceList);
  }

  public async Task<IActionResult> Details(int id)
  {
    List<Service> ServiceList = new List<Service> { };
    using (var httpClient = new HttpClient())
    {
      using (var response = await httpClient.GetAsync($"https://localhost:7225/api/Services?id={id}"))
      {
        string apiResponse = await response.Content.ReadAsStringAsync();
        JObject jsonResponse = JObject.Parse(apiResponse);
        JArray serviceArray = (JArray)jsonResponse["data"];
        ServiceList = serviceArray.ToObject<List<Service>>();
      }
    }
        
    
    string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    ViewBag.UserId = userId;
    Service service = ServiceList[0];
    return View(service);
  }

  public ActionResult Create()
  {
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> Create(Service service)
  {
    if(!ModelState.IsValid)
    {
      return View(service);
    }
    string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
    service.UserId = userId;
    Service.Post(service);
    return RedirectToAction("Index");
  }

  public async Task<IActionResult> Edit(int id)
  {
    List<Service> ServiceList = new List<Service> { };
    using (var httpClient = new HttpClient())
    {
      using (var response = await httpClient.GetAsync($"https://localhost:7225/api/Services?id={id}"))
      {
        string apiResponse = await response.Content.ReadAsStringAsync();
        JObject jsonResponse = JObject.Parse(apiResponse);
        JArray serviceArray = (JArray)jsonResponse["data"];
        ServiceList = serviceArray.ToObject<List<Service>>();
      }
    }
  
    Service service = ServiceList[0];
    return View(service);
    
  }

  [HttpPost]
  public ActionResult Edit(Service service)
  {
    Service.Put(service);
    return RedirectToAction("Details", new { id = service.ServiceId });
  }

  public ActionResult Delete(int id)
  {
    Service service = Service.GetDetails(id);
    return View(service);
    
  }


  [HttpPost, ActionName("Delete")]
  public ActionResult DeleteConfirmed(int id)
  {
    Service.Delete(id);
    return RedirectToAction("Index");
  }

  [HttpGet]
  public ActionResult Error(Error error)
  {
    return View(error);
  }

  [HttpPost, ActionName("Search")]
  public async Task<IActionResult> Search(string name)
  {
    if(name == null)
    {
      return RedirectToAction("Index");
    }
    List<Service> ServiceList = new List<Service> { };
    using (var httpClient = new HttpClient())
    {
      using (var response = await httpClient.GetAsync($"https://localhost:7225/api/Services?pageSize=1001"))
      {
        string apiResponse = await response.Content.ReadAsStringAsync();
        JObject jsonResponse = JObject.Parse(apiResponse);
        JArray serviceArray = (JArray)jsonResponse["data"];
        ServiceList = serviceArray.ToObject<List<Service>>();
      }
    }
    List<Service> result = new List<Service> { };
    foreach(Service service in ServiceList)
    {
      if (service.Description.ToLower().Contains(name.ToLower()))
      {
        result.Add(service);
      }
    }
    ViewBag.SearchResults = name;
    return View(result);
  }
}
