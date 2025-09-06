using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;

namespace Projects
{
    [TransactionAttribute(TransactionMode.Manual)]
    internal class Pr_036 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            View view = doc.ActiveView;

            using (Transaction tx = new Transaction(doc, "Distance"))
            {
                tx.Start();

                XYZ p1 = new XYZ(0,0,0); // Начало линии
                XYZ p2 = new XYZ(10, 10, 0); // Конец линии
                XYZ p3 = new XYZ(0, 5, 0); // Конец линии

                Arc arc = Arc.Create(p1, p2, p3);
                DetailCurve detailArc = doc.Create.NewDetailCurve(view, arc);


                //Wall newWall = Wall.Create(doc, newArc, false);

                TaskDialog dialog = new TaskDialog("Pr_034");
                dialog.MainInstruction = "Distance";
                dialog.MainContent = "Bob";
                dialog.CommonButtons = TaskDialogCommonButtons.Ok | TaskDialogCommonButtons.Cancel;
                dialog.DefaultButton = TaskDialogResult.Ok;

                TaskDialogResult result = dialog.Show();

                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
