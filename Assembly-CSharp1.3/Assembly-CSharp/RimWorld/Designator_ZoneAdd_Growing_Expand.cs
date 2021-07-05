using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012CF RID: 4815
	public class Designator_ZoneAdd_Growing_Expand : Designator_ZoneAdd_Growing
	{
		// Token: 0x06007323 RID: 29475 RVA: 0x0026714C File Offset: 0x0026534C
		public Designator_ZoneAdd_Growing_Expand()
		{
			this.defaultLabel = "DesignatorZoneExpand".Translate();
			this.hotKey = KeyBindingDefOf.Misc6;
		}
	}
}
