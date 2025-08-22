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
    internal class Pr_014 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;


            using (Transaction tx = new Transaction(doc, "Create Rectangular Walls"))
            {
                tx.Start();

                Reference wallRef = uidoc.Selection.PickObject(ObjectType.Element, "Select a Wall to place the door.");
                Wall hostWall = doc.GetElement(wallRef) as Wall;

                if (hostWall == null)
                {
                    TaskDialog.Show("Ошибка", "No wall selected.");
                    return Result.Cancelled;
                }

                // 3. Find the desired door family symbol (type)
                FamilySymbol doorSymbol = null;
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfCategory(BuiltInCategory.OST_Doors).OfClass(typeof(FamilySymbol));

                foreach (FamilySymbol fs in collector)
                {
                    if (fs.Name == "0915 x 2134 мм") // Replace with your desired door type name
                    {
                        doorSymbol = fs;
                        break;
                    }
                }

                if (doorSymbol == null)
                {
                    TaskDialog.Show("Ошибка", "Desired door type not found. Please load the family.");
                    return Result.Cancelled;
                }

                // 4. Define placement location and orientation
                LocationCurve wallCurve = hostWall.Location as LocationCurve;
                if (wallCurve == null)
                {
                    TaskDialog.Show("Ошибка", "Не удалось получить кривую стены.");
                    return Result.Failed;
                }

                XYZ location = (wallCurve.Curve.GetEndPoint(0) + wallCurve.Curve.GetEndPoint(1)) / 2;


                // 5. Create the family instance
                FamilyInstance newDoor = doc.Create.NewFamilyInstance(location, doorSymbol, hostWall, StructuralType.NonStructural);



                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
