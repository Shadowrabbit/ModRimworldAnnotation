using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A2B RID: 2603
	public class IngestionOutcomeDoer_GiveHediff : IngestionOutcomeDoer
	{
		// Token: 0x06003F2F RID: 16175 RVA: 0x00158674 File Offset: 0x00156874
		protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
		{
			Hediff hediff = HediffMaker.MakeHediff(this.hediffDef, pawn, null);
			float num;
			if (this.severity > 0f)
			{
				num = this.severity;
			}
			else
			{
				num = this.hediffDef.initialSeverity;
			}
			if (this.divideByBodySize)
			{
				num /= pawn.BodySize;
			}
			AddictionUtility.ModifyChemicalEffectForToleranceAndBodySize(pawn, this.toleranceChemical, ref num);
			hediff.Severity = num;
			pawn.health.AddHediff(hediff, null, null, null);
		}

		// Token: 0x06003F30 RID: 16176 RVA: 0x001586EE File Offset: 0x001568EE
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			if (parentDef.IsDrug && this.chance >= 1f)
			{
				foreach (StatDrawEntry statDrawEntry in this.hediffDef.SpecialDisplayStats(StatRequest.ForEmpty()))
				{
					yield return statDrawEntry;
				}
				IEnumerator<StatDrawEntry> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x04002292 RID: 8850
		public HediffDef hediffDef;

		// Token: 0x04002293 RID: 8851
		public float severity = -1f;

		// Token: 0x04002294 RID: 8852
		public ChemicalDef toleranceChemical;

		// Token: 0x04002295 RID: 8853
		private bool divideByBodySize;
	}
}
