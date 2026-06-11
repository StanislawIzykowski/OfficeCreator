using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeCreator
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class CreateFloor : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData) 
        {
            UIApplication uiapp = commandData.Application;
            Document doc = uiapp.ActiveUIDocument.Document;

            TaskDialog.Show("dziala", "dziala");

            OCWindow oCWindow = new OCWindow();

            return Result.Succeeded;
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            throw new NotImplementedException();
        }

        Result OnStartup(UIControlledApplication application)
        {
            throw new NotImplementedException();
        }
    }
}
