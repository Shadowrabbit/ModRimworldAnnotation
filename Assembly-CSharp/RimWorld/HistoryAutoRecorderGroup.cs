using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001027 RID: 4135
	public class HistoryAutoRecorderGroup : IExposable
	{
		// Token: 0x06005A40 RID: 23104 RVA: 0x001D4C68 File Offset: 0x001D2E68
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

		// Token: 0x06005A41 RID: 23105 RVA: 0x001D4CEC File Offset: 0x001D2EEC
		public void Tick()
		{
			for (int i = 0; i < this.recorders.Count; i++)
			{
				this.recorders[i].Tick();
			}
		}

		// Token: 0x06005A42 RID: 23106 RVA: 0x001D4D20 File Offset: 0x001D2F20
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

		// Token: 0x06005A43 RID: 23107 RVA: 0x0003E93A File Offset: 0x0003CB3A
		public void ExposeData()
		{
			Scribe_Defs.Look<HistoryAutoRecorderGroupDef>(ref this.def, "def");
			Scribe_Collections.Look<HistoryAutoRecorder>(ref this.recorders, "recorders", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.AddOrRemoveHistoryRecorders();
			}
		}

		// Token: 0x06005A44 RID: 23108 RVA: 0x001D4EF8 File Offset: 0x001D30F8
		public void AddOrRemoveHistoryRecorders()
		{
			if (this.recorders.RemoveAll((HistoryAutoRecorder x) => x == null) != 0)
			{
				Log.Warning("Some history auto recorders were null.", false);
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

		// Token: 0x04003CB9 RID: 15545
		public HistoryAutoRecorderGroupDef def;

		// Token: 0x04003CBA RID: 15546
		public List<HistoryAutoRecorder> recorders = new List<HistoryAutoRecorder>();

		// Token: 0x04003CBB RID: 15547
		private List<SimpleCurveDrawInfo> curves = new List<SimpleCurveDrawInfo>();

		// Token: 0x04003CBC RID: 15548
		private int cachedGraphTickCount = -1;
	}
}
