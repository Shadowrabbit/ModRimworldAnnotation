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
	// Token: 0x020016A3 RID: 5795
	public class Building_Bed : Building
	{
		// Token: 0x1700139A RID: 5018
		// (get) Token: 0x06007EBE RID: 32446 RVA: 0x00055295 File Offset: 0x00053495
		public List<Pawn> OwnersForReading
		{
			get
			{
				return this.CompAssignableToPawn.AssignedPawnsForReading;
			}
		}

		// Token: 0x1700139B RID: 5019
		// (get) Token: 0x06007EBF RID: 32447 RVA: 0x000552A2 File Offset: 0x000534A2
		public CompAssignableToPawn CompAssignableToPawn
		{
			get
			{
				return base.GetComp<CompAssignableToPawn>();
			}
		}

		// Token: 0x1700139C RID: 5020
		// (get) Token: 0x06007EC0 RID: 32448 RVA: 0x000552AA File Offset: 0x000534AA
		// (set) Token: 0x06007EC1 RID: 32449 RVA: 0x0025AB94 File Offset: 0x00258D94
		public bool ForPrisoners
		{
			get
			{
				return this.forPrisonersInt;
			}
			set
			{
				if (value == this.forPrisonersInt || !this.def.building.bed_humanlike)
				{
					return;
				}
				if (Current.ProgramState != ProgramState.Playing && Scribe.mode != LoadSaveMode.Inactive)
				{
					Log.Error("Tried to set ForPrisoners while game mode was " + Current.ProgramState, false);
					return;
				}
				this.RemoveAllOwners();
				this.forPrisonersInt = value;
				this.Notify_ColorChanged();
				this.NotifyRoomBedTypeChanged();
			}
		}

		// Token: 0x1700139D RID: 5021
		// (get) Token: 0x06007EC2 RID: 32450 RVA: 0x000552B2 File Offset: 0x000534B2
		// (set) Token: 0x06007EC3 RID: 32451 RVA: 0x0025AC00 File Offset: 0x00258E00
		public bool Medical
		{
			get
			{
				return this.medicalInt;
			}
			set
			{
				if (value == this.medicalInt || !this.def.building.bed_humanlike)
				{
					return;
				}
				this.RemoveAllOwners();
				this.medicalInt = value;
				this.Notify_ColorChanged();
				if (base.Spawned)
				{
					base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
					this.NotifyRoomBedTypeChanged();
				}
				this.FacilityChanged();
			}
		}

		// Token: 0x1700139E RID: 5022
		// (get) Token: 0x06007EC4 RID: 32452 RVA: 0x000552BA File Offset: 0x000534BA
		public bool AnyUnownedSleepingSlot
		{
			get
			{
				if (this.Medical)
				{
					Log.Warning("Tried to check for unowned sleeping slot on medical bed " + this, false);
					return false;
				}
				return this.CompAssignableToPawn.HasFreeSlot;
			}
		}

		// Token: 0x1700139F RID: 5023
		// (get) Token: 0x06007EC5 RID: 32453 RVA: 0x0025AC68 File Offset: 0x00258E68
		public bool AnyUnoccupiedSleepingSlot
		{
			get
			{
				for (int i = 0; i < this.SleepingSlotsCount; i++)
				{
					if (this.GetCurOccupant(i) == null)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170013A0 RID: 5024
		// (get) Token: 0x06007EC6 RID: 32454 RVA: 0x000552E2 File Offset: 0x000534E2
		public IEnumerable<Pawn> CurOccupants
		{
			get
			{
				int num;
				for (int i = 0; i < this.SleepingSlotsCount; i = num + 1)
				{
					Pawn curOccupant = this.GetCurOccupant(i);
					if (curOccupant != null)
					{
						yield return curOccupant;
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x170013A1 RID: 5025
		// (get) Token: 0x06007EC7 RID: 32455 RVA: 0x000552F2 File Offset: 0x000534F2
		public override Color DrawColor
		{
			get
			{
				if (this.def.MadeFromStuff)
				{
					return base.DrawColor;
				}
				return this.DrawColorTwo;
			}
		}

		// Token: 0x170013A2 RID: 5026
		// (get) Token: 0x06007EC8 RID: 32456 RVA: 0x0025AC94 File Offset: 0x00258E94
		public override Color DrawColorTwo
		{
			get
			{
				if (!this.def.building.bed_humanlike)
				{
					return base.DrawColorTwo;
				}
				bool forPrisoners = this.ForPrisoners;
				bool medical = this.Medical;
				if (forPrisoners && medical)
				{
					return Building_Bed.SheetColorMedicalForPrisoner;
				}
				if (forPrisoners)
				{
					return Building_Bed.SheetColorForPrisoner;
				}
				if (medical)
				{
					return Building_Bed.SheetColorMedical;
				}
				if (this.def == ThingDefOf.RoyalBed)
				{
					return Building_Bed.SheetColorRoyal;
				}
				return Building_Bed.SheetColorNormal;
			}
		}

		// Token: 0x170013A3 RID: 5027
		// (get) Token: 0x06007EC9 RID: 32457 RVA: 0x0005530E File Offset: 0x0005350E
		public int SleepingSlotsCount
		{
			get
			{
				return BedUtility.GetSleepingSlotsCount(this.def.size);
			}
		}

		// Token: 0x170013A4 RID: 5028
		// (get) Token: 0x06007ECA RID: 32458 RVA: 0x00055320 File Offset: 0x00053520
		private bool PlayerCanSeeOwners
		{
			get
			{
				return this.CompAssignableToPawn.PlayerCanSeeAssignments;
			}
		}

		// Token: 0x06007ECB RID: 32459 RVA: 0x0025AD00 File Offset: 0x00258F00
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			Region validRegionAt_NoRebuild = map.regionGrid.GetValidRegionAt_NoRebuild(base.Position);
			if (validRegionAt_NoRebuild != null && validRegionAt_NoRebuild.Room.isPrisonCell)
			{
				this.ForPrisoners = true;
			}
			if (!this.alreadySetDefaultMed)
			{
				this.alreadySetDefaultMed = true;
				if (this.def.building.bed_defaultMedical)
				{
					this.Medical = true;
				}
			}
		}

		// Token: 0x06007ECC RID: 32460 RVA: 0x0025AD68 File Offset: 0x00258F68
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			this.RemoveAllOwners();
			this.ForPrisoners = false;
			this.Medical = false;
			this.alreadySetDefaultMed = false;
			Room room = this.GetRoom(RegionType.Set_Passable);
			base.DeSpawn(mode);
			if (room != null)
			{
				room.Notify_RoomShapeOrContainedBedsChanged();
			}
		}

		// Token: 0x06007ECD RID: 32461 RVA: 0x0005532D File Offset: 0x0005352D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.forPrisonersInt, "forPrisoners", false, false);
			Scribe_Values.Look<bool>(ref this.medicalInt, "medical", false, false);
			Scribe_Values.Look<bool>(ref this.alreadySetDefaultMed, "alreadySetDefaultMed", false, false);
		}

		// Token: 0x06007ECE RID: 32462 RVA: 0x0025ADA8 File Offset: 0x00258FA8
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			Room room = this.GetRoom(RegionType.Set_Passable);
			if (room != null && Building_Bed.RoomCanBePrisonCell(room))
			{
				room.DrawFieldEdges();
			}
		}

		// Token: 0x06007ECF RID: 32463 RVA: 0x0005536B File Offset: 0x0005356B
		public static bool RoomCanBePrisonCell(Room r)
		{
			return !r.TouchesMapEdge && !r.IsHuge && r.RegionType == RegionType.Normal;
		}

		// Token: 0x06007ED0 RID: 32464 RVA: 0x00055388 File Offset: 0x00053588
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.def.building.bed_humanlike && base.Faction == Faction.OfPlayer)
			{
				Command_Toggle command_Toggle = new Command_Toggle();
				command_Toggle.defaultLabel = "CommandBedSetForPrisonersLabel".Translate();
				command_Toggle.defaultDesc = "CommandBedSetForPrisonersDesc".Translate();
				command_Toggle.icon = ContentFinder<Texture2D>.Get("UI/Commands/ForPrisoners", true);
				command_Toggle.isActive = (() => this.ForPrisoners);
				command_Toggle.toggleAction = delegate()
				{
					this.ToggleForPrisonersByInterface();
				};
				if (!Building_Bed.RoomCanBePrisonCell(this.GetRoom(RegionType.Set_Passable)) && !this.ForPrisoners)
				{
					command_Toggle.Disable("CommandBedSetForPrisonersFailOutdoors".Translate());
				}
				command_Toggle.hotKey = KeyBindingDefOf.Misc3;
				command_Toggle.turnOffSound = null;
				command_Toggle.turnOnSound = null;
				yield return command_Toggle;
				yield return new Command_Toggle
				{
					defaultLabel = "CommandBedSetAsMedicalLabel".Translate(),
					defaultDesc = "CommandBedSetAsMedicalDesc".Translate(),
					icon = ContentFinder<Texture2D>.Get("UI/Commands/AsMedical", true),
					isActive = (() => this.Medical),
					toggleAction = delegate()
					{
						this.Medical = !this.Medical;
					},
					hotKey = KeyBindingDefOf.Misc2
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x06007ED1 RID: 32465 RVA: 0x0025ADD4 File Offset: 0x00258FD4
		private void ToggleForPrisonersByInterface()
		{
			if (Building_Bed.lastPrisonerSetChangeFrame == Time.frameCount)
			{
				return;
			}
			Building_Bed.lastPrisonerSetChangeFrame = Time.frameCount;
			bool newForPrisoners = !this.ForPrisoners;
			(newForPrisoners ? SoundDefOf.Checkbox_TurnedOn : SoundDefOf.Checkbox_TurnedOff).PlayOneShotOnCamera(null);
			List<Building_Bed> bedsToAffect = new List<Building_Bed>();
			foreach (Building_Bed building_Bed in Find.Selector.SelectedObjects.OfType<Building_Bed>())
			{
				if (building_Bed.ForPrisoners != newForPrisoners)
				{
					Room room = building_Bed.GetRoom(RegionType.Set_Passable);
					if (room == null || !Building_Bed.RoomCanBePrisonCell(room))
					{
						if (!bedsToAffect.Contains(building_Bed))
						{
							bedsToAffect.Add(building_Bed);
						}
					}
					else
					{
						foreach (Building_Bed item in room.ContainedBeds)
						{
							if (!bedsToAffect.Contains(item))
							{
								bedsToAffect.Add(item);
							}
						}
					}
				}
			}
			Action action = delegate()
			{
				List<Room> list = new List<Room>();
				foreach (Building_Bed building_Bed3 in bedsToAffect)
				{
					Room room2 = building_Bed3.GetRoom(RegionType.Set_Passable);
					building_Bed3.ForPrisoners = (newForPrisoners && !room2.TouchesMapEdge);
					for (int j = 0; j < this.SleepingSlotsCount; j++)
					{
						Pawn curOccupant = this.GetCurOccupant(j);
						if (curOccupant != null)
						{
							curOccupant.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
						}
					}
					if (!list.Contains(room2) && !room2.TouchesMapEdge)
					{
						list.Add(room2);
					}
				}
				foreach (Room room3 in list)
				{
					room3.Notify_RoomShapeOrContainedBedsChanged();
				}
			};
			if ((from b in bedsToAffect
			where b.OwnersForReading.Any<Pawn>() && b != this
			select b).Count<Building_Bed>() == 0)
			{
				action();
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (newForPrisoners)
			{
				stringBuilder.Append("TurningOnPrisonerBedWarning".Translate());
			}
			else
			{
				stringBuilder.Append("TurningOffPrisonerBedWarning".Translate());
			}
			stringBuilder.AppendLine();
			foreach (Building_Bed building_Bed2 in bedsToAffect)
			{
				if ((newForPrisoners && !building_Bed2.ForPrisoners) || (!newForPrisoners && building_Bed2.ForPrisoners))
				{
					for (int i = 0; i < building_Bed2.OwnersForReading.Count; i++)
					{
						stringBuilder.AppendLine();
						stringBuilder.Append(building_Bed2.OwnersForReading[i].LabelShort);
					}
				}
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.Append("AreYouSure".Translate());
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(stringBuilder.ToString(), action, false, null));
		}

		// Token: 0x06007ED2 RID: 32466 RVA: 0x0025B074 File Offset: 0x00259274
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (this.def.building.bed_humanlike)
			{
				if (this.ForPrisoners)
				{
					stringBuilder.AppendInNewLine("ForPrisonerUse".Translate());
				}
				else if (this.PlayerCanSeeOwners)
				{
					stringBuilder.AppendInNewLine("ForColonistUse".Translate());
				}
				if (this.Medical)
				{
					stringBuilder.AppendInNewLine("MedicalBed".Translate());
					if (base.Spawned)
					{
						stringBuilder.AppendInNewLine("RoomInfectionChanceFactor".Translate() + ": " + this.GetRoom(RegionType.Set_Passable).GetStat(RoomStatDefOf.InfectionChanceFactor).ToStringPercent());
					}
				}
				else if (this.PlayerCanSeeOwners)
				{
					if (this.OwnersForReading.Count == 0)
					{
						stringBuilder.AppendInNewLine("Owner".Translate() + ": " + "Nobody".Translate());
					}
					else if (this.OwnersForReading.Count == 1)
					{
						stringBuilder.AppendInNewLine("Owner".Translate() + ": " + this.OwnersForReading[0].Label);
					}
					else
					{
						stringBuilder.AppendInNewLine("Owners".Translate() + ": ");
						bool flag = false;
						for (int i = 0; i < this.OwnersForReading.Count; i++)
						{
							if (flag)
							{
								stringBuilder.Append(", ");
							}
							flag = true;
							stringBuilder.Append(this.OwnersForReading[i].LabelShort);
						}
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06007ED3 RID: 32467 RVA: 0x00055398 File Offset: 0x00053598
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
		{
			if (myPawn.RaceProps.Humanlike && !this.ForPrisoners && this.Medical && !myPawn.Drafted && base.Faction == Faction.OfPlayer && RestUtility.CanUseBedEver(myPawn, this.def))
			{
				if (!HealthAIUtility.ShouldSeekMedicalRest(myPawn) && !HealthAIUtility.ShouldSeekMedicalRestUrgent(myPawn))
				{
					yield return new FloatMenuOption("UseMedicalBed".Translate() + " (" + "NotInjured".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
				}
				else
				{
					Action action = delegate()
					{
						if (!this.ForPrisoners && this.Medical && myPawn.CanReserveAndReach(this, PathEndMode.ClosestTouch, Danger.Deadly, this.SleepingSlotsCount, -1, null, true))
						{
							if (myPawn.CurJobDef == JobDefOf.LayDown && myPawn.CurJob.GetTarget(TargetIndex.A).Thing == this)
							{
								myPawn.CurJob.restUntilHealed = true;
							}
							else
							{
								Job job = JobMaker.MakeJob(JobDefOf.LayDown, this);
								job.restUntilHealed = true;
								myPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
							}
							myPawn.mindState.ResetLastDisturbanceTick();
						}
					};
					yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("UseMedicalBed".Translate(), action, MenuOptionPriority.Default, null, null, 0f, null, null), myPawn, this, (this.AnyUnoccupiedSleepingSlot ? "ReservedBy" : "SomeoneElseSleeping").CapitalizeFirst());
				}
			}
			yield break;
		}

		// Token: 0x06007ED4 RID: 32468 RVA: 0x0025B244 File Offset: 0x00259444
		public override void DrawGUIOverlay()
		{
			if (this.Medical)
			{
				return;
			}
			if (Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest && this.PlayerCanSeeOwners)
			{
				Color defaultThingLabelColor = GenMapUI.DefaultThingLabelColor;
				if (!this.OwnersForReading.Any<Pawn>())
				{
					GenMapUI.DrawThingLabel(this, "Unowned".Translate(), defaultThingLabelColor);
					return;
				}
				if (this.OwnersForReading.Count == 1)
				{
					if (this.OwnersForReading[0].InBed() && this.OwnersForReading[0].CurrentBed() == this)
					{
						return;
					}
					GenMapUI.DrawThingLabel(this, this.OwnersForReading[0].LabelShort, defaultThingLabelColor);
					return;
				}
				else
				{
					for (int i = 0; i < this.OwnersForReading.Count; i++)
					{
						if (!this.OwnersForReading[i].InBed() || this.OwnersForReading[i].CurrentBed() != this || !(this.OwnersForReading[i].Position == this.GetSleepingSlotPos(i)))
						{
							GenMapUI.DrawThingLabel(this.GetMultiOwnersLabelScreenPosFor(i), this.OwnersForReading[i].LabelShort, defaultThingLabelColor);
						}
					}
				}
			}
		}

		// Token: 0x06007ED5 RID: 32469 RVA: 0x0025B36C File Offset: 0x0025956C
		public Pawn GetCurOccupant(int slotIndex)
		{
			if (!base.Spawned)
			{
				return null;
			}
			IntVec3 sleepingSlotPos = this.GetSleepingSlotPos(slotIndex);
			List<Thing> list = base.Map.thingGrid.ThingsListAt(sleepingSlotPos);
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn = list[i] as Pawn;
				if (pawn != null && pawn.CurJob != null && pawn.GetPosture() == PawnPosture.LayingInBed)
				{
					return pawn;
				}
			}
			return null;
		}

		// Token: 0x06007ED6 RID: 32470 RVA: 0x0025B3D4 File Offset: 0x002595D4
		public int GetCurOccupantSlotIndex(Pawn curOccupant)
		{
			for (int i = 0; i < this.SleepingSlotsCount; i++)
			{
				if (this.GetCurOccupant(i) == curOccupant)
				{
					return i;
				}
			}
			Log.Error("Could not find pawn " + curOccupant + " on any of sleeping slots.", false);
			return 0;
		}

		// Token: 0x06007ED7 RID: 32471 RVA: 0x0025B418 File Offset: 0x00259618
		public Pawn GetCurOccupantAt(IntVec3 pos)
		{
			for (int i = 0; i < this.SleepingSlotsCount; i++)
			{
				if (this.GetSleepingSlotPos(i) == pos)
				{
					return this.GetCurOccupant(i);
				}
			}
			return null;
		}

		// Token: 0x06007ED8 RID: 32472 RVA: 0x000553AF File Offset: 0x000535AF
		public IntVec3 GetSleepingSlotPos(int index)
		{
			return BedUtility.GetSleepingSlotPos(index, base.Position, base.Rotation, this.def.size);
		}

		// Token: 0x06007ED9 RID: 32473 RVA: 0x0025B450 File Offset: 0x00259650
		private void RemoveAllOwners()
		{
			for (int i = this.OwnersForReading.Count - 1; i >= 0; i--)
			{
				this.OwnersForReading[i].ownership.UnclaimBed();
			}
		}

		// Token: 0x06007EDA RID: 32474 RVA: 0x0025B48C File Offset: 0x0025968C
		private void NotifyRoomBedTypeChanged()
		{
			Room room = this.GetRoom(RegionType.Set_Passable);
			if (room != null)
			{
				room.Notify_BedTypeChanged();
			}
		}

		// Token: 0x06007EDB RID: 32475 RVA: 0x0025B4AC File Offset: 0x002596AC
		private void FacilityChanged()
		{
			CompFacility compFacility = this.TryGetComp<CompFacility>();
			CompAffectedByFacilities compAffectedByFacilities = this.TryGetComp<CompAffectedByFacilities>();
			if (compFacility != null)
			{
				compFacility.Notify_ThingChanged();
			}
			if (compAffectedByFacilities != null)
			{
				compAffectedByFacilities.Notify_ThingChanged();
			}
		}

		// Token: 0x06007EDC RID: 32476 RVA: 0x0025B4DC File Offset: 0x002596DC
		private Vector3 GetMultiOwnersLabelScreenPosFor(int slotIndex)
		{
			IntVec3 sleepingSlotPos = this.GetSleepingSlotPos(slotIndex);
			Vector3 drawPos = this.DrawPos;
			if (base.Rotation.IsHorizontal)
			{
				drawPos.z = (float)sleepingSlotPos.z + 0.6f;
			}
			else
			{
				drawPos.x = (float)sleepingSlotPos.x + 0.5f;
				drawPos.z += -0.4f;
			}
			Vector2 v = drawPos.MapToUIPosition();
			if (!base.Rotation.IsHorizontal && this.SleepingSlotsCount == 2)
			{
				v = this.AdjustOwnerLabelPosToAvoidOverlapping(v, slotIndex);
			}
			return v;
		}

		// Token: 0x06007EDD RID: 32477 RVA: 0x0025B57C File Offset: 0x0025977C
		private Vector3 AdjustOwnerLabelPosToAvoidOverlapping(Vector3 screenPos, int slotIndex)
		{
			Text.Font = GameFont.Tiny;
			float num = Text.CalcSize(this.OwnersForReading[slotIndex].LabelShort).x + 1f;
			Vector2 vector = this.DrawPos.MapToUIPosition();
			float num2 = Mathf.Abs(screenPos.x - vector.x);
			IntVec3 sleepingSlotPos = this.GetSleepingSlotPos(slotIndex);
			if (num > num2 * 2f)
			{
				float num3;
				if (slotIndex == 0)
				{
					num3 = (float)this.GetSleepingSlotPos(1).x;
				}
				else
				{
					num3 = (float)this.GetSleepingSlotPos(0).x;
				}
				if ((float)sleepingSlotPos.x < num3)
				{
					screenPos.x -= (num - num2 * 2f) / 2f;
				}
				else
				{
					screenPos.x += (num - num2 * 2f) / 2f;
				}
			}
			return screenPos;
		}

		// Token: 0x04005276 RID: 21110
		private bool forPrisonersInt;

		// Token: 0x04005277 RID: 21111
		private bool medicalInt;

		// Token: 0x04005278 RID: 21112
		private bool alreadySetDefaultMed;

		// Token: 0x04005279 RID: 21113
		private static int lastPrisonerSetChangeFrame = -1;

		// Token: 0x0400527A RID: 21114
		private static readonly Color SheetColorNormal = new Color(0.6313726f, 0.8352941f, 0.7058824f);

		// Token: 0x0400527B RID: 21115
		private static readonly Color SheetColorRoyal = new Color(0.67058825f, 0.9137255f, 0.74509805f);

		// Token: 0x0400527C RID: 21116
		public static readonly Color SheetColorForPrisoner = new Color(1f, 0.7176471f, 0.12941177f);

		// Token: 0x0400527D RID: 21117
		private static readonly Color SheetColorMedical = new Color(0.3882353f, 0.62352943f, 0.8862745f);

		// Token: 0x0400527E RID: 21118
		private static readonly Color SheetColorMedicalForPrisoner = new Color(0.654902f, 0.3764706f, 0.15294118f);
	}
}
