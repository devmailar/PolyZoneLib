using CitizenFX.Core;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace PolyZoneLib.Client
{
    internal class PolyZone
    {
        private readonly List<Vector2> Points = new List<Vector2>();
        private readonly float CeilingZ = 10.73f;
        private readonly float FloorZ = 5.61f;

        internal PolyZone(Vector2[] points)
        {
            Points.AddRange(points);
        }

        internal async Task Draw()
        {
            if (Points.Count == 0)
            {
                return;
            }

            var vertices = new List<Vector3>();
            foreach (var point in Points)
            {
                vertices.Add(new Vector3(point.X, point.Y, CeilingZ));
            }

            var center = new Vector3(Points[0].X, Points[0].Y, CeilingZ);

            for (var i = 0; i < Points.Count; i++)
            {
                Vector2 startPoint2D = Points[i];
                Vector2 endPoint2D = Points[(i + 1) % Points.Count];

                Vector3 startPoint3D = new Vector3(startPoint2D.X, startPoint2D.Y, CeilingZ);
                Vector3 endPoint3D = new Vector3(endPoint2D.X, endPoint2D.Y, CeilingZ);

                World.DrawLine(startPoint3D, endPoint3D, Color.FromArgb(200, 255, 30, 68));

                Vector3 ceiling = new Vector3(Points[i].X, Points[i].Y, CeilingZ);
                Vector3 floor = new Vector3(Points[i].X, Points[i].Y, FloorZ);

                World.DrawLine(ceiling, floor, Color.FromArgb(200, 255, 30, 68));
            }

            // Fill ceiling
            for (int i = 1; i < vertices.Count - 1; i++)
            {
                Vector3 p1 = center;
                Vector3 p2 = vertices[i];
                Vector3 p3 = vertices[i + 1];

                World.DrawPoly(p1, p2, p3, Color.FromArgb(200, 255, 30, 68));
                World.DrawPoly(p2, p1, p3, Color.FromArgb(200, 255, 30, 68));
            }

            // Fill floor
            for (int i = 1; i < vertices.Count - 1; i++)
            {
                Vector3 p1 = new Vector3(Points[0].X, Points[0].Y, FloorZ);
                Vector3 p2 = new Vector3(Points[i].X, Points[i].Y, FloorZ);
                Vector3 p3 = new Vector3(Points[i + 1].X, Points[i + 1].Y, FloorZ);

                World.DrawPoly(p1, p2, p3, Color.FromArgb(200, 255, 30, 68));
                World.DrawPoly(p2, p1, p3, Color.FromArgb(200, 255, 30, 68));
            }

            // Fill vertical walls (double-sided)
            for (var i = 0; i < Points.Count; i++)
            {
                Vector2 startPoint2D = Points[i];
                Vector2 endPoint2D = Points[(i + 1) % Points.Count];

                Vector3 ceiling1 = new Vector3(startPoint2D.X, startPoint2D.Y, CeilingZ);
                Vector3 ceiling2 = new Vector3(endPoint2D.X, endPoint2D.Y, CeilingZ);
                Vector3 floor1 = new Vector3(startPoint2D.X, startPoint2D.Y, FloorZ);
                Vector3 floor2 = new Vector3(endPoint2D.X, endPoint2D.Y, FloorZ);

                // Draw front-facing wall
                World.DrawPoly(ceiling1, floor1, floor2, Color.FromArgb(150, 255, 30, 68));
                World.DrawPoly(floor2, ceiling2, ceiling1, Color.FromArgb(150, 255, 30, 68));

                // Draw reverse-facing wall
                World.DrawPoly(floor1, ceiling1, floor2, Color.FromArgb(150, 255, 30, 68));
                World.DrawPoly(ceiling2, floor2, ceiling1, Color.FromArgb(150, 255, 30, 68));
            }

            await Task.FromResult(0);
        }
    }
}