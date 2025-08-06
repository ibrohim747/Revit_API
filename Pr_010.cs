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
    internal class Pr_010 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            String Project = "Pr_010";
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            Units units = doc.GetUnits();
            FormatOptions lengthFormat = units.GetFormatOptions(SpecTypeId.Length);
            ForgeTypeId unitType = lengthFormat.GetUnitTypeId();
            string unitName = unitType.TypeId;

            TaskDialog dialog = new TaskDialog(Project);
            dialog.MainInstruction = "Units = " + unitName;
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
