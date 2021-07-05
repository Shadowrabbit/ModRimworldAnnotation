using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B1E RID: 2846
	public class QuestPart_PassOutInterval : QuestPartActivable
	{
		// Token: 0x060042E4 RID: 17124 RVA: 0x00165D30 File Offset: 0x00163F30
		public override void QuestPartTick()
		{
			if (this.currentInterval < 0)
			{
				foreach (string tag in this.outSignals)
				{
					Find.SignalManager.SendSignal(new Signal(tag));
				}
				this.currentInterval = this.ticksInterval.RandomInRange;
			}
			this.currentInterval--;
		}

		// Token: 0x060042E5 RID: 17125 RVA: 0x00165DB4 File Offset: 0x00163FB4
		protected override void ProcessQuestSignal(Signal signal)
		{
			if (this.inSignalsDisable.Contains(signal.tag))
			{
				this.Disable();
			}
		}

		// Token: 0x060042E6 RID: 17126 RVA: 0x00165DD0 File Offset: 0x00163FD0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.outSignals, "outSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.inSignalsDisable, "inSignalsDisable", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.currentInterval, "currentInterval", 0, false);
			Scribe_Values.Look<IntRange>(ref this.ticksInterval, "ticksInterval", default(IntRange), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.outSignals = (this.outSignals ?? new List<string>());
				this.inSignalsDisable = (this.inSignalsDisable ?? new List<string>());
			}
		}

		// Token: 0x060042E7 RID: 17127 RVA: 0x00165E70 File Offset: 0x00164070
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			if (base.State != QuestPartState.Enabled)
			{
				return;
			}
			Rect rect = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect, "Reset Interval " + this.ToString(), true, true, true))
			{
				this.currentInterval = 0;
			}
			curY += rect.height + 4f;
		}

		// Token: 0x040028B7 RID: 10423
		public IntRange ticksInterval;

		// Token: 0x040028B8 RID: 10424
		public List<string> outSignals = new List<string>();

		// Token: 0x040028B9 RID: 10425
		public List<string> inSignalsDisable = new List<string>();

		// Token: 0x040028BA RID: 10426
		private int currentInterval;
	}
}
