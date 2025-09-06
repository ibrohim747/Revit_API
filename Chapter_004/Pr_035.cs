using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Projects
{
    [TransactionAttribute(TransactionMode.Manual)]
    internal class Pr_035 : IExternalCommand
    {

        public static double CalculateRoomArea(List<XYZ> wallPoints)
        {
            if (wallPoints == null || wallPoints.Count < 6) // Минимум 3 стены (6 точек)
                return 0;

            // Удаляем дубликаты точек с учетом погрешности
            var uniquePoints = RemoveDuplicatePoints(wallPoints, 0.001);

            // Строим упорядоченный полигон из точек стен
            var polygonVertices = BuildPolygonFromWalls(uniquePoints);

            if (polygonVertices.Count < 3)
                return 0;

            // Вычисляем площадь методом Shoelace (формула трапеций)
            return CalculatePolygonArea(polygonVertices);
        }

        private static List<XYZ> RemoveDuplicatePoints(List<XYZ> points, double tolerance)
        {
            var unique = new List<XYZ>();

            foreach (var point in points)
            {
                bool isDuplicate = false;
                foreach (var existing in unique)
                {
                    if (Math.Abs(point.X - existing.X) < tolerance &&
                        Math.Abs(point.Y - existing.Y) < tolerance)
                    {
                        isDuplicate = true;
                        break;
                    }
                }

                if (!isDuplicate)
                {
                    unique.Add(point);
                }
            }

            return unique;
        }

        private static List<XYZ> BuildPolygonFromWalls(List<XYZ> points)
        {
            if (points.Count < 3)
                return new List<XYZ>();

            var polygon = new List<XYZ>();
            var usedPoints = new HashSet<int>();

            // Начинаем с первой точки
            polygon.Add(points[0]);
            usedPoints.Add(0);

            var currentPoint = points[0];

            // Строим полигон, последовательно находя ближайшие точки
            while (usedPoints.Count < points.Count)
            {
                int nearestIndex = -1;
                double minDistance = double.MaxValue;

                for (int i = 0; i < points.Count; i++)
                {
                    if (usedPoints.Contains(i))
                        continue;

                    double distance = currentPoint.DistanceTo(points[i]);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestIndex = i;
                    }
                }

                if (nearestIndex == -1)
                    break;

                currentPoint = points[nearestIndex];
                polygon.Add(currentPoint);
                usedPoints.Add(nearestIndex);
            }

            return polygon;
        }

        private static double CalculatePolygonArea(List<XYZ> vertices)
        {
            if (vertices.Count < 3)
                return 0;

            double area = 0;
            int n = vertices.Count;

            // Формула Shoelace (Gauss area formula)
            for (int i = 0; i < n; i++)
            {
                int j = (i + 1) % n;
                area += vertices[i].X * vertices[j].Y;
                area -= vertices[j].X * vertices[i].Y;
            }

            return Math.Abs(area) / 2.0;
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            double GetPolygonArea(List<XYZ> polygon)
            {
                double area = 0;
                int n = polygon.Count;

                for (int i = 0; i < n; i++)
                {
                    XYZ p1 = polygon[i];
                    XYZ p2 = polygon[(i + 1) % n]; // Следующая точка (с замыканием)
                    area += (p1.X * p2.Y - p2.X * p1.Y);
                }

                return Math.Abs(area) / 2.0;
            }

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            using (Transaction tx = new Transaction(doc, "Location Curve"))
            {
                tx.Start();

                List<XYZ> points = new List<XYZ>();

                // Выбираем элементы (стены)
                IList<Reference> wallReferences = uidoc.Selection.PickObjects(
                    ObjectType.Element,
                    "Выберите стены комнаты");

                foreach (Reference reference in wallReferences)
                {
                    Element element = doc.GetElement(reference.ElementId);

                    // Проверяем, что это стена
                    if (element is Wall wall)
                    {
                        LocationCurve wallLine = wall.Location as LocationCurve;
                        if (wallLine != null)
                        {
                            Curve curve = wallLine.Curve;
                            points.Add(curve.GetEndPoint(0)); // Начало стены
                            points.Add(curve.GetEndPoint(1)); // Конец стены
                        }
                    }
                }

                if (points.Count < 6) // Минимум 3 стены
                {
                    TaskDialog.Show("Ошибка", "Выберите минимум 3 стены для расчета площади");
                }

                // Вычисляем площадь
                double areaSquareFeet = CalculateRoomArea(points);

                // Конвертируем в квадратные метры (1 кв.фут = 0.092903 кв.м)
                double areaSquareMeters = areaSquareFeet * 0.092903;

                string msg = "hello";
                // Выводим результат
                msg = $"Площадь комнаты:\n" +
                                $"• {areaSquareFeet:F2} кв. футов\n" +
                                $"• {areaSquareMeters:F2} кв. метров\n" +
                                $"Обработано точек: {points.Count}";

                TaskDialog dialog = new TaskDialog("Pr_032");
                dialog.MainInstruction = "Точки кривой:";
                dialog.MainContent = msg;
                dialog.CommonButtons = TaskDialogCommonButtons.Ok | TaskDialogCommonButtons.Cancel;
                dialog.DefaultButton = TaskDialogResult.Ok;

                TaskDialogResult result = dialog.Show();

                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
