using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace AojCs
{
  internal class WebAccess
  {
    private static readonly HttpClient client = new HttpClient();
    private static async Task<string> PostAccess(Uri uri, StringContent data)
    {
      var response = client.PostAsync(uri, data).Result;
      return await response.Content.ReadAsStringAsync();
    }

    public static string JsonPost(Uri uri, object jsonInfo)
    {
      var dataString = JsonConvert.SerializeObject(jsonInfo);
      var content = new StringContent(dataString, Encoding.UTF8, "application/json");
      return Task.Run(() => PostAccess(uri, content)).Result;
    }

    private static async Task<string> GetAccess(string uri, Dictionary<string, string> info)
    {
      uri += "?";
      foreach (var i in info)
      {
        uri += i.Key + "=" + i.Value + "&";
      }
      uri = uri.Substring(0, uri.Length - 1);
      var response = await client.GetAsync(uri);
      return await response.Content.ReadAsStringAsync();
    }
    public static string Get(string uri, Dictionary<string, string> info)
    {
      return Task.Run(() => GetAccess(uri, info)).Result;
    }

    public static string Get(Uri uri, Dictionary<string, string> info)
    {
      return Get(uri.ToString(), info);
    }
    public static string Get(Uri uri)
    {
      return Get(uri.ToString(), new Dictionary<string, string>());
    }
    public static string Get(string uri)
    {
      return Get(uri, new Dictionary<string, string>());
    }

    private static async Task<string> DeleteAccess(Uri uri)
    {
      var response = await client.DeleteAsync(uri);
      return await response.Content.ReadAsStringAsync();
    }
    public static void Delete(string uri)
    {
      Task.Run(() => DeleteAccess(new Uri(uri)));
    }
    public static void Delete(Uri uri)
    {
      Task.Run(() => DeleteAccess(uri));
    }
  }
}

