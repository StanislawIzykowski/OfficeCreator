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
    public class OfficeCreatorCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //creating viewmodel
            MainViewModel vm = new MainViewModel();

            //creating window
            OCWindow oCWindow = new OCWindow();

            //connecting data context
            oCWindow.DataContext = vm;

            vm.RequestClose += () => oCWindow.DialogResult = true;
            //oppening window
            if (oCWindow.ShowDialog() == true)
            {
                
            }            
            
            TaskDialog.Show("debug", $"Value = {vm.Value}");
            //collecting data context

            //transactions

            return Result.Succeeded;
        }
    }
}
