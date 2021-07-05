using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AFA RID: 2810
	public class HistoryAutoRecorderGroup : IExposable
	{
		// Token: 0x06004227 RID: 16935 RVA: 0x0016229C File Offset: 0x0016049C
		public float GetMaxDay()
		{
			float num = 0f;
			foreach (HistoryAutoRecorder historyAutoRecorder in this.recorders)
			{
				int count = historyAutoRecorder.records.Count;
				if (count != 0)
				{
					float num2 = (float)((count - 1) * historyAutoRecorder.def.recordTicksFrequency) / 60000f;
					if (num2 > num)
					{
						num = num2;
					}
				}
			}
			return num;
		}

		// Token: 0x06004228 RID: 16936 RVA: 0x00162320 File Offset: 0x00160520
		public void Tick()
		{
			for (int i = 0; i < this.recorders.Count; i++)
			{
				this.recorders[i].Tick();
			}
		}

		// Token: 0x06004229 RID: 16937 RVA: 0x00162354 File Offset: 0x00160554
		public void DrawGraph(Rect graphRect, Rect legendRect, FloatRange section, List<CurveMark> marks)
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (ticksGame != this.cachedGraphTickCount)
			{
				this.cachedGraphTickCount = ticksGame;
				this.curves.Clear();
				for (int i = 0; i < this.recorders.Count; i++)
				{
					HistoryAutoRecorder historyAutoRecorder = this.recorders[i];
					SimpleCurveDrawInfo simpleCurveDrawInfo = new SimpleCurveDrawInfo();
					simpleCurveDrawInfo.color = historyAutoRecorder.def.graphColor;
					simpleCurveDrawInfo.label = historyAutoRecorder.def.LabelCap;
					simpleCurveDrawInfo.valueFormat = historyAutoRecorder.def.valueFormat;
					simpleCurveDrawInfo.curve = new SimpleCurve();
					for (int j = 0; j < historyAutoRecorder.records.Count; j++)
					{
						simpleCurveDrawInfo.curve.Add(new CurvePoint((float)j * (float)historyAutoRecorder.def.recordTicksFrequency / 60000f, historyAutoRecorder.records[j]), false);
					}
					simpleCurveDrawInfo.curve.SortPoints();
					if (historyAutoRecorder.records.Count == 1)
					{
						simpleCurveDrawInfo.curve.Add(new CurvePoint(1.6666667E-05f, historyAutoRecorder.records[0]), true);
					}
					this.curves.Add(simpleCurveDrawInfo);
				}
			}
			if (Mathf.Approximately(section.min, section.max))
			{
				section.max += 1.6666667E-05f;
			}
			SimpleCurveDrawerStyle curveDrawerStyle = Find.History.curveDrawerStyle;
			curveDrawerStyle.FixedSection = section;
			curveDrawerStyle.UseFixedScale = this.def.useFixedScale;
			curveDrawerStyle.FixedScale = this.def.fixedScale;
			curveDrawerStyle.YIntegersOnly = this.def.integersOnly;
			curveDrawerStyle.OnlyPositiveValues = this.def.onlyPositiveValues;
			SimpleCurveDrawer.DrawCurves(graphRect, this.curves, curveDrawerStyle, marks, legendRect);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0600422A RID: 16938 RVA: 0x00162529 File Offset: 0x00160729
		public void ExposeData()
		{
			Scribe_Defs.Look<HistoryAutoRecorderGroupDef>(ref this.def, "def");
			Scribe_Collections.Look<HistoryAutoRecorder>(ref this.recorders, "recorders", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.AddOrRemoveHistoryRecorders();
			}
		}

		// Token: 0x0600422B RID: 16939 RVA: 0x00162560 File Offset: 0x00160760
		public void AddOrRemoveHistoryRecorders()
		{
			if (this.recorders.RemoveAll((HistoryAutoRecorder x) => x == null) != 0)
			{
				Log.Warning("Some history auto recorders were null.");
			}
			using (List<HistoryAutoRecorderDef>.Enumerator enumerator = this.def.historyAutoRecorderDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					HistoryAutoRecorderDef recorderDef = enumerator.Current;
					if (!this.recorders.Any((HistoryAutoRecorder x) => x.def == recorderDef))
					{
						HistoryAutoRecorder historyAutoRecorder = new HistoryAutoRecorder();
						historyAutoRecorder.def = recorderDef;
						this.recorders.Add(historyAutoRecorder);
					}
				}
			}
			this.recorders.RemoveAll((HistoryAutoRecorder x) => x.def == null);
		}

		// Token: 0x0400284E RID: 10318
		public HistoryAutoRecorderGroupDef def;

		// Token: 0x0400284F RID: 10319
		public List<HistoryAutoRecorder> recorders = new List<HistoryAutoRecorder>();

		// Token: 0x04002850 RID: 10320
		private List<SimpleCurveDrawInfo> curves = new List<SimpleCurveDrawInfo>();

		// Token: 0x04002851 RID: 10321
		private int cachedGraphTickCount = -1;
	}
}
