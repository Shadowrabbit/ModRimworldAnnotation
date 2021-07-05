using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C4 RID: 2500
	public abstract class ThoughtWorker_RoomRequirementsNotMet : ThoughtWorker
	{
		// Token: 0x06003E20 RID: 15904
		protected abstract IEnumerable<string> UnmetRequirements(Pawn p);

		// Token: 0x06003E21 RID: 15905 RVA: 0x001547CB File Offset: 0x001529CB
		protected bool Active(Pawn p)
		{
			return p.royalty != null && p.MapHeld != null && p.MapHeld.IsPlayerHome && this.UnmetRequirements(p).Any<string>();
		}

		// Token: 0x06003E22 RID: 15906 RVA: 0x001547F8 File Offset: 0x001529F8
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
