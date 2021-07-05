using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008D7 RID: 2263
	public abstract class RaidStrategyWorker_WithRequiredPawnKinds : RaidStrategyWorker
	{
		// Token: 0x06003B71 RID: 15217
		protected abstract bool MatchesRequiredPawnKind(PawnKindDef kind);

		// Token: 0x06003B72 RID: 15218
		protected abstract int MinRequiredPawnsForPoints(float pointsTotal);

		// Token: 0x06003B73 RID: 15219 RVA: 0x0014BF34 File Offset: 0x0014A134
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return this.PawnGenOptionsWithRequiredPawns(parms.faction, groupKind).Any<PawnGroupMaker>() && base.CanUseWith(parms, groupKind);
		}

		// Token: 0x06003B74 RID: 15220 RVA: 0x0014BF59 File Offset: 0x0014A159
		public override float MinimumPoints(Faction faction, PawnGroupKindDef groupKind)
		{
			return Mathf.Max(base.MinimumPoints(faction, groupKind), this.CheapestRequiredPawnCost(faction, groupKind));
		}

		// Token: 0x06003B75 RID: 15221 RVA: 0x0014BF70 File Offset: 0x0014A170
		public override float MinMaxAllowedPawnGenOptionCost(Faction faction, PawnGroupKindDef groupKind)
		{
			return this.CheapestRequiredPawnCost(faction, groupKind);
		}

		// Token: 0x06003B76 RID: 15222 RVA: 0x0014BF7C File Offset: 0x0014A17C
		private float CheapestRequiredPawnCost(Faction faction, PawnGroupKindDef groupKind)
		{
			IEnumerable<PawnGroupMaker> enumerable = this.PawnGenOptionsWithRequiredPawns(faction, groupKind);
			if (!enumerable.Any<PawnGroupMaker>())
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to get MinimumPoints for ",
					base.GetType().ToString(),
					" for faction ",
					faction.ToString(),
					" but the faction has no groups with the required pawn kind. groupKind=",
					groupKind
				}));
				return 99999f;
			}
			float num = 9999999f;
			foreach (PawnGroupMaker pawnGroupMaker in enumerable)
			{
				foreach (PawnGenOption pawnGenOption in from op in pawnGroupMaker.options
				where this.MatchesRequiredPawnKind(op.kind)
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

		// Token: 0x06003B77 RID: 15223 RVA: 0x0014C078 File Offset: 0x0014A278
		public override bool CanUsePawnGenOption(float pointsTotal, PawnGenOption opt, List<PawnGenOption> chosenOpts)
		{
			return chosenOpts.Count >= this.MinRequiredPawnsForPoints(pointsTotal) || this.MatchesRequiredPawnKind(opt.kind);
		}

		// Token: 0x06003B78 RID: 15224 RVA: 0x0014C09C File Offset: 0x0014A29C
		private IEnumerable<PawnGroupMaker> PawnGenOptionsWithRequiredPawns(Faction faction, PawnGroupKindDef groupKind)
		{
			if (faction.def.pawnGroupMakers == null)
			{
				return Enumerable.Empty<PawnGroupMaker>();
			}
			Predicate<PawnGenOption> <>9__1;
			return faction.def.pawnGroupMakers.Where(delegate(PawnGroupMaker gm)
			{
				if (gm.kindDef == groupKind && gm.options != null)
				{
					List<PawnGenOption> options = gm.options;
					Predicate<PawnGenOption> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = ((PawnGenOption op) => this.MatchesRequiredPawnKind(op.kind)));
					}
					return options.Any(predicate);
				}
				return false;
			});
		}
	}
}
