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
        public int cooldownMax = Config.Instance.FlamethrowerCooldown;
        public int durationMax = Config.Instance.FlamethrowerDuration;
        public int cooldown = 0;
        public int duration = 0;
        public int counter;
        public bool boss = false;
        public bool flame = false;
        public int[] BossParts = { NPCID.EaterofWorldsHead, NPCID.EaterofWorldsBody, NPCID.EaterofWorldsTail, NPCID.Creeper, NPCID.SkeletronHand, NPCID.SkeletronHead, NPCID.WallofFleshEye, NPCID.TheDestroyer, NPCID.TheDestroyerBody, NPCID.TheDestroyerTail, NPCID.Probe, NPCID.PrimeCannon, NPCID.PrimeLaser, NPCID.PrimeSaw, NPCID.PrimeVice, NPCID.PlanterasHook, NPCID.PlanterasTentacle, NPCID.GolemFistLeft, NPCID.GolemFistRight, NPCID.GolemHead, NPCID.GolemHeadFree, NPCID.CultistBossClone, NPCID.MoonLordCore, NPCID.MoonLordHand, NPCID.MoonLordHead, NPCID.MoonLordFreeEye, NPCID.MoonLordLeechBlob };

        public override void PostUpdateWorld()
        {
            cooldownMax = Config.Instance.FlamethrowerCooldown;
            durationMax = Config.Instance.FlamethrowerDuration;
            if (Config.Instance.UseMinutes)
            {
                cooldownMax = (int)(Config.Instance.FlamethrowerCooldown * 60f);
                durationMax = (int)(Config.Instance.FlamethrowerDuration * 60f);
            }
            else
            {
                cooldownMax = Config.Instance.FlamethrowerCooldown;
                durationMax = Config.Instance.FlamethrowerDuration;
            }


            boss = false;
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].boss && Main.npc[i].active == true)
                {
                    boss = true;
                }

                foreach (int bossPart in BossParts)
                {
                    if (Main.npc[i].type == bossPart && Main.npc[i].active == true)
                    {
                        boss = true;
                    }
                }
            }

            if (boss)
            {
                if (cooldown > cooldownMax)
                {
                    flame = true;
                    cooldown = 0;
                }

                if (duration > durationMax)
                {
                    flame = false;
                    duration = 0;
                }

                counter++;
                if (counter >= 60)
                {
                    if (flame)
                    {
                        duration++;
                    }
                    else
                    {
                        cooldown++;
                    }

                    counter = 0;

                    if (Main.netMode != NetmodeID.SinglePlayer)
                    {
                        ModPacket packet = ModContent.GetInstance<BossFlamethrower>().GetPacket();
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
                flame = false;
                cooldown = 0;
                duration = 0;
            }

            base.PostUpdateWorld();
        }
    }
}
