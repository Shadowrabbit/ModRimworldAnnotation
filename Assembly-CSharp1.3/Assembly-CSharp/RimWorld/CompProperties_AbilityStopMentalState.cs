using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D70 RID: 3440
	public class CompProperties_AbilityStopMentalState : CompProperties_AbilityEffect
	{
		// Token: 0x17000DCF RID: 3535
		// (get) Token: 0x06004FCB RID: 20427 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool OverridesPsyfocusCost
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004FCC RID: 20428 RVA: 0x001AB1EB File Offset: 0x001A93EB
		public CompProperties_AbilityStopMentalState()
		{
			this.compClass = typeof(CompAbilityEffect_StopMentalState);
		}

		// Token: 0x17000DD0 RID: 3536
		// (get) Token: 0x06004FCD RID: 20429 RVA: 0x001AB224 File Offset: 0x001A9424
		public override string PsyfocusCostExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder("PsyfocusCostPerMentalBreakIntensity".Translate() + ":");
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("  - " + "MentalBreakIntensityMinor".Translate().CapitalizeFirst() + ": " + this.psyfocusCostForMinor.ToStringPercent());
				stringBuilder.AppendLine("  - " + "MentalBreakIntensityMajor".Translate().CapitalizeFirst() + ": " + this.psyfocusCostForMajor.ToStringPercent());
				stringBuilder.AppendLine("  - " + "MentalBreakIntensityExtreme".Translate().CapitalizeFirst() + ": " + this.psyfocusCostForExtreme.ToStringPercent());
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000DD1 RID: 3537
		// (get) Token: 0x06004FCE RID: 20430 RVA: 0x001AB321 File Offset: 0x001A9521
		public override FloatRange PsyfocusCostRange
		{
			get
			{
				return new FloatRange(this.psyfocusCostForMinor, this.psyfocusCostForExtreme);
			}
		}

		// Token: 0x06004FCF RID: 20431 RVA: 0x001AB334 File Offset: 0x001A9534
		public override IEnumerable<string> ConfigErrors(AbilityDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.psyfocusCostForMinor < 0f)
			{
				yield return "psyfocusCostForMinor must be defined ";
			}
			if (this.psyfocusCostForMajor < 0f)
			{
				yield return "psyfocusCostForMajor must be defined ";
			}
			if (this.psyfocusCostForExtreme < 0f)
			{
				yield return "psyfocusCostForExtreme must be defined ";
			}
			yield break;
			yield break;
		}

		// Token: 0x04002FC9 RID: 12233
		public List<MentalStateDef> exceptions;

		// Token: 0x04002FCA RID: 12234
		public float psyfocusCostForMinor = -1f;

		// Token: 0x04002FCB RID: 12235
		public float psyfocusCostForMajor = -1f;

		// Token: 0x04002FCC RID: 12236
		public float psyfocusCostForExtreme = -1f;
	}
}
