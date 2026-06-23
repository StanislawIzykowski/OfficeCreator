using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using System.Reflection;



namespace OfficeCreator
    
{
    public class Ribbon : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application) 
        {
            string tabName1 = "Ma Apps";
            //stworzenie ribbon taba
            application.CreateRibbonTab(tabName1);
            
            string assemblyPath = Assembly.GetExecutingAssembly().Location;
            //stworzenie ribbona
            RibbonPanel panel1 = application.CreateRibbonPanel(tabName1, "Guzik");
            //stworzenie guzika
            PushButtonData button = new PushButtonData("NAME", "TEXT", assemblyPath, "OfficeCreator.Commands.OpenWindowCommand");
            //dodanie buttona do panelu
            panel1.AddItem(button);

            return Result.Succeeded;
        }

       
    }
}
