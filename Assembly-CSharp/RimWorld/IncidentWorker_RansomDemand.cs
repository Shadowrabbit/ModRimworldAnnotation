using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020011D9 RID: 4569
	public class IncidentWorker_RansomDemand : IncidentWorker
	{
		// Token: 0x06006426 RID: 25638 RVA: 0x00044B78 File Offset: 0x00042D78
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return CommsConsoleUtility.PlayerHasPoweredCommsConsole((Map)parms.target) && this.RandomKidnappedColonist() != null && base.CanFireNowSub(parms);
		}

		// Token: 0x06006427 RID: 25639 RVA: 0x001F1C70 File Offset: 0x001EFE70
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Pawn pawn = this.RandomKidnappedColonist();
			if (pawn == null)
			{
				return false;
			}
			Faction faction = this.FactionWhichKidnapped(pawn);
			int num = this.RandomFee(pawn);
			ChoiceLetter_RansomDemand choiceLetter_RansomDemand = (ChoiceLetter_RansomDemand)LetterMaker.MakeLetter(this.def.letterLabel, "RansomDemand".Translate(pawn.LabelShort, faction.NameColored, num, pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true), this.def.letterDef, null, null);
			choiceLetter_RansomDemand.title = "RansomDemandTitle".Translate(map.Parent.Label);
			choiceLetter_RansomDemand.radioMode = true;
			choiceLetter_RansomDemand.kidnapped = pawn;
			choiceLetter_RansomDemand.faction = faction;
			choiceLetter_RansomDemand.map = map;
			choiceLetter_RansomDemand.fee = num;
			choiceLetter_RansomDemand.relatedFaction = faction;
			choiceLetter_RansomDemand.quest = parms.quest;
			choiceLetter_RansomDemand.StartTimeout(60000);
			Find.LetterStack.ReceiveLetter(choiceLetter_RansomDemand, null);
			return true;
		}

		// Token: 0x06006428 RID: 25640 RVA: 0x001F1D8C File Offset: 0x001EFF8C
		private Pawn RandomKidnappedColonist()
		{
			IncidentWorker_RansomDemand.candidates.Clear();
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				List<Pawn> kidnappedPawnsListForReading = allFactionsListForReading[i].kidnapped.KidnappedPawnsListForReading;
				for (int j = 0; j < kidnappedPawnsListForReading.Count; j++)
				{
					if (kidnappedPawnsListForReading[j].Faction == Faction.OfPlayer && kidnappedPawnsListForReading[j].RaceProps.Humanlike)
					{
						IncidentWorker_RansomDemand.candidates.Add(kidnappedPawnsListForReading[j]);
					}
				}
			}
			List<Letter> lettersListForReading = Find.LetterStack.LettersListForReading;
			for (int k = 0; k < lettersListForReading.Count; k++)
			{
				ChoiceLetter_RansomDemand choiceLetter_RansomDemand = lettersListForReading[k] as ChoiceLetter_RansomDemand;
				if (choiceLetter_RansomDemand != null)
				{
					IncidentWorker_RansomDemand.candidates.Remove(choiceLetter_RansomDemand.kidnapped);
				}
			}
			Pawn result;
			if (!IncidentWorker_RansomDemand.candidates.TryRandomElement(out result))
			{
				return null;
			}
			IncidentWorker_RansomDemand.candidates.Clear();
			return result;
		}

		// Token: 0x06006429 RID: 25641 RVA: 0x001F1E88 File Offset: 0x001F0088
		private Faction FactionWhichKidnapped(Pawn pawn)
		{
			return Find.FactionManager.AllFactionsListForReading.Find((Faction x) => x.kidnapped.KidnappedPawnsListForReading.Contains(pawn));
		}

		// Token: 0x0600642A RID: 25642 RVA: 0x001F1EC0 File Offset: 0x001F00C0
		private int RandomFee(Pawn pawn)
		{
			return (int)(pawn.MarketValue * DiplomacyTuning.RansomFeeMarketValueFactorRange.RandomInRange);
		}

		// Token: 0x040042E8 RID: 17128
		private const int TimeoutTicks = 60000;

		// Token: 0x040042E9 RID: 17129
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
