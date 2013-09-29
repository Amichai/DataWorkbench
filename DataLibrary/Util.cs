using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary
{
    public static class FunctionLibrary
    {
        private static System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

        public static string GetChart() {
            var d = new[] { 
                new { frequency = .01, letter = "A" }, 
                new { frequency = .041, letter = "B" }, 
                new { frequency = .041, letter = "C" }, 
                new { frequency = .31, letter = "D" }, 
                new { frequency = .21, letter = "E" }, 
                new { frequency = .11, letter = "F" }, 
            };
            
            var serialized = serializer.Serialize(d);
            var j =JArray.Parse(serialized);
            var root = new JObject(new JProperty("Chart", j));
            return root.ToString();
        }

        private static double parse(string val) {
            double toReturn;
            if (double.TryParse(val, out toReturn)) {
                return toReturn;
            } else {
                return 0;
            }
        }

        public static string Chart(this JArray json, string prop1, string prop2) {
            //var d = new List<dynamic>();
            var d = json.Select(i => new {
                y =
                
                parse(i[prop1].ToString())
                
                , 
                
                x = i[prop2].ToString() });
            //foreach (var e in json) {
            //    try {
            //        d.Add(new { y = double.Parse(e[prop1].ToString()), x = e[prop2].ToString() });
            //    } catch {

            //    }
            //}
            //var d = new[] { 
            //    new { y = .01, x = "A" }, 
            //    new { y = .041, x = "B" }, 
            //    new { y = .041, x = "C" }, 
            //    new { y = .31, x = "D" }, 
            //    new { y = .21, x = "E" }, 
            //    new { y = .11, x = "F" }, 
            //};

            var serialized = serializer.Serialize(d);
            var j = JArray.Parse(serialized);
            var root = new JObject(new JProperty("Chart", j));
            return root.ToString();
        }
    }
}
