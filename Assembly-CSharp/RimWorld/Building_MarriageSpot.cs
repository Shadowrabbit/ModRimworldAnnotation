using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020016BE RID: 5822
	public class Building_MarriageSpot : Building
	{
		// Token: 0x06007FBE RID: 32702 RVA: 0x0025E0E0 File Offset: 0x0025C2E0
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (stringBuilder.Length != 0)
			{
				stringBuilder.AppendLine();
			}
			stringBuilder.Append(this.UsableNowStatus());
			return stringBuilder.ToString();
		}

		// Token: 0x06007FBF RID: 32703 RVA: 0x0025E124 File Offset: 0x0025C324
		private string UsableNowStatus()
		{
			if (!this.AnyCoupleForWhichIsValid())
			{
				StringBuilder stringBuilder = new StringBuilder();
				Pair<Pawn, Pawn> pair;
				if (this.TryFindAnyFiancesCouple(out pair))
				{
					if (!MarriageSpotUtility.IsValidMarriageSpotFor(base.Position, pair.First, pair.Second, stringBuilder))
					{
						return "MarriageSpotNotUsable".Translate(stringBuilder);
					}
				}
				else if (!MarriageSpotUtility.IsValidMarriageSpot(base.Position, base.Map, stringBuilder))
				{
					return "MarriageSpotNotUsable".Translate(stringBuilder);
				}
			}
			return "MarriageSpotUsable".Translate();
		}

		// Token: 0x06007FC0 RID: 32704 RVA: 0x00055CF7 File Offset: 0x00053EF7
		private bool AnyCoupleForWhichIsValid()
		{
			return base.Map.mapPawns.FreeColonistsSpawned.Any(delegate(Pawn p)
			{
				Pawn firstDirectRelationPawn = p.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance, (Pawn x) => x.Spawned);
				return firstDirectRelationPawn != null && MarriageSpotUtility.IsValidMarriageSpotFor(base.Position, p, firstDirectRelationPawn, null);
			});
		}

		// Token: 0x06007FC1 RID: 32705 RVA: 0x0025E1B4 File Offset: 0x0025C3B4
		private bool TryFindAnyFiancesCouple(out Pair<Pawn, Pawn> fiances)
		{
			foreach (Pawn pawn in base.Map.mapPawns.FreeColonistsSpawned)
			{
				Pawn firstDirectRelationPawn = pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance, (Pawn x) => x.Spawned);
				if (firstDirectRelationPawn != null)
				{
					fiances = new Pair<Pawn, Pawn>(pawn, firstDirectRelationPawn);
					return true;
				}
			}
			fiances = default(Pair<Pawn, Pawn>);
			return false;
		}
	}
}
