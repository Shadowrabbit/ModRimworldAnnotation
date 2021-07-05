using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001613 RID: 5651
	public static class QuestGen_Get
	{
		// Token: 0x06008469 RID: 33897 RVA: 0x002F7A10 File Offset: 0x002F5C10
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
