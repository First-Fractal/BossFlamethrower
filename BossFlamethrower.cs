using Terraria;
using Terraria.ID;
using Terraria.Chat;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.ModLoader.Config;
using System;
using System.ComponentModel;
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
    }

    public class GlobalBossFlamethrower : GlobalNPC
    {
        static float cooldownMax = Config.Instance.FlamethrowerCooldown;
        static float durationMax = Config.Instance.FlamethrowerDuration;
        static float cooldown = cooldownMax;
        static float duration = durationMax;
        static bool flame = false;
        static bool talked = false;

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (Config.Instance.MoonCursed)
            {
                if (npc.type == NPCID.MoonLordCore)
                {
                    BossFlamethrower BF = new BossFlamethrower();
                    BF.Talk(Language.GetTextValue("Mods.BossFlamethrower.Warning"), Color.Cyan);
                }
            }
            base.OnSpawn(npc, source);
        }

        public override void AI(NPC npc)
        {
            //check to see if the max time is set correctly
            cooldownMax = Config.Instance.FlamethrowerCooldown;
            durationMax = Config.Instance.FlamethrowerDuration;
            if (Config.Instance.UseMinutes)
            {
                cooldownMax = Config.Instance.FlamethrowerCooldown * 60f * 60f;
                durationMax = Config.Instance.FlamethrowerDuration * 60f * 60f;
            } else
            {
                cooldownMax = Config.Instance.FlamethrowerCooldown * 60f;
                durationMax = Config.Instance.FlamethrowerDuration * 60f;
            }

            //check if the timer is bigger then the max time (such as lower down the max in the config), then set it correctly
            if (cooldown > cooldownMax)
            {
                cooldown = cooldownMax;
            }
            if (duration > durationMax)
            {
                duration = durationMax;
            }

            // get all of the bosses and boss parts
            if (npc.boss || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.CultistBossClone)
            {
                if (flame)
                {
                    //slowly decrease the time and stop when times up
                    duration--;
                    //Main.NewText("The duration of the flamethrower is currently at " + duration.ToString());
                    if (duration < 0)
                    {
                        flame = false;
                        duration = durationMax;
                    }

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
                                Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, velocity, ProjectileID.EyeFire, npc.damage / 4, 0f, Main.myPlayer);
                            }
                        } else
                        {
                            //create the flame thrower
                            Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, velocity, ProjectileID.EyeFire, npc.damage / 4, 0f, Main.myPlayer);

                            if (talked == false)
                            {
                                BossFlamethrower BF = new BossFlamethrower();
                                string message = "";
                                if (npc.type == NPCID.QueenBee || npc.type == NPCID.QueenSlimeBoss || npc.type == NPCID.Plantera || npc.type == NPCID.HallowBoss)
                                {
                                    message = npc.FullName + " " + Language.GetTextValue("Mods.BossFlamethrower.PyroF");
                                } else
                                {
                                    message = npc.FullName + " " + Language.GetTextValue("Mods.BossFlamethrower.PyroM");
                                }

                                BF.Talk(message, new Color(255, 96, 10));
                                talked = true;
                            }
                        }
                    }
                }
                else
                {
                    //countdown the cooldown till flame time
                    cooldown--;
                    if (cooldown < 0)
                    {
                        flame = true;
                        cooldown = cooldownMax;
                        talked = false;
                    }
                }
            }
            base.AI(npc);
        }

        //reset the cooldown 
        public override void OnKill(NPC npc)
        {
            if (npc.boss || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.CultistBossClone)
            {
                if (npc.type != NPCID.MoonLordCore && npc.type != NPCID.MoonLordFreeEye && npc.type != NPCID.MoonLordHand && npc.type != NPCID.MoonLordHead && npc.type != NPCID.MoonLordLeechBlob && npc.type != NPCID.Spazmatism)
                    {
                        cooldown = cooldownMax;
                        duration = durationMax;
                    }
            }
            base.OnKill(npc);
        }
    }

    public class GlobalBuffFlamethrower : GlobalBuff
    {
        public override void Update(int type, Player player, ref int buffIndex)
        {
            //check if the buff is from the player
            if (type == BuffID.CursedInferno)
            {
                //get all of the current npcs
                foreach(NPC npc in Main.npc)
                {
                    //remove the debuff, depending on the config setup
                    if (Config.Instance.UseCursed == false)
                    {
                        player.ClearBuff(BuffID.CursedInferno);
                    } else if (Main.hardMode == false && Config.Instance.CursedPHM == false)
                    {
                        player.ClearBuff(BuffID.CursedInferno);
                    }

                }
            }
            base.Update(type, player, ref buffIndex);
        }
    }

    [Label("$Mods.BossFlamethrower.Config.Label")]
    public class Config : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;
        public static Config Instance;

        [Header("$Mods.BossFlamethrower.Config.Header.GeneralOptions")]

        [Label("$Mods.BossFlamethrower.Config.FlamethrowerCooldown.Label")]
        [Tooltip("$Mods.BossFlamethrower.Config.FlamethrowerCooldown.Tooltip")]
        [DefaultValue(30)]
        public int FlamethrowerCooldown;

        [Label("$Mods.BossFlamethrower.Config.FlamethrowerDuration.Label")]
        [Tooltip("$Mods.BossFlamethrower.Config.FlamethrowerDuration.Tooltip")]
        [DefaultValue(10)]
        public int FlamethrowerDuration;

        [Label("$Mods.BossFlamethrower.Config.UseMinutes.Label")]
        [Tooltip("$Mods.BossFlamethrower.Config.UseMinutes.Tooltip")]
        [DefaultValue(false)]
        public bool UseMinutes;

        [Label("$Mods.BossFlamethrower.Config.CursedPHM.Label")]
        [Tooltip("$Mods.BossFlamethrower.Config.CursedPHM.Tooltip")]
        [DefaultValue(true)]
        public bool CursedPHM;

        [Label("$Mods.BossFlamethrower.Config.UseCursed.Label")]
        [Tooltip("$Mods.BossFlamethrower.Config.UseCursed.Tooltip")]
        [DefaultValue(true)]
        public bool UseCursed;

        [Label("$Mods.BossFlamethrower.Config.FlameDis.Label")]
        [Tooltip("$Mods.BossFlamethrower.Config.FlameDis.Tooltip")]
        [DefaultValue(6f)]
        [Range(1f, 25f)]
        public float FlameDis;

        [Label("$Mods.BossFlamethrower.Config.MoonCursed.Label")]
        [Tooltip("$Mods.BossFlamethrower.Config.MoonCursed.Tooltip")]
        [DefaultValue(false)]
        public bool MoonCursed;
    }
}