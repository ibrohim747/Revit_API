using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projects
{
    [TransactionAttribute(TransactionMode.ReadOnly)]
    internal class Pr_011 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            String Project = "Pr_011";
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            Reference pickedObj = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);
            String eleId = pickedObj.ElementId.ToString();

            TaskDialog dialog = new TaskDialog(Project);
            dialog.MainInstruction = "Element ID = " + eleId;
            dialog.CommonButtons = TaskDialogCommonButtons.Ok | TaskDialogCommonButtons.Cancel;
            dialog.DefaultButton = TaskDialogResult.Ok;
            TaskDialogResult result = dialog.Show();

            if (result == TaskDialogResult.Ok)
            {
                TaskDialog dialog2 = new TaskDialog(Project);
                dialog2.MainInstruction = "You pushed OK button";
                TaskDialogResult result2 = dialog2.Show();
            }

            return Result.Succeeded;
        }
    }
}
