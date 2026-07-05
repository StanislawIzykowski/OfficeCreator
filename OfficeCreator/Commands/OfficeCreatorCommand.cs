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
            var windowsData = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Windows).WhereElementIsElementType().ToDictionary(e => e.Name, e => e.Id);
            var doorsData = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Doors).WhereElementIsElementType().ToDictionary(e => e.Name, e => e.Id);

            //creating viewmodel
            MainViewModel vm = new MainViewModel();

            // connecting list to drop boxes columns
            vm.ColumnsTile.ListOne   = columnsData;
            vm.ColumnsTile.ListTwo   = columnsData;
            vm.ColumnsTile.ListThree = columnsData;

            // connecting list to drop boxes walls
            vm.WallsTile.ListOne   = wallsData;
            vm.WallsTile.ListTwo   = wallsData;
            vm.WallsTile.ListThree = wallsData;            
            
            // connecting list to drop boxes floors
            vm.FloorsTile.ListOne   = floorsData;
            vm.FloorsTile.ListTwo   = floorsData;
            vm.FloorsTile.ListThree = floorsData;

            //connecting windows and doors
            vm.WindowTile.ListOne = windowsData;
            vm.DoorTile.ListOne= doorsData;

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

                //created walls 
                IList<Wall> createdWalls = new List<Wall>();

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
                    //getting picked elems
                    ElementId firstPickedId = vm.ColumnsTile.PickedElemIdFromListOne;
                    ElementId secondPickedId = vm.ColumnsTile.PickedElemIdFromListTwo;
                    ElementId thirdPickedId = vm.ColumnsTile.PickedElemIdFromListThree;

                    using (Transaction t = new Transaction(doc))
                    {
                        t.Start("Starting transaction");
                        
                        //Creating columns
                        ColumnsService columnsService = new ColumnsService();
                        columnsService.Create(doc, points, firstPickedId, secondPickedId, thirdPickedId);

                        t.Commit();
                        
                    }
                }

                //Creating Floors transaction
                if (vm.FloorsTile.IsEnabled)
                {
                    //getting picked elems
                    ElementId firstPickedId =  vm.FloorsTile.PickedElemIdFromListOne;
                    ElementId secondPickedId = vm.FloorsTile.PickedElemIdFromListTwo;
                    ElementId thirdPickedId =  vm.FloorsTile.PickedElemIdFromListThree;

                    using (Transaction t = new Transaction(doc))
                    {
                        t.Start("Starting transaction");

                        //creating floors
                        FloorsService floorService = new FloorsService();
                        floorService.Create(doc, points, firstPickedId, secondPickedId, thirdPickedId);

                        t.Commit();
                    }
                }

                //creating walls transaction
                if (vm.WallsTile.IsEnabled)
                {
                    //getting picked elems
                    ElementId firstPickedId =  vm.WallsTile.PickedElemIdFromListOne;
                    //ElementId secondPickedId = vm.WallsTile.PickedElemIdFromListTwo;
                    //ElementId thirdPickedId =  vm.WallsTile.PickedElemIdFromListThree;

                    using (Transaction t = new Transaction(doc))
                    {
                        t.Start("Starting transaction");

                        //creating walls
                        WallsService wallsService = new WallsService();
                        createdWalls = wallsService.Create(doc, points, firstPickedId);

                        t.Commit();
                    }
                }

                //creating windows transaction
                if (vm.WindowTile.IsEnabled)
                {
                    ElementId firstPickedId = vm.WindowTile.PickedElemIdFromListOne;

                    using (Transaction t = new Transaction(doc))
                    {                      
                        t.Start("Starting transaction");

                        WindowService windowService = new WindowService();
                        windowService.Create(doc, points, vm.GridTile.ModuleX, vm.GridTile.ModuleY, firstPickedId, createdWalls);

                        t.Commit();
                    }
                }

                //creating doors transation
                if (vm.DoorTile.IsEnabled)
                {
                    using(Transaction t = new Transaction(doc))
                    {
                        t.Start("Starting transaction");

                        DoorService doorService = new DoorService();
                        doorService.Create(doc, points);
                    }
                }

                //creating views transaction
                using (Transaction t = new Transaction(doc))
                {
                    t.Start("Starting transaction");

                    ViewService viewService = new ViewService();
                    viewService.Create(doc);

                    t.Commit();
                }

                double elev0 = 0.0;
                double elev4 = UnitUtils.ConvertToInternalUnits(4, UnitTypeId.Meters);

                ElementId levelId0 = Level.GetNearestLevelId(doc, elev0);
                ElementId levelId4 = Level.GetNearestLevelId(doc, elev4);

                using (StairsEditScope editScope = new StairsEditScope(doc, "Tworzenie schodów"))
                {
                    // Start edit scope BEZ otwartej transakcji na zewnątrz
                    ElementId newStairsId = editScope.Start(levelId0, levelId4);

                    using (Transaction t = new Transaction(doc, "Dodawanie biegów schodów"))
                    {
                        t.Start();

                        StairsService stairsService = new StairsService();
                        //stairsService.Create(doc, points, newStairsId, vm.GridTile.ModuleX, vm.GridTile.ModuleY);

                        t.Commit();
                    }

                    // Zamknięcie edit scope'a
                    editScope.Commit(new StairsFailurePreprocessor());
                }

            }
                                 

            return Result.Succeeded;
        }
    }
}
