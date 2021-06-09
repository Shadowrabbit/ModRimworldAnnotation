using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E1F RID: 3615
	public class RaidStrategyWorker_ImmediateAttackSappers : RaidStrategyWorker
	{
		// Token: 0x06005209 RID: 21001 RVA: 0x000395C1 File Offset: 0x000377C1
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return this.PawnGenOptionsWithSappers(parms.faction, groupKind).Any<PawnGroupMaker>() && base.CanUseWith(parms, groupKind);
		}

		// Token: 0x0600520A RID: 21002 RVA: 0x000395E6 File Offset: 0x000377E6
		public override float MinimumPoints(Faction faction, PawnGroupKindDef groupKind)
		{
			return Mathf.Max(base.MinimumPoints(faction, groupKind), this.CheapestSapperCost(faction, groupKind));
		}

		// Token: 0x0600520B RID: 21003 RVA: 0x000395FD File Offset: 0x000377FD
		public override float MinMaxAllowedPawnGenOptionCost(Faction faction, PawnGroupKindDef groupKind)
		{
			return this.CheapestSapperCost(faction, groupKind);
		}

		// Token: 0x0600520C RID: 21004 RVA: 0x001BD574 File Offset: 0x001BB774
		private float CheapestSapperCost(Faction faction, PawnGroupKindDef groupKind)
		{
			IEnumerable<PawnGroupMaker> enumerable = this.PawnGenOptionsWithSappers(faction, groupKind);
			if (!enumerable.Any<PawnGroupMaker>())
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to get MinimumPoints for ",
					base.GetType().ToString(),
					" for faction ",
					faction.ToString(),
					" but the faction has no groups with sappers. groupKind=",
					groupKind
				}), false);
				return 99999f;
			}
			float num = 9999999f;
			foreach (PawnGroupMaker pawnGroupMaker in enumerable)
			{
				foreach (PawnGenOption pawnGenOption in from op in pawnGroupMaker.options
				where op.kind.canBeSapper
				select op)
				{
					if (pawnGenOption.Cost < num)
					{
						num = pawnGenOption.Cost;
					}
				}
			}
			return num;
		}

		// Token: 0x0600520D RID: 21005 RVA: 0x00039607 File Offset: 0x00037807
		public override bool CanUsePawnGenOption(PawnGenOption opt, List<PawnGenOption> chosenOpts)
		{
			return chosenOpts.Count != 0 || opt.kind.canBeSapper;
		}

		// Token: 0x0600520E RID: 21006 RVA: 0x00039621 File Offset: 0x00037821
		public override bool CanUsePawn(Pawn p, List<Pawn> otherPawns)
		{
			return (otherPawns.Count != 0 || SappersUtility.IsGoodSapper(p) || SappersUtility.IsGoodBackupSapper(p)) && (!p.kindDef.canBeSapper || !SappersUtility.HasBuildingDestroyerWeapon(p) || SappersUtility.IsGoodSapper(p));
		}

		// Token: 0x0600520F RID: 21007 RVA: 0x001BD684 File Offset: 0x001BB884
		private IEnumerable<PawnGroupMaker> PawnGenOptionsWithSappers(Faction faction, PawnGroupKindDef groupKind)
		{
			if (faction.def.pawnGroupMakers == null)
			{
				return Enumerable.Empty<PawnGroupMaker>();
			}
			return faction.def.pawnGroupMakers.Where(delegate(PawnGroupMaker gm)
			{
				if (gm.kindDef == groupKind && gm.options != null)
				{
					return gm.options.Any((PawnGenOption op) => op.kind.canBeSapper);
				}
				return false;
			});
		}

		// Token: 0x06005210 RID: 21008 RVA: 0x0003965D File Offset: 0x0003785D
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			return new LordJob_AssaultColony(parms.faction, true, true, true, true, true);
		}
	}
}
