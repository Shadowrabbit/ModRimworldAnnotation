using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C16 RID: 3094
	public class IncidentWorker_RansomDemand : IncidentWorker
	{
		// Token: 0x060048A7 RID: 18599 RVA: 0x001805EC File Offset: 0x0017E7EC
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return CommsConsoleUtility.PlayerHasPoweredCommsConsole((Map)parms.target) && this.RandomKidnappedColonist() != null && base.CanFireNowSub(parms);
		}

		// Token: 0x060048A8 RID: 18600 RVA: 0x00180614 File Offset: 0x0017E814
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

		// Token: 0x060048A9 RID: 18601 RVA: 0x00180730 File Offset: 0x0017E930
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

		// Token: 0x060048AA RID: 18602 RVA: 0x0018082C File Offset: 0x0017EA2C
		private Faction FactionWhichKidnapped(Pawn pawn)
		{
			return Find.FactionManager.AllFactionsListForReading.Find((Faction x) => x.kidnapped.KidnappedPawnsListForReading.Contains(pawn));
		}

		// Token: 0x060048AB RID: 18603 RVA: 0x00180864 File Offset: 0x0017EA64
		private int RandomFee(Pawn pawn)
		{
			return (int)(pawn.MarketValue * DiplomacyTuning.RansomFeeMarketValueFactorRange.RandomInRange);
		}

		// Token: 0x04002C6E RID: 11374
		private const int TimeoutTicks = 60000;

		// Token: 0x04002C6F RID: 11375
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
