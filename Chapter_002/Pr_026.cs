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
    internal class Pr_026 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;


            using (Transaction tx = new Transaction(doc, "Create Grid"))
            {
                tx.Start();

                IList<Reference> pickedElem = uidoc.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element);

                List<ElementId> ids = new List<ElementId>();

                foreach (Reference r in pickedElem)
                {
                    ids.Add(r.ElementId);
                }

                Group newgroup = doc.Create.NewGroup(ids.ToArray());

                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
