using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001177 RID: 4471
	public class CompProperties_RadialPulse : CompProperties
	{
		// Token: 0x06006B74 RID: 27508 RVA: 0x00241615 File Offset: 0x0023F815
		public CompProperties_RadialPulse()
		{
			this.compClass = typeof(CompRadialPulse);
		}

		// Token: 0x04003BC3 RID: 15299
		public int ticksBetweenPulses = 300;

		// Token: 0x04003BC4 RID: 15300
		public int ticksPerPulse = 60;

		// Token: 0x04003BC5 RID: 15301
		public Color color;

		// Token: 0x04003BC6 RID: 15302
		public float radius;
	}
}
