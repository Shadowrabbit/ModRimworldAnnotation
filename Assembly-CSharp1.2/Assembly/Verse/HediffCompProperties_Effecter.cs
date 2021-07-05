using System;

namespace Verse
{
	// Token: 0x020003D4 RID: 980
	public class HediffCompProperties_Effecter : HediffCompProperties
	{
		// Token: 0x06001839 RID: 6201 RVA: 0x000170A4 File Offset: 0x000152A4
		public HediffCompProperties_Effecter()
		{
			this.compClass = typeof(HediffComp_Effecter);
		}

		// Token: 0x0400125B RID: 4699
		public EffecterDef stateEffecter;

		// Token: 0x0400125C RID: 4700
		public IntRange severityIndices = new IntRange(-1, -1);
	}
}
