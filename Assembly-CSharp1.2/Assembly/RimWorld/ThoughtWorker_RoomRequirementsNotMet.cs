using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ED0 RID: 3792
	public abstract class ThoughtWorker_RoomRequirementsNotMet : ThoughtWorker
	{
		// Token: 0x0600540C RID: 21516
		protected abstract IEnumerable<string> UnmetRequirements(Pawn p);

		// Token: 0x0600540D RID: 21517 RVA: 0x0003A63F File Offset: 0x0003883F
		protected bool Active(Pawn p)
		{
			return p.royalty != null && p.MapHeld != null && p.MapHeld.IsPlayerHome && this.UnmetRequirements(p).Any<string>();
		}

		// Token: 0x0600540E RID: 21518 RVA: 0x0003A66C File Offset: 0x0003886C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!this.Active(p))
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(0);
		}
	}
}
