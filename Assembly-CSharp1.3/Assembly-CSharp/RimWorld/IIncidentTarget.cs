using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BEE RID: 3054
	public interface IIncidentTarget : ILoadReferenceable
	{
		// Token: 0x17000C96 RID: 3222
		// (get) Token: 0x060047DF RID: 18399
		int Tile { get; }

		// Token: 0x17000C97 RID: 3223
		// (get) Token: 0x060047E0 RID: 18400
		StoryState StoryState { get; }

		// Token: 0x17000C98 RID: 3224
		// (get) Token: 0x060047E1 RID: 18401
		GameConditionManager GameConditionManager { get; }

		// Token: 0x17000C99 RID: 3225
		// (get) Token: 0x060047E2 RID: 18402
		float PlayerWealthForStoryteller { get; }

		// Token: 0x17000C9A RID: 3226
		// (get) Token: 0x060047E3 RID: 18403
		IEnumerable<Pawn> PlayerPawnsForStoryteller { get; }

		// Token: 0x17000C9B RID: 3227
		// (get) Token: 0x060047E4 RID: 18404
		FloatRange IncidentPointsRandomFactorRange { get; }

		// Token: 0x17000C9C RID: 3228
		// (get) Token: 0x060047E5 RID: 18405
		int ConstantRandSeed { get; }

		// Token: 0x060047E6 RID: 18406
		IEnumerable<IncidentTargetTagDef> IncidentTargetTags();
	}
}
