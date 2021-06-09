using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A1B RID: 2587
	public class MentalBreakWorker
	{
		// Token: 0x06003DD0 RID: 15824 RVA: 0x00176A04 File Offset: 0x00174C04
		public virtual float CommonalityFor(Pawn pawn, bool moodCaused = false)
		{
			float num = this.def.baseCommonality;
			if (pawn.Faction == Faction.OfPlayer && this.def.commonalityFactorPerPopulationCurve != null)
			{
				num *= this.def.commonalityFactorPerPopulationCurve.Evaluate((float)PawnsFinder.AllMaps_FreeColonists.Count<Pawn>());
			}
			return num;
		}

		// Token: 0x06003DD1 RID: 15825 RVA: 0x00176A58 File Offset: 0x00174C58
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

		// Token: 0x06003DD2 RID: 15826 RVA: 0x0002E8F8 File Offset: 0x0002CAF8
		public virtual bool TryStart(Pawn pawn, string reason, bool causedByMood)
		{
			return pawn.mindState.mentalStateHandler.TryStartMentalState(this.def.mentalState, reason, false, causedByMood, null, false);
		}

		// Token: 0x06003DD3 RID: 15827 RVA: 0x00176B8C File Offset: 0x00174D8C
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

		// Token: 0x04002AD6 RID: 10966
		public MentalBreakDef def;
	}
}
