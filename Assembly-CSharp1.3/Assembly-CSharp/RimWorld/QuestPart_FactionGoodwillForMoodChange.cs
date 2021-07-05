using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B42 RID: 2882
	public class QuestPart_FactionGoodwillForMoodChange : QuestPartActivable
	{
		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x06004367 RID: 17255 RVA: 0x0016785F File Offset: 0x00165A5F
		public override string ExpiryInfoPart
		{
			get
			{
				return "QuestAveragePawnMood".Translate(120000.ToStringTicksToPeriod(true, false, true, true), this.cachedMovingAverage.ToStringPercent());
			}
		}

		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x06004368 RID: 17256 RVA: 0x00167894 File Offset: 0x00165A94
		public override string ExpiryInfoPartTip
		{
			get
			{
				return "QuestAveragePawnMoodTargets".Translate((from p in this.pawns
				select p.LabelShort).ToCommaList(true, false), 120000.ToStringTicksToPeriod(true, false, true, true));
			}
		}

		// Token: 0x17000BCF RID: 3023
		// (get) Token: 0x06004369 RID: 17257 RVA: 0x001678FC File Offset: 0x00165AFC
		private float AveragePawnMoodPercent
		{
			get
			{
				float num = 0f;
				int num2 = 0;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.pawns[i].needs != null && this.pawns[i].needs.mood != null)
					{
						num += this.pawns[i].needs.mood.CurLevelPercentage;
						num2++;
					}
				}
				if (num2 == 0)
				{
					return 0f;
				}
				return num / (float)num2;
			}
		}

		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x0600436A RID: 17258 RVA: 0x00167984 File Offset: 0x00165B84
		private float MovingAveragePawnMoodPercent
		{
			get
			{
				if (this.movingAverage.Count == 0)
				{
					return this.AveragePawnMoodPercent;
				}
				float num = 0f;
				for (int i = 0; i < this.movingAverage.Count; i++)
				{
					num += this.movingAverage[i];
				}
				return num / (float)this.movingAverage.Count;
			}
		}

		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x0600436B RID: 17259 RVA: 0x001679DE File Offset: 0x00165BDE
		public int SampleSize
		{
			get
			{
				return Mathf.FloorToInt(48f);
			}
		}

		// Token: 0x0600436C RID: 17260 RVA: 0x001679EC File Offset: 0x00165BEC
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			this.currentInterval++;
			if (this.currentInterval >= 2500)
			{
				this.currentInterval = 0;
				while (this.movingAverage.Count >= this.SampleSize)
				{
					this.movingAverage.RemoveLast<float>();
				}
				this.movingAverage.Insert(0, this.AveragePawnMoodPercent);
				this.cachedMovingAverage = this.MovingAveragePawnMoodPercent;
			}
		}

		// Token: 0x0600436D RID: 17261 RVA: 0x00167A60 File Offset: 0x00165C60
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (!this.inSignal.NullOrEmpty() && signal.tag == this.inSignal)
			{
				float movingAveragePawnMoodPercent = this.MovingAveragePawnMoodPercent;
				int num = Mathf.RoundToInt(QuestPart_FactionGoodwillForMoodChange.GoodwillFromAverageMoodCurve.Evaluate(movingAveragePawnMoodPercent));
				SignalArgs args = new SignalArgs(num.Named("GOODWILL"), movingAveragePawnMoodPercent.ToStringPercent().Named("AVERAGEMOOD"));
				if (num > 0)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalSuccess, args));
					return;
				}
				Find.SignalManager.SendSignal(new Signal(this.outSignalFailed, args));
			}
		}

		// Token: 0x0600436E RID: 17262 RVA: 0x00167B08 File Offset: 0x00165D08
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalSuccess, "outSignalSuccess", null, false);
			Scribe_Values.Look<string>(ref this.outSignalFailed, "outSignalFailed", null, false);
			Scribe_Values.Look<int>(ref this.currentInterval, "currentInterval", 0, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<float>(ref this.movingAverage, "movingAverage", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
				if (this.movingAverage == null)
				{
					this.movingAverage = new List<float>();
				}
				this.cachedMovingAverage = this.MovingAveragePawnMoodPercent;
			}
		}

		// Token: 0x040028F6 RID: 10486
		public string inSignal;

		// Token: 0x040028F7 RID: 10487
		public string outSignalSuccess;

		// Token: 0x040028F8 RID: 10488
		public string outSignalFailed;

		// Token: 0x040028F9 RID: 10489
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x040028FA RID: 10490
		private int currentInterval = 2500;

		// Token: 0x040028FB RID: 10491
		private List<float> movingAverage = new List<float>();

		// Token: 0x040028FC RID: 10492
		private float cachedMovingAverage;

		// Token: 0x040028FD RID: 10493
		private static readonly SimpleCurve GoodwillFromAverageMoodCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(1f, 20f),
				true
			}
		};

		// Token: 0x040028FE RID: 10494
		public const int Interval = 2500;

		// Token: 0x040028FF RID: 10495
		public const int Range = 120000;
	}
}
