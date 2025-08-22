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
    internal class Pr_025 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;


            using (Transaction tx = new Transaction(doc, "Create Grid"))
            {
                tx.Start();

                // Находим тип вида для 3D
                ViewFamilyType viewFamilyType3D = new FilteredElementCollector(doc)
                    .OfClass(typeof(ViewFamilyType))
                    .Cast<ViewFamilyType>()
                    .FirstOrDefault(x => x.ViewFamily == ViewFamily.ThreeDimensional);

                if (viewFamilyType3D != null)
                {
                    View3D Bob = View3D.CreateIsometric(doc, viewFamilyType3D.Id);
                }

                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
