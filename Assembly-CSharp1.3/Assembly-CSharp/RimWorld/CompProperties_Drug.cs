using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009FB RID: 2555
	public class CompProperties_Drug : CompProperties
	{
		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x06003ED0 RID: 16080 RVA: 0x00157533 File Offset: 0x00155733
		public bool Addictive
		{
			get
			{
				return this.addictiveness > 0f;
			}
		}

		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x06003ED1 RID: 16081 RVA: 0x00157542 File Offset: 0x00155742
		public bool CanCauseOverdose
		{
			get
			{
				return this.overdoseSeverityOffset.TrueMax > 0f;
			}
		}

		// Token: 0x06003ED2 RID: 16082 RVA: 0x00157556 File Offset: 0x00155756
		public CompProperties_Drug()
		{
			this.compClass = typeof(CompDrug);
		}

		// Token: 0x06003ED3 RID: 16083 RVA: 0x0015758F File Offset: 0x0015578F
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

		// Token: 0x0400219F RID: 8607
		public ChemicalDef chemical;

		// Token: 0x040021A0 RID: 8608
		public float addictiveness;

		// Token: 0x040021A1 RID: 8609
		public float minToleranceToAddict;

		// Token: 0x040021A2 RID: 8610
		public float existingAddictionSeverityOffset = 0.1f;

		// Token: 0x040021A3 RID: 8611
		public float needLevelOffset = 1f;

		// Token: 0x040021A4 RID: 8612
		public FloatRange overdoseSeverityOffset = FloatRange.Zero;

		// Token: 0x040021A5 RID: 8613
		public float largeOverdoseChance;

		// Token: 0x040021A6 RID: 8614
		public bool isCombatEnhancingDrug;

		// Token: 0x040021A7 RID: 8615
		public float listOrder;
	}
}
