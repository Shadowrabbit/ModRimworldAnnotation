using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001362 RID: 4962
	public class MainButtonWorker_ToggleTab : MainButtonWorker
	{
		// Token: 0x06007862 RID: 30818 RVA: 0x002A6EDB File Offset: 0x002A50DB
		public override void Activate()
		{
			Find.MainTabsRoot.ToggleTab(this.def, true);
		}
	}
}
