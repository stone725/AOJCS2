using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AojCs
{
    [JsonObject("SampleCase")]
    public class SampleCase
    {
        [JsonProperty("problemId")]
        public string ProblemId { get; private set; }

        [JsonProperty("serial")]
        public ushort? Serial { get; private set; }

        [JsonProperty("in")]
        public string Input { get; private set; }

        [JsonProperty("out")]
        public string Output { get; private set; }
    }
}
