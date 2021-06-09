using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011E3 RID: 4579
	public class IncidentWorker_WandererJoin : IncidentWorker
	{
		// Token: 0x0600644B RID: 25675 RVA: 0x001F2650 File Offset: 0x001F0850
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			return this.CanSpawnJoiner(map);
		}

		// Token: 0x0600644C RID: 25676 RVA: 0x001F267C File Offset: 0x001F087C
		public virtual Pawn GeneratePawn()
		{
			Gender? fixedGender = null;
			if (this.def.pawnFixedGender != Gender.None)
			{
				fixedGender = new Gender?(this.def.pawnFixedGender);
			}
			return PawnGenerator.GeneratePawn(new PawnGenerationRequest(this.def.pawnKind, Faction.OfPlayer, PawnGenerationContext.NonPlayer, -1, true, false, false, false, true, this.def.pawnMustBeCapableOfViolence, 20f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, null, null, fixedGender, null, null, null, null));
		}

		// Token: 0x0600644D RID: 25677 RVA: 0x001F2720 File Offset: 0x001F0920
		public virtual bool CanSpawnJoiner(Map map)
		{
			IntVec3 intVec;
			return this.TryFindEntryCell(map, out intVec);
		}

		// Token: 0x0600644E RID: 25678 RVA: 0x001F2738 File Offset: 0x001F0938
		public virtual void SpawnJoiner(Map map, Pawn pawn)
		{
			IntVec3 loc;
			this.TryFindEntryCell(map, out loc);
			GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
		}

		// Token: 0x0600644F RID: 25679 RVA: 0x001F275C File Offset: 0x001F095C
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

		// Token: 0x06006450 RID: 25680 RVA: 0x001F288C File Offset: 0x001F0A8C
		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c) && !c.Fogged(map), map, CellFinder.EdgeRoadChance_Neutral, out cell);
		}

		// Token: 0x040042EF RID: 17135
		private const float RelationWithColonistWeight = 20f;
	}
}
