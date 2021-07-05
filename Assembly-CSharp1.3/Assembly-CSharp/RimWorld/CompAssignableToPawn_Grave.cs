using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001100 RID: 4352
	public class CompAssignableToPawn_Grave : CompAssignableToPawn
	{
		// Token: 0x170011D2 RID: 4562
		// (get) Token: 0x0600686A RID: 26730 RVA: 0x002349B4 File Offset: 0x00232BB4
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

		// Token: 0x0600686B RID: 26731 RVA: 0x00234A53 File Offset: 0x00232C53
		public override bool AssignedAnything(Pawn pawn)
		{
			return pawn.ownership.AssignedGrave != null;
		}

		// Token: 0x0600686C RID: 26732 RVA: 0x00234A63 File Offset: 0x00232C63
		public override void TryAssignPawn(Pawn pawn)
		{
			pawn.ownership.ClaimGrave((Building_Grave)this.parent);
		}

		// Token: 0x0600686D RID: 26733 RVA: 0x00234A7C File Offset: 0x00232C7C
		public override void TryUnassignPawn(Pawn pawn, bool sort = true)
		{
			pawn.ownership.UnclaimGrave();
		}

		// Token: 0x0600686E RID: 26734 RVA: 0x00234A8A File Offset: 0x00232C8A
		protected override bool ShouldShowAssignmentGizmo()
		{
			return !((Building_Grave)this.parent).HasCorpse;
		}

		// Token: 0x0600686F RID: 26735 RVA: 0x00234A9F File Offset: 0x00232C9F
		protected override string GetAssignmentGizmoLabel()
		{
			return "CommandGraveAssignColonistLabel".Translate();
		}

		// Token: 0x06006870 RID: 26736 RVA: 0x00234AB0 File Offset: 0x00232CB0
		protected override string GetAssignmentGizmoDesc()
		{
			return "CommandGraveAssignColonistDesc".Translate();
		}

		// Token: 0x06006871 RID: 26737 RVA: 0x00234AC4 File Offset: 0x00232CC4
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.assignedPawns.RemoveAll((Pawn x) => x.ownership.AssignedGrave != this.parent) > 0)
			{
				Log.Warning(this.parent.ToStringSafe<ThingWithComps>() + " had pawns assigned that don't have it as an assigned grave. Removing.");
			}
		}
	}
}
