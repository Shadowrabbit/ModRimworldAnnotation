using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002A8 RID: 680
	public class HediffComp_Invisibility : HediffComp
	{
		// Token: 0x060012A6 RID: 4774 RVA: 0x0006B425 File Offset: 0x00069625
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.UpdateTarget();
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x0006B434 File Offset: 0x00069634
		public override void CompPostPostRemoved()
		{
			base.CompPostPostRemoved();
			this.UpdateTarget();
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x0006B444 File Offset: 0x00069644
		private void UpdateTarget()
		{
			if (!ModLister.CheckRoyalty("Invisibility hediff"))
			{
				return;
			}
			Pawn pawn = this.parent.pawn;
			if (pawn.Spawned)
			{
				pawn.Map.attackTargetsCache.UpdateTarget(pawn);
			}
			PortraitsCache.SetDirty(pawn);
			GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(pawn);
		}
	}
}
