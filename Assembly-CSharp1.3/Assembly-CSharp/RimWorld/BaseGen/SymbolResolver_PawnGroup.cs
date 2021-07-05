using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015C4 RID: 5572
	public class SymbolResolver_PawnGroup : SymbolResolver
	{
		// Token: 0x0600833B RID: 33595 RVA: 0x002EBCE0 File Offset: 0x002E9EE0
		public static PawnGroupMakerParms GetGroupMakerParms(Map map, ResolveParams rp)
		{
			PawnGroupMakerParms pawnGroupMakerParms = rp.pawnGroupMakerParams;
			if (pawnGroupMakerParms == null)
			{
				pawnGroupMakerParms = new PawnGroupMakerParms();
				pawnGroupMakerParms.tile = map.Tile;
				pawnGroupMakerParms.faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
				pawnGroupMakerParms.points = 250f;
			}
			pawnGroupMakerParms.groupKind = (rp.pawnGroupKindDef ?? PawnGroupKindDefOf.Combat);
			return pawnGroupMakerParms;
		}

		// Token: 0x0600833C RID: 33596 RVA: 0x002EBD40 File Offset: 0x002E9F40
		public override bool CanResolve(ResolveParams rp)
		{
			if (!base.CanResolve(rp))
			{
				return false;
			}
			return (from x in rp.rect.Cells
			where x.Standable(BaseGen.globalSettings.map)
			select x).Any<IntVec3>();
		}

		// Token: 0x0600833D RID: 33597 RVA: 0x002EBD94 File Offset: 0x002E9F94
		public override void Resolve(ResolveParams rp)
		{
			PawnGroupMakerParms groupMakerParms = SymbolResolver_PawnGroup.GetGroupMakerParms(BaseGen.globalSettings.map, rp);
			List<PawnKindDef> list = new List<PawnKindDef>();
			foreach (Pawn pawn in PawnGroupMakerUtility.GeneratePawns(groupMakerParms, true))
			{
				list.Add(pawn.kindDef);
				ResolveParams resolveParams = rp;
				resolveParams.singlePawnToSpawn = pawn;
				BaseGen.symbolStack.Push("pawn", resolveParams, null);
			}
		}

		// Token: 0x04005200 RID: 20992
		private const float DefaultPoints = 250f;
	}
}
