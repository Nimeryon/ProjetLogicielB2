using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVPGameClient
{
    public class Transform : PVPGameLibrary.Transform
    {
        public Color Color = Color.White;
        public Alignment Alignment = Alignment.TopLeft;
        public Vector2 Origin = Vector2.Zero;

        public Vector2 TransformVector(Vector2 point)
        {
            Vector2 result = Vector2.Transform(point, Matrix.CreateRotationZ(Rotation * Math.Sign(Scale.X)));
            result *= Scale;
            result += Position;
            return result;
        }
        public static Transform Compose(Transform a, Transform b)
        {
            Transform result = new Transform();
            Vector2 transformedPosition = a.TransformVector(b.Position);
            result.Position = transformedPosition;
            result.Rotation = a.Rotation + b.Rotation;
            result.Scale = a.Scale * b.Scale;
            result.Color = MultiplyColors(a.Color, b.Color);
            return result;
        }
        public static Color MultiplyColors(Color a, Color b)
        {
            return new Color(
                (a.R / 255f) * (b.R / 255f),
                (a.G / 255f) * (b.G / 255f),
                (a.B / 255f) * (b.B / 255f),
                (a.A / 255f) * (b.A / 255f)
            );
        }
    }
}
