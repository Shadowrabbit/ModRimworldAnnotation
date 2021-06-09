using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001726 RID: 5926
	public class Projectile_DoomsdayRocket : Projectile
	{
		// Token: 0x060082B8 RID: 33464 RVA: 0x0026C360 File Offset: 0x0026A560
		protected override void Impact(Thing hitThing)
		{
			Map map = base.Map;
			base.Impact(hitThing);
			IntVec3 position = base.Position;
			Map map2 = map;
			float explosionRadius = this.def.projectile.explosionRadius;
			DamageDef bomb = DamageDefOf.Bomb;
			Thing launcher = this.launcher;
			int damageAmount = base.DamageAmount;
			float armorPenetration = base.ArmorPenetration;
			SoundDef explosionSound = null;
			ThingDef equipmentDef = this.equipmentDef;
			ThingDef def = this.def;
			ThingDef filth_Fuel = ThingDefOf.Filth_Fuel;
			GenExplosion.DoExplosion(position, map2, explosionRadius, bomb, launcher, damageAmount, armorPenetration, explosionSound, equipmentDef, def, this.intendedTarget.Thing, filth_Fuel, 0.2f, 1, false, null, 0f, 1, 0.4f, false, null, null);
			CellRect cellRect = CellRect.CenteredOn(base.Position, 5);
			cellRect.ClipInsideMap(map);
			for (int i = 0; i < 3; i++)
			{
				IntVec3 randomCell = cellRect.RandomCell;
				this.DoFireExplosion(randomCell, map, 3.9f);
			}
		}

		// Token: 0x060082B9 RID: 33465 RVA: 0x0026C428 File Offset: 0x0026A628
		protected void DoFireExplosion(IntVec3 pos, Map map, float radius)
		{
			GenExplosion.DoExplosion(pos, map, radius, DamageDefOf.Flame, this.launcher, base.DamageAmount, base.ArmorPenetration, null, this.equipmentDef, this.def, this.intendedTarget.Thing, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
		}

		// Token: 0x040054BD RID: 21693
		private const int ExtraExplosionCount = 3;

		// Token: 0x040054BE RID: 21694
		private const int ExtraExplosionRadius = 5;
	}
}
