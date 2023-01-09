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
        public override void AI(NPC npc)
        {
            if (npc.boss && npc.type != NPCID.Spazmatism)
            {
                float num465 = 6f;
                int attackDamage_ForProjectiles7 = npc.GetAttackDamage_ForProjectiles(30f, 27f);
                int num466 = 85;
                Vector2 vector43 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                float num462 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector43.X;
                float num463 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector43.Y;
                float num464 = (float)Math.Sqrt(num462 * num462 + num463 * num463);
                num464 = num465 / num464;
                num462 *= num464;
                num463 *= num464;
                num463 += (float)Main.rand.Next(-40, 41) * 0.01f;
                num462 += (float)Main.rand.Next(-40, 41) * 0.01f;
                num463 += npc.velocity.Y * 0.5f;
                num462 += npc.velocity.X * 0.5f;
                vector43.X -= num462 * 1f;
                vector43.Y -= num463 * 1f;
                int num467 = Projectile.NewProjectile(npc.GetSource_FromAI(), vector43.X, vector43.Y, num462, num463, num466, attackDamage_ForProjectiles7, 0f, Main.myPlayer);
            }
            base.AI(npc);
        }
    }
}