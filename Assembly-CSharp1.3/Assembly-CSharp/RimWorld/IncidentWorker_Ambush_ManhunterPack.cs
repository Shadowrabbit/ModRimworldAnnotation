using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BF5 RID: 3061
	public class IncidentWorker_Ambush_ManhunterPack : IncidentWorker_Ambush
	{
		// Token: 0x06004811 RID: 18449 RVA: 0x0017C7D8 File Offset: 0x0017A9D8
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			PawnKindDef pawnKindDef;
			return base.CanFireNowSub(parms) && ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(this.AdjustedPoints(parms.points), -1, out pawnKindDef);
		}

		// Token: 0x06004812 RID: 18450 RVA: 0x0017C804 File Offset: 0x0017AA04
		protected override List<Pawn> GeneratePawns(IncidentParms parms)
		{
			PawnKindDef animalKind;
			if (!ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(this.AdjustedPoints(parms.points), parms.target.Tile, out animalKind) && !ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(this.AdjustedPoints(parms.points), -1, out animalKind))
			{
				Log.Error("Could not find any valid animal kind for " + this.def + " incident.");
				return new List<Pawn>();
			}
			return ManhunterPackIncidentUtility.GenerateAnimals(animalKind, parms.target.Tile, this.AdjustedPoints(parms.points), 0);
		}

		// Token: 0x06004813 RID: 18451 RVA: 0x0017C888 File Offset: 0x0017AA88
		protected override void PostProcessGeneratedPawnsAfterSpawning(List<Pawn> generatedPawns)
		{
			for (int i = 0; i < generatedPawns.Count; i++)
			{
				generatedPawns[i].health.AddHediff(HediffDefOf.Scaria, null, null, null);
				generatedPawns[i].mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null, false, false, null, false, false, false);
			}
		}

		// Token: 0x06004814 RID: 18452 RVA: 0x0017C8EB File Offset: 0x0017AAEB
		private float AdjustedPoints(float basePoints)
		{
			return basePoints * 0.75f;
		}

		// Token: 0x06004815 RID: 18453 RVA: 0x0017C8F4 File Offset: 0x0017AAF4
		protected override string GetLetterText(Pawn anyPawn, IncidentParms parms)
		{
			Caravan caravan = parms.target as Caravan;
			return string.Format(this.def.letterText, (caravan != null) ? caravan.Name : "yourCaravan".TranslateSimple(), anyPawn.GetKindLabelPlural(-1)).CapitalizeFirst();
		}

		// Token: 0x04002C3E RID: 11326
		private const float ManhunterAmbushPointsFactor = 0.75f;
	}
}
