using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projects
{
    [TransactionAttribute(TransactionMode.ReadOnly)]
    internal class Pr_003 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument; 
            Document doc = uidoc.Document;
            TaskDialog dialog = new TaskDialog("Pr_003");
            String projectInfo = doc.Title.ToString(); //Имя ревит проекта
            String Path = doc.PathName;  //Расположение ревит файла

            var versionNumber = commandData.Application.Application.VersionNumber;  //2021
            var versionName = commandData.Application.Application.VersionName;  //Autodesk Revit 2021
            var buildNumber = commandData.Application.Application.VersionBuild;  //21.0.0.383

            dialog.MainInstruction = versionNumber + ", " + versionName + ", " + buildNumber;
            dialog.CommonButtons = TaskDialogCommonButtons.Ok | TaskDialogCommonButtons.Cancel;
            dialog.DefaultButton = TaskDialogResult.Ok;
            TaskDialogResult result = dialog.Show();

            if (result == TaskDialogResult.Ok)
            {
                TaskDialog dialog2 = new TaskDialog("Pr_003_R");
                dialog2.MainInstruction = "Good Luck";
                TaskDialogResult result2 = dialog2.Show();
            }

            return Result.Succeeded;
        }
    }
}
