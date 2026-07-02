using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using OfficeCreator.ViewModel;
using System.Windows.Controls;
using System.Windows.Ink;

namespace OfficeCreator.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class OfficeCreatorCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData,  ref string message, ElementSet elements)
        {
            //declering app and doc
            UIApplication uiapp = commandData.Application;
            Document doc = uiapp.ActiveUIDocument.Document;

            //collectiong data for dictonaries
            var columnsData = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_StructuralColumns).WhereElementIsElementType().ToDictionary(e => e.Name, e => e.Id);
            var wallsData = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Walls).WhereElementIsElementType().ToDictionary(e => e.Name, e => e.Id);
            var floorsData= new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Floors).WhereElementIsElementType().ToDictionary(e => e.Name, e => e.Id);

            //creating viewmodel
            MainViewModel vm = new MainViewModel();

            // 2. Podłączasz listy do poszczególnych kafelków
            vm.ColumnsTile.ColumnsList = columnsData;
            vm.WallsTile.WallsList = wallsData;
            vm.FloorsTile.FloorsList = floorsData;

            //creating window and connecting data context
            OCWindow oCWindow = new OCWindow();
            oCWindow.DataContext = vm;

            //closing window
            vm.RequestClose += () => oCWindow.DialogResult = true;


            //oppening window
            if (oCWindow.ShowDialog() == true)
            {


                // point list
                IList<IList<XYZ>> points = new List<IList<XYZ>>();

                //generating points based on input
                PointsGenerator pointsGen = new PointsGenerator();
                points = pointsGen.Create(vm.GridTile.ModuleX, vm.GridTile.ModuleY, vm.GridTile.DistanceX, vm.GridTile.DistanceY);


                //creating grids transaction
                if (vm.GridTile.IsEnabled)
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
                if (vm.ColumnsTile.IsEnabled)
                {
                    ElementId columnPickedId = vm.ColumnsTile.PickedColumnId;
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
                if (vm.FloorsTile.IsEnabled) 
                {
                    ElementId floorPickedId = vm.FloorsTile.PickedFloorId;
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
                if (vm.WallsTile.IsEnabled)
                {
                    ElementId wallPickedId = vm.WallsTile.PickedWallId;
                    using (Transaction t = new Transaction(doc))
                    {
                        t.Start("Starting transaction");

                        //creating walls
                        WallsService wallsService = new WallsService();
                        wallsService.Create(doc, points);

                        t.Commit();
                    }
                }

                using (Transaction t = new Transaction(doc))
                {
                    t.Start("Starting transaction");

                    ViewService viewService = new ViewService();
                    viewService.Create(doc);

                    t.Commit();
                }

                double elev0 = 0.0;
                double elev4 = 4.0 * 3.28084;

                ElementId levelId0 = Level.GetNearestLevelId(doc, elev0);
                ElementId levelId4 = Level.GetNearestLevelId(doc, elev4);

                // oppening editing scope of stairs, close after transaction!
                StairsEditScope editScope = new StairsEditScope(doc, "Tworzenie schodów");
                ElementId newStairsId = editScope.Start(levelId0, levelId4);

                using (Transaction t = new Transaction(doc))
                {
                    t.Start("Starting transaction");

                    StairsService starisService = new StairsService();
                    starisService.Create(doc, points, newStairsId, vm.GridTile.ModuleX, vm.GridTile.ModuleY);

                    t.Commit();
                }
                //closing edit scope of staris
                editScope.Commit(new StairsFailurePreprocessor());

            }
                                 

            return Result.Succeeded;
        }
    }
}
