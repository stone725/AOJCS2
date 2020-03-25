using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AojCs
{
    public class AojProblem
    {
        public string Id { get; }

        public SampleCase[] sampleCases { get; }

        public AojProblem(string Id)
        {
            this.Id = Id;
            var sessionUri = new Uri("https://judgedat.u-aizu.ac.jp/testcases/samples/" + Id);
            var result = WebAccess.Get(sessionUri);
            sampleCases = JsonConvert.DeserializeObject<SampleCase[]>(result);
        }
    }
}
