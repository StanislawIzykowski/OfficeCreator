using Autodesk.Revit.DB;

namespace OfficeCreator.Commands
{
    public class WallsService
    {
        public IList<Wall> Create(Document doc, IList<IList<XYZ>> points, ElementId firstWallId)
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

            //ciekawe, collecting element form id and casting to something else
            //WallType wallType = doc.GetElement( firstWallId ) as WallType;

            //new loop with offset
            CurveLoop offsetPerimeter = CurveLoop.CreateViaOffset(perimeter, curveLoopOffset, XYZ.BasisZ);

            //ElementId wallTypeId = wallType.Id;

            IList<Wall> createdWalls = new List<Wall>();

            foreach (Curve curve in offsetPerimeter)
            {
                Wall wall = Wall.Create(doc, curve, firstWallId, levelId, height, offset, false, false);
                createdWalls.Add(wall);
            }

            return createdWalls;
        }
    }
}
