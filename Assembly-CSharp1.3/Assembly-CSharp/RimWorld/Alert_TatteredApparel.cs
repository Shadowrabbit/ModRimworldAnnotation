using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200128F RID: 4751
	public class Alert_TatteredApparel : Alert_Thought
	{
		// Token: 0x170013C8 RID: 5064
		// (get) Token: 0x06007175 RID: 29045 RVA: 0x0025D3EB File Offset: 0x0025B5EB
		protected override ThoughtDef Thought
		{
			get
			{
				return ThoughtDefOf.ApparelDamaged;
			}
		}

		// Token: 0x06007176 RID: 29046 RVA: 0x0025D3F2 File Offset: 0x0025B5F2
		public Alert_TatteredApparel()
		{
			this.defaultLabel = "AlertTatteredApparel".Translate();
			this.explanationKey = "AlertTatteredApparelDesc";
		}
	}
}
