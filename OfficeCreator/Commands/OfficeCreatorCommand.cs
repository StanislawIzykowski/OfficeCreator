using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using OfficeCreator.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeCreator.ViewModel;

namespace OfficeCreator.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class OfficeCreatorCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //creating viewmodel
            MainViewModel vm = new MainViewModel();

            //creating window and connecting data context
            OCWindow oCWindow = new OCWindow();
            oCWindow.DataContext = vm;

            //closing window
            vm.RequestClose += () => oCWindow.DialogResult = true;


            //oppening window
            if (oCWindow.ShowDialog() == true)
            {
                //declering app and doc
                UIApplication uiapp = commandData.Application;
                Document doc = uiapp.ActiveUIDocument.Document;


                // point list
                IList<IList<XYZ>> points = new List<IList<XYZ>>();

                                
                float moduleX = vm.ModuleX;
                float moduleY = vm.ModuleY;

                
                //creating list of points
                for (int i = 0; i < vm.DistanceX; i++)
                {
                    IList<XYZ> row = new List<XYZ>();
                    points.Add(row);

                    for (int j = 0; j < vm.DistanceY; j++)
                    {
                        XYZ point = new XYZ(j * moduleX * 3.281f, i * moduleY * 3.281f, 0);

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



                double elevation = 0;
                ElementId levelId = Level.GetNearestLevelId(doc, elevation);

                //family symbol columny
                FamilySymbol symbol = new FilteredElementCollector(doc)
                    .OfClass(typeof(FamilySymbol))
                    .OfCategory(BuiltInCategory.OST_Columns)
                    .Cast<FamilySymbol>()
                    .FirstOrDefault(); // .OfCategory(BuiltInCategory.OST_StructuralColumns) // konstrukcyjna

                Level poziom = new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .Cast<Level>()
                .FirstOrDefault();

                //creating grids transaction
                if (vm.GridChecker)
                {
                    using (Transaction t = new Transaction(doc))
                    {
                        t.Start("Starting transaction");

                        //Creating grid
                        GridService gridService = new GridService();

                        gridService.Create(doc, points);

                        t.Commit();                        
                    }
                }

                //Creating Columns transation
                if (vm.ColumnChecker)
                {
                    using (Transaction t = new Transaction(doc))
                    {
                        t.Start("Starting transaction");
                        
                        //Creating columns
                        ColumnsService columnsService = new ColumnsService();
                        columnsService.Create(doc, points);

                        t.Commit();
                        
                    }
                }

                //Creating Floors transaction
                if (vm.FloorChecker) 
                {
                    using (Transaction t = new Transaction(doc))
                    {
                        t.Start("Starting transaction");

                        //creating floors
                        FloorsService floorService = new FloorsService();
                        floorService.Create(doc, points);

                        t.Commit();
                    }
                }

                //creating walls transaction
                if (vm.WallChecker)
                {
                    using (Transaction t = new Transaction(doc))
                    {
                        t.Start("Starting transaction");

                        //creating walls
                        WallsService wallsService = new WallsService();
                        wallsService.Create(doc, points, levelId);

                        t.Commit();
                    }
                }
            }
            
            TaskDialog.Show("debug", $"Value = {vm.ModuleX}");
            TaskDialog.Show("debug", $"Value = {vm.ModuleY}");
                     

            return Result.Succeeded;
        }
    }
}
