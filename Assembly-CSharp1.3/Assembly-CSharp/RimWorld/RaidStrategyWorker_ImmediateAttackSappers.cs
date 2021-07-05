using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008D6 RID: 2262
	public class RaidStrategyWorker_ImmediateAttackSappers : RaidStrategyWorker_WithRequiredPawnKinds
	{
		// Token: 0x06003B6C RID: 15212 RVA: 0x0014BECF File Offset: 0x0014A0CF
		protected override bool MatchesRequiredPawnKind(PawnKindDef kind)
		{
			return kind.canBeSapper;
		}

		// Token: 0x06003B6D RID: 15213 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override int MinRequiredPawnsForPoints(float pointsTotal)
		{
			return 1;
		}

		// Token: 0x06003B6E RID: 15214 RVA: 0x0014BED7 File Offset: 0x0014A0D7
		public override bool CanUsePawn(float pointsTotal, Pawn p, List<Pawn> otherPawns)
		{
			return (otherPawns.Count != 0 || SappersUtility.IsGoodSapper(p) || SappersUtility.IsGoodBackupSapper(p)) && (!p.kindDef.canBeSapper || !SappersUtility.HasBuildingDestroyerWeapon(p) || SappersUtility.IsGoodSapper(p));
		}

		// Token: 0x06003B6F RID: 15215 RVA: 0x0014BF13 File Offset: 0x0014A113
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			return new LordJob_AssaultColony(parms.faction, true, parms.canTimeoutOrFlee, true, true, true, false, false);
		}
	}
}
