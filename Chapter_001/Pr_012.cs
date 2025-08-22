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
    internal class Pr_012 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            String Project = "Pr_012";
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Get selected elements from current document.
            ICollection<ElementId> selectedIds = uidoc.Selection.GetElementIds();

            // Go through the selected items and filter out walls only.
            ICollection<ElementId> selectedWallIds = new List<ElementId>();

            foreach (ElementId id in selectedIds)
            {
                Element elements1 = uidoc.Document.GetElement(id);
                if (elements1 is Wall)
                {
                    selectedWallIds.Add(id);
                }
            }

            // Give the user some information.
            if (0 != selectedWallIds.Count)
            {
                TaskDialog.Show("Revit", selectedWallIds.Count.ToString() + " Walls are selected!");
            }
            else
            {
                TaskDialog.Show("Revit", "No Walls have been selected!");
            }

            TaskDialog dialog = new TaskDialog(Project);
            dialog.MainInstruction = "Number of selected elements: " + selectedIds.Count.ToString();
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
