using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001791 RID: 6033
	public class CompAssignableToPawn_Throne : CompAssignableToPawn
	{
		// Token: 0x06008538 RID: 34104 RVA: 0x000594CE File Offset: 0x000576CE
		protected override string GetAssignmentGizmoDesc()
		{
			return "CommandThroneSetOwnerDesc".Translate();
		}

		// Token: 0x1700149B RID: 5275
		// (get) Token: 0x06008539 RID: 34105 RVA: 0x00275EA0 File Offset: 0x002740A0
		public override IEnumerable<Pawn> AssigningCandidates
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return Enumerable.Empty<Pawn>();
				}
				return from p in this.parent.Map.mapPawns.FreeColonists
				where p.royalty != null && p.royalty.AllTitlesForReading.Any<RoyalTitle>()
				orderby this.CanAssignTo(p).Accepted descending
				select p;
			}
		}

		// Token: 0x0600853A RID: 34106 RVA: 0x00275DAC File Offset: 0x00273FAC
		public override string CompInspectStringExtra()
		{
			if (base.AssignedPawnsForReading.Count == 0)
			{
				return "Owner".Translate() + ": " + "Nobody".Translate();
			}
			if (base.AssignedPawnsForReading.Count == 1)
			{
				return "Owner".Translate() + ": " + base.AssignedPawnsForReading[0].Label;
			}
			return "";
		}

		// Token: 0x0600853B RID: 34107 RVA: 0x000594DF File Offset: 0x000576DF
		public override bool AssignedAnything(Pawn pawn)
		{
			return pawn.ownership.AssignedThrone != null;
		}

		// Token: 0x0600853C RID: 34108 RVA: 0x000594EF File Offset: 0x000576EF
		public override void TryAssignPawn(Pawn pawn)
		{
			pawn.ownership.ClaimThrone((Building_Throne)this.parent);
		}

		// Token: 0x0600853D RID: 34109 RVA: 0x00059508 File Offset: 0x00057708
		public override void TryUnassignPawn(Pawn pawn, bool sort = true)
		{
			pawn.ownership.UnclaimThrone();
		}

		// Token: 0x0600853E RID: 34110 RVA: 0x00275F0C File Offset: 0x0027410C
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.assignedPawns.RemoveAll((Pawn x) => x.ownership.AssignedThrone != this.parent) > 0)
			{
				Log.Warning(this.parent.ToStringSafe<ThingWithComps>() + " had pawns assigned that don't have it as an assigned throne. Removing.", false);
			}
		}
	}
}
