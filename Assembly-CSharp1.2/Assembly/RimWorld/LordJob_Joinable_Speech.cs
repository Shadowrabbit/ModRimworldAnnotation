using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DDC RID: 3548
	public class LordJob_Joinable_Speech : LordJob_Joinable_Gathering
	{
		// Token: 0x17000C6B RID: 3179
		// (get) Token: 0x060050F6 RID: 20726 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C6C RID: 3180
		// (get) Token: 0x060050F7 RID: 20727 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool OrganizerIsStartingPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060050F8 RID: 20728 RVA: 0x00038BD2 File Offset: 0x00036DD2
		public LordJob_Joinable_Speech()
		{
		}

		// Token: 0x060050F9 RID: 20729 RVA: 0x00038C80 File Offset: 0x00036E80
		public LordJob_Joinable_Speech(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef) : base(spot, organizer, gatheringDef)
		{
		}

		// Token: 0x060050FA RID: 20730 RVA: 0x00038C8B File Offset: 0x00036E8B
		protected override LordToil CreateGatheringToil(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef)
		{
			return new LordToil_Speech(spot, gatheringDef, organizer);
		}

		// Token: 0x060050FB RID: 20731 RVA: 0x001BA088 File Offset: 0x001B8288
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil lordToil = this.CreateGatheringToil(this.spot, this.organizer, this.gatheringDef);
			stateGraph.AddToil(lordToil);
			LordToil_End lordToil_End = new LordToil_End();
			stateGraph.AddToil(lordToil_End);
			float speechDuration = 10000f;
			Transition transition = new Transition(lordToil, lordToil_End, false, true);
			transition.AddTrigger(new Trigger_TickCondition(new Func<bool>(this.ShouldBeCalledOff), 1));
			transition.AddTrigger(new Trigger_PawnKilled());
			transition.AddTrigger(new Trigger_PawnLost(PawnLostCondition.LeftVoluntarily, this.organizer));
			transition.AddPreAction(new TransitionAction_Custom(delegate()
			{
				this.ApplyOutcome((float)this.lord.ticksInToil / speechDuration);
			}));
			stateGraph.AddTransition(transition, false);
			this.timeoutTrigger = new Trigger_TicksPassedAfterConditionMet((int)speechDuration, () => GatheringsUtility.InGatheringArea(this.organizer.Position, this.spot, this.organizer.Map), 60);
			Transition transition2 = new Transition(lordToil, lordToil_End, false, true);
			transition2.AddTrigger(this.timeoutTrigger);
			transition2.AddPreAction(new TransitionAction_Custom(delegate()
			{
				this.ApplyOutcome(1f);
			}));
			stateGraph.AddTransition(transition2, false);
			return stateGraph;
		}

		// Token: 0x060050FC RID: 20732 RVA: 0x00038C95 File Offset: 0x00036E95
		public override string GetReport(Pawn pawn)
		{
			if (pawn != this.organizer)
			{
				return "LordReportListeningSpeech".Translate(this.organizer.Named("ORGANIZER"));
			}
			return "LordReportGivingSpeech".Translate();
		}

		// Token: 0x060050FD RID: 20733 RVA: 0x001BA198 File Offset: 0x001B8398
		protected virtual void ApplyOutcome(float progress)
		{
			if (progress < 0.5f)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelSpeechCancelled".Translate(), "LetterSpeechCancelled".Translate(this.organizer.Named("ORGANIZER")).CapitalizeFirst(), LetterDefOf.NegativeEvent, this.organizer, null, null, null, null);
				return;
			}
			ThoughtDef key = LordJob_Joinable_Speech.OutcomeThoughtChances.RandomElementByWeight(delegate(KeyValuePair<ThoughtDef, float> t)
			{
				if (!LordJob_Joinable_Speech.PositiveOutcome(t.Key))
				{
					return t.Value;
				}
				return t.Value * this.organizer.GetStatValue(StatDefOf.SocialImpact, true) * progress;
			}).Key;
			string text = "";
			foreach (Pawn pawn in this.lord.ownedPawns)
			{
				if (pawn != this.organizer && this.organizer.Position.InHorDistOf(pawn.Position, 18f))
				{
					pawn.needs.mood.thoughts.memories.TryGainMemory(key, this.organizer);
					if (key == ThoughtDefOf.InspirationalSpeech && Rand.Chance(LordJob_Joinable_Speech.InspirationChanceFromInspirationalSpeech))
					{
						InspirationDef randomAvailableInspirationDef = pawn.mindState.inspirationHandler.GetRandomAvailableInspirationDef();
						if (randomAvailableInspirationDef != null && pawn.mindState.inspirationHandler.TryStartInspiration_NewTemp(randomAvailableInspirationDef, "LetterSpeechInspiration".Translate(pawn.Named("PAWN"), this.organizer.Named("SPEAKER"))))
						{
							text = text + "  - " + pawn.NameShortColored.Resolve() + "\n";
						}
					}
				}
			}
			TaggedString taggedString = "LetterFinishedSpeech".Translate(this.organizer.Named("ORGANIZER")).CapitalizeFirst() + " " + ("Letter" + key.defName).Translate();
			if (!text.NullOrEmpty())
			{
				taggedString += "\n\n" + "LetterSpeechInspiredListeners".Translate() + "\n\n" + text.TrimEndNewlines();
			}
			if (progress < 1f)
			{
				taggedString += "\n\n" + "LetterSpeechInterrupted".Translate(progress.ToStringPercent(), this.organizer.Named("ORGANIZER"));
			}
			Find.LetterStack.ReceiveLetter(key.stages[0].LabelCap, taggedString, LordJob_Joinable_Speech.PositiveOutcome(key) ? LetterDefOf.PositiveEvent : LetterDefOf.NegativeEvent, this.organizer, null, null, null, null);
			Ability ability = this.organizer.abilities.GetAbility(AbilityDefOf.Speech);
			RoyalTitle mostSeniorTitle = this.organizer.royalty.MostSeniorTitle;
			if (ability != null && mostSeniorTitle != null)
			{
				ability.StartCooldown(mostSeniorTitle.def.speechCooldown.RandomInRange);
			}
		}

		// Token: 0x060050FE RID: 20734 RVA: 0x00038CCF File Offset: 0x00036ECF
		private static bool PositiveOutcome(ThoughtDef outcome)
		{
			return outcome == ThoughtDefOf.EncouragingSpeech || outcome == ThoughtDefOf.InspirationalSpeech;
		}

		// Token: 0x060050FF RID: 20735 RVA: 0x001BA4BC File Offset: 0x001B86BC
		public static IEnumerable<Tuple<ThoughtDef, float>> OutcomeChancesForPawn(Pawn p)
		{
			LordJob_Joinable_Speech.outcomeChancesTemp.Clear();
			float num = 1f / LordJob_Joinable_Speech.OutcomeThoughtChances.Sum(delegate(KeyValuePair<ThoughtDef, float> c)
			{
				if (!LordJob_Joinable_Speech.PositiveOutcome(c.Key))
				{
					return c.Value;
				}
				return c.Value * p.GetStatValue(StatDefOf.SocialImpact, true);
			});
			foreach (KeyValuePair<ThoughtDef, float> keyValuePair in LordJob_Joinable_Speech.OutcomeThoughtChances)
			{
				LordJob_Joinable_Speech.outcomeChancesTemp.Add(new Tuple<ThoughtDef, float>(keyValuePair.Key, (LordJob_Joinable_Speech.PositiveOutcome(keyValuePair.Key) ? (keyValuePair.Value * p.GetStatValue(StatDefOf.SocialImpact, true)) : keyValuePair.Value) * num));
			}
			return LordJob_Joinable_Speech.outcomeChancesTemp;
		}

		// Token: 0x04003418 RID: 13336
		public const float DurationHours = 4f;

		// Token: 0x04003419 RID: 13337
		public static readonly Dictionary<ThoughtDef, float> OutcomeThoughtChances = new Dictionary<ThoughtDef, float>
		{
			{
				ThoughtDefOf.TerribleSpeech,
				0.05f
			},
			{
				ThoughtDefOf.UninspiringSpeech,
				0.15f
			},
			{
				ThoughtDefOf.EncouragingSpeech,
				0.6f
			},
			{
				ThoughtDefOf.InspirationalSpeech,
				0.2f
			}
		};

		// Token: 0x0400341A RID: 13338
		private static readonly float InspirationChanceFromInspirationalSpeech = 0.05f;

		// Token: 0x0400341B RID: 13339
		private static List<Tuple<ThoughtDef, float>> outcomeChancesTemp = new List<Tuple<ThoughtDef, float>>();
	}
}
