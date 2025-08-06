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
    internal class Pr_018 : IExternalCommand
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

                Parameter heightParam = hostWall.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM);

                double newHeightInFeet = 3000;

                double newHeightFeet = UnitUtils.ConvertToInternalUnits(newHeightInFeet, DisplayUnitType.DUT_MILLIMETERS);

                heightParam.Set(newHeightFeet);



                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
