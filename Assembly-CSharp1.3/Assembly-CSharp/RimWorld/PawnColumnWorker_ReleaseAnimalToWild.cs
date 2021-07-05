using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200137B RID: 4987
	public class PawnColumnWorker_ReleaseAnimalToWild : PawnColumnWorker_Designator
	{
		// Token: 0x1700155B RID: 5467
		// (get) Token: 0x06007956 RID: 31062 RVA: 0x002AF809 File Offset: 0x002ADA09
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.ReleaseAnimalToWild;
			}
		}

		// Token: 0x06007957 RID: 31063 RVA: 0x002AF810 File Offset: 0x002ADA10
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorReleaseAnimalToWildDesc".Translate();
		}

		// Token: 0x06007958 RID: 31064 RVA: 0x002AF821 File Offset: 0x002ADA21
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.RaceProps.IsFlesh && pawn.Faction == Faction.OfPlayer && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x06007959 RID: 31065 RVA: 0x002AF852 File Offset: 0x002ADA52
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			ReleaseAnimalToWildUtility.CheckWarnAboutBondedAnimal(pawn);
		}
	}
}
