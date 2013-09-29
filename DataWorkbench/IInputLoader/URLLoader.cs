using Roslyn.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataWorkbench.IInputLoader {
    public class URLLoader : IInputLoader {
        public object Load(string path, Session session, string content, out Type type) {
            type = typeof(XElement);
            //var info = new System.IO.FileInfo(path);
            //string toExecute = "var _ = XElement.Load(@\"" + info.FullName + "\");";
            //var result = session.Execute(toExecute);

            return XElement.Parse(content);
        }
    }
}
