using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001275 RID: 4725
	public class Alert_NeedResearchProject : Alert
	{
		// Token: 0x06007109 RID: 28937 RVA: 0x0025A9EE File Offset: 0x00258BEE
		public Alert_NeedResearchProject()
		{
			this.defaultLabel = "NeedResearchProject".Translate();
			this.defaultExplanation = "NeedResearchProjectDesc".Translate();
		}

		// Token: 0x0600710A RID: 28938 RVA: 0x0025AA20 File Offset: 0x00258C20
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
