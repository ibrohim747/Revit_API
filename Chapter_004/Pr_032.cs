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
    internal class Pr_032 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            using (Transaction tx = new Transaction(doc, "Change Parameter"))
            {
                tx.Start();

                // Выбираем элементы (например, стены)
                Reference ref1 = uidoc.Selection.PickObject(ObjectType.Element, "Выберите элемент");
                Element elem = doc.GetElement(ref1.ElementId);

                LocationPoint locationPoint= elem.Location as LocationPoint;

                XYZ point = locationPoint.Point;

                TaskDialog dialog = new TaskDialog("Pr_032");
                dialog.MainInstruction = point.ToString();
                dialog.CommonButtons = TaskDialogCommonButtons.Ok | TaskDialogCommonButtons.Cancel;
                dialog.DefaultButton = TaskDialogResult.Ok;

                TaskDialogResult result = dialog.Show();

                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
