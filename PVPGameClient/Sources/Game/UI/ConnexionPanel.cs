using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

// using GeonBit UI elements
using GeonBit.UI.Entities;
using GeonBit.UI.Animators;
using GeonBit.UI.Entities.TextValidators;
using GeonBit.UI.DataTypes;
using GeonBit.UI.Utils.Forms;
using GeonBit.UI;

namespace PVPGameClient
{
    public class ConnexionPanel : Panel
    {
        // UI
        public TextInput Pseudo;

        private RichParagraph connectionText;

        public ConnexionPanel(Vector2 size) : base(size)
        {
            AddChild(new Header("Connexion"));
            AddChild(new HorizontalLine());

            Pseudo = new TextInput(false);
            Pseudo.PlaceholderText = "Pseudo...";
            AddChild(Pseudo);

            Button connexion = new Button("Connexion");
            connexion.OnClick += (_) => {
                if (Pseudo.Value != "")
                {
                    GameHandler.ClienTCP.ConnectToServer();
                    if (GameHandler.I.AwaitConnexion == false)
                    {
                        GameHandler.I.AwaitConnexion = true;
                        GameHandler.I.ConnexionTimer = new Timer((_) =>
                        {
                            Console.WriteLine("Serveur non trouvé");
                            Pseudo.Value = "";
                            Pseudo.PlaceholderText = "Serveur non disponible";
                            connectionText.Visible = false;
                            GameHandler.I.AwaitConnexion = false;
                            GameHandler.ClienTCP.Disconnect();
                            GameHandler.I.ConnexionTimer.Dispose();
                        }, new AutoResetEvent(false), 5000, Timeout.Infinite);
                    }
                    connectionText.Visible = true;
                    return;
                }
                Pseudo.PlaceholderText = "Aucun pseudo";
            };
            AddChild(connexion);

            Button quit = new Button("Quitter");
            quit.OnClick += (_) => { GameHandler.I.Exit(); };
            AddChild(quit);

            connectionText = new RichParagraph("Connexion...", Anchor.TopCenter, Color.Yellow) { Offset = new Vector2(0, -64) };
            connectionText.AttachAnimator(new TextWaveAnimator(){ SpeedFactor = 15f, WaveLengthFactor = 1f, WaveHeight = 10f });
            connectionText.Visible = false;
            AddChild(connectionText);
        }
        public void Reset()
        {
            Pseudo.Value = "";
            Pseudo.PlaceholderText = "Pseudo...";
            connectionText.Visible = false;
        }
        public void Error()
        {
            Pseudo.PlaceholderText = "Connexion perdu...";
        }
    }
}