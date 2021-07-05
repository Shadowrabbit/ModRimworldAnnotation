using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A56 RID: 2646
	public class ChemicalDef : Def
	{
		// Token: 0x06003FB8 RID: 16312 RVA: 0x00159EE5 File Offset: 0x001580E5
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.addictionHediff == null)
			{
				yield return "addictionHediff is null";
			}
			yield break;
			yield break;
		}

		// Token: 0x0400235B RID: 9051
		public HediffDef addictionHediff;

		// Token: 0x0400235C RID: 9052
		public HediffDef toleranceHediff;

		// Token: 0x0400235D RID: 9053
		public bool canBinge = true;

		// Token: 0x0400235E RID: 9054
		public float onGeneratedAddictedToleranceChance;

		// Token: 0x0400235F RID: 9055
		public List<HediffGiver_Event> onGeneratedAddictedEvents;
	}
}
