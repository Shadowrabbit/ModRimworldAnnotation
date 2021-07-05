using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013C1 RID: 5057
	public class Instruction_BuildInRoom : Instruction_BuildAtRoom
	{
		// Token: 0x17001585 RID: 5509
		// (get) Token: 0x06007AFF RID: 31487 RVA: 0x002B71E3 File Offset: 0x002B53E3
		protected override CellRect BuildableRect
		{
			get
			{
				return Find.TutorialState.roomRect.ContractedBy(1);
			}
		}
	}
}
