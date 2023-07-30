using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossFlamethrower
{
    public class GlobalBuffFlamethrower : GlobalBuff
    {
        public override void Update(int type, Player player, ref int buffIndex)
        {
            //check if the buff is from the player
            if (type == BuffID.CursedInferno)
            {
                //get all of the current npcs
                foreach (NPC npc in Main.npc)
                {
                    //remove the debuff, depending on the config setup
                    if (Config.Instance.UseCursed == false)
                    {
                        player.ClearBuff(BuffID.CursedInferno);
                    }
                    else if (Main.hardMode == false && Config.Instance.CursedPHM == false)
                    {
                        player.ClearBuff(BuffID.CursedInferno);
                    }

                }
            }
            base.Update(type, player, ref buffIndex);
        }
    }
}
