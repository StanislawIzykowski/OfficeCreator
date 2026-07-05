using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OfficeCreator.Commands
{
    public class StairsService
    {
        public void Create(Document doc, IList<IList<XYZ>> points, ElementId newStairsId, double moduleX, double moduleY)
        {
            StairsRunType runType = new FilteredElementCollector(doc)
                .OfClass(typeof(StairsRunType))
                .Cast<StairsRunType>()
                .FirstOrDefault();
            if (runType == null) return;

            // --- Parametry geometryczne ---
            double runWidth = UnitUtils.ConvertToInternalUnits(1200, UnitTypeId.Millimeters); // szerokość biegu
            double treadDepth = UnitUtils.ConvertToInternalUnits(300, UnitTypeId.Millimeters);  // przybliżona głębokość stopnia
            double riserHeight = UnitUtils.ConvertToInternalUnits(175, UnitTypeId.Millimeters); // przybliżona wysokość stopnia
            double totalHeight = UnitUtils.ConvertToInternalUnits(4000, UnitTypeId.Millimeters); // 0 -> 4m

            int totalRisers = (int)Math.Ceiling(totalHeight / riserHeight);
            int risersPerRun = totalRisers / 2;              // dzielimy na 2 biegi
            double runLength = risersPerRun * treadDepth;    // długość jednego biegu
            double midHeight = risersPerRun * riserHeight;   // wysokość po pierwszym biegu (poziom spocznika)

            XYZ basePoint = points[0][0];

            // --- Bieg 1: w górę wzdłuż Y, start na poziomie 0 ---
            XYZ run1Start = new XYZ(basePoint.X, basePoint.Y, 0);
            XYZ run1End = new XYZ(basePoint.X, basePoint.Y + runLength, 0);
            Line run1Line = Line.CreateBound(run1Start, run1End);

            StairsRun run1 = StairsRun.CreateStraightRun(doc, newStairsId, run1Line, StairsRunJustification.Center);
            run1.ChangeTypeId(runType.Id);

            // --- Bieg 2: równoległy, przesunięty o szerokość biegu, w przeciwnym kierunku ---
            // Z (wysokość bazowa) = wysokość, na której kończy się bieg 1
            XYZ run2Start = new XYZ(basePoint.X + runWidth, basePoint.Y + runLength, midHeight);
            XYZ run2End = new XYZ(basePoint.X + runWidth, basePoint.Y, midHeight);
            Line run2Line = Line.CreateBound(run2Start, run2End);

            StairsRun run2 = StairsRun.CreateStraightRun(doc, newStairsId, run2Line, StairsRunJustification.Center);
            run2.ChangeTypeId(runType.Id);

            // --- Automatyczny spocznik między biegami (tworzy kształt U) ---
            if (StairsLanding.CanCreateAutomaticLanding(doc, run1.Id, run2.Id))
            {
                StairsLanding.CreateAutomaticLanding(doc, run1.Id, run2.Id);
            }
        }
    }
}