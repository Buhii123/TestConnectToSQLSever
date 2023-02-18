using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace TestFilterDuctPipeWithSQL
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class CommandDuctPipe : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication UIAPP = commandData.Application;
            UIDocument UIDOC = UIAPP.ActiveUIDocument;
            Document DOC = UIDOC.Document;

            





            ViewMainApp app = new ViewMainApp(UIDOC);
            app.ShowDialog();
            return Result.Succeeded;
        }
    }
}
