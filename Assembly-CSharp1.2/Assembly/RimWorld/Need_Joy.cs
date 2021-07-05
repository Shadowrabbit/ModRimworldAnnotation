using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014EB RID: 5355
	public class Need_Joy : Need
	{
		// Token: 0x170011C0 RID: 4544
		// (get) Token: 0x06007363 RID: 29539 RVA: 0x0023387C File Offset: 0x00231A7C
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

		// Token: 0x170011C1 RID: 4545
		// (get) Token: 0x06007364 RID: 29540 RVA: 0x002338D8 File Offset: 0x00231AD8
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

		// Token: 0x170011C2 RID: 4546
		// (get) Token: 0x06007365 RID: 29541 RVA: 0x0004DAD9 File Offset: 0x0004BCD9
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

		// Token: 0x170011C3 RID: 4547
		// (get) Token: 0x06007366 RID: 29542 RVA: 0x0004DAF0 File Offset: 0x0004BCF0
		private bool GainingJoy
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastGainTick + 10;
			}
		}

		// Token: 0x06007367 RID: 29543 RVA: 0x00233938 File Offset: 0x00231B38
		public Need_Joy(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.15f);
			this.threshPercents.Add(0.3f);
			this.threshPercents.Add(0.7f);
			this.threshPercents.Add(0.85f);
		}

		// Token: 0x06007368 RID: 29544 RVA: 0x0004DB07 File Offset: 0x0004BD07
		public override void ExposeData()
		{
			base.ExposeData();
			this.tolerances.ExposeData();
		}

		// Token: 0x06007369 RID: 29545 RVA: 0x0004DB1A File Offset: 0x0004BD1A
		public override void SetInitialLevel()
		{
			this.CurLevel = Rand.Range(0.5f, 0.6f);
		}

		// Token: 0x0600736A RID: 29546 RVA: 0x002339B0 File Offset: 0x00231BB0
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

		// Token: 0x0600736B RID: 29547 RVA: 0x0004DB31 File Offset: 0x0004BD31
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

		// Token: 0x0600736C RID: 29548 RVA: 0x00233A18 File Offset: 0x00231C18
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

		// Token: 0x04004C26 RID: 19494
		public JoyToleranceSet tolerances = new JoyToleranceSet();

		// Token: 0x04004C27 RID: 19495
		private int lastGainTick = -999;
	}
}
