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
    internal class Pr_029 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            using (Transaction tx = new Transaction(doc, "Get parameter"))
            {
                tx.Start();

                // Выбираем элементы (например, стены)
                Reference ref1 = uidoc.Selection.PickObject(ObjectType.Element, "Выберите элемент");
                Element elem = doc.GetElement(ref1.ElementId);

                Parameter baseConstraintParam = elem.get_Parameter(BuiltInParameter.WALL_BASE_CONSTRAINT);

                TaskDialog dialog = new TaskDialog("Pr_001");
                dialog.MainInstruction = baseConstraintParam.AsDouble().ToString();

                TaskDialogResult result = dialog.Show();

                return Result.Succeeded;

                tx.Commit();
            }
        }
    }
}
