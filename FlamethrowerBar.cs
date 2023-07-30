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
        public ReLogic.Content.Asset<Texture2D> imageBar = ModContent.Request<Texture2D>("BossFlamethrower/bar");
        public UIElement panel;
        public UIImage bar;
        public UIText title;

        public override void OnInitialize()
        {
            panel = new UIElement();
            panel.Width.Set(imageBar.Width(), 0);
            panel.Height.Set(imageBar.Height(), 0);
            Append(panel);

            bar = new UIImage(imageBar);
            panel.Append(bar);

            title = new UIText("Boss Flamethrower Bar");
            panel.Append(title);

            base.OnInitialize();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (GUIConfig.Instance.DisplayBar)
            {
                if (Main.netMode == NetmodeID.SinglePlayer && ModContent.GetInstance<FlamethrowerSystem>().boss == false)
                {
                    return;
                }
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

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            panel.Left.Set((int)(Main.MenuUI.GetDimensions().Width * (GUIConfig.Instance.FlamethrowerBarX * 0.01)) - imageBar.Width() / 2, 0);
            panel.Top.Set((int)(Main.MenuUI.GetDimensions().Height * (GUIConfig.Instance.FlamethrowerBarY * 0.01)) - imageBar.Height() / 2, 0);

            title.Left.Set(imageBar.Width() - imageBar.Width() / 2, 0);
            title.Top.Set(imageBar.Height() + title.Height.Pixels + 10, 0);
            title.HAlign = 0.5f;

            float quotient = 0f;
            Color color = Color.Lime;

            FlamethrowerSystem system = ModContent.GetInstance<FlamethrowerSystem>();
            FlamethrowerPlayer player = Main.LocalPlayer.GetModPlayer<FlamethrowerPlayer>();

            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                if (system.flame)
                {
                    quotient = (float)system.duration / system.durationMax;
                    color = new Color(255, 96, 10);
                } else
                {
                    quotient = (float)system.cooldown / system.cooldownMax;
                    color = new Color(179, 252, 0);
                }
            }
            else
            {
                if (system.flame)
                {
                    quotient = (float)player.duration / player.durationMax;
                    color = new Color(255, 96, 10);
                }
                else
                {
                    quotient = (float)player.cooldown / player.cooldownMax;
                    color = new Color(179, 252, 0);
                }
            }

            quotient = Utils.Clamp(quotient, 0f, 1f);
            int offet = 4;

            Rectangle progressBar = new Rectangle((int)panel.Left.Pixels + offet, (int)panel.Top.Pixels + offet, imageBar.Width() - offet * 2, imageBar.Height() - offet * 2);
            progressBar.Width = (int)(progressBar.Width * quotient);
            

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
            if (!Main.dedServ)
            {
                FlamethrowerInterface = new UserInterface();
                AUI = new FlamethrowerUI();
                AUI.Activate();
            }
            base.Load();
        }

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
