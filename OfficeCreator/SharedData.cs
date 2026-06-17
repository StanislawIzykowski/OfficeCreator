using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeCreator
{
    public static  class SharedData
    {
        public static XYZ IntersectionPoint {  get; set; }
        public static float ModuleX { get; set; } = 6;
        public static float ModuleY { get; set; } = 5.5f;
        //public static float ModuleZ { get; set; } 
        public static int Columns { get; set; } = 10;
        public static int Rows { get; set; } = 10;
        public static int Floors { get; set; } = 2; // GF, 1F
    }
}
