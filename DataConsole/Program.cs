using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataWorkbench;
using Newtonsoft.Json.Linq;

namespace DataConsole {
    class Program {
        static void Main(string[] args) {
            string url = @"https://explore.data.gov/resource/veterans-burial-sites.json";
            var content = url.GetContent();
            var json = JArray.Parse(content);
            
        }
    }
}
