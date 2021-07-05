using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200095F RID: 2399
	public class ThoughtWorker_Precept_NoRecentHumanMeat : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
	{
		// Token: 0x06003D22 RID: 15650 RVA: 0x00151480 File Offset: 0x0014F680
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			int num = Mathf.Max(0, p.mindState.lastHumanMeatIngestedTick);
			return Find.TickManager.TicksGame - num > 480000;
		}

		// Token: 0x06003D23 RID: 15651 RVA: 0x001514B7 File Offset: 0x0014F6B7
		public IEnumerable<NamedArgument> GetDescriptionArgs()
		{
			yield return 8.Named("HUMANMEATREQUIREDINTERVAL");
			yield break;
		}

		// Token: 0x040020CC RID: 8396
		public const int MinDaysSinceLastHumanMeatForThought = 8;
	}
}
