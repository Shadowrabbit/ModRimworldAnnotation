using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BD0 RID: 7120
	public class Instruction_ExpandAreaBuildRoof : Instruction_ExpandArea
	{
		// Token: 0x1700189B RID: 6299
		// (get) Token: 0x06009CC1 RID: 40129 RVA: 0x00068360 File Offset: 0x00066560
		protected override Area MyArea
		{
			get
			{
				return base.Map.areaManager.BuildRoof;
			}
		}
	}
}
