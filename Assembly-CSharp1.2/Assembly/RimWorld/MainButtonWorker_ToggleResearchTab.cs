using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B31 RID: 6961
	public class MainButtonWorker_ToggleResearchTab : MainButtonWorker_ToggleTab
	{
		// Token: 0x17001830 RID: 6192
		// (get) Token: 0x06009952 RID: 39250 RVA: 0x002D0B90 File Offset: 0x002CED90
		public override float ButtonBarPercent
		{
			get
			{
				ResearchProjectDef currentProj = Find.ResearchManager.currentProj;
				if (currentProj == null)
				{
					return 0f;
				}
				return currentProj.ProgressPercent;
			}
		}
	}
}
