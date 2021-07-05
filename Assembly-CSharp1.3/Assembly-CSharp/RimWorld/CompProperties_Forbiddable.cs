using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A15 RID: 2581
	public class CompProperties_Forbiddable : CompProperties
	{
		// Token: 0x06003F0E RID: 16142 RVA: 0x00158033 File Offset: 0x00156233
		public CompProperties_Forbiddable()
		{
			this.compClass = typeof(CompForbiddable);
		}

		// Token: 0x04002222 RID: 8738
		public bool allowNonPlayer;
	}
}
