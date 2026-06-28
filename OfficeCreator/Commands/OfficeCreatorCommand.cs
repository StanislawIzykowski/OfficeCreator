using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
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
                if (vm.ColumnTile.IsEnabled)
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
                if (vm.FloorsTile.IsEnabled) 
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
                if (vm.WallsTile.IsEnabled)
                {
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




            }
                                 

            return Result.Succeeded;
        }
    }
}
