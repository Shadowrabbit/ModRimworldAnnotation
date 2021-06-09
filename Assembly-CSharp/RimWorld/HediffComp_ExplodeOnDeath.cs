using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DF7 RID: 7671
	public class HediffComp_ExplodeOnDeath : HediffComp
	{
		// Token: 0x17001962 RID: 6498
		// (get) Token: 0x0600A635 RID: 42549 RVA: 0x0006DF65 File Offset: 0x0006C165
		public HediffCompProperties_ExplodeOnDeath Props
		{
			get
			{
				return (HediffCompProperties_ExplodeOnDeath)this.props;
			}
		}

		// Token: 0x0600A636 RID: 42550 RVA: 0x00303184 File Offset: 0x00301384
		public override void Notify_PawnKilled()
		{
			GenExplosion.DoExplosion(base.Pawn.Position, base.Pawn.Map, this.Props.explosionRadius, this.Props.damageDef, base.Pawn, this.Props.damageAmount, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
			if (this.Props.destroyGear)
			{
				base.Pawn.equipment.DestroyAllEquipment(DestroyMode.Vanish);
				base.Pawn.apparel.DestroyAll(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600A637 RID: 42551 RVA: 0x0006DF72 File Offset: 0x0006C172
		public override void Notify_PawnDied()
		{
			if (this.Props.destroyBody)
			{
				base.Pawn.Corpse.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
