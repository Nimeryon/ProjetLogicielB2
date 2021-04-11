using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace PVPGameClient
{
    public class GameHandler : Game
    {
        private GraphicsDeviceManager Graphics;
        public static SpriteBatch SpriteBatch;

        // Events
        public delegate void DrawEvent();
        public delegate void UpdateEvent();
        public static event DrawEvent OnDraw;
        public static event UpdateEvent OnBeforeUpdate;
        public static event UpdateEvent OnUpdate;
        public static event UpdateEvent OnLateUpdate;

        // Important System
        public static TickSystem Ticks;
        public static InputSystem Inputs;
        private FPSCounter FPS;

        // Start
        public static SpriteFont _font;
        Texture2D _texture;
        Vector2 _position;
        float _speed = 10f;
        Text test;

        private ClientTCP ClienTCP;
        private ClientDataHandler DataHandler;

        public GameHandler()
        {
            Graphics = new GraphicsDeviceManager(this)
            {
                SynchronizeWithVerticalRetrace = false
            };
            Graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // More than 60 FPS
            IsFixedTimeStep = false;
            Graphics.PreferredBackBufferWidth = 1080;
            Graphics.PreferredBackBufferHeight = 720;
            Graphics.ApplyChanges();

            // Server part
            //DataHandler = new ClientDataHandler();
            //ClienTCP = new ClientTCP();
            base.Initialize();

            // Send login to server
            //ClienTCP.SendLogin();
        }
        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            _font = Content.Load<SpriteFont>("Fonts/PixelFont");
            _texture = Content.Load<Texture2D>("Sprites/Characters/player_1");

            int width = Graphics.GraphicsDevice.Viewport.Width;
            int height = Graphics.GraphicsDevice.Viewport.Height;
            _position = new Vector2(width / 2, height / 2);

            // Create text
            new Text(_font, "Top Center", new Vector2(width / 2, 0f), Alignment.TopCenter);
            new Text(_font, "Top Right", new Vector2(width, 0f), Alignment.TopRight);
            new Text(_font, "Middle Left", new Vector2(0, height / 2), Alignment.MiddleLeft);
            test = new Text(_font, "Middle Center", new Vector2(width / 2, height / 2), Alignment.MiddleCenter);
            new Text(_font, "Middle Right", new Vector2(width, height / 2), Alignment.MiddleRight);
            new Text(_font, "Bottom Left", new Vector2(0, height), Alignment.BottomLeft);
            new Text(_font, "Bottom Center", new Vector2(width / 2, height), Alignment.BottomCenter);
            new Text(_font, "Bottom Right", new Vector2(width, height), Alignment.BottomRight);

            //var spr = new Sprite(_texture, new Vector2(width / 2, height / 2), Alignment.MiddleCenter);
            //spr.SetFromSpriteSheet(new Point(1, 0), new Point(3, 4));

            // Initialize important systems
            Ticks = new TickSystem();
            Inputs = new InputSystem();
            FPS = new FPSCounter();
        }
        protected override void Update(GameTime gameTime)
        {
            // Set DeltaTime
            Globals.DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            BeforeUpdate();

            // Events
            OnUpdate?.Invoke();

            // Keyboard Input
            /*if (keyState.IsKeyDown(Keys.Z))
            {
                _position.Y -= _speed * Globals.DeltaTime10;
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                _position.Y += _speed * Globals.DeltaTime10;
            }
            if (keyState.IsKeyDown(Keys.Q))
            {
                _position.X -= _speed * Globals.DeltaTime10;
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                _position.X += _speed * Globals.DeltaTime10;
            }*/

            base.Update(gameTime);
            LateUpdate();
        }
        protected void BeforeUpdate()
        {
            OnBeforeUpdate?.Invoke();
        }
        protected void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SamplerState state = new SamplerState { Filter = TextureFilter.Point };
            SpriteBatch.Begin(samplerState: state);
            // Events
            OnDraw?.Invoke();

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
