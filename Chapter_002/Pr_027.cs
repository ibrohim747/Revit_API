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
using System.Xml.Linq;
using Document = Autodesk.Revit.DB.Document;

namespace Projects
{
    [TransactionAttribute(TransactionMode.Manual)]
    internal class Pr_027 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;


            using (Transaction tx = new Transaction(doc, "Create Viev plan"))
            {
                tx.Start();

                ViewFamilyType floorPlanViewFamilyType = new FilteredElementCollector(doc).OfClass(typeof(ViewFamilyType)).Cast<ViewFamilyType>().FirstOrDefault(vft => vft.ViewFamily == ViewFamily.FloorPlan);

                Reference pickedRef = uidoc.Selection.PickObject(ObjectType.Element, "Select a Level");
                Element elem = doc.GetElement(pickedRef);
                Level level = elem as Level;

                if (level == null)
                {
                    TaskDialog.Show("Ошибка", "Выбранный элемент не является уровнем.");
                    return Result.Cancelled;
                }

                ViewPlan newFloorPlan = ViewPlan.Create(doc, floorPlanViewFamilyType.Id, level.Id);

                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
