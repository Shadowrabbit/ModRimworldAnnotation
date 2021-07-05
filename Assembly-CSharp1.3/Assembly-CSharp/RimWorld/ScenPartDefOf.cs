using System;

namespace RimWorld
{
	// Token: 0x0200140E RID: 5134
	[DefOf]
	public static class ScenPartDefOf
	{
		// Token: 0x06007D01 RID: 32001 RVA: 0x002C462D File Offset: 0x002C282D
		static ScenPartDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ScenPartDefOf));
		}

		// Token: 0x040047A8 RID: 18344
		public static ScenPartDef PlayerFaction;

		// Token: 0x040047A9 RID: 18345
		public static ScenPartDef ConfigPage_ConfigureStartingPawns;

		// Token: 0x040047AA RID: 18346
		public static ScenPartDef PlayerPawnsArriveMethod;

		// Token: 0x040047AB RID: 18347
		public static ScenPartDef ForcedTrait;

		// Token: 0x040047AC RID: 18348
		public static ScenPartDef ForcedHediff;

		// Token: 0x040047AD RID: 18349
		public static ScenPartDef StartingAnimal;

		// Token: 0x040047AE RID: 18350
		public static ScenPartDef ScatterThingsNearPlayerStart;

		// Token: 0x040047AF RID: 18351
		public static ScenPartDef StartingThing_Defined;

		// Token: 0x040047B0 RID: 18352
		public static ScenPartDef ScatterThingsAnywhere;

		// Token: 0x040047B1 RID: 18353
		public static ScenPartDef GameStartDialog;
	}
}
