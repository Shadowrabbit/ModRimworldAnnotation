using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using Verse.AI.Group;
using Verse.Profile;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200056B RID: 1387
	public static class DebugActionsMisc
	{
		// Token: 0x0600233E RID: 9022 RVA: 0x0010CC88 File Offset: 0x0010AE88
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DestroyAllPlants()
		{
			foreach (Thing thing in Find.CurrentMap.listerThings.AllThings.ToList<Thing>())
			{
				if (thing is Plant)
				{
					thing.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x0600233F RID: 9023 RVA: 0x0010CCF4 File Offset: 0x0010AEF4
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DestroyAllThings()
		{
			foreach (Thing thing in Find.CurrentMap.listerThings.AllThings.ToList<Thing>())
			{
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06002340 RID: 9024 RVA: 0x0010CD54 File Offset: 0x0010AF54
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DestroyClutter()
		{
			foreach (Thing thing in Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.Chunk).ToList<Thing>())
			{
				thing.Destroy(DestroyMode.Vanish);
			}
			foreach (Thing thing2 in Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.Filth).ToList<Thing>())
			{
				thing2.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06002341 RID: 9025 RVA: 0x0001DFF4 File Offset: 0x0001C1F4
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FinishAllResearch()
		{
			Find.ResearchManager.DebugSetAllProjectsFinished();
			Messages.Message("All research finished.", MessageTypeDefOf.TaskCompletion, false);
		}

		// Token: 0x06002342 RID: 9026 RVA: 0x0010CE08 File Offset: 0x0010B008
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ReplaceAllTradeShips()
		{
			Find.CurrentMap.passingShipManager.DebugSendAllShipsAway();
			for (int i = 0; i < 5; i++)
			{
				IncidentParms incidentParms = new IncidentParms();
				incidentParms.target = Find.CurrentMap;
				IncidentDefOf.OrbitalTraderArrival.Worker.TryExecute(incidentParms);
			}
		}

		// Token: 0x06002343 RID: 9027 RVA: 0x0010CE54 File Offset: 0x0010B054
		[DebugAction("General", "Change weather...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ChangeWeather()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (WeatherDef localWeather2 in DefDatabase<WeatherDef>.AllDefs)
			{
				WeatherDef localWeather = localWeather2;
				list.Add(new DebugMenuOption(localWeather.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					Find.CurrentMap.weatherManager.TransitionTo(localWeather);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06002344 RID: 9028 RVA: 0x0010CEE4 File Offset: 0x0010B0E4
		[DebugAction("General", "Play song...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void PlaySong()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (SongDef localSong2 in DefDatabase<SongDef>.AllDefs)
			{
				SongDef localSong = localSong2;
				list.Add(new DebugMenuOption(localSong.defName, DebugMenuOptionMode.Action, delegate()
				{
					Find.MusicManagerPlay.ForceStartSong(localSong, false);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06002345 RID: 9029 RVA: 0x0010CF70 File Offset: 0x0010B170
		[DebugAction("General", "Play sound...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void PlaySound()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (SoundDef localSd2 in from s in DefDatabase<SoundDef>.AllDefs
			where !s.sustain
			select s)
			{
				SoundDef localSd = localSd2;
				list.Add(new DebugMenuOption(localSd.defName, DebugMenuOptionMode.Action, delegate()
				{
					if (localSd.subSounds.Any((SubSoundDef sub) => sub.onCamera))
					{
						localSd.PlayOneShotOnCamera(null);
						return;
					}
					localSd.PlayOneShot(SoundInfo.InMap(new TargetInfo(Find.CameraDriver.MapPosition, Find.CurrentMap, false), MaintenanceType.None));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06002346 RID: 9030 RVA: 0x0010D020 File Offset: 0x0010B220
		[DebugAction("General", "End game condition...", allowedGameStates = (AllowedGameStates.Playing | AllowedGameStates.IsCurrentlyOnMap | AllowedGameStates.HasGameCondition))]
		private static void EndGameCondition()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (GameCondition localMc2 in Find.CurrentMap.gameConditionManager.ActiveConditions)
			{
				GameCondition localMc = localMc2;
				list.Add(new DebugMenuOption(localMc.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					localMc.End();
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06002347 RID: 9031 RVA: 0x0001E010 File Offset: 0x0001C210
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddPrisoner()
		{
			DebugActionsMisc.AddGuest(true);
		}

		// Token: 0x06002348 RID: 9032 RVA: 0x0001E018 File Offset: 0x0001C218
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddGuest()
		{
			DebugActionsMisc.AddGuest(false);
		}

		// Token: 0x06002349 RID: 9033 RVA: 0x0010D0BC File Offset: 0x0010B2BC
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceEnemyAssault()
		{
			foreach (Lord lord in Find.CurrentMap.lordManager.lords)
			{
				LordToil_Stage lordToil_Stage = lord.CurLordToil as LordToil_Stage;
				if (lordToil_Stage != null)
				{
					foreach (Transition transition in lord.Graph.transitions)
					{
						if (transition.sources.Contains(lordToil_Stage) && transition.target is LordToil_AssaultColony)
						{
							Messages.Message("Debug forcing to assault toil: " + lord.faction, MessageTypeDefOf.TaskCompletion, false);
							lord.GotoToil(transition.target);
							return;
						}
					}
				}
			}
		}

		// Token: 0x0600234A RID: 9034 RVA: 0x0010D1B0 File Offset: 0x0010B3B0
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceEnemyFlee()
		{
			foreach (Lord lord in Find.CurrentMap.lordManager.lords)
			{
				if (lord.faction != null && lord.faction.HostileTo(Faction.OfPlayer) && lord.faction.def.autoFlee)
				{
					LordToil lordToil = lord.Graph.lordToils.FirstOrDefault((LordToil st) => st is LordToil_PanicFlee);
					if (lordToil != null)
					{
						lord.GotoToil(lordToil);
					}
				}
			}
		}

		// Token: 0x0600234B RID: 9035 RVA: 0x0001E020 File Offset: 0x0001C220
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AdaptionProgress10Days()
		{
			Find.StoryWatcher.watcherAdaptation.Debug_OffsetAdaptDays(10f);
		}

		// Token: 0x0600234C RID: 9036 RVA: 0x0001E036 File Offset: 0x0001C236
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void UnloadUnusedAssets()
		{
			MemoryUtility.UnloadUnusedUnityAssets();
		}

		// Token: 0x0600234D RID: 9037 RVA: 0x0010D26C File Offset: 0x0010B46C
		[DebugAction("General", "Name settlement...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void NameSettlement()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("Faction", DebugMenuOptionMode.Action, delegate()
			{
				Find.WindowStack.Add(new Dialog_NamePlayerFaction());
			}));
			if (Find.CurrentMap != null && Find.CurrentMap.IsPlayerHome && Find.CurrentMap.Parent is Settlement)
			{
				Settlement factionBase = (Settlement)Find.CurrentMap.Parent;
				list.Add(new DebugMenuOption("Faction base", DebugMenuOptionMode.Action, delegate()
				{
					Find.WindowStack.Add(new Dialog_NamePlayerSettlement(factionBase));
				}));
				list.Add(new DebugMenuOption("Faction and faction base", DebugMenuOptionMode.Action, delegate()
				{
					Find.WindowStack.Add(new Dialog_NamePlayerFactionAndSettlement(factionBase));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x0600234E RID: 9038 RVA: 0x0001E03D File Offset: 0x0001C23D
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void NextLesson()
		{
			LessonAutoActivator.DebugForceInitiateBestLessonNow();
		}

		// Token: 0x0600234F RID: 9039 RVA: 0x0001E044 File Offset: 0x0001C244
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RegenAllMapMeshSections()
		{
			Find.CurrentMap.mapDrawer.RegenerateEverythingNow();
		}

		// Token: 0x06002350 RID: 9040 RVA: 0x0010D338 File Offset: 0x0010B538
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ChangeCameraConfig()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Type localType2 in typeof(CameraMapConfig).AllSubclasses())
			{
				Type localType = localType2;
				string text = localType.Name;
				if (text.StartsWith("CameraMapConfig_"))
				{
					text = text.Substring("CameraMapConfig_".Length);
				}
				list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
				{
					Find.CameraDriver.config = (CameraMapConfig)Activator.CreateInstance(localType);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06002351 RID: 9041 RVA: 0x0001E055 File Offset: 0x0001C255
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceShipCountdown()
		{
			ShipCountdown.InitiateCountdown(null);
		}

		// Token: 0x06002352 RID: 9042 RVA: 0x0010D3F4 File Offset: 0x0010B5F4
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceStartShip()
		{
			Map currentMap = Find.CurrentMap;
			if (currentMap == null)
			{
				return;
			}
			Building_ShipComputerCore building_ShipComputerCore = (Building_ShipComputerCore)currentMap.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.Ship_ComputerCore).FirstOrDefault<Building>();
			if (building_ShipComputerCore == null)
			{
				Messages.Message("Could not find any compute core on current map!", MessageTypeDefOf.NeutralEvent, true);
			}
			building_ShipComputerCore.ForceLaunch();
		}

		// Token: 0x06002353 RID: 9043 RVA: 0x0010D440 File Offset: 0x0010B640
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FlashTradeDropSpot()
		{
			IntVec3 intVec = DropCellFinder.TradeDropSpot(Find.CurrentMap);
			Find.CurrentMap.debugDrawer.FlashCell(intVec, 0f, null, 50);
			Log.Message("trade drop spot: " + intVec, false);
		}

		// Token: 0x06002354 RID: 9044 RVA: 0x0010D488 File Offset: 0x0010B688
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void KillFactionLeader()
		{
			Pawn leader = (from x in Find.FactionManager.AllFactions
			where x.leader != null
			select x).RandomElement<Faction>().leader;
			int num = 0;
			while (!leader.Dead)
			{
				if (++num > 1000)
				{
					Log.Warning("Could not kill faction leader.", false);
					return;
				}
				DamageInfo dinfo = new DamageInfo(DamageDefOf.Bullet, 30f, 999f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
				dinfo.SetIgnoreInstantKillProtection(true);
				leader.TakeDamage(dinfo);
			}
		}

		// Token: 0x06002355 RID: 9045 RVA: 0x0010D524 File Offset: 0x0010B724
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void KillKidnappedPawn()
		{
			IEnumerable<Pawn> pawnsBySituation = Find.WorldPawns.GetPawnsBySituation(WorldPawnSituation.Kidnapped);
			if (pawnsBySituation.Any<Pawn>())
			{
				Pawn pawn = pawnsBySituation.RandomElement<Pawn>();
				pawn.Kill(null, null);
				Messages.Message("Killed " + pawn.LabelCap, MessageTypeDefOf.NeutralEvent, false);
			}
		}

		// Token: 0x06002356 RID: 9046 RVA: 0x0010D578 File Offset: 0x0010B778
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SetFactionRelations()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (Faction localFac2 in Find.FactionManager.AllFactionsVisibleInViewOrder)
			{
				Faction localFac = localFac2;
				foreach (object obj in Enum.GetValues(typeof(FactionRelationKind)))
				{
					FactionRelationKind localRk2 = (FactionRelationKind)obj;
					FactionRelationKind localRk = localRk2;
					FloatMenuOption item = new FloatMenuOption(localFac + " - " + localRk, delegate()
					{
						localFac.TrySetRelationKind(Faction.OfPlayer, localRk, true, null, null);
					}, MenuOptionPriority.Default, null, null, 0f, null, null);
					list.Add(item);
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06002357 RID: 9047 RVA: 0x0010D6A0 File Offset: 0x0010B8A0
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void VisitorGift()
		{
			List<Pawn> list = new List<Pawn>();
			foreach (Pawn pawn in Find.CurrentMap.mapPawns.AllPawnsSpawned)
			{
				if (pawn.Faction != null && !pawn.Faction.IsPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer))
				{
					list.Add(pawn);
					break;
				}
			}
			VisitorGiftForPlayerUtility.GiveGift(list, list[0].Faction);
		}

		// Token: 0x06002358 RID: 9048 RVA: 0x0001E05D File Offset: 0x0001C25D
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RefogMap()
		{
			FloodFillerFog.DebugRefogMap(Find.CurrentMap);
		}

		// Token: 0x06002359 RID: 9049 RVA: 0x0010D740 File Offset: 0x0010B940
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void UseGenStep()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Type localGenStep2 in typeof(GenStep).AllSubclassesNonAbstract())
			{
				Type localGenStep = localGenStep2;
				list.Add(new DebugMenuOption(localGenStep.Name, DebugMenuOptionMode.Action, delegate()
				{
					((GenStep)Activator.CreateInstance(localGenStep)).Generate(Find.CurrentMap, default(GenStepParams));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x0600235A RID: 9050 RVA: 0x0001E069 File Offset: 0x0001C269
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void IncrementTime1Hour()
		{
			Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 2500);
		}

		// Token: 0x0600235B RID: 9051 RVA: 0x0001E085 File Offset: 0x0001C285
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void IncrementTime6Hours()
		{
			Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 15000);
		}

		// Token: 0x0600235C RID: 9052 RVA: 0x0001E0A1 File Offset: 0x0001C2A1
		[DebugAction("General", "Increment time 1 day", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void IncrementTime1Day()
		{
			Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 60000);
		}

		// Token: 0x0600235D RID: 9053 RVA: 0x0001E0BD File Offset: 0x0001C2BD
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void IncrementTime1Season()
		{
			Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 900000);
		}

		// Token: 0x0600235E RID: 9054 RVA: 0x0010D7D8 File Offset: 0x0010B9D8
		[DebugAction("General", "Storywatcher tick 1 day", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void StorywatcherTick1Day()
		{
			for (int i = 0; i < 60000; i++)
			{
				Find.StoryWatcher.StoryWatcherTick();
				Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 1);
			}
		}

		// Token: 0x0600235F RID: 9055 RVA: 0x0010D818 File Offset: 0x0010BA18
		[DebugAction("General", "Add techprint to project", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddTechprintsForProject()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ResearchProjectDef localProject2 in from p in DefDatabase<ResearchProjectDef>.AllDefsListForReading
			where !p.TechprintRequirementMet
			select p)
			{
				ResearchProjectDef localProject = localProject2;
				list.Add(new DebugMenuOption(localProject.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					Find.ResearchManager.AddTechprints(localProject, localProject.TechprintCount - Find.ResearchManager.GetTechprints(localProject));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06002360 RID: 9056 RVA: 0x0010D8CC File Offset: 0x0010BACC
		[DebugAction("General", "Apply techprint on project", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ApplyTechprintsForProject()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ResearchProjectDef localProject2 in from p in DefDatabase<ResearchProjectDef>.AllDefsListForReading
			where !p.TechprintRequirementMet
			select p)
			{
				ResearchProjectDef localProject = localProject2;
				Action <>9__2;
				list.Add(new DebugMenuOption(localProject.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					List<DebugMenuOption> list3 = list2;
					string label = "None";
					DebugMenuOptionMode mode = DebugMenuOptionMode.Action;
					Action method;
					if ((method = <>9__2) == null)
					{
						method = (<>9__2 = delegate()
						{
							Find.ResearchManager.ApplyTechprint(localProject, null);
						});
					}
					list3.Add(new DebugMenuOption(label, mode, method));
					foreach (Pawn localColonist2 in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
					{
						Pawn localColonist = localColonist2;
						list2.Add(new DebugMenuOption(localColonist.LabelCap, DebugMenuOptionMode.Action, delegate()
						{
							Find.ResearchManager.ApplyTechprint(localProject, localColonist);
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06002361 RID: 9057 RVA: 0x0010D980 File Offset: 0x0010BB80
		private static void AddGuest(bool prisoner)
		{
			foreach (Building_Bed building_Bed in Find.CurrentMap.listerBuildings.AllBuildingsColonistOfClass<Building_Bed>())
			{
				if (building_Bed.ForPrisoners == prisoner && (!building_Bed.OwnersForReading.Any<Pawn>() || (prisoner && building_Bed.AnyUnownedSleepingSlot)))
				{
					PawnKindDef pawnKindDef;
					if (!prisoner)
					{
						pawnKindDef = PawnKindDefOf.SpaceRefugee;
					}
					else
					{
						pawnKindDef = (from pk in DefDatabase<PawnKindDef>.AllDefs
						where pk.defaultFactionType != null && !pk.defaultFactionType.isPlayer && pk.RaceProps.Humanlike
						select pk).RandomElement<PawnKindDef>();
					}
					Faction faction = FactionUtility.DefaultFactionFrom(pawnKindDef.defaultFactionType);
					Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, faction);
					GenSpawn.Spawn(pawn, building_Bed.Position, Find.CurrentMap, WipeMode.Vanish);
					foreach (ThingWithComps eq in pawn.equipment.AllEquipmentListForReading.ToList<ThingWithComps>())
					{
						ThingWithComps thingWithComps;
						if (pawn.equipment.TryDropEquipment(eq, out thingWithComps, pawn.Position, true))
						{
							thingWithComps.Destroy(DestroyMode.Vanish);
						}
					}
					pawn.inventory.innerContainer.Clear();
					pawn.ownership.ClaimBedIfNonMedical(building_Bed);
					pawn.guest.SetGuestStatus(Faction.OfPlayer, prisoner);
					break;
				}
			}
		}

		// Token: 0x06002362 RID: 9058 RVA: 0x0010DB18 File Offset: 0x0010BD18
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void KillRandomLentColonist()
		{
			if (QuestUtility.TotalBorrowedColonistCount() > 0)
			{
				DebugActionsMisc.tmpLentColonists.Clear();
				List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
				for (int i = 0; i < questsListForReading.Count; i++)
				{
					if (questsListForReading[i].State == QuestState.Ongoing)
					{
						List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
						for (int j = 0; j < partsListForReading.Count; j++)
						{
							QuestPart_LendColonistsToFaction questPart_LendColonistsToFaction;
							if ((questPart_LendColonistsToFaction = (partsListForReading[j] as QuestPart_LendColonistsToFaction)) != null)
							{
								List<Thing> lentColonistsListForReading = questPart_LendColonistsToFaction.LentColonistsListForReading;
								for (int k = 0; k < lentColonistsListForReading.Count; k++)
								{
									Pawn pawn;
									if ((pawn = (lentColonistsListForReading[k] as Pawn)) != null && !pawn.Dead)
									{
										DebugActionsMisc.tmpLentColonists.Add(pawn);
									}
								}
							}
						}
					}
				}
				Pawn pawn2 = DebugActionsMisc.tmpLentColonists.RandomElement<Pawn>();
				bool flag = pawn2.health.hediffSet.hediffs.Any((Hediff x) => x.def.isBad);
				pawn2.Kill(null, flag ? pawn2.health.hediffSet.hediffs.RandomElement<Hediff>() : null);
			}
		}

		// Token: 0x06002363 RID: 9059 RVA: 0x0010DC54 File Offset: 0x0010BE54
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DestroyAllHats()
		{
			foreach (Pawn pawn in PawnsFinder.AllMaps)
			{
				if (pawn.RaceProps.Humanlike)
				{
					for (int i = pawn.apparel.WornApparel.Count - 1; i >= 0; i--)
					{
						Apparel apparel = pawn.apparel.WornApparel[i];
						if (apparel.def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.FullHead) || apparel.def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.UpperHead))
						{
							apparel.Destroy(DestroyMode.Vanish);
						}
					}
				}
			}
		}

		// Token: 0x06002364 RID: 9060 RVA: 0x0010DD20 File Offset: 0x0010BF20
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void PawnKindApparelCheck()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (PawnKindDef localKindDef2 in from kd in DefDatabase<PawnKindDef>.AllDefs
			where kd.race == ThingDefOf.Human
			orderby kd.defName
			select kd)
			{
				PawnKindDef localKindDef = localKindDef2;
				list.Add(new DebugMenuOption(localKindDef.defName, DebugMenuOptionMode.Action, delegate()
				{
					Faction faction = FactionUtility.DefaultFactionFrom(localKindDef.defaultFactionType);
					bool flag = false;
					for (int i = 0; i < 100; i++)
					{
						Pawn pawn = PawnGenerator.GeneratePawn(localKindDef, faction);
						if (pawn.royalty != null)
						{
							RoyalTitle mostSeniorTitle = pawn.royalty.MostSeniorTitle;
							if (mostSeniorTitle != null && !mostSeniorTitle.def.requiredApparel.NullOrEmpty<RoyalTitleDef.ApparelRequirement>())
							{
								for (int j = 0; j < mostSeniorTitle.def.requiredApparel.Count; j++)
								{
									if (!mostSeniorTitle.def.requiredApparel[j].IsMet(pawn))
									{
										Log.Error(string.Concat(new object[]
										{
											localKindDef,
											" (",
											mostSeniorTitle.def.label,
											")  does not have its title requirements met. index=",
											j,
											DebugActionsMisc.<PawnKindApparelCheck>g__logApparel|42_0(pawn)
										}), false);
										flag = true;
									}
								}
							}
						}
						List<Apparel> wornApparel = pawn.apparel.WornApparel;
						for (int k = 0; k < wornApparel.Count; k++)
						{
							string text = DebugActionsMisc.<PawnKindApparelCheck>g__apparelOkayToWear|42_1(pawn, wornApparel[k]);
							if (text != "OK")
							{
								Log.Error(text + " - " + wornApparel[k].Label + DebugActionsMisc.<PawnKindApparelCheck>g__logApparel|42_0(pawn), false);
								flag = true;
							}
						}
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
					}
					if (!flag)
					{
						Log.Message("No errors for " + localKindDef.defName, false);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06002365 RID: 9061 RVA: 0x0010DDF4 File Offset: 0x0010BFF4
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void PawnKindAbilityCheck()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			StringBuilder sb = new StringBuilder();
			foreach (PawnKindDef localKindDef2 in from kd in DefDatabase<PawnKindDef>.AllDefs
			where kd.titleRequired != null || !kd.titleSelectOne.NullOrEmpty<RoyalTitleDef>()
			orderby kd.defName
			select kd)
			{
				PawnKindDef localKindDef = localKindDef2;
				list.Add(new DebugMenuOption(localKindDef.defName, DebugMenuOptionMode.Action, delegate()
				{
					Faction faction = FactionUtility.DefaultFactionFrom(localKindDef.defaultFactionType);
					for (int i = 0; i < 100; i++)
					{
						RoyalTitleDef fixedTitle = null;
						if (localKindDef.titleRequired != null)
						{
							fixedTitle = localKindDef.titleRequired;
						}
						else if (!localKindDef.titleSelectOne.NullOrEmpty<RoyalTitleDef>() && Rand.Chance(localKindDef.royalTitleChance))
						{
							fixedTitle = localKindDef.titleSelectOne.RandomElementByWeight((RoyalTitleDef t) => t.commonality);
						}
						Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(localKindDef, faction, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, fixedTitle));
						RoyalTitle mostSeniorTitle = pawn.royalty.MostSeniorTitle;
						if (mostSeniorTitle != null)
						{
							Hediff_Psylink mainPsylinkSource = pawn.GetMainPsylinkSource();
							if (mainPsylinkSource == null)
							{
								if (mostSeniorTitle.def.MaxAllowedPsylinkLevel(faction.def) > 0)
								{
									string text = mostSeniorTitle.def.LabelCap + " - No psylink.";
									if (pawn.abilities.abilities.Any((Ability x) => x.def.level > 0))
									{
										text += " Has psycasts without psylink.";
									}
									sb.AppendLine(text);
								}
							}
							else if (mainPsylinkSource.level < mostSeniorTitle.def.MaxAllowedPsylinkLevel(faction.def))
							{
								sb.AppendLine(string.Concat(new object[]
								{
									"Psylink at level ",
									mainPsylinkSource.level,
									", but requires ",
									mostSeniorTitle.def.MaxAllowedPsylinkLevel(faction.def)
								}));
							}
							else if (mainPsylinkSource.level > mostSeniorTitle.def.MaxAllowedPsylinkLevel(faction.def))
							{
								sb.AppendLine(string.Concat(new object[]
								{
									"Psylink at level ",
									mainPsylinkSource.level,
									". Max is ",
									mostSeniorTitle.def.MaxAllowedPsylinkLevel(faction.def)
								}));
							}
						}
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
					}
					if (sb.Length == 0)
					{
						Log.Message("No errors for " + localKindDef.defName, false);
						return;
					}
					Log.Error("Errors:\n" + sb.ToString(), false);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06002367 RID: 9063 RVA: 0x0010DEE4 File Offset: 0x0010C0E4
		[CompilerGenerated]
		internal static string <PawnKindApparelCheck>g__logApparel|42_0(Pawn p)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(string.Format("Apparel of {0}:", p.LabelShort));
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				stringBuilder.AppendLine("  - " + wornApparel[i].Label);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002368 RID: 9064 RVA: 0x0010DF58 File Offset: 0x0010C158
		[CompilerGenerated]
		internal static string <PawnKindApparelCheck>g__apparelOkayToWear|42_1(Pawn pawn, Apparel apparel)
		{
			ApparelProperties app = apparel.def.apparel;
			if (!pawn.kindDef.apparelRequired.NullOrEmpty<ThingDef>() && pawn.kindDef.apparelRequired.Contains(apparel.def))
			{
				return "OK";
			}
			if (!app.CorrectGenderForWearing(pawn.gender))
			{
				return "Wrong gender.";
			}
			List<SpecificApparelRequirement> specificApparelRequirements = pawn.kindDef.specificApparelRequirements;
			if (specificApparelRequirements != null)
			{
				for (int i = 0; i < specificApparelRequirements.Count; i++)
				{
					if (PawnApparelGenerator.ApparelRequirementHandlesThing(specificApparelRequirements[i], apparel.def) && PawnApparelGenerator.ApparelRequirementTagsMatch(specificApparelRequirements[i], apparel.def))
					{
						return "OK";
					}
				}
			}
			if (!pawn.kindDef.apparelTags.NullOrEmpty<string>())
			{
				if (!app.tags.Any((string tag) => pawn.kindDef.apparelTags.Contains(tag)))
				{
					return "Required tag missing.";
				}
				if ((pawn.royalty == null || pawn.royalty.MostSeniorTitle == null) && app.tags.Contains("Royal") && !pawn.kindDef.apparelTags.Any((string tag) => app.tags.Contains(tag)))
				{
					return "Royal apparel on non-royal pawn.";
				}
			}
			if (!pawn.kindDef.apparelDisallowTags.NullOrEmpty<string>() && pawn.kindDef.apparelDisallowTags.Any((string t) => app.tags.Contains(t)))
			{
				return "Has a disallowed tag.";
			}
			if (pawn.royalty != null && pawn.royalty.AllTitlesInEffectForReading.Any<RoyalTitle>())
			{
				RoyalTitle mostSeniorTitle = pawn.royalty.MostSeniorTitle;
				QualityCategory qualityCategory;
				if (apparel.TryGetQuality(out qualityCategory) && qualityCategory < mostSeniorTitle.def.requiredMinimumApparelQuality)
				{
					return "Quality too low.";
				}
			}
			return "OK";
		}

		// Token: 0x040017D4 RID: 6100
		private static List<Pawn> tmpLentColonists = new List<Pawn>();

		// Token: 0x040017D5 RID: 6101
		private const string NoErrorString = "OK";

		// Token: 0x040017D6 RID: 6102
		private const string RoyalApparelTag = "Royal";

		// Token: 0x040017D7 RID: 6103
		private const int PawnsToGenerate = 100;
	}
}
