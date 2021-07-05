using System;

namespace Verse
{
	// Token: 0x020001B1 RID: 433
	public static class EdificeUtility
	{
		// Token: 0x06000C30 RID: 3120 RVA: 0x000419D8 File Offset: 0x0003FBD8
		public static bool IsEdifice(this BuildableDef def)
		{
			ThingDef thingDef = def as ThingDef;
			return thingDef != null && thingDef.category == ThingCategory.Building && thingDef.building.isEdifice;
		}
	}
}
