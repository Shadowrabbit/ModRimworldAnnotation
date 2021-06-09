using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019CF RID: 6607
	public class Designator_ZoneAdd_Growing_Expand : Designator_ZoneAdd_Growing
	{
		// Token: 0x06009216 RID: 37398 RVA: 0x00061DB2 File Offset: 0x0005FFB2
		public Designator_ZoneAdd_Growing_Expand()
		{
			this.defaultLabel = "DesignatorZoneExpand".Translate();
			this.hotKey = KeyBindingDefOf.Misc6;
		}
	}
}
