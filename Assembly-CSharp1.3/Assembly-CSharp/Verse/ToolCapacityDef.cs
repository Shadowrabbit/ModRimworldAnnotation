using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x0200011B RID: 283
	public class ToolCapacityDef : Def
	{
		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060007A8 RID: 1960 RVA: 0x00023AB2 File Offset: 0x00021CB2
		public IEnumerable<ManeuverDef> Maneuvers
		{
			get
			{
				return from x in DefDatabase<ManeuverDef>.AllDefsListForReading
				where x.requiredCapacity == this
				select x;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060007A9 RID: 1961 RVA: 0x00023ACA File Offset: 0x00021CCA
		public IEnumerable<VerbProperties> VerbsProperties
		{
			get
			{
				return from x in this.Maneuvers
				select x.verb;
			}
		}
	}
}
