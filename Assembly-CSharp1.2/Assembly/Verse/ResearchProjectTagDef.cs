using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000179 RID: 377
	public class ResearchProjectTagDef : Def
	{
		// Token: 0x06000991 RID: 2449 RVA: 0x00099AE0 File Offset: 0x00097CE0
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
