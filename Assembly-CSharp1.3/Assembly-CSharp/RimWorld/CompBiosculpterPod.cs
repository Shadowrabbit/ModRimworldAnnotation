using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001107 RID: 4359
	[StaticConstructorOnStartup]
	public class CompBiosculpterPod : ThingComp, IThingHolder, ISuspendableThingHolder, IThingHolderWithDrawnPawn, IStoreSettingsParent
	{
		// Token: 0x170011E0 RID: 4576
		// (get) Token: 0x060068A4 RID: 26788 RVA: 0x002350B7 File Offset: 0x002332B7
		public CompProperties_BiosculpterPod Props
		{
			get
			{
				return this.props as CompProperties_BiosculpterPod;
			}
		}

		// Token: 0x170011E1 RID: 4577
		// (get) Token: 0x060068A5 RID: 26789 RVA: 0x000126F5 File Offset: 0x000108F5
		public bool IsContentsSuspended
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170011E2 RID: 4578
		// (get) Token: 0x060068A6 RID: 26790 RVA: 0x002350C4 File Offset: 0x002332C4
		public BiosculpterPodState State
		{
			get
			{
				if (this.currentCycleKey == null)
				{
					return BiosculpterPodState.Inactive;
				}
				if (this.Occupant != null)
				{
					return BiosculpterPodState.Occupied;
				}
				if (this.nutritionLoaded)
				{
					return BiosculpterPodState.WaitingForOccupant;
				}
				return BiosculpterPodState.LoadingNutrition;
			}
		}

		// Token: 0x170011E3 RID: 4579
		// (get) Token: 0x060068A7 RID: 26791 RVA: 0x002350E5 File Offset: 0x002332E5
		public Pawn Occupant
		{
			get
			{
				if (this.currentCycleKey == null)
				{
					return null;
				}
				if (this.innerContainer.Count != 1)
				{
					return null;
				}
				return this.innerContainer[0] as Pawn;
			}
		}

		// Token: 0x170011E4 RID: 4580
		// (get) Token: 0x060068A8 RID: 26792 RVA: 0x00235114 File Offset: 0x00233314
		public float NutritionStored
		{
			get
			{
				float num = 0f;
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					Thing thing = this.innerContainer[i];
					num += (float)thing.stackCount * thing.GetStatValue(StatDefOf.Nutrition, true);
				}
				return num;
			}
		}

		// Token: 0x170011E5 RID: 4581
		// (get) Token: 0x060068A9 RID: 26793 RVA: 0x00235164 File Offset: 0x00233364
		public float RequiredNutritionRemaining
		{
			get
			{
				CompBiosculpterPod_Cycle currentCycle = this.CurrentCycle;
				if (currentCycle == null)
				{
					return 0f;
				}
				return Mathf.Max(0f, currentCycle.Props.nutritionRequired - this.NutritionStored);
			}
		}

		// Token: 0x170011E6 RID: 4582
		// (get) Token: 0x060068AA RID: 26794 RVA: 0x002351A0 File Offset: 0x002333A0
		public CompBiosculpterPod_Cycle CurrentCycle
		{
			get
			{
				if (this.currentCycleKey == null)
				{
					return null;
				}
				foreach (CompBiosculpterPod_Cycle compBiosculpterPod_Cycle in this.AvailableCycles)
				{
					if (compBiosculpterPod_Cycle.Props.key == this.currentCycleKey)
					{
						return compBiosculpterPod_Cycle;
					}
				}
				return null;
			}
		}

		// Token: 0x170011E7 RID: 4583
		// (get) Token: 0x060068AB RID: 26795 RVA: 0x00235218 File Offset: 0x00233418
		public List<CompBiosculpterPod_Cycle> AvailableCycles
		{
			get
			{
				if (this.cachedAvailableCycles == null)
				{
					this.cachedAvailableCycles = new List<CompBiosculpterPod_Cycle>();
					this.cachedAvailableCycles.AddRange(this.parent.AllComps.OfType<CompBiosculpterPod_Cycle>());
				}
				return this.cachedAvailableCycles;
			}
		}

		// Token: 0x170011E8 RID: 4584
		// (get) Token: 0x060068AC RID: 26796 RVA: 0x00235250 File Offset: 0x00233450
		public float CycleSpeedFactor
		{
			get
			{
				float num = 1f;
				FactionIdeosTracker ideos = Faction.OfPlayer.ideos;
				Ideo ideo = (ideos != null) ? ideos.PrimaryIdeo : null;
				if (ideo != null)
				{
					foreach (Precept precept in ideo.PreceptsListForReading)
					{
						num *= precept.def.biosculpterPodCycleSpeedFactor;
					}
				}
				float num2 = num;
				Room room = this.parent.GetRoom(RegionType.Set_All);
				num = num2 + ((room != null) ? room.GetStat(RoomStatDefOf.BiosculpterPodSpeedFactorOffset) : RoomStatDefOf.BiosculpterPodSpeedFactorOffset.roomlessScore);
				num = Mathf.Max(0.1f, num);
				return num;
			}
		}

		// Token: 0x170011E9 RID: 4585
		// (get) Token: 0x060068AD RID: 26797 RVA: 0x00235304 File Offset: 0x00233504
		public bool PowerOn
		{
			get
			{
				return this.parent.TryGetComp<CompPowerTrader>().PowerOn;
			}
		}

		// Token: 0x170011EA RID: 4586
		// (get) Token: 0x060068AE RID: 26798 RVA: 0x00235316 File Offset: 0x00233516
		public float HeldPawnDrawPos_Y
		{
			get
			{
				return this.parent.DrawPos.y - 0.04054054f;
			}
		}

		// Token: 0x170011EB RID: 4587
		// (get) Token: 0x060068AF RID: 26799 RVA: 0x00235330 File Offset: 0x00233530
		public float HeldPawnBodyAngle
		{
			get
			{
				return this.parent.Rotation.Opposite.AsAngle;
			}
		}

		// Token: 0x170011EC RID: 4588
		// (get) Token: 0x060068B0 RID: 26800 RVA: 0x000126F5 File Offset: 0x000108F5
		public PawnPosture HeldPawnPosture
		{
			get
			{
				return PawnPosture.LayingOnGroundFaceUp;
			}
		}

		// Token: 0x060068B1 RID: 26801 RVA: 0x00235358 File Offset: 0x00233558
		public CompBiosculpterPod()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x060068B2 RID: 26802 RVA: 0x0023536C File Offset: 0x0023356C
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.allowedNutritionSettings = new StorageSettings(this);
			if (this.parent.def.building.defaultStorageSettings != null)
			{
				this.allowedNutritionSettings.CopyFrom(this.parent.def.building.defaultStorageSettings);
			}
		}

		// Token: 0x060068B3 RID: 26803 RVA: 0x002353C4 File Offset: 0x002335C4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<string>(ref this.currentCycleKey, "currentCycleKey", null, false);
			Scribe_Values.Look<float>(ref this.currentCycleTicksRemaining, "currentCycleTicksRemaining", 0f, false);
			Scribe_Values.Look<int>(ref this.currentCyclePowerCutTicks, "currentCyclePowerCutTicks", 0, false);
			Scribe_Values.Look<bool>(ref this.nutritionLoaded, "nutritionLoaded", false, false);
			Scribe_References.Look<Pawn>(ref this.biotunedTo, "biotunedTo", false);
			Scribe_Values.Look<int>(ref this.biotunedCountdownTicks, "biotunedCountdownTicks", 0, false);
			Scribe_Deep.Look<StorageSettings>(ref this.allowedNutritionSettings, "allowedNutritionSettings", Array.Empty<object>());
			if (this.allowedNutritionSettings == null)
			{
				this.allowedNutritionSettings = new StorageSettings(this);
				if (this.parent.def.building.defaultStorageSettings != null)
				{
					this.allowedNutritionSettings.CopyFrom(this.parent.def.building.defaultStorageSettings);
				}
			}
		}

		// Token: 0x060068B4 RID: 26804 RVA: 0x002354C0 File Offset: 0x002336C0
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			if (mode == DestroyMode.Deconstruct || mode == DestroyMode.KillFinalize)
			{
				this.EjectContents(true, false, previousMap);
			}
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			base.PostDestroy(mode, previousMap);
		}

		// Token: 0x060068B5 RID: 26805 RVA: 0x002354E8 File Offset: 0x002336E8
		public override void PostDeSpawn(Map map)
		{
			Effecter effecter = this.progressBarEffecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.progressBarEffecter = null;
			Effecter effecter2 = this.operatingEffecter;
			if (effecter2 != null)
			{
				effecter2.Cleanup();
			}
			this.operatingEffecter = null;
			Effecter effecter3 = this.readyEffecter;
			if (effecter3 != null)
			{
				effecter3.Cleanup();
			}
			this.readyEffecter = null;
			this.currentCycleKey = null;
			this.nutritionLoaded = false;
		}

		// Token: 0x060068B6 RID: 26806 RVA: 0x0023554C File Offset: 0x0023374C
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			BiosculpterPodState state = this.State;
			if (this.parent.Spawned)
			{
				CompBiosculpterPod_Cycle currentCycle = this.CurrentCycle;
				if (currentCycle != null)
				{
					stringBuilder.AppendLineIfNotEmpty().Append("BiosculpterPodCycleLabel".Translate()).Append(": ").Append(currentCycle.Props.LabelCap);
				}
				if (state == BiosculpterPodState.LoadingNutrition)
				{
					stringBuilder.Append(" (").Append("BiosculpterPodCycleLabelLoading".Translate()).Append(")");
				}
				if (state == BiosculpterPodState.WaitingForOccupant)
				{
					stringBuilder.Append(" (").Append("BiosculpterPodCycleLabelReady".Translate()).Append(")");
				}
				if (state == BiosculpterPodState.LoadingNutrition)
				{
					stringBuilder.AppendLineIfNotEmpty().Append("Nutrition".Translate()).Append(": ").Append(this.NutritionStored.ToStringByStyle(ToStringStyle.FloatMaxOne, ToStringNumberSense.Absolute)).Append(" / ").Append(this.CurrentCycle.Props.nutritionRequired);
				}
				if (state == BiosculpterPodState.Occupied)
				{
					float num = this.currentCycleTicksRemaining / this.CycleSpeedFactor;
					stringBuilder.AppendLineIfNotEmpty().Append("Contains".Translate()).Append(": ").Append(this.Occupant.NameShortColored.Resolve());
					stringBuilder.AppendLine().Append("BiosculpterCycleTimeRemaining".Translate()).Append(": ").Append(((int)num).ToStringTicksToPeriod(true, false, true, true).Colorize(ColoredText.DateTimeColor));
				}
			}
			if (this.biotunedTo != null && state != BiosculpterPodState.Occupied)
			{
				stringBuilder.AppendLineIfNotEmpty().Append("BiosculpterBiotunedTo".Translate()).Append(": ").Append(this.biotunedTo.Label).Append(" (").Append(this.biotunedCountdownTicks.ToStringTicksToPeriod(true, false, true, true)).Append(")");
			}
			if (stringBuilder.Length <= 0)
			{
				return null;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060068B7 RID: 26807 RVA: 0x00235777 File Offset: 0x00233977
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			BiosculpterPodState state = this.State;
			if (state == BiosculpterPodState.Inactive)
			{
				using (List<CompBiosculpterPod_Cycle>.Enumerator enumerator = this.AvailableCycles.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CompBiosculpterPod_Cycle cycle = enumerator.Current;
						yield return new Command_Action
						{
							defaultLabel = "BiosculpterPodCycleCommand".Translate(cycle.Props.label),
							defaultDesc = this.CycleDescription(cycle),
							icon = cycle.Props.Icon,
							action = delegate()
							{
								this.StartCycle(cycle);
							},
							activateSound = SoundDefOf.Tick_Tiny
						};
					}
				}
				List<CompBiosculpterPod_Cycle>.Enumerator enumerator = default(List<CompBiosculpterPod_Cycle>.Enumerator);
			}
			if (state == BiosculpterPodState.Occupied)
			{
				yield return new Command_Action
				{
					defaultLabel = "BiosculpterInteruptCycle".Translate(),
					defaultDesc = "BiosculpterInteruptCycleDesc".Translate(),
					icon = CompBiosculpterPod.InterruptCycleIcon,
					action = delegate()
					{
						this.EjectContents(true, true, null);
					},
					activateSound = SoundDefOf.Designate_Cancel
				};
			}
			if (state == BiosculpterPodState.LoadingNutrition || state == BiosculpterPodState.WaitingForOccupant)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandCancelLoad".Translate(),
					defaultDesc = "CommandCancelLoadDesc".Translate(),
					icon = CompBiosculpterPod.CancelLoadingIcon,
					action = delegate()
					{
						this.EjectContents(true, true, null);
					},
					activateSound = SoundDefOf.Designate_Cancel
				};
			}
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "Dev: complete cycle",
					action = delegate()
					{
						this.currentCycleTicksRemaining = 10f;
					},
					disabled = (this.State != BiosculpterPodState.Occupied)
				};
				yield return new Command_Action
				{
					defaultLabel = "Dev: advance cycle +1 day",
					action = delegate()
					{
						this.currentCycleTicksRemaining -= 60000f;
					},
					disabled = (this.State != BiosculpterPodState.Occupied)
				};
				yield return new Command_Action
				{
					defaultLabel = "Dev: complete biotune timer",
					action = delegate()
					{
						this.biotunedCountdownTicks = 10;
					},
					disabled = (this.biotunedCountdownTicks <= 0)
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x060068B8 RID: 26808 RVA: 0x00235788 File Offset: 0x00233988
		private string CycleDescription(CompBiosculpterPod_Cycle cycle)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(cycle.Props.description);
			float f = cycle.Props.durationDays / this.CycleSpeedFactor;
			stringBuilder.AppendLine().AppendLine().Append("durationDays".Translate().CapitalizeFirst()).Append(": ").Append(f.ToStringByStyle(ToStringStyle.FloatMaxOne, ToStringNumberSense.Absolute));
			stringBuilder.AppendLine().AppendLine().Append("BiosculpterLoadingStartedMessage".Translate(cycle.Props.nutritionRequired.Named("NUTRITION")));
			return stringBuilder.ToString();
		}

		// Token: 0x060068B9 RID: 26809 RVA: 0x0023583D File Offset: 0x00233A3D
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			if (this.State != BiosculpterPodState.WaitingForOccupant || !this.PowerOn)
			{
				yield break;
			}
			if (selPawn.IsQuestLodger())
			{
				yield return new FloatMenuOption("CannotUseReason".Translate("CryptosleepCasketGuestsNotAllowed".Translate()), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			else if (this.biotunedTo != null && this.biotunedTo != selPawn)
			{
				yield return new FloatMenuOption("CannotUseReason".Translate("BiosculpterBiotunedToAnother".Translate()), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			else if (!selPawn.CanReach(this.parent, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				yield return new FloatMenuOption("CannotUseNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			else
			{
				string label = "EnterBiosculpterPod".Translate();
				Action action = delegate()
				{
					Job job = JobMaker.MakeJob(JobDefOf.EnterBiosculpterPod, this.parent);
					selPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				};
				yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), selPawn, this.parent, "ReservedBy");
			}
			yield break;
		}

		// Token: 0x060068BA RID: 26810 RVA: 0x00235854 File Offset: 0x00233A54
		public bool CanAcceptNutrition(Thing thing)
		{
			return this.allowedNutritionSettings.AllowedToAccept(thing);
		}

		// Token: 0x060068BB RID: 26811 RVA: 0x00235862 File Offset: 0x00233A62
		public bool CanAccept(Pawn pawn)
		{
			return this.State == BiosculpterPodState.WaitingForOccupant && this.PowerOn && (this.biotunedTo == null || this.biotunedTo == pawn);
		}

		// Token: 0x060068BC RID: 26812 RVA: 0x0023588C File Offset: 0x00233A8C
		public void TryAcceptPawn(Pawn pawn)
		{
			if (!this.CanAccept(pawn))
			{
				return;
			}
			if (pawn.Spawned)
			{
				pawn.DeSpawn(DestroyMode.Vanish);
			}
			if (pawn.holdingOwner != null)
			{
				pawn.holdingOwner.TryTransferToContainer(pawn, this.innerContainer, true);
			}
			else
			{
				this.innerContainer.TryAdd(pawn, true);
			}
			CompBiosculpterPod_Cycle currentCycle = this.CurrentCycle;
			this.currentCycleTicksRemaining = currentCycle.Props.durationDays * 60000f;
			this.nutritionLoaded = false;
			this.biotunedTo = pawn;
			this.biotunedCountdownTicks = 3600000;
		}

		// Token: 0x060068BD RID: 26813 RVA: 0x00235918 File Offset: 0x00233B18
		public void EjectContents(bool interrupted, bool playSounds, Map destMap = null)
		{
			if (destMap == null)
			{
				destMap = this.parent.Map;
			}
			Pawn occupant = this.Occupant;
			this.currentCycleKey = null;
			this.currentCycleTicksRemaining = 0f;
			this.currentCyclePowerCutTicks = 0;
			this.nutritionLoaded = false;
			this.innerContainer.TryDropAll(this.parent.InteractionCell, destMap, ThingPlaceMode.Near, null, null, true);
			if (occupant != null)
			{
				FilthMaker.TryMakeFilth(this.parent.InteractionCell, destMap, ThingDefOf.Filth_PodSlime, new IntRange(3, 6).RandomInRange, FilthSourceFlags.None);
				if (interrupted)
				{
					Pawn_NeedsTracker needs = occupant.needs;
					if (needs != null)
					{
						needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SoakingWet, null, null);
					}
					Pawn_HealthTracker health = occupant.health;
					if (health != null)
					{
						health.AddHediff(HediffDefOf.BiosculptingSickness, null, null, null);
					}
				}
			}
			if (playSounds)
			{
				SoundDef exitSound = this.Props.exitSound;
				if (exitSound == null)
				{
					return;
				}
				exitSound.PlayOneShot(SoundInfo.InMap(new TargetInfo(this.parent.Position, this.parent.Map, false), MaintenanceType.None));
			}
		}

		// Token: 0x060068BE RID: 26814 RVA: 0x00235A28 File Offset: 0x00233C28
		private void StartCycle(CompBiosculpterPod_Cycle cycle)
		{
			this.currentCycleKey = cycle.Props.key;
			this.currentCycleTicksRemaining = 0f;
			this.currentCyclePowerCutTicks = 0;
			this.nutritionLoaded = false;
			Messages.Message("BiosculpterLoadingStartedMessage".Translate(cycle.Props.nutritionRequired.Named("NUTRITION")), this.parent, MessageTypeDefOf.SilentInput, false);
		}

		// Token: 0x060068BF RID: 26815 RVA: 0x00235AA0 File Offset: 0x00233CA0
		private void CycleCompleted()
		{
			Pawn occupant = this.Occupant;
			CompBiosculpterPod_Cycle currentCycle = this.CurrentCycle;
			currentCycle.CycleCompleted(occupant);
			this.EjectContents(false, true, null);
			if (occupant != null)
			{
				Pawn_NeedsTracker needs = occupant.needs;
				Need_Food need_Food = (needs != null) ? needs.food : null;
				if (need_Food != null)
				{
					need_Food.CurLevelPercentage = 1f;
				}
				Pawn_NeedsTracker needs2 = occupant.needs;
				Need_Rest need_Rest = (needs2 != null) ? needs2.rest : null;
				if (need_Rest != null)
				{
					need_Rest.CurLevelPercentage = 1f;
				}
				if (currentCycle.Props.gainThoughtOnCompletion != null)
				{
					Pawn_NeedsTracker needs3 = occupant.needs;
					if (needs3 != null)
					{
						Need_Mood mood = needs3.mood;
						if (mood != null)
						{
							mood.thoughts.memories.TryGainMemory(ThoughtDefOf.AgeReversalReceived, null, null);
						}
					}
				}
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.UsedBiosculpterPod, occupant.Named(HistoryEventArgsNames.Doer)), true);
			}
		}

		// Token: 0x060068C0 RID: 26816 RVA: 0x00235B70 File Offset: 0x00233D70
		public override void CompTick()
		{
			if (!ModLister.CheckIdeology("Biosculpting"))
			{
				return;
			}
			base.CompTick();
			this.innerContainer.ThingOwnerTick(true);
			if (this.State == BiosculpterPodState.LoadingNutrition && this.RequiredNutritionRemaining <= 0f)
			{
				this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
				this.nutritionLoaded = true;
			}
			if (this.State != BiosculpterPodState.WaitingForOccupant)
			{
				Effecter effecter = this.readyEffecter;
				if (effecter != null)
				{
					effecter.Cleanup();
				}
				this.readyEffecter = null;
			}
			else if (this.Props.readyEffecter != null)
			{
				if (this.readyEffecter == null)
				{
					this.readyEffecter = this.Props.readyEffecter.Spawn();
					this.ColorizeEffecter(this.readyEffecter);
					this.readyEffecter.Trigger(this.parent, new TargetInfo(this.parent.InteractionCell, this.parent.Map, false));
				}
				this.readyEffecter.EffectTick(this.parent, new TargetInfo(this.parent.InteractionCell, this.parent.Map, false));
			}
			if (this.State != BiosculpterPodState.Occupied)
			{
				Effecter effecter2 = this.progressBarEffecter;
				if (effecter2 != null)
				{
					effecter2.Cleanup();
				}
				this.progressBarEffecter = null;
				Effecter effecter3 = this.operatingEffecter;
				if (effecter3 != null)
				{
					effecter3.Cleanup();
				}
				this.operatingEffecter = null;
			}
			else
			{
				Pawn occupant = this.Occupant;
				this.biotunedCountdownTicks = 3600000;
				if (this.PowerOn)
				{
					int num = 1;
					this.currentCycleTicksRemaining -= (float)num * this.CycleSpeedFactor;
					if (this.currentCycleTicksRemaining <= 0f)
					{
						this.CycleCompleted();
					}
				}
				else
				{
					this.currentCyclePowerCutTicks++;
					if (this.currentCyclePowerCutTicks >= 60000)
					{
						this.EjectContents(true, true, null);
						Messages.Message("BiosculpterNoPowerEjectedMessage".Translate(occupant.Named("PAWN")), occupant, MessageTypeDefOf.NegativeEvent, false);
					}
				}
				if (this.currentCycleTicksRemaining > 0f)
				{
					if (this.progressBarEffecter == null)
					{
						this.progressBarEffecter = EffecterDefOf.ProgressBar.Spawn();
					}
					this.progressBarEffecter.EffectTick(this.parent, TargetInfo.Invalid);
					SubEffecter_ProgressBar subEffecter_ProgressBar = this.progressBarEffecter.children[0] as SubEffecter_ProgressBar;
					MoteProgressBar moteProgressBar = (subEffecter_ProgressBar != null) ? subEffecter_ProgressBar.mote : null;
					if (moteProgressBar != null)
					{
						float num2 = this.CurrentCycle.Props.durationDays * 60000f;
						moteProgressBar.progress = 1f - Mathf.Clamp01(this.currentCycleTicksRemaining / num2);
						int num3 = (this.parent.RotatedSize.z - 1) / 2;
						moteProgressBar.offsetZ = -((float)num3 + 0.5f);
					}
					if (this.Props.operatingEffecter != null)
					{
						if (this.operatingEffecter == null)
						{
							this.operatingEffecter = this.Props.operatingEffecter.Spawn();
							this.ColorizeEffecter(this.operatingEffecter);
							this.operatingEffecter.Trigger(this.parent, new TargetInfo(this.parent.InteractionCell, this.parent.Map, false));
						}
						this.operatingEffecter.EffectTick(this.parent, new TargetInfo(this.parent.InteractionCell, this.parent.Map, false));
					}
				}
			}
			if (this.PowerOn && this.biotunedCountdownTicks > 0)
			{
				this.biotunedCountdownTicks--;
			}
			if (this.biotunedCountdownTicks <= 0)
			{
				this.biotunedTo = null;
			}
		}

		// Token: 0x060068C1 RID: 26817 RVA: 0x00235EEC File Offset: 0x002340EC
		private void ColorizeEffecter(Effecter effecter)
		{
			foreach (SubEffecter subEffecter in effecter.children)
			{
				SubEffecter_Sprayer subEffecter_Sprayer;
				if ((subEffecter_Sprayer = (subEffecter as SubEffecter_Sprayer)) != null)
				{
					subEffecter_Sprayer.colorOverride = new Color?(this.CurrentCycle.Props.operatingColor * subEffecter.def.color);
				}
			}
		}

		// Token: 0x060068C2 RID: 26818 RVA: 0x00235F70 File Offset: 0x00234170
		public override void PostDraw()
		{
			base.PostDraw();
			Rot4 rotation = this.parent.Rotation;
			Vector3 s = new Vector3(this.parent.def.graphicData.drawSize.x * 0.9f, 1f, this.parent.def.graphicData.drawSize.y * 0.9f);
			Vector3 drawPos = this.parent.DrawPos;
			drawPos.y -= 0.08108108f;
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(drawPos, rotation.AsQuat, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, CompBiosculpterPod.BackgroundMat, 0);
			if (this.State == BiosculpterPodState.Occupied)
			{
				Pawn occupant = this.Occupant;
				Vector3 drawLoc = this.parent.DrawPos + this.FloatingOffset();
				Rot4 rotation2 = this.parent.Rotation;
				if (rotation2 == Rot4.East || rotation2 == Rot4.West)
				{
					drawLoc.z += 0.2f;
				}
				occupant.Drawer.renderer.RenderPawnAt(drawLoc, null, true);
			}
		}

		// Token: 0x060068C3 RID: 26819 RVA: 0x0023609C File Offset: 0x0023429C
		private Vector3 FloatingOffset()
		{
			float num = (this.currentCycleTicksRemaining + (float)this.currentCyclePowerCutTicks) % 500f / 500f;
			float num2 = Mathf.Sin(3.1415927f * num);
			float z = num2 * num2 * 0.04f;
			return new Vector3(0f, 0f, z);
		}

		// Token: 0x060068C4 RID: 26820 RVA: 0x002360EA File Offset: 0x002342EA
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x060068C5 RID: 26821 RVA: 0x002360F8 File Offset: 0x002342F8
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x060068C6 RID: 26822 RVA: 0x00236100 File Offset: 0x00234300
		public StorageSettings GetStoreSettings()
		{
			return this.allowedNutritionSettings;
		}

		// Token: 0x060068C7 RID: 26823 RVA: 0x00236108 File Offset: 0x00234308
		public StorageSettings GetParentStoreSettings()
		{
			return this.parent.def.building.fixedStorageSettings;
		}

		// Token: 0x170011ED RID: 4589
		// (get) Token: 0x060068C8 RID: 26824 RVA: 0x000126F5 File Offset: 0x000108F5
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060068C9 RID: 26825 RVA: 0x0023611F File Offset: 0x0023431F
		public static IEnumerable<Thing> FindPodsFor(Pawn pawn, Pawn traveller)
		{
			CompBiosculpterPod.<>c__DisplayClass70_0 CS$<>8__locals1 = new CompBiosculpterPod.<>c__DisplayClass70_0();
			CS$<>8__locals1.traveller = traveller;
			IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
			where def.GetCompProperties<CompProperties_BiosculpterPod>() != null
			select def;
			foreach (ThingDef podDef in enumerable)
			{
				foreach (CompProperties compProperties in podDef.comps)
				{
					CompBiosculpterPod.<>c__DisplayClass70_1 CS$<>8__locals2 = new CompBiosculpterPod.<>c__DisplayClass70_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					CS$<>8__locals2.cycle = (compProperties as CompProperties_BiosculpterPod_BaseCycle);
					if (CS$<>8__locals2.cycle != null)
					{
						Thing thing = GenClosest.ClosestThingReachable(CS$<>8__locals2.CS$<>8__locals1.traveller.Position, pawn.Map, ThingRequest.ForDef(podDef), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, new Predicate<Thing>(CS$<>8__locals2.<FindPodsFor>g__PodValidator|1), null, 0, -1, false, RegionType.Set_Passable, false);
						if (thing != null)
						{
							yield return thing;
						}
					}
				}
				List<CompProperties>.Enumerator enumerator2 = default(List<CompProperties>.Enumerator);
				podDef = null;
			}
			IEnumerator<ThingDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060068CA RID: 26826 RVA: 0x00236138 File Offset: 0x00234338
		public static void AddCarryToPodJobs(List<FloatMenuOption> opts, Pawn pawn, Pawn traveller)
		{
			CompBiosculpterPod.<>c__DisplayClass71_0 CS$<>8__locals1 = new CompBiosculpterPod.<>c__DisplayClass71_0();
			CS$<>8__locals1.traveller = traveller;
			CS$<>8__locals1.pawn = pawn;
			if (!CS$<>8__locals1.pawn.CanReserveAndReach(CS$<>8__locals1.traveller, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, true))
			{
				return;
			}
			using (IEnumerator<Thing> enumerator = CompBiosculpterPod.FindPodsFor(CS$<>8__locals1.pawn, CS$<>8__locals1.traveller).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CompBiosculpterPod.<>c__DisplayClass71_1 CS$<>8__locals2 = new CompBiosculpterPod.<>c__DisplayClass71_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					CS$<>8__locals2.pod = enumerator.Current;
					CompBiosculpterPod podComp = CS$<>8__locals2.pod.TryGetComp<CompBiosculpterPod>();
					string text = "CarryToBiosculpterPod".Translate(CS$<>8__locals2.CS$<>8__locals1.traveller.Named("PAWN"), podComp.CurrentCycle.Props.label.Named("CYCLE"));
					Action action = delegate()
					{
						if (!podComp.CanAccept(CS$<>8__locals2.CS$<>8__locals1.traveller))
						{
							Messages.Message("CannotCarryToBiosculpterPod".Translate() + ": " + "NoBiosculpterPod".Translate(), CS$<>8__locals2.CS$<>8__locals1.traveller, MessageTypeDefOf.RejectInput, false);
							return;
						}
						Job job = JobMaker.MakeJob(JobDefOf.CarryToBiosculpterPod, CS$<>8__locals2.CS$<>8__locals1.traveller, CS$<>8__locals2.pod);
						job.count = 1;
						CS$<>8__locals2.CS$<>8__locals1.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
					};
					if (CS$<>8__locals2.CS$<>8__locals1.traveller.IsQuestLodger())
					{
						text += " (" + "CryptosleepCasketGuestsNotAllowed".Translate() + ")";
						opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, null, MenuOptionPriority.Default, null, CS$<>8__locals2.CS$<>8__locals1.traveller, 0f, null, null, true, 0), CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.CS$<>8__locals1.traveller, "ReservedBy"));
					}
					else if (CS$<>8__locals2.CS$<>8__locals1.traveller.GetExtraHostFaction(null) != null)
					{
						text += " (" + "CryptosleepCasketGuestPrisonersNotAllowed".Translate() + ")";
						opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, null, MenuOptionPriority.Default, null, CS$<>8__locals2.CS$<>8__locals1.traveller, 0f, null, null, true, 0), CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.CS$<>8__locals1.traveller, "ReservedBy"));
					}
					else
					{
						opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, action, MenuOptionPriority.Default, null, CS$<>8__locals2.CS$<>8__locals1.traveller, 0f, null, null, true, 0), CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.CS$<>8__locals1.traveller, "ReservedBy"));
					}
				}
			}
		}

		// Token: 0x060068CB RID: 26827 RVA: 0x002363F8 File Offset: 0x002345F8
		public static bool WasLoadingCanceled(Thing thing)
		{
			CompBiosculpterPod compBiosculpterPod = thing.TryGetComp<CompBiosculpterPod>();
			return compBiosculpterPod != null && compBiosculpterPod.State != BiosculpterPodState.LoadingNutrition;
		}

		// Token: 0x04003AA5 RID: 15013
		private const int NoPowerEjectCumulativeTicks = 60000;

		// Token: 0x04003AA6 RID: 15014
		private const int BiotunedDuration = 3600000;

		// Token: 0x04003AA7 RID: 15015
		public static readonly Texture2D InterruptCycleIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x04003AA8 RID: 15016
		public static readonly Texture2D CancelLoadingIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x04003AA9 RID: 15017
		private static readonly Material BackgroundMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.082f, 0.078f, 0.063f), false);

		// Token: 0x04003AAA RID: 15018
		private const float BackgroundRect_YOff = 0.08108108f;

		// Token: 0x04003AAB RID: 15019
		private const float Pawn_YOff = 0.04054054f;

		// Token: 0x04003AAC RID: 15020
		private string currentCycleKey;

		// Token: 0x04003AAD RID: 15021
		private float currentCycleTicksRemaining;

		// Token: 0x04003AAE RID: 15022
		private int currentCyclePowerCutTicks;

		// Token: 0x04003AAF RID: 15023
		private ThingOwner innerContainer;

		// Token: 0x04003AB0 RID: 15024
		private bool nutritionLoaded;

		// Token: 0x04003AB1 RID: 15025
		private Pawn biotunedTo;

		// Token: 0x04003AB2 RID: 15026
		private int biotunedCountdownTicks;

		// Token: 0x04003AB3 RID: 15027
		private StorageSettings allowedNutritionSettings;

		// Token: 0x04003AB4 RID: 15028
		private Effecter progressBarEffecter;

		// Token: 0x04003AB5 RID: 15029
		private Effecter operatingEffecter;

		// Token: 0x04003AB6 RID: 15030
		private Effecter readyEffecter;

		// Token: 0x04003AB7 RID: 15031
		private List<CompBiosculpterPod_Cycle> cachedAvailableCycles;
	}
}
