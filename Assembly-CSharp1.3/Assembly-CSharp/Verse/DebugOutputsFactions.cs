using System;

namespace Verse
{
	// Token: 0x020003A9 RID: 937
	public class DebugOutputsFactions
	{
		// Token: 0x06001CB2 RID: 7346 RVA: 0x000AC036 File Offset: 0x000AA236
		[DebugOutput("Factions", false)]
		public static void AllFactions()
		{
			Find.FactionManager.LogAllFactions();
		}

		// Token: 0x06001CB3 RID: 7347 RVA: 0x000AC042 File Offset: 0x000AA242
		[DebugOutput("Factions", false)]
		public static void AllFactionsToRemove()
		{
			Find.FactionManager.LogFactionsToRemove();
		}

		// Token: 0x06001CB4 RID: 7348 RVA: 0x000AC04E File Offset: 0x000AA24E
		[DebugOutput("Factions", false)]
		public static void AllFactionsFromPawns()
		{
			Find.FactionManager.LogFactionsOnPawns();
		}
	}
}
