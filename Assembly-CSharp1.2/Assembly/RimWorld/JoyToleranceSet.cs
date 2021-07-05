using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014D8 RID: 5336
	public class JoyToleranceSet : IExposable
	{
		// Token: 0x1700118D RID: 4493
		public float this[JoyKindDef d]
		{
			get
			{
				return this.tolerances[d];
			}
		}

		// Token: 0x060072FC RID: 29436 RVA: 0x0004D53D File Offset: 0x0004B73D
		public void ExposeData()
		{
			Scribe_Deep.Look<DefMap<JoyKindDef, float>>(ref this.tolerances, "tolerances", Array.Empty<object>());
			Scribe_Deep.Look<DefMap<JoyKindDef, bool>>(ref this.bored, "bored", Array.Empty<object>());
			if (this.bored == null)
			{
				this.bored = new DefMap<JoyKindDef, bool>();
			}
		}

		// Token: 0x060072FD RID: 29437 RVA: 0x0004D57C File Offset: 0x0004B77C
		public bool BoredOf(JoyKindDef def)
		{
			return this.bored[def];
		}

		// Token: 0x060072FE RID: 29438 RVA: 0x002320CC File Offset: 0x002302CC
		public void Notify_JoyGained(float amount, JoyKindDef joyKind)
		{
			float num = Mathf.Min(this.tolerances[joyKind] + amount * 0.65f, 1f);
			this.tolerances[joyKind] = num;
			if (num > 0.5f)
			{
				this.bored[joyKind] = true;
			}
		}

		// Token: 0x060072FF RID: 29439 RVA: 0x0004D58A File Offset: 0x0004B78A
		public float JoyFactorFromTolerance(JoyKindDef joyKind)
		{
			return 1f - this.tolerances[joyKind];
		}

		// Token: 0x06007300 RID: 29440 RVA: 0x0023211C File Offset: 0x0023031C
		public void NeedInterval(Pawn pawn)
		{
			float num = ExpectationsUtility.CurrentExpectationFor(pawn).joyToleranceDropPerDay * 150f / 60000f;
			for (int i = 0; i < this.tolerances.Count; i++)
			{
				float num2 = this.tolerances[i];
				num2 -= num;
				if (num2 < 0f)
				{
					num2 = 0f;
				}
				this.tolerances[i] = num2;
				if (this.bored[i] && num2 < 0.3f)
				{
					this.bored[i] = false;
				}
			}
		}

		// Token: 0x06007301 RID: 29441 RVA: 0x002321A8 File Offset: 0x002303A8
		public string TolerancesString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			List<JoyKindDef> allDefsListForReading = DefDatabase<JoyKindDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				JoyKindDef joyKindDef = allDefsListForReading[i];
				float num = this.tolerances[joyKindDef];
				if (num > 0.01f)
				{
					if (stringBuilder.Length == 0)
					{
						stringBuilder.AppendLine("JoyTolerances".Translate() + ":");
					}
					string text = "   " + joyKindDef.LabelCap + ": " + num.ToStringPercent();
					if (this.bored[joyKindDef])
					{
						text += " (" + "bored".Translate() + ")";
					}
					stringBuilder.AppendLine(text);
				}
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06007302 RID: 29442 RVA: 0x002322A0 File Offset: 0x002304A0
		public bool BoredOfAllAvailableJoyKinds(Pawn pawn)
		{
			List<JoyKindDef> list = JoyUtility.JoyKindsOnMapTempList(pawn.MapHeld);
			bool result = true;
			for (int i = 0; i < list.Count; i++)
			{
				if (!this.bored[list[i]])
				{
					result = false;
					break;
				}
			}
			list.Clear();
			return result;
		}

		// Token: 0x04004BB9 RID: 19385
		private DefMap<JoyKindDef, float> tolerances = new DefMap<JoyKindDef, float>();

		// Token: 0x04004BBA RID: 19386
		private DefMap<JoyKindDef, bool> bored = new DefMap<JoyKindDef, bool>();
	}
}
