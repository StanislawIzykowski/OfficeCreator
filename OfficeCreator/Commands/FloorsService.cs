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

            int rowCount = points.Count;      
            int colCount = points[0].Count;
            double curveLoopOffset = 1.295932;

            //curveloop object
            CurveLoop curves = new CurveLoop();

            //creating lines based on points, closed loop!
            //obwód
            curves.Append(Line.CreateBound(points[0][0], points[0][colCount - 1]));
            curves.Append(Line.CreateBound(points[0][colCount - 1], points[rowCount - 1][colCount - 1]));
            curves.Append(Line.CreateBound(points[rowCount - 1][colCount - 1], points[rowCount - 1][0]));
            curves.Append(Line.CreateBound(points[rowCount - 1][0], points[0][0]));

            CurveLoop offsetPerimeter = CurveLoop.CreateViaOffset(curves, curveLoopOffset, XYZ.BasisZ);
            IList<CurveLoop> curveLoopList = new List<CurveLoop>() { offsetPerimeter };

            ElementId floorTypeId = Floor.GetDefaultFloorType(doc, true);

            double elev0 = 0.0;
            double elev4 = 4.0 * 3.28084;
            double elev8 = 8.0 * 3.28084;

            // Szukamy ID poziomów w dokumencie
            ElementId levelId0 = Level.GetNearestLevelId(doc, elev0);
            ElementId levelId4 = Level.GetNearestLevelId(doc, elev4);
            ElementId levelId8 = Level.GetNearestLevelId(doc, elev8);

            // Tworzymy 3 podłogi na 3 poziomach
            if (levelId0 != null) Floor.Create(doc, curveLoopList, floorTypeId, levelId0);
            if (levelId4 != null) Floor.Create(doc, curveLoopList, floorTypeId, levelId4);
            if (levelId8 != null) Floor.Create(doc, curveLoopList, floorTypeId, levelId8);
        }                    
    }
}
