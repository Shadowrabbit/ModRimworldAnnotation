using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F52 RID: 3922
	public class OutcomeChance
	{
		// Token: 0x1700101D RID: 4125
		// (get) Token: 0x06005D17 RID: 23831 RVA: 0x001FF701 File Offset: 0x001FD901
		public bool Positive
		{
			get
			{
				return this.positivityIndex >= 0;
			}
		}

		// Token: 0x06005D18 RID: 23832 RVA: 0x001FF710 File Offset: 0x001FD910
		public bool BestPositiveOutcome(LordJob_Ritual ritual)
		{
			using (List<OutcomeChance>.Enumerator enumerator = ritual.Ritual.outcomeEffect.def.outcomeChances.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.positivityIndex > this.positivityIndex)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x040035E2 RID: 13794
		[MustTranslate]
		public string label;

		// Token: 0x040035E3 RID: 13795
		[MustTranslate]
		public string description;

		// Token: 0x040035E4 RID: 13796
		[MustTranslate]
		public string potentialExtraOutcomeDesc;

		// Token: 0x040035E5 RID: 13797
		public float chance;

		// Token: 0x040035E6 RID: 13798
		public ThoughtDef memory;

		// Token: 0x040035E7 RID: 13799
		public int positivityIndex;

		// Token: 0x040035E8 RID: 13800
		[NoTranslate]
		public List<string> roleIdsNotGainingMemory;

		// Token: 0x040035E9 RID: 13801
		public float ideoCertaintyOffset;
	}
}
