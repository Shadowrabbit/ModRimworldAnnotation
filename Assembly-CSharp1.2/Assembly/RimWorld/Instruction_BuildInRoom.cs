using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BC3 RID: 7107
	public class Instruction_BuildInRoom : Instruction_BuildAtRoom
	{
		// Token: 0x1700188F RID: 6287
		// (get) Token: 0x06009C80 RID: 40064 RVA: 0x00067FB4 File Offset: 0x000661B4
		protected override CellRect BuildableRect
		{
			get
			{
				return Find.TutorialState.roomRect.ContractedBy(1);
			}
		}
	}
}
