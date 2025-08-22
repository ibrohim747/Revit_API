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
    internal class Pr_019 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;


            using (Transaction tx = new Transaction(doc, "Move"))
            {
                tx.Start();

                Reference pickedObj = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);
                String eleId = pickedObj.ElementId.ToString();

                if (pickedObj == null)
                {
                    TaskDialog.Show("Ошибка", "No object selected.");
                    return Result.Cancelled;
                }
                XYZ newPlace = new XYZ(10, 20, 0);

                ElementTransformUtils.MoveElement(doc, pickedObj.ElementId, newPlace);

                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
