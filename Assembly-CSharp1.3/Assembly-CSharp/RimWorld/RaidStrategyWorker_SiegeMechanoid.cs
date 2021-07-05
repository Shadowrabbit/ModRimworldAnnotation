using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008DF RID: 2271
	public class RaidStrategyWorker_SiegeMechanoid : RaidStrategyWorker_Siege
	{
		// Token: 0x06003B90 RID: 15248 RVA: 0x0014C3E6 File Offset: 0x0014A5E6
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return parms.points >= this.MinimumPoints(parms.faction, groupKind) && Faction.OfMechanoids != null && parms.faction == Faction.OfMechanoids && ModsConfig.RoyaltyActive;
		}

		// Token: 0x06003B91 RID: 15249 RVA: 0x0014C41C File Offset: 0x0014A61C
		public override void TryGenerateThreats(IncidentParms parms)
		{
			parms.mechClusterSketch = MechClusterGenerator.GenerateClusterSketch(parms.points, parms.target as Map, true, false);
		}

		// Token: 0x06003B92 RID: 15250 RVA: 0x0014C43C File Offset: 0x0014A63C
		public override List<Pawn> SpawnThreats(IncidentParms parms)
		{
			return MechClusterUtility.SpawnCluster(parms.spawnCenter, (Map)parms.target, parms.mechClusterSketch, true, true, parms.questTag).OfType<Pawn>().ToList<Pawn>();
		}

		// Token: 0x06003B93 RID: 15251 RVA: 0x00002688 File Offset: 0x00000888
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			return null;
		}

		// Token: 0x06003B94 RID: 15252 RVA: 0x0000313F File Offset: 0x0000133F
		public override void MakeLords(IncidentParms parms, List<Pawn> pawns)
		{
		}
	}
}
