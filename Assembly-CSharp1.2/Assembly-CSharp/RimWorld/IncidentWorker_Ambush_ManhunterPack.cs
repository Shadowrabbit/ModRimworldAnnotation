using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001193 RID: 4499
	public class IncidentWorker_Ambush_ManhunterPack : IncidentWorker_Ambush
	{
		// Token: 0x0600632A RID: 25386 RVA: 0x001EE130 File Offset: 0x001EC330
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			PawnKindDef pawnKindDef;
			return base.CanFireNowSub(parms) && ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(this.AdjustedPoints(parms.points), -1, out pawnKindDef);
		}

		// Token: 0x0600632B RID: 25387 RVA: 0x001EE15C File Offset: 0x001EC35C
		protected override List<Pawn> GeneratePawns(IncidentParms parms)
		{
			PawnKindDef animalKind;
			if (!ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(this.AdjustedPoints(parms.points), parms.target.Tile, out animalKind) && !ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(this.AdjustedPoints(parms.points), -1, out animalKind))
			{
				Log.Error("Could not find any valid animal kind for " + this.def + " incident.", false);
				return new List<Pawn>();
			}
			return ManhunterPackIncidentUtility.GenerateAnimals_NewTmp(animalKind, parms.target.Tile, this.AdjustedPoints(parms.points), 0);
		}

		// Token: 0x0600632C RID: 25388 RVA: 0x001EE1E0 File Offset: 0x001EC3E0
		protected override void PostProcessGeneratedPawnsAfterSpawning(List<Pawn> generatedPawns)
		{
			for (int i = 0; i < generatedPawns.Count; i++)
			{
				generatedPawns[i].health.AddHediff(HediffDefOf.Scaria, null, null, null);
				generatedPawns[i].mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null, false, false, null, false);
			}
		}

		// Token: 0x0600632D RID: 25389 RVA: 0x00044348 File Offset: 0x00042548
		private float AdjustedPoints(float basePoints)
		{
			return basePoints * 0.75f;
		}

		// Token: 0x0600632E RID: 25390 RVA: 0x001EE244 File Offset: 0x001EC444
		protected override string GetLetterText(Pawn anyPawn, IncidentParms parms)
		{
			Caravan caravan = parms.target as Caravan;
			return string.Format(this.def.letterText, (caravan != null) ? caravan.Name : "yourCaravan".TranslateSimple(), anyPawn.GetKindLabelPlural(-1)).CapitalizeFirst();
		}

		// Token: 0x0400425E RID: 16990
		private const float ManhunterAmbushPointsFactor = 0.75f;
	}
}
