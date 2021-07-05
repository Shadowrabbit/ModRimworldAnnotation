using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200098C RID: 2444
	public class ThoughtWorker_ImprisonedByFaction : ThoughtWorker
	{
		// Token: 0x06003DA0 RID: 15776 RVA: 0x00152BFB File Offset: 0x00150DFB
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			return p.IsPrisoner && p.guest.HostFaction == other.Faction;
		}
	}
}
