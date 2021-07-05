using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001974 RID: 6516
	public class Alert_TatteredApparel : Alert_Thought
	{
		// Token: 0x170016C6 RID: 5830
		// (get) Token: 0x06009010 RID: 36880 RVA: 0x00060B3F File Offset: 0x0005ED3F
		protected override ThoughtDef Thought
		{
			get
			{
				return ThoughtDefOf.ApparelDamaged;
			}
		}

		// Token: 0x06009011 RID: 36881 RVA: 0x00060B46 File Offset: 0x0005ED46
		public Alert_TatteredApparel()
		{
			this.defaultLabel = "AlertTatteredApparel".Translate();
			this.explanationKey = "AlertTatteredApparelDesc";
		}
	}
}
