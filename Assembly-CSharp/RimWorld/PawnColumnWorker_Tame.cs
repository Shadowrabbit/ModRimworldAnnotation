using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B66 RID: 7014
	public class PawnColumnWorker_Tame : PawnColumnWorker_Designator
	{
		// Token: 0x17001862 RID: 6242
		// (get) Token: 0x06009A98 RID: 39576 RVA: 0x000618B9 File Offset: 0x0005FAB9
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Tame;
			}
		}

		// Token: 0x06009A99 RID: 39577 RVA: 0x00066EFE File Offset: 0x000650FE
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorTameDesc".Translate();
		}

		// Token: 0x06009A9A RID: 39578 RVA: 0x00066E5B File Offset: 0x0006505B
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.AnimalOrWildMan() && pawn.RaceProps.IsFlesh && (pawn.Faction == null || !pawn.Faction.def.humanlikeFaction) && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x06009A9B RID: 39579 RVA: 0x00066F0F File Offset: 0x0006510F
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			pawn.MapHeld.designationManager.TryRemoveDesignationOn(pawn, DesignationDefOf.Hunt);
			TameUtility.ShowDesignationWarnings(pawn, false);
		}
	}
}
