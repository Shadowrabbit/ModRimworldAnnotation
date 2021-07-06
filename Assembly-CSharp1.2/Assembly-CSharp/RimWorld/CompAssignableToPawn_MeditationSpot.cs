using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001790 RID: 6032
	public class CompAssignableToPawn_MeditationSpot : CompAssignableToPawn
	{
		// Token: 0x0600852E RID: 34094 RVA: 0x0005942B File Offset: 0x0005762B
		protected override string GetAssignmentGizmoDesc()
		{
			return "CommandMeditationSpotSetOwnerDesc".Translate();
		}

		// Token: 0x1700149A RID: 5274
		// (get) Token: 0x0600852F RID: 34095 RVA: 0x0005943C File Offset: 0x0005763C
		public override IEnumerable<Pawn> AssigningCandidates
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return Enumerable.Empty<Pawn>();
				}
				return from p in this.parent.Map.mapPawns.FreeColonists
				orderby this.CanAssignTo(p).Accepted descending
				select p;
			}
		}

		// Token: 0x06008530 RID: 34096 RVA: 0x00275DAC File Offset: 0x00273FAC
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

		// Token: 0x06008531 RID: 34097 RVA: 0x00059477 File Offset: 0x00057677
		public override bool AssignedAnything(Pawn pawn)
		{
			return pawn.ownership.AssignedMeditationSpot != null;
		}

		// Token: 0x06008532 RID: 34098 RVA: 0x00059487 File Offset: 0x00057687
		public override void TryAssignPawn(Pawn pawn)
		{
			pawn.ownership.ClaimMeditationSpot((Building)this.parent);
		}

		// Token: 0x06008533 RID: 34099 RVA: 0x000594A0 File Offset: 0x000576A0
		public override void TryUnassignPawn(Pawn pawn, bool sort = true)
		{
			pawn.ownership.UnclaimMeditationSpot();
		}

		// Token: 0x06008534 RID: 34100 RVA: 0x00275E34 File Offset: 0x00274034
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.assignedPawns.RemoveAll((Pawn x) => x.ownership.AssignedMeditationSpot != this.parent) > 0)
			{
				Log.Warning(this.parent.ToStringSafe<ThingWithComps>() + " had pawns assigned that don't have it as an assigned meditation spot. Removing.", false);
			}
		}
	}
}
