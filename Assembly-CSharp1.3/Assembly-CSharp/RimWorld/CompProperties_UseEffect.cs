using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A13 RID: 2579
	public class CompProperties_UseEffect : CompProperties
	{
		// Token: 0x06003F0C RID: 16140 RVA: 0x00157FCC File Offset: 0x001561CC
		public CompProperties_UseEffect()
		{
			this.compClass = typeof(CompUseEffect);
		}

		// Token: 0x0400221A RID: 8730
		public bool doCameraShake;

		// Token: 0x0400221B RID: 8731
		public ThingDef moteOnUsed;

		// Token: 0x0400221C RID: 8732
		public float moteOnUsedScale = 1f;

		// Token: 0x0400221D RID: 8733
		public FleckDef fleckOnUsed;

		// Token: 0x0400221E RID: 8734
		public float fleckOnUsedScale = 1f;
	}
}
