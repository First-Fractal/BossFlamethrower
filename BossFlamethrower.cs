using Terraria;
using System.IO;
using Terraria.ID;
using Terraria.Chat;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace BossFlamethrower
{
    public class BossFlamethrower : Mod
    {
        public void Talk(string message, Color color)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(message, color);
            }
            else
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(message), color);
            }
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            FlamethrowerPlayer BAP = Main.CurrentPlayer.GetModPlayer<FlamethrowerPlayer>();

            BAP.boss = reader.ReadBoolean();
            BAP.flame = reader.ReadBoolean();
            BAP.cooldown = reader.ReadInt32();
            BAP.cooldownMax = reader.ReadInt32();
            BAP.duration = reader.ReadInt32();
            BAP.durationMax = reader.ReadInt32();
        }
    }
}