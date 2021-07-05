using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018A2 RID: 6306
	public class CompProperties_Scanner : CompProperties
	{
		// Token: 0x06008BFA RID: 35834 RVA: 0x0005DDEB File Offset: 0x0005BFEB
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.scanFindMtbDays <= 0f)
			{
				yield return "scanFindMtbDays not set";
			}
			yield break;
		}

		// Token: 0x040059AB RID: 22955
		public float scanFindMtbDays;

		// Token: 0x040059AC RID: 22956
		public float scanFindGuaranteedDays = -1f;

		// Token: 0x040059AD RID: 22957
		public StatDef scanSpeedStat;

		// Token: 0x040059AE RID: 22958
		public SoundDef soundWorking;
	}
}
