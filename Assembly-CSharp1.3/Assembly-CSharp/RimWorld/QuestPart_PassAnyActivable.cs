using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B1C RID: 2844
	public class QuestPart_PassAnyActivable : QuestPartActivable
	{
		// Token: 0x060042DC RID: 17116 RVA: 0x00165B6A File Offset: 0x00163D6A
		protected override void ProcessQuestSignal(Signal signal)
		{
			base.ProcessQuestSignal(signal);
			if (this.inSignals.Contains(signal.tag))
			{
				base.Complete();
			}
		}

		// Token: 0x060042DD RID: 17117 RVA: 0x00165B8C File Offset: 0x00163D8C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x060042DE RID: 17118 RVA: 0x00165BAC File Offset: 0x00163DAC
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignals.Clear();
			for (int i = 0; i < 3; i++)
			{
				this.inSignals.Add("DebugSignal" + Rand.Int);
			}
		}

		// Token: 0x040028B4 RID: 10420
		public List<string> inSignals = new List<string>();
	}
}
