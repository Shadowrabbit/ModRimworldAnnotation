using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200137A RID: 4986
	public class PawnColumnWorker_Hunt : PawnColumnWorker_Designator
	{
		// Token: 0x1700155A RID: 5466
		// (get) Token: 0x06007951 RID: 31057 RVA: 0x00262A82 File Offset: 0x00260C82
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Hunt;
			}
		}

		// Token: 0x06007952 RID: 31058 RVA: 0x002AF79F File Offset: 0x002AD99F
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorHuntDesc".Translate();
		}

		// Token: 0x06007953 RID: 31059 RVA: 0x002AF7B0 File Offset: 0x002AD9B0
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.AnimalOrWildMan() && pawn.RaceProps.IsFlesh && (pawn.Faction == null || !pawn.Faction.def.humanlikeFaction) && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x06007954 RID: 31060 RVA: 0x002AF7E9 File Offset: 0x002AD9E9
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			pawn.MapHeld.designationManager.TryRemoveDesignationOn(pawn, DesignationDefOf.Tame);
		}
	}
}
