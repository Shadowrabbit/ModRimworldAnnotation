using System;

namespace RimWorld
{
	// Token: 0x0200147B RID: 5243
	[DefOf]
	public static class RitualVisualEffectDefOf
	{
		// Token: 0x06007D6D RID: 32109 RVA: 0x002C4D59 File Offset: 0x002C2F59
		static RitualVisualEffectDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RitualVisualEffectDefOf));
		}

		// Token: 0x04004E39 RID: 20025
		[MayRequireIdeology]
		public static RitualVisualEffectDef Basic;
	}
}
