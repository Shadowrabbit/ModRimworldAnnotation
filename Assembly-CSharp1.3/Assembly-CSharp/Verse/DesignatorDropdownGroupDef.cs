using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x020000C2 RID: 194
	public class DesignatorDropdownGroupDef : Def
	{
		// Token: 0x060005E0 RID: 1504 RVA: 0x0001E1DF File Offset: 0x0001C3DF
		public IEnumerable<BuildableDef> BuildablesWithoutDefaultDesignators()
		{
			return from x in DefDatabase<ThingDef>.AllDefs.Concat(DefDatabase<TerrainDef>.AllDefs)
			where x.designatorDropdown == this && !x.canGenerateDefaultDesignator
			select x;
		}
	}
}
