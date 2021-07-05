using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F48 RID: 3912
	public class RitualObligationTrigger_MemberDied : RitualObligationTrigger
	{
		// Token: 0x06005CF6 RID: 23798 RVA: 0x001FEF74 File Offset: 0x001FD174
		public override void Notify_MemberDied(Pawn p)
		{
			if ((this.mustBePlayerIdeo && !Faction.OfPlayer.ideos.Has(this.ritual.ideo)) || (p.HomeFaction != Faction.OfPlayer && !p.IsSlave) || !p.IsFreeColonist || p.IsKidnapped())
			{
				return;
			}
			this.ritual.AddObligation(new RitualObligation(this.ritual, p.Corpse, true)
			{
				showAlert = !p.IsSlave
			});
		}
	}
}
