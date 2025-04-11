using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace PolyZoneLib.Client
{
    internal class PolyZone
    {
        private readonly List<Vector2> Points = new List<Vector2>();
        private readonly float Ceiling;
        private readonly float Floor;
        private readonly Color Color;
        private bool isInside = false;

        internal PolyZone(Vector2[] points, float floor, float ceiling, Color color)
        {
            Points.AddRange(points);
            Floor = floor;
            Ceiling = ceiling;
            Color = color;
        }

        private bool IsPointInPolygon(Vector2 point)
        {
            bool inside = false;
            int j = Points.Count - 1;

            for (int i = 0; i < Points.Count; j = i++)
            {
                if ((Points[i].Y > point.Y) != (Points[j].Y > point.Y) &&
                    point.X < (Points[j].X - Points[i].X) * (point.Y - Points[i].Y) / (Points[j].Y - Points[i].Y) + Points[i].X)
                {
                    inside = !inside;
                }
            }

            return inside;
        }

        internal async Task IsPlayerInside(Action onEnterAction)
        {
            Vector3 playerPos = Game.PlayerPed.Position;
            Vector2 player2D = new Vector2(playerPos.X, playerPos.Y);

            bool currentlyInside = playerPos.Z >= Floor && playerPos.Z <= Ceiling && IsPointInPolygon(player2D);

            if (currentlyInside && !isInside)
            {
                isInside = true;
                onEnterAction.Invoke();
            }
            else if (!currentlyInside && isInside)
            {
                isInside = false;
            }

            await Task.FromResult(0);
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
                vertices.Add(new Vector3(point.X, point.Y, Ceiling));
            }

            var center = new Vector3(Points[0].X, Points[0].Y, Ceiling);

            for (var i = 0; i < Points.Count; i++)
            {
                Vector2 startPoint2D = Points[i];
                Vector2 endPoint2D = Points[(i + 1) % Points.Count];

                Vector3 startPoint3D = new Vector3(startPoint2D.X, startPoint2D.Y, Ceiling);
                Vector3 endPoint3D = new Vector3(endPoint2D.X, endPoint2D.Y, Ceiling);

                World.DrawLine(startPoint3D, endPoint3D, Color);

                Vector3 ceiling = new Vector3(Points[i].X, Points[i].Y, Ceiling);
                Vector3 floor = new Vector3(Points[i].X, Points[i].Y, Floor);

                World.DrawLine(ceiling, floor, Color);
            }

            for (int i = 1; i < vertices.Count - 1; i++)
            {
                Vector3 p1 = center;
                Vector3 p2 = vertices[i];
                Vector3 p3 = vertices[i + 1];

                World.DrawPoly(p1, p2, p3, Color);
                World.DrawPoly(p2, p1, p3, Color);
            }

            for (int i = 1; i < vertices.Count - 1; i++)
            {
                Vector3 p1 = new Vector3(Points[0].X, Points[0].Y, Floor);
                Vector3 p2 = new Vector3(Points[i].X, Points[i].Y, Floor);
                Vector3 p3 = new Vector3(Points[i + 1].X, Points[i + 1].Y, Floor);

                World.DrawPoly(p1, p2, p3, Color);
                World.DrawPoly(p2, p1, p3, Color);
            }

            for (var i = 0; i < Points.Count; i++)
            {
                Vector2 startPoint2D = Points[i];
                Vector2 endPoint2D = Points[(i + 1) % Points.Count];

                Vector3 ceiling1 = new Vector3(startPoint2D.X, startPoint2D.Y, Ceiling);
                Vector3 ceiling2 = new Vector3(endPoint2D.X, endPoint2D.Y, Ceiling);
                Vector3 floor1 = new Vector3(startPoint2D.X, startPoint2D.Y, Floor);
                Vector3 floor2 = new Vector3(endPoint2D.X, endPoint2D.Y, Floor);

                World.DrawPoly(ceiling1, floor1, floor2, Color);
                World.DrawPoly(floor2, ceiling2, ceiling1, Color);
                World.DrawPoly(floor1, ceiling1, floor2, Color);
                World.DrawPoly(ceiling2, floor2, ceiling1, Color);
            }

            await Task.FromResult(0);
        }
    }
}