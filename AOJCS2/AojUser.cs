using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AojCs
{
  [JsonObject("AojUser")]
  public class AojUser
  {
    [JsonProperty("id")]
    public string Id { get; protected set; }

    [JsonProperty("name")]
    public string Name { get; private set; }

    [JsonProperty("affiliation")]
    public string Affiliation { get; private set; }

    [JsonProperty("registerDate")]
    public long RegisterDate { get; private set; }

    [JsonProperty("lastSubmitDate")]
    public long LastSubmitDate { get; private set; }

    [JsonProperty("policy")]
    public string Policy { get; private set; }

    [JsonProperty("country")]
    public string Country { get; private set; }

    [JsonProperty("birthYear")]
    public string BirthYear { get; private set; }

    [JsonProperty("displayLanguage")]
    public string DisplayLanguage { get; private set; }

    [JsonProperty("defaultProgrammingLanguage")]
    public string DefaultProgrammingLanguage { get; private set; }

    [JsonProperty("status")]
    public userStatus Status { get; private set; } 

    [JsonProperty("url")]
    public string Url { get; private set; }

    public AojUser(string userId)
    {
      var sessionUri = new Uri("https://judgeapi.u-aizu.ac.jp/users/" + userId);
      var result = WebAccess.Get(sessionUri);
      var info = JsonConvert.DeserializeObject<AojUser>(result);

      //infoの中身をthisにコピーする
      foreach (var item in info.GetType().GetProperties())
      {
        item.SetValue(this, item.GetValue(info));
      }
    }

    public AojUser(AojUser user)
    {
      foreach(var item in user.GetType().GetProperties())
      {
        item.SetValue(this, item.GetValue(user));
      }
    }

    public AojUser()
    {
    }

    public List<string> GetSolvedProblems()
    {
      var sessionUri = new Uri("https://judgeapi.u-aizu.ac.jp/solutions/users/" + Id + "?size=1000000");
      var result = WebAccess.Get(sessionUri);
      var info = JsonConvert.DeserializeObject<solution[]>(result);
      List<string> solvedProblems = new List<string>();
      foreach (var i in info)
      {
        if (solvedProblems.Contains(i.ProblemId))
        {
          continue;
        }
        solvedProblems.Add(i.ProblemId);
      }
      solvedProblems.Sort();
      return solvedProblems;
    }

    //引数にとったユーザが解いた中で自分が解いていない問題をListに入れる
    public List<string> GetDiff(params AojUser[] rivals)
    {
      var solvedProblems = GetSolvedProblems();
      var rivalsOnlySolvedList = new List<string>();

      foreach (var rival in rivals)
      {
        rivalsOnlySolvedList.AddRange(rival.GetSolvedProblems());
      }

      rivalsOnlySolvedList.Sort();
      rivalsOnlySolvedList.RemoveAll(solvedProblems.Contains);

      return rivalsOnlySolvedList;
    }
  }

  [JsonObject()]
  public class userStatus
  {
    [JsonProperty("submissions")]
    public int Submissions { get; private set; }

    [JsonProperty("solved")]
    public int Solved { get; private set; }

    [JsonProperty("accepted")]
    public int Accepted { get; private set; }

    [JsonProperty("wrongAnswer")]
    public int WrongAnswer { get; private set; }

    [JsonProperty("timeLimit")]
    public int TimeLimit { get; private set; }

    [JsonProperty("memoryLimit")]
    public int MemoryLimit { get; private set; }

    [JsonProperty("outputLimit")]
    public int OutputLimit { get; private set; }

    [JsonProperty("compileError")]
    public int CompileError { get; private set; }

    [JsonProperty("runtimeError")]
    public int RuntimeError { get; private set; }
  }

  [JsonObject]
  internal class solution
  {
    [JsonProperty("judgeId")]
    public long JudgeId { get; private set; }

    [JsonProperty("userId")]
    public string UserId { get; private set; }

    [JsonProperty("problemId")]
    public string ProblemId { get; private set; }

    [JsonProperty("language")]
    public string Language { get; private set; }

    [JsonProperty("version")]
    public string Version { get; private set; }

    [JsonProperty("submissionDate")]
    public long? SubmissionDate { get; private set; }

    [JsonProperty("judgeDate")]
    public long? JudgeDate { get; private set; }

    [JsonProperty("cpuTime")]
    public long? CpuTime { get; private set; }

    [JsonProperty("memory")]
    public int? Memory { get; private set; }

    [JsonProperty("codeSize")]
    public int? CodeSize { get; private set; }

    [JsonProperty("server")]
    public int? Server { get; private set; }

    [JsonProperty("policy")]
    public string Policy { get; private set; }

    [JsonProperty("rating")]
    public double? Rating { get; private set; }

    [JsonProperty("review")]
    public int? Review { get; private set; }
  }
}
