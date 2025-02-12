using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent;
using System.Collections.Generic;
using Terraria.ID;

namespace BossFlamethrower
{
    public class FlamethrowerUI : UIState
    {
        //get the bar image
        public ReLogic.Content.Asset<Texture2D> imageBar = ModContent.Request<Texture2D>("BossFlamethrower/bar");

        //create the panel, bar, and title values
        public UIElement panel;
        public UIImage bar;
        public UIText title;

        //initalize the assets
        public override void OnInitialize()
        {
            //create the panel to group the bar and text
            panel = new UIElement();

            //set it to be the size of the image bar
            panel.Width.Set(imageBar.Width(), 0);
            panel.Height.Set(imageBar.Height(), 0);

            //add the panel to the UI
            Append(panel);

            //create the bar element from the image
            bar = new UIImage(imageBar);

            //insert it into the panel
            panel.Append(bar);

            //set the UI text for the bar and add it into the panel
            title = new UIText("Boss Flamethrower Bar");
            panel.Append(title);

            base.OnInitialize();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //check if the bar should be displayed
            if (GUIConfig.Instance.DisplayBar)
            {
                //check if there is no boss from the singeplayer way, and don't draw the bar
                if (Main.netMode == NetmodeID.SinglePlayer && ModContent.GetInstance<FlamethrowerSystem>().boss == false)
                {
                    return;
                }
                //check if there is no boss from the multiplayer client way, and don't draw the bar
                else if (Main.netMode == NetmodeID.MultiplayerClient && Main.LocalPlayer.GetModPlayer<FlamethrowerPlayer>().boss == false)
                {
                    return;
                }
            }
            else
            {
                return;
            }

            base.Draw(spriteBatch);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            //set the panel to be where the player set it in the config
            panel.Left.Set((int)(Main.MenuUI.GetDimensions().Width * (GUIConfig.Instance.FlamethrowerBarX * 0.01)) - imageBar.Width() / 2, 0);
            panel.Top.Set((int)(Main.MenuUI.GetDimensions().Height * (GUIConfig.Instance.FlamethrowerBarY * 0.01)) - imageBar.Height() / 2, 0);

            //set the X and Y of the text be in the center of the bar
            title.Left.Set(imageBar.Width() - imageBar.Width() / 2, 0);
            title.Top.Set(imageBar.Height() + title.Height.Pixels + 10, 0);

            //set the text to be align in the center
            title.HAlign = 0.5f;

            //value for seeing how much of the bar should be filled
            float quotient = 0f;

            //the default color of the bar
            Color color = Color.Lime;

            //get the instance of the system and the player
            FlamethrowerSystem system = ModContent.GetInstance<FlamethrowerSystem>();
            FlamethrowerPlayer player = Main.LocalPlayer.GetModPlayer<FlamethrowerPlayer>();

            //check if its singleplayer
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                //check if the flame is enabled
                if (system.flame)
                {
                    //calculate the qoutient of the bar, and change the color to orange
                    quotient = (float)system.duration / system.durationMax;
                    color = new Color(255, 96, 10);
                } else
                {
                    //calculate the qoutient of the bar, and change the color to lime green
                    quotient = (float)system.cooldown / system.cooldownMax;
                    color = new Color(179, 252, 0);
                }
            }
            //then its a multiplayer client
            else
            {
                //check if the flame is enabled
                if (system.flame)
                {
                    //calculate the qoutient of the bar, and change the color to orange
                    quotient = (float)system.duration / system.durationMax;
                    color = new Color(255, 96, 10);
                }
                else
                {
                    //calculate the qoutient of the bar, and change the color to lime green
                    quotient = (float)system.cooldown / system.cooldownMax;
                    color = new Color(179, 252, 0);
                }
            }

            //make sure that it dosn't overflow
            quotient = Utils.Clamp(quotient, 0f, 1f);

            //move the bar slightly away from the border of the bar
            int offet = 4;

            //set the corners of the progress bar
            Rectangle progressBar = new Rectangle((int)panel.Left.Pixels + offet, (int)panel.Top.Pixels + offet, imageBar.Width() - offet * 2, imageBar.Height() - offet * 2);

            //set how much of the bar to fill out
            progressBar.Width = (int)(progressBar.Width * quotient);

            //draw the progress bar
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, progressBar, color);

            base.DrawSelf(spriteBatch);
        }
    }

    public class FlamethrowerUISystem : ModSystem
    {
        internal UserInterface FlamethrowerInterface;
        internal FlamethrowerUI AUI;
        private GameTime lastGameTime;

        public override void Load()
        {
            //active the ui for non server states
            if (!Main.dedServ)
            {
                FlamethrowerInterface = new UserInterface();
                AUI = new FlamethrowerUI();
                AUI.Activate();
            }
            base.Load();
        }


        //idk what the rest of the code does since I stole it from TML Example Mod
        public override void UpdateUI(GameTime gameTime)
        {
            lastGameTime = gameTime;
            if (FlamethrowerInterface?.CurrentState != null)
            {
                FlamethrowerInterface.Update(gameTime);
            }

            FlamethrowerInterface?.SetState(AUI);
            base.UpdateUI(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "Boss Flamethrower Counter",
                    delegate
                    {
                        if (lastGameTime != null && FlamethrowerInterface?.CurrentState != null)
                        {
                            FlamethrowerInterface.Draw(Main.spriteBatch, lastGameTime);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI));
            }
            base.ModifyInterfaceLayers(layers);
        }
    }
}
