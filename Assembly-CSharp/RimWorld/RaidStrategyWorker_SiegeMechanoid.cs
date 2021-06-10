using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E27 RID: 3623
	public class RaidStrategyWorker_SiegeMechanoid : RaidStrategyWorker_Siege
	{
		// Token: 0x06005225 RID: 21029 RVA: 0x00039734 File Offset: 0x00037934
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return parms.points >= this.MinimumPoints(parms.faction, groupKind) && parms.faction == Faction.OfMechanoids && ModsConfig.RoyaltyActive;
		}

		// Token: 0x06005226 RID: 21030 RVA: 0x00039761 File Offset: 0x00037961
		public override void TryGenerateThreats(IncidentParms parms)
		{
			//生成机械集群
			parms.mechClusterSketch = MechClusterGenerator.GenerateClusterSketch_NewTemp(parms.points, parms.target as Map, true, false);
		}

		// Token: 0x06005227 RID: 21031 RVA: 0x00039781 File Offset: 0x00037981
		public override List<Pawn> SpawnThreats(IncidentParms parms)
		{
			return MechClusterUtility.SpawnCluster(parms.spawnCenter, (Map)parms.target, parms.mechClusterSketch, true, true, parms.questTag).OfType<Pawn>().ToList<Pawn>();
		}

		// Token: 0x06005228 RID: 21032 RVA: 0x0000C32E File Offset: 0x0000A52E
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			return null;
		}

		// Token: 0x06005229 RID: 21033 RVA: 0x00006A05 File Offset: 0x00004C05
		public override void MakeLords(IncidentParms parms, List<Pawn> pawns)
		{
		}
	}
}
