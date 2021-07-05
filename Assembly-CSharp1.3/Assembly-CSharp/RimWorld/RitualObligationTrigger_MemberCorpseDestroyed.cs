using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F4A RID: 3914
	public class RitualObligationTrigger_MemberCorpseDestroyed : RitualObligationTrigger
	{
		// Token: 0x06005CF9 RID: 23801 RVA: 0x001FF01C File Offset: 0x001FD21C
		public override void Notify_MemberCorpseDestroyed(Pawn p)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			if ((this.mustBePlayerIdeo && !Faction.OfPlayer.ideos.Has(this.ritual.ideo)) || p.HomeFaction != Faction.OfPlayer || !p.IsFreeNonSlaveColonist || p.IsKidnapped())
			{
				return;
			}
			this.ritual.AddObligation(new RitualObligation(this.ritual, p, true));
		}
	}
}
