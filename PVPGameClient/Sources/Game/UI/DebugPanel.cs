﻿using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVPGameClient
{
    public class DebugPanel : Panel, IDisposable
    {
        Paragraph Position;
        Paragraph SamePosition;
        Paragraph Velocity;
        Paragraph IsGrounded;
        Paragraph MousePos;

        public DebugPanel(Vector2 size) : base(size, anchor: Anchor.TopRight)
        {
            AddChild(new Header("Debug"));
            AddChild(new HorizontalLine());

            Position = new Paragraph();
            AddChild(Position);

            SamePosition = new Paragraph();
            AddChild(SamePosition);

            Velocity = new Paragraph();
            AddChild(Velocity);

            IsGrounded = new Paragraph();
            AddChild(IsGrounded);

            MousePos = new Paragraph();
            AddChild(MousePos);

            Visible = false;

            GameHandler.OnLateUpdate += Update;
        }

        public void Update()
        {
            if (InputSystem.GetKeyDown(Keys.F3)) Visible = !IsVisible();

            if (GameHandler.Players[GameHandler.CurrentPlayerIndex] != null)
            {
                Position.Text = string.Format("Pos: X:{0:0.000} / Y:{1:0.000}", GameHandler.Players[GameHandler.CurrentPlayerIndex].Position.X, GameHandler.Players[GameHandler.CurrentPlayerIndex].Position.Y);
                SamePosition.Text = string.Format("Same Pos: {0}", GameHandler.Players[GameHandler.CurrentPlayerIndex].Position.X == GameHandler.Players[GameHandler.CurrentPlayerIndex].OldPosition.X);
                Velocity.Text = string.Format("Vel: X:{0:0.000} / Y:{1:0.000}", GameHandler.Players[GameHandler.CurrentPlayerIndex].Velocity.X, GameHandler.Players[GameHandler.CurrentPlayerIndex].Velocity.Y);
                IsGrounded.Text = GameHandler.Players[GameHandler.CurrentPlayerIndex].IsGrounded ? "Grounded" : "Not Grounded";
                MousePos.Text = string.Format("Mouse Pos: X:{0} / Y:{1}", InputSystem.GetMousePos().X, InputSystem.GetMousePos().Y);
            }
        }
        
        public new void Dispose()
        {
            GameHandler.OnLateUpdate -= Update;
            base.Dispose();
        }
    }
}
