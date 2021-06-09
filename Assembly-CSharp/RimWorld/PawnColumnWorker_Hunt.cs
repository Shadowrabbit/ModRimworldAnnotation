using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B64 RID: 7012
	public class PawnColumnWorker_Hunt : PawnColumnWorker_Designator
	{
		// Token: 0x17001860 RID: 6240
		// (get) Token: 0x06009A8E RID: 39566 RVA: 0x000614A0 File Offset: 0x0005F6A0
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Hunt;
			}
		}

		// Token: 0x06009A8F RID: 39567 RVA: 0x00066E4A File Offset: 0x0006504A
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorHuntDesc".Translate();
		}

		// Token: 0x06009A90 RID: 39568 RVA: 0x00066E5B File Offset: 0x0006505B
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.AnimalOrWildMan() && pawn.RaceProps.IsFlesh && (pawn.Faction == null || !pawn.Faction.def.humanlikeFaction) && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x06009A91 RID: 39569 RVA: 0x00066E94 File Offset: 0x00065094
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			pawn.MapHeld.designationManager.TryRemoveDesignationOn(pawn, DesignationDefOf.Tame);
		}
	}
}
