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
using GeonBit.UI.Utils;

namespace PVPGameClient
{
    public class ConnexionPanel : Panel
    {
        // UI
        public TextInput Pseudo;
        public Image PlayerImage;
        public SelectList CharacterList;

        public string Character = "frog";

        private RichParagraph connectionText;

        public ConnexionPanel(Vector2 size) : base(size)
        {
            AddChild(new Header("Connexion"));
            AddChild(new HorizontalLine());

            Pseudo = new TextInput(false);
            Pseudo.PlaceholderText = "Pseudo...";
            AddChild(Pseudo);

            Panel entitiesGroup = new Panel(new Vector2(0, 200), PanelSkin.None, Anchor.Auto);
            entitiesGroup.Padding = Vector2.Zero;
            AddChild(entitiesGroup);

            var columnPanels = PanelsGrid.GenerateColums(2, entitiesGroup);
            foreach (var column in columnPanels) { column.Padding = Vector2.Zero; }
            Panel leftPanel = columnPanels[0];
            Panel rightPanel = columnPanels[1];

            Panel imagePanel = new Panel(new Vector2(200, 200), PanelSkin.Simple, Anchor.AutoCenter);
            rightPanel.AddChild(imagePanel);

            PlayerImage = new Image(Loader.Frog[0], Vector2.Zero);
            imagePanel.AddChild(PlayerImage);

            leftPanel.AddChild(new Label(@"Characters", Anchor.AutoCenter));
            CharacterList = new SelectList(new Vector2(0, 170), Anchor.Auto);
            CharacterList.AddItem("Frog");
            CharacterList.AddItem("Mask");
            CharacterList.AddItem("Pink");
            CharacterList.AddItem("Virtual");
            CharacterList.SelectedIndex = 0;
            leftPanel.AddChild(CharacterList);
            CharacterList.OnValueChange = (Entity entity) =>
            {
                string name = ((SelectList)(entity)).SelectedValue.ToLower();
                Character = name;

                switch (name)
                {
                    case "frog":
                        PlayerImage.Texture = Loader.Frog[0];
                        break;
                    case "mask":
                        PlayerImage.Texture = Loader.Mask[0];
                        break;
                    case "pink":
                        PlayerImage.Texture = Loader.Pink[0];
                        break;
                    case "virtual":
                        PlayerImage.Texture = Loader.Virtual[0];
                        break;
                }
            };

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