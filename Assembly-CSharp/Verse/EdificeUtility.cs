using System;

namespace Verse
{
	// Token: 0x02000268 RID: 616
	public static class EdificeUtility
	{
		// Token: 0x06000FAE RID: 4014 RVA: 0x000B73F8 File Offset: 0x000B55F8
		public static bool IsEdifice(this BuildableDef def)
		{
			ThingDef thingDef = def as ThingDef;
			return thingDef != null && thingDef.category == ThingCategory.Building && thingDef.building.isEdifice;
		}
	}
}
