using System;

namespace RimWorld
{
	// Token: 0x02001C4E RID: 7246
	[DefOf]
	public static class ScenPartDefOf
	{
		// Token: 0x06009F51 RID: 40785 RVA: 0x0006A1CE File Offset: 0x000683CE
		static ScenPartDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ScenPartDefOf));
		}

		// Token: 0x0400677D RID: 26493
		public static ScenPartDef PlayerFaction;

		// Token: 0x0400677E RID: 26494
		public static ScenPartDef ConfigPage_ConfigureStartingPawns;

		// Token: 0x0400677F RID: 26495
		public static ScenPartDef PlayerPawnsArriveMethod;

		// Token: 0x04006780 RID: 26496
		public static ScenPartDef ForcedTrait;

		// Token: 0x04006781 RID: 26497
		public static ScenPartDef ForcedHediff;

		// Token: 0x04006782 RID: 26498
		public static ScenPartDef StartingAnimal;

		// Token: 0x04006783 RID: 26499
		public static ScenPartDef ScatterThingsNearPlayerStart;

		// Token: 0x04006784 RID: 26500
		public static ScenPartDef StartingThing_Defined;

		// Token: 0x04006785 RID: 26501
		public static ScenPartDef ScatterThingsAnywhere;

		// Token: 0x04006786 RID: 26502
		public static ScenPartDef GameStartDialog;
	}
}
