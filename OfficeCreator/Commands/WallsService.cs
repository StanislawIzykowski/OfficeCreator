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

            double height = 31.496063;

            double offset = 9.84251969;


            
            Line wallLine1 = Line.CreateBound(points[0][0], points[0][colCount - 1]);
            Line wallLine2 = Line.CreateBound(points[0][colCount - 1], points[rowCount - 1][colCount - 1]);
            Line wallLine3 = Line.CreateBound(points[rowCount - 1][colCount - 1], points[rowCount - 1][0]);
            Line wallLine4 = Line.CreateBound(points[rowCount - 1][0], points[0][0]);

            IList<Curve> curveListWall = new List<Curve>()
            {
                wallLine1, wallLine2, wallLine3, wallLine4
            };

            WallType wallType = new FilteredElementCollector(doc)
                .OfClass(typeof(WallType))
                .Cast<WallType>()
                .Where(w => w.Kind != WallKind.Curtain)
                .FirstOrDefault();

            ElementId wallTypeId = wallType.Id;

            foreach (Curve curve in curveListWall)
            {
                Wall.Create(doc, curve, wallTypeId, levelId, height, offset, false, false);
            }
        }
    }
}
