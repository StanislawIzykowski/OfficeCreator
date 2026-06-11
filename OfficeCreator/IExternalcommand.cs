using Autodesk.Revit.UI;

namespace OfficeCreator
{
    public interface IExternalcommand
    {
        Result OnStartup(UIControlledApplication application);
    }
}