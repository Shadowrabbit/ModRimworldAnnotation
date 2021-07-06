using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x020001AC RID: 428
	public class ToolCapacityDef : Def
	{
		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x0000E890 File Offset: 0x0000CA90
		public IEnumerable<ManeuverDef> Maneuvers
		{
			get
			{
				return from x in DefDatabase<ManeuverDef>.AllDefsListForReading
				where x.requiredCapacity == this
				select x;
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x0000E8A8 File Offset: 0x0000CAA8
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
