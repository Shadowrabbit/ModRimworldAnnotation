using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F23 RID: 3875
	public class CompProperties_UseEffect : CompProperties
	{
		// Token: 0x0600558C RID: 21900 RVA: 0x0003B621 File Offset: 0x00039821
		public CompProperties_UseEffect()
		{
			this.compClass = typeof(CompUseEffect);
		}

		// Token: 0x040036BB RID: 14011
		public bool doCameraShake;

		// Token: 0x040036BC RID: 14012
		public ThingDef moteOnUsed;

		// Token: 0x040036BD RID: 14013
		public float moteOnUsedScale = 1f;
	}
}
