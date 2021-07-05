using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000431 RID: 1073
	public class SummaryHealthHandler
	{
		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x060019F7 RID: 6647 RVA: 0x000E4548 File Offset: 0x000E2748
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

		// Token: 0x060019F8 RID: 6648 RVA: 0x0001824F File Offset: 0x0001644F
		public SummaryHealthHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060019F9 RID: 6649 RVA: 0x00018270 File Offset: 0x00016470
		public void Notify_HealthChanged()
		{
			this.dirty = true;
		}

		// Token: 0x0400134E RID: 4942
		private Pawn pawn;

		// Token: 0x0400134F RID: 4943
		private float cachedSummaryHealthPercent = 1f;

		// Token: 0x04001350 RID: 4944
		private bool dirty = true;
	}
}
