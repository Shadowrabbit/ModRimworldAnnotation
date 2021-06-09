using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200102A RID: 4138
	public class HistoryAutoRecorderWorker_ColonistMood : HistoryAutoRecorderWorker
	{
		// Token: 0x06005A4C RID: 23116 RVA: 0x001D4FF0 File Offset: 0x001D31F0
		public override float PullRecord()
		{
			List<Pawn> allMaps_FreeColonists = PawnsFinder.AllMaps_FreeColonists;
			if (!allMaps_FreeColonists.Any<Pawn>())
			{
				return 0f;
			}
			return (from x in allMaps_FreeColonists
			where x.needs.mood != null
			select x).Average((Pawn x) => x.needs.mood.CurLevel * 100f);
		}
	}
}
