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
	// Token: 0x0200088E RID: 2190
	public class LordJob_Ritual : LordJob_Joinable_Gathering
	{
		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x060039C4 RID: 14788 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A4B RID: 2635
		// (get) Token: 0x060039C5 RID: 14789 RVA: 0x00143628 File Offset: 0x00141828
		public Precept_Ritual Ritual
		{
			get
			{
				return this.ritual;
			}
		}

		// Token: 0x17000A4C RID: 2636
		// (get) Token: 0x060039C6 RID: 14790 RVA: 0x00143630 File Offset: 0x00141830
		public virtual string RitualLabel
		{
			get
			{
				if (this.ritual == null)
				{
					return "Ritual".Translate().Resolve();
				}
				return this.ritual.LabelCap;
			}
		}

		// Token: 0x17000A4D RID: 2637
		// (get) Token: 0x060039C7 RID: 14791 RVA: 0x00143663 File Offset: 0x00141863
		protected string CallOffSignal
		{
			get
			{
				return this.lord.GetUniqueLoadID() + ".callOffRitual";
			}
		}

		// Token: 0x17000A4E RID: 2638
		// (get) Token: 0x060039C8 RID: 14792 RVA: 0x0014367A File Offset: 0x0014187A
		protected string CancelSignal
		{
			get
			{
				return this.lord.GetUniqueLoadID() + ".cancelRitual";
			}
		}

		// Token: 0x17000A4F RID: 2639
		// (get) Token: 0x060039C9 RID: 14793 RVA: 0x00143691 File Offset: 0x00141891
		public override int TicksLeft
		{
			get
			{
				return (int)((float)this.durationTicks - this.ticksPassedWithProgress);
			}
		}

		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x060039CA RID: 14794 RVA: 0x001436A2 File Offset: 0x001418A2
		public float Progress
		{
			get
			{
				return this.ticksPassedWithProgress / (float)this.durationTicks;
			}
		}

		// Token: 0x17000A51 RID: 2641
		// (get) Token: 0x060039CB RID: 14795 RVA: 0x001436B2 File Offset: 0x001418B2
		public int StageIndex
		{
			get
			{
				return this.stageIndex;
			}
		}

		// Token: 0x17000A52 RID: 2642
		// (get) Token: 0x060039CC RID: 14796 RVA: 0x001436BA File Offset: 0x001418BA
		protected string TimeLeftPostfix
		{
			get
			{
				return "(" + "RitualEndsIn".Translate(this.TicksLeft.ToStringTicksToPeriod(true, false, true, true)) + ")";
			}
		}

		// Token: 0x17000A53 RID: 2643
		// (get) Token: 0x060039CD RID: 14797 RVA: 0x001436F3 File Offset: 0x001418F3
		protected virtual int MinTicksToFinish
		{
			get
			{
				if (this.ignoreDurationToFinish)
				{
					return -1;
				}
				if (this.lastEssentialStageIndex != -1)
				{
					return this.lastEssentialStageEndedTick + 1;
				}
				return this.durationTicks;
			}
		}

		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x060039CE RID: 14798 RVA: 0x00143717 File Offset: 0x00141917
		public Room GetRoom
		{
			get
			{
				return this.spot.GetRoom(base.Map);
			}
		}

		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x060039CF RID: 14799 RVA: 0x0014372C File Offset: 0x0014192C
		public IntVec2 RoomBoundsCached
		{
			get
			{
				if (this.roomBoundsCached.IsInvalid)
				{
					new IntVec2(int.MaxValue, int.MaxValue);
					new IntVec2(int.MinValue, int.MinValue);
					Room getRoom = this.GetRoom;
					if (getRoom == null || getRoom.PsychologicallyOutdoors || !getRoom.ProperRoom)
					{
						this.roomBoundsCached = IntVec2.Invalid;
					}
					else
					{
						this.roomBoundsCached = new IntVec2(getRoom.ExtentsClose.Width, getRoom.ExtentsClose.Height);
					}
				}
				return this.roomBoundsCached;
			}
		}

		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x060039D0 RID: 14800 RVA: 0x001437BA File Offset: 0x001419BA
		public Sustainer AmbiencePlaying
		{
			get
			{
				Sustainer soundPlaying;
				if ((soundPlaying = this.ambiencePlaying) == null)
				{
					Precept_Ritual precept_Ritual = this.ritual;
					if (precept_Ritual == null)
					{
						return null;
					}
					RitualBehaviorWorker behavior = precept_Ritual.behavior;
					if (behavior == null)
					{
						return null;
					}
					soundPlaying = behavior.SoundPlaying;
				}
				return soundPlaying;
			}
		}

		// Token: 0x060039D1 RID: 14801 RVA: 0x001437E4 File Offset: 0x001419E4
		public LordJob_Ritual()
		{
		}

		// Token: 0x060039D2 RID: 14802 RVA: 0x00143874 File Offset: 0x00141A74
		public LordJob_Ritual(TargetInfo selectedTarget, Precept_Ritual ritual, RitualObligation obligation, List<RitualStage> allStages, RitualRoleAssignments assignments, Pawn organizer = null)
		{
			this.spot = ((selectedTarget.Thing != null) ? selectedTarget.Thing.OccupiedRect().CenterCell : selectedTarget.Cell);
			this.selectedTarget = selectedTarget;
			this.ritual = ritual;
			this.obligation = obligation;
			this.stages = allStages;
			this.organizer = organizer;
			this.assignments = assignments;
			if (ritual != null && ritual.playsIdeoMusic)
			{
				this.ambienceDef = ritual.ideo.SoundOngoingRitual;
			}
			this.durationTicks = ritual.behavior.def.durationTicks.RandomInRange;
			this.repeatPenalty = ritual.RepeatPenaltyActive;
		}

		// Token: 0x060039D3 RID: 14803 RVA: 0x001439A0 File Offset: 0x00141BA0
		public void PreparePawns()
		{
			foreach (Pawn pawn in this.assignments.Participants)
			{
				if (pawn.drafter != null)
				{
					pawn.drafter.Drafted = false;
				}
				if (!pawn.Awake())
				{
					RestUtility.WakeUp(pawn);
				}
			}
		}

		// Token: 0x060039D4 RID: 14804 RVA: 0x00143A14 File Offset: 0x00141C14
		public override void Notify_AddedToLord()
		{
			this.stageSecondFocus = new List<TargetInfo>();
			this.stagePawnSecondFocus = new List<RitualStagePawnSecondFocus>();
			this.ritualStagePositions = new List<RitualStagePositions>();
			this.ritualStageOnTickActions = new List<RitualStageOnTickActions>();
			if (this.stages != null)
			{
				for (int i = 0; i < this.stages.Count; i++)
				{
					RitualStage ritualStage = this.stages[i];
					if (ritualStage.essential && ritualStage != this.stages.Last<RitualStage>())
					{
						this.lastEssentialStageIndex = i;
					}
					this.stageSecondFocus.Add(ritualStage.GetSecondFocus(this));
					IEnumerable<RitualStagePawnSecondFocus> pawnSecondFoci = ritualStage.GetPawnSecondFoci(this);
					if (pawnSecondFoci != null)
					{
						foreach (RitualStagePawnSecondFocus ritualStagePawnSecondFocus in pawnSecondFoci)
						{
							ritualStagePawnSecondFocus.stageIndex = i;
							this.stagePawnSecondFocus.Add(ritualStagePawnSecondFocus);
						}
					}
					this.ritualStagePositions.Add(new RitualStagePositions());
					foreach (Pawn pawn in this.assignments.Participants)
					{
						this.ritualStagePositions[i].positions.Add(pawn, ritualStage.GetPawnPosition(this.spot, pawn, this, null));
					}
					List<ActionOnTick> list = new List<ActionOnTick>();
					if (ritualStage.tickActionMaker != null)
					{
						list.AddRange(ritualStage.tickActionMaker.GenerateTimedActions(this, ritualStage));
					}
					this.ritualStageOnTickActions.Add(new RitualStageOnTickActions
					{
						actions = list
					});
				}
			}
			if (this.ritual != null && this.ritual.ideo != null && this.ritual.ideo.RitualEffect != null && this.ritual.def.usesIdeoVisualEffects)
			{
				RitualVisualEffect instance = this.ritual.ideo.RitualEffect.GetInstance();
				instance.Setup(this, false);
				this.effectWorkers.Add(instance);
			}
			bool flag = this.GetRoom != null && !this.GetRoom.PsychologicallyOutdoors && this.GetRoom.ProperRoom;
			this.lord.lordManager.stencilDrawers.Add(new StencilDrawerForCells
			{
				sourceLord = this.lord,
				cells = (flag ? this.GetRoom.Cells.ToList<IntVec3>() : null),
				center = this.selectedTarget.CenterVector3,
				dimensionsIfNoCells = LordJob_Ritual.DefaultRitualVfxScale,
				ticksLeftWithoutLord = 60
			});
			this.initedVisualEffects = true;
		}

		// Token: 0x060039D5 RID: 14805 RVA: 0x00143CB8 File Offset: 0x00141EB8
		public override StateGraph CreateGraph()
		{
			LordJob_Ritual.<>c__DisplayClass64_0 CS$<>8__locals1 = new LordJob_Ritual.<>c__DisplayClass64_0();
			CS$<>8__locals1.<>4__this = this;
			StateGraph stateGraph = new StateGraph();
			CS$<>8__locals1.toils = new List<LordToil_Ritual>();
			foreach (RitualStage stage in this.stages)
			{
				LordToil_Ritual lordToil_Ritual = this.MakeToil(stage);
				stateGraph.AddToil(lordToil_Ritual);
				CS$<>8__locals1.toils.Add(lordToil_Ritual);
			}
			LordToil_End lordToil_End = new LordToil_End();
			stateGraph.AddToil(lordToil_End);
			for (int i = 0; i < this.stages.Count; i++)
			{
				LordJob_Ritual.<>c__DisplayClass64_1 CS$<>8__locals2 = new LordJob_Ritual.<>c__DisplayClass64_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				LordToil_Ritual lordToil_Ritual2 = CS$<>8__locals2.CS$<>8__locals1.toils[i];
				CS$<>8__locals2.stage = this.stages[i];
				CS$<>8__locals2.iCapture = i;
				lordToil_Ritual2.startAction = delegate()
				{
					CS$<>8__locals2.CS$<>8__locals1.<>4__this.ignoreDurationToFinish = (CS$<>8__locals2.CS$<>8__locals1.<>4__this.ignoreDurationToFinish | CS$<>8__locals2.stage.ignoreDurationToFinishAfterStage);
					CS$<>8__locals2.CS$<>8__locals1.<>4__this.stageTicks = 0;
					Find.SignalManager.SendSignal(new Signal("RitualStarted"));
					LordJob_Ritual.<>c__DisplayClass64_2 CS$<>8__locals5;
					CS$<>8__locals5.existingPosHighlights = new Dictionary<IntVec3, Mote>();
					CS$<>8__locals5.existingPawnHighlights = new Dictionary<Pawn, Mote>();
					foreach (KeyValuePair<IntVec3, Mote> keyValuePair in CS$<>8__locals2.CS$<>8__locals1.<>4__this.highlightedPositions)
					{
						CS$<>8__locals5.existingPosHighlights.Add(keyValuePair.Key, keyValuePair.Value);
					}
					foreach (KeyValuePair<Pawn, Mote> keyValuePair2 in CS$<>8__locals2.CS$<>8__locals1.<>4__this.highlightedPawns)
					{
						CS$<>8__locals5.existingPawnHighlights.Add(keyValuePair2.Key, keyValuePair2.Value);
					}
					CS$<>8__locals2.CS$<>8__locals1.<>4__this.highlightedPositions.Clear();
					CS$<>8__locals2.CS$<>8__locals1.<>4__this.highlightedPawns.Clear();
					foreach (Pawn pawn in CS$<>8__locals2.CS$<>8__locals1.<>4__this.assignments.Participants)
					{
						RitualRole ritualRole = CS$<>8__locals2.CS$<>8__locals1.<>4__this.RoleFor(pawn, true);
						if (ritualRole != null && CS$<>8__locals2.stage.highlightRolePositions.Contains(ritualRole.id))
						{
							CS$<>8__locals2.CS$<>8__locals1.<CreateGraph>g__TryAddPosHighlight|11(pawn.Position, ref CS$<>8__locals5);
						}
						if (ritualRole != null && CS$<>8__locals2.stage.highlightRolePawns.Contains(ritualRole.id))
						{
							CS$<>8__locals2.CS$<>8__locals1.<CreateGraph>g__TryAddPawnHighlight|12(pawn, ref CS$<>8__locals5);
						}
					}
					foreach (KeyValuePair<Pawn, PawnStagePosition> keyValuePair3 in CS$<>8__locals2.CS$<>8__locals1.<>4__this.ritualStagePositions[CS$<>8__locals2.iCapture].positions)
					{
						if (keyValuePair3.Value.highlight)
						{
							CS$<>8__locals2.CS$<>8__locals1.<CreateGraph>g__TryAddPosHighlight|11(keyValuePair3.Value.cell, ref CS$<>8__locals5);
						}
					}
					foreach (RitualVisualEffect ritualVisualEffect in CS$<>8__locals2.CS$<>8__locals1.<>4__this.effectWorkersCurrentStage)
					{
						ritualVisualEffect.Cleanup();
					}
					CS$<>8__locals2.CS$<>8__locals1.<>4__this.effectWorkersCurrentStage.Clear();
					if (CS$<>8__locals2.stage.visualEffectDef != null)
					{
						RitualVisualEffect instance = CS$<>8__locals2.stage.visualEffectDef.GetInstance();
						instance.Setup(CS$<>8__locals2.CS$<>8__locals1.<>4__this, false);
						CS$<>8__locals2.CS$<>8__locals1.<>4__this.effectWorkersCurrentStage.Add(instance);
					}
				};
				if (CS$<>8__locals2.stage.endTriggers.Any((StageEndTrigger e) => e is StageEndTrigger_DurationPercentage))
				{
					lordToil_Ritual2.tickAction = delegate()
					{
						CS$<>8__locals2.CS$<>8__locals1.<>4__this.ticksPassedWithProgress = CS$<>8__locals2.CS$<>8__locals1.<>4__this.ticksPassedWithProgress + CS$<>8__locals2.stage.ProgressPerTick(CS$<>8__locals2.CS$<>8__locals1.<>4__this);
					};
				}
				Transition transition = new Transition(lordToil_Ritual2, lordToil_End, false, true);
				foreach (Trigger trigger in this.CallOffTriggers())
				{
					transition.AddTrigger(trigger);
				}
				if (this.organizer != null)
				{
					transition.AddTrigger(new Trigger_PawnLost(PawnLostCondition.LeftVoluntarily, this.organizer));
				}
				Transition transition2 = transition;
				Action action;
				if ((action = CS$<>8__locals2.CS$<>8__locals1.<>9__4) == null)
				{
					action = (CS$<>8__locals2.CS$<>8__locals1.<>9__4 = delegate()
					{
						CS$<>8__locals2.CS$<>8__locals1.<>4__this.ApplyOutcome(base.<CreateGraph>g__Progress|3(), CS$<>8__locals2.CS$<>8__locals1.toils, true, true, false);
					});
				}
				transition2.AddPreAction(new TransitionAction_Custom(action));
				transition.AddPreAction(new TransitionAction_Custom(delegate()
				{
					RitualStageAction interruptedAction = CS$<>8__locals2.stage.interruptedAction;
					if (interruptedAction == null)
					{
						return;
					}
					interruptedAction.Apply(CS$<>8__locals2.CS$<>8__locals1.<>4__this);
				}));
				stateGraph.AddTransition(transition, false);
				Transition transition3 = new Transition(lordToil_Ritual2, lordToil_End, false, true);
				transition3.AddTrigger(new Trigger_Signal(this.CancelSignal));
				Transition transition4 = transition3;
				Action action2;
				if ((action2 = CS$<>8__locals2.CS$<>8__locals1.<>9__6) == null)
				{
					action2 = (CS$<>8__locals2.CS$<>8__locals1.<>9__6 = delegate()
					{
						CS$<>8__locals2.CS$<>8__locals1.<>4__this.ApplyOutcome(0f, CS$<>8__locals2.CS$<>8__locals1.toils, false, true, true);
					});
				}
				transition4.AddPreAction(new TransitionAction_Custom(action2));
				transition3.AddPreAction(new TransitionAction_Custom(delegate()
				{
					RitualStageAction interruptedAction = CS$<>8__locals2.stage.interruptedAction;
					if (interruptedAction == null)
					{
						return;
					}
					interruptedAction.Apply(CS$<>8__locals2.CS$<>8__locals1.<>4__this);
				}));
				stateGraph.AddTransition(transition3, false);
				if (!CS$<>8__locals2.stage.failTriggers.NullOrEmpty<StageFailTrigger>())
				{
					using (List<StageFailTrigger>.Enumerator enumerator3 = CS$<>8__locals2.stage.failTriggers.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							LordJob_Ritual.<>c__DisplayClass64_3 CS$<>8__locals3 = new LordJob_Ritual.<>c__DisplayClass64_3();
							CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals2;
							CS$<>8__locals3.f = enumerator3.Current;
							int i1 = i;
							Transition transition5 = new Transition(lordToil_Ritual2, lordToil_End, false, true);
							transition5.AddTrigger(new Trigger_TickCondition(() => CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.ticksPassed > CS$<>8__locals3.f.allowanceTicks && CS$<>8__locals3.f.Failed(CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this, CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.selectedTarget, CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.SecondFocusForStage(i1, null)), 1));
							transition5.AddPreAction(new TransitionAction_Message("RitualCalledOff".Translate(this.ritual.Label) + " " + "Reason".Translate() + ": " + CS$<>8__locals3.f.Reason(this, this.selectedTarget), MessageTypeDefOf.NegativeEvent, this.selectedTarget, null, 1f));
							Transition transition6 = transition5;
							Action action3;
							if ((action3 = CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>9__14) == null)
							{
								action3 = (CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>9__14 = delegate()
								{
									CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.ApplyOutcome(base.<CreateGraph>g__Progress|3(), CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.toils, true, false, false);
								});
							}
							transition6.AddPreAction(new TransitionAction_Custom(action3));
							stateGraph.AddTransition(transition5, false);
						}
					}
				}
				List<Trigger> list = new List<Trigger>();
				foreach (StageEndTrigger stageEndTrigger in CS$<>8__locals2.stage.endTriggers)
				{
					list.Add(stageEndTrigger.MakeTrigger(this, this.selectedTarget, this.AllSecondFoci(i), CS$<>8__locals2.stage));
				}
				bool flag = i == this.stages.Count - 1;
				Transition transition7 = new Transition(lordToil_Ritual2, flag ? lordToil_End : CS$<>8__locals2.CS$<>8__locals1.toils[i + 1], false, true);
				foreach (Trigger trigger2 in list)
				{
					transition7.AddTrigger(trigger2);
				}
				transition7.AddPreAction(new TransitionAction_Custom(delegate()
				{
					RitualStageAction preAction = CS$<>8__locals2.stage.preAction;
					if (preAction == null)
					{
						return;
					}
					preAction.Apply(CS$<>8__locals2.CS$<>8__locals1.<>4__this);
				}));
				CS$<>8__locals2.i2 = i;
				transition7.AddPostAction(new TransitionAction_Custom(delegate()
				{
					RitualStageAction postAction = CS$<>8__locals2.stage.postAction;
					if (postAction != null)
					{
						postAction.Apply(CS$<>8__locals2.CS$<>8__locals1.<>4__this);
					}
					if (CS$<>8__locals2.i2 == CS$<>8__locals2.CS$<>8__locals1.<>4__this.lastEssentialStageIndex)
					{
						CS$<>8__locals2.CS$<>8__locals1.<>4__this.lastEssentialStageEndedTick = CS$<>8__locals2.CS$<>8__locals1.<>4__this.ticksPassed;
					}
					CS$<>8__locals2.CS$<>8__locals1.<>4__this.stageIndex = CS$<>8__locals2.CS$<>8__locals1.<>4__this.stageIndex + 1;
				}));
				if (flag)
				{
					Transition transition8 = transition7;
					Action action4;
					if ((action4 = CS$<>8__locals2.CS$<>8__locals1.<>9__10) == null)
					{
						action4 = (CS$<>8__locals2.CS$<>8__locals1.<>9__10 = delegate()
						{
							CS$<>8__locals2.CS$<>8__locals1.<>4__this.ApplyOutcome(1f, CS$<>8__locals2.CS$<>8__locals1.toils, true, true, false);
						});
					}
					transition8.AddPreAction(new TransitionAction_Custom(action4));
				}
				stateGraph.AddTransition(transition7, false);
			}
			return stateGraph;
		}

		// Token: 0x060039D6 RID: 14806 RVA: 0x00144298 File Offset: 0x00142498
		protected virtual IEnumerable<Trigger> CallOffTriggers()
		{
			yield return new Trigger_TickCondition(new Func<bool>(this.ShouldBeCalledOff), 1);
			yield return new Trigger_PawnKilled(this.pawnsDeathIgnored);
			yield return new Trigger_Signal(this.CallOffSignal);
			if (!this.ritual.behavior.def.cancellationTriggers.NullOrEmpty<RitualCancellationTrigger>())
			{
				foreach (RitualCancellationTrigger ritualCancellationTrigger in this.ritual.behavior.def.cancellationTriggers)
				{
					foreach (Trigger trigger in ritualCancellationTrigger.CancellationTriggers(this.assignments))
					{
						yield return trigger;
					}
					IEnumerator<Trigger> enumerator2 = null;
				}
				List<RitualCancellationTrigger>.Enumerator enumerator = default(List<RitualCancellationTrigger>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x060039D7 RID: 14807 RVA: 0x001442A8 File Offset: 0x001424A8
		public override string GetReport(Pawn pawn)
		{
			return "LordReportAttending".Translate() + " " + ((this.ritual != null) ? this.ritual.Label : this.gatheringDef.label) + " " + this.TimeLeftPostfix;
		}

		// Token: 0x060039D8 RID: 14808 RVA: 0x00144308 File Offset: 0x00142508
		protected virtual LordToil_Ritual MakeToil(RitualStage stage)
		{
			return new LordToil_Ritual(this.spot, this, stage, this.organizer);
		}

		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x060039D9 RID: 14809 RVA: 0x0014431D File Offset: 0x0014251D
		public virtual IEnumerable<Pawn> PawnsToCountTowardsPresence
		{
			get
			{
				return this.lord.ownedPawns;
			}
		}

		// Token: 0x060039DA RID: 14810 RVA: 0x0014432C File Offset: 0x0014252C
		protected virtual void ApplyOutcome(float progress, List<LordToil_Ritual> toils, bool showFinishedMessage = true, bool showFailedMessage = true, bool cancelled = false)
		{
			if (this.ticksPassedWithProgress >= (float)this.MinTicksToFinish && (this.lastEssentialStageIndex == -1 || this.lastEssentialStageEndedTick != -1) && !cancelled)
			{
				this.totalPresenceTmp.Clear();
				foreach (LordToil_Ritual lordToil_Ritual in toils)
				{
					foreach (KeyValuePair<Pawn, int> keyValuePair in lordToil_Ritual.Data.presentForTicks)
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
				}
				float tickScale = this.ticksPassedWithProgress / (float)this.ticksPassed;
				this.totalPresenceTmp.RemoveAll((KeyValuePair<Pawn, int> tp) => (float)(this.durationTicks * tp.Value) < tickScale / 2f);
				if (this.totalPresenceTmp.Count > 0)
				{
					this.AddRelicInRoomThought();
					try
					{
						this.ritual.outcomeEffect.Apply(progress, this.totalPresenceTmp, this);
					}
					catch (Exception arg)
					{
						Log.Error("Error while applying ritual outcome effect: " + arg);
					}
					if (this.obligation != null)
					{
						this.ritual.activeObligations.Remove(this.obligation);
					}
					if (showFinishedMessage)
					{
						Messages.Message("RitualFinished".Translate(this.ritual.Label), new TargetInfo(this.spot, base.Map, false), MessageTypeDefOf.SilentInput, true);
					}
				}
				else
				{
					Messages.Message("RitualNobodyAttended".Translate(this.ritual.Label), new TargetInfo(this.spot, base.Map, false), MessageTypeDefOf.NegativeEvent, true);
				}
				this.totalPresenceTmp.Clear();
				if (this.Ritual != null && this.Ritual.ideo != null)
				{
					using (List<Precept>.Enumerator enumerator3 = this.Ritual.ideo.PreceptsListForReading.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							Precept_Ritual precept_Ritual;
							if ((precept_Ritual = (enumerator3.Current as Precept_Ritual)) != null && !precept_Ritual.obligationTriggers.NullOrEmpty<RitualObligationTrigger>())
							{
								foreach (RitualObligationTrigger ritualObligationTrigger in precept_Ritual.obligationTriggers)
								{
									ritualObligationTrigger.Notify_RitualExecuted(this);
								}
							}
						}
					}
				}
				this.ritual.lastFinishedTick = GenTicks.TicksGame;
			}
			else
			{
				if (showFailedMessage)
				{
					Messages.Message("RitualCalledOff".Translate(this.ritual.Label).CapitalizeFirst(), new TargetInfo(this.spot, base.Map, false), MessageTypeDefOf.NegativeEvent, true);
				}
				try
				{
					if (this.ritual.outcomeEffect.ApplyOnFailure)
					{
						this.ritual.outcomeEffect.Apply(progress, this.totalPresenceTmp, this);
					}
				}
				catch (Exception arg2)
				{
					Log.Error("Error while applying ritual outcome effect: " + arg2);
				}
			}
			base.Map.lordManager.RemoveLord(this.lord);
		}

		// Token: 0x060039DB RID: 14811 RVA: 0x00144724 File Offset: 0x00142924
		private void AddRelicInRoomThought()
		{
			if (!ModsConfig.IdeologyActive)
			{
				return;
			}
			Room room = this.selectedTarget.Cell.GetRoom(this.selectedTarget.Map);
			if (room != null && !room.TouchesMapEdge)
			{
				foreach (Thing thing in room.ContainedThings(ThingDefOf.Reliquary))
				{
					CompRelicContainer compRelicContainer = thing.TryGetComp<CompRelicContainer>();
					if (compRelicContainer != null && compRelicContainer.ContainedThing != null)
					{
						using (Dictionary<Pawn, int>.Enumerator enumerator2 = this.totalPresenceTmp.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								KeyValuePair<Pawn, int> keyValuePair = enumerator2.Current;
								Thought_RelicAtRitual thought_RelicAtRitual = (Thought_RelicAtRitual)ThoughtMaker.MakeThought(ThoughtDefOf.RelicAtRitual);
								thought_RelicAtRitual.relicName = Find.ActiveLanguageWorker.WithDefiniteArticle(compRelicContainer.ContainedThing.Label, Gender.None, false, false);
								keyValuePair.Key.needs.mood.thoughts.memories.TryGainMemory(thought_RelicAtRitual, null);
							}
							break;
						}
					}
				}
			}
		}

		// Token: 0x060039DC RID: 14812 RVA: 0x00144850 File Offset: 0x00142A50
		protected override LordToil CreateGatheringToil(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef)
		{
			return this.MakeToil((!this.stages.NullOrEmpty<RitualStage>()) ? this.stages[0] : null);
		}

		// Token: 0x060039DD RID: 14813 RVA: 0x00144874 File Offset: 0x00142A74
		protected override bool ShouldBeCalledOff()
		{
			if (this.lord.ownedPawns.Count == 0)
			{
				return true;
			}
			if (this.selectedTarget.ThingDestroyed)
			{
				return true;
			}
			if (this.organizer != null && this.organizer.Downed)
			{
				return true;
			}
			foreach (RitualRole ritualRole in this.assignments.AllRolesForReading)
			{
				if (ritualRole.required)
				{
					Pawn pawn = this.assignments.FirstAssignedPawn(ritualRole);
					if (pawn != null && !this.lord.ownedPawns.Contains(pawn) && this.ShouldCallOffBecausePawnNoLongerOwned(pawn))
					{
						return true;
					}
				}
			}
			foreach (Pawn pawn2 in this.assignments.ExtraRequiredPawnsForReading)
			{
				if (!this.lord.ownedPawns.Contains(pawn2) && this.ShouldCallOffBecausePawnNoLongerOwned(pawn2))
				{
					return true;
				}
				IThingHolder thingHolder;
				if (pawn2.Corpse == null || pawn2.Corpse.ParentHolder == null)
				{
					IThingHolder carriedBy = pawn2.CarriedBy;
					thingHolder = carriedBy;
				}
				else
				{
					thingHolder = pawn2.Corpse.ParentHolder.ParentHolder;
				}
				Pawn pawn3 = thingHolder as Pawn;
				if (pawn3 != null && pawn3.GetLord() != this.lord)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060039DE RID: 14814 RVA: 0x001449F8 File Offset: 0x00142BF8
		protected virtual bool ShouldCallOffBecausePawnNoLongerOwned(Pawn p)
		{
			return !this.pawnsDeathIgnored.Contains(p);
		}

		// Token: 0x060039DF RID: 14815 RVA: 0x00144A0C File Offset: 0x00142C0C
		public bool RoleFilled(string roleId)
		{
			return !this.ritual.behavior.def.roles.NullOrEmpty<RitualRole>() && this.ritual.behavior.def.roles.FirstOrDefault((RitualRole r) => r.id == roleId) != null && this.assignments.FirstAssignedPawn(roleId) != null;
		}

		// Token: 0x060039E0 RID: 14816 RVA: 0x00144A82 File Offset: 0x00142C82
		public RitualRole RoleFor(Pawn p, bool includeForced = false)
		{
			RitualRoleAssignments ritualRoleAssignments = this.assignments;
			if (ritualRoleAssignments == null)
			{
				return null;
			}
			return ritualRoleAssignments.RoleForPawn(p, includeForced);
		}

		// Token: 0x060039E1 RID: 14817 RVA: 0x00144A97 File Offset: 0x00142C97
		public Pawn PawnWithRole(string roleId)
		{
			return this.assignments.FirstAssignedPawn(roleId);
		}

		// Token: 0x060039E2 RID: 14818 RVA: 0x00144AA8 File Offset: 0x00142CA8
		public RitualRole GetRole(string id)
		{
			if (this.ritual.behavior.def.roles.NullOrEmpty<RitualRole>())
			{
				return null;
			}
			return this.ritual.behavior.def.roles.FirstOrDefault((RitualRole r) => r.id == id);
		}

		// Token: 0x060039E3 RID: 14819 RVA: 0x00144B06 File Offset: 0x00142D06
		public override IEnumerable<Gizmo> GetPawnGizmos(Pawn p)
		{
			if (p != this.organizer && !this.assignments.ExtraRequiredPawnsForReading.Contains(p))
			{
				Command_Action command_Action = new Command_Action();
				command_Action.defaultLabel = "CommandLeaveRitual".Translate(this.ritual.Named("RITUAL"));
				command_Action.defaultDesc = "CommandLeaveRitualDesc".Translate(this.ritual.Named("RITUAL"));
				command_Action.icon = this.ritual.Icon;
				command_Action.defaultIconColor = this.ritual.ideo.Color;
				command_Action.action = delegate()
				{
					this.pawnsForcedToLeave.Add(p);
					this.lord.Notify_PawnLost(p, PawnLostCondition.ForcedByPlayerAction, null);
					Pawn_JobTracker jobs = p.jobs;
					if (jobs != null)
					{
						jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				};
				if (this.lord.ownedPawns.Count < 2)
				{
					command_Action.Disable("CommandLeaveLastParticipant".Translate(this.ritual.Named("RITUAL")));
				}
				command_Action.hotKey = KeyBindingDefOf.Misc5;
				yield return command_Action;
			}
			yield return this.GetCancelGizmo();
			yield break;
		}

		// Token: 0x060039E4 RID: 14820 RVA: 0x00144B20 File Offset: 0x00142D20
		public Gizmo GetCancelGizmo()
		{
			return new Command_Action
			{
				defaultLabel = "CommandCancelRitual".Translate(this.ritual.Named("RITUAL")),
				defaultDesc = "CommandCancelRitualDesc".Translate(this.ritual.Named("RITUAL")),
				icon = this.ritual.Icon,
				defaultIconColor = this.ritual.ideo.Color,
				action = delegate()
				{
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("CommandCancelRitualConfirm".Translate(this.ritual.Named("RITUAL")), delegate
					{
						Find.SignalManager.SendSignal(new Signal(this.CancelSignal));
					}, false, null));
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				},
				hotKey = KeyBindingDefOf.Misc6
			};
		}

		// Token: 0x060039E5 RID: 14821 RVA: 0x00144BC0 File Offset: 0x00142DC0
		public override float VoluntaryJoinPriorityFor(Pawn p)
		{
			if (!this.IsInvited(p))
			{
				return 0f;
			}
			RitualRole ritualRole = this.RoleFor(p, true);
			if (!GatheringsUtility.ShouldPawnKeepAttendingRitual(p, this.ritual, ritualRole != null && ritualRole.ignoreBleeding))
			{
				return 0f;
			}
			if (this.spot.IsForbidden(p))
			{
				return 0f;
			}
			if (!this.lord.ownedPawns.Contains(p) && base.IsGatheringAboutToEnd())
			{
				return 0f;
			}
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def.blocksSocialInteraction)
				{
					return 0f;
				}
			}
			return VoluntarilyJoinableLordJobJoinPriorities.SocialGathering;
		}

		// Token: 0x060039E6 RID: 14822 RVA: 0x00144C7D File Offset: 0x00142E7D
		protected override bool IsInvited(Pawn p)
		{
			return !this.pawnsForcedToLeave.Contains(p) && (this.assignments == null || this.assignments.PawnParticipating(p));
		}

		// Token: 0x060039E7 RID: 14823 RVA: 0x00144CA8 File Offset: 0x00142EA8
		public override void LordJobTick()
		{
			base.LordJobTick();
			if (this.progressBar == null)
			{
				this.progressBar = EffecterDefOf.ProgressBarAlwaysVisible.Spawn();
			}
			Building edifice = this.spot.GetEdifice(base.Map);
			TargetInfo targetInfo = (edifice != null) ? edifice : new TargetInfo(this.spot, base.Map, false);
			this.progressBar.EffectTick(targetInfo, TargetInfo.Invalid);
			MoteProgressBar mote = ((SubEffecter_ProgressBar)this.progressBar.children[0]).mote;
			if (mote != null)
			{
				mote.progress = 1f - Mathf.Clamp01((float)this.TicksLeft / (float)base.DurationTicks);
				mote.offsetZ = -0.5f;
			}
			if (this.ritual != null)
			{
				LordToil_Ritual lordToil_Ritual;
				if ((lordToil_Ritual = (this.lord.CurLordToil as LordToil_Ritual)) != null && lordToil_Ritual.stage != null)
				{
					this.ritual.outcomeEffect.Tick(this, lordToil_Ritual.stage.ProgressPerTick(this));
				}
				else
				{
					this.ritual.outcomeEffect.Tick(this, 1f);
				}
			}
			if (this.ritual != null && this.ritual.behavior != null)
			{
				this.ritual.behavior.Tick(this);
			}
			if (DebugSettings.playRitualAmbience && this.ambienceDef != null && (this.ambiencePlaying == null || this.ambiencePlaying.Ended))
			{
				this.ambiencePlaying = this.ambienceDef.TrySpawnSustainer(SoundInfo.InMap(targetInfo, MaintenanceType.PerTick));
			}
			Sustainer sustainer = this.ambiencePlaying;
			if (sustainer != null)
			{
				sustainer.Maintain();
			}
			LordToil_Ritual lordToil_Ritual2;
			if ((lordToil_Ritual2 = (this.lord.CurLordToil as LordToil_Ritual)) != null)
			{
				RitualStage stage = lordToil_Ritual2.stage;
				if (stage != null)
				{
					foreach (ActionOnTick actionOnTick in this.ritualStageOnTickActions[this.stages.IndexOf(stage)].actions)
					{
						if (actionOnTick.tick == this.stageTicks)
						{
							try
							{
								actionOnTick.Apply(this);
							}
							catch (Exception arg)
							{
								Log.Error("Error while applying ritual on-tick action: " + arg);
							}
						}
					}
				}
			}
			if (!this.initedVisualEffects)
			{
				foreach (RitualVisualEffect ritualVisualEffect in this.effectWorkers)
				{
					ritualVisualEffect.ritual = this;
					ritualVisualEffect.Setup(this, true);
				}
				foreach (RitualVisualEffect ritualVisualEffect2 in this.effectWorkersCurrentStage)
				{
					ritualVisualEffect2.ritual = this;
					ritualVisualEffect2.Setup(this, true);
				}
				this.initedVisualEffects = true;
			}
			foreach (RitualVisualEffect ritualVisualEffect3 in this.effectWorkers)
			{
				ritualVisualEffect3.Tick();
			}
			foreach (RitualVisualEffect ritualVisualEffect4 in this.effectWorkersCurrentStage)
			{
				ritualVisualEffect4.Tick();
			}
			foreach (KeyValuePair<IntVec3, Mote> keyValuePair in this.highlightedPositions)
			{
				keyValuePair.Value.Maintain();
			}
			foreach (KeyValuePair<Pawn, Mote> keyValuePair2 in this.highlightedPawns)
			{
				keyValuePair2.Value.Maintain();
			}
			this.ticksPassed++;
			this.stageTicks++;
			if (this.lastEssentialStageEndedTick != -1 && this.ticksPassed > this.lastEssentialStageEndedTick)
			{
				this.ticksSinceLastEssentialStage++;
			}
		}

		// Token: 0x060039E8 RID: 14824 RVA: 0x001450DC File Offset: 0x001432DC
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.progressBar != null)
			{
				this.progressBar.Cleanup();
				this.progressBar = null;
			}
			RitualBehaviorWorker behavior = this.ritual.behavior;
			if (behavior != null)
			{
				behavior.Cleanup(this);
			}
			this.highlightedPositions.Clear();
			this.highlightedPawns.Clear();
			foreach (RitualVisualEffect ritualVisualEffect in this.effectWorkers)
			{
				ritualVisualEffect.Cleanup();
			}
			foreach (RitualVisualEffect ritualVisualEffect2 in this.effectWorkersCurrentStage)
			{
				ritualVisualEffect2.Cleanup();
			}
		}

		// Token: 0x060039E9 RID: 14825 RVA: 0x001451B8 File Offset: 0x001433B8
		public override void PostCleanup()
		{
			RitualBehaviorWorker behavior = this.ritual.behavior;
			if (behavior == null)
			{
				return;
			}
			behavior.PostCleanup(this);
		}

		// Token: 0x060039EA RID: 14826 RVA: 0x001451D0 File Offset: 0x001433D0
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			base.Notify_PawnLost(p, condition);
			if (this.progressBar != null)
			{
				this.progressBar.Cleanup();
				this.progressBar = null;
			}
			LordToil_Ritual lordToil_Ritual;
			if ((lordToil_Ritual = (this.lord.CurLordToil as LordToil_Ritual)) != null)
			{
				RitualStage stage = lordToil_Ritual.stage;
				if (stage == null)
				{
					return;
				}
				RitualStageAction pawnLeaveAction = stage.pawnLeaveAction;
				if (pawnLeaveAction == null)
				{
					return;
				}
				pawnLeaveAction.ApplyToPawn(this, p);
			}
		}

		// Token: 0x060039EB RID: 14827 RVA: 0x00145230 File Offset: 0x00143430
		public override void Notify_InMentalState(Pawn pawn, MentalStateDef stateDef)
		{
			base.Notify_InMentalState(pawn, stateDef);
			this.lord.Notify_PawnLost(pawn, PawnLostCondition.InMentalState, null);
		}

		// Token: 0x060039EC RID: 14828 RVA: 0x0014525C File Offset: 0x0014345C
		public void AddTagForPawn(Pawn p, string tag)
		{
			if (this.perPawnTags.ContainsKey(p))
			{
				if (!this.perPawnTags[p].tags.Contains(tag))
				{
					this.perPawnTags[p].tags.Add(tag);
					return;
				}
			}
			else
			{
				this.perPawnTags[p] = new PawnTags
				{
					tags = new List<string>
					{
						tag
					}
				};
			}
		}

		// Token: 0x060039ED RID: 14829 RVA: 0x001452CB File Offset: 0x001434CB
		public bool PawnTagSet(Pawn p, string tag)
		{
			return this.perPawnTags.ContainsKey(p) && this.perPawnTags[p].tags.Contains(tag);
		}

		// Token: 0x060039EE RID: 14830 RVA: 0x001452F4 File Offset: 0x001434F4
		public TargetInfo SecondFocusForStage(int index, Pawn forPawn = null)
		{
			if (forPawn != null)
			{
				foreach (RitualStagePawnSecondFocus ritualStagePawnSecondFocus in this.stagePawnSecondFocus)
				{
					if (ritualStagePawnSecondFocus.stageIndex == index && ritualStagePawnSecondFocus.pawn == forPawn)
					{
						return ritualStagePawnSecondFocus.target;
					}
				}
			}
			return this.stageSecondFocus[index];
		}

		// Token: 0x060039EF RID: 14831 RVA: 0x0014536C File Offset: 0x0014356C
		public IEnumerable<TargetInfo> AllSecondFoci(int index)
		{
			yield return this.stageSecondFocus[index];
			foreach (RitualStagePawnSecondFocus ritualStagePawnSecondFocus in this.stagePawnSecondFocus)
			{
				if (ritualStagePawnSecondFocus.stageIndex == index)
				{
					yield return ritualStagePawnSecondFocus.target;
				}
			}
			List<RitualStagePawnSecondFocus>.Enumerator enumerator = default(List<RitualStagePawnSecondFocus>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060039F0 RID: 14832 RVA: 0x00145383 File Offset: 0x00143583
		public TargetInfo SecondFocusForStage(RitualStage stage, Pawn forPawn = null)
		{
			return this.SecondFocusForStage(this.stages.IndexOf(stage), forPawn);
		}

		// Token: 0x060039F1 RID: 14833 RVA: 0x00145398 File Offset: 0x00143598
		public IEnumerable<TargetInfo> AllSecondFoci(RitualStage stage)
		{
			return this.AllSecondFoci(this.stages.IndexOf(stage));
		}

		// Token: 0x060039F2 RID: 14834 RVA: 0x001453AC File Offset: 0x001435AC
		public PawnStagePosition PawnPositionForStage(Pawn pawn, RitualStage stage)
		{
			int num = this.stages.IndexOf(stage);
			if (num == -1 || this.ritualStagePositions.Count <= num)
			{
				Log.Error("Invalid stage id for ritual stage position: " + num);
				return null;
			}
			Dictionary<Pawn, PawnStagePosition> positions = this.ritualStagePositions[num].positions;
			if (!positions.ContainsKey(pawn))
			{
				return null;
			}
			return positions[pawn];
		}

		// Token: 0x060039F3 RID: 14835 RVA: 0x00145414 File Offset: 0x00143614
		private void AddPositionHighlight(IntVec3 pos)
		{
			Mote mote = MoteMaker.MakeStaticMote(pos.ToVector3Shifted(), base.Map, ThingDefOf.Mote_RolePositionHighlight, 1f);
			this.highlightedPositions.Add(pos, mote);
			mote.Maintain();
		}

		// Token: 0x060039F4 RID: 14836 RVA: 0x00145454 File Offset: 0x00143654
		private void AddPawnHighlight(Pawn pawn)
		{
			Mote mote = MoteMaker.MakeAttachedOverlay(pawn, ThingDefOf.Mote_RolePawnHighlight, new Vector3(0f, 0f, -0.4f), 1f, -1f);
			this.highlightedPawns.Add(pawn, mote);
			mote.Maintain();
		}

		// Token: 0x060039F5 RID: 14837 RVA: 0x0014549E File Offset: 0x0014369E
		public bool IsParticipating(Pawn p)
		{
			return this.lord.ownedPawns.Contains(p) && GatheringsUtility.InGatheringArea(p.Position, this.Spot, p.MapHeld);
		}

		// Token: 0x060039F6 RID: 14838 RVA: 0x001454CC File Offset: 0x001436CC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Precept_Ritual>(ref this.ritual, "ritual", false);
			Scribe_References.Look<RitualObligation>(ref this.obligation, "obligation", false);
			Scribe_Values.Look<int>(ref this.durationTicks, "durationTicks", 0, false);
			Scribe_Values.Look<int>(ref this.ticksPassed, "ticksPassed", 0, false);
			Scribe_Values.Look<float>(ref this.ticksPassedWithProgress, "ticksPassedWithProgress", 0f, false);
			Scribe_Values.Look<int>(ref this.stageTicks, "stageTicks", 0, false);
			Scribe_Values.Look<int>(ref this.lastEssentialStageIndex, "lastEssentialStageIndex", 0, false);
			Scribe_Values.Look<int>(ref this.lastEssentialStageEndedTick, "lastEssentialStageEndedTick", 0, false);
			Scribe_Values.Look<int>(ref this.ticksSinceLastEssentialStage, "ticksSinceLastEssentialStage", 0, false);
			Scribe_Values.Look<bool>(ref this.repeatPenalty, "repeatPenalty", false, false);
			Scribe_Values.Look<int>(ref this.stageIndex, "stageIndex", 0, false);
			Scribe_Defs.Look<SoundDef>(ref this.ambienceDef, "ambienceDef");
			Scribe_TargetInfo.Look(ref this.selectedTarget, "selectedTarget");
			Scribe_Collections.Look<Pawn>(ref this.pawnsForcedToLeave, "pawnsForcedToLeave", LookMode.Reference);
			Scribe_Collections.Look<Thing>(ref this.usedThings, "usedThings", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawnsDeathIgnored, true, "pawnsDeathIgnored", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Pawn, PawnTags>(ref this.perPawnTags, "perPawnTags", LookMode.Reference, LookMode.Deep, ref this.tmpTagPawns, ref this.tmpTags);
			Scribe_Collections.Look<RitualStage>(ref this.stages, "stages", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<TargetInfo>(ref this.stageSecondFocus, "stageSecondFocus", LookMode.TargetInfo, Array.Empty<object>());
			Scribe_Collections.Look<RitualStagePawnSecondFocus>(ref this.stagePawnSecondFocus, "stagePawnSecondFocus", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<RitualStagePositions>(ref this.ritualStagePositions, "ritualStagePositions", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<RitualStageOnTickActions>(ref this.ritualStageOnTickActions, "ritualStageOnTickActions", LookMode.Deep, Array.Empty<object>());
			Scribe_Deep.Look<RitualRoleAssignments>(ref this.assignments, "assignments", Array.Empty<object>());
			Scribe_Collections.Look<RitualVisualEffect>(ref this.effectWorkers, "effectWorkers", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<RitualVisualEffect>(ref this.effectWorkersCurrentStage, "effectWorkersCurrentStage", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.ignoreDurationToFinish, "ignoreDurationToFinish", false, false);
		}

		// Token: 0x04001FB7 RID: 8119
		protected Precept_Ritual ritual;

		// Token: 0x04001FB8 RID: 8120
		public RitualObligation obligation;

		// Token: 0x04001FB9 RID: 8121
		public TargetInfo selectedTarget;

		// Token: 0x04001FBA RID: 8122
		public RitualRoleAssignments assignments;

		// Token: 0x04001FBB RID: 8123
		private List<RitualStage> stages;

		// Token: 0x04001FBC RID: 8124
		public List<Pawn> pawnsDeathIgnored = new List<Pawn>();

		// Token: 0x04001FBD RID: 8125
		protected HashSet<Pawn> pawnsForcedToLeave = new HashSet<Pawn>();

		// Token: 0x04001FBE RID: 8126
		public List<Thing> usedThings = new List<Thing>();

		// Token: 0x04001FBF RID: 8127
		public Dictionary<Pawn, PawnTags> perPawnTags = new Dictionary<Pawn, PawnTags>();

		// Token: 0x04001FC0 RID: 8128
		protected SoundDef ambienceDef;

		// Token: 0x04001FC1 RID: 8129
		protected int stageTicks;

		// Token: 0x04001FC2 RID: 8130
		protected int ticksPassed;

		// Token: 0x04001FC3 RID: 8131
		protected int stageIndex;

		// Token: 0x04001FC4 RID: 8132
		protected float ticksPassedWithProgress;

		// Token: 0x04001FC5 RID: 8133
		public bool repeatPenalty;

		// Token: 0x04001FC6 RID: 8134
		private List<TargetInfo> stageSecondFocus;

		// Token: 0x04001FC7 RID: 8135
		private List<RitualStagePawnSecondFocus> stagePawnSecondFocus;

		// Token: 0x04001FC8 RID: 8136
		private List<RitualStagePositions> ritualStagePositions;

		// Token: 0x04001FC9 RID: 8137
		private List<RitualStageOnTickActions> ritualStageOnTickActions;

		// Token: 0x04001FCA RID: 8138
		private List<RitualVisualEffect> effectWorkers = new List<RitualVisualEffect>();

		// Token: 0x04001FCB RID: 8139
		private List<RitualVisualEffect> effectWorkersCurrentStage = new List<RitualVisualEffect>();

		// Token: 0x04001FCC RID: 8140
		private int lastEssentialStageIndex = -1;

		// Token: 0x04001FCD RID: 8141
		private int lastEssentialStageEndedTick = -1;

		// Token: 0x04001FCE RID: 8142
		private int ticksSinceLastEssentialStage;

		// Token: 0x04001FCF RID: 8143
		private bool ignoreDurationToFinish;

		// Token: 0x04001FD0 RID: 8144
		protected Effecter progressBar;

		// Token: 0x04001FD1 RID: 8145
		protected Sustainer ambiencePlaying;

		// Token: 0x04001FD2 RID: 8146
		private bool initedVisualEffects;

		// Token: 0x04001FD3 RID: 8147
		private Room roomCached;

		// Token: 0x04001FD4 RID: 8148
		private IntVec2 roomBoundsCached = IntVec2.Invalid;

		// Token: 0x04001FD5 RID: 8149
		private Dictionary<IntVec3, Mote> highlightedPositions = new Dictionary<IntVec3, Mote>();

		// Token: 0x04001FD6 RID: 8150
		private Dictionary<Pawn, Mote> highlightedPawns = new Dictionary<Pawn, Mote>();

		// Token: 0x04001FD7 RID: 8151
		public const string RitualStartedSignal = "RitualStarted";

		// Token: 0x04001FD8 RID: 8152
		public static readonly IntVec2 DefaultRitualVfxScale = new IntVec2(28, 28);

		// Token: 0x04001FD9 RID: 8153
		private Dictionary<Pawn, int> totalPresenceTmp = new Dictionary<Pawn, int>();

		// Token: 0x04001FDA RID: 8154
		private List<Pawn> tmpTagPawns;

		// Token: 0x04001FDB RID: 8155
		private List<Pawn> tmpSubRolePawns;

		// Token: 0x04001FDC RID: 8156
		private List<Pawn> tmpForcedRolePawns;

		// Token: 0x04001FDD RID: 8157
		private List<PawnTags> tmpTags;

		// Token: 0x04001FDE RID: 8158
		private List<string> tmpForcedRoleIds;

		// Token: 0x04001FDF RID: 8159
		private List<string> tmpSubRoleIds;
	}
}
