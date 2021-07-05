using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001113 RID: 4371
	public class QuestPart_SetFactionHidden : QuestPart
	{
		// Token: 0x17000ED8 RID: 3800
		// (get) Token: 0x06005F77 RID: 24439 RVA: 0x000420F8 File Offset: 0x000402F8
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

		// Token: 0x06005F78 RID: 24440 RVA: 0x001E2394 File Offset: 0x001E0594
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.faction.Hidden != this.hidden)
			{
				this.faction.hidden = new bool?(this.hidden);
			}
		}

		// Token: 0x06005F79 RID: 24441 RVA: 0x00042108 File Offset: 0x00040308
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x06005F7A RID: 24442 RVA: 0x0004211A File Offset: 0x0004031A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x04003FD3 RID: 16339
		public string inSignal;

		// Token: 0x04003FD4 RID: 16340
		public Faction faction;

		// Token: 0x04003FD5 RID: 16341
		public bool hidden;
	}
}
