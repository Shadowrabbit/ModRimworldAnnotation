using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B22 RID: 2850
	public class QuestPart_PassWithFactionArg : QuestPart_Pass
	{
		// Token: 0x060042F4 RID: 17140 RVA: 0x0016613C File Offset: 0x0016433C
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

		// Token: 0x060042F5 RID: 17141 RVA: 0x001661C4 File Offset: 0x001643C4
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x060042F6 RID: 17142 RVA: 0x001661D6 File Offset: 0x001643D6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x040028C1 RID: 10433
		public Faction faction;
	}
}
