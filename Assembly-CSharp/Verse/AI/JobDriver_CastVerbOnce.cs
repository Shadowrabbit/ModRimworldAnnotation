using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000986 RID: 2438
	public class JobDriver_CastVerbOnce : JobDriver
	{
		// Token: 0x06003BA2 RID: 15266 RVA: 0x0016E5C8 File Offset: 0x0016C7C8
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

		// Token: 0x06003BA3 RID: 15267 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06003BA4 RID: 15268 RVA: 0x0002DA5E File Offset: 0x0002BC5E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Combat.GotoCastPosition(TargetIndex.A, false, 1f);
			yield return Toils_Combat.CastVerb(TargetIndex.A, true);
			yield break;
		}
	}
}
