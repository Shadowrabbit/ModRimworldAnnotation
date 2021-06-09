using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020013B9 RID: 5049
	public class Hediff_Addiction : HediffWithComps
	{
		// Token: 0x170010F8 RID: 4344
		// (get) Token: 0x06006D84 RID: 28036 RVA: 0x00218FEC File Offset: 0x002171EC
		public Need_Chemical Need
		{
			get
			{
				if (this.pawn.Dead)
				{
					return null;
				}
				List<Need> allNeeds = this.pawn.needs.AllNeeds;
				for (int i = 0; i < allNeeds.Count; i++)
				{
					if (allNeeds[i].def == this.def.causesNeed)
					{
						return (Need_Chemical)allNeeds[i];
					}
				}
				return null;
			}
		}

		// Token: 0x170010F9 RID: 4345
		// (get) Token: 0x06006D85 RID: 28037 RVA: 0x00219054 File Offset: 0x00217254
		public ChemicalDef Chemical
		{
			get
			{
				List<ChemicalDef> allDefsListForReading = DefDatabase<ChemicalDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].addictionHediff == this.def)
					{
						return allDefsListForReading[i];
					}
				}
				return null;
			}
		}

		// Token: 0x170010FA RID: 4346
		// (get) Token: 0x06006D86 RID: 28038 RVA: 0x00219098 File Offset: 0x00217298
		public override string LabelInBrackets
		{
			get
			{
				string labelInBrackets = base.LabelInBrackets;
				string text = (1f - this.Severity).ToStringPercent("F0");
				if (this.def.CompProps<HediffCompProperties_SeverityPerDay>() == null)
				{
					return labelInBrackets;
				}
				if (!labelInBrackets.NullOrEmpty())
				{
					return labelInBrackets + " " + text;
				}
				return text;
			}
		}

		// Token: 0x170010FB RID: 4347
		// (get) Token: 0x06006D87 RID: 28039 RVA: 0x002190E8 File Offset: 0x002172E8
		public override string TipStringExtra
		{
			get
			{
				Need_Chemical need = this.Need;
				if (need != null)
				{
					return "CreatesNeed".Translate() + ": " + need.LabelCap + " (" + need.CurLevelPercentage.ToStringPercent("F0") + ")";
				}
				return null;
			}
		}

		// Token: 0x170010FC RID: 4348
		// (get) Token: 0x06006D88 RID: 28040 RVA: 0x00219150 File Offset: 0x00217350
		public override int CurStageIndex
		{
			get
			{
				Need_Chemical need = this.Need;
				if (need == null || need.CurCategory != DrugDesireCategory.Withdrawal)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x06006D89 RID: 28041 RVA: 0x0004A72D File Offset: 0x0004892D
		public void Notify_NeedCategoryChanged()
		{
			this.pawn.health.Notify_HediffChanged(this);
		}

		// Token: 0x04004865 RID: 18533
		private const int DefaultStageIndex = 0;

		// Token: 0x04004866 RID: 18534
		private const int WithdrawalStageIndex = 1;
	}
}
