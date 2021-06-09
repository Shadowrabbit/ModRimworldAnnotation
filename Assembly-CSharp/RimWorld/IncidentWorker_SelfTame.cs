using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020011DC RID: 4572
	public class IncidentWorker_SelfTame : IncidentWorker
	{
		// Token: 0x06006431 RID: 25649 RVA: 0x00044BC3 File Offset: 0x00042DC3
		private IEnumerable<Pawn> Candidates(Map map)
		{
			return from x in map.mapPawns.AllPawnsSpawned
			where x.RaceProps.Animal && x.Faction == null && !x.Position.Fogged(x.Map) && !x.InMentalState && !x.Downed && x.RaceProps.wildness > 0f
			select x;
		}

		// Token: 0x06006432 RID: 25650 RVA: 0x001F1F58 File Offset: 0x001F0158
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return this.Candidates(map).Any<Pawn>();
		}

		// Token: 0x06006433 RID: 25651 RVA: 0x001F1F80 File Offset: 0x001F0180
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Pawn pawn = null;
			if (!this.Candidates(map).TryRandomElement(out pawn))
			{
				return false;
			}
			if (pawn.guest != null)
			{
				pawn.guest.SetGuestStatus(null, false);
			}
			string value = pawn.LabelIndefinite();
			bool flag = pawn.Name != null;
			pawn.SetFaction(Faction.OfPlayer, null);
			string str;
			if (!flag && pawn.Name != null)
			{
				if (pawn.Name.Numerical)
				{
					str = "LetterAnimalSelfTameAndNameNumerical".Translate(value, pawn.Name.ToStringFull, pawn.Named("ANIMAL")).CapitalizeFirst();
				}
				else
				{
					str = "LetterAnimalSelfTameAndName".Translate(value, pawn.Name.ToStringFull, pawn.Named("ANIMAL")).CapitalizeFirst();
				}
			}
			else
			{
				str = "LetterAnimalSelfTame".Translate(pawn).CapitalizeFirst();
			}
			base.SendStandardLetter("LetterLabelAnimalSelfTame".Translate(pawn.KindLabel, pawn).CapitalizeFirst(), str, LetterDefOf.PositiveEvent, parms, pawn, Array.Empty<NamedArgument>());
			return true;
		}
	}
}
