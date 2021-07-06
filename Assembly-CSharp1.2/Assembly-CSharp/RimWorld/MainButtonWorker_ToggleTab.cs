using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B32 RID: 6962
	public class MainButtonWorker_ToggleTab : MainButtonWorker
	{
		// Token: 0x06009954 RID: 39252 RVA: 0x000662AF File Offset: 0x000644AF
		public override void Activate()
		{
			Find.MainTabsRoot.ToggleTab(this.def, true);
		}
	}
}
