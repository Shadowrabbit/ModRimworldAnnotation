using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001115 RID: 4373
	public class QuestPart_SetFactionRelations : QuestPart
	{
		// Token: 0x17000EDB RID: 3803
		// (get) Token: 0x06005F86 RID: 24454 RVA: 0x0004218B File Offset: 0x0004038B
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in base.InvolvedFactions)
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.faction != null)
				{
					yield return this.faction;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06005F87 RID: 24455 RVA: 0x001E2538 File Offset: 0x001E0738
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.faction != null && this.faction != Faction.OfPlayer)
			{
				this.faction.TrySetRelationKind(Faction.OfPlayer, this.relationKind, this.canSendLetter, null, null);
			}
		}

		// Token: 0x06005F88 RID: 24456 RVA: 0x0004219B File Offset: 0x0004039B
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x06005F89 RID: 24457 RVA: 0x001E259C File Offset: 0x001E079C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<FactionRelationKind>(ref this.relationKind, "relationKind", FactionRelationKind.Hostile, false);
			Scribe_Values.Look<bool>(ref this.canSendLetter, "canSendLetter", false, false);
		}

		// Token: 0x04003FDB RID: 16347
		public string inSignal;

		// Token: 0x04003FDC RID: 16348
		public Faction faction;

		// Token: 0x04003FDD RID: 16349
		public FactionRelationKind relationKind;

		// Token: 0x04003FDE RID: 16350
		public bool canSendLetter;
	}
}
