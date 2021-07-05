using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F71 RID: 3953
	public class RitualOutcomeEffectWorker_Speech : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x17001035 RID: 4149
		// (get) Token: 0x06005DBF RID: 23999 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005DC0 RID: 24000 RVA: 0x002019DD File Offset: 0x001FFBDD
		public RitualOutcomeEffectWorker_Speech()
		{
		}

		// Token: 0x06005DC1 RID: 24001 RVA: 0x002019E5 File Offset: 0x001FFBE5
		public RitualOutcomeEffectWorker_Speech(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x06005DC2 RID: 24002 RVA: 0x002023EC File Offset: 0x002005EC
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			Pawn organizer = jobRitual.Organizer;
			float quality = base.GetQuality(jobRitual, progress);
			OutcomeChance outcome = this.GetOutcome(quality, jobRitual);
			ThoughtDef memory = outcome.memory;
			LookTargets lookTargets = organizer;
			string text = null;
			if (jobRitual.Ritual != null)
			{
				this.ApplyAttachableOutcome(totalPresence, jobRitual, outcome, out text, ref lookTargets);
			}
			string text2 = "";
			string text3 = "";
			foreach (KeyValuePair<Pawn, int> keyValuePair in totalPresence)
			{
				Pawn key = keyValuePair.Key;
				if (key != organizer && organizer.Position.InHorDistOf(key.Position, 18f))
				{
					Thought_Memory thought_Memory = base.MakeMemory(key, jobRitual, memory);
					thought_Memory.otherPawn = organizer;
					thought_Memory.moodPowerFactor = ((key.Ideo == organizer.Ideo) ? 1f : 0.5f);
					key.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, null);
					if (memory == ThoughtDefOf.InspirationalSpeech)
					{
						if (Rand.Chance(RitualOutcomeEffectWorker_Speech.InspirationChanceFromInspirationalSpeech))
						{
							InspirationDef randomAvailableInspirationDef = key.mindState.inspirationHandler.GetRandomAvailableInspirationDef();
							if (randomAvailableInspirationDef != null && key.mindState.inspirationHandler.TryStartInspiration(randomAvailableInspirationDef, "LetterSpeechInspiration".Translate(key.Named("PAWN"), organizer.Named("SPEAKER")), true))
							{
								text2 = text2 + "  - " + key.NameShortColored.Resolve() + "\n";
							}
						}
						if (ModsConfig.IdeologyActive && key.Ideo != organizer.Ideo && Rand.Chance(RitualOutcomeEffectWorker_Speech.ConversionChanceFromInspirationalSpeech))
						{
							key.ideo.SetIdeo(organizer.Ideo);
							text3 = text3 + "  - " + key.NameShortColored.Resolve() + "\n";
						}
					}
				}
			}
			TaggedString taggedString = "LetterFinishedSpeech".Translate(organizer.Named("ORGANIZER")).CapitalizeFirst() + " " + ("Letter" + memory.defName).Translate() + "\n\n" + this.OutcomeQualityBreakdownDesc(quality, progress, jobRitual);
			if (!text3.NullOrEmpty())
			{
				taggedString += "\n\n" + "LetterSpeechConvertedListeners".Translate(organizer.Named("PAWN"), organizer.Ideo.Named("IDEO")).CapitalizeFirst() + "\n\n" + text3.TrimEndNewlines();
			}
			if (!text2.NullOrEmpty())
			{
				taggedString += "\n\n" + "LetterSpeechInspiredListeners".Translate() + "\n\n" + text2.TrimEndNewlines();
			}
			if (progress < 1f)
			{
				taggedString += "\n\n" + "LetterSpeechInterrupted".Translate(progress.ToStringPercent(), organizer.Named("ORGANIZER"));
			}
			if (text != null)
			{
				taggedString += "\n\n" + text;
			}
			Find.LetterStack.ReceiveLetter("OutcomeLetterLabel".Translate(outcome.label.Named("OUTCOMELABEL"), jobRitual.Ritual.Label.Named("RITUALLABEL")), taggedString, RitualOutcomeEffectWorker_Speech.PositiveOutcome(memory) ? LetterDefOf.RitualOutcomePositive : LetterDefOf.RitualOutcomeNegative, lookTargets, null, null, null, null);
			Ability ability = organizer.abilities.GetAbility(AbilityDefOf.Speech, true);
			RoyalTitle mostSeniorTitle = organizer.royalty.MostSeniorTitle;
			if (ability != null && mostSeniorTitle != null)
			{
				ability.StartCooldown(mostSeniorTitle.def.speechCooldown.RandomInRange);
			}
		}

		// Token: 0x06005DC3 RID: 24003 RVA: 0x002027E0 File Offset: 0x002009E0
		private static bool PositiveOutcome(ThoughtDef outcome)
		{
			return outcome == ThoughtDefOf.EncouragingSpeech || outcome == ThoughtDefOf.InspirationalSpeech;
		}

		// Token: 0x0400362C RID: 13868
		private static readonly float InspirationChanceFromInspirationalSpeech = 0.05f;

		// Token: 0x0400362D RID: 13869
		private static readonly float ConversionChanceFromInspirationalSpeech = 0.02f;
	}
}
