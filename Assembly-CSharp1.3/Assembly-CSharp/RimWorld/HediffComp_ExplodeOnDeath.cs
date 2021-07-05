using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200156E RID: 5486
	public class HediffComp_ExplodeOnDeath : HediffComp
	{
		// Token: 0x170015F4 RID: 5620
		// (get) Token: 0x060081CD RID: 33229 RVA: 0x002DE1DC File Offset: 0x002DC3DC
		public HediffCompProperties_ExplodeOnDeath Props
		{
			get
			{
				return (HediffCompProperties_ExplodeOnDeath)this.props;
			}
		}

		// Token: 0x060081CE RID: 33230 RVA: 0x002DE1EC File Offset: 0x002DC3EC
		public override void Notify_PawnKilled()
		{
			GenExplosion.DoExplosion(base.Pawn.Position, base.Pawn.Map, this.Props.explosionRadius, this.Props.damageDef, base.Pawn, this.Props.damageAmount, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
			if (this.Props.destroyGear)
			{
				base.Pawn.equipment.DestroyAllEquipment(DestroyMode.Vanish);
				base.Pawn.apparel.DestroyAll(DestroyMode.Vanish);
			}
		}

		// Token: 0x060081CF RID: 33231 RVA: 0x002DE292 File Offset: 0x002DC492
		public override void Notify_PawnDied()
		{
			if (this.Props.destroyBody)
			{
				base.Pawn.Corpse.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
