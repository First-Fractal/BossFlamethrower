using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace BossFlamethrower
{
    public class BossFlamethrower : Mod
    {
    }

    public class GlobalBossFlamethrower : GlobalNPC
    {
        static float cooldownMax = 1f;
        static float cooldown = cooldownMax;
        static float durationMax = 100000 * 60f;
        static float duration = durationMax;
        static bool flame = false;
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            npc.buffImmune[BuffID.CursedInferno] = true;
            base.OnSpawn(npc, source);
        }

        public override void AI(NPC npc)
        {
            // get all of the bosses and boss parts
            if (npc.boss || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.CultistBossClone)
            {
                if (flame)
                {
                    //slowly decrease the time and stop when times up
                    duration--;
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
                        //if the boss is a eye, boss then rotate the flamethrower it like this to make it come out of the eye
                        if (npc.type == NPCID.EyeofCthulhu || npc.type == NPCID.Retinazer)
                        {
                            velocity = velocity.RotatedBy(npc.rotation + MathHelper.ToRadians(90));
                        }
                        //if the boss is a plantera, then rotate the flamethrower it like this to make it come out of the mouth
                        else if (npc.type == NPCID.Plantera)
                        {
                            velocity = velocity.RotatedBy(npc.rotation + MathHelper.ToRadians(270));
                        }
                        //if the boss is a duke fishron, then rotate the flamethrower it like this to make it come out of the mouth
                        else if (npc.type == NPCID.DukeFishron)
                        {
                            velocity = velocity.RotatedBy(npc.rotation * npc.direction);
                        }
                        //if the boss is a worm boss, then rotate the flamethrower it like this to make it come out of the mouth
                        else if (npc.type == NPCID.TheDestroyer || npc.type == NPCID.EaterofWorldsHead)
                        {
                            velocity = velocity.RotatedBy(npc.rotation + MathHelper.ToRadians(270));
                        }

                        //if the boss is a 2D Side boss, then rotate the flamethrower it like this to make it come out of the direction that it is facing at
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
                    if (cooldown < 0)
                    {
                        flame = true;
                        cooldown = cooldownMax;

                    }
                }
            }
            base.AI(npc);
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
                    //check if the npc is a boss, and if so, then remove the cursed inferno
                    if (npc.boss || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.CultistBossClone)
                    {
                        player.ClearBuff(BuffID.CursedInferno);
                    }
                }
            }
            base.Update(type, player, ref buffIndex);
        }
    }
}