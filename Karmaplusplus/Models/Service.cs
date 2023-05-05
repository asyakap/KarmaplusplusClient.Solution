using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace Karmaplusplus.Models
{
  public class Service
  {
    public int ServiceId { get; set; }
    [Required(ErrorMessage = "Please enter the type of service you want to offer")]
    public string ServiceName { get; set; }
    public string Description { get; set; }
    public string Email { get; set; }
    public int ZipCode { get; set; }
    public string UserId { get; set; }


    public static List<Service> GetServices()
    {
      var apiCallTask = ApiHelper.GetAll();
      var result = apiCallTask.Result;

      JArray jsonResponse = JsonConvert.DeserializeObject<JArray>(result);
      List<Service> serviceList = JsonConvert.DeserializeObject<List<Service>>(jsonResponse.ToString());

      return serviceList;
    }
    public static Service GetDetails(int id)
    {
      var apiCallTask = ApiHelper.Get(id);
      var result = apiCallTask.Result;

      JObject jsonResponse = JObject.Parse(result);
      Service service = JsonConvert.DeserializeObject<Service>(jsonResponse.ToString());

      return service;
    }

    public static void Post(Service service)
    {
      string jsonService = JsonConvert.SerializeObject(service);
      ApiHelper.Post(jsonService);
    }

    public static void Put(Service service)
    {
      string jsonService = JsonConvert.SerializeObject(service);
      ApiHelper.Put(backyard.ServiceId, jsonService);
    }

    public static void Delete(int id)
    {
      ApiHelper.Delete(id);
    }
  }
}