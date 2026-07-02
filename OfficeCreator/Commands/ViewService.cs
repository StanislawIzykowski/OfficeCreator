using Autodesk.Revit.DB;
using System.Linq;

namespace OfficeCreator.Commands
{
    public class ViewService
    {
        public void Create(Document doc)
        {
            ViewFamilyType viewFamilyType = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewFamilyType))
                .Cast<ViewFamilyType>()
                .FirstOrDefault(x => x.ViewFamily == ViewFamily.FloorPlan);

            ViewFamilyType elevationType = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewFamilyType))
                .Cast<ViewFamilyType>()
                .FirstOrDefault(x => x.ViewFamily == ViewFamily.Elevation);

            
            FamilySymbol titleBlock = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .OfCategory(BuiltInCategory.OST_TitleBlocks)
                .Cast<FamilySymbol>()
                .FirstOrDefault();

            if (titleBlock != null && !titleBlock.IsActive)
                titleBlock.Activate();

            ElementId titleBlockId = titleBlock?.Id ?? ElementId.InvalidElementId;

            double elev0 = 0.0;
            double elev4 = UnitUtils.ConvertToInternalUnits(4.0, UnitTypeId.Meters);
            double elev8 = UnitUtils.ConvertToInternalUnits(8.0, UnitTypeId.Meters);

            ElementId levelId0 = Level.GetNearestLevelId(doc, elev0);
            ElementId levelId4 = Level.GetNearestLevelId(doc, elev4);
            ElementId levelId8 = Level.GetNearestLevelId(doc, elev8);

            // creating views
            ViewPlan groundFloorViewPlan = ViewPlan.Create(doc, viewFamilyType.Id, levelId0);
            groundFloorViewPlan.Name = "Ground Floor Plan";

            ViewPlan firstFloorViewPlan = ViewPlan.Create(doc, viewFamilyType.Id, levelId4);
            firstFloorViewPlan.Name = "First Floor Plan";

            ViewPlan roofViewPlan = ViewPlan.Create(doc, viewFamilyType.Id, levelId8);
            roofViewPlan.Name = "Roof Plan";

            // creating sheets
            ViewSheet groundFloorViewSheet = ViewSheet.Create(doc, titleBlockId);
            groundFloorViewSheet.Name = "Ground Floor Plan";
            groundFloorViewSheet.SheetNumber = "A101";

            ViewSheet firstFloorViewSheet = ViewSheet.Create(doc, titleBlockId);
            firstFloorViewSheet.Name = "First Floor Plan";
            firstFloorViewSheet.SheetNumber = "A201";

            ViewSheet roofViewSheet = ViewSheet.Create(doc, titleBlockId);
            roofViewSheet.Name = "Roof Plan";
            roofViewSheet.SheetNumber = "A301";

            // placing views on sheets
            Viewport.Create(doc, groundFloorViewSheet.Id, groundFloorViewPlan.Id, new XYZ(1, 1, 0));
            Viewport.Create(doc, firstFloorViewSheet.Id, firstFloorViewPlan.Id, new XYZ(1, 1, 0));
            Viewport.Create(doc, roofViewSheet.Id, roofViewPlan.Id, new XYZ(1, 1, 0));
        }
    }
}