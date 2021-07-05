using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020010FF RID: 4351
	public class CompAssignableToPawn_Bed : CompAssignableToPawn
	{
		// Token: 0x170011D1 RID: 4561
		// (get) Token: 0x0600685D RID: 26717 RVA: 0x002346C8 File Offset: 0x002328C8
		public override IEnumerable<Pawn> AssigningCandidates
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return Enumerable.Empty<Pawn>();
				}
				if (!this.parent.def.building.bed_humanlike)
				{
					return from p in this.parent.Map.mapPawns.SpawnedColonyAnimals
					orderby this.CanAssignTo(p).Accepted descending
					select p;
				}
				return from p in this.parent.Map.mapPawns.FreeColonists
				orderby this.CanAssignTo(p).Accepted && !this.IdeoligionForbids(p) descending
				select p;
			}
		}

		// Token: 0x0600685E RID: 26718 RVA: 0x0023474C File Offset: 0x0023294C
		protected override string GetAssignmentGizmoDesc()
		{
			return "CommandBedSetOwnerDesc".Translate();
		}

		// Token: 0x0600685F RID: 26719 RVA: 0x0023475D File Offset: 0x0023295D
		public override bool AssignedAnything(Pawn pawn)
		{
			return pawn.ownership.OwnedBed != null;
		}

		// Token: 0x06006860 RID: 26720 RVA: 0x00234770 File Offset: 0x00232970
		public override void TryAssignPawn(Pawn pawn)
		{
			Building_Bed building_Bed = (Building_Bed)this.parent;
			pawn.ownership.ClaimBedIfNonMedical(building_Bed);
			building_Bed.NotifyRoomAssignedPawnsChanged();
		}

		// Token: 0x06006861 RID: 26721 RVA: 0x0023479C File Offset: 0x0023299C
		public override void TryUnassignPawn(Pawn pawn, bool sort = true)
		{
			Building_Bed ownedBed = pawn.ownership.OwnedBed;
			pawn.ownership.UnclaimBed();
			if (ownedBed != null)
			{
				ownedBed.NotifyRoomAssignedPawnsChanged();
			}
		}

		// Token: 0x06006862 RID: 26722 RVA: 0x002347CC File Offset: 0x002329CC
		protected override bool ShouldShowAssignmentGizmo()
		{
			Building_Bed building_Bed = (Building_Bed)this.parent;
			return building_Bed.Faction == Faction.OfPlayer && !building_Bed.ForPrisoners && !building_Bed.Medical;
		}

		// Token: 0x06006863 RID: 26723 RVA: 0x00234808 File Offset: 0x00232A08
		public override AcceptanceReport CanAssignTo(Pawn pawn)
		{
			Building_Bed building_Bed = (Building_Bed)this.parent;
			if (pawn.BodySize > building_Bed.def.building.bed_maxBodySize)
			{
				return "TooLargeForBed".Translate();
			}
			if (building_Bed.ForSlaves && !pawn.IsSlave)
			{
				return "CannotAssignBedToColonist".Translate();
			}
			if (building_Bed.ForColonists && pawn.IsSlave)
			{
				return "CannotAssignBedToSlave".Translate();
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06006864 RID: 26724 RVA: 0x00234890 File Offset: 0x00232A90
		public override bool IdeoligionForbids(Pawn pawn)
		{
			if (!ModsConfig.IdeologyActive || base.Props.maxAssignedPawnsCount == 1)
			{
				return base.IdeoligionForbids(pawn);
			}
			foreach (Pawn pawn2 in base.AssignedPawns)
			{
				if (!BedUtility.WillingToShareBed(pawn, pawn2))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006865 RID: 26725 RVA: 0x00234904 File Offset: 0x00232B04
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.assignedPawns.RemoveAll((Pawn x) => x.ownership.OwnedBed != this.parent) > 0)
			{
				Log.Warning(this.parent.ToStringSafe<ThingWithComps>() + " had pawns assigned that don't have it as an assigned bed. Removing.");
			}
		}
	}
}
