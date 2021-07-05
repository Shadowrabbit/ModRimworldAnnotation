using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000F9 RID: 249
	public class ResearchProjectTagDef : Def
	{
		// Token: 0x060006C8 RID: 1736 RVA: 0x00021010 File Offset: 0x0001F210
		public int CompletedProjects()
		{
			int num = 0;
			List<ResearchProjectDef> allDefsListForReading = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				ResearchProjectDef researchProjectDef = allDefsListForReading[i];
				if (researchProjectDef.IsFinished && researchProjectDef.HasTag(this))
				{
					num++;
				}
			}
			return num;
		}
	}
}
