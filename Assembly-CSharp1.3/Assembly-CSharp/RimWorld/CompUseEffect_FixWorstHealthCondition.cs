using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001209 RID: 4617
	public class CompUseEffect_FixWorstHealthCondition : CompUseEffect
	{
		// Token: 0x06006EED RID: 28397 RVA: 0x002515F9 File Offset: 0x0024F7F9
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			HealthUtility.FixWorstHealthCondition(usedBy);
		}
	}
}
