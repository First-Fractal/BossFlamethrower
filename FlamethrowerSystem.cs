using Terraria;
using Terraria.ID;
using Terraria.Chat;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace BossFlamethrower
{
    public class FlamethrowerSystem : ModSystem
    {
        //set the duration values from the config
        public int cooldownMax = Config.Instance.FlamethrowerCooldown;
        public int durationMax = Config.Instance.FlamethrowerDuration;

        //the counter values
        public int cooldown = 0;
        public int duration = 0;
        public int counter;

        //flags for seeing if there is a boss/flames active
        public bool boss = false;
        public bool flame = false;

        //list of all of the boss parts
        public int[] BossParts = { NPCID.EaterofWorldsHead, NPCID.EaterofWorldsBody, NPCID.EaterofWorldsTail, NPCID.Creeper, NPCID.SkeletronHand, NPCID.SkeletronHead, NPCID.WallofFleshEye, NPCID.TheDestroyer, NPCID.TheDestroyerBody, NPCID.TheDestroyerTail, NPCID.Probe, NPCID.PrimeCannon, NPCID.PrimeLaser, NPCID.PrimeSaw, NPCID.PrimeVice, NPCID.PlanterasHook, NPCID.PlanterasTentacle, NPCID.GolemFistLeft, NPCID.GolemFistRight, NPCID.GolemHead, NPCID.GolemHeadFree, NPCID.CultistBossClone, NPCID.MoonLordCore, NPCID.MoonLordHand, NPCID.MoonLordHead, NPCID.MoonLordFreeEye, NPCID.MoonLordLeechBlob };

        public override void PostUpdateWorld()
        {
            //update the duration values from the config
            cooldownMax = Config.Instance.FlamethrowerCooldown;
            durationMax = Config.Instance.FlamethrowerDuration;
            
            //update the timers to the minute usage 
            if (Config.Instance.UseMinutes)
            {
                cooldownMax = (int)(Config.Instance.FlamethrowerCooldown * 60f);
                durationMax = (int)(Config.Instance.FlamethrowerDuration * 60f);
            }

            //set the boss flag to be false
            boss = false;

            //loop through all of the npc in the world
            for (int i = 0; i < Main.npc.Length; i++)
            {
                //check if the npc is a boss and active
                if (Main.npc[i].boss && Main.npc[i].active == true)
                {
                    //set the boss flag to be true and exit
                    boss = true;
                    break;
                }

                //loop through all of the boss parts
                foreach (int bossPart in BossParts)
                {
                    //check if the npc is a boss part 
                    if (Main.npc[i].type == bossPart && Main.npc[i].active == true)
                    {
                        //set the boss flag to be true and exit
                        boss = true;
                        break;
                    }
                }
            }

            //check if the boss is active
            if (boss)
            {
                //check if the cooldown is above the max
                if (cooldown > cooldownMax)
                {
                    //enable the flame and reset the cooldown
                    flame = true;
                    cooldown = 0;
                }

                //check if the duration is above the max
                if (duration > durationMax)
                {
                    //disable the flame and reset the duration
                    flame = false;
                    duration = 0;
                }

                //increase the tick counter
                counter++;
                //check if 60 ticks has passed (60 ticks = 1 second)
                if (counter >= 60)
                {
                    //check which one to increase based on the flame state
                    if (flame)
                    {
                        duration++;
                    }
                    else
                    {
                        cooldown++;
                    }

                    //reset the tick counter
                    counter = 0;

                    //check if it isnt singleplayer
                    if (Main.netMode != NetmodeID.SinglePlayer)
                    {
                        //create a new modded packet
                        ModPacket packet = ModContent.GetInstance<BossFlamethrower>().GetPacket();

                        //save all of the important values
                        packet.Write(boss);
                        packet.Write(flame);
                        packet.Write(cooldown);
                        packet.Write(cooldownMax);
                        packet.Write(duration);
                        packet.Write(durationMax);
                        packet.Send();
                    }
                }
            } else
            {
                //reset the values for when there is no boss alive
                flame = false;
                cooldown = 0;
                duration = 0;
            }

            base.PostUpdateWorld();
        }
    }
}
