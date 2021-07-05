using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020010FE RID: 4350
	public class CompAssignableToPawn_Throne : CompAssignableToPawn
	{
		// Token: 0x06006853 RID: 26707 RVA: 0x00234504 File Offset: 0x00232704
		protected override string GetAssignmentGizmoDesc()
		{
			return "CommandThroneSetOwnerDesc".Translate();
		}

		// Token: 0x170011D0 RID: 4560
		// (get) Token: 0x06006854 RID: 26708 RVA: 0x00234518 File Offset: 0x00232718
		public override IEnumerable<Pawn> AssigningCandidates
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return Enumerable.Empty<Pawn>();
				}
				return from p in this.parent.Map.mapPawns.FreeColonists.Where(delegate(Pawn p)
				{
					Faction faction;
					return p.royalty != null && (p.royalty.AllTitlesForReading.Any<RoyalTitle>() || p.royalty.CanUpdateTitleOfAnyFaction(out faction));
				})
				orderby this.CanAssignTo(p).Accepted descending
				select p;
			}
		}

		// Token: 0x06006855 RID: 26709 RVA: 0x00234584 File Offset: 0x00232784
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

		// Token: 0x06006856 RID: 26710 RVA: 0x0023460A File Offset: 0x0023280A
		public override bool AssignedAnything(Pawn pawn)
		{
			return pawn.ownership.AssignedThrone != null;
		}

		// Token: 0x06006857 RID: 26711 RVA: 0x0023461A File Offset: 0x0023281A
		public override void TryAssignPawn(Pawn pawn)
		{
			pawn.ownership.ClaimThrone((Building_Throne)this.parent);
		}

		// Token: 0x06006858 RID: 26712 RVA: 0x00234633 File Offset: 0x00232833
		public override void TryUnassignPawn(Pawn pawn, bool sort = true)
		{
			pawn.ownership.UnclaimThrone();
		}

		// Token: 0x06006859 RID: 26713 RVA: 0x00234644 File Offset: 0x00232844
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.assignedPawns.RemoveAll((Pawn x) => x.ownership.AssignedThrone != this.parent) > 0)
			{
				Log.Warning(this.parent.ToStringSafe<ThingWithComps>() + " had pawns assigned that don't have it as an assigned throne. Removing.");
			}
		}
	}
}
