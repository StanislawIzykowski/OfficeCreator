using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace OfficeCreator.Commands
{
    public class ViewService
    {
        public void Create(Autodesk.Revit.DB.Document doc)
        {
            ViewFamilyType viewFamilyType = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewFamilyType))
                .Cast<ViewFamilyType>()
                .FirstOrDefault(x => x.ViewFamily == ViewFamily.FloorPlan);

            ViewFamilyType elevationType = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewFamilyType))
                .Cast<ViewFamilyType>()
                .FirstOrDefault(x => x.ViewFamily == ViewFamily.Elevation);

            ViewFamilyType sheetType = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewFamilyType))
                .Cast<ViewFamilyType>()
                .FirstOrDefault(x => x.ViewFamily == ViewFamily.Sheet);

            double elev0 = 0.0;
            double elev4 = 4.0 * 3.28084;
            double elev8 = 8.0 * 3.28084;

            ElementId levelId0 = Level.GetNearestLevelId(doc, elev0);
            ElementId levelId4 = Level.GetNearestLevelId(doc, elev4);
            ElementId levelId8 = Level.GetNearestLevelId(doc, elev8);

            //creating view plan to place it on a sheet
            ViewPlan groundfloorViewPlan = ViewPlan.Create(doc, viewFamilyType.Id, levelId0);
            groundfloorViewPlan.Name = "Ground Floor Plan";

            ViewPlan firstFloorViewPlan = ViewPlan.Create(doc, viewFamilyType.Id, levelId4);
            firstFloorViewPlan.Name = "First Floor Plan";

            ViewPlan roofViewPlan = ViewPlan.Create(doc, viewFamilyType.Id, levelId8);
            roofViewPlan.Name = "Roof Plan";

            ViewSheet groundFloorViewSheet = ViewSheet.Create(doc, sheetType.Id);
            groundFloorViewSheet.Name = "Ground Floor Plan";
            groundFloorViewSheet.SheetNumber = "A101";

            ViewSheet firstFloorViewSheet = ViewSheet.Create(doc, sheetType.Id);
            firstFloorViewSheet.Name = "First Floor Plan";
            firstFloorViewSheet.SheetNumber = "A201";

            ViewSheet roofViewSheet = ViewSheet.Create(doc, sheetType.Id);
            roofViewSheet.Name = "Roof Plan";
            roofViewSheet.SheetNumber = "A301";


            Viewport.Create(doc, groundFloorViewSheet.Id, groundfloorViewPlan.Id, new XYZ(0.5, -0.5, 0));
            Viewport.Create(doc, firstFloorViewSheet.Id, firstFloorViewPlan.Id, new XYZ(0.5, -0.5, 0));
            Viewport.Create(doc, roofViewSheet.Id, roofViewPlan.Id, new XYZ(0.5, -0.5, 0));

        }
    }
}
