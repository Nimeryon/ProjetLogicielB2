using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVPGameClient
{
    class Renderable : IDisposable
    {
        public Renderable Parent = null;
        public List<Renderable> Childrens = new List<Renderable>();
        public Transform LocalTransform = new Transform();
        public Transform WorldTransfrom = new Transform();
        public bool Visible = true;
        public bool FlipX
        {
            set
            {
                LocalTransform.Scale.X = Math.Abs(LocalTransform.Scale.X) * (value ? -1f : 1f);
            }
            get
            {
                return LocalTransform.Scale.X < 0f;
            }
        }
        public bool FlipY
        {
            set
            {
                LocalTransform.Scale.Y = Math.Abs(LocalTransform.Scale.Y) * (value ? -1f : 1f);
            }
            get
            {
                return LocalTransform.Scale.Y < 0f;
            }
        }
        public Vector2 Position { get { return LocalTransform.Position; } set { LocalTransform.Position = value; NeedUpdate(); } }
        public Vector2 Scale { get { return LocalTransform.Scale; } set { LocalTransform.Scale = value; NeedUpdate(); } }
        public Alignment Alignment { get { return LocalTransform.Alignment; } set { LocalTransform.Alignment = value; OriginUpdate(); } }
        public Vector2 Origin { get { return LocalTransform.Origin; } set { LocalTransform.Origin = value; Alignment = Alignment.Custom; OriginUpdate(); } }
        public float ScaleScalar { get { return LocalTransform.Scale.X; } set { Scale = Vector2.One * value; } }
        public float Rotation { get { return LocalTransform.Rotation; } set { LocalTransform.Rotation = value; NeedUpdate(); } }
        public float ZIndex { get { return _ZIndex; } set { _ZIndex = value; NeedUpdate(); } }
        private float _ZIndex = 0f;
        public Color Color { get { return LocalTransform.Color; } set { LocalTransform.Color = value; NeedUpdate(); } }
        public SpriteEffects Effects { get { return _Effects; } set { _Effects = value; NeedUpdate(); } }
        private SpriteEffects _Effects = SpriteEffects.None;
         
        private float _finalZIndex = 0f;
        private bool _needUpdate = false;
        private bool _originUpdate = false;

        public Renderable()
        {
            GameHandler.OnDraw += DrawCall;
        }

        private void NeedUpdate()
        {
            _needUpdate = true;
        }
        private void OriginUpdate()
        {
            _originUpdate = true;
            NeedUpdate();
        }
        public void Rotate(float _rotation)
        {
            if (_rotation != 0f)
            {
                Rotation += _rotation;
                NeedUpdate();
            }
        }
        public void Move(Vector2 _position)
        {
            if (_position != Vector2.Zero)
            {
                Position += _position;
                NeedUpdate();
            }
        }
        public Renderable GetChild(int index)
        {
            return Childrens[index];
        }
        public void AddChild(Renderable child)
        {
            if (child.Parent != null) throw new Exception("Child already have a parent.");

            Childrens.Add(child);
            child.Parent = this;
            child.NeedUpdate();
        }
        public void RemoveChild(Renderable child)
        {
            if (child.Parent != this) throw new Exception("Renderable is not a child of this parent.");

            Childrens.Remove(child);
            child.Parent = null;
            child.NeedUpdate();
        }
        public void RemoveFromParent()
        {
            Parent.RemoveChild(this);
        }
        public void DrawCall()
        {
            if (!Visible) return;

            if (_originUpdate)
            {
                Align();
                _originUpdate = false;
            }

            if (_needUpdate)
            {
                WorldTransfrom = Parent != null ? Transform.Compose(Parent.WorldTransfrom, LocalTransform) : LocalTransform;
                _finalZIndex = Parent != null ? Parent._finalZIndex + ZIndex : ZIndex;

                foreach (var child in Childrens)
                {
                    child.NeedUpdate();
                }

                _needUpdate = false;
            }

            Draw();
        }
        public void Align()
        {
            Vector2 size = GetSize();
            if (Alignment == Alignment.Custom)
            {
                Origin = new Vector2(size.X * Origin.X, size.Y * Origin.Y);
                return;
            }

            switch (Alignment)
            {
                case Alignment.TopLeft:
                    Origin = Vector2.Zero;
                    break;
                case Alignment.TopCenter:
                    Origin = new Vector2(size.X / 2, 0);
                    break;
                case Alignment.TopRight:
                    Origin = new Vector2(size.X, 0);
                    break;
                case Alignment.MiddleLeft:
                    Origin = new Vector2(0, size.Y / 2);
                    break;
                case Alignment.MiddleCenter:
                    Origin = new Vector2(size.X / 2, size.Y / 2);
                    break;
                case Alignment.MiddleRight:
                    Origin = new Vector2(size.X, size.Y / 2);
                    break;
                case Alignment.BottomLeft:
                    Origin = new Vector2(0, size.Y);
                    break;
                case Alignment.BottomCenter:
                    Origin = new Vector2(size.X / 2, size.Y);
                    break;
                case Alignment.BottomRight:
                    Origin = size;
                    break;
            }
        }
        public virtual Vector2 GetSize()
        {
            return Vector2.Zero;
        }
        public virtual void Draw()
        {

        }
        public void Dispose()
        {
            GameHandler.OnDraw -= DrawCall;
        }
    }
}
