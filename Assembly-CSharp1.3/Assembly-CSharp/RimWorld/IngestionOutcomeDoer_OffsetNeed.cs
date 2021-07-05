using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A2C RID: 2604
	public class IngestionOutcomeDoer_OffsetNeed : IngestionOutcomeDoer
	{
		// Token: 0x06003F32 RID: 16178 RVA: 0x00158718 File Offset: 0x00156918
		protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
		{
			if (pawn.needs == null)
			{
				return;
			}
			Need need = pawn.needs.TryGetNeed(this.need);
			if (need == null)
			{
				return;
			}
			float num = this.offset;
			AddictionUtility.ModifyChemicalEffectForToleranceAndBodySize(pawn, this.toleranceChemical, ref num);
			if (this.perIngested)
			{
				num *= (float)ingested.stackCount;
			}
			need.CurLevel += num;
		}

		// Token: 0x06003F33 RID: 16179 RVA: 0x00158779 File Offset: 0x00156979
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			string str = (this.offset >= 0f) ? "+" : string.Empty;
			yield return new StatDrawEntry(StatCategoryDefOf.Drug, this.need.LabelCap, str + this.offset.ToStringPercent(), this.need.description, this.need.listPriority, null, null, false);
			yield break;
		}

		// Token: 0x04002296 RID: 8854
		public NeedDef need;

		// Token: 0x04002297 RID: 8855
		public float offset;

		// Token: 0x04002298 RID: 8856
		public ChemicalDef toleranceChemical;

		// Token: 0x04002299 RID: 8857
		public bool perIngested;
	}
}
