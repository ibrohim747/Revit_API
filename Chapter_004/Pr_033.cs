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
    internal class Pr_033 : IExternalCommand
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

                LocationCurve wallLine = elem.Location as LocationCurve;

                List<XYZ> points = new List<XYZ>();

                if (wallLine != null)
                {
                    Curve curve = wallLine.Curve;
                    points.Add(curve.GetEndPoint(0)); // Начало линии
                    points.Add(curve.GetEndPoint(1)); // Конец линии
                }

                // Преобразуем список точек в текст
                string msg = string.Join(Environment.NewLine, points.Select(p => $"X:{p.X:F2}, Y:{p.Y:F2}, Z:{p.Z:F2}"));

                Double length = wallLine.Curve.Length;

                TaskDialog dialog = new TaskDialog("Pr_032");
                dialog.MainInstruction = "Точки кривой:";
                dialog.MainContent = length.ToString();
                dialog.CommonButtons = TaskDialogCommonButtons.Ok | TaskDialogCommonButtons.Cancel;
                dialog.DefaultButton = TaskDialogResult.Ok;

                TaskDialogResult result = dialog.Show();

                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
