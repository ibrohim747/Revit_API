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
    internal class Pr_024 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;


            using (Transaction tx = new Transaction(doc, "Create Grid"))
            {
                List<string> list = new List<string>() {"A", "B", "C", "D", "E"};

                double HeightInFeet = 3000;

                double HeightFeet = UnitUtils.ConvertToInternalUnits(HeightInFeet, DisplayUnitType.DUT_MILLIMETERS);

                tx.Start();

                for (int i = 0; i < list.Count; i++)
                {
                    XYZ start = new XYZ(HeightFeet, -10, 0);
                    XYZ end = new XYZ(HeightFeet, 30, 0);
                    Line geomLine = Line.CreateBound(start, end);

                    Grid lineGrid = Grid.Create(doc, geomLine);

                    if (null == lineGrid)
                    {
                        throw new Exception("Create a new straight grid failed.");
                    }

                    double newHeightInFeet = 6000;

                    double newHeightFeet = UnitUtils.ConvertToInternalUnits(newHeightInFeet, DisplayUnitType.DUT_MILLIMETERS);

                    lineGrid.Name = list[i];
                    HeightFeet = HeightFeet + newHeightFeet;
                }

                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
