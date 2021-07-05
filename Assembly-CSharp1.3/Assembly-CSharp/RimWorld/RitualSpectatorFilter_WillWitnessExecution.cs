using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F9E RID: 3998
	public class RitualSpectatorFilter_WillWitnessExecution : RitualSpectatorFilter
	{
		// Token: 0x06005E8D RID: 24205 RVA: 0x00206A0E File Offset: 0x00204C0E
		public override bool Allowed(Pawn p)
		{
			return p.IsSlave || p.Ideo == null || p.Ideo.MemberWillingToDo(new HistoryEvent(HistoryEventDefOf.ExecutedPrisoner, "SUBJECT"));
		}
	}
}
