using DataWorkbench.IInputLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataWorkbench;
using System.Diagnostics;
using Roslyn.Scripting;
using Roslyn.Scripting.CSharp;
using Roslyn.Compilers;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using DataLibrary;

namespace DataWeb.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            loadScriptEngine();
            return View();
        }

        public ActionResult About() {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        static List<IInputLoader> loaders = new List<IInputLoader>() {
            //new XMLLoader(),
            //new URLLoader(),
            new JSONLoader()
        };

        private static Session session;
        private static ScriptEngine engine;

        private void loadScriptEngine() {
            engine = new ScriptEngine();
            engine.AddReference(typeof(System.Linq.Enumerable).Assembly.Location);
            engine.AddReference(typeof(JObject).Assembly.Location);
            engine.AddReference(typeof(XElement).Assembly.Location);
            
            engine.AddReference(typeof(FunctionLibrary).Assembly.Location);



            engine.AddReference(new MetadataFileReference(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.dll"));
            engine.AddReference(new MetadataFileReference(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Xml.dll"));


            engine.ImportNamespace("System");
            engine.ImportNamespace("System.Collections.Generic");
            engine.ImportNamespace("System.Linq");
            engine.ImportNamespace("System.Text");
            engine.ImportNamespace("System.Diagnostics");
            engine.ImportNamespace("Newtonsoft.Json.Linq");
            engine.ImportNamespace("System.Xml.Linq");

            session = engine.CreateSession(this);

            session.Execute("using DataLibrary;");
        }

        

        [HttpPost]
        public ActionResult GetChart() {
            //var d = new[] { 
            //    new { frequency = .01, letter = "A" }, 
            //    new { frequency = .041, letter = "B" }, 
            //    new { frequency = .041, letter = "C" }, 
            //    new { frequency = .31, letter = "D" }, 
            //    new { frequency = .21, letter = "E" }, 
            //    new { frequency = .11, letter = "F" }, 
            //};
            //var serialized = serializer.Serialize(d);
            //return Json(serialized);
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult Load(string inputText) {
            if (string.IsNullOrWhiteSpace(inputText)) {
                return Json("");
            }
            Type dataType;

            foreach (var loader in loaders) {
                string content = null;
                try {
                    content = inputText.GetContent();
                } catch {

                }

                try {
                    var result = loader.Load(inputText, session, content, out dataType);
                    if (inputText.Last() != ';') {
                        return Json(result.ToString());
                    } else {
                        return Json("Success.");
                    }
                } catch (Exception ex){
                    Debug.Print("Error: " + ex.Message);
                }
            }

            return Json("Failed to load.");
        }

        [HttpPost]
        public ActionResult Query(string queryText) {
            try {
                var result = session.Execute(queryText);
                return Json(result.ToString());
            } catch (Exception ex) {
                return Json(ex.Message);
            }

        }
    }
}
