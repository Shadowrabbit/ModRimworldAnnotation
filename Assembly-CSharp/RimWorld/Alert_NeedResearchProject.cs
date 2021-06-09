using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001965 RID: 6501
	public class Alert_NeedResearchProject : Alert
	{
		// Token: 0x06008FD7 RID: 36823 RVA: 0x000606C4 File Offset: 0x0005E8C4
		public Alert_NeedResearchProject()
		{
			this.defaultLabel = "NeedResearchProject".Translate();
			this.defaultExplanation = "NeedResearchProjectDesc".Translate();
		}

		// Token: 0x06008FD8 RID: 36824 RVA: 0x00296878 File Offset: 0x00294A78
		public override AlertReport GetReport()
		{
			if (Find.AnyPlayerHomeMap == null)
			{
				return false;
			}
			if (Find.ResearchManager.currentProj != null)
			{
				return false;
			}
			bool flag = false;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome && maps[i].listerBuildings.ColonistsHaveResearchBench())
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
			if (!Find.ResearchManager.AnyProjectIsAvailable)
			{
				return false;
			}
			return true;
		}
	}
}
