using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A37 RID: 2615
	public class MentalState_SadisticRageTantrum : MentalState_TantrumRandom
	{
		// Token: 0x06003E5B RID: 15963 RVA: 0x0002EDFD File Offset: 0x0002CFFD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.hits, "hits", 0, false);
		}

		// Token: 0x06003E5C RID: 15964 RVA: 0x0002EE17 File Offset: 0x0002D017
		protected override void GetPotentialTargets(List<Thing> outThings)
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, outThings, this.GetCustomValidator(), 0, 40);
		}

		// Token: 0x06003E5D RID: 15965 RVA: 0x0002EE39 File Offset: 0x0002D039
		protected override Predicate<Thing> GetCustomValidator()
		{
			return (Thing x) => TantrumMentalStateUtility.CanAttackPrisoner(this.pawn, x);
		}

		// Token: 0x06003E5E RID: 15966 RVA: 0x0002EE47 File Offset: 0x0002D047
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

		// Token: 0x04002AFA RID: 11002
		private int hits;

		// Token: 0x04002AFB RID: 11003
		public const int MaxHits = 7;
	}
}
