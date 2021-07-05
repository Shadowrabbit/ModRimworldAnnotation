using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x020005A0 RID: 1440
	public class JobDriver_CastVerbOnce : JobDriver
	{
		// Token: 0x060029F5 RID: 10741 RVA: 0x000FD138 File Offset: 0x000FB338
		public override string GetReport()
		{
			string value;
			if (base.TargetA.HasThing)
			{
				value = base.TargetThingA.LabelCap;
			}
			else
			{
				value = "AreaLower".Translate();
			}
			return "UsingVerb".Translate(this.job.verbToUse.ReportLabel, value);
		}

		// Token: 0x060029F6 RID: 10742 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x060029F7 RID: 10743 RVA: 0x000FD19D File Offset: 0x000FB39D
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Combat.GotoCastPosition(TargetIndex.A, TargetIndex.B, false, 1f);
			yield return Toils_Combat.CastVerb(TargetIndex.A, true);
			yield break;
		}
	}
}
