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
            //curveloop object
            CurveLoop curves = new CurveLoop();

            IList<CurveLoop> curveLooplist = new List<CurveLoop>() { curves };

            //creating lines based on points, closed loop!
            curves.Append(Line.CreateBound(points[0][0], points[0][points.Count - 1]));
            curves.Append(Line.CreateBound(points[0][points.Count - 1], points[points.Count - 1][points.Count - 1]));
            curves.Append(Line.CreateBound(points[points.Count - 1][points.Count - 1], points[points.Count - 1][0]));
            curves.Append(Line.CreateBound(points[points.Count - 1][0], points[0][0]));

            ElementId floorTypeId = Floor.GetDefaultFloorType(doc, true);

            double elevation = 0;
            ElementId levelId = Level.GetNearestLevelId(doc, elevation);

            Floor.Create(doc, curveLooplist, floorTypeId, levelId);
        }                    
    }
}
