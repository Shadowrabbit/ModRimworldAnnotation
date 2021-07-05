using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BFC RID: 3068
	public class IncidentWorker_AnimalInsanitySingle : IncidentWorker
	{
		// Token: 0x0600483A RID: 18490 RVA: 0x0017DBE8 File Offset: 0x0017BDE8
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			Pawn pawn;
			return this.TryFindRandomAnimal(map, out pawn);
		}

		// Token: 0x0600483B RID: 18491 RVA: 0x0017DC18 File Offset: 0x0017BE18
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Pawn pawn;
			if (!this.TryFindRandomAnimal(map, out pawn))
			{
				return false;
			}
			IncidentWorker_AnimalInsanityMass.DriveInsane(pawn);
			string str = "AnimalInsanitySingle".Translate(pawn.Label, pawn.Named("ANIMAL")).CapitalizeFirst();
			base.SendStandardLetter("LetterLabelAnimalInsanitySingle".Translate(pawn.Label, pawn.Named("ANIMAL")).CapitalizeFirst(), str, LetterDefOf.ThreatSmall, parms, pawn, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x0600483C RID: 18492 RVA: 0x0017DCB8 File Offset: 0x0017BEB8
		private bool TryFindRandomAnimal(Map map, out Pawn animal)
		{
			int maxPoints = 150;
			if (GenDate.DaysPassed < 7)
			{
				maxPoints = 40;
			}
			return (from p in map.mapPawns.AllPawnsSpawned
			where p.RaceProps.Animal && p.kindDef.combatPower <= (float)maxPoints && IncidentWorker_AnimalInsanityMass.AnimalUsable(p)
			select p).TryRandomElement(out animal);
		}

		// Token: 0x04002C50 RID: 11344
		private const int FixedPoints = 30;
	}
}
