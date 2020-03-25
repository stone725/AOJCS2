using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AojCs
{
  [JsonObject("AojId")]
  public class AojAccount : AojUser
  {
    public AojAccount(string id, string passWord)
    {
      Id = id;
      PassWord = passWord;
    }
    
    [JsonProperty("id")]
    public string Id { get; }

    [JsonProperty("password")]
    public string PassWord { get; }
    
    public void Login()
    {
      var uri = new Uri("https://judgeapi.u-aizu.ac.jp/session");
      var userInfo = new { password = PassWord, id = Id };
      var sessionUri = new Uri("https://judgeapi.u-aizu.ac.jp/session");
      var result = WebAccess.JsonPost(sessionUri, userInfo);
      var info = JsonConvert.DeserializeObject<AojUser>(result);

      //infoの中身をthisにコピーする
      foreach (var item in info.GetType().GetProperties())
      {
        item.SetValue(this, item.GetValue(info));
      }
    }

    public void GetSession()
    {
      var sessionUri = new Uri("https://judgeapi.u-aizu.ac.jp/self");
      var result = WebAccess.Get(sessionUri);
      var info = JsonConvert.DeserializeObject<AojUser>(result);

      //infoの中身をthisにコピーする
      foreach (var item in info.GetType().GetProperties())
      {
        item.SetValue(this, item.GetValue(info));
      }
    }

    public void Logout()
    {
      var uri = new Uri("https://judgeapi.u-aizu.ac.jp/session");
      WebAccess.Delete(uri);
    }

    ~AojAccount()
    {
      var uri = new Uri("https://judgeapi.u-aizu.ac.jp/session");
      WebAccess.Delete(uri);
    }

    //実際に提出しトークンを受け取る関数
    private string GetSubmissionToken(string problemId, string language, string sourceCode)
    {
      try
      {
        var uri = new Uri("https://judgeapi.u-aizu.ac.jp/submissions");
        var submitInfo = new { problemId, language, sourceCode };
        var result = WebAccess.JsonPost(uri, submitInfo);
        var tokenInfo = JsonConvert.DeserializeObject<submissionToken>(result);
        return tokenInfo.Token;
      }
      catch(Exception e)
      {
        throw new AojSubmitException("", e);
      }
    }

    //最近の全体の提出記録を取得する関数
    private submissionRecord[] GetSubmissionRecords()
    {
      var uri = new Uri("https://judgeapi.u-aizu.ac.jp/submission_records/recent");
      var result = WebAccess.Get(uri);
      return JsonConvert.DeserializeObject<submissionRecord[]>(result);
    }

    public submitStatus Submit(string problemId, string language, string sourceCode)
    {
      string token = GetSubmissionToken(problemId, language, sourceCode);

      //200ms * 100 = 20000ms→20sより100回試行
      for (int i = 0; i < 100; i++)
      {
        var records = GetSubmissionRecords();
        foreach (var record in records)
        {
          if (record.Token == token)
          {
            if (record.Status != submitStatus.STATE_RUNNING && record.Status != submitStatus.STATE_WAITING)
            {
              return record.Status;
            }
            break;
          }
        }
        Thread.Sleep(200);
      }
      throw new AojSubmitException();
    }

    
  }

  public enum submitStatus
  {
    STATE_COMPILEERROR,
    STATE_WRONGANSWER,
    STATE_TIMELIMIT,
    STATE_MEMORYLIMIT,
    STATE_ACCEPTED,
    STATE_WAITING,
    STATE_OUTPUTLIMIT,
    STATE_RUNTIMEERROR,
    STATE_PRESENTATIONERROR,
    STATE_RUNNING,
  }

  [JsonObject]
  internal class submissionToken
  {
    [JsonProperty("token")]
    public string Token { get; private set; }
  }

  [JsonObject]
  internal class submissionRecord
  {
    [JsonProperty("judgeId")]
    public int JudgeId { get; private set; }

    [JsonProperty("judgeType")]
    public int JudgeType { get; private set; }

    [JsonProperty("userId")]
    public string UserId { get; private set; }

    [JsonProperty("problemId")]
    public string ProblemId { get; private set; }

    [JsonProperty("submissionDate")]
    public long SubmissionDate { get; private set; }

    [JsonProperty("language")]
    public string Language { get; private set; }

    [JsonProperty("status")]
    public submitStatus Status { get; private set; }
    
    [JsonProperty("cpuTime")]
    public int CpuTime { get; private set; }
    
    [JsonProperty("memory")]
    public int Memory { get; private set; }
    
    [JsonProperty("codeSize")]
    public int CodeSize { get; private set; }
    [JsonProperty("accuracy")]
    public string Accuracy { get; private set; }
    
    [JsonProperty("judgeDate")]
    public long JudgeDate { get; private set; }
    
    [JsonProperty("score")]
    public int? Score { get; private set; }

    [JsonProperty("problemTitle")]
    public string ProblemTitle { get; private set; }
    
    [JsonProperty("token")]
    public string Token { get; private set; }
 }

}
