using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DC2 RID: 3522
	public class PawnGroupKindWorker_Normal : PawnGroupKindWorker
	{
		// Token: 0x06005182 RID: 20866 RVA: 0x001B75BC File Offset: 0x001B57BC
		public override float MinPointsToGenerateAnything(PawnGroupMaker groupMaker, PawnGroupMakerParms parms = null)
		{
			IEnumerable<PawnGenOption> source;
			if (parms == null)
			{
				source = from x in groupMaker.options
				where x.kind.isFighter
				select x;
				if (!source.Any<PawnGenOption>())
				{
					source = groupMaker.options;
				}
			}
			else
			{
				source = from x in groupMaker.options
				where PawnGroupMakerUtility.PawnGenOptionValid(x, parms)
				select x;
			}
			return source.Min((PawnGenOption g) => g.Cost);
		}

		// Token: 0x06005183 RID: 20867 RVA: 0x001B7657 File Offset: 0x001B5857
		public override bool CanGenerateFrom(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
		{
			return base.CanGenerateFrom(parms, groupMaker) && PawnGroupMakerUtility.ChoosePawnGenOptionsByPoints(parms.points, groupMaker.options, parms).Any<PawnGenOption>();
		}

		// Token: 0x06005184 RID: 20868 RVA: 0x001B7684 File Offset: 0x001B5884
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
					}));
				}
				return;
			}
			bool allowFood = parms.raidStrategy == null || parms.raidStrategy.pawnsCanBringFood || (parms.faction != null && !parms.faction.HostileTo(Faction.OfPlayer));
			Predicate<Pawn> validatorPostGear = (parms.raidStrategy != null) ? ((Pawn p) => parms.raidStrategy.Worker.CanUsePawn(parms.points, p, outPawns)) : null;
			bool flag = false;
			foreach (PawnGenOption pawnGenOption in PawnGroupMakerUtility.ChoosePawnGenOptionsByPoints(parms.points, groupMaker.options, parms))
			{
				PawnKindDef kind = pawnGenOption.kind;
				Faction faction = parms.faction;
				PawnGenerationContext context = PawnGenerationContext.NonPlayer;
				Ideo ideo = parms.ideo;
				Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, faction, context, parms.tile, false, false, false, false, true, true, 1f, false, true, allowFood, true, parms.inhabitants, false, false, false, 0f, 0f, null, 1f, null, validatorPostGear, null, null, null, null, null, null, null, null, null, null, ideo, false, false));
				if (parms.forceOneIncap && !flag)
				{
					pawn.health.forceIncap = true;
					pawn.mindState.canFleeIndividual = false;
					flag = true;
				}
				outPawns.Add(pawn);
			}
		}

		// Token: 0x06005185 RID: 20869 RVA: 0x001B78A0 File Offset: 0x001B5AA0
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
