using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001391 RID: 5009
	public class CompProperties_AbilityStopMentalState : CompProperties_AbilityEffect
	{
		// Token: 0x170010C8 RID: 4296
		// (get) Token: 0x06006CAB RID: 27819 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool OverridesPsyfocusCost
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006CAC RID: 27820 RVA: 0x00049EB9 File Offset: 0x000480B9
		public CompProperties_AbilityStopMentalState()
		{
			this.compClass = typeof(CompAbilityEffect_StopMentalState);
		}

		// Token: 0x170010C9 RID: 4297
		// (get) Token: 0x06006CAD RID: 27821 RVA: 0x00215E9C File Offset: 0x0021409C
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

		// Token: 0x170010CA RID: 4298
		// (get) Token: 0x06006CAE RID: 27822 RVA: 0x00049EF2 File Offset: 0x000480F2
		public override FloatRange PsyfocusCostRange
		{
			get
			{
				return new FloatRange(this.psyfocusCostForMinor, this.psyfocusCostForExtreme);
			}
		}

		// Token: 0x06006CAF RID: 27823 RVA: 0x00049F05 File Offset: 0x00048105
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

		// Token: 0x04004805 RID: 18437
		public List<MentalStateDef> exceptions;

		// Token: 0x04004806 RID: 18438
		public float psyfocusCostForMinor = -1f;

		// Token: 0x04004807 RID: 18439
		public float psyfocusCostForMajor = -1f;

		// Token: 0x04004808 RID: 18440
		public float psyfocusCostForExtreme = -1f;
	}
}
