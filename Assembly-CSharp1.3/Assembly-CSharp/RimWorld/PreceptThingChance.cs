using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AA5 RID: 2725
	public struct PreceptThingChance
	{
		// Token: 0x060040C8 RID: 16584 RVA: 0x0015DDE4 File Offset: 0x0015BFE4
		public static implicit operator PreceptThingChance(PreceptThingChanceClass c)
		{
			return new PreceptThingChance
			{
				chance = c.chance,
				def = c.def
			};
		}

		// Token: 0x040025B4 RID: 9652
		public ThingDef def;

		// Token: 0x040025B5 RID: 9653
		public float chance;
	}
}
