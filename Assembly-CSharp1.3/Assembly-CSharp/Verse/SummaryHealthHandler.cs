using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002E2 RID: 738
	public class SummaryHealthHandler
	{
		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x060013F0 RID: 5104 RVA: 0x000717FC File Offset: 0x0006F9FC
		public float SummaryHealthPercent
		{
			get
			{
				if (this.pawn.Dead)
				{
					return 0f;
				}
				if (this.dirty)
				{
					List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
					float num = 1f;
					for (int i = 0; i < hediffs.Count; i++)
					{
						if (!(hediffs[i] is Hediff_MissingPart))
						{
							float num2 = Mathf.Min(hediffs[i].SummaryHealthPercentImpact, 0.95f);
							num *= 1f - num2;
						}
					}
					List<Hediff_MissingPart> missingPartsCommonAncestors = this.pawn.health.hediffSet.GetMissingPartsCommonAncestors();
					for (int j = 0; j < missingPartsCommonAncestors.Count; j++)
					{
						float num3 = Mathf.Min(missingPartsCommonAncestors[j].SummaryHealthPercentImpact, 0.95f);
						num *= 1f - num3;
					}
					this.cachedSummaryHealthPercent = Mathf.Clamp(num, 0.05f, 1f);
					this.dirty = false;
				}
				return this.cachedSummaryHealthPercent;
			}
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x000718F7 File Offset: 0x0006FAF7
		public SummaryHealthHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x00071918 File Offset: 0x0006FB18
		public void Notify_HealthChanged()
		{
			this.dirty = true;
		}

		// Token: 0x04000E89 RID: 3721
		private Pawn pawn;

		// Token: 0x04000E8A RID: 3722
		private float cachedSummaryHealthPercent = 1f;

		// Token: 0x04000E8B RID: 3723
		private bool dirty = true;
	}
}
