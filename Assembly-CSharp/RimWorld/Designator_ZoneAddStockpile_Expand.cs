using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019CD RID: 6605
	public class Designator_ZoneAddStockpile_Expand : Designator_ZoneAddStockpile_Resources
	{
		// Token: 0x06009211 RID: 37393 RVA: 0x00061D5D File Offset: 0x0005FF5D
		public Designator_ZoneAddStockpile_Expand()
		{
			this.defaultLabel = "DesignatorZoneExpand".Translate();
			this.hotKey = KeyBindingDefOf.Misc6;
		}
	}
}
