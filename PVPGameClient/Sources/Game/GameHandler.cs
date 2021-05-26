using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;

// using GeonBit UI elements
using GeonBit.UI.Entities;
using GeonBit.UI.Entities.TextValidators;
using GeonBit.UI.DataTypes;
using GeonBit.UI.Utils.Forms;
using GeonBit.UI;
using PVPGameLibrary;

namespace PVPGameClient
{
    public class GameHandler : Game
    {
        public static GameHandler I;

        public static GraphicsDeviceManager Graphics;
        public static SpriteBatch SpriteBatch;

        // Events
        public delegate void DrawEvent();
        public delegate void UpdateEvent();
        public static event DrawEvent OnDraw;
        public static event UpdateEvent OnBeforeUpdate;
        public static event UpdateEvent OnUpdate;
        public static event UpdateEvent OnLateUpdate;

        // System Info
        public static int Width, Height;
        public static Viewport Viewport;

        // Important System
        FPSCounter FPS;

        // UI
        public ConnexionPanel ConnexionPanel;
        public CharacterSelectionPanel CharacterSelectionPanel;
        public DebugPanel DebugPanel;

        // Test
        public BackGround BackGround;

        // Connexion state
        public Timer ConnexionTimer;
        public bool AwaitConnexion = false;
        public static int CurrentPlayerIndex;

        // Start
        public static Player[] Players = new Player[Constants.MAX_PLAYERS];
        public static Grid Grid;
        public static VisualGrid VisualGrid;
        public static SpriteFont _font;
        public static Texture2D _texture;
        public Texture2D PlayerBody;

        public static ClientTCP ClienTCP;
        public static ClientDataHandler DataHandler;

        public GameHandler()
        {
            I = this;
            Graphics = new GraphicsDeviceManager(this)
            {
                SynchronizeWithVerticalRetrace = false
            };
            Graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // create and init the UI manager
            UserInterface.Initialize(Content, BuiltinThemes.hd);
            UserInterface.Active.UseRenderTarget = true;

            // draw cursor outside the render target
            UserInterface.Active.IncludeCursorInRenderTarget = false;

            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // make the window fullscreen (but still with border and top control bar)
            Width = 16 * 64;
            Height = 16 * 48;

            Graphics.PreferredBackBufferWidth = Width;
            Graphics.PreferredBackBufferHeight = Height;
            Graphics.IsFullScreen = false;
            Graphics.ApplyChanges();

            // Server part
            DataHandler = new ClientDataHandler();
            ClienTCP = new ClientTCP(false);

            base.Initialize();
        }
        protected override void LoadContent()
        {
            // Load Content
            Viewport = GraphicsDevice.Viewport;
            Loader.Content = Content;
            Loader.Load();

            PlayerBody = Loader.LoadTexture("Sprites/Characters/body");
            _texture = Loader.LoadTexture("Sprites/Characters/player_1");
            _font = Content.Load<SpriteFont>("Fonts/PixelFont");

            BackGround = new BackGround(BackGroundColor.Gray);

            // Initialize important systems
            new TickSystem();
            new InputSystem();
            FPS = new FPSCounter();

            //InitializeUI();

            // Load Grid
            Grid = new Grid();
            VisualGrid = new VisualGrid();

            base.LoadContent();
        }
        protected override void Update(GameTime gameTime)
        {
            //if (!IsActive) return;

            // Set Time
            Globals.GameTime = gameTime;
            BeforeUpdate();

            // Events
            OnUpdate?.Invoke();

            if (InputSystem.GetKeyUp(Keys.F3)) FPS.HideShow();

            // Update UI
            UserInterface.Active.Update(gameTime);

            LateUpdate();

            base.Update(gameTime);
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
            // draw ui
            UserInterface.Active.Draw(SpriteBatch);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            SamplerState state = new SamplerState { Filter = TextureFilter.Point };
            SpriteBatch.Begin(samplerState: state);
            OnDraw?.Invoke();
            SpriteBatch.End();

            // finalize ui rendering
            UserInterface.Active.DrawMainRenderTarget(SpriteBatch);

            base.Draw(gameTime);
        }
        private void InitializeUI()
        {
            // Connexion panel
            ConnexionPanel = new ConnexionPanel(new Vector2(480, -1));
            UserInterface.Active.AddEntity(ConnexionPanel);

            //CharacterSelectionPanel = new CharacterSelectionPanel(new Vector2(480, -1));
            //UserInterface.Active.AddEntity(CharacterSelectionPanel);
        }
        public void Connected()
        {
            Console.WriteLine("Serveur connecté!");
            ConnexionTimer.Dispose();
            ClienTCP.SendLogin(ConnexionPanel.Pseudo.Value, ConnexionPanel.Character);

            UserInterface.Active.RemoveEntity(ConnexionPanel);
            ConnexionPanel.Dispose();
            ConnexionPanel = null;

            DebugPanel = new DebugPanel(new Vector2(480f, -1));
            UserInterface.Active.AddEntity(DebugPanel);
        }
        public void Disconnected()
        {
            Console.WriteLine("Connexion perdu!");

            for(int i = 0; i < Players.Length; i++)
            {
                if (Players[i] == null) break;

                Players[i].Dispose();
                Players[i] = null;
            }

            ConnexionTimer.Dispose();

            UserInterface.Active.RemoveEntity(DebugPanel);
            DebugPanel.Dispose();

            // Connexion panel
            ConnexionPanel = new ConnexionPanel(new Vector2(480, -1));
            ConnexionPanel.Error();
            UserInterface.Active.AddEntity(ConnexionPanel);
        }
    }
}