using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeCreator.Commands
{
    public class WallsService
    {
        public void Create(Document doc, IList<IList<XYZ>> points)
        {

            int rowCount = points.Count;      
            int colCount = points[0].Count;  
            
            double elevation = 0;
            ElementId levelId = Level.GetNearestLevelId(doc, elevation);

            double height = 27.06693;
            double offset = 0;
            double curveLoopOffset = 1.295932;

            //obwód
            CurveLoop perimeter = new CurveLoop();
            perimeter.Append(Line.CreateBound(points[0][0], points[0][colCount - 1]));
            perimeter.Append(Line.CreateBound(points[0][colCount - 1], points[rowCount - 1][colCount - 1]));
            perimeter.Append(Line.CreateBound(points[rowCount - 1][colCount - 1], points[rowCount - 1][0]));
            perimeter.Append(Line.CreateBound(points[rowCount - 1][0], points[0][0]));

            WallType wallType = new FilteredElementCollector(doc)
                .OfClass(typeof(WallType))
                .Cast<WallType>()
                .Where(w => w.Kind != WallKind.Curtain)
                .FirstOrDefault();

            //new loop with offset
            CurveLoop offsetPerimeter = CurveLoop.CreateViaOffset(perimeter, curveLoopOffset, XYZ.BasisZ);

            ElementId wallTypeId = wallType.Id;

            foreach (Curve curve in offsetPerimeter)
            {
                Wall.Create(doc, curve, wallTypeId, levelId, height, offset, false, false);
            }
        }
    }
}
