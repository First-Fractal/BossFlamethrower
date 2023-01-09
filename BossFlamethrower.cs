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

	public class GlobalBossFlamethrower: GlobalNPC
	{
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            npc.buffImmune[BuffID.CursedInferno] = true;
            base.OnSpawn(npc, source);
        }

        public override void AI(NPC npc)
        {
            // get all of the bosses in the list
            if (npc.boss || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.CultistBossClone)
            {
                //dont give moon lord a flamethrower, due to the lighting hiding it
                if (npc.type != NPCID.MoonLordCore && npc.type != NPCID.MoonLordFreeEye && npc.type != NPCID.MoonLordHand && npc.type != NPCID.MoonLordHead && npc.type != NPCID.MoonLordLeechBlob)
                {
                    //set the length of the flamethrower
                    Vector2 velocity = new Vector2(6, 0);
                    //if the boss is a eye, boss then rotate the flamethrower it like this to make it come out of the eye
                    if (npc.type == NPCID.EyeofCthulhu || npc.type == NPCID.Retinazer || npc.type == NPCID.Spazmatism)
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
                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, velocity, ProjectileID.EyeFire, npc.damage / 6, 0f, Main.myPlayer);
                }
            }
            base.AI(npc);
        }
    }
}