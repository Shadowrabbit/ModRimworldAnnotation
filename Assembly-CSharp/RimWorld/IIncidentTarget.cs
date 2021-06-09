using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001187 RID: 4487
	public interface IIncidentTarget : ILoadReferenceable
	{
		// Token: 0x17000F82 RID: 3970
		// (get) Token: 0x060062E8 RID: 25320
		int Tile { get; }

		// Token: 0x17000F83 RID: 3971
		// (get) Token: 0x060062E9 RID: 25321
		StoryState StoryState { get; }

		// Token: 0x17000F84 RID: 3972
		// (get) Token: 0x060062EA RID: 25322
		GameConditionManager GameConditionManager { get; }

		// Token: 0x17000F85 RID: 3973
		// (get) Token: 0x060062EB RID: 25323
		float PlayerWealthForStoryteller { get; }

		// Token: 0x17000F86 RID: 3974
		// (get) Token: 0x060062EC RID: 25324
		IEnumerable<Pawn> PlayerPawnsForStoryteller { get; }

		// Token: 0x17000F87 RID: 3975
		// (get) Token: 0x060062ED RID: 25325
		FloatRange IncidentPointsRandomFactorRange { get; }

		// Token: 0x17000F88 RID: 3976
		// (get) Token: 0x060062EE RID: 25326
		int ConstantRandSeed { get; }

		// Token: 0x060062EF RID: 25327
		IEnumerable<IncidentTargetTagDef> IncidentTargetTags();
	}
}
