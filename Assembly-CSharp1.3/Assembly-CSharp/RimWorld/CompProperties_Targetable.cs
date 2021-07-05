using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A0F RID: 2575
	public class CompProperties_Targetable : CompProperties_UseEffect
	{
		// Token: 0x06003F08 RID: 16136 RVA: 0x00157F0F File Offset: 0x0015610F
		public CompProperties_Targetable()
		{
			this.compClass = typeof(CompTargetable);
		}

		// Token: 0x04002201 RID: 8705
		public bool psychicSensitiveTargetsOnly;

		// Token: 0x04002202 RID: 8706
		public bool fleshCorpsesOnly;

		// Token: 0x04002203 RID: 8707
		public bool nonDessicatedCorpsesOnly;

		// Token: 0x04002204 RID: 8708
		public bool nonDownedPawnOnly;

		// Token: 0x04002205 RID: 8709
		public bool ignoreQuestLodgerPawns;

		// Token: 0x04002206 RID: 8710
		public bool ignorePlayerFactionPawns;

		// Token: 0x04002207 RID: 8711
		public ThingDef moteOnTarget;

		// Token: 0x04002208 RID: 8712
		public ThingDef moteConnecting;

		// Token: 0x04002209 RID: 8713
		public FleckDef fleckOnTarget;

		// Token: 0x0400220A RID: 8714
		public FleckDef fleckConnecting;
	}
}
