using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E3B RID: 3643
	public class JoyToleranceSet : IExposable
	{
		// Token: 0x17000E5B RID: 3675
		public float this[JoyKindDef d]
		{
			get
			{
				return this.tolerances[d];
			}
		}

		// Token: 0x0600545C RID: 21596 RVA: 0x001C98F2 File Offset: 0x001C7AF2
		public void ExposeData()
		{
			Scribe_Deep.Look<DefMap<JoyKindDef, float>>(ref this.tolerances, "tolerances", Array.Empty<object>());
			Scribe_Deep.Look<DefMap<JoyKindDef, bool>>(ref this.bored, "bored", Array.Empty<object>());
			if (this.bored == null)
			{
				this.bored = new DefMap<JoyKindDef, bool>();
			}
		}

		// Token: 0x0600545D RID: 21597 RVA: 0x001C9931 File Offset: 0x001C7B31
		public bool BoredOf(JoyKindDef def)
		{
			return this.bored[def];
		}

		// Token: 0x0600545E RID: 21598 RVA: 0x001C9940 File Offset: 0x001C7B40
		public void Notify_JoyGained(float amount, JoyKindDef joyKind)
		{
			float num = Mathf.Min(this.tolerances[joyKind] + amount * 0.65f, 1f);
			this.tolerances[joyKind] = num;
			if (num > 0.5f)
			{
				this.bored[joyKind] = true;
			}
		}

		// Token: 0x0600545F RID: 21599 RVA: 0x001C998E File Offset: 0x001C7B8E
		public float JoyFactorFromTolerance(JoyKindDef joyKind)
		{
			return 1f - this.tolerances[joyKind];
		}

		// Token: 0x06005460 RID: 21600 RVA: 0x001C99A4 File Offset: 0x001C7BA4
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

		// Token: 0x06005461 RID: 21601 RVA: 0x001C9A30 File Offset: 0x001C7C30
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

		// Token: 0x06005462 RID: 21602 RVA: 0x001C9B28 File Offset: 0x001C7D28
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

		// Token: 0x040031B7 RID: 12727
		private DefMap<JoyKindDef, float> tolerances = new DefMap<JoyKindDef, float>();

		// Token: 0x040031B8 RID: 12728
		private DefMap<JoyKindDef, bool> bored = new DefMap<JoyKindDef, bool>();
	}
}
