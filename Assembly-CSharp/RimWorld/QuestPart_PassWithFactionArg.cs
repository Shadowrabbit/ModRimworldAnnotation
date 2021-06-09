using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001052 RID: 4178
	public class QuestPart_PassWithFactionArg : QuestPart_Pass
	{
		// Token: 0x06005B0A RID: 23306 RVA: 0x001D7344 File Offset: 0x001D5544
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				SignalArgs args = new SignalArgs(signal.args);
				if (this.outSignalOutcomeArg != null)
				{
					args.Add(this.outSignalOutcomeArg.Value.Named("OUTCOME"));
				}
				args.Add(this.faction.Named("FACTION"));
				Find.SignalManager.SendSignal(new Signal(this.outSignal, args));
			}
		}

		// Token: 0x06005B0B RID: 23307 RVA: 0x0003F2F9 File Offset: 0x0003D4F9
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x06005B0C RID: 23308 RVA: 0x0003F30B File Offset: 0x0003D50B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x04003D25 RID: 15653
		public Faction faction;
	}
}
