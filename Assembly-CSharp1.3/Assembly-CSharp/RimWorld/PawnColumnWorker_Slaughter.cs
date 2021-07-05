using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200137C RID: 4988
	public class PawnColumnWorker_Slaughter : PawnColumnWorker_Designator
	{
		// Token: 0x1700155C RID: 5468
		// (get) Token: 0x0600795B RID: 31067 RVA: 0x0026399F File Offset: 0x00261B9F
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Slaughter;
			}
		}

		// Token: 0x0600795C RID: 31068 RVA: 0x002AF85A File Offset: 0x002ADA5A
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorSlaughterDesc".Translate();
		}

		// Token: 0x0600795D RID: 31069 RVA: 0x002AF821 File Offset: 0x002ADA21
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.RaceProps.IsFlesh && pawn.Faction == Faction.OfPlayer && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x0600795E RID: 31070 RVA: 0x002AF86B File Offset: 0x002ADA6B
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			SlaughterDesignatorUtility.CheckWarnAboutBondedAnimal(pawn);
		}
	}
}
