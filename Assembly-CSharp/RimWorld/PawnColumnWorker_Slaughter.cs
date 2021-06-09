using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B65 RID: 7013
	public class PawnColumnWorker_Slaughter : PawnColumnWorker_Designator
	{
		// Token: 0x17001861 RID: 6241
		// (get) Token: 0x06009A93 RID: 39571 RVA: 0x000616FA File Offset: 0x0005F8FA
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Slaughter;
			}
		}

		// Token: 0x06009A94 RID: 39572 RVA: 0x00066EB4 File Offset: 0x000650B4
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorSlaughterDesc".Translate();
		}

		// Token: 0x06009A95 RID: 39573 RVA: 0x00066EC5 File Offset: 0x000650C5
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.RaceProps.IsFlesh && pawn.Faction == Faction.OfPlayer && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x06009A96 RID: 39574 RVA: 0x00066EF6 File Offset: 0x000650F6
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			SlaughterDesignatorUtility.CheckWarnAboutBondedAnimal(pawn);
		}
	}
}
