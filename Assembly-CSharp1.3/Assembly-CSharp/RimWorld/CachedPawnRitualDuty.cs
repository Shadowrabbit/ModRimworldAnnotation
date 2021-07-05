using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020008CC RID: 2252
	public struct CachedPawnRitualDuty
	{
		// Token: 0x04002046 RID: 8262
		public DutyDef duty;

		// Token: 0x04002047 RID: 8263
		public IntVec3 spot;

		// Token: 0x04002048 RID: 8264
		public Thing usedThing;

		// Token: 0x04002049 RID: 8265
		public Rot4 overrideFacing;

		// Token: 0x0400204A RID: 8266
		public LocalTargetInfo secondFocus;
	}
}
