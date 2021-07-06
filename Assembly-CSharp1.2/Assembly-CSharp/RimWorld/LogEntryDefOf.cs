using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C9F RID: 7327
	[DefOf]
	public static class LogEntryDefOf
	{
		// Token: 0x06009FA2 RID: 40866 RVA: 0x0006A72F File Offset: 0x0006892F
		static LogEntryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(LogEntryDefOf));
		}

		// Token: 0x04006C55 RID: 27733
		public static LogEntryDef MeleeAttack;
	}
}
