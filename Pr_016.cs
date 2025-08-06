using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
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
    internal class Pr_016 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;


            using (Transaction tx = new Transaction(doc, "Create Level"))
            {
                tx.Start();

                double wallThickness = UnitUtils.ConvertToInternalUnits(200, DisplayUnitType.DUT_MILLIMETERS); // 200 мм
                double offset = wallThickness / 2;

                // Размеры прямоугольника (в футах)
                double width = 30;  // по X
                double height = 20; // по Y

                // Точки прямоугольника по часовой стрелке
                XYZ pt1 = new XYZ(0, 0, 0);
                XYZ pt2 = new XYZ(width, 0, 0);
                XYZ pt3 = new XYZ(width, height, 0);
                XYZ pt4 = new XYZ(0, height, 0);

                // Смещение прямоугольника внутрь по X и Y
                XYZ newPt1 = new XYZ(pt1.X + offset, pt1.Y + offset, 0);
                XYZ newPt2 = new XYZ(pt2.X - offset, pt2.Y + offset, 0);
                XYZ newPt3 = new XYZ(pt3.X - offset, pt3.Y - offset, 0);
                XYZ newPt4 = new XYZ(pt4.X + offset, pt4.Y - offset, 0);

                // Построение контура перекрытия
                CurveArray profile = new CurveArray();
                profile.Append(Line.CreateBound(newPt1, newPt2));
                profile.Append(Line.CreateBound(newPt2, newPt3));
                profile.Append(Line.CreateBound(newPt3, newPt4));
                profile.Append(Line.CreateBound(newPt4, newPt1));

                // Получаем тип перекрытия по умолчанию
                FloorType floorType = new FilteredElementCollector(doc)
                    .OfClass(typeof(FloorType))
                    .Cast<FloorType>()
                    .FirstOrDefault();

                // Уровень
                Level level = new FilteredElementCollector(doc)
                    .OfClass(typeof(Level))
                    .Cast<Level>()
                    .FirstOrDefault(l => l.Name == "Уровень 1");

                Floor floor = null;

                if (floorType != null && level != null)
                {
                    floor = doc.Create.NewFloor(profile, floorType, level, false);
                }

                double offset2 = UnitUtils.ConvertToInternalUnits(150, DisplayUnitType.DUT_MILLIMETERS); // 150 мм

                // После создания пола:
                floor.get_Parameter(BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM).Set(offset2);

                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
