using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EBC RID: 7868
	public static class QuestGen_Get
	{
		// Token: 0x0600A8F0 RID: 43248 RVA: 0x00314468 File Offset: 0x00312668
		public static Map GetMap(bool mustBeInfestable = false, int? preferMapWithMinFreeColonists = null)
		{
			int minCount = preferMapWithMinFreeColonists ?? 1;
			IEnumerable<Map> source = Find.Maps.Where(delegate(Map x)
			{
				IntVec3 intVec;
				return x.IsPlayerHome && x != null && (!mustBeInfestable || InfestationCellFinder.TryFindCell(out intVec, x));
			});
			Map result;
			if (!(from x in source
			where x.mapPawns.FreeColonists.Count >= minCount
			select x).TryRandomElement(out result))
			{
				(from x in source
				where x.mapPawns.FreeColonists.Any<Pawn>()
				select x).TryRandomElement(out result);
			}
			return result;
		}
	}
}
