using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200130C RID: 4876
	public static class PawnNeedsUIUtility
	{
		// Token: 0x06007543 RID: 30019 RVA: 0x00283FF7 File Offset: 0x002821F7
		public static void SortInDisplayOrder(List<Need> needs)
		{
			needs.Sort((Need a, Need b) => b.def.listPriority.CompareTo(a.def.listPriority));
		}

		// Token: 0x06007544 RID: 30020 RVA: 0x00284020 File Offset: 0x00282220
		public static Thought GetLeadingThoughtInGroup(List<Thought> thoughtsInGroup)
		{
			Thought result = null;
			int num = -1;
			for (int i = 0; i < thoughtsInGroup.Count; i++)
			{
				if (thoughtsInGroup[i].CurStageIndex > num)
				{
					num = thoughtsInGroup[i].CurStageIndex;
					result = thoughtsInGroup[i];
				}
			}
			return result;
		}

		// Token: 0x06007545 RID: 30021 RVA: 0x00284068 File Offset: 0x00282268
		public static void GetThoughtGroupsInDisplayOrder(Need_Mood mood, List<Thought> outThoughtGroupsPresent)
		{
			mood.thoughts.GetDistinctMoodThoughtGroups(outThoughtGroupsPresent);
			for (int i = outThoughtGroupsPresent.Count - 1; i >= 0; i--)
			{
				if (!outThoughtGroupsPresent[i].VisibleInNeedsTab)
				{
					outThoughtGroupsPresent.RemoveAt(i);
				}
			}
			outThoughtGroupsPresent.SortByDescending((Thought t) => mood.thoughts.MoodOffsetOfGroup(t), (Thought t) => t.GetHashCode());
		}
	}
}
