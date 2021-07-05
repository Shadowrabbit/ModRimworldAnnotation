using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200137D RID: 4989
	public class PawnColumnWorker_Tame : PawnColumnWorker_Designator
	{
		// Token: 0x1700155D RID: 5469
		// (get) Token: 0x06007960 RID: 31072 RVA: 0x002643A7 File Offset: 0x002625A7
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Tame;
			}
		}

		// Token: 0x06007961 RID: 31073 RVA: 0x002AF873 File Offset: 0x002ADA73
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorTameDesc".Translate();
		}

		// Token: 0x06007962 RID: 31074 RVA: 0x002AF884 File Offset: 0x002ADA84
		protected override bool HasCheckbox(Pawn pawn)
		{
			return TameUtility.CanTame(pawn) && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x06007963 RID: 31075 RVA: 0x002AF896 File Offset: 0x002ADA96
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			pawn.MapHeld.designationManager.TryRemoveDesignationOn(pawn, DesignationDefOf.Hunt);
			TameUtility.ShowDesignationWarnings(pawn, false);
		}
	}
}
