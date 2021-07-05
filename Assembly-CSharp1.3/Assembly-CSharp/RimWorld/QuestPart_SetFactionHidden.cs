using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BA8 RID: 2984
	public class QuestPart_SetFactionHidden : QuestPart
	{
		// Token: 0x17000C29 RID: 3113
		// (get) Token: 0x0600459F RID: 17823 RVA: 0x00170DDA File Offset: 0x0016EFDA
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

		// Token: 0x060045A0 RID: 17824 RVA: 0x00170DEC File Offset: 0x0016EFEC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.faction != null && this.faction.Hidden != this.hidden)
			{
				this.faction.hidden = new bool?(this.hidden);
			}
		}

		// Token: 0x060045A1 RID: 17825 RVA: 0x00170E44 File Offset: 0x0016F044
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x060045A2 RID: 17826 RVA: 0x00170E56 File Offset: 0x0016F056
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x04002A61 RID: 10849
		public string inSignal;

		// Token: 0x04002A62 RID: 10850
		public Faction faction;

		// Token: 0x04002A63 RID: 10851
		public bool hidden;
	}
}
