using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005C1 RID: 1473
	public class MentalBreakWorker
	{
		// Token: 0x06002AED RID: 10989 RVA: 0x001015D0 File Offset: 0x000FF7D0
		public virtual float CommonalityFor(Pawn pawn, bool moodCaused = false)
		{
			float num = this.def.baseCommonality;
			if (pawn.Faction == Faction.OfPlayer && this.def.commonalityFactorPerPopulationCurve != null)
			{
				num *= this.def.commonalityFactorPerPopulationCurve.Evaluate((float)PawnsFinder.AllMaps_FreeColonists.Count<Pawn>());
			}
			return num;
		}

		// Token: 0x06002AEE RID: 10990 RVA: 0x00101624 File Offset: 0x000FF824
		public virtual bool BreakCanOccur(Pawn pawn)
		{
			if (this.def.requiredTrait != null && (pawn.story == null || !pawn.story.traits.HasTrait(this.def.requiredTrait)))
			{
				return false;
			}
			if (this.def.mentalState != null && pawn.story != null && pawn.story.traits.allTraits.Any((Trait tr) => tr.CurrentData.disallowedMentalStates != null && tr.CurrentData.disallowedMentalStates.Contains(this.def.mentalState)))
			{
				return false;
			}
			if (this.def.mentalState != null && !this.def.mentalState.Worker.StateCanOccur(pawn))
			{
				return false;
			}
			if (pawn.story != null)
			{
				IEnumerable<MentalBreakDef> theOnlyAllowedMentalBreaks = pawn.story.traits.TheOnlyAllowedMentalBreaks;
				if (!theOnlyAllowedMentalBreaks.Contains(this.def) && theOnlyAllowedMentalBreaks.Any((MentalBreakDef x) => x.intensity == this.def.intensity && x.Worker.BreakCanOccur(pawn)))
				{
					return false;
				}
			}
			return !TutorSystem.TutorialMode || pawn.Faction != Faction.OfPlayer;
		}

		// Token: 0x06002AEF RID: 10991 RVA: 0x00101758 File Offset: 0x000FF958
		public virtual bool TryStart(Pawn pawn, string reason, bool causedByMood)
		{
			return pawn.mindState.mentalStateHandler.TryStartMentalState(this.def.mentalState, reason, false, causedByMood, null, false, false, false);
		}

		// Token: 0x06002AF0 RID: 10992 RVA: 0x00101788 File Offset: 0x000FF988
		protected bool TrySendLetter(Pawn pawn, string textKey, string reason)
		{
			if (!PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				return false;
			}
			TaggedString label = this.def.LabelCap + ": " + pawn.LabelShortCap;
			TaggedString taggedString = textKey.Translate(pawn.Label, pawn.Named("PAWN")).CapitalizeFirst();
			if (reason != null)
			{
				taggedString += "\n\n" + reason;
			}
			taggedString = taggedString.AdjustedFor(pawn, "PAWN", true);
			Find.LetterStack.ReceiveLetter(label, taggedString, LetterDefOf.NegativeEvent, pawn, null, null, null, null);
			return true;
		}

		// Token: 0x04001A5A RID: 6746
		public MentalBreakDef def;
	}
}
