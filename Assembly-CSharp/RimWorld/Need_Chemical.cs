using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014E0 RID: 5344
	public class Need_Chemical : Need
	{
		// Token: 0x170011A2 RID: 4514
		// (get) Token: 0x06007330 RID: 29488 RVA: 0x000236C9 File Offset: 0x000218C9
		public override int GUIChangeArrow
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x170011A3 RID: 4515
		// (get) Token: 0x06007331 RID: 29489 RVA: 0x0004D7DD File Offset: 0x0004B9DD
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

		// Token: 0x170011A4 RID: 4516
		// (get) Token: 0x06007332 RID: 29490 RVA: 0x0004D7FE File Offset: 0x0004B9FE
		// (set) Token: 0x06007333 RID: 29491 RVA: 0x00232FCC File Offset: 0x002311CC
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

		// Token: 0x170011A5 RID: 4517
		// (get) Token: 0x06007334 RID: 29492 RVA: 0x00232FF8 File Offset: 0x002311F8
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

		// Token: 0x170011A6 RID: 4518
		// (get) Token: 0x06007335 RID: 29493 RVA: 0x0004D806 File Offset: 0x0004BA06
		private float ChemicalFallPerTick
		{
			get
			{
				return this.def.fallPerDay / 60000f;
			}
		}

		// Token: 0x06007336 RID: 29494 RVA: 0x0004D819 File Offset: 0x0004BA19
		public Need_Chemical(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.1f);
		}

		// Token: 0x06007337 RID: 29495 RVA: 0x0004D83D File Offset: 0x0004BA3D
		public override void SetInitialLevel()
		{
			base.CurLevelPercentage = Rand.Range(0.8f, 1f);
		}

		// Token: 0x06007338 RID: 29496 RVA: 0x0004D854 File Offset: 0x0004BA54
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				this.CurLevel -= this.ChemicalFallPerTick * 150f;
			}
		}

		// Token: 0x06007339 RID: 29497 RVA: 0x00233054 File Offset: 0x00231254
		private void CategoryChanged()
		{
			Hediff_Addiction addictionHediff = this.AddictionHediff;
			if (addictionHediff != null)
			{
				addictionHediff.Notify_NeedCategoryChanged();
			}
		}

		// Token: 0x04004BE4 RID: 19428
		private const float ThreshDesire = 0.01f;

		// Token: 0x04004BE5 RID: 19429
		private const float ThreshSatisfied = 0.1f;
	}
}
