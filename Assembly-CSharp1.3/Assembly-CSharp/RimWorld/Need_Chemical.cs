using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E43 RID: 3651
	public class Need_Chemical : Need
	{
		// Token: 0x17000E70 RID: 3696
		// (get) Token: 0x06005490 RID: 21648 RVA: 0x000B955A File Offset: 0x000B775A
		public override int GUIChangeArrow
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000E71 RID: 3697
		// (get) Token: 0x06005491 RID: 21649 RVA: 0x001CAA8E File Offset: 0x001C8C8E
		public DrugDesireCategory CurCategory
		{
			get
			{
				if (this.CurLevel > 0.1f)
				{
					return DrugDesireCategory.Satisfied;
				}
				if (this.CurLevel > 0.01f)
				{
					return DrugDesireCategory.Desire;
				}
				return DrugDesireCategory.Withdrawal;
			}
		}

		// Token: 0x17000E72 RID: 3698
		// (get) Token: 0x06005492 RID: 21650 RVA: 0x001CAAAF File Offset: 0x001C8CAF
		// (set) Token: 0x06005493 RID: 21651 RVA: 0x001CAAB8 File Offset: 0x001C8CB8
		public override float CurLevel
		{
			get
			{
				return base.CurLevel;
			}
			set
			{
				DrugDesireCategory curCategory = this.CurCategory;
				base.CurLevel = value;
				if (this.CurCategory != curCategory)
				{
					this.CategoryChanged();
				}
			}
		}

		// Token: 0x17000E73 RID: 3699
		// (get) Token: 0x06005494 RID: 21652 RVA: 0x001CAAE4 File Offset: 0x001C8CE4
		public Hediff_Addiction AddictionHediff
		{
			get
			{
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					Hediff_Addiction hediff_Addiction = hediffs[i] as Hediff_Addiction;
					if (hediff_Addiction != null && hediff_Addiction.def.causesNeed == this.def)
					{
						return hediff_Addiction;
					}
				}
				return null;
			}
		}

		// Token: 0x17000E74 RID: 3700
		// (get) Token: 0x06005495 RID: 21653 RVA: 0x001CAB3E File Offset: 0x001C8D3E
		private float ChemicalFallPerTick
		{
			get
			{
				return this.def.fallPerDay / 60000f;
			}
		}

		// Token: 0x06005496 RID: 21654 RVA: 0x001CAB51 File Offset: 0x001C8D51
		public Need_Chemical(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.1f);
		}

		// Token: 0x06005497 RID: 21655 RVA: 0x001CAB75 File Offset: 0x001C8D75
		public override void SetInitialLevel()
		{
			base.CurLevelPercentage = Rand.Range(0.8f, 1f);
		}

		// Token: 0x06005498 RID: 21656 RVA: 0x001CAB8C File Offset: 0x001C8D8C
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				this.CurLevel -= this.ChemicalFallPerTick * 150f;
			}
		}

		// Token: 0x06005499 RID: 21657 RVA: 0x001CABB0 File Offset: 0x001C8DB0
		private void CategoryChanged()
		{
			Hediff_Addiction addictionHediff = this.AddictionHediff;
			if (addictionHediff != null)
			{
				addictionHediff.Notify_NeedCategoryChanged();
			}
		}

		// Token: 0x040031E2 RID: 12770
		private const float ThreshDesire = 0.01f;

		// Token: 0x040031E3 RID: 12771
		private const float ThreshSatisfied = 0.1f;
	}
}
