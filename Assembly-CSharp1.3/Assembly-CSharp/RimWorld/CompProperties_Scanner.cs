using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020011D4 RID: 4564
	public class CompProperties_Scanner : CompProperties
	{
		// Token: 0x06006E2E RID: 28206 RVA: 0x0024F0CC File Offset: 0x0024D2CC
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.scanFindMtbDays <= 0f)
			{
				yield return "scanFindMtbDays not set";
			}
			yield break;
		}

		// Token: 0x04003D25 RID: 15653
		public float scanFindMtbDays;

		// Token: 0x04003D26 RID: 15654
		public float scanFindGuaranteedDays = -1f;

		// Token: 0x04003D27 RID: 15655
		public StatDef scanSpeedStat;

		// Token: 0x04003D28 RID: 15656
		public SoundDef soundWorking;
	}
}
