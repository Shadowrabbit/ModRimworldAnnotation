using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017CA RID: 6090
	public class CompProperties_GiveThoughtToAllMapPawnsOnDestroy : CompProperties
	{
		// Token: 0x060086B4 RID: 34484 RVA: 0x0005A5EF File Offset: 0x000587EF
		public CompProperties_GiveThoughtToAllMapPawnsOnDestroy()
		{
			this.compClass = typeof(CompGiveThoughtToAllMapPawnsOnDestroy);
		}

		// Token: 0x040056A1 RID: 22177
		public ThoughtDef thought;

		// Token: 0x040056A2 RID: 22178
		[MustTranslate]
		public string message;
	}
}
