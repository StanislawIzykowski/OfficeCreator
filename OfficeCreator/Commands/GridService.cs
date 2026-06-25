using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using OfficeCreator.ViewModel;

namespace OfficeCreator.Commands
{
    public class GridService
    {
        public void Create(Document doc, IList<IList<XYZ>> points)
        {   
            //how many columns and rows
            int rowCount = points.Count;
            int colCount = points[0].Count;


            //creating columns on point list X axis
            //conntecting first with last point

            for (int i = 0; i < rowCount; i++)
            {
                XYZ startPoint = points[i][0];
                XYZ endPoint = points[i][colCount - 1];

                Line gridLineX = Line.CreateBound(startPoint, endPoint);
                Grid.Create(doc, gridLineX);
            }

            //creating columns on point list Y axis
            //conntecting first with last point
            for (int j = 0; j < colCount; j++)
            {
                XYZ startPoint = points[0][j];
                XYZ endPoint = points[colCount - 1][j];

                Line gridLineY = Line.CreateBound(startPoint, endPoint);
                Grid.Create(doc, gridLineY);
            }
        }
    }
}

