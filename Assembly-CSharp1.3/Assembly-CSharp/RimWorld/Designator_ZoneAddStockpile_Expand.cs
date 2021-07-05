using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012CD RID: 4813
	public class Designator_ZoneAddStockpile_Expand : Designator_ZoneAddStockpile_Resources
	{
		// Token: 0x0600731E RID: 29470 RVA: 0x00267001 File Offset: 0x00265201
		public Designator_ZoneAddStockpile_Expand()
		{
			this.defaultLabel = "DesignatorZoneExpand".Translate();
			this.hotKey = KeyBindingDefOf.Misc6;
		}
	}
}
