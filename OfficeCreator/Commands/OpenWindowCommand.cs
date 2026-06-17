using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeCreator.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class OpenWindowCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            OCWindow oCWindow = new OCWindow();

            oCWindow.Show();
            
            return Result.Succeeded;
        }
    }
}
