using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BC4 RID: 7108
	public class Instruction_BuildNearRoom : Instruction_BuildAtRoom
	{
		// Token: 0x17001890 RID: 6288
		// (get) Token: 0x06009C82 RID: 40066 RVA: 0x00067FCE File Offset: 0x000661CE
		protected override CellRect BuildableRect
		{
			get
			{
				return Find.TutorialState.roomRect.ExpandedBy(10);
			}
		}

		// Token: 0x06009C83 RID: 40067 RVA: 0x00067FE1 File Offset: 0x000661E1
		protected override bool AllowBuildAt(IntVec3 c)
		{
			return base.AllowBuildAt(c) && !Find.TutorialState.roomRect.Contains(c);
		}
	}
}
