using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F1F RID: 3871
	public class CompProperties_Targetable : CompProperties_UseEffect
	{
		// Token: 0x06005588 RID: 21896 RVA: 0x0003B5BF File Offset: 0x000397BF
		public CompProperties_Targetable()
		{
			this.compClass = typeof(CompTargetable);
		}

		// Token: 0x040036A4 RID: 13988
		public bool psychicSensitiveTargetsOnly;

		// Token: 0x040036A5 RID: 13989
		public bool fleshCorpsesOnly;

		// Token: 0x040036A6 RID: 13990
		public bool nonDessicatedCorpsesOnly;

		// Token: 0x040036A7 RID: 13991
		public bool nonDownedPawnOnly;

		// Token: 0x040036A8 RID: 13992
		public bool ignoreQuestLodgerPawns;

		// Token: 0x040036A9 RID: 13993
		public bool ignorePlayerFactionPawns;

		// Token: 0x040036AA RID: 13994
		public ThingDef moteOnTarget;

		// Token: 0x040036AB RID: 13995
		public ThingDef moteConnecting;
	}
}
