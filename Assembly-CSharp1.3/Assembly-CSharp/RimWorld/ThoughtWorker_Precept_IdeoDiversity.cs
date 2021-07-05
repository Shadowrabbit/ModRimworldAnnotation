﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000954 RID: 2388
	public class ThoughtWorker_Precept_IdeoDiversity : ThoughtWorker_Precept
	{
		// Token: 0x06003D06 RID: 15622 RVA: 0x00150DF0 File Offset: 0x0014EFF0
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (p.Faction == null || !p.IsColonist)
			{
				return false;
			}
			List<Pawn> list = p.Map.mapPawns.SpawnedPawnsInFaction(p.Faction);
			int i = 0;
			while (i < list.Count)
			{
				if (!list[i].IsQuestLodger() && list[i] != p && list[i].RaceProps.Humanlike && !list[i].IsSlave && list[i].Ideo != p.Ideo)
				{
					int num = 0;
					List<Pawn> list2 = p.Map.mapPawns.SpawnedPawnsInFaction(p.Faction);
					for (int j = 0; j < list2.Count; j++)
					{
						if (!list2[j].IsQuestLodger() && list2[j] != p && list2[j].RaceProps.Humanlike && !list2[j].IsSlave && list2[j].Ideo != p.Ideo)
						{
							num++;
						}
					}
					if (num == 0)
					{
						return ThoughtState.Inactive;
					}
					return ThoughtState.ActiveAtStage(Mathf.RoundToInt((float)num / (float)(list.Count - 1) * (float)(this.def.stages.Count - 1)));
				}
				else
				{
					i++;
				}
			}
			return false;
		}
	}
}
