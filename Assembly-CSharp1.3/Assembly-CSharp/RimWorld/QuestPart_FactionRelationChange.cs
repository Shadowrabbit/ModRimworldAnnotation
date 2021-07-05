using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B7A RID: 2938
	public class QuestPart_FactionRelationChange : QuestPart
	{
		// Token: 0x060044AF RID: 17583 RVA: 0x0016C2D0 File Offset: 0x0016A4D0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.faction != null && this.faction != Faction.OfPlayer)
			{
				this.faction.SetRelationDirect(Faction.OfPlayer, this.relationKind, this.canSendHostilityLetter, null, null);
			}
		}

		// Token: 0x060044B0 RID: 17584 RVA: 0x0016C332 File Offset: 0x0016A532
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x060044B1 RID: 17585 RVA: 0x0016C344 File Offset: 0x0016A544
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<FactionRelationKind>(ref this.relationKind, "relationKind", FactionRelationKind.Hostile, false);
			Scribe_Values.Look<bool>(ref this.canSendHostilityLetter, "canSendHostilityLetter", false, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x040029AE RID: 10670
		public string inSignal;

		// Token: 0x040029AF RID: 10671
		public Faction faction;

		// Token: 0x040029B0 RID: 10672
		public FactionRelationKind relationKind;

		// Token: 0x040029B1 RID: 10673
		public bool canSendHostilityLetter;
	}
}
