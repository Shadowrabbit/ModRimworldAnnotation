using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000956 RID: 2390
	public class ThoughtWorker_Precept_IdeoDiversity_Uniform : ThoughtWorker_Precept
	{
		// Token: 0x06003D0A RID: 15626 RVA: 0x00150F88 File Offset: 0x0014F188
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (p.Faction == null || !p.IsColonist)
			{
				return false;
			}
			List<Pawn> list = p.Map.mapPawns.SpawnedPawnsInFaction(p.Faction);
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] != p && list[i].RaceProps.Humanlike && !list[i].IsSlave && !list[i].IsQuestLodger())
				{
					if (list[i].Ideo != p.Ideo)
					{
						return false;
					}
					num++;
				}
			}
			return num > 0;
		}
	}
}
