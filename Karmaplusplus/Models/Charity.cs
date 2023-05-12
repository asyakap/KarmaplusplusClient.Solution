using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace Karmaplusplus.Models
{
[Keyless]
  public class Charity
  {
    public string name { get; }
    public string profileUrl { get; }
    public string description { get; }
    public string ein { get; }
    public string logoCloudinaryId { get; }
    public string logoUrl { get; }
    public string location { get; }



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