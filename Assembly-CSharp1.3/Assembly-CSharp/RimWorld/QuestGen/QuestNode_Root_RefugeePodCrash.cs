using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001702 RID: 5890
	public class QuestNode_Root_RefugeePodCrash : QuestNode_Root_WandererJoin
	{
		// Token: 0x06008803 RID: 34819 RVA: 0x0030C9FC File Offset: 0x0030ABFC
		public override Pawn GeneratePawn()
		{
			return ThingUtility.FindPawn(ThingSetMakerDefOf.RefugeePod.root.Generate());
		}

		// Token: 0x06008804 RID: 34820 RVA: 0x0030CA14 File Offset: 0x0030AC14
		public override void SendLetter(Quest quest, Pawn pawn)
		{
			TaggedString label = "LetterLabelRefugeePodCrash".Translate();
			TaggedString taggedString = "RefugeePodCrash".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			taggedString += "\n\n";
			if (pawn.Faction == null)
			{
				taggedString += "RefugeePodCrash_Factionless".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			}
			else if (pawn.Faction.HostileTo(Faction.OfPlayer))
			{
				taggedString += "RefugeePodCrash_Hostile".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			}
			else
			{
				taggedString += "RefugeePodCrash_NonHostile".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			}
			QuestNode_Root_WandererJoin_WalkIn.AppendCharityInfoToLetter("JoinerCharityInfo".Translate(pawn), ref taggedString);
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, ref label, pawn);
			Find.LetterStack.ReceiveLetter(label, taggedString, LetterDefOf.NeutralEvent, new TargetInfo(pawn), null, null, null, null);
		}
	}
}
