using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EB3 RID: 3763
	public class ThoughtWorker_GameCondition : ThoughtWorker
	{
		// Token: 0x060053B7 RID: 21431 RVA: 0x001C18DC File Offset: 0x001BFADC
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.SpawnedOrAnyParentSpawned && p.MapHeld.gameConditionManager.ConditionIsActive(this.def.gameCondition))
			{
				return true;
			}
			if (Find.World.gameConditionManager.ConditionIsActive(this.def.gameCondition))
			{
				return true;
			}
			return false;
		}
	}
}
