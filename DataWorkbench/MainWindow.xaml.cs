using DataWorkbench.IInputLoader;
using Newtonsoft.Json.Linq;
using Roslyn.Compilers;
using Roslyn.Scripting;
using Roslyn.Scripting.CSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace DataWorkbench {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged {
        public MainWindow() {
            InitializeComponent();
            this.loaders = new List<IInputLoader.IInputLoader>();
            this.loaders.Add(new XMLLoader());
            this.loaders.Add(new URLLoader());
            this.loaders.Add(new JSONLoader());
            loadScriptEngine();
            loadDataSources();
        }
        ///TODO: 
        ///Intellisense
        ///the ability to save the results of queries
        ///charting
        ///a data panel and a query panel (a visualization panel?)
        ///Supported formats: xml, json, csv, text
        ///Tabbed interface

        

        private void loadDataSources() {           
            XElement root = XElement.Load(@"..\..\DataSources.xml");
            this.dataSources.ItemsSource = root.Elements("Source").Select(i => i.Value).ToList();
        }

        private void loadScriptEngine() {
            engine = new ScriptEngine();
            engine.AddReference(typeof(System.Linq.Enumerable).Assembly.Location);
            engine.AddReference(typeof(JObject).Assembly.Location);
            engine.AddReference(typeof(XElement).Assembly.Location);

            
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
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name) {
            var eh = PropertyChanged;
            if (eh != null) {
                eh(this, new PropertyChangedEventArgs(name));
            }
        }

        ScriptEngine engine;
        Session session;

        //Text, csv, json, xml
        List<IInputLoader.IInputLoader> loaders;

        Type dataType;
        object dataSet;
        public string Tester { get; set; }
        private void query(string inputText) {
            if (inputText == "") {
                return;
            }
            
            try {
                var result = session.Execute(inputText);
                appendQueryResult(result);
            } catch (Exception ex) {
                appendQueryResult(ex.Message);
            }
        }

        private void appendQueryResult(object result) {
            this.queryResult.Text += result.ToString() + "\n";
            this.queryScrollViewer.ScrollToEnd();
        }

        private void load(string inputText) {
            this.inputStack.Push(inputText);
            foreach (var loader in loaders) {
                string content = null;
                try {
                    content = inputText.GetContent();
                } catch {

                }

                try {
                    this.dataSet = loader.Load(inputText, session, content, out dataType);
                    appendText(dataSet);
                    break;
                } catch {

                }
            }

            //try {
            //    appendText(inputText.GetContent());
            //} catch {
            //    try {
            //        var result = session.Execute(inputText);
            //        appendText(result.ToString());
            //    } catch (Exception ex) {
            //        appendText(ex.Message);
            //    }
            //}
        }



        private void appendText(dynamic text) {
            //text = text.Replace("\"", "\"\"");
            //session.Execute("var _ = @\"" + text + "\";");
            this.resultWindow.Text += text + "\n";
            this.scrollViewer.ScrollToEnd();
        }

        Stack<string> inputStack = new Stack<string>();

        private void Window_KeyDown_1(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                this.load(this.inputText.Text);
                this.inputText.Text = "";
                this.query(this.queryText.Text);
                this.queryText.Text = "";

            }
        }

        private void inputText_KeyDown_1(object sender, KeyEventArgs e) {
            if (e.Key == Key.Up) {
                if (!this.inputStack.Any()) {
                    return;
                }
                this.inputText.Text = this.inputStack.Pop();
            }
        }

        private void TextBox_PreviewMouseDown_1(object sender, MouseButtonEventArgs e) {
            this.inputText.Text = (sender as TextBlock).Tag as string;
        }

        private void Save_Click_1(object sender, RoutedEventArgs e) {

        }

        private void queryText_PreviewKeyDown_1(object sender, KeyEventArgs e) {
            
        }
    }
}
