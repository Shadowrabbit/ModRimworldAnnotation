using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001361 RID: 4961
	public class MainButtonWorker_ToggleResearchTab : MainButtonWorker_ToggleTab
	{
		// Token: 0x1700152D RID: 5421
		// (get) Token: 0x06007860 RID: 30816 RVA: 0x002A6EAC File Offset: 0x002A50AC
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
