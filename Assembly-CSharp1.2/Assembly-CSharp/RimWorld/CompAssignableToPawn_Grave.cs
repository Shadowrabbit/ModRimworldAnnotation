using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001794 RID: 6036
	public class CompAssignableToPawn_Grave : CompAssignableToPawn
	{
		// Token: 0x1700149C RID: 5276
		// (get) Token: 0x0600854D RID: 34125 RVA: 0x00275FF8 File Offset: 0x002741F8
		public override IEnumerable<Pawn> AssigningCandidates
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return Enumerable.Empty<Pawn>();
				}
				IEnumerable<Pawn> second = from Corpse x in this.parent.Map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse)
				where x.InnerPawn.IsColonist
				select x.InnerPawn;
				return this.parent.Map.mapPawns.FreeColonistsSpawned.Concat(second);
			}
		}

		// Token: 0x0600854E RID: 34126 RVA: 0x000595B6 File Offset: 0x000577B6
		public override bool AssignedAnything(Pawn pawn)
		{
			return pawn.ownership.AssignedGrave != null;
		}

		// Token: 0x0600854F RID: 34127 RVA: 0x000595C6 File Offset: 0x000577C6
		public override void TryAssignPawn(Pawn pawn)
		{
			pawn.ownership.ClaimGrave((Building_Grave)this.parent);
		}

		// Token: 0x06008550 RID: 34128 RVA: 0x000595DF File Offset: 0x000577DF
		public override void TryUnassignPawn(Pawn pawn, bool sort = true)
		{
			pawn.ownership.UnclaimGrave();
		}

		// Token: 0x06008551 RID: 34129 RVA: 0x000595ED File Offset: 0x000577ED
		protected override bool ShouldShowAssignmentGizmo()
		{
			return !((Building_Grave)this.parent).HasCorpse;
		}

		// Token: 0x06008552 RID: 34130 RVA: 0x00059602 File Offset: 0x00057802
		protected override string GetAssignmentGizmoLabel()
		{
			return "CommandGraveAssignColonistLabel".Translate();
		}

		// Token: 0x06008553 RID: 34131 RVA: 0x00059613 File Offset: 0x00057813
		protected override string GetAssignmentGizmoDesc()
		{
			return "CommandGraveAssignColonistDesc".Translate();
		}

		// Token: 0x06008554 RID: 34132 RVA: 0x00276098 File Offset: 0x00274298
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.assignedPawns.RemoveAll((Pawn x) => x.ownership.AssignedGrave != this.parent) > 0)
			{
				Log.Warning(this.parent.ToStringSafe<ThingWithComps>() + " had pawns assigned that don't have it as an assigned grave. Removing.", false);
			}
		}
	}
}
