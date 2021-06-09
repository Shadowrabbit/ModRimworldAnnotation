using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001546 RID: 5446
	public class Pawn_InteractionsTracker : IExposable
	{
		// Token: 0x17001245 RID: 4677
		// (get) Token: 0x060075EF RID: 30191 RVA: 0x0023E3CC File Offset: 0x0023C5CC
		private RandomSocialMode CurrentSocialMode
		{
			get
			{
				if (!InteractionUtility.CanInitiateRandomInteraction(this.pawn))
				{
					return RandomSocialMode.Off;
				}
				RandomSocialMode randomSocialMode = RandomSocialMode.Normal;
				JobDriver curDriver = this.pawn.jobs.curDriver;
				if (curDriver != null)
				{
					randomSocialMode = curDriver.DesiredSocialMode();
				}
				PawnDuty duty = this.pawn.mindState.duty;
				if (duty != null && duty.def.socialModeMax < randomSocialMode)
				{
					randomSocialMode = duty.def.socialModeMax;
				}
				if (this.pawn.Drafted && randomSocialMode > RandomSocialMode.Quiet)
				{
					randomSocialMode = RandomSocialMode.Quiet;
				}
				if (this.pawn.InMentalState && randomSocialMode > this.pawn.MentalState.SocialModeMax())
				{
					randomSocialMode = this.pawn.MentalState.SocialModeMax();
				}
				return randomSocialMode;
			}
		}

		// Token: 0x060075F0 RID: 30192 RVA: 0x0004F8B9 File Offset: 0x0004DAB9
		public Pawn_InteractionsTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060075F1 RID: 30193 RVA: 0x0004F8D3 File Offset: 0x0004DAD3
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.wantsRandomInteract, "wantsRandomInteract", false, false);
			Scribe_Values.Look<int>(ref this.lastInteractionTime, "lastInteractionTime", -9999, false);
		}

		// Token: 0x060075F2 RID: 30194 RVA: 0x0023E478 File Offset: 0x0023C678
		public void InteractionsTrackerTick()
		{
			RandomSocialMode currentSocialMode = this.CurrentSocialMode;
			if (currentSocialMode == RandomSocialMode.Off)
			{
				this.wantsRandomInteract = false;
				return;
			}
			if (currentSocialMode == RandomSocialMode.Quiet)
			{
				this.wantsRandomInteract = false;
			}
			if (!this.wantsRandomInteract)
			{
				if (Find.TickManager.TicksGame > this.lastInteractionTime + 320 && this.pawn.IsHashIntervalTick(60))
				{
					int num = 0;
					switch (currentSocialMode)
					{
					case RandomSocialMode.Quiet:
						num = 22000;
						break;
					case RandomSocialMode.Normal:
						num = 6600;
						break;
					case RandomSocialMode.SuperActive:
						num = 550;
						break;
					}
					if (Rand.MTBEventOccurs((float)num, 1f, 60f) && !this.TryInteractRandomly())
					{
						this.wantsRandomInteract = true;
						return;
					}
				}
			}
			else if (this.pawn.IsHashIntervalTick(91) && this.TryInteractRandomly())
			{
				this.wantsRandomInteract = false;
			}
		}

		// Token: 0x060075F3 RID: 30195 RVA: 0x0004F8FD File Offset: 0x0004DAFD
		public bool InteractedTooRecentlyToInteract()
		{
			return Find.TickManager.TicksGame < this.lastInteractionTime + 120;
		}

		// Token: 0x060075F4 RID: 30196 RVA: 0x0004F914 File Offset: 0x0004DB14
		public bool CanInteractNowWith(Pawn recipient, InteractionDef interactionDef = null)
		{
			return recipient.Spawned && InteractionUtility.IsGoodPositionForInteraction(this.pawn, recipient) && InteractionUtility.CanInitiateInteraction(this.pawn, interactionDef) && InteractionUtility.CanReceiveInteraction(recipient, interactionDef);
		}

		// Token: 0x060075F5 RID: 30197 RVA: 0x0023E540 File Offset: 0x0023C740
		public bool TryInteractWith(Pawn recipient, InteractionDef intDef)
		{
			if (DebugSettings.alwaysSocialFight)
			{
				intDef = InteractionDefOf.Insult;
			}
			if (this.pawn == recipient)
			{
				Log.Warning(this.pawn + " tried to interact with self, interaction=" + intDef.defName, false);
				return false;
			}
			if (!this.CanInteractNowWith(recipient, intDef))
			{
				return false;
			}
			if (!intDef.ignoreTimeSinceLastInteraction && this.InteractedTooRecentlyToInteract())
			{
				Log.Error(string.Concat(new object[]
				{
					this.pawn,
					" tried to do interaction ",
					intDef,
					" to ",
					recipient,
					" only ",
					Find.TickManager.TicksGame - this.lastInteractionTime,
					" ticks since last interaction (min is ",
					120,
					")."
				}), false);
				return false;
			}
			List<RulePackDef> list = new List<RulePackDef>();
			if (intDef.initiatorThought != null)
			{
				Pawn_InteractionsTracker.AddInteractionThought(this.pawn, recipient, intDef.initiatorThought);
			}
			if (intDef.recipientThought != null && recipient.needs.mood != null)
			{
				Pawn_InteractionsTracker.AddInteractionThought(recipient, this.pawn, intDef.recipientThought);
			}
			if (intDef.initiatorXpGainSkill != null)
			{
				this.pawn.skills.Learn(intDef.initiatorXpGainSkill, (float)intDef.initiatorXpGainAmount, false);
			}
			if (intDef.recipientXpGainSkill != null && recipient.RaceProps.Humanlike)
			{
				recipient.skills.Learn(intDef.recipientXpGainSkill, (float)intDef.recipientXpGainAmount, false);
			}
			bool flag = false;
			if (recipient.RaceProps.Humanlike)
			{
				flag = recipient.interactions.CheckSocialFightStart(intDef, this.pawn);
			}
			string text;
			string str;
			LetterDef letterDef;
			LookTargets lookTargets;
			if (!flag)
			{
				intDef.Worker.Interacted(this.pawn, recipient, list, out text, out str, out letterDef, out lookTargets);
			}
			else
			{
				text = null;
				str = null;
				letterDef = null;
				lookTargets = null;
			}
			MoteMaker.MakeInteractionBubble(this.pawn, recipient, intDef.interactionMote, intDef.Symbol);
			this.lastInteractionTime = Find.TickManager.TicksGame;
			if (flag)
			{
				list.Add(RulePackDefOf.Sentence_SocialFightStarted);
			}
			PlayLogEntry_Interaction playLogEntry_Interaction = new PlayLogEntry_Interaction(intDef, this.pawn, recipient, list);
			Find.PlayLog.Add(playLogEntry_Interaction);
			if (letterDef != null)
			{
				string text2 = playLogEntry_Interaction.ToGameStringFromPOV(this.pawn, false);
				if (!text.NullOrEmpty())
				{
					text2 = text2 + "\n\n" + text;
				}
				Find.LetterStack.ReceiveLetter(str, text2, letterDef, lookTargets ?? this.pawn, null, null, null, null);
			}
			return true;
		}

		// Token: 0x060075F6 RID: 30198 RVA: 0x0023E7A4 File Offset: 0x0023C9A4
		private static void AddInteractionThought(Pawn pawn, Pawn otherPawn, ThoughtDef thoughtDef)
		{
			if (pawn.needs.mood == null)
			{
				return;
			}
			float statValue = otherPawn.GetStatValue(StatDefOf.SocialImpact, true);
			Thought_Memory thought_Memory = (Thought_Memory)ThoughtMaker.MakeThought(thoughtDef);
			thought_Memory.moodPowerFactor = statValue;
			Thought_MemorySocial thought_MemorySocial = thought_Memory as Thought_MemorySocial;
			if (thought_MemorySocial != null)
			{
				thought_MemorySocial.opinionOffset *= statValue;
			}
			pawn.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, otherPawn);
		}

		// Token: 0x060075F7 RID: 30199 RVA: 0x0023E814 File Offset: 0x0023CA14
		private bool TryInteractRandomly()
		{
			if (this.InteractedTooRecentlyToInteract())
			{
				return false;
			}
			if (!InteractionUtility.CanInitiateRandomInteraction(this.pawn))
			{
				return false;
			}
			List<Pawn> collection = this.pawn.Map.mapPawns.SpawnedPawnsInFaction(this.pawn.Faction);
			Pawn_InteractionsTracker.workingList.Clear();
			Pawn_InteractionsTracker.workingList.AddRange(collection);
			Pawn_InteractionsTracker.workingList.Shuffle<Pawn>();
			List<InteractionDef> allDefsListForReading = DefDatabase<InteractionDef>.AllDefsListForReading;
			for (int i = 0; i < Pawn_InteractionsTracker.workingList.Count; i++)
			{
				Pawn p = Pawn_InteractionsTracker.workingList[i];
				InteractionDef intDef;
				if (p != this.pawn && this.CanInteractNowWith(p, null) && InteractionUtility.CanReceiveRandomInteraction(p) && !this.pawn.HostileTo(p) && allDefsListForReading.TryRandomElementByWeight(delegate(InteractionDef x)
				{
					if (!this.CanInteractNowWith(p, x))
					{
						return 0f;
					}
					return x.Worker.RandomSelectionWeight(this.pawn, p);
				}, out intDef))
				{
					if (this.TryInteractWith(p, intDef))
					{
						Pawn_InteractionsTracker.workingList.Clear();
						return true;
					}
					Log.Error(this.pawn + " failed to interact with " + p, false);
				}
			}
			Pawn_InteractionsTracker.workingList.Clear();
			return false;
		}

		// Token: 0x060075F8 RID: 30200 RVA: 0x0023E954 File Offset: 0x0023CB54
		public bool CheckSocialFightStart(InteractionDef interaction, Pawn initiator)
		{
			if (!DebugSettings.enableRandomMentalStates)
			{
				return false;
			}
			if (this.pawn.needs.mood == null || TutorSystem.TutorialMode)
			{
				return false;
			}
			if (DebugSettings.alwaysSocialFight || Rand.Value < this.SocialFightChance(interaction, initiator))
			{
				this.StartSocialFight(initiator);
				return true;
			}
			return false;
		}

		// Token: 0x060075F9 RID: 30201 RVA: 0x0023E9A8 File Offset: 0x0023CBA8
		public void StartSocialFight(Pawn otherPawn)
		{
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn) || PawnUtility.ShouldSendNotificationAbout(otherPawn))
			{
				Messages.Message("MessageSocialFight".Translate(this.pawn.LabelShort, otherPawn.LabelShort, this.pawn.Named("PAWN1"), otherPawn.Named("PAWN2")), this.pawn, MessageTypeDefOf.ThreatSmall, true);
			}
			this.pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.SocialFighting, null, false, false, otherPawn, false);
			otherPawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.SocialFighting, null, false, false, this.pawn, false);
			TaleRecorder.RecordTale(TaleDefOf.SocialFight, new object[]
			{
				this.pawn,
				otherPawn
			});
		}

		// Token: 0x060075FA RID: 30202 RVA: 0x0023EA84 File Offset: 0x0023CC84
		public float SocialFightChance(InteractionDef interaction, Pawn initiator)
		{
			if (!this.pawn.RaceProps.Humanlike || !initiator.RaceProps.Humanlike)
			{
				return 0f;
			}
			if (!InteractionUtility.HasAnyVerbForSocialFight(this.pawn) || !InteractionUtility.HasAnyVerbForSocialFight(initiator))
			{
				return 0f;
			}
			if (this.pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return 0f;
			}
			if (initiator.Downed || this.pawn.Downed)
			{
				return 0f;
			}
			float num = interaction.socialFightBaseChance;
			num *= Mathf.InverseLerp(0.3f, 1f, this.pawn.health.capacities.GetLevel(PawnCapacityDefOf.Manipulation));
			num *= Mathf.InverseLerp(0.3f, 1f, this.pawn.health.capacities.GetLevel(PawnCapacityDefOf.Moving));
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].CurStage != null)
				{
					num *= hediffs[i].CurStage.socialFightChanceFactor;
				}
			}
			float num2 = (float)this.pawn.relations.OpinionOf(initiator);
			if (num2 < 0f)
			{
				num *= GenMath.LerpDouble(-100f, 0f, 4f, 1f, num2);
			}
			else
			{
				num *= GenMath.LerpDouble(0f, 100f, 1f, 0.6f, num2);
			}
			if (this.pawn.RaceProps.Humanlike)
			{
				List<Trait> allTraits = this.pawn.story.traits.allTraits;
				for (int j = 0; j < allTraits.Count; j++)
				{
					num *= allTraits[j].CurrentData.socialFightChanceFactor;
				}
			}
			int num3 = Mathf.Abs(this.pawn.ageTracker.AgeBiologicalYears - initiator.ageTracker.AgeBiologicalYears);
			if (num3 > 10)
			{
				if (num3 > 50)
				{
					num3 = 50;
				}
				num *= GenMath.LerpDouble(10f, 50f, 1f, 0.25f, (float)num3);
			}
			return Mathf.Clamp01(num);
		}

		// Token: 0x04004DE9 RID: 19945
		private Pawn pawn;

		// Token: 0x04004DEA RID: 19946
		private bool wantsRandomInteract;

		// Token: 0x04004DEB RID: 19947
		private int lastInteractionTime = -9999;

		// Token: 0x04004DEC RID: 19948
		private const int RandomInteractMTBTicks_Quiet = 22000;

		// Token: 0x04004DED RID: 19949
		private const int RandomInteractMTBTicks_Normal = 6600;

		// Token: 0x04004DEE RID: 19950
		private const int RandomInteractMTBTicks_SuperActive = 550;

		// Token: 0x04004DEF RID: 19951
		public const int RandomInteractIntervalMin = 320;

		// Token: 0x04004DF0 RID: 19952
		private const int RandomInteractCheckInterval = 60;

		// Token: 0x04004DF1 RID: 19953
		private const int InteractIntervalAbsoluteMin = 120;

		// Token: 0x04004DF2 RID: 19954
		public const int DirectTalkInteractInterval = 320;

		// Token: 0x04004DF3 RID: 19955
		private static List<Pawn> workingList = new List<Pawn>();
	}
}
