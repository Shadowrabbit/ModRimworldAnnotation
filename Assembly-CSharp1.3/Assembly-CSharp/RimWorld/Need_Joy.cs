using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E4E RID: 3662
	public class Need_Joy : Need
	{
		// Token: 0x17000E92 RID: 3730
		// (get) Token: 0x060054CE RID: 21710 RVA: 0x001CB920 File Offset: 0x001C9B20
		public JoyCategory CurCategory
		{
			get
			{
				if (this.CurLevel < 0.01f)
				{
					return JoyCategory.Empty;
				}
				if (this.CurLevel < 0.15f)
				{
					return JoyCategory.VeryLow;
				}
				if (this.CurLevel < 0.3f)
				{
					return JoyCategory.Low;
				}
				if (this.CurLevel < 0.7f)
				{
					return JoyCategory.Satisfied;
				}
				if (this.CurLevel < 0.85f)
				{
					return JoyCategory.High;
				}
				return JoyCategory.Extreme;
			}
		}

		// Token: 0x17000E93 RID: 3731
		// (get) Token: 0x060054CF RID: 21711 RVA: 0x001CB97C File Offset: 0x001C9B7C
		private float FallPerInterval
		{
			get
			{
				switch (this.CurCategory)
				{
				case JoyCategory.Empty:
					return 0.0015f;
				case JoyCategory.VeryLow:
					return 0.0006f;
				case JoyCategory.Low:
					return 0.00105f;
				case JoyCategory.Satisfied:
					return 0.0015f;
				case JoyCategory.High:
					return 0.0015f;
				case JoyCategory.Extreme:
					return 0.0015f;
				default:
					throw new InvalidOperationException();
				}
			}
		}

		// Token: 0x17000E94 RID: 3732
		// (get) Token: 0x060054D0 RID: 21712 RVA: 0x001CB9D9 File Offset: 0x001C9BD9
		public override int GUIChangeArrow
		{
			get
			{
				if (this.IsFrozen)
				{
					return 0;
				}
				if (!this.GainingJoy)
				{
					return -1;
				}
				return 1;
			}
		}

		// Token: 0x17000E95 RID: 3733
		// (get) Token: 0x060054D1 RID: 21713 RVA: 0x001CB9F0 File Offset: 0x001C9BF0
		private bool GainingJoy
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastGainTick + 10;
			}
		}

		// Token: 0x060054D2 RID: 21714 RVA: 0x001CBA08 File Offset: 0x001C9C08
		public Need_Joy(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.15f);
			this.threshPercents.Add(0.3f);
			this.threshPercents.Add(0.7f);
			this.threshPercents.Add(0.85f);
		}

		// Token: 0x060054D3 RID: 21715 RVA: 0x001CBA7D File Offset: 0x001C9C7D
		public override void ExposeData()
		{
			base.ExposeData();
			this.tolerances.ExposeData();
		}

		// Token: 0x060054D4 RID: 21716 RVA: 0x001CBA90 File Offset: 0x001C9C90
		public override void SetInitialLevel()
		{
			this.CurLevel = Rand.Range(0.5f, 0.6f);
		}

		// Token: 0x060054D5 RID: 21717 RVA: 0x001CBAA8 File Offset: 0x001C9CA8
		public void GainJoy(float amount, JoyKindDef joyKind)
		{
			if (amount <= 0f)
			{
				return;
			}
			amount *= this.tolerances.JoyFactorFromTolerance(joyKind);
			amount = Mathf.Min(amount, 1f - this.CurLevel);
			this.curLevelInt += amount;
			if (joyKind != null)
			{
				this.tolerances.Notify_JoyGained(amount, joyKind);
			}
			this.lastGainTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060054D6 RID: 21718 RVA: 0x001CBB10 File Offset: 0x001C9D10
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				this.tolerances.NeedInterval(this.pawn);
				if (!this.GainingJoy)
				{
					this.CurLevel -= this.FallPerInterval;
				}
			}
		}

		// Token: 0x060054D7 RID: 21719 RVA: 0x001CBB48 File Offset: 0x001C9D48
		public override string GetTipString()
		{
			string text = base.GetTipString();
			string text2 = this.tolerances.TolerancesString();
			if (!string.IsNullOrEmpty(text2))
			{
				text = text + "\n\n" + text2;
			}
			if (this.pawn.MapHeld != null)
			{
				ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(this.pawn);
				text += "\n\n" + "CurrentExpectationsAndRecreation".Translate(expectationDef.label, expectationDef.joyToleranceDropPerDay.ToStringPercent(), expectationDef.joyKindsNeeded);
				text = text + "\n\n" + JoyUtility.JoyKindsOnMapString(this.pawn.MapHeld);
			}
			else
			{
				Caravan caravan = this.pawn.GetCaravan();
				if (caravan != null)
				{
					float num = caravan.needs.GetCurrentJoyGainPerTick(this.pawn) * 2500f;
					if (num > 0f)
					{
						text += "\n\n" + "GainingJoyBecauseCaravanNotMoving".Translate() + ": +" + num.ToStringPercent() + "/" + "LetterHour".Translate();
					}
				}
			}
			return text;
		}

		// Token: 0x0400322B RID: 12843
		public JoyToleranceSet tolerances = new JoyToleranceSet();

		// Token: 0x0400322C RID: 12844
		private int lastGainTick = -999;
	}
}
