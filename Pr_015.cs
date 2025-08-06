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
    internal class Pr_015 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;


            using (Transaction tx = new Transaction(doc, "Create Level"))
            {
                tx.Start();

                double n = 3;  //inches
                Level newLevel = Level.Create(doc, n);

                if (newLevel != null)
                {
                    // Set the name of the new level
                    newLevel.Name = "Created Level";

                    TaskDialog.Show("Level Creation", "New Level 'Created Level' created at elevation " + n + ".");
                }
                else
                {
                    TaskDialog.Show("Level Creation", "Failed to create new level.");
                }
                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
