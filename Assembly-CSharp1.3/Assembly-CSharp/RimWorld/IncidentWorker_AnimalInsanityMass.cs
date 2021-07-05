using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000BFD RID: 3069
	public class IncidentWorker_AnimalInsanityMass : IncidentWorker
	{
		// Token: 0x0600483E RID: 18494 RVA: 0x0017DD08 File Offset: 0x0017BF08
		public static bool AnimalUsable(Pawn p)
		{
			return p.Spawned && !p.Position.Fogged(p.Map) && (!p.InMentalState || !p.MentalStateDef.IsAggro) && !p.Downed && p.Faction == null;
		}

		// Token: 0x0600483F RID: 18495 RVA: 0x0017DD58 File Offset: 0x0017BF58
		public static void DriveInsane(Pawn p)
		{
			p.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, true, false, null, false, false, false);
		}

		// Token: 0x06004840 RID: 18496 RVA: 0x0017DD84 File Offset: 0x0017BF84
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (parms.points <= 0f)
			{
				Log.Error("AnimalInsanity running without points.");
				parms.points = (float)((int)(map.strengthWatcher.StrengthRating * 50f));
			}
			float adjustedPoints = parms.points;
			if (adjustedPoints > 250f)
			{
				adjustedPoints -= 250f;
				adjustedPoints *= 0.5f;
				adjustedPoints += 250f;
			}
			PawnKindDef animalDef;
			if (!(from def in DefDatabase<PawnKindDef>.AllDefs
			where def.RaceProps.Animal && def.combatPower <= adjustedPoints && (from p in map.mapPawns.AllPawnsSpawned
			where p.kindDef == def && IncidentWorker_AnimalInsanityMass.AnimalUsable(p)
			select p).Count<Pawn>() >= 3
			select def).TryRandomElement(out animalDef))
			{
				return false;
			}
			List<Pawn> list = (from p in map.mapPawns.AllPawnsSpawned
			where p.kindDef == animalDef && IncidentWorker_AnimalInsanityMass.AnimalUsable(p)
			select p).ToList<Pawn>();
			float combatPower = animalDef.combatPower;
			float num = 0f;
			int num2 = 0;
			Pawn pawn = null;
			list.Shuffle<Pawn>();
			foreach (Pawn pawn2 in list)
			{
				if (num + combatPower > adjustedPoints)
				{
					break;
				}
				IncidentWorker_AnimalInsanityMass.DriveInsane(pawn2);
				num += combatPower;
				num2++;
				pawn = pawn2;
			}
			if (num == 0f)
			{
				return false;
			}
			string str;
			string str2;
			LetterDef baseLetterDef;
			if (num2 == 1)
			{
				str = "LetterLabelAnimalInsanitySingle".Translate(pawn.LabelShort, pawn.Named("ANIMAL")).CapitalizeFirst();
				str2 = "AnimalInsanitySingle".Translate(pawn.LabelShort, pawn.Named("ANIMAL")).CapitalizeFirst();
				baseLetterDef = LetterDefOf.ThreatSmall;
			}
			else
			{
				str = "LetterLabelAnimalInsanityMultiple".Translate(animalDef.GetLabelPlural(-1)).CapitalizeFirst();
				str2 = "AnimalInsanityMultiple".Translate(animalDef.GetLabelPlural(-1)).CapitalizeFirst();
				baseLetterDef = LetterDefOf.ThreatBig;
			}
			base.SendStandardLetter(str, str2, baseLetterDef, parms, pawn, Array.Empty<NamedArgument>());
			SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(map);
			if (map == Find.CurrentMap)
			{
				Find.CameraDriver.shaker.DoShake(1f);
			}
			return true;
		}
	}
}
