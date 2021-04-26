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
using GeonBit.UI.Utils;
using GeonBit.UI;

namespace PVPGameClient
{
    public class CharacterSelectionPanel : Panel
    {
        Image player;

        Color outline = new Color(34, 32, 52);
        Color eyes = new Color(238, 119, 85);
        Color skin = new Color(204, 204, 119);
        Color darkSkin = new Color(170, 170, 85);
        Color cloth = new Color(119, 34, 170);
        Color darkCloth = new Color(85, 0, 119);
        int darkerColor = 34;

        public CharacterSelectionPanel(Vector2 size) : base(size)
        {
            // Create color dictionnary
            Dictionary<Color, Color> colors = new Dictionary<Color, Color>();
            colors.Add(eyes, Color.White);
            colors.Add(skin, Color.Gray);
            colors.Add(darkSkin, Colors.DarkColor(Color.Gray, darkerColor));
            colors.Add(cloth, Color.White);
            colors.Add(darkCloth, Color.White);

            // Create UI
            AddChild(new Header("Creation de personnage"));
            AddChild(new HorizontalLine());

            Panel entitiesGroup = new Panel(new Vector2(0, 250), PanelSkin.None, Anchor.Auto);
            entitiesGroup.Padding = Vector2.Zero;
            AddChild(entitiesGroup);

            var columnPanels = PanelsGrid.GenerateColums(3, entitiesGroup);
            foreach (var column in columnPanels) { column.Padding = Vector2.Zero; }
            Panel leftPanel = columnPanels[0];
            Panel centerPanel = columnPanels[1];
            Panel rightPanel = columnPanels[2];

            Panel imagePanel = new Panel(new Vector2(160, 160), PanelSkin.Simple, Anchor.AutoCenter);
            centerPanel.AddChild(imagePanel);

            player = new Image(GameHandler.I.PlayerBody, Vector2.Zero);
            //Colors.ReplaceColor(player.Texture, colors);
            imagePanel.AddChild(player);
        }
    }
}