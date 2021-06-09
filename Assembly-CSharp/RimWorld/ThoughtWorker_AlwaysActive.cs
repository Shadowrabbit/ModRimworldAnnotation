using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EC9 RID: 3785
	public class ThoughtWorker_AlwaysActive : ThoughtWorker
	{
		// Token: 0x060053E4 RID: 21476 RVA: 0x0003A557 File Offset: 0x00038757
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return true;
		}

		// Token: 0x060053E5 RID: 21477 RVA: 0x0003A557 File Offset: 0x00038757
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
		{
			return true;
		}
	}
}
