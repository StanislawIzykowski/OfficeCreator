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
        public void Create(Document doc, IList<IList<XYZ>> points, ElementId levelId)
        {

            Line wallLine1 = Line.CreateBound(points[0][0], points[0][points.Count - 1]);
            Line wallLine2 = Line.CreateBound(points[0][points.Count - 1], points[points.Count - 1][points.Count - 1]);
            Line wallLine3 = Line.CreateBound(points[points.Count - 1][points.Count - 1], points[points.Count - 1][0]);
            Line wallLine4 = Line.CreateBound(points[points.Count - 1][0], points[0][0]);
            IList<Curve> curveListWall = new List<Curve>();
            curveListWall.Add(wallLine1);
            curveListWall.Add(wallLine2);
            curveListWall.Add(wallLine3);
            curveListWall.Add(wallLine4);


            WallType wallType = new FilteredElementCollector(doc)
                .OfClass(typeof(WallType))
                .Cast<WallType>()
                .FirstOrDefault();

            foreach (Curve curve in curveListWall)
            {
                Wall.Create(doc, curve, levelId, false);
            }

        }
    }
}
