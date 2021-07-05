using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001067 RID: 4199
	public class Building_AncientCommsConsole : Building
	{
		// Token: 0x06006383 RID: 25475 RVA: 0x00219EE8 File Offset: 0x002180E8
		public override string GetInspectString()
		{
			return base.GetInspectString() + ("\n" + "LinkedTo".Translate() + ": " + "SupplySatellite".Translate());
		}
	}
}
