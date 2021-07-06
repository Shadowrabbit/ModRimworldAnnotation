using System;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000F99 RID: 3993
	public class GatheringWorker_Speech : GatheringWorker
	{
		// Token: 0x0600578F RID: 22415 RVA: 0x0003CB72 File Offset: 0x0003AD72
		protected override LordJob CreateLordJob(IntVec3 spot, Pawn organizer)
		{
			return new LordJob_Joinable_Speech(spot, organizer, this.def);
		}

		// Token: 0x06005790 RID: 22416 RVA: 0x001CD9B0 File Offset: 0x001CBBB0
		public override bool CanExecute(Map map, Pawn organizer = null)
		{
			IntVec3 intVec;
			return organizer != null && this.TryFindGatherSpot(organizer, out intVec);
		}

		// Token: 0x06005791 RID: 22417 RVA: 0x001CD9D0 File Offset: 0x001CBBD0
		protected override bool TryFindGatherSpot(Pawn organizer, out IntVec3 spot)
		{
			Building_Throne building_Throne = RoyalTitleUtility.FindBestUsableThrone(organizer);
			if (building_Throne != null)
			{
				spot = building_Throne.InteractionCell;
				return true;
			}
			spot = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06005792 RID: 22418 RVA: 0x001CDA04 File Offset: 0x001CBC04
		protected override void SendLetter(IntVec3 spot, Pawn organizer)
		{
			Find.LetterStack.ReceiveLetter(this.def.letterTitle, this.def.letterText.Formatted(organizer.Named("ORGANIZER")) + "\n\n" + GatheringWorker_Speech.OutcomeBreakdownForPawn(organizer), LetterDefOf.PositiveEvent, new TargetInfo(spot, organizer.Map, false), null, null, null, null);
		}

		// Token: 0x06005793 RID: 22419 RVA: 0x001CDA78 File Offset: 0x001CBC78
		public static string OutcomeBreakdownForPawn(Pawn organizer)
		{
			return "AbilitySpeechStatInfo".Translate(organizer.Named("ORGANIZER"), StatDefOf.SocialImpact.label) + ": " + organizer.GetStatValue(StatDefOf.SocialImpact, true).ToStringPercent() + "\n\n" + "AbilitySpeechPossibleOutcomes".Translate() + ":\n" + (from o in LordJob_Joinable_Speech.OutcomeChancesForPawn(organizer).Reverse<Tuple<ThoughtDef, float>>()
			select o.Item1.stages[0].LabelCap + " " + o.Item2.ToStringPercent()).ToLineList("  - ", false);
		}
	}
}
