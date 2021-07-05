using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010FB RID: 4347
	public abstract class QuestPart_RequirementsToAccept : QuestPart
	{
		// Token: 0x17000EC1 RID: 3777
		// (get) Token: 0x06005EFB RID: 24315 RVA: 0x00041BAA File Offset: 0x0003FDAA
		public virtual IEnumerable<GlobalTargetInfo> Culprits
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x06005EFC RID: 24316
		public abstract AcceptanceReport CanAccept();

		// Token: 0x06005EFD RID: 24317 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool CanPawnAccept(Pawn p)
		{
			return true;
		}
	}
}
