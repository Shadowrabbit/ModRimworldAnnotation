using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001039 RID: 4153
	public class Building_Art : Building
	{
		// Token: 0x06006218 RID: 25112 RVA: 0x00214AD4 File Offset: 0x00212CD4
		public override string GetInspectString()
		{
			return base.GetInspectString() + ("\n" + StatDefOf.Beauty.LabelCap + ": " + StatDefOf.Beauty.ValueToString(this.GetStatValue(StatDefOf.Beauty, true), ToStringNumberSense.Absolute, true));
		}
	}
}
