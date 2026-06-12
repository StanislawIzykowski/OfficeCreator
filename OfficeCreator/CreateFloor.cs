using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OfficeCreator
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class CreateFloor : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements) 
        {
            UIApplication uiapp = commandData.Application;
            Document doc = uiapp.ActiveUIDocument.Document;

            TaskDialog.Show("dziala", "dziala");

            OCWindow oCWindow = new OCWindow();

            //points for curves
            XYZ p1 = new XYZ( 0, 0, 0);
            XYZ p2 = new XYZ( 10, 0, 0);
            XYZ p3 = new XYZ( 10, 10, 0);
            XYZ p4 = new XYZ( 0, 10, 0);

            //curveloop object
            CurveLoop curves = new CurveLoop();

            IList<CurveLoop> curveLooplist = new List<CurveLoop>() { curves };

            //creating lines based on points, closed loop!
            curves.Append(Line.CreateBound(p1, p2));
            curves.Append(Line.CreateBound(p2 , p3));
            curves.Append(Line.CreateBound(p3, p4));
            curves.Append(Line.CreateBound(p4 , p1));
            

            

            ElementId floorTypeId = Floor.GetDefaultFloorType(doc, true);

            double elevation = 0;
            ElementId levelId = Level.GetNearestLevelId(doc, elevation);

            

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Starting transaction");

                //creating floor
                Floor.Create(doc, curveLooplist, floorTypeId, levelId);

                t.Commit();
            }

                return Result.Succeeded;
        }

       

        Result OnStartup(UIControlledApplication application)
        {
            throw new NotImplementedException();
        }
    }
}
