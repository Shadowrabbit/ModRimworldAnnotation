using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AFE RID: 2814
	public class HistoryAutoRecorderWorker_ColonistMood : HistoryAutoRecorderWorker
	{
		// Token: 0x06004241 RID: 16961 RVA: 0x0016308C File Offset: 0x0016128C
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
