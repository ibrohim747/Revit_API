using Autodesk.Revit.Attributes;
using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Xml;
using System.Xml.Linq;
using Document = Autodesk.Revit.DB.Document;

namespace Projects
{
    [TransactionAttribute(TransactionMode.Manual)]
    internal class Pr_028 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Выбираем элементы (например, стены)
            Autodesk.Revit.DB.Reference ref1 = uidoc.Selection.PickObject(ObjectType.Element, "Выберите первый элемент");
            Autodesk.Revit.DB.Reference ref2 = uidoc.Selection.PickObject(ObjectType.Element, "Выберите второй элемент");

            // Получаем активный вид (должен быть план или разрез, НЕ 3D)
            View view = doc.ActiveView;

            // Создаём линию размера (ось, на которой будет размер)
            XYZ p1 = new XYZ(0, 0, 0);
            XYZ p2 = new XYZ(5, 0, 0);
            Line dimLine = Line.CreateBound(p1, p2);

            // Собираем References для размера
            ReferenceArray refArray = new ReferenceArray();
            refArray.Append(ref1);
            refArray.Append(ref2);

            using (Transaction tx = new Transaction(doc, "Create Dimension"))
            {
                tx.Start();

                // Создание размера
                Dimension dim = doc.Create.NewDimension(view, dimLine, refArray);

                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
