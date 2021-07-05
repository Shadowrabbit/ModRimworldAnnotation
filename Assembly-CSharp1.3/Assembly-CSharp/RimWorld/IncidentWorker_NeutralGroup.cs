using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C24 RID: 3108
	public abstract class IncidentWorker_NeutralGroup : IncidentWorker_PawnsArrive
	{
		// Token: 0x17000CA5 RID: 3237
		// (get) Token: 0x06004902 RID: 18690 RVA: 0x001828B8 File Offset: 0x00180AB8
		protected virtual PawnGroupKindDef PawnGroupKindDef
		{
			get
			{
				return PawnGroupKindDefOf.Peaceful;
			}
		}

		// Token: 0x06004903 RID: 18691 RVA: 0x001828C0 File Offset: 0x00180AC0
		protected override bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false)
		{
			return base.FactionCanBeGroupSource(f, map, desperate) && !f.Hidden && !f.HostileTo(Faction.OfPlayer) && f.def.pawnGroupMakers != null && f.def.pawnGroupMakers.Any((PawnGroupMaker x) => x.kindDef == this.PawnGroupKindDef) && !NeutralGroupIncidentUtility.AnyBlockingHostileLord(map, f);
		}

		// Token: 0x06004904 RID: 18692 RVA: 0x00182924 File Offset: 0x00180B24
		protected bool TryResolveParms(IncidentParms parms)
		{
			if (!this.TryResolveParmsGeneral(parms))
			{
				return false;
			}
			this.ResolveParmsPoints(parms);
			return true;
		}

		// Token: 0x06004905 RID: 18693 RVA: 0x0018293C File Offset: 0x00180B3C
		protected virtual bool TryResolveParmsGeneral(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return (parms.spawnCenter.IsValid || RCellFinder.TryFindRandomPawnEntryCell(out parms.spawnCenter, map, CellFinder.EdgeRoadChance_Neutral, false, null)) && (parms.faction != null || base.CandidateFactions(map, false).TryRandomElement(out parms.faction) || base.CandidateFactions(map, true).TryRandomElement(out parms.faction));
		}

		// Token: 0x06004906 RID: 18694
		protected abstract void ResolveParmsPoints(IncidentParms parms);

		// Token: 0x06004907 RID: 18695 RVA: 0x001829B0 File Offset: 0x00180BB0
		protected List<Pawn> SpawnPawns(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(IncidentParmsUtility.GetDefaultPawnGroupMakerParms(this.PawnGroupKindDef, parms, true), false).ToList<Pawn>();
			foreach (Pawn pawn in list)
			{
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(parms.spawnCenter, map, 5, null);
				GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
				List<Pawn> storeGeneratedNeutralPawns = parms.storeGeneratedNeutralPawns;
				if (storeGeneratedNeutralPawns != null)
				{
					storeGeneratedNeutralPawns.Add(pawn);
				}
			}
			return list;
		}
	}
}
