using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D6A RID: 3434
	public class CompProperties_AbilityStartRitualOnPawn : CompProperties_AbilityStartRitual
	{
		// Token: 0x06004FBB RID: 20411 RVA: 0x001AAF1F File Offset: 0x001A911F
		public CompProperties_AbilityStartRitualOnPawn()
		{
			this.compClass = typeof(CompAbilityEffect_StartRitualOnPawn);
		}

		// Token: 0x04002FC5 RID: 12229
		[NoTranslate]
		public string targetRoleId;
	}
}
