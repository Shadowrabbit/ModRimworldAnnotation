using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B97 RID: 2967
	public abstract class QuestPart_RequirementsToAccept : QuestPart
	{
		// Token: 0x17000C1D RID: 3101
		// (get) Token: 0x06004558 RID: 17752 RVA: 0x0016FF8B File Offset: 0x0016E18B
		public virtual IEnumerable<GlobalTargetInfo> Culprits
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x06004559 RID: 17753
		public abstract AcceptanceReport CanAccept();

		// Token: 0x0600455A RID: 17754 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool CanPawnAccept(Pawn p)
		{
			return true;
		}
	}
}
