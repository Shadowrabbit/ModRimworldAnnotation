using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
    // Token: 0x020011FC RID: 4604
    public abstract class IncidentWorker_NeutralGroup : IncidentWorker_PawnsArrive
    {
        // Token: 0x17000F92 RID: 3986
        // (get) Token: 0x060064C2 RID: 25794 RVA: 0x000450CB File Offset: 0x000432CB
        protected virtual PawnGroupKindDef PawnGroupKindDef
        {
            get
            {
                return PawnGroupKindDefOf.Peaceful;
            }
        }

        // Token: 0x060064C3 RID: 25795 RVA: 0x001F3EE0 File Offset: 0x001F20E0
        protected override bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false)
        {
            return base.FactionCanBeGroupSource(f, map, desperate) && !f.Hidden && !f.HostileTo(Faction.OfPlayer) &&
                   f.def.pawnGroupMakers != null && f.def.pawnGroupMakers.Any((PawnGroupMaker x) => x.kindDef == this.PawnGroupKindDef) &&
                   !NeutralGroupIncidentUtility.AnyBlockingHostileLord(map, f);
        }

        // Token: 0x060064C4 RID: 25796 RVA: 0x000450D2 File Offset: 0x000432D2
        protected bool TryResolveParms(IncidentParms parms)
        {
            if (!this.TryResolveParmsGeneral(parms))
            {
                return false;
            }
            this.ResolveParmsPoints(parms);
            return true;
        }

        // Token: 0x060064C5 RID: 25797 RVA: 0x001F3F44 File Offset: 0x001F2144
        protected virtual bool TryResolveParmsGeneral(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            return (parms.spawnCenter.IsValid ||
                    RCellFinder.TryFindRandomPawnEntryCell(out parms.spawnCenter, map, CellFinder.EdgeRoadChance_Neutral, false, null)) &&
                   (parms.faction != null || base.CandidateFactions(map, false).TryRandomElement(out parms.faction) ||
                    base.CandidateFactions(map, true).TryRandomElement(out parms.faction));
        }

        // Token: 0x060064C6 RID: 25798
        protected abstract void ResolveParmsPoints(IncidentParms parms);

        // Token: 0x060064C7 RID: 25799 RVA: 0x001F3FB8 File Offset: 0x001F21B8
        protected List<Pawn> SpawnPawns(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            List<Pawn> list = PawnGroupMakerUtility
                .GeneratePawns(IncidentParmsUtility.GetDefaultPawnGroupMakerParms(this.PawnGroupKindDef, parms, true), false)
                .ToList<Pawn>();
            foreach (Thing newThing in list)
            {
                IntVec3 loc = CellFinder.RandomClosewalkCellNear(parms.spawnCenter, map, 5, null);
                GenSpawn.Spawn(newThing, loc, map, WipeMode.Vanish);
            }
            return list;
        }
    }
}
