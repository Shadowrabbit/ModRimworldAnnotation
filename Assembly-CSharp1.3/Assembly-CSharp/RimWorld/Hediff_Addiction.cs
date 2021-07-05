using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D91 RID: 3473
	public class Hediff_Addiction : HediffWithComps
	{
		// Token: 0x17000DFA RID: 3578
		// (get) Token: 0x06005095 RID: 20629 RVA: 0x001AF888 File Offset: 0x001ADA88
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

		// Token: 0x17000DFB RID: 3579
		// (get) Token: 0x06005096 RID: 20630 RVA: 0x001AF8F0 File Offset: 0x001ADAF0
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

		// Token: 0x17000DFC RID: 3580
		// (get) Token: 0x06005097 RID: 20631 RVA: 0x001AF934 File Offset: 0x001ADB34
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

		// Token: 0x17000DFD RID: 3581
		// (get) Token: 0x06005098 RID: 20632 RVA: 0x001AF984 File Offset: 0x001ADB84
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

		// Token: 0x17000DFE RID: 3582
		// (get) Token: 0x06005099 RID: 20633 RVA: 0x001AF9EC File Offset: 0x001ADBEC
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

		// Token: 0x0600509A RID: 20634 RVA: 0x001AFA0E File Offset: 0x001ADC0E
		public void Notify_NeedCategoryChanged()
		{
			this.pawn.health.Notify_HediffChanged(this);
		}

		// Token: 0x04002FFC RID: 12284
		private const int DefaultStageIndex = 0;

		// Token: 0x04002FFD RID: 12285
		private const int WithdrawalStageIndex = 1;
	}
}
