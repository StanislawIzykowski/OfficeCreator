using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OfficeCreator.Commands
{
    internal class FloorsService
    {
        public void Create(Document doc, IList<IList<XYZ>> points)
        {

            int rowCount = points.Count;      // Y
            int colCount = points[0].Count;   // X
            //curveloop object
            CurveLoop curves = new CurveLoop();

            IList<CurveLoop> curveLooplist = new List<CurveLoop>() { curves };

            //creating lines based on points, closed loop!
            curves.Append(Line.CreateBound(points[0][0], points[0][colCount - 1]));                         
            curves.Append(Line.CreateBound(points[0][colCount - 1], points[rowCount - 1][colCount - 1])); 
            curves.Append(Line.CreateBound(points[rowCount - 1][colCount - 1], points[rowCount - 1][0])); 
            curves.Append(Line.CreateBound(points[rowCount - 1][0], points[0][0]));

            ElementId floorTypeId = Floor.GetDefaultFloorType(doc, true);

            double elev0 = 0.0;
            double elev4 = 4.0 * 3.28084;
            double elev8 = 8.0 * 3.28084;

            // Szukamy ID poziomów w dokumencie
            ElementId levelId0 = Level.GetNearestLevelId(doc, elev0);
            ElementId levelId4 = Level.GetNearestLevelId(doc, elev4);
            ElementId levelId8 = Level.GetNearestLevelId(doc, elev8);

            // Tworzymy 3 podłogi na 3 poziomach
            if (levelId0 != null) Floor.Create(doc, curveLooplist, floorTypeId, levelId0);
            if (levelId4 != null) Floor.Create(doc, curveLooplist, floorTypeId, levelId4);
            if (levelId8 != null) Floor.Create(doc, curveLooplist, floorTypeId, levelId8);
        }                    
    }
}
