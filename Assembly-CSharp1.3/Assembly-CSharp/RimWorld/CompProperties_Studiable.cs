using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011AB RID: 4523
	public class CompProperties_Studiable : CompProperties
	{
		// Token: 0x06006CF3 RID: 27891 RVA: 0x0024918D File Offset: 0x0024738D
		public CompProperties_Studiable()
		{
			this.compClass = typeof(CompStudiable);
		}

		// Token: 0x04003C9D RID: 15517
		public int cost = 100;
	}
}
