using Newtonsoft.Json.Linq;
using Roslyn.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataWorkbench.IInputLoader {
    public class JSONLoader : IInputLoader {
        public object Load(string path, Session session, string content, out Type type) {
            type = typeof(JObject);

            var jobject = JArray.Parse(content);
            var escapedString = content.Replace("\"", "\"\"");
            var assign = "var stringVal = @\"" + escapedString + "\";";
            var result = session.Execute(assign);

            string toExecute = "var _ = JArray.Parse(stringVal);";
            result = session.Execute(toExecute);
            
            ///Excecute an include statement!
            return jobject;
        }
    }
}
