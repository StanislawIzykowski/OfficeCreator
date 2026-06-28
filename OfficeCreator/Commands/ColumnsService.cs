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
            int rowCount = points.Count;      // Y
            int colCount = points[0].Count;   // X

            double elev0 = 0.0;
            double elev4 = 4.0 * 3.28084;
            double elev8 = 8.0 * 3.28084;

            // getting lvl id
            ElementId levelId0 = Level.GetNearestLevelId(doc, elev0);
            ElementId levelId4 = Level.GetNearestLevelId(doc, elev4);

            //creating list to cycle
            List<ElementId> levelIds = new List<ElementId> { levelId0, levelId4 };

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

            symbol.Activate();

            foreach( ElementId lvlId in levelIds)
            {
                Level currentLevel = doc.GetElement(lvlId) as Level;
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {

                        //creating columns on point list
                        FamilyInstance columnGF = doc.Create.NewFamilyInstance(points[i][j], symbol, currentLevel, 0);

                    }
                }
            }
     
        }
    }
}
