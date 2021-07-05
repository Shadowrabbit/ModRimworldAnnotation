using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200145F RID: 5215
	[DefOf]
	public static class LogEntryDefOf
	{
		// Token: 0x06007D52 RID: 32082 RVA: 0x002C4B8E File Offset: 0x002C2D8E
		static LogEntryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(LogEntryDefOf));
		}

		// Token: 0x04004D48 RID: 19784
		public static LogEntryDef MeleeAttack;
	}
}
