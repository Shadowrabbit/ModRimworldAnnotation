using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x020005F2 RID: 1522
	public class MentalState_SadisticRageTantrum : MentalState_TantrumRandom
	{
		// Token: 0x06002BCC RID: 11212 RVA: 0x00104A5F File Offset: 0x00102C5F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.hits, "hits", 0, false);
		}

		// Token: 0x06002BCD RID: 11213 RVA: 0x00104A79 File Offset: 0x00102C79
		protected override void GetPotentialTargets(List<Thing> outThings)
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, outThings, this.GetCustomValidator(), 0, 40);
		}

		// Token: 0x06002BCE RID: 11214 RVA: 0x00104A9B File Offset: 0x00102C9B
		protected override Predicate<Thing> GetCustomValidator()
		{
			return (Thing x) => TantrumMentalStateUtility.CanAttackPrisoner(this.pawn, x);
		}

		// Token: 0x06002BCF RID: 11215 RVA: 0x00104AA9 File Offset: 0x00102CA9
		public override void Notify_AttackedTarget(LocalTargetInfo hitTarget)
		{
			base.Notify_AttackedTarget(hitTarget);
			if (this.target != null && hitTarget.Thing == this.target)
			{
				this.hits++;
				if (this.hits >= 7)
				{
					base.RecoverFromState();
				}
			}
		}

		// Token: 0x04001AA0 RID: 6816
		private int hits;

		// Token: 0x04001AA1 RID: 6817
		public const int MaxHits = 7;
	}
}
