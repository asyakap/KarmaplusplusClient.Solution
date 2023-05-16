using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace Karmaplusplus.Models
{
  public class Volunteering
  {
    public int VolunteeringId { get; set; }
    [Required(ErrorMessage = "Please enter the type of volunteering you require")]
    public string VolunteeringName { get; set; }
    public string Description { get; set; }
    public string Email { get; set; }
    public string Location { get; set; }
    public string ZipCode { get; set; }
    public string Date { get; set; }
    public string Time { get; set; }
    public string UserId { get; set; }


    public static List<Volunteering> GetVolunteerings()
    {
      var apiCallTask = ApiHelper.GetAllVolunteerings();
      var result = apiCallTask.Result;

      JArray jsonResponse = JsonConvert.DeserializeObject<JArray>(result);
      List<Volunteering> volunteeringList = JsonConvert.DeserializeObject<List<Volunteering>>(jsonResponse.ToString());

      return volunteeringList;
    }
    public static Volunteering GetVolunteeringDetails(int id)
    {
      var apiCallTask = ApiHelper.GetVolunteering(id);
      var result = apiCallTask.Result;

      JObject jsonResponse = JObject.Parse(result);
      Volunteering volunteering = JsonConvert.DeserializeObject<Volunteering>(jsonResponse.ToString());

      return volunteering;
    }

    public static void PostVolunteering(Volunteering volunteering)
    {
      string jsonVoluteering = JsonConvert.SerializeObject(volunteering);
      ApiHelper.PostVolunteering(jsonVoluteering);
    }

    public static void PutVolunteering(Volunteering volunteering)
    {
      string jsonVoluteering = JsonConvert.SerializeObject(volunteering);
      ApiHelper.PutVolunteering(volunteering.VolunteeringId, jsonVoluteering);
    }

    public static void DeleteVolunteering(int id)
    {
      ApiHelper.DeleteVolunteering(id);
    }
  }
}