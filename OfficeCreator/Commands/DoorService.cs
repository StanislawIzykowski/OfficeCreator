using Autodesk.Revit.DB;

namespace OfficeCreator.Commands
{
    public class DoorService
    {
        public void Create(Document doc, IList<IList<XYZ>> points )
        {

            int rowCount = points.Count;
            int colCount = points[0].Count;


            //doc.Create.NewFamilyInstance();
        }
    }
}
