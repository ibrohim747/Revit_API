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
using System.Xml;
using Document = Autodesk.Revit.DB.Document;

namespace Projects
{
    [TransactionAttribute(TransactionMode.Manual)]
    internal class Pr_023 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;


            using (Transaction tx = new Transaction(doc, "Rotate"))
            {
                tx.Start();

                Reference pickedObj = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);
                ElementId eleId = pickedObj.ElementId;

                if (pickedObj == null)
                {
                    TaskDialog.Show("Ошибка", "No object selected.");
                    return Result.Cancelled;
                }

                XYZ p1 = new XYZ(0, 0, 0);
                XYZ p2 = new XYZ(10, 10, 10);
                Line axis = Line.CreateBound(p1, p2);

                double angleInRadians = (Math.PI / 180) * 90;

                ElementTransformUtils.RotateElement(doc, eleId, axis, angleInRadians);

                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
