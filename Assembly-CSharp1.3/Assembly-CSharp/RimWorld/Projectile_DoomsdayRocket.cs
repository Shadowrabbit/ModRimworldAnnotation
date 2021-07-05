using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010C4 RID: 4292
	public class Projectile_DoomsdayRocket : Projectile
	{
		// Token: 0x170011A0 RID: 4512
		// (get) Token: 0x060066B8 RID: 26296 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool AnimalsFleeImpact
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060066B9 RID: 26297 RVA: 0x0022B39C File Offset: 0x0022959C
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

		// Token: 0x060066BA RID: 26298 RVA: 0x0022B464 File Offset: 0x00229664
		protected void DoFireExplosion(IntVec3 pos, Map map, float radius)
		{
			GenExplosion.DoExplosion(pos, map, radius, DamageDefOf.Flame, this.launcher, base.DamageAmount, base.ArmorPenetration, null, this.equipmentDef, this.def, this.intendedTarget.Thing, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
		}

		// Token: 0x040039FC RID: 14844
		private const int ExtraExplosionCount = 3;

		// Token: 0x040039FD RID: 14845
		private const int ExtraExplosionRadius = 5;
	}
}
