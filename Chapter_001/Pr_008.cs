using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projects
{
    [TransactionAttribute(TransactionMode.ReadOnly)]
    internal class Pr_008 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            String Project = "Pr_008";
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            TaskDialog dialog = new TaskDialog(Project);
            dialog.MainInstruction = "Это пример диалога";
            dialog.MainContent = "Выберите двери или окна";
            dialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, "Двери");
            dialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink2, "Окна");

            TaskDialogResult result = dialog.Show();

            if (result == TaskDialogResult.CommandLink1)
            {
                TaskDialog dialog2 = new TaskDialog(Project);

                FilteredElementCollector doorcollector = new FilteredElementCollector(doc);
                var doors = doorcollector.OfCategory(BuiltInCategory.OST_Doors).OfClass(typeof(FamilyInstance)).Cast<FamilyInstance>();
                int doorcount = doors.Count();

                dialog2.MainInstruction = "Вы выбрали 'Двери' = " + doorcount;
                dialog2.Show();
                return Result.Succeeded;
            }
            else if (result == TaskDialogResult.CommandLink2)
            {
                TaskDialog dialog2 = new TaskDialog(Project);

                FilteredElementCollector windowcollector = new FilteredElementCollector(doc);
                var windows = windowcollector.OfCategory(BuiltInCategory.OST_Windows).OfClass(typeof(FamilyInstance)).Cast<FamilyInstance>();
                int windowscount = windows.Count();

                dialog2.MainInstruction = "Вы выбрали 'Окна' = " + windowscount;
                dialog2.Show();
                return Result.Succeeded;
            }
            else { return Result.Failed; }
        }
    }
}
