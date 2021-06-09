using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020011A4 RID: 4516
	public class IncidentWorker_AnimalInsanityMass : IncidentWorker
	{
		// Token: 0x06006378 RID: 25464 RVA: 0x001EF704 File Offset: 0x001ED904
		public static bool AnimalUsable(Pawn p)
		{
			return p.Spawned && !p.Position.Fogged(p.Map) && (!p.InMentalState || !p.MentalStateDef.IsAggro) && !p.Downed && p.Faction == null;
		}

		// Token: 0x06006379 RID: 25465 RVA: 0x000445FC File Offset: 0x000427FC
		public static void DriveInsane(Pawn p)
		{
			p.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, true, false, null, false);
		}

		// Token: 0x0600637A RID: 25466 RVA: 0x001EF754 File Offset: 0x001ED954
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (parms.points <= 0f)
			{
				Log.Error("AnimalInsanity running without points.", false);
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
				str = "LetterLabelAnimalInsanitySingle".Translate(pawn.LabelShort, pawn.Named("ANIMAL"));
				str2 = "AnimalInsanitySingle".Translate(pawn.LabelShort, pawn.Named("ANIMAL"));
				baseLetterDef = LetterDefOf.ThreatSmall;
			}
			else
			{
				str = "LetterLabelAnimalInsanityMultiple".Translate(animalDef.GetLabelPlural(-1));
				str2 = "AnimalInsanityMultiple".Translate(animalDef.GetLabelPlural(-1));
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
