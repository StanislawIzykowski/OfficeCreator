using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System.Collections.Generic;

namespace OfficeCreator.Commands
{
    public class WindowService
    {
        public void Create(Document doc, IList<IList<XYZ>> points, double moduleX, double moduleY, ElementId windowId, IList<Wall> WallsList)
        {
            FamilySymbol windowSymbol = doc.GetElement(windowId) as FamilySymbol;

            if (!windowSymbol.IsActive) windowSymbol.Activate();

            double sillHeightLow = UnitUtils.ConvertToInternalUnits(1100, UnitTypeId.Millimeters); // 1.10 m
            double sillHeightHigh = UnitUtils.ConvertToInternalUnits(5100, UnitTypeId.Millimeters); // 5.10 m

            const int windowsPerRow = 4; 

            foreach (Wall wall in WallsList)
            {
                LocationCurve locCurve = wall.Location as LocationCurve;
                if (locCurve == null) continue;

                Line wallLine = locCurve.Curve as Line;

                XYZ start = wallLine.GetEndPoint(0);
                XYZ end = wallLine.GetEndPoint(1);
                XYZ direction = (end - start).Normalize();
                double wallLength = wallLine.Length;

                ElementId levelId = wall.LevelId;
                Level level = doc.GetElement(levelId) as Level;
                double baseElevation = level != null ? level.Elevation : 0;

                double segmentLength = wallLength / windowsPerRow;

                for (int i = 0; i < windowsPerRow; i++)
                {
                    double distanceAlongWall = segmentLength * (i + 0.5);
                    XYZ basePoint = start + direction * distanceAlongWall;

                    
                    XYZ lowPoint = new XYZ(basePoint.X, basePoint.Y, baseElevation + sillHeightLow);
                    FamilyInstance lowWindow = doc.Create.NewFamilyInstance(
                        lowPoint, windowSymbol, wall, level, StructuralType.NonStructural);

                    Parameter lowSillParam = lowWindow.get_Parameter(BuiltInParameter.INSTANCE_SILL_HEIGHT_PARAM);
                    lowSillParam?.Set(sillHeightLow);

                    
                    XYZ highPoint = new XYZ(basePoint.X, basePoint.Y, baseElevation + sillHeightHigh);
                    FamilyInstance highWindow = doc.Create.NewFamilyInstance(
                        highPoint, windowSymbol, wall, level, StructuralType.NonStructural);

                    Parameter highSillParam = highWindow.get_Parameter(BuiltInParameter.INSTANCE_SILL_HEIGHT_PARAM);
                    highSillParam?.Set(sillHeightHigh);
                }
            }
        }
    }
}