using System;

namespace Verse
{
	// Token: 0x02000291 RID: 657
	public class HediffCompProperties_Effecter : HediffCompProperties
	{
		// Token: 0x06001263 RID: 4707 RVA: 0x0006A25C File Offset: 0x0006845C
		public HediffCompProperties_Effecter()
		{
			this.compClass = typeof(HediffComp_Effecter);
		}

		// Token: 0x04000DEA RID: 3562
		public EffecterDef stateEffecter;

		// Token: 0x04000DEB RID: 3563
		public IntRange severityIndices = new IntRange(-1, -1);
	}
}
