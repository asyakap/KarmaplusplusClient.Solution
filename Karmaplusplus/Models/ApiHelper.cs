using RestSharp;

namespace Karmaplusplus.Models
{
  public class ApiHelper
  {
    public static async Task<string> GetAll()
    {
      RestClient client = new RestClient("https://localhost:7225/");
      RestRequest request = new RestRequest($"api/Services", Method.Get);
      RestResponse response = await client.GetAsync(request);
      return response.Content;
    }

    public static async Task<string> Get(int id)
    {
      RestClient client = new RestClient("https://localhost:7225/");
      RestRequest request = new RestRequest($"api/Services/{id}", Method.Get);
      RestResponse response = await client.GetAsync(request);
      return response.Content;
    }

    public static async void Post(string newServices)
    {
      RestClient client = new RestClient("https://localhost:7225/");
      RestRequest request = new RestRequest($"api/Services", Method.Post);
      request.AddHeader("Content-Type", "application/json");
      request.AddJsonBody(newServices);
      await client.PostAsync(request);
    }

    public static async void Put(int id, string newServices)
    {
      RestClient client = new RestClient("https://localhost:7225/");
      RestRequest request = new RestRequest($"api/Services/{id}", Method.Put);
      request.AddHeader("Content-Type", "application/json");
      request.AddJsonBody(newServices);
      await client.PutAsync(request);
    }
    
    public static async void Delete(int id)
    {
      RestClient client = new RestClient("https://localhost:7225/");
      RestRequest request = new RestRequest($"api/Services/{id}", Method.Delete);
      request.AddHeader("Content-Type", "application/json");
      await client.DeleteAsync(request);
    }

    public static async Task<string> Search(string name)
    {
      RestClient client = new RestClient("http://localhost:7225/");
      RestRequest request = new RestRequest($"api/Services", Method.Get);
      RestResponse response = await client.GetAsync(request);
      return response.Content;
    }
  }
}