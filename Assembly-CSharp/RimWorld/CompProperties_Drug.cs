using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F05 RID: 3845
	public class CompProperties_Drug : CompProperties
	{
		// Token: 0x17000CF7 RID: 3319
		// (get) Token: 0x0600550C RID: 21772 RVA: 0x0003AFEA File Offset: 0x000391EA
		public bool Addictive
		{
			get
			{
				return this.addictiveness > 0f;
			}
		}

		// Token: 0x17000CF8 RID: 3320
		// (get) Token: 0x0600550D RID: 21773 RVA: 0x0003AFF9 File Offset: 0x000391F9
		public bool CanCauseOverdose
		{
			get
			{
				return this.overdoseSeverityOffset.TrueMax > 0f;
			}
		}

		// Token: 0x0600550E RID: 21774 RVA: 0x0003B00D File Offset: 0x0003920D
		public CompProperties_Drug()
		{
			this.compClass = typeof(CompDrug);
		}

		// Token: 0x0600550F RID: 21775 RVA: 0x0003B046 File Offset: 0x00039246
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.Addictive && this.chemical == null)
			{
				yield return "addictive but chemical is null";
			}
			yield break;
			yield break;
		}

		// Token: 0x04003612 RID: 13842
		public ChemicalDef chemical;

		// Token: 0x04003613 RID: 13843
		public float addictiveness;

		// Token: 0x04003614 RID: 13844
		public float minToleranceToAddict;

		// Token: 0x04003615 RID: 13845
		public float existingAddictionSeverityOffset = 0.1f;

		// Token: 0x04003616 RID: 13846
		public float needLevelOffset = 1f;

		// Token: 0x04003617 RID: 13847
		public FloatRange overdoseSeverityOffset = FloatRange.Zero;

		// Token: 0x04003618 RID: 13848
		public float largeOverdoseChance;

		// Token: 0x04003619 RID: 13849
		public bool isCombatEnhancingDrug;

		// Token: 0x0400361A RID: 13850
		public float listOrder;
	}
}
