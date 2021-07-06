using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F41 RID: 3905
	public class IngestionOutcomeDoer_OffsetNeed : IngestionOutcomeDoer
	{
		// Token: 0x060055E0 RID: 21984 RVA: 0x001C9388 File Offset: 0x001C7588
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
			need.CurLevel += num;
		}

		// Token: 0x060055E1 RID: 21985 RVA: 0x0003BA51 File Offset: 0x00039C51
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			string str = (this.offset >= 0f) ? "+" : string.Empty;
			yield return new StatDrawEntry(StatCategoryDefOf.Drug, this.need.LabelCap, str + this.offset.ToStringPercent(), this.need.description, this.need.listPriority, null, null, false);
			yield break;
		}

		// Token: 0x04003748 RID: 14152
		public NeedDef need;

		// Token: 0x04003749 RID: 14153
		public float offset;

		// Token: 0x0400374A RID: 14154
		public ChemicalDef toleranceChemical;
	}
}
