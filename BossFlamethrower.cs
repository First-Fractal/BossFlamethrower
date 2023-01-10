using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader.Config;
using System.ComponentModel;

namespace BossFlamethrower
{
    public class BossFlamethrower : Mod
    {
    }

    public class GlobalBossFlamethrower : GlobalNPC
    {
        static float cooldownMax = Config.Instance.FlamethrowerCooldown;
        static float durationMax = Config.Instance.FlamethrowerDuration;
        static float cooldown = cooldownMax;
        static float duration = durationMax;
        static bool flame = false;

        public override void AI(NPC npc)
        {
            //check to see if the max time is set correctly
            cooldownMax = Config.Instance.FlamethrowerCooldown;
            durationMax = Config.Instance.FlamethrowerDuration;
            if (Config.Instance.UseMinutes)
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

                    //dont give moon lord a flamethrower, due to the lighting hiding it, and dont give spazmatism a flame thrower, since he already has one
                    if (npc.type != NPCID.MoonLordCore && npc.type != NPCID.MoonLordFreeEye && npc.type != NPCID.MoonLordHand && npc.type != NPCID.MoonLordHead && npc.type != NPCID.MoonLordLeechBlob && npc.type != NPCID.Spazmatism)
                    {
                        //set the length of the flamethrower
                        Vector2 velocity = new Vector2(4, 0);
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
                            velocity = velocity.RotatedBy(npc.rotation * npc.direction);
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
                        //create the flame thrower
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, velocity, ProjectileID.EyeFire, npc.damage / 4, 0f, Main.myPlayer);
                    }
                }
                else
                {
                    //countdown the cooldown till flame time
                    cooldown--;
                    //Main.NewText("The cooldown of the flamethrower is currently at " + cooldown.ToString());
                    if (cooldown < 0)
                    {
                        flame = true;
                        cooldown = cooldownMax;

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
                    //check if the npc is a boss, and if so, then remove the cursed flame
                    if (npc.boss || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.CultistBossClone)
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
    }
}