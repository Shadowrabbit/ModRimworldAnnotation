using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001793 RID: 6035
	public class CompAssignableToPawn_Bed : CompAssignableToPawn
	{
		// Token: 0x06008545 RID: 34117 RVA: 0x00059556 File Offset: 0x00057756
		protected override string GetAssignmentGizmoDesc()
		{
			return "CommandBedSetOwnerDesc".Translate();
		}

		// Token: 0x06008546 RID: 34118 RVA: 0x00059567 File Offset: 0x00057767
		public override bool AssignedAnything(Pawn pawn)
		{
			return pawn.ownership.OwnedBed != null;
		}

		// Token: 0x06008547 RID: 34119 RVA: 0x00059577 File Offset: 0x00057777
		public override void TryAssignPawn(Pawn pawn)
		{
			pawn.ownership.ClaimBedIfNonMedical((Building_Bed)this.parent);
		}

		// Token: 0x06008548 RID: 34120 RVA: 0x00059590 File Offset: 0x00057790
		public override void TryUnassignPawn(Pawn pawn, bool sort = true)
		{
			pawn.ownership.UnclaimBed();
		}

		// Token: 0x06008549 RID: 34121 RVA: 0x00275F5C File Offset: 0x0027415C
		protected override bool ShouldShowAssignmentGizmo()
		{
			Building_Bed building_Bed = (Building_Bed)this.parent;
			return building_Bed.def.building.bed_humanlike && building_Bed.Faction == Faction.OfPlayer && !building_Bed.ForPrisoners && !building_Bed.Medical;
		}

		// Token: 0x0600854A RID: 34122 RVA: 0x00275FA8 File Offset: 0x002741A8
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.assignedPawns.RemoveAll((Pawn x) => x.ownership.OwnedBed != this.parent) > 0)
			{
				Log.Warning(this.parent.ToStringSafe<ThingWithComps>() + " had pawns assigned that don't have it as an assigned bed. Removing.", false);
			}
		}
	}
}
