using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001140 RID: 4416
	public class CompProperties_GiveThoughtToAllMapPawnsOnDestroy : CompProperties
	{
		// Token: 0x06006A0E RID: 27150 RVA: 0x0023B2FF File Offset: 0x002394FF
		public CompProperties_GiveThoughtToAllMapPawnsOnDestroy()
		{
			this.compClass = typeof(CompGiveThoughtToAllMapPawnsOnDestroy);
		}

		// Token: 0x04003B2B RID: 15147
		public ThoughtDef thought;

		// Token: 0x04003B2C RID: 15148
		[MustTranslate]
		public string message;
	}
}
