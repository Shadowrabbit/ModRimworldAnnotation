using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001023 RID: 4131
	public sealed class History : IExposable
	{
		// Token: 0x06005A2E RID: 23086 RVA: 0x001D488C File Offset: 0x001D2A8C
		public History()
		{
			this.autoRecorderGroups = new List<HistoryAutoRecorderGroup>();
			this.AddOrRemoveHistoryRecorderGroups();
			this.curveDrawerStyle = new SimpleCurveDrawerStyle();
			this.curveDrawerStyle.DrawMeasures = true;
			this.curveDrawerStyle.DrawPoints = false;
			this.curveDrawerStyle.DrawBackground = true;
			this.curveDrawerStyle.DrawBackgroundLines = false;
			this.curveDrawerStyle.DrawLegend = true;
			this.curveDrawerStyle.DrawCurveMousePoint = true;
			this.curveDrawerStyle.OnlyPositiveValues = true;
			this.curveDrawerStyle.UseFixedSection = true;
			this.curveDrawerStyle.UseAntiAliasedLines = true;
			this.curveDrawerStyle.PointsRemoveOptimization = true;
			this.curveDrawerStyle.MeasureLabelsXCount = 10;
			this.curveDrawerStyle.MeasureLabelsYCount = 5;
			this.curveDrawerStyle.XIntegersOnly = true;
			this.curveDrawerStyle.LabelX = "Day".Translate();
		}

		// Token: 0x06005A2F RID: 23087 RVA: 0x001D4988 File Offset: 0x001D2B88
		public void HistoryTick()
		{
			for (int i = 0; i < this.autoRecorderGroups.Count; i++)
			{
				this.autoRecorderGroups[i].Tick();
			}
		}

		// Token: 0x06005A30 RID: 23088 RVA: 0x0003E8E6 File Offset: 0x0003CAE6
		public List<HistoryAutoRecorderGroup> Groups()
		{
			return this.autoRecorderGroups;
		}

		// Token: 0x06005A31 RID: 23089 RVA: 0x001D49BC File Offset: 0x001D2BBC
		public void ExposeData()
		{
			Scribe_Deep.Look<Archive>(ref this.archive, "archive", Array.Empty<object>());
			Scribe_Collections.Look<HistoryAutoRecorderGroup>(ref this.autoRecorderGroups, "autoRecorderGroups", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.lastPsylinkAvailable, "lastPsylinkAvailable", -999999, false);
			BackCompatibility.PostExposeData(this);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.AddOrRemoveHistoryRecorderGroups();
				if (this.lastPsylinkAvailable == -999999)
				{
					this.lastPsylinkAvailable = Find.TickManager.TicksGame;
				}
			}
		}

		// Token: 0x06005A32 RID: 23090 RVA: 0x0003E8EE File Offset: 0x0003CAEE
		public void Notify_PsylinkAvailable()
		{
			this.lastPsylinkAvailable = Find.TickManager.TicksGame;
		}

		// Token: 0x06005A33 RID: 23091 RVA: 0x0003E8EE File Offset: 0x0003CAEE
		public void FinalizeInit()
		{
			this.lastPsylinkAvailable = Find.TickManager.TicksGame;
		}

		// Token: 0x06005A34 RID: 23092 RVA: 0x001D4A3C File Offset: 0x001D2C3C
		private void AddOrRemoveHistoryRecorderGroups()
		{
			if (this.autoRecorderGroups.RemoveAll((HistoryAutoRecorderGroup x) => x == null) != 0)
			{
				Log.Warning("Some history auto recorder groups were null.", false);
			}
			using (IEnumerator<HistoryAutoRecorderGroupDef> enumerator = DefDatabase<HistoryAutoRecorderGroupDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					HistoryAutoRecorderGroupDef def = enumerator.Current;
					if (!this.autoRecorderGroups.Any((HistoryAutoRecorderGroup x) => x.def == def))
					{
						HistoryAutoRecorderGroup historyAutoRecorderGroup = new HistoryAutoRecorderGroup();
						historyAutoRecorderGroup.def = def;
						historyAutoRecorderGroup.AddOrRemoveHistoryRecorders();
						this.autoRecorderGroups.Add(historyAutoRecorderGroup);
					}
				}
			}
			this.autoRecorderGroups.RemoveAll((HistoryAutoRecorderGroup x) => x.def == null);
		}

		// Token: 0x04003CAF RID: 15535
		public Archive archive = new Archive();

		// Token: 0x04003CB0 RID: 15536
		private List<HistoryAutoRecorderGroup> autoRecorderGroups;

		// Token: 0x04003CB1 RID: 15537
		public SimpleCurveDrawerStyle curveDrawerStyle;

		// Token: 0x04003CB2 RID: 15538
		public int lastPsylinkAvailable = -999999;
	}
}
