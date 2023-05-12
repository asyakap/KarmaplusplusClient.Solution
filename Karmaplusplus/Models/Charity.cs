using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace Karmaplusplus.Models
{
[Keyless]
  public class Charity
  {
    public string name { get; set; }
    public string profileUrl { get; set; }
    public string description { get; set; }
    public string ein { get; set; }
    public string logoCloudinaryId { get; set; }
    public string logoUrl { get; set; }
    public string location { get; set; }



    public static List<Charity> GetCharities()
    {
      var apiCallTask = ApiHelper.GetAllCharities();
      var result = apiCallTask.Result;

      JArray jsonResponse = JsonConvert.DeserializeObject<JArray>(result);
      List<Charity> charityList = JsonConvert.DeserializeObject<List<Charity>>(jsonResponse.ToString());

      return charityList;
    }
  }
}