using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200106D RID: 4205
	public class QuestPart_FactionGoodwillForMoodChange : QuestPartActivable
	{
		// Token: 0x17000E23 RID: 3619
		// (get) Token: 0x06005B72 RID: 23410 RVA: 0x0003F646 File Offset: 0x0003D846
		public override string ExpiryInfoPart
		{
			get
			{
				return "QuestAveragePawnMood".Translate(120000.ToStringTicksToPeriod(true, false, true, true), this.cachedMovingAverage.ToStringPercent());
			}
		}

		// Token: 0x17000E24 RID: 3620
		// (get) Token: 0x06005B73 RID: 23411 RVA: 0x001D8304 File Offset: 0x001D6504
		public override string ExpiryInfoPartTip
		{
			get
			{
				return "QuestAveragePawnMoodTargets".Translate((from p in this.pawns
				select p.LabelShort).ToCommaList(true), 120000.ToStringTicksToPeriod(true, false, true, true));
			}
		}

		// Token: 0x17000E25 RID: 3621
		// (get) Token: 0x06005B74 RID: 23412 RVA: 0x001D8368 File Offset: 0x001D6568
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

		// Token: 0x17000E26 RID: 3622
		// (get) Token: 0x06005B75 RID: 23413 RVA: 0x001D83F0 File Offset: 0x001D65F0
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

		// Token: 0x17000E27 RID: 3623
		// (get) Token: 0x06005B76 RID: 23414 RVA: 0x0003F67A File Offset: 0x0003D87A
		public int SampleSize
		{
			get
			{
				return Mathf.FloorToInt(48f);
			}
		}

		// Token: 0x06005B77 RID: 23415 RVA: 0x001D844C File Offset: 0x001D664C
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

		// Token: 0x06005B78 RID: 23416 RVA: 0x001D84C0 File Offset: 0x001D66C0
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

		// Token: 0x06005B79 RID: 23417 RVA: 0x001D8568 File Offset: 0x001D6768
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

		// Token: 0x04003D61 RID: 15713
		public string inSignal;

		// Token: 0x04003D62 RID: 15714
		public string outSignalSuccess;

		// Token: 0x04003D63 RID: 15715
		public string outSignalFailed;

		// Token: 0x04003D64 RID: 15716
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04003D65 RID: 15717
		private int currentInterval = 2500;

		// Token: 0x04003D66 RID: 15718
		private List<float> movingAverage = new List<float>();

		// Token: 0x04003D67 RID: 15719
		private float cachedMovingAverage;

		// Token: 0x04003D68 RID: 15720
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

		// Token: 0x04003D69 RID: 15721
		public const int Interval = 2500;

		// Token: 0x04003D6A RID: 15722
		public const int Range = 120000;
	}
}
