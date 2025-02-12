using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace BossFlamethrower
{
    public class GlobalBossFlamethrower : GlobalNPC
    {
        //get an instance of the mod
        static BossFlamethrower BF = new BossFlamethrower();

        //check if talked about the flamethrower change
        static bool talked = false;

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            //check if moonlord should have a flamethrower
            if (Config.Instance.MoonCursed)
            {
                //check if the npc that just spawn in is moon lord
                if (npc.type == NPCID.MoonLordCore)
                {
                    //warn them about the lighting issue
                    BF.Talk(Language.GetTextValue("Mods.BossFlamethrower.Warning"), Color.Cyan);
                }
            }
            base.OnSpawn(npc, source);
        }

        public override void AI(NPC npc)
        {
            //get all of the bosses and boss parts
            if (npc.boss || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.CultistBossClone)
            {
                //check the flamethrower state
                if (ModContent.GetInstance<FlamethrowerSystem>().flame)
                {
                    //dont give spazmatism a flame thrower, since he already has one
                    if (npc.type != NPCID.Spazmatism)
                    {
                        //set the length of the flamethrower
                        Vector2 velocity = new Vector2(Config.Instance.FlameDis, 0);
                        //if the boss is a eye, boss then rotate the flamethrower like this to make it come out of the eye
                        if (npc.type == NPCID.EyeofCthulhu || npc.type == NPCID.Retinazer)
                        {
                            velocity = velocity.RotatedBy(npc.rotation + MathHelper.ToRadians(90));
                        }
                        //if the boss is a plantera, then rotate the flamethrower like this to make it come out of the mouth
                        else if (npc.type == NPCID.Plantera)
                        {
                            velocity = velocity.RotatedBy(npc.rotation + MathHelper.ToRadians(270));
                        }
                        //if the boss is a duke fishron, then rotate the flamethrower like this to make it come out of the mouth
                        else if (npc.type == NPCID.DukeFishron)
                        {
                            velocity = velocity.RotatedBy(npc.rotation);
                        }
                        //if the boss is EOW, then rotate the flamethrower like this to make it come out of the mouth
                        else if (npc.type == NPCID.EaterofWorldsHead)
                        {
                            velocity = velocity.RotatedBy(npc.rotation + MathHelper.ToRadians(270));
                        }
                        //if the boss is The Destroyer, then make the flamethrower longer, and rotate it like this to make it come out of the mouth
                        else if (npc.type == NPCID.TheDestroyer)
                        {
                            velocity = new Vector2(8, 0).RotatedBy(npc.rotation + MathHelper.ToRadians(270));
                        }
                        //if the boss is Skeletron or Skeletron Prime, then make the flamethrower appear at the chin of the mouth. 
                        else if (npc.type == NPCID.SkeletronHead || npc.type == NPCID.SkeletronPrime)
                        {
                            velocity = velocity.RotatedBy(npc.rotation + MathHelper.ToRadians(90));
                        }
                        //if the boss is a 2D Side boss, then rotate the flamethrower like this to make it come out of the direction that it is facing at
                        else if (npc.type == NPCID.Deerclops || npc.type == NPCID.WallofFlesh || npc.type == NPCID.CultistBoss || npc.type == NPCID.CultistBossClone)
                        {
                            if (npc.direction == -1)
                            {
                                velocity = new Vector2(-velocity.X, velocity.Y);
                            }
                        }
                        //for everyone else, make the flamethower point torwards the player
                        else
                        {
                            Player player = Main.player[npc.target];
                            float deltaX = player.position.X - npc.position.X;
                            float deltaY = npc.position.Y - player.position.Y;
                            velocity = velocity.RotatedBy(-Math.Atan2(deltaY, deltaX));
                        }
                        //check if moon lord is the current npc
                        if (npc.type == NPCID.MoonLordCore || npc.type == NPCID.MoonLordFreeEye || npc.type == NPCID.MoonLordHand || npc.type == NPCID.MoonLordHead || npc.type == NPCID.MoonLordLeechBlob)
                        {
                            //check if moon lord should be allowed to use flamethrowers
                            if (Config.Instance.MoonCursed)
                            {
                                //create the flame thrower
                                Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, velocity, ProjectileID.EyeFire, (int)(npc.damage * Config.Instance.FlameDamMulti) / 4, 0f, Main.myPlayer);
                                if (talked == false)
                                {
                                    //tell them about the flamethrower warning
                                    BF.Talk("Moon Lord " + Language.GetTextValue("Mods.BossFlamethrower.PyroM"), new Color(255, 96, 10));
                                    talked = true;
                                }
                            }
                        }
                        else
                        {
                            //create the flame thrower
                            Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, velocity, ProjectileID.EyeFire, (int)(npc.damage * Config.Instance.FlameDamMulti) / 4, 0f, Main.myPlayer);

                            //check if I already tell them
                            if (talked == false)
                            {
                                //prepare to create the message
                                string message = "";

                                //check if the boss is a known female one
                                if (npc.type == NPCID.QueenBee || npc.type == NPCID.QueenSlimeBoss || npc.type == NPCID.Plantera || npc.type == NPCID.HallowBoss)
                                {
                                    message = npc.FullName + " " + Language.GetTextValue("Mods.BossFlamethrower.PyroF");
                                }
                                else
                                {
                                    message = npc.FullName + " " + Language.GetTextValue("Mods.BossFlamethrower.PyroM");
                                }

                                //talk about the message
                                BF.Talk(message, new Color(255, 96, 10));
                                talked = true;
                            }
                        }
                    }
                }
            }
            base.AI(npc);
        }
    }
}
