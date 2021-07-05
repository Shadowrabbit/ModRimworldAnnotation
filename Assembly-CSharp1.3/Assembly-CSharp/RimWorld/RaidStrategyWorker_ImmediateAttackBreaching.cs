using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008DB RID: 2267
	public class RaidStrategyWorker_ImmediateAttackBreaching : RaidStrategyWorker_WithRequiredPawnKinds
	{
		// Token: 0x06003B82 RID: 15234 RVA: 0x0014C210 File Offset: 0x0014A410
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return ModLister.CheckIdeology("Breach raid") && base.CanUseWith(parms, groupKind);
		}

		// Token: 0x06003B83 RID: 15235 RVA: 0x0014C228 File Offset: 0x0014A428
		protected override bool MatchesRequiredPawnKind(PawnKindDef kind)
		{
			return kind.isGoodBreacher;
		}

		// Token: 0x06003B84 RID: 15236 RVA: 0x0014C230 File Offset: 0x0014A430
		protected override int MinRequiredPawnsForPoints(float pointsTotal)
		{
			return Mathf.RoundToInt(RaidStrategyWorker_ImmediateAttackBreaching.MinGoodBreachersFromPointCurve.Evaluate(pointsTotal));
		}

		// Token: 0x06003B85 RID: 15237 RVA: 0x0014C242 File Offset: 0x0014A442
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			return new LordJob_AssaultColony(parms.faction, true, parms.canTimeoutOrFlee, false, this.useAvoidGridSmart, true, true, false);
		}

		// Token: 0x06003B86 RID: 15238 RVA: 0x0014C260 File Offset: 0x0014A460
		public override bool CanUsePawn(float pointsTotal, Pawn p, List<Pawn> otherPawns)
		{
			return otherPawns.Count >= this.MinRequiredPawnsForPoints(pointsTotal) || BreachingUtility.IsGoodBreacher(p);
		}

		// Token: 0x04002063 RID: 8291
		private static SimpleCurve MinGoodBreachersFromPointCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(200f, 1f),
				true
			},
			{
				new CurvePoint(1000f, 3f),
				true
			},
			{
				new CurvePoint(4000f, 4f),
				true
			}
		};

		// Token: 0x04002064 RID: 8292
		protected bool useAvoidGridSmart;
	}
}
