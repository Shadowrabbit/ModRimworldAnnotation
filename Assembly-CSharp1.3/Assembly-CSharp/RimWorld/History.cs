using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AF8 RID: 2808
	public sealed class History : IExposable
	{
		// Token: 0x0600421A RID: 16922 RVA: 0x00161E30 File Offset: 0x00160030
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

		// Token: 0x0600421B RID: 16923 RVA: 0x00161F44 File Offset: 0x00160144
		public void HistoryTick()
		{
			for (int i = 0; i < this.autoRecorderGroups.Count; i++)
			{
				this.autoRecorderGroups[i].Tick();
			}
			this.historyEventsManager.HistoryEventsManagerTick();
		}

		// Token: 0x0600421C RID: 16924 RVA: 0x00161F83 File Offset: 0x00160183
		public List<HistoryAutoRecorderGroup> Groups()
		{
			return this.autoRecorderGroups;
		}

		// Token: 0x0600421D RID: 16925 RVA: 0x00161F8C File Offset: 0x0016018C
		public void ExposeData()
		{
			Scribe_Deep.Look<Archive>(ref this.archive, "archive", Array.Empty<object>());
			Scribe_Collections.Look<HistoryAutoRecorderGroup>(ref this.autoRecorderGroups, "autoRecorderGroups", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.lastPsylinkAvailable, "lastPsylinkAvailable", -999999, false);
			Scribe_Values.Look<int>(ref this.lastTickPlayerRaidedSomeone, "lastTickPlayerRaidedSomeone", -9999999, false);
			Scribe_Deep.Look<HistoryEventsManager>(ref this.historyEventsManager, "historyEventsManager", Array.Empty<object>());
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

		// Token: 0x0600421E RID: 16926 RVA: 0x00162036 File Offset: 0x00160236
		public void Notify_PsylinkAvailable()
		{
			this.lastPsylinkAvailable = Find.TickManager.TicksGame;
		}

		// Token: 0x0600421F RID: 16927 RVA: 0x00162048 File Offset: 0x00160248
		public void Notify_PlayerRaidedSomeone()
		{
			this.lastTickPlayerRaidedSomeone = Find.TickManager.TicksGame;
		}

		// Token: 0x06004220 RID: 16928 RVA: 0x00162036 File Offset: 0x00160236
		public void FinalizeInit()
		{
			this.lastPsylinkAvailable = Find.TickManager.TicksGame;
		}

		// Token: 0x06004221 RID: 16929 RVA: 0x0016205C File Offset: 0x0016025C
		private void AddOrRemoveHistoryRecorderGroups()
		{
			if (this.autoRecorderGroups.RemoveAll((HistoryAutoRecorderGroup x) => x == null) != 0)
			{
				Log.Warning("Some history auto recorder groups were null.");
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

		// Token: 0x04002846 RID: 10310
		public Archive archive = new Archive();

		// Token: 0x04002847 RID: 10311
		private List<HistoryAutoRecorderGroup> autoRecorderGroups;

		// Token: 0x04002848 RID: 10312
		public SimpleCurveDrawerStyle curveDrawerStyle;

		// Token: 0x04002849 RID: 10313
		public int lastPsylinkAvailable = -999999;

		// Token: 0x0400284A RID: 10314
		public int lastTickPlayerRaidedSomeone = -9999999;

		// Token: 0x0400284B RID: 10315
		public HistoryEventsManager historyEventsManager = new HistoryEventsManager();
	}
}
