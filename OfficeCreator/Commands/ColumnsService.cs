using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OfficeCreator.Commands
{

    public class ColumnsService
    {     
        public void Create(Document doc, IList<IList<XYZ>> points)
        {
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

            Level groundLevel = new FilteredElementCollector(doc)
            .OfClass(typeof(Level))
            .Cast<Level>()
            .FirstOrDefault();



            for (int i = 0; i < points.Count; i++)
            {
                for (int j = 0; j < points.Count; j++)
                {
                    symbol.Activate();
                    //creating columns on point list
                    FamilyInstance columnGF = doc.Create.NewFamilyInstance(points[i][j], symbol, groundLevel, 0);
                    
                }
            }
     
        }
    }
}
