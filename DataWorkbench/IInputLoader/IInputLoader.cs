using Roslyn.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataWorkbench.IInputLoader {
    public interface IInputLoader {
        object Load(string path, Session session, string content, out Type type);
    }
}
