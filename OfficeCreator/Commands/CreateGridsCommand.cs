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
    [Regeneration(RegenerationOption.Manual)]
    public class CreateGridsCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            Document doc = uiapp.ActiveUIDocument.Document;

            OCWindow oCWindow = new OCWindow();

            //column point list
            IList<IList<XYZ>> points = new List<IList<XYZ>>();

            int module = 6;
            //creating list of points
            for (int i = 0; i < 100; i += 10)
            {
                IList<XYZ> row = new List<XYZ>();
                points.Add(row);

                for (int j = 0; j < 100; j += 10)
                {
                    XYZ point = new XYZ(j * module, i * module, 0);

                    //adding point
                    row.Add(point);
                }
            }


            //curveloop object
            CurveLoop curves = new CurveLoop();

            IList<CurveLoop> curveLooplist = new List<CurveLoop>() { curves };

            //creating lines based on points, closed loop!
            curves.Append(Line.CreateBound(points[0][0], points[0][points.Count - 1]));
            curves.Append(Line.CreateBound(points[0][points.Count - 1], points[points.Count - 1][points.Count - 1]));
            curves.Append(Line.CreateBound(points[points.Count - 1][points.Count - 1], points[points.Count - 1][0]));
            curves.Append(Line.CreateBound(points[points.Count - 1][0], points[0][0]));

            ElementId floorTypeId = Floor.GetDefaultFloorType(doc, true);

            double elevation = 0;
            ElementId levelId = Level.GetNearestLevelId(doc, elevation);

            //family symbol columny
            FamilySymbol symbol = new FilteredElementCollector(doc)
            .OfClass(typeof(FamilySymbol))
            .OfCategory(BuiltInCategory.OST_Columns) // architektoniczna
            // .OfCategory(BuiltInCategory.OST_StructuralColumns) // konstrukcyjna
            .Cast<FamilySymbol>()
            .FirstOrDefault();

            Level poziom = new FilteredElementCollector(doc)
            .OfClass(typeof(Level))
            .Cast<Level>()
            .FirstOrDefault();

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Starting transaction");

                //Creating grid

                for (int i = 0; i < 10; i++)
                {
                    Line gridLineX = Line.CreateBound(points[i][0], points[i][points.Count - 1]);
                    Line gridLineY = Line.CreateBound(points[0][i], points[points.Count - 1][i]);
                    Grid.Create(doc, gridLineX);
                    Grid.Create(doc, gridLineY);
                }

                t.Commit();
            }

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Starting transaction");

                for (int i = 0; i < points.Count; i++)
                {
                    for (int j = 0; j < points.Count; j++)
                    {
                        //creating columns on point list
                        FamilyInstance column = doc.Create.NewFamilyInstance(points[i][j], symbol, poziom, 0);
                    }
                }

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
