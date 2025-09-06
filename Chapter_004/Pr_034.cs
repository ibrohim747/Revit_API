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
    internal class Pr_034 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            using (Transaction tx = new Transaction(doc, "Distance"))
            {
                tx.Start();

                // Выбираем элементы (например, стены)
                Reference ref1 = uidoc.Selection.PickObject(ObjectType.Element, "Выберите элемент");
                Element elem = doc.GetElement(ref1.ElementId);

                LocationCurve wallLine = elem.Location as LocationCurve;

                double distance = new double();

                if (wallLine != null)
                {
                    Curve curve = wallLine.Curve;
                    XYZ p1 = curve.GetEndPoint(0); // Начало линии
                    XYZ p2 = curve.GetEndPoint(1); // Конец линии
                    distance = p1.DistanceTo(p2); // в Revit API уже есть готовый метод
                }

                Double length = wallLine.Curve.Length;    //Длина стены

                TaskDialog dialog = new TaskDialog("Pr_034");
                dialog.MainInstruction = "Distance";
                dialog.MainContent = distance.ToString();
                dialog.CommonButtons = TaskDialogCommonButtons.Ok | TaskDialogCommonButtons.Cancel;
                dialog.DefaultButton = TaskDialogResult.Ok;

                TaskDialogResult result = dialog.Show();

                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
