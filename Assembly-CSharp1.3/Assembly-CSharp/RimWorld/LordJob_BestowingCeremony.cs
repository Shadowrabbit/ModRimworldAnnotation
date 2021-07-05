using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000873 RID: 2163
	public class LordJob_BestowingCeremony : LordJob_Ritual
	{
		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x06003906 RID: 14598 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool AlwaysShowWeapon
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x06003907 RID: 14599 RVA: 0x0013F600 File Offset: 0x0013D800
		public override IntVec3 Spot
		{
			get
			{
				return this.targetSpot.Cell;
			}
		}

		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x06003908 RID: 14600 RVA: 0x0013F610 File Offset: 0x0013D810
		public override string RitualLabel
		{
			get
			{
				return "BestowingCeremonyLabel".Translate().CapitalizeFirst();
			}
		}

		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x06003909 RID: 14601 RVA: 0x0013F634 File Offset: 0x0013D834
		public override bool AllowStartNewGatherings
		{
			get
			{
				return this.lord.CurLordToil != this.ceremonyToil;
			}
		}

		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x0600390A RID: 14602 RVA: 0x0013F64C File Offset: 0x0013D84C
		public Texture2D Icon
		{
			get
			{
				if (this.icon == null)
				{
					this.icon = ContentFinder<Texture2D>.Get("UI/Icons/Rituals/BestowCeremony", true);
				}
				return this.icon;
			}
		}

		// Token: 0x0600390B RID: 14603 RVA: 0x0013F673 File Offset: 0x0013D873
		public LordJob_BestowingCeremony()
		{
		}

		// Token: 0x0600390C RID: 14604 RVA: 0x0013F694 File Offset: 0x0013D894
		public LordJob_BestowingCeremony(Pawn bestower, Pawn target, LocalTargetInfo targetSpot, IntVec3 spotCell, Thing shuttle = null, string questEndedSignal = null)
		{
			this.bestower = bestower;
			this.target = target;
			this.targetSpot = targetSpot;
			this.spotCell = spotCell;
			this.shuttle = shuttle;
			this.questEndedSignal = questEndedSignal;
		}

		// Token: 0x0600390D RID: 14605 RVA: 0x0013F6EC File Offset: 0x0013D8EC
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			if (!ModLister.CheckRoyalty("Bestowing ceremony"))
			{
				return stateGraph;
			}
			this.outcome = (RitualOutcomeEffectWorker_Bestowing)RitualOutcomeEffectDefOf.BestowingCeremony.GetInstance();
			LordToil_Wait lordToil_Wait = new LordToil_Wait();
			stateGraph.AddToil(lordToil_Wait);
			LordToil_Wait lordToil_Wait2 = new LordToil_Wait();
			stateGraph.AddToil(lordToil_Wait2);
			LordToil_Wait lordToil_Wait3 = new LordToil_Wait();
			stateGraph.AddToil(lordToil_Wait3);
			LordToil_BestowingCeremony_MoveInPlace lordToil_BestowingCeremony_MoveInPlace = new LordToil_BestowingCeremony_MoveInPlace(this.spotCell, this.target);
			stateGraph.AddToil(lordToil_BestowingCeremony_MoveInPlace);
			LordToil_BestowingCeremony_Wait lordToil_BestowingCeremony_Wait = new LordToil_BestowingCeremony_Wait(this.target, this.bestower);
			stateGraph.AddToil(lordToil_BestowingCeremony_Wait);
			this.ceremonyToil = new LordToil_BestowingCeremony_Perform(this.target, this.bestower);
			stateGraph.AddToil(this.ceremonyToil);
			this.exitToil = ((this.shuttle == null) ? new LordToil_ExitMap(LocomotionUrgency.Walk, false, false) : new LordToil_EnterShuttleOrLeave(this.shuttle, LocomotionUrgency.Walk, true, true));
			stateGraph.AddToil(this.exitToil);
			TransitionAction_Custom action = new TransitionAction_Custom(delegate()
			{
				this.lord.RemovePawns(this.colonistParticipants);
			});
			Transition transition = new Transition(lordToil_Wait, lordToil_Wait2, false, true);
			transition.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.Tick && this.bestower.Spawned));
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_Wait2, lordToil_BestowingCeremony_MoveInPlace, false, true);
			transition2.AddTrigger(new Trigger_TicksPassed(600));
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_BestowingCeremony_MoveInPlace, lordToil_BestowingCeremony_Wait, false, true);
			transition3.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.Tick && this.bestower.Position == this.spotCell));
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(lordToil_BestowingCeremony_Wait, this.ceremonyToil, false, true);
			transition4.AddTrigger(new Trigger_Memo(LordJob_BestowingCeremony.MemoCeremonyStarted));
			transition4.postActions.Add(new TransitionAction_Custom(delegate()
			{
				this.ceremonyStarted = true;
			}));
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = new Transition(lordToil_BestowingCeremony_Wait, this.exitToil, false, true);
			transition5.AddTrigger(new Trigger_TicksPassed(30000));
			transition5.AddPreAction(action);
			transition5.AddPostAction(new TransitionAction_Custom(delegate()
			{
				QuestUtility.SendQuestTargetSignals(this.lord.questTags, "CeremonyExpired", this.lord.Named("SUBJECT"));
			}));
			stateGraph.AddTransition(transition5, false);
			Transition transition6 = new Transition(this.ceremonyToil, this.exitToil, false, true);
			transition6.AddTrigger(new Trigger_Signal(this.questEndedSignal));
			transition6.AddPreAction(action);
			transition6.AddPreAction(new TransitionAction_Custom(delegate()
			{
				Messages.Message("MessageBestowingInterrupted".Translate(), this.bestower, MessageTypeDefOf.NegativeEvent, true);
			}));
			stateGraph.AddTransition(transition6, false);
			Transition transition7 = new Transition(this.ceremonyToil, lordToil_Wait3, false, true);
			transition7.AddTrigger(new Trigger_Memo("CeremonyFinished"));
			transition7.AddPostAction(new TransitionAction_Custom(delegate()
			{
				QuestUtility.SendQuestTargetSignals(this.lord.questTags, "CeremonyDone", this.lord.Named("SUBJECT"));
			}));
			stateGraph.AddTransition(transition7, false);
			Transition transition8 = new Transition(lordToil_Wait3, this.exitToil, false, true);
			transition8.AddPreAction(action);
			transition8.AddTrigger(new Trigger_TicksPassed(600));
			stateGraph.AddTransition(transition8, false);
			Transition transition9 = new Transition(lordToil_BestowingCeremony_MoveInPlace, this.exitToil, false, true);
			transition9.AddSource(lordToil_BestowingCeremony_Wait);
			transition9.AddTrigger(new Trigger_BecamePlayerEnemy());
			transition9.AddPreAction(action);
			stateGraph.AddTransition(transition9, false);
			Transition transition10 = new Transition(lordToil_BestowingCeremony_MoveInPlace, this.exitToil, false, true);
			transition10.AddSource(lordToil_BestowingCeremony_Wait);
			transition10.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.Tick && this.bestower.Spawned && !this.bestower.CanReach(this.spotCell, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn)));
			transition10.AddPreAction(action);
			transition10.AddPostAction(new TransitionAction_Custom(delegate()
			{
				Messages.Message("MessageBestowingSpotUnreachable".Translate(), this.bestower, MessageTypeDefOf.NegativeEvent, true);
				QuestUtility.SendQuestTargetSignals(this.lord.questTags, "CeremonyFailed", this.lord.Named("SUBJECT"));
			}));
			stateGraph.AddTransition(transition10, false);
			if (!this.questEndedSignal.NullOrEmpty())
			{
				Transition transition11 = new Transition(lordToil_BestowingCeremony_MoveInPlace, this.exitToil, false, true);
				transition11.AddSource(lordToil_BestowingCeremony_Wait);
				transition11.AddSource(lordToil_Wait);
				transition11.AddSource(lordToil_Wait2);
				transition11.AddTrigger(new Trigger_Signal(this.questEndedSignal));
				transition11.AddPreAction(action);
				stateGraph.AddTransition(transition11, false);
			}
			return stateGraph;
		}

		// Token: 0x0600390E RID: 14606 RVA: 0x0013FAB1 File Offset: 0x0013DCB1
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			if (p == this.bestower || p == this.target)
			{
				this.MakeCeremonyFail();
			}
		}

		// Token: 0x0600390F RID: 14607 RVA: 0x0013FACB File Offset: 0x0013DCCB
		public void MakeCeremonyFail()
		{
			QuestUtility.SendQuestTargetSignals(this.lord.questTags, "CeremonyFailed", this.lord.Named("SUBJECT"));
		}

		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x06003910 RID: 14608 RVA: 0x0013FAF2 File Offset: 0x0013DCF2
		public override IEnumerable<Pawn> PawnsToCountTowardsPresence
		{
			get
			{
				return from p in this.lord.ownedPawns
				where p != this.bestower && p != this.target && p.IsColonist
				select p;
			}
		}

		// Token: 0x06003911 RID: 14609 RVA: 0x0013FB10 File Offset: 0x0013DD10
		public override void LordJobTick()
		{
			if (this.ritual != null && this.ritual.behavior != null)
			{
				this.ritual.behavior.Tick(this);
			}
			if (this.ceremonyStarted)
			{
				RitualOutcomeEffectWorker_Bestowing ritualOutcomeEffectWorker_Bestowing = this.outcome;
				if (ritualOutcomeEffectWorker_Bestowing == null)
				{
					return;
				}
				ritualOutcomeEffectWorker_Bestowing.Tick(this, 1f);
			}
		}

		// Token: 0x06003912 RID: 14610 RVA: 0x0013FB64 File Offset: 0x0013DD64
		private bool CanUseSpot(IntVec3 spot)
		{
			return spot.InBounds(this.bestower.Map) && spot.Standable(this.bestower.Map) && GenSight.LineOfSight(spot, this.bestower.Position, this.bestower.Map, false, null, 0, 0) && this.bestower.CanReach(this.targetSpot, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn);
		}

		// Token: 0x06003913 RID: 14611 RVA: 0x0013FBDC File Offset: 0x0013DDDC
		private IntVec3 TryGetUsableSpotAdjacentToBestower()
		{
			foreach (int num in Enumerable.Range(1, 4).InRandomOrder(null))
			{
				IntVec3 intVec = this.bestower.Position + GenRadial.ManualRadialPattern[num];
				if (this.CanUseSpot(intVec))
				{
					return intVec;
				}
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06003914 RID: 14612 RVA: 0x0013FC5C File Offset: 0x0013DE5C
		public IntVec3 GetSpot()
		{
			IntVec3 result = IntVec3.Invalid;
			if (this.targetSpot.Thing != null)
			{
				IntVec3 interactionCell = this.targetSpot.Thing.InteractionCell;
				IntVec3 intVec = this.spotCell;
				foreach (IntVec3 intVec2 in GenSight.PointsOnLineOfSight(interactionCell, intVec))
				{
					if (!(intVec2 == interactionCell) && !(intVec2 == intVec) && this.CanUseSpot(intVec2))
					{
						result = intVec2;
						break;
					}
				}
			}
			if (result.IsValid)
			{
				return result;
			}
			return this.TryGetUsableSpotAdjacentToBestower();
		}

		// Token: 0x06003915 RID: 14613 RVA: 0x0013FD04 File Offset: 0x0013DF04
		public override string GetReport(Pawn pawn)
		{
			return "LordReportAttending".Translate() + " " + "BestowingCeremonyLabel".Translate();
		}

		// Token: 0x06003916 RID: 14614 RVA: 0x0013FD30 File Offset: 0x0013DF30
		public void FinishCeremony(Pawn pawn)
		{
			this.lord.ReceiveMemo("CeremonyFinished");
			this.totalPresenceTmp.Clear();
			foreach (KeyValuePair<Pawn, int> keyValuePair in this.ceremonyToil.Data.presentForTicks)
			{
				if (keyValuePair.Key != null && !keyValuePair.Key.Dead)
				{
					if (!this.totalPresenceTmp.ContainsKey(keyValuePair.Key))
					{
						this.totalPresenceTmp.Add(keyValuePair.Key, keyValuePair.Value);
					}
					else
					{
						Dictionary<Pawn, int> dictionary = this.totalPresenceTmp;
						Pawn key = keyValuePair.Key;
						dictionary[key] += keyValuePair.Value;
					}
				}
			}
			this.totalPresenceTmp.RemoveAll((KeyValuePair<Pawn, int> tp) => tp.Value < 2500);
			this.outcome.Apply(1f, this.totalPresenceTmp, this);
		}

		// Token: 0x06003917 RID: 14615 RVA: 0x0013FE50 File Offset: 0x0013E050
		public override IEnumerable<Gizmo> GetPawnGizmos(Pawn p)
		{
			if (p != this.bestower && p != this.target)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandLeaveBestowingCeremony".Translate(),
					defaultDesc = "CommandLeaveBestowingCeremonyDesc".Translate(),
					icon = this.Icon,
					action = delegate()
					{
						this.lord.Notify_PawnLost(p, PawnLostCondition.ForcedByPlayerAction, null);
						SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					},
					hotKey = KeyBindingDefOf.Misc5
				};
			}
			yield break;
		}

		// Token: 0x06003918 RID: 14616 RVA: 0x0013FE68 File Offset: 0x0013E068
		public override void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.bestower, "bestower", false);
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_TargetInfo.Look(ref this.targetSpot, "targetSpot");
			Scribe_Values.Look<string>(ref this.questEndedSignal, "questEndedSignal", null, false);
			Scribe_Values.Look<IntVec3>(ref this.spotCell, "spotCell", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.ceremonyStarted, "ceremonyStarted", false, false);
			Scribe_Collections.Look<Pawn>(ref this.colonistParticipants, "colonistParticipants", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x04001F56 RID: 8022
		public const int ExpirationTicks = 30000;

		// Token: 0x04001F57 RID: 8023
		public static readonly string MemoCeremonyStarted = "CeremonyStarted";

		// Token: 0x04001F58 RID: 8024
		private const string MemoCeremonyFinished = "CeremonyFinished";

		// Token: 0x04001F59 RID: 8025
		public const int WaitTimeTicks = 600;

		// Token: 0x04001F5A RID: 8026
		public Pawn bestower;

		// Token: 0x04001F5B RID: 8027
		public Pawn target;

		// Token: 0x04001F5C RID: 8028
		public LocalTargetInfo targetSpot;

		// Token: 0x04001F5D RID: 8029
		public IntVec3 spotCell;

		// Token: 0x04001F5E RID: 8030
		public Thing shuttle;

		// Token: 0x04001F5F RID: 8031
		public string questEndedSignal;

		// Token: 0x04001F60 RID: 8032
		public List<Pawn> colonistParticipants = new List<Pawn>();

		// Token: 0x04001F61 RID: 8033
		public bool ceremonyStarted;

		// Token: 0x04001F62 RID: 8034
		private LordToil_BestowingCeremony_Perform ceremonyToil;

		// Token: 0x04001F63 RID: 8035
		private LordToil exitToil;

		// Token: 0x04001F64 RID: 8036
		private RitualOutcomeEffectWorker_Bestowing outcome;

		// Token: 0x04001F65 RID: 8037
		private Texture2D icon;

		// Token: 0x04001F66 RID: 8038
		private Dictionary<Pawn, int> totalPresenceTmp = new Dictionary<Pawn, int>();
	}
}
