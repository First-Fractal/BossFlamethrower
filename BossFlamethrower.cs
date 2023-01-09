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
            if (npc.boss)
            {
                Vector2 velocity = new Vector2(8, 0).RotatedBy(npc.rotation+MathHelper.ToRadians(90));
                Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, velocity, ProjectileID.EyeFire, npc.damage/6, 0f, Main.myPlayer);
            }
            base.AI(npc);
        }
    }
}