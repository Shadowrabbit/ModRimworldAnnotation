using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001424 RID: 5156
	public class PawnGroupKindWorker_Normal : PawnGroupKindWorker
	{
		// Token: 0x06006F4B RID: 28491 RVA: 0x002219CC File Offset: 0x0021FBCC
		public override float MinPointsToGenerateAnything(PawnGroupMaker groupMaker)
		{
			return (from x in groupMaker.options
			where x.kind.isFighter
			select x).Min((PawnGenOption g) => g.Cost);
		}

		// Token: 0x06006F4C RID: 28492 RVA: 0x0004B434 File Offset: 0x00049634
		public override bool CanGenerateFrom(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
		{
			return base.CanGenerateFrom(parms, groupMaker) && PawnGroupMakerUtility.ChoosePawnGenOptionsByPoints(parms.points, groupMaker.options, parms).Any<PawnGenOption>();
		}

		// Token: 0x06006F4D RID: 28493 RVA: 0x00221A28 File Offset: 0x0021FC28
		protected override void GeneratePawns(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, List<Pawn> outPawns, bool errorOnZeroResults = true)
		{
			if (!this.CanGenerateFrom(parms, groupMaker))
			{
				if (errorOnZeroResults)
				{
					Log.Error(string.Concat(new object[]
					{
						"Cannot generate pawns for ",
						parms.faction,
						" with ",
						parms.points,
						". Defaulting to a single random cheap group."
					}), false);
				}
				return;
			}
			bool allowFood = parms.raidStrategy == null || parms.raidStrategy.pawnsCanBringFood || (parms.faction != null && !parms.faction.HostileTo(Faction.OfPlayer));
			Predicate<Pawn> validatorPostGear = (parms.raidStrategy != null) ? ((Pawn p) => parms.raidStrategy.Worker.CanUsePawn(p, outPawns)) : null;
			bool flag = false;
			foreach (PawnGenOption pawnGenOption in PawnGroupMakerUtility.ChoosePawnGenOptionsByPoints(parms.points, groupMaker.options, parms))
			{
				Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnGenOption.kind, parms.faction, PawnGenerationContext.NonPlayer, parms.tile, false, false, false, false, true, true, 1f, false, true, allowFood, true, parms.inhabitants, false, false, false, 0f, null, 1f, null, validatorPostGear, null, null, null, null, null, null, null, null, null, null));
				if (parms.forceOneIncap && !flag)
				{
					pawn.health.forceIncap = true;
					pawn.mindState.canFleeIndividual = false;
					flag = true;
				}
				outPawns.Add(pawn);
			}
		}

		// Token: 0x06006F4E RID: 28494 RVA: 0x0004B45E File Offset: 0x0004965E
		public override IEnumerable<PawnKindDef> GeneratePawnKindsExample(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
		{
			foreach (PawnGenOption pawnGenOption in PawnGroupMakerUtility.ChoosePawnGenOptionsByPoints(parms.points, groupMaker.options, parms))
			{
				yield return pawnGenOption.kind;
			}
			IEnumerator<PawnGenOption> enumerator = null;
			yield break;
			yield break;
		}
	}
}
