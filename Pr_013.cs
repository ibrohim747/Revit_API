using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Projects
{
    [TransactionAttribute(TransactionMode.Manual)]
    internal class Pr_013 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;


            using (Transaction tx = new Transaction(doc, "Create Rectangular Walls"))
            {
                tx.Start();

                // Размеры прямоугольника (в футах)
                double width = 30;  // по X
                double height = 20; // по Y

                // Точки прямоугольника по часовой стрелке
                XYZ pt1 = new XYZ(0, 0, 0);
                XYZ pt2 = new XYZ(width, 0, 0);
                XYZ pt3 = new XYZ(width, height, 0);
                XYZ pt4 = new XYZ(0, height, 0);

                // Уровень
                Level level = new FilteredElementCollector(doc)
                    .OfClass(typeof(Level))
                    .Cast<Level>()
                    .FirstOrDefault(l => l.Name == "Уровень 1");

                if (level == null)
                {
                    TaskDialog.Show("Ошибка", "Уровень 'Уровень 1' не найден.");
                    return Result.Failed;
                }

                // Создаём стены по линиям
                List<Line> lines = new List<Line>
                {
                    Line.CreateBound(pt1, pt2),
                    Line.CreateBound(pt2, pt3),
                    Line.CreateBound(pt3, pt4),
                    Line.CreateBound(pt4, pt1)
                };

                foreach (Line line in lines)
                {
                    Wall.Create(doc, line, level.Id, false);
                }

                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
