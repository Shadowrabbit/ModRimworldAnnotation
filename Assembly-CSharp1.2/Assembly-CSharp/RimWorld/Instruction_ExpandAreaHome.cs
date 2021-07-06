using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BCF RID: 7119
	public class Instruction_ExpandAreaHome : Instruction_ExpandArea
	{
		// Token: 0x1700189A RID: 6298
		// (get) Token: 0x06009CBF RID: 40127 RVA: 0x00068346 File Offset: 0x00066546
		protected override Area MyArea
		{
			get
			{
				return base.Map.areaManager.Home;
			}
		}
	}
}
