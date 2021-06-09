using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F80 RID: 3968
	public class ChemicalDef : Def
	{
		// Token: 0x0600570F RID: 22287 RVA: 0x0003C5CA File Offset: 0x0003A7CA
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

		// Token: 0x04003869 RID: 14441
		public HediffDef addictionHediff;

		// Token: 0x0400386A RID: 14442
		public HediffDef toleranceHediff;

		// Token: 0x0400386B RID: 14443
		public bool canBinge = true;

		// Token: 0x0400386C RID: 14444
		public float onGeneratedAddictedToleranceChance;

		// Token: 0x0400386D RID: 14445
		public List<HediffGiver_Event> onGeneratedAddictedEvents;
	}
}
