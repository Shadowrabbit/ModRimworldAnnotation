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
    // Token: 0x0200106E RID: 4206
    public class Building_Bed : Building
    {
        // Token: 0x170010FC RID: 4348
        // (get) Token: 0x060063A0 RID: 25504 RVA: 0x0021A67B File Offset: 0x0021887B
        public List<Pawn> OwnersForReading
        {
            get { return this.CompAssignableToPawn.AssignedPawnsForReading; }
        }

        // Token: 0x170010FD RID: 4349
        // (get) Token: 0x060063A1 RID: 25505 RVA: 0x0021A688 File Offset: 0x00218888
        public CompAssignableToPawn CompAssignableToPawn
        {
            get { return base.GetComp<CompAssignableToPawn>(); }
        }

        // Token: 0x170010FE RID: 4350
        // (get) Token: 0x060063A2 RID: 25506 RVA: 0x0021A690 File Offset: 0x00218890
        // (set) Token: 0x060063A3 RID: 25507 RVA: 0x0021A69C File Offset: 0x0021889C
        public bool ForPrisoners
        {
            get { return this.forOwnerType == BedOwnerType.Prisoner; }
            set
            {
                if (value == this.ForPrisoners || !this.def.building.bed_humanlike)
                {
                    return;
                }

                if (Current.ProgramState != ProgramState.Playing && Scribe.mode != LoadSaveMode.Inactive)
                {
                    Log.Error("Tried to set ForPrisoners while game mode was " + Current.ProgramState);
                    return;
                }

                this.RemoveAllOwners(false);
                this.forOwnerType = BedOwnerType.Prisoner;
                this.Notify_ColorChanged();
                this.NotifyRoomBedTypeChanged();
            }
        }

        // Token: 0x170010FF RID: 4351
        // (get) Token: 0x060063A4 RID: 25508 RVA: 0x0021A708 File Offset: 0x00218908
        public bool ForSlaves
        {
            get { return this.ForOwnerType == BedOwnerType.Slave; }
        }

        // Token: 0x17001100 RID: 4352
        // (get) Token: 0x060063A5 RID: 25509 RVA: 0x0021A713 File Offset: 0x00218913
        public bool ForColonists
        {
            get { return this.ForOwnerType == BedOwnerType.Colonist; }
        }

        // Token: 0x17001101 RID: 4353
        // (get) Token: 0x060063A6 RID: 25510 RVA: 0x0021A71E File Offset: 0x0021891E
        // (set) Token: 0x060063A7 RID: 25511 RVA: 0x0021A728 File Offset: 0x00218928
        public BedOwnerType ForOwnerType
        {
            get { return this.forOwnerType; }
            set
            {
                if (value == this.forOwnerType || !this.def.building.bed_humanlike)
                {
                    return;
                }

                if (value == BedOwnerType.Slave && !ModLister.CheckIdeology("Slavery"))
                {
                    return;
                }

                this.RemoveAllOwners(false);
                this.forOwnerType = value;
                this.Notify_ColorChanged();
                this.NotifyRoomBedTypeChanged();
            }
        }

        // Token: 0x17001102 RID: 4354
        // (get) Token: 0x060063A8 RID: 25512 RVA: 0x0021A77C File Offset: 0x0021897C
        // (set) Token: 0x060063A9 RID: 25513 RVA: 0x0021A784 File Offset: 0x00218984
        public bool Medical
        {
            get { return this.medicalInt; }
            set
            {
                if (value == this.medicalInt)
                {
                    return;
                }

                this.RemoveAllOwners(false);
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

        // Token: 0x17001103 RID: 4355
        // (get) Token: 0x060063AA RID: 25514 RVA: 0x0021A7DA File Offset: 0x002189DA
        public bool AnyUnownedSleepingSlot
        {
            get
            {
                if (this.Medical)
                {
                    Log.Warning("Tried to check for unowned sleeping slot on medical bed " + this);
                    return false;
                }

                return this.CompAssignableToPawn.HasFreeSlot;
            }
        }

        // Token: 0x17001104 RID: 4356
        // (get) Token: 0x060063AB RID: 25515 RVA: 0x0021A801 File Offset: 0x00218A01
        public int TotalSleepingSlots
        {
            get
            {
                if (this.Medical)
                {
                    Log.Warning("Tried to check for total sleeping slots on medical bed " + this);
                    return 0;
                }

                return this.CompAssignableToPawn.TotalSlots;
            }
        }

        // Token: 0x17001105 RID: 4357
        // (get) Token: 0x060063AC RID: 25516 RVA: 0x0021A828 File Offset: 0x00218A28
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

        // Token: 0x17001106 RID: 4358
        // (get) Token: 0x060063AD RID: 25517 RVA: 0x0021A852 File Offset: 0x00218A52
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

        // Token: 0x17001107 RID: 4359
        // (get) Token: 0x060063AE RID: 25518 RVA: 0x0021A864 File Offset: 0x00218A64
        public bool AnyOccupants
        {
            get
            {
                for (int i = 0; i < this.SleepingSlotsCount; i++)
                {
                    if (this.GetCurOccupant(i) != null)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        // Token: 0x17001108 RID: 4360
        // (get) Token: 0x060063AF RID: 25519 RVA: 0x0021A88E File Offset: 0x00218A8E
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

        // Token: 0x17001109 RID: 4361
        // (get) Token: 0x060063B0 RID: 25520 RVA: 0x0021A8AC File Offset: 0x00218AAC
        public override Color DrawColorTwo
        {
            get
            {
                bool medical = this.Medical;
                BedOwnerType bedOwnerType = this.forOwnerType;
                if (bedOwnerType != BedOwnerType.Prisoner)
                {
                    if (bedOwnerType != BedOwnerType.Slave)
                    {
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
                    else
                    {
                        if (!medical)
                        {
                            return Building_Bed.SheetColorForSlave;
                        }

                        return Building_Bed.SheetColorMedicalForSlave;
                    }
                }
                else
                {
                    if (!medical)
                    {
                        return Building_Bed.SheetColorForPrisoner;
                    }

                    return Building_Bed.SheetColorMedicalForPrisoner;
                }
            }
        }

        // Token: 0x1700110A RID: 4362
        // (get) Token: 0x060063B1 RID: 25521 RVA: 0x0021A910 File Offset: 0x00218B10
        public int SleepingSlotsCount
        {
            get { return BedUtility.GetSleepingSlotsCount(this.def.size); }
        }

        // Token: 0x1700110B RID: 4363
        // (get) Token: 0x060063B2 RID: 25522 RVA: 0x0021A922 File Offset: 0x00218B22
        private bool PlayerCanSeeOwners
        {
            get { return this.CompAssignableToPawn.PlayerCanSeeAssignments; }
        }

        // Token: 0x060063B3 RID: 25523 RVA: 0x0021A930 File Offset: 0x00218B30
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            Region validRegionAt_NoRebuild = map.regionGrid.GetValidRegionAt_NoRebuild(base.Position);
            if (validRegionAt_NoRebuild != null && validRegionAt_NoRebuild.Room.IsPrisonCell)
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

            if (!respawningAfterLoad)
            {
                District district = this.GetDistrict(RegionType.Set_Passable);
                if (district != null)
                {
                    district.Notify_RoomShapeOrContainedBedsChanged();
                    district.Room.Notify_RoomShapeChanged();
                }
            }
        }

        // Token: 0x060063B4 RID: 25524 RVA: 0x0021A9B8 File Offset: 0x00218BB8
        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            this.RemoveAllOwners(mode == DestroyMode.KillFinalize);
            this.ForPrisoners = false;
            this.Medical = false;
            this.alreadySetDefaultMed = false;
            District district = this.GetDistrict(RegionType.Set_Passable);
            base.DeSpawn(mode);
            if (district != null)
            {
                district.Notify_RoomShapeOrContainedBedsChanged();
                district.Room.Notify_RoomShapeChanged();
            }
        }

        // Token: 0x060063B5 RID: 25525 RVA: 0x0021AA08 File Offset: 0x00218C08
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<bool>(ref this.medicalInt, "medical", false, false);
            Scribe_Values.Look<bool>(ref this.alreadySetDefaultMed, "alreadySetDefaultMed", false, false);
            Scribe_Values.Look<BedOwnerType>(ref this.forOwnerType, "forOwnerType", BedOwnerType.Colonist, false);
            BackCompatibility.PostExposeData(this);
        }

        // Token: 0x060063B6 RID: 25526 RVA: 0x0021AA58 File Offset: 0x00218C58
        public override void DrawExtraSelectionOverlays()
        {
            base.DrawExtraSelectionOverlays();
            Room room = this.GetRoom(RegionType.Set_All);
            if (room != null && Building_Bed.RoomCanBePrisonCell(room))
            {
                room.DrawFieldEdges();
            }
        }

        // Token: 0x060063B7 RID: 25527 RVA: 0x0021AA85 File Offset: 0x00218C85
        public static bool RoomCanBePrisonCell(Room r)
        {
            return r.ProperRoom && !r.IsHuge;
        }

        // Token: 0x060063B8 RID: 25528 RVA: 0x0021AA9A File Offset: 0x00218C9A
        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo gizmo in base.GetGizmos())
            {
                yield return gizmo;
            }

            IEnumerator<Gizmo> enumerator = null;
            if (base.Faction == Faction.OfPlayer)
            {
                if (this.def.building.bed_humanlike)
                {
                    if (ModsConfig.IdeologyActive)
                    {
                        yield return new Command_SetBedOwnerType(this);
                    }
                    else
                    {
                        Command_Toggle command_Toggle = new Command_Toggle();
                        command_Toggle.defaultLabel = "CommandBedSetForPrisonersLabel".Translate();
                        command_Toggle.defaultDesc = "CommandBedSetForPrisonersDesc".Translate();
                        command_Toggle.icon = ContentFinder<Texture2D>.Get("UI/Commands/ForPrisoners", true);
                        command_Toggle.isActive = (() => this.ForPrisoners);
                        command_Toggle.toggleAction = delegate()
                        {
                            this.SetBedOwnerTypeByInterface(this.ForPrisoners
                                ? BedOwnerType.Colonist
                                : BedOwnerType.Prisoner);
                        };
                        if (!Building_Bed.RoomCanBePrisonCell(this.GetRoom(RegionType.Set_All)) && !this.ForPrisoners)
                        {
                            command_Toggle.Disable("CommandBedSetForPrisonersFailOutdoors".Translate());
                        }

                        command_Toggle.hotKey = KeyBindingDefOf.Misc3;
                        command_Toggle.turnOffSound = null;
                        command_Toggle.turnOnSound = null;
                        yield return command_Toggle;
                    }
                }

                yield return new Command_Toggle
                {
                    defaultLabel = "CommandBedSetAsMedicalLabel".Translate(),
                    defaultDesc = "CommandBedSetAsMedicalDesc".Translate(),
                    icon = ContentFinder<Texture2D>.Get("UI/Commands/AsMedical", true),
                    isActive = (() => this.Medical),
                    toggleAction = delegate() { this.Medical = !this.Medical; },
                    hotKey = KeyBindingDefOf.Misc2
                };
            }

            yield break;
            yield break;
        }

        // Token: 0x060063B9 RID: 25529 RVA: 0x0021AAAC File Offset: 0x00218CAC
        public void SetBedOwnerTypeByInterface(BedOwnerType ownerType)
        {
            if (Building_Bed.lastBedOwnerSetChangeFrame == Time.frameCount)
            {
                return;
            }

            Building_Bed.lastBedOwnerSetChangeFrame = Time.frameCount;
            ((this.ForOwnerType != ownerType) ? SoundDefOf.Checkbox_TurnedOn : SoundDefOf.Checkbox_TurnedOff)
                .PlayOneShotOnCamera(null);
            List<Building_Bed> bedsToAffect = new List<Building_Bed>();
            foreach (Building_Bed building_Bed in Find.Selector.SelectedObjects.OfType<Building_Bed>())
            {
                if (building_Bed.ForOwnerType != ownerType)
                {
                    Room room = building_Bed.GetRoom(RegionType.Set_All);
                    if (room == null && ownerType != BedOwnerType.Prisoner)
                    {
                        if (!bedsToAffect.Contains(building_Bed))
                        {
                            bedsToAffect.Add(building_Bed);
                        }
                    }
                    else
                    {
                        foreach (Building_Bed building_Bed2 in room.ContainedBeds)
                        {
                            if (building_Bed2.ForOwnerType != ownerType)
                            {
                                if (building_Bed2.ForOwnerType == BedOwnerType.Prisoner &&
                                    !bedsToAffect.Contains(building_Bed2))
                                {
                                    bedsToAffect.Add(building_Bed2);
                                }
                                else if (ownerType == BedOwnerType.Prisoner && Building_Bed.RoomCanBePrisonCell(room) &&
                                         !bedsToAffect.Contains(building_Bed2))
                                {
                                    bedsToAffect.Add(building_Bed2);
                                }
                                else if (building_Bed2 == building_Bed && !bedsToAffect.Contains(building_Bed2))
                                {
                                    bedsToAffect.Add(building_Bed2);
                                }
                            }
                        }
                    }
                }
            }

            Action action = delegate()
            {
                List<District> list = new List<District>();
                List<Room> list2 = new List<Room>();
                foreach (Building_Bed building_Bed4 in bedsToAffect)
                {
                    District district = building_Bed4.GetDistrict(RegionType.Set_Passable);
                    Room room2 = district.Room;
                    if (ownerType == BedOwnerType.Prisoner && room2.TouchesMapEdge)
                    {
                        building_Bed4.ForOwnerType = BedOwnerType.Colonist;
                    }
                    else
                    {
                        building_Bed4.ForOwnerType = ownerType;
                    }

                    for (int j = 0; j < this.SleepingSlotsCount; j++)
                    {
                        Pawn curOccupant = this.GetCurOccupant(j);
                        if (curOccupant != null)
                        {
                            curOccupant.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
                        }
                    }

                    if (!room2.TouchesMapEdge)
                    {
                        if (!list2.Contains(room2))
                        {
                            list2.Add(room2);
                        }

                        if (!list.Contains(district))
                        {
                            list.Add(district);
                        }
                    }
                }

                foreach (District district2 in list)
                {
                    district2.Notify_RoomShapeOrContainedBedsChanged();
                }

                foreach (Room room3 in list2)
                {
                    room3.Notify_RoomShapeChanged();
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
            if (!ModsConfig.IdeologyActive)
            {
                if (ownerType == BedOwnerType.Prisoner)
                {
                    stringBuilder.Append("TurningOnPrisonerBedWarning".Translate());
                }
                else
                {
                    stringBuilder.Append("TurningOffPrisonerBedWarning".Translate());
                }
            }
            else
            {
                stringBuilder.Append("ChangingOwnerTypeBedWarning".Translate());
            }

            stringBuilder.AppendLine();
            foreach (Building_Bed building_Bed3 in bedsToAffect)
            {
                if (ownerType != building_Bed3.ForOwnerType)
                {
                    for (int i = 0; i < building_Bed3.OwnersForReading.Count; i++)
                    {
                        stringBuilder.AppendLine();
                        stringBuilder.Append(building_Bed3.OwnersForReading[i].LabelShort);
                    }
                }
            }

            stringBuilder.AppendLine();
            stringBuilder.AppendLine();
            stringBuilder.Append("AreYouSure".Translate());
            Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(stringBuilder.ToString(), action, false, null));
        }

        // Token: 0x060063BA RID: 25530 RVA: 0x0021AE00 File Offset: 0x00219000
        public override string GetInspectString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(base.GetInspectString());
            if (this.def.building.bed_humanlike && base.Faction == Faction.OfPlayer)
            {
                switch (this.ForOwnerType)
                {
                    case BedOwnerType.Colonist:
                        stringBuilder.AppendInNewLine("ForColonistUse".Translate());
                        break;
                    case BedOwnerType.Prisoner:
                        stringBuilder.AppendInNewLine("ForPrisonerUse".Translate());
                        break;
                    case BedOwnerType.Slave:
                        stringBuilder.AppendInNewLine("ForSlaveUse".Translate());
                        break;
                    default:
                        Log.Error(string.Format("Unknown bed owner type: {0}", this.ForOwnerType));
                        break;
                }
            }

            if (this.Medical)
            {
                stringBuilder.AppendInNewLine("MedicalBed".Translate());
                if (base.Spawned)
                {
                    stringBuilder.AppendInNewLine("RoomInfectionChanceFactor".Translate() + ": " +
                                                  this.GetRoom(RegionType.Set_All)
                                                      .GetStat(RoomStatDefOf.InfectionChanceFactor).ToStringPercent());
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

            return stringBuilder.ToString();
        }

        // Token: 0x060063BB RID: 25531 RVA: 0x0021B01A File Offset: 0x0021921A
        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
        {
            if (myPawn.RaceProps.Humanlike && !this.ForPrisoners && this.Medical && !myPawn.Drafted &&
                base.Faction == Faction.OfPlayer && RestUtility.CanUseBedEver(myPawn, this.def))
            {
                if (!HealthAIUtility.ShouldSeekMedicalRest(myPawn) &&
                    !HealthAIUtility.ShouldSeekMedicalRestUrgent(myPawn))
                {
                    yield return new FloatMenuOption(
                        "UseMedicalBed".Translate() + " (" + "NotInjured".Translate() + ")", null,
                        MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
                }
                else
                {
                    Action action = delegate()
                    {
                        if (!this.ForPrisoners && this.Medical && myPawn.CanReserveAndReach(this,
                            PathEndMode.ClosestTouch, Danger.Deadly, this.SleepingSlotsCount, -1, null, true))
                        {
                            if (myPawn.CurJobDef == JobDefOf.LayDown &&
                                myPawn.CurJob.GetTarget(TargetIndex.A).Thing == this)
                            {
                                myPawn.CurJob.restUntilHealed = true;
                            }
                            else
                            {
                                Job job = JobMaker.MakeJob(JobDefOf.LayDown, this);
                                job.restUntilHealed = true;
                                myPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
                            }

                            myPawn.mindState.ResetLastDisturbanceTick();
                        }
                    };
                    yield return FloatMenuUtility.DecoratePrioritizedTask(
                        new FloatMenuOption("UseMedicalBed".Translate(), action, MenuOptionPriority.Default, null, null,
                            0f, null, null, true, 0), myPawn, this,
                        (this.AnyUnoccupiedSleepingSlot ? "ReservedBy" : "SomeoneElseSleeping").CapitalizeFirst());
                }
            }

            yield break;
        }

        // Token: 0x060063BC RID: 25532 RVA: 0x0021B034 File Offset: 0x00219234
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
                        if (!this.OwnersForReading[i].InBed() || this.OwnersForReading[i].CurrentBed() != this ||
                            !(this.OwnersForReading[i].Position == this.GetSleepingSlotPos(i)))
                        {
                            GenMapUI.DrawThingLabel(this.GetMultiOwnersLabelScreenPosFor(i),
                                this.OwnersForReading[i].LabelShort, defaultThingLabelColor);
                        }
                    }
                }
            }
        }

        // Token: 0x060063BD RID: 25533 RVA: 0x0021B15C File Offset: 0x0021935C
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

        // Token: 0x060063BE RID: 25534 RVA: 0x0021B1C4 File Offset: 0x002193C4
        public int GetCurOccupantSlotIndex(Pawn curOccupant)
        {
            for (int i = 0; i < this.SleepingSlotsCount; i++)
            {
                if (this.GetCurOccupant(i) == curOccupant)
                {
                    return i;
                }
            }

            Log.Error("Could not find pawn " + curOccupant + " on any of sleeping slots.");
            return 0;
        }

        // Token: 0x060063BF RID: 25535 RVA: 0x0021B204 File Offset: 0x00219404
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

        // Token: 0x060063C0 RID: 25536 RVA: 0x0021B23A File Offset: 0x0021943A
        public IntVec3 GetSleepingSlotPos(int index)
        {
            return BedUtility.GetSleepingSlotPos(index, base.Position, base.Rotation, this.def.size);
        }

        // Token: 0x060063C1 RID: 25537 RVA: 0x0021B25C File Offset: 0x0021945C
        private void RemoveAllOwners(bool destroyed = false)
        {
            for (int i = this.OwnersForReading.Count - 1; i >= 0; i--)
            {
                Pawn pawn = this.OwnersForReading[i];
                pawn.ownership.UnclaimBed();
                string key = "MessageBedLostAssignment";
                if (destroyed)
                {
                    key = "MessageBedDestroyed";
                }

                Messages.Message(key.Translate(this.def, pawn), new LookTargets(new TargetInfo[]
                {
                    this,
                    pawn
                }), MessageTypeDefOf.CautionInput, false);
            }
        }

        // Token: 0x060063C2 RID: 25538 RVA: 0x0021B2F8 File Offset: 0x002194F8
        private void NotifyRoomBedTypeChanged()
        {
            Room room = this.GetRoom(RegionType.Set_All);
            if (room != null)
            {
                room.Notify_BedTypeChanged();
            }
        }

        // Token: 0x060063C3 RID: 25539 RVA: 0x0021B318 File Offset: 0x00219518
        public void NotifyRoomAssignedPawnsChanged()
        {
            Room room = this.GetRoom(RegionType.Set_All);
            if (room != null)
            {
                room.Notify_BedTypeChanged();
            }
        }

        // Token: 0x060063C4 RID: 25540 RVA: 0x0021B338 File Offset: 0x00219538
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

        // Token: 0x060063C5 RID: 25541 RVA: 0x0021B368 File Offset: 0x00219568
        private Vector3 GetMultiOwnersLabelScreenPosFor(int slotIndex)
        {
            IntVec3 sleepingSlotPos = this.GetSleepingSlotPos(slotIndex);
            Vector3 drawPos = this.DrawPos;
            if (base.Rotation.IsHorizontal)
            {
                drawPos.z = (float) sleepingSlotPos.z + 0.6f;
            }
            else
            {
                drawPos.x = (float) sleepingSlotPos.x + 0.5f;
                drawPos.z += -0.4f;
            }

            Vector2 v = drawPos.MapToUIPosition();
            if (!base.Rotation.IsHorizontal && this.SleepingSlotsCount == 2)
            {
                v = this.AdjustOwnerLabelPosToAvoidOverlapping(v, slotIndex);
            }

            return v;
        }

        // Token: 0x060063C6 RID: 25542 RVA: 0x0021B408 File Offset: 0x00219608
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
                    num3 = (float) this.GetSleepingSlotPos(1).x;
                }
                else
                {
                    num3 = (float) this.GetSleepingSlotPos(0).x;
                }

                if ((float) sleepingSlotPos.x < num3)
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

        // Token: 0x0400386E RID: 14446
        private BedOwnerType forOwnerType;

        // Token: 0x0400386F RID: 14447
        private bool medicalInt;

        // Token: 0x04003870 RID: 14448
        private bool alreadySetDefaultMed;

        // Token: 0x04003871 RID: 14449
        private static int lastBedOwnerSetChangeFrame = -1;

        // Token: 0x04003872 RID: 14450
        private static readonly Color SheetColorNormal = new Color(0.6313726f, 0.8352941f, 0.7058824f);

        // Token: 0x04003873 RID: 14451
        private static readonly Color SheetColorRoyal = new Color(0.67058825f, 0.9137255f, 0.74509805f);

        // Token: 0x04003874 RID: 14452
        public static readonly Color SheetColorForPrisoner = new Color(1f, 0.7176471f, 0.12941177f);

        // Token: 0x04003875 RID: 14453
        private static readonly Color SheetColorMedical = new Color(0.3882353f, 0.62352943f, 0.8862745f);

        // Token: 0x04003876 RID: 14454
        private static readonly Color SheetColorMedicalForPrisoner = new Color(0.654902f, 0.3764706f, 0.15294118f);

        // Token: 0x04003877 RID: 14455
        private static readonly Color SheetColorForSlave = new Color32(252, 244, 3, byte.MaxValue);

        // Token: 0x04003878 RID: 14456
        private static readonly Color SheetColorMedicalForSlave = new Color32(153, 148, 0, byte.MaxValue);
    }
}