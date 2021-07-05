using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013C2 RID: 5058
	public class Instruction_BuildNearRoom : Instruction_BuildAtRoom
	{
		// Token: 0x17001586 RID: 5510
		// (get) Token: 0x06007B01 RID: 31489 RVA: 0x002B71FD File Offset: 0x002B53FD
		protected override CellRect BuildableRect
		{
			get
			{
				return Find.TutorialState.roomRect.ExpandedBy(10);
			}
		}

		// Token: 0x06007B02 RID: 31490 RVA: 0x002B7210 File Offset: 0x002B5410
		protected override bool AllowBuildAt(IntVec3 c)
		{
			return base.AllowBuildAt(c) && !Find.TutorialState.roomRect.Contains(c);
		}
	}
}
