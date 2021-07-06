using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001814 RID: 6164
	public class CompProperties_RadialPulse : CompProperties
	{
		// Token: 0x0600886A RID: 34922 RVA: 0x0005B911 File Offset: 0x00059B11
		public CompProperties_RadialPulse()
		{
			this.compClass = typeof(CompRadialPulse);
		}

		// Token: 0x04005787 RID: 22407
		public int ticksBetweenPulses = 300;

		// Token: 0x04005788 RID: 22408
		public int ticksPerPulse = 60;

		// Token: 0x04005789 RID: 22409
		public Color color;

		// Token: 0x0400578A RID: 22410
		public float radius;
	}
}
