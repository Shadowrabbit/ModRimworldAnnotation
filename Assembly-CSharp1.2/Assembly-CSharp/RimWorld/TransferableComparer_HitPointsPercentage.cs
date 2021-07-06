using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BB5 RID: 7093
	public class TransferableComparer_HitPointsPercentage : TransferableComparer
	{
		// Token: 0x06009C3A RID: 39994 RVA: 0x002DDAA4 File Offset: 0x002DBCA4
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return this.GetValueFor(lhs).CompareTo(this.GetValueFor(rhs));
		}

		// Token: 0x06009C3B RID: 39995 RVA: 0x002DDAC8 File Offset: 0x002DBCC8
		private float GetValueFor(Transferable t)
		{
			Thing anyThing = t.AnyThing;
			Pawn pawn = anyThing as Pawn;
			if (pawn != null)
			{
				return pawn.health.summaryHealth.SummaryHealthPercent;
			}
			if (!anyThing.def.useHitPoints || !anyThing.def.healthAffectsPrice)
			{
				return 1f;
			}
			return (float)anyThing.HitPoints / (float)anyThing.MaxHitPoints;
		}
	}
}
