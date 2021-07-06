using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200165F RID: 5727
	public class Building_Art : Building
	{
		// Token: 0x06007CB8 RID: 31928 RVA: 0x00254B78 File Offset: 0x00252D78
		public override string GetInspectString()
		{
			return base.GetInspectString() + ("\n" + StatDefOf.Beauty.LabelCap + ": " + StatDefOf.Beauty.ValueToString(this.GetStatValue(StatDefOf.Beauty, true), ToStringNumberSense.Absolute, true));
		}
	}
}
