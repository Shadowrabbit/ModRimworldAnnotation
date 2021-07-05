using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A9 RID: 2473
	public class ThoughtWorker_GameCondition : ThoughtWorker
	{
		// Token: 0x06003DE0 RID: 15840 RVA: 0x00153A0C File Offset: 0x00151C0C
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
