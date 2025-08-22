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
    internal class Pr_017 : IExternalCommand
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

                // Вычисляем точку внутри (центр)
                XYZ center = (newPt1 + newPt3) / 2;
                UV pointInRoom = new UV(center.X, center.Y);

                // Уровень
                Level level = new FilteredElementCollector(doc)
                    .OfClass(typeof(Level))
                    .Cast<Level>()
                    .FirstOrDefault(l => l.Name == "Уровень 1");

                // Создание комнаты

                Room newRoom = doc.Create.NewRoom(level, pointInRoom);

                string newRoomNumber = "101";
                string newRoomName = "Class Room 1";

                newRoom.Name = newRoomName;
                newRoom.Number = newRoomNumber;


                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
