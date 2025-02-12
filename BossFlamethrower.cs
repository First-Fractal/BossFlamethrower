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
        //function for talking to everyone on the world
        public void Talk(string message, Color color)
        {
            //check if its singleplayer or multiplayer
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                //display the message into the chat with the speific color
                Main.NewText(message, color);
            }
            else
            {
                //brodcast the message to everyone in the world
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(message), color);
            }
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            //get the custom player class from the current player
            FlamethrowerPlayer BAP = Main.CurrentPlayer.GetModPlayer<FlamethrowerPlayer>();

            //save the values from the server 
            BAP.boss = reader.ReadBoolean();
            BAP.flame = reader.ReadBoolean();
            BAP.cooldown = reader.ReadInt32();
            BAP.cooldownMax = reader.ReadInt32();
            BAP.duration = reader.ReadInt32();
            BAP.durationMax = reader.ReadInt32();
        }
    }
}