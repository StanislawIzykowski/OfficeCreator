using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
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
        public void Create(Document doc, IList<IList<XYZ>> points, ElementId cornerColumnId, ElementId wallColumnId, ElementId innerColumnId)
        {
            int rowCount = points.Count;          // Y
            int colCount = points[0].Count;       // X

            double elev0 = 0.0;
            double elev4 = UnitUtils.ConvertToInternalUnits(4, UnitTypeId.Meters);

            ElementId levelId0 = Level.GetNearestLevelId(doc, elev0);
            ElementId levelId4 = Level.GetNearestLevelId(doc, elev4);
            List<ElementId> levelIds = new List<ElementId> { levelId0, levelId4 };

            // collecting symbols form elemIds
            FamilySymbol cornerSymbol = doc.GetElement(cornerColumnId) as FamilySymbol;
            FamilySymbol wallSymbol = doc.GetElement(wallColumnId) as FamilySymbol;
            FamilySymbol innerSymbol = doc.GetElement(innerColumnId) as FamilySymbol;

            if (cornerSymbol == null || wallSymbol == null || innerSymbol == null)
                throw new InvalidOperationException("Nie udało się pobrać jednego z FamilySymbol po podanym ElementId.");

            // activating symblos
            if (!cornerSymbol.IsActive) cornerSymbol.Activate();
            if (!wallSymbol.IsActive) wallSymbol.Activate();
            if (!innerSymbol.IsActive) innerSymbol.Activate();

            foreach (ElementId lvlId in levelIds)
            {
                Level currentLevel = doc.GetElement(lvlId) as Level;

                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        bool isTopOrBottomRow = (i == 0 || i == rowCount - 1);
                        bool isLeftOrRightCol = (j == 0 || j == colCount - 1);

                        FamilySymbol symbolToUse;

                        if (isTopOrBottomRow && isLeftOrRightCol)
                        {
                            symbolToUse = cornerSymbol;
                        }
                        else if (isTopOrBottomRow || isLeftOrRightCol)
                        {
                            symbolToUse = wallSymbol;
                        }
                        else
                        {
                            symbolToUse = innerSymbol;
                        }

                        doc.Create.NewFamilyInstance(points[i][j], symbolToUse, currentLevel, StructuralType.NonStructural);
                    }
                }
            }
        }
    }
}
