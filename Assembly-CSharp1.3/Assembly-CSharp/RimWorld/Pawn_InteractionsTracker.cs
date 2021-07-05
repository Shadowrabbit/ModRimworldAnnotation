using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E88 RID: 3720
	public class Pawn_InteractionsTracker : IExposable
	{
		// Token: 0x17000F29 RID: 3881
		// (get) Token: 0x0600571E RID: 22302 RVA: 0x001D90A8 File Offset: 0x001D72A8
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

		// Token: 0x0600571F RID: 22303 RVA: 0x001D9154 File Offset: 0x001D7354
		public Pawn_InteractionsTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06005720 RID: 22304 RVA: 0x001D916E File Offset: 0x001D736E
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.wantsRandomInteract, "wantsRandomInteract", false, false);
			Scribe_Values.Look<int>(ref this.lastInteractionTime, "lastInteractionTime", -9999, false);
		}

		// Token: 0x06005721 RID: 22305 RVA: 0x001D9198 File Offset: 0x001D7398
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

		// Token: 0x06005722 RID: 22306 RVA: 0x001D9260 File Offset: 0x001D7460
		public bool InteractedTooRecentlyToInteract()
		{
			return Find.TickManager.TicksGame < this.lastInteractionTime + 120;
		}

		// Token: 0x06005723 RID: 22307 RVA: 0x001D9277 File Offset: 0x001D7477
		public bool CanInteractNowWith(Pawn recipient, InteractionDef interactionDef = null)
		{
			return recipient.Spawned && InteractionUtility.IsGoodPositionForInteraction(this.pawn, recipient) && InteractionUtility.CanInitiateInteraction(this.pawn, interactionDef) && InteractionUtility.CanReceiveInteraction(recipient, interactionDef);
		}

		// Token: 0x06005724 RID: 22308 RVA: 0x001D92B0 File Offset: 0x001D74B0
		public bool TryInteractWith(Pawn recipient, InteractionDef intDef)
		{
			if (DebugSettings.alwaysSocialFight)
			{
				intDef = InteractionDefOf.Insult;
			}
			if (this.pawn == recipient)
			{
				Log.Warning(this.pawn + " tried to interact with self, interaction=" + intDef.defName);
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
				}));
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
			MoteMaker.MakeInteractionBubble(this.pawn, recipient, intDef.interactionMote, intDef.GetSymbol(this.pawn.Faction, this.pawn.Ideo), intDef.GetSymbolColor(this.pawn.Faction));
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

		// Token: 0x06005725 RID: 22309 RVA: 0x001D9538 File Offset: 0x001D7738
		public static void AddInteractionThought(Pawn pawn, Pawn otherPawn, ThoughtDef thoughtDef)
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

		// Token: 0x06005726 RID: 22310 RVA: 0x001D95A8 File Offset: 0x001D77A8
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
					Log.Error(this.pawn + " failed to interact with " + p);
				}
			}
			Pawn_InteractionsTracker.workingList.Clear();
			return false;
		}

		// Token: 0x06005727 RID: 22311 RVA: 0x001D96E4 File Offset: 0x001D78E4
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
				this.StartSocialFight(initiator, "MessageSocialFight");
				return true;
			}
			return false;
		}

		// Token: 0x06005728 RID: 22312 RVA: 0x001D973C File Offset: 0x001D793C
		public void StartSocialFight(Pawn otherPawn, string messageKey = "MessageSocialFight")
		{
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn) || PawnUtility.ShouldSendNotificationAbout(otherPawn))
			{
				Messages.Message(messageKey.Translate(this.pawn.LabelShort, otherPawn.LabelShort, this.pawn.Named("PAWN1"), otherPawn.Named("PAWN2")), this.pawn, MessageTypeDefOf.ThreatSmall, true);
			}
			this.pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.SocialFighting, null, false, false, otherPawn, false, false, false);
			otherPawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.SocialFighting, null, false, false, this.pawn, false, false, false);
			TaleRecorder.RecordTale(TaleDefOf.SocialFight, new object[]
			{
				this.pawn,
				otherPawn
			});
		}

		// Token: 0x06005729 RID: 22313 RVA: 0x001D9818 File Offset: 0x001D7A18
		public bool SocialFightPossible(Pawn otherPawn)
		{
			return this.pawn.RaceProps.Humanlike && otherPawn.RaceProps.Humanlike && InteractionUtility.HasAnyVerbForSocialFight(this.pawn) && InteractionUtility.HasAnyVerbForSocialFight(otherPawn) && !this.pawn.WorkTagIsDisabled(WorkTags.Violent) && !otherPawn.Downed && !this.pawn.Downed;
		}

		// Token: 0x0600572A RID: 22314 RVA: 0x001D9888 File Offset: 0x001D7A88
		public float SocialFightChance(InteractionDef interaction, Pawn initiator)
		{
			if (!this.SocialFightPossible(initiator))
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

		// Token: 0x0400338E RID: 13198
		private Pawn pawn;

		// Token: 0x0400338F RID: 13199
		private bool wantsRandomInteract;

		// Token: 0x04003390 RID: 13200
		private int lastInteractionTime = -9999;

		// Token: 0x04003391 RID: 13201
		private const int RandomInteractMTBTicks_Quiet = 22000;

		// Token: 0x04003392 RID: 13202
		private const int RandomInteractMTBTicks_Normal = 6600;

		// Token: 0x04003393 RID: 13203
		private const int RandomInteractMTBTicks_SuperActive = 550;

		// Token: 0x04003394 RID: 13204
		public const int RandomInteractIntervalMin = 320;

		// Token: 0x04003395 RID: 13205
		private const int RandomInteractCheckInterval = 60;

		// Token: 0x04003396 RID: 13206
		private const int InteractIntervalAbsoluteMin = 120;

		// Token: 0x04003397 RID: 13207
		public const int DirectTalkInteractInterval = 320;

		// Token: 0x04003398 RID: 13208
		private static List<Pawn> workingList = new List<Pawn>();
	}
}
