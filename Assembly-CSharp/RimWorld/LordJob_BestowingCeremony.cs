using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DB9 RID: 3513
	public class LordJob_BestowingCeremony : LordJob
	{
		// Token: 0x17000C44 RID: 3140
		// (get) Token: 0x0600500C RID: 20492 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool AlwaysShowWeapon
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600500D RID: 20493 RVA: 0x00030B4B File Offset: 0x0002ED4B
		public LordJob_BestowingCeremony()
		{
		}

		// Token: 0x0600500E RID: 20494 RVA: 0x000382E1 File Offset: 0x000364E1
		public LordJob_BestowingCeremony(Pawn bestower, Pawn target, LocalTargetInfo spot, IntVec3 spotCell, Thing shuttle = null, string questEndedSignal = null)
		{
			this.bestower = bestower;
			this.target = target;
			this.spot = spot;
			this.spotCell = spotCell;
			this.shuttle = shuttle;
			this.questEndedSignal = questEndedSignal;
		}

		// Token: 0x0600500F RID: 20495 RVA: 0x001B64FC File Offset: 0x001B46FC
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Bestowing ceremony is a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 3454535, false);
				return stateGraph;
			}
			LordToil_Wait lordToil_Wait = new LordToil_Wait();
			stateGraph.AddToil(lordToil_Wait);
			LordToil_Wait lordToil_Wait2 = new LordToil_Wait();
			stateGraph.AddToil(lordToil_Wait2);
			LordToil_Wait lordToil_Wait3 = new LordToil_Wait();
			stateGraph.AddToil(lordToil_Wait3);
			LordToil_BestowingCeremony_MoveInPlace lordToil_BestowingCeremony_MoveInPlace = new LordToil_BestowingCeremony_MoveInPlace(this.spotCell, this.target);
			stateGraph.AddToil(lordToil_BestowingCeremony_MoveInPlace);
			LordToil_BestowingCeremony_Wait lordToil_BestowingCeremony_Wait = new LordToil_BestowingCeremony_Wait(this.target);
			stateGraph.AddToil(lordToil_BestowingCeremony_Wait);
			this.exitToil = ((this.shuttle == null) ? new LordToil_ExitMap(LocomotionUrgency.Walk, false, false) : new LordToil_EnterShuttleOrLeave(this.shuttle, LocomotionUrgency.Walk, true, true));
			stateGraph.AddToil(this.exitToil);
			Transition transition = new Transition(lordToil_Wait, lordToil_Wait2, false, true);
			transition.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.Tick && this.bestower.Spawned));
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_Wait2, lordToil_BestowingCeremony_MoveInPlace, false, true);
			transition2.AddTrigger(new Trigger_TicksPassed(600));
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_BestowingCeremony_MoveInPlace, lordToil_BestowingCeremony_Wait, false, true);
			transition3.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.Tick && this.bestower.Position == this.spotCell));
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(lordToil_BestowingCeremony_Wait, this.exitToil, false, true);
			transition4.AddTrigger(new Trigger_TicksPassed(30000));
			transition4.AddPostAction(new TransitionAction_Custom(delegate()
			{
				QuestUtility.SendQuestTargetSignals(this.lord.questTags, "CeremonyExpired", this.lord.Named("SUBJECT"));
			}));
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = new Transition(lordToil_BestowingCeremony_Wait, lordToil_Wait3, false, true);
			transition5.AddTrigger(new Trigger_Memo("CeremonyFinished"));
			transition5.AddPostAction(new TransitionAction_Custom(delegate()
			{
				QuestUtility.SendQuestTargetSignals(this.lord.questTags, "CeremonyDone", this.lord.Named("SUBJECT"));
			}));
			stateGraph.AddTransition(transition5, false);
			Transition transition6 = new Transition(lordToil_Wait3, this.exitToil, false, true);
			transition6.AddTrigger(new Trigger_TicksPassed(600));
			stateGraph.AddTransition(transition6, false);
			Transition transition7 = new Transition(lordToil_BestowingCeremony_MoveInPlace, this.exitToil, false, true);
			transition7.AddSource(lordToil_BestowingCeremony_Wait);
			transition7.AddTrigger(new Trigger_BecamePlayerEnemy());
			stateGraph.AddTransition(transition7, false);
			Transition transition8 = new Transition(lordToil_BestowingCeremony_MoveInPlace, this.exitToil, false, true);
			transition8.AddSource(lordToil_BestowingCeremony_Wait);
			transition8.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.Tick && this.bestower.Spawned && !this.bestower.CanReach(this.spotCell, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn)));
			transition8.AddPostAction(new TransitionAction_Custom(delegate()
			{
				Messages.Message("MessageBestowingSpotUnreachable".Translate(), this.bestower, MessageTypeDefOf.NegativeEvent, true);
				QuestUtility.SendQuestTargetSignals(this.lord.questTags, "CeremonyFailed", this.lord.Named("SUBJECT"));
			}));
			stateGraph.AddTransition(transition8, false);
			if (!this.questEndedSignal.NullOrEmpty())
			{
				Transition transition9 = new Transition(lordToil_BestowingCeremony_MoveInPlace, this.exitToil, false, true);
				transition9.AddSource(lordToil_BestowingCeremony_Wait);
				transition9.AddSource(lordToil_Wait);
				transition9.AddSource(lordToil_Wait2);
				transition9.AddTrigger(new Trigger_Signal(this.questEndedSignal));
				stateGraph.AddTransition(transition9, false);
			}
			return stateGraph;
		}

		// Token: 0x06005010 RID: 20496 RVA: 0x00038316 File Offset: 0x00036516
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			if (p == this.bestower)
			{
				this.MakeCeremonyFail();
			}
		}

		// Token: 0x06005011 RID: 20497 RVA: 0x00038327 File Offset: 0x00036527
		public void MakeCeremonyFail()
		{
			QuestUtility.SendQuestTargetSignals(this.lord.questTags, "CeremonyFailed", this.lord.Named("SUBJECT"));
		}

		// Token: 0x06005012 RID: 20498 RVA: 0x001B67B4 File Offset: 0x001B49B4
		private bool CanUseSpot(IntVec3 spot)
		{
			return spot.InBounds(this.bestower.Map) && spot.Standable(this.bestower.Map) && GenSight.LineOfSight(spot, this.bestower.Position, this.bestower.Map, false, null, 0, 0) && this.bestower.CanReach(this.spot, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn);
		}

		// Token: 0x06005013 RID: 20499 RVA: 0x001B682C File Offset: 0x001B4A2C
		private bool TryGetUsableSpotAdjacentToBestower(out IntVec3 pos)
		{
			foreach (int num in Enumerable.Range(1, 4).InRandomOrder(null))
			{
				IntVec3 intVec = this.bestower.Position + GenRadial.ManualRadialPattern[num];
				if (this.CanUseSpot(intVec))
				{
					pos = intVec;
					return true;
				}
			}
			pos = IntVec3.Zero;
			return false;
		}

		// Token: 0x06005014 RID: 20500 RVA: 0x001B68B8 File Offset: 0x001B4AB8
		public void StartCeremony(Pawn pawn)
		{
			if (!JobDriver_BestowingCeremony.AnalyzeThroneRoom(this.bestower, this.target))
			{
				Messages.Message("BestowingCeremonyThroneroomRequirementsNotSatisfied".Translate(this.target.Named("PAWN"), this.target.royalty.GetTitleAwardedWhenUpdating(this.bestower.Faction, this.target.royalty.GetFavor(this.bestower.Faction)).label.Named("TITLE")), this.target, MessageTypeDefOf.NegativeEvent, true);
				((LordJob_BestowingCeremony)this.bestower.GetLord().LordJob).MakeCeremonyFail();
			}
			IntVec3 c = IntVec3.Invalid;
			if (this.spot.Thing != null)
			{
				IntVec3 interactionCell = this.spot.Thing.InteractionCell;
				IntVec3 intVec = this.spotCell;
				foreach (IntVec3 intVec2 in GenSight.PointsOnLineOfSight(interactionCell, intVec))
				{
					if (!(intVec2 == interactionCell) && !(intVec2 == intVec) && this.CanUseSpot(intVec2))
					{
						c = intVec2;
						break;
					}
				}
			}
			if (!c.IsValid && !this.TryGetUsableSpotAdjacentToBestower(out c))
			{
				Messages.Message("MessageBestowerUnreachable".Translate(), this.bestower, MessageTypeDefOf.CautionInput, true);
				return;
			}
			Job job = JobMaker.MakeJob(JobDefOf.BestowingCeremony, this.bestower, c);
			pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}

		// Token: 0x06005015 RID: 20501 RVA: 0x001B6A60 File Offset: 0x001B4C60
		public void FinishCeremony(Pawn pawn)
		{
			LordJob_BestowingCeremony.<>c__DisplayClass20_0 CS$<>8__locals1 = new LordJob_BestowingCeremony.<>c__DisplayClass20_0();
			this.lord.ReceiveMemo("CeremonyFinished");
			RoyalTitleDef currentTitle = this.target.royalty.GetCurrentTitle(this.bestower.Faction);
			RoyalTitleDef titleAwardedWhenUpdating = this.target.royalty.GetTitleAwardedWhenUpdating(this.bestower.Faction, this.target.royalty.GetFavor(this.bestower.Faction));
			string text;
			string str;
			Pawn_RoyaltyTracker.MakeLetterTextForTitleChange(this.target, this.bestower.Faction, currentTitle, titleAwardedWhenUpdating, out text, out str);
			if (pawn.royalty != null)
			{
				pawn.royalty.TryUpdateTitle_NewTemp(this.bestower.Faction, false, titleAwardedWhenUpdating);
			}
			Hediff_Psylink mainPsylinkSource = this.target.GetMainPsylinkSource();
			LordJob_BestowingCeremony.<>c__DisplayClass20_0 CS$<>8__locals2 = CS$<>8__locals1;
			List<AbilityDef> abilitiesPreUpdate;
			if (mainPsylinkSource != null)
			{
				abilitiesPreUpdate = (from a in pawn.abilities.abilities
				select a.def).ToList<AbilityDef>();
			}
			else
			{
				abilitiesPreUpdate = new List<AbilityDef>();
			}
			CS$<>8__locals2.abilitiesPreUpdate = abilitiesPreUpdate;
			ThingOwner<Thing> innerContainer = this.bestower.inventory.innerContainer;
			for (int i = pawn.GetPsylinkLevel(); i < pawn.GetMaxPsylinkLevelByTitle(); i++)
			{
				for (int j = innerContainer.Count - 1; j >= 0; j--)
				{
					if (innerContainer[j].def == ThingDefOf.PsychicAmplifier)
					{
						Thing thing = innerContainer[j];
						innerContainer.RemoveAt(j);
						thing.Destroy(DestroyMode.Vanish);
						break;
					}
				}
				pawn.ChangePsylinkLevel(1, false);
			}
			mainPsylinkSource = this.target.GetMainPsylinkSource();
			List<AbilityDef> list;
			if (mainPsylinkSource != null)
			{
				list = (from a in pawn.abilities.abilities
				select a.def into def
				where !CS$<>8__locals1.abilitiesPreUpdate.Contains(def)
				select def).ToList<AbilityDef>();
			}
			else
			{
				list = new List<AbilityDef>();
			}
			List<AbilityDef> newAbilities = list;
			string text2 = text;
			text2 = text2 + "\n\n" + Hediff_Psylink.MakeLetterTextNewPsylinkLevel(this.target, pawn.GetPsylinkLevel(), newAbilities);
			text2 = text2 + "\n\n" + str;
			Find.LetterStack.ReceiveLetter("LetterLabelGainedRoyalTitle".Translate(titleAwardedWhenUpdating.GetLabelCapFor(pawn).Named("TITLE"), pawn.Named("PAWN")), text2, LetterDefOf.PositiveEvent, pawn, this.bestower.Faction, null, null, null);
		}

		// Token: 0x06005016 RID: 20502 RVA: 0x001B6CC8 File Offset: 0x001B4EC8
		public override void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.bestower, "bestower", false);
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_TargetInfo.Look(ref this.spot, "spot");
			Scribe_Values.Look<string>(ref this.questEndedSignal, "questEndedSignal", null, false);
			Scribe_Values.Look<IntVec3>(ref this.spotCell, "spotCell", default(IntVec3), false);
		}

		// Token: 0x040033BE RID: 13246
		public const int ExpirationTicks = 30000;

		// Token: 0x040033BF RID: 13247
		private const string MemoCeremonyFinished = "CeremonyFinished";

		// Token: 0x040033C0 RID: 13248
		public const int WaitTimeTicks = 600;

		// Token: 0x040033C1 RID: 13249
		public Pawn bestower;

		// Token: 0x040033C2 RID: 13250
		public Pawn target;

		// Token: 0x040033C3 RID: 13251
		public LocalTargetInfo spot;

		// Token: 0x040033C4 RID: 13252
		public IntVec3 spotCell;

		// Token: 0x040033C5 RID: 13253
		public Thing shuttle;

		// Token: 0x040033C6 RID: 13254
		public string questEndedSignal;

		// Token: 0x040033C7 RID: 13255
		private LordToil exitToil;
	}
}
