using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E97 RID: 3735
	public class ThoughtWorker_ImprisonedByFaction : ThoughtWorker
	{
		// Token: 0x0600537D RID: 21373 RVA: 0x0003A3B8 File Offset: 0x000385B8
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			return p.IsPrisoner && p.guest.HostFaction == other.Faction;
		}
	}
}
