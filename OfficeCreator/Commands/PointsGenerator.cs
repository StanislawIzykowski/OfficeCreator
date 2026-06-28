using Autodesk.Revit.DB;

namespace OfficeCreator.Commands
{
    public class PointsGenerator
    {
        public IList<IList<XYZ>> Create(double moduleX, double moduleY, double axisAmountX, double axisAmountY)
        {
            //creating list to return
            IList<IList<XYZ>> pointList = new List<IList<XYZ>>();

            //creating list in x axis
            for (int i = 0; i < axisAmountX; i++)
            {
                //creating list to nest
                IList<XYZ> row = new List<XYZ>();

                //creating y axis rows
                for (int j = 0; j < axisAmountY; j++)
                {
                    XYZ point = new XYZ(j * moduleX * 3.281f, i * moduleY * 3.281f, 0);

                    //adding points to row
                    row.Add(point);
                }

                //adding rows to list
                pointList.Add(row);
            }

            return pointList;
        }
    }
}
