using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001103 RID: 4355
	public class CompProperties_Biocodable : CompProperties
	{
		// Token: 0x06006890 RID: 26768 RVA: 0x00234ED4 File Offset: 0x002330D4
		public CompProperties_Biocodable()
		{
			this.compClass = typeof(CompBiocodable);
		}

		// Token: 0x04003A98 RID: 15000
		public bool biocodeOnEquip;
	}
}
