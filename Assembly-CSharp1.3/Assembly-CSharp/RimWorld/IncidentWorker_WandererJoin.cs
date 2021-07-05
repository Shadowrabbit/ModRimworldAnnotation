using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C1C RID: 3100
	public class IncidentWorker_WandererJoin : IncidentWorker
	{
		// Token: 0x060048C3 RID: 18627 RVA: 0x00180E88 File Offset: 0x0017F088
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			return this.CanSpawnJoiner(map);
		}

		// Token: 0x060048C4 RID: 18628 RVA: 0x00180EB4 File Offset: 0x0017F0B4
		public virtual Pawn GeneratePawn()
		{
			Gender? fixedGender = null;
			if (this.def.pawnFixedGender != Gender.None)
			{
				fixedGender = new Gender?(this.def.pawnFixedGender);
			}
			return PawnGenerator.GeneratePawn(new PawnGenerationRequest(this.def.pawnKind, Faction.OfPlayer, PawnGenerationContext.NonPlayer, -1, true, false, false, false, true, this.def.pawnMustBeCapableOfViolence, 20f, false, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, fixedGender, null, null, null, null, null, false, false));
		}

		// Token: 0x060048C5 RID: 18629 RVA: 0x00180F60 File Offset: 0x0017F160
		public virtual bool CanSpawnJoiner(Map map)
		{
			IntVec3 intVec;
			return this.TryFindEntryCell(map, out intVec);
		}

		// Token: 0x060048C6 RID: 18630 RVA: 0x00180F78 File Offset: 0x0017F178
		public virtual void SpawnJoiner(Map map, Pawn pawn)
		{
			IntVec3 loc;
			this.TryFindEntryCell(map, out loc);
			GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
		}

		// Token: 0x060048C7 RID: 18631 RVA: 0x00180F9C File Offset: 0x0017F19C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!this.CanSpawnJoiner(map))
			{
				return false;
			}
			Pawn pawn = this.GeneratePawn();
			this.SpawnJoiner(map, pawn);
			if (this.def.pawnHediff != null)
			{
				pawn.health.AddHediff(this.def.pawnHediff, null, null, null);
			}
			TaggedString baseLetterText = (this.def.pawnHediff != null) ? this.def.letterText.Formatted(pawn.Named("PAWN"), this.def.pawnHediff.Named("HEDIFF")).AdjustedFor(pawn, "PAWN", true) : this.def.letterText.Formatted(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			TaggedString baseLetterLabel = this.def.letterLabel.Formatted(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref baseLetterText, ref baseLetterLabel, pawn);
			base.SendStandardLetter(baseLetterLabel, baseLetterText, LetterDefOf.PositiveEvent, parms, pawn, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x060048C8 RID: 18632 RVA: 0x001810CC File Offset: 0x0017F2CC
		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c) && !c.Fogged(map), map, CellFinder.EdgeRoadChance_Neutral, out cell);
		}

		// Token: 0x04002C71 RID: 11377
		private const float RelationWithColonistWeight = 20f;
	}
}
