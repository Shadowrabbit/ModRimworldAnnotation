using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020010FD RID: 4349
	public class CompAssignableToPawn_MeditationSpot : CompAssignableToPawn
	{
		// Token: 0x06006849 RID: 26697 RVA: 0x00234369 File Offset: 0x00232569
		protected override string GetAssignmentGizmoDesc()
		{
			return "CommandMeditationSpotSetOwnerDesc".Translate();
		}

		// Token: 0x170011CF RID: 4559
		// (get) Token: 0x0600684A RID: 26698 RVA: 0x0023437A File Offset: 0x0023257A
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

		// Token: 0x0600684B RID: 26699 RVA: 0x002343B8 File Offset: 0x002325B8
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

		// Token: 0x0600684C RID: 26700 RVA: 0x0023443E File Offset: 0x0023263E
		public override bool AssignedAnything(Pawn pawn)
		{
			return pawn.ownership.AssignedMeditationSpot != null;
		}

		// Token: 0x0600684D RID: 26701 RVA: 0x0023444E File Offset: 0x0023264E
		public override void TryAssignPawn(Pawn pawn)
		{
			pawn.ownership.ClaimMeditationSpot((Building)this.parent);
		}

		// Token: 0x0600684E RID: 26702 RVA: 0x00234467 File Offset: 0x00232667
		public override void TryUnassignPawn(Pawn pawn, bool sort = true)
		{
			pawn.ownership.UnclaimMeditationSpot();
		}

		// Token: 0x0600684F RID: 26703 RVA: 0x00234478 File Offset: 0x00232678
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.assignedPawns.RemoveAll((Pawn x) => x.ownership.AssignedMeditationSpot != this.parent) > 0)
			{
				Log.Warning(this.parent.ToStringSafe<ThingWithComps>() + " had pawns assigned that don't have it as an assigned meditation spot. Removing.");
			}
		}
	}
}
