using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace Karmaplusplus.Models
{
[Keyless]
  public class Charity
  {
    public int Name { get; }
    public string ProfileUrl { get; }
    public string Description { get; }
    public string Ein { get; }
    public string LogoCloudinaryId { get; }
    public string LogoUrl { get; }
    public string Location { get; }



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