using Autodesk.Revit.DB.Mechanical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OfficeCreator.Commands
{
    public class UnitChanger(float num)
    {
        public float ImpToMet()
        {
            float metric = num / 3.281f;

            return metric;
        }
        
        public float MetToImp()
        { 
            float imp = num * 3.281f;

            return imp;
        }
    }
}
