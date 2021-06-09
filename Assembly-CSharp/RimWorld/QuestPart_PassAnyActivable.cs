using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200104E RID: 4174
	public class QuestPart_PassAnyActivable : QuestPartActivable
	{
		// Token: 0x06005AFA RID: 23290 RVA: 0x0003F1CE File Offset: 0x0003D3CE
		protected override void ProcessQuestSignal(Signal signal)
		{
			base.ProcessQuestSignal(signal);
			if (this.inSignals.Contains(signal.tag))
			{
				base.Complete();
			}
		}

		// Token: 0x06005AFB RID: 23291 RVA: 0x0003F1F0 File Offset: 0x0003D3F0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x06005AFC RID: 23292 RVA: 0x001D70C8 File Offset: 0x001D52C8
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignals.Clear();
			for (int i = 0; i < 3; i++)
			{
				this.inSignals.Add("DebugSignal" + Rand.Int);
			}
		}

		// Token: 0x04003D1E RID: 15646
		public List<string> inSignals = new List<string>();
	}
}
