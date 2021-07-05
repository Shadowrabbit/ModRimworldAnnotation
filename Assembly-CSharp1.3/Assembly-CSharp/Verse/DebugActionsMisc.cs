using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI.Group;
using Verse.Profile;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200039C RID: 924
	public static class DebugActionsMisc
	{
		// Token: 0x06001B53 RID: 6995 RVA: 0x0009EB30 File Offset: 0x0009CD30
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

		// Token: 0x06001B54 RID: 6996 RVA: 0x0009EB9C File Offset: 0x0009CD9C
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DestroyAllThings()
		{
			foreach (Thing thing in Find.CurrentMap.listerThings.AllThings.ToList<Thing>())
			{
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06001B55 RID: 6997 RVA: 0x0009EBFC File Offset: 0x0009CDFC
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

		// Token: 0x06001B56 RID: 6998 RVA: 0x0009ECB0 File Offset: 0x0009CEB0
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FinishAllResearch()
		{
			Find.ResearchManager.DebugSetAllProjectsFinished();
			Messages.Message("All research finished.", MessageTypeDefOf.TaskCompletion, false);
		}

		// Token: 0x06001B57 RID: 6999 RVA: 0x0009ECCC File Offset: 0x0009CECC
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

		// Token: 0x06001B58 RID: 7000 RVA: 0x0009ED18 File Offset: 0x0009CF18
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddTradeShipOfKind()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<TraderKindDef> enumerator = (from t in DefDatabase<TraderKindDef>.AllDefs
			where t.orbital
			select t).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TraderKindDef traderKind = enumerator.Current;
					list.Add(new DebugMenuOption(traderKind.label, DebugMenuOptionMode.Action, delegate()
					{
						Find.CurrentMap.passingShipManager.DebugSendAllShipsAway();
						IncidentParms incidentParms = new IncidentParms();
						incidentParms.target = Find.CurrentMap;
						incidentParms.traderKind = traderKind;
						IncidentDefOf.OrbitalTraderArrival.Worker.TryExecute(incidentParms);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001B59 RID: 7001 RVA: 0x0009EDC8 File Offset: 0x0009CFC8
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

		// Token: 0x06001B5A RID: 7002 RVA: 0x0009EE58 File Offset: 0x0009D058
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

		// Token: 0x06001B5B RID: 7003 RVA: 0x0009EEE4 File Offset: 0x0009D0E4
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

		// Token: 0x06001B5C RID: 7004 RVA: 0x0009EF94 File Offset: 0x0009D194
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

		// Token: 0x06001B5D RID: 7005 RVA: 0x0009F030 File Offset: 0x0009D230
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddPrisoner()
		{
			DebugActionsMisc.AddGuest(GuestStatus.Prisoner);
		}

		// Token: 0x06001B5E RID: 7006 RVA: 0x0009F038 File Offset: 0x0009D238
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddGuest()
		{
			DebugActionsMisc.AddGuest(GuestStatus.Guest);
		}

		// Token: 0x06001B5F RID: 7007 RVA: 0x0009F040 File Offset: 0x0009D240
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

		// Token: 0x06001B60 RID: 7008 RVA: 0x0009F134 File Offset: 0x0009D334
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

		// Token: 0x06001B61 RID: 7009 RVA: 0x0009F1F0 File Offset: 0x0009D3F0
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AdaptionProgress10Days()
		{
			Find.StoryWatcher.watcherAdaptation.Debug_OffsetAdaptDays(10f);
		}

		// Token: 0x06001B62 RID: 7010 RVA: 0x0009F206 File Offset: 0x0009D406
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void UnloadUnusedAssets()
		{
			MemoryUtility.UnloadUnusedUnityAssets();
		}

		// Token: 0x06001B63 RID: 7011 RVA: 0x0009F210 File Offset: 0x0009D410
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

		// Token: 0x06001B64 RID: 7012 RVA: 0x0009F2DC File Offset: 0x0009D4DC
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void NextLesson()
		{
			LessonAutoActivator.DebugForceInitiateBestLessonNow();
		}

		// Token: 0x06001B65 RID: 7013 RVA: 0x0009F2E3 File Offset: 0x0009D4E3
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RegenAllMapMeshSections()
		{
			Find.CurrentMap.mapDrawer.RegenerateEverythingNow();
		}

		// Token: 0x06001B66 RID: 7014 RVA: 0x0009F2F4 File Offset: 0x0009D4F4
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

		// Token: 0x06001B67 RID: 7015 RVA: 0x0009F3B4 File Offset: 0x0009D5B4
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceShipCountdown()
		{
			ShipCountdown.InitiateCountdown(null);
		}

		// Token: 0x06001B68 RID: 7016 RVA: 0x0009F3BC File Offset: 0x0009D5BC
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

		// Token: 0x06001B69 RID: 7017 RVA: 0x0009F408 File Offset: 0x0009D608
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FlashTradeDropSpot()
		{
			IntVec3 intVec = DropCellFinder.TradeDropSpot(Find.CurrentMap);
			Find.CurrentMap.debugDrawer.FlashCell(intVec, 0f, null, 50);
			Log.Message("trade drop spot: " + intVec);
		}

		// Token: 0x06001B6A RID: 7018 RVA: 0x0009F450 File Offset: 0x0009D650
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MakeFactionLeader(Pawn p)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<Faction> enumerator = Find.FactionManager.AllFactionsVisible.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Faction faction = enumerator.Current;
					list.Add(new DebugMenuOption(faction.Name, DebugMenuOptionMode.Action, delegate()
					{
						if (faction.leader != p)
						{
							faction.leader = p;
							if (ModsConfig.IdeologyActive)
							{
								using (List<Precept>.Enumerator enumerator2 = faction.ideos.PrimaryIdeo.PreceptsListForReading.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										Precept_Role precept_Role;
										if ((precept_Role = (enumerator2.Current as Precept_Role)) != null && precept_Role.def.leaderRole)
										{
											precept_Role.Assign(p, false);
											break;
										}
									}
								}
							}
							DebugActionsUtility.DustPuffFrom(p);
						}
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001B6B RID: 7019 RVA: 0x0009F4F4 File Offset: 0x0009D6F4
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
					Log.Warning("Could not kill faction leader.");
					return;
				}
				DamageInfo dinfo = new DamageInfo(DamageDefOf.Bullet, 30f, 999f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true);
				dinfo.SetIgnoreInstantKillProtection(true);
				leader.TakeDamage(dinfo);
			}
		}

		// Token: 0x06001B6C RID: 7020 RVA: 0x0009F590 File Offset: 0x0009D790
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

		// Token: 0x06001B6D RID: 7021 RVA: 0x0009F5E4 File Offset: 0x0009D7E4
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void KillWorldPawn()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Pawn pawn in Find.WorldPawns.AllPawnsAlive)
			{
				Pawn pLocal = pawn;
				list.Add(new DebugMenuOption(pawn.LabelShort, DebugMenuOptionMode.Action, delegate()
				{
					pLocal.Kill(null, null);
					Messages.Message("Killed " + pLocal.LabelCap, MessageTypeDefOf.NeutralEvent, false);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001B6E RID: 7022 RVA: 0x0009F678 File Offset: 0x0009D878
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
						if (localRk == FactionRelationKind.Hostile)
						{
							Faction.OfPlayer.TryAffectGoodwillWith(localFac, -100, true, true, HistoryEventDefOf.DebugGoodwill, null);
							return;
						}
						if (localRk == FactionRelationKind.Ally)
						{
							Faction.OfPlayer.TryAffectGoodwillWith(localFac, 100, true, true, HistoryEventDefOf.DebugGoodwill, null);
						}
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					list.Add(item);
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06001B6F RID: 7023 RVA: 0x0009F7A0 File Offset: 0x0009D9A0
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
			VisitorGiftForPlayerUtility.GiveRandomGift(list, list[0].Faction);
		}

		// Token: 0x06001B70 RID: 7024 RVA: 0x0009F840 File Offset: 0x0009DA40
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RefogMap()
		{
			FloodFillerFog.DebugRefogMap(Find.CurrentMap);
		}

		// Token: 0x06001B71 RID: 7025 RVA: 0x0009F84C File Offset: 0x0009DA4C
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

		// Token: 0x06001B72 RID: 7026 RVA: 0x0009F8E8 File Offset: 0x0009DAE8
		[DebugAction("General", "Increment time 1 hour", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void IncrementTime1Hour()
		{
			Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 2500);
		}

		// Token: 0x06001B73 RID: 7027 RVA: 0x0009F904 File Offset: 0x0009DB04
		[DebugAction("General", "Increment time 6 hours", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void IncrementTime6Hours()
		{
			Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 15000);
		}

		// Token: 0x06001B74 RID: 7028 RVA: 0x0009F920 File Offset: 0x0009DB20
		[DebugAction("General", "Increment time 12 hours", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void IncrementTime12Hours()
		{
			Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 30000);
		}

		// Token: 0x06001B75 RID: 7029 RVA: 0x0009F93C File Offset: 0x0009DB3C
		[DebugAction("General", "Increment time 1 day", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void IncrementTime1Day()
		{
			Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 60000);
		}

		// Token: 0x06001B76 RID: 7030 RVA: 0x0009F958 File Offset: 0x0009DB58
		[DebugAction("General", "Increment time 1 season", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void IncrementTime1Season()
		{
			Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 900000);
		}

		// Token: 0x06001B77 RID: 7031 RVA: 0x0009F974 File Offset: 0x0009DB74
		[DebugAction("General", "Storywatcher tick 1 day", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void StorywatcherTick1Day()
		{
			for (int i = 0; i < 60000; i++)
			{
				Find.StoryWatcher.StoryWatcherTick();
				Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 1);
			}
		}

		// Token: 0x06001B78 RID: 7032 RVA: 0x0009F9B4 File Offset: 0x0009DBB4
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

		// Token: 0x06001B79 RID: 7033 RVA: 0x0009FA68 File Offset: 0x0009DC68
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

		// Token: 0x06001B7A RID: 7034 RVA: 0x0009FB1C File Offset: 0x0009DD1C
		public static void AddGuest(GuestStatus guestStatus)
		{
			foreach (Building_Bed building_Bed in Find.CurrentMap.listerBuildings.AllBuildingsColonistOfClass<Building_Bed>())
			{
				if ((!building_Bed.OwnersForReading.Any<Pawn>() || building_Bed.AnyUnownedSleepingSlot) && (guestStatus != GuestStatus.Prisoner || building_Bed.ForPrisoners) && (guestStatus != GuestStatus.Slave || building_Bed.ForSlaves))
				{
					PawnKindDef pawnKindDef;
					if (guestStatus == GuestStatus.Guest)
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
					pawn.guest.SetGuestStatus(Faction.OfPlayer, guestStatus);
					break;
				}
			}
		}

		// Token: 0x06001B7B RID: 7035 RVA: 0x0009FCC0 File Offset: 0x0009DEC0
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

		// Token: 0x06001B7C RID: 7036 RVA: 0x0009FDFC File Offset: 0x0009DFFC
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

		// Token: 0x06001B7D RID: 7037 RVA: 0x0009FEC8 File Offset: 0x0009E0C8
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
							if (mostSeniorTitle != null && !mostSeniorTitle.def.requiredApparel.NullOrEmpty<ApparelRequirement>())
							{
								for (int j = 0; j < mostSeniorTitle.def.requiredApparel.Count; j++)
								{
									ApparelRequirement apparelRequirement = mostSeniorTitle.def.requiredApparel[j];
									if (apparelRequirement.IsActive(pawn) && !apparelRequirement.IsMet(pawn))
									{
										Log.Error(string.Concat(new object[]
										{
											localKindDef,
											" (",
											mostSeniorTitle.def.label,
											")  does not have its title requirements met. index=",
											j,
											DebugActionsMisc.<PawnKindApparelCheck>g__logApparel|46_0(pawn)
										}));
										flag = true;
									}
								}
							}
						}
						List<Apparel> wornApparel = pawn.apparel.WornApparel;
						for (int k = 0; k < wornApparel.Count; k++)
						{
							string text = DebugActionsMisc.<PawnKindApparelCheck>g__apparelOkayToWear|46_1(pawn, wornApparel[k]);
							if (text != "OK")
							{
								Log.Error(text + " - " + wornApparel[k].Label + DebugActionsMisc.<PawnKindApparelCheck>g__logApparel|46_0(pawn));
								flag = true;
							}
						}
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
					}
					if (!flag)
					{
						Log.Message("No errors for " + localKindDef.defName);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001B7E RID: 7038 RVA: 0x0009FF9C File Offset: 0x0009E19C
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DumpPawnAtlases()
		{
			string text = Application.dataPath + "\\atlasDump_Pawn";
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			GlobalTextureAtlasManager.DumpPawnAtlases(text);
		}

		// Token: 0x06001B7F RID: 7039 RVA: 0x0009FFD0 File Offset: 0x0009E1D0
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DumpStaticAtlases()
		{
			string text = Application.dataPath + "\\atlasDump_Static";
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			GlobalTextureAtlasManager.DumpStaticAtlases(text);
		}

		// Token: 0x06001B80 RID: 7040 RVA: 0x000A0004 File Offset: 0x0009E204
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
						Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(localKindDef, faction, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, fixedTitle, null, false, false));
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
						Log.Message("No errors for " + localKindDef.defName);
						return;
					}
					Log.Error("Errors:\n" + sb.ToString());
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001B81 RID: 7041 RVA: 0x000A00F4 File Offset: 0x0009E2F4
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ClearPrisonerInteractionSchedule(Pawn p)
		{
			if (p.IsPrisonerOfColony)
			{
				p.mindState.lastAssignedInteractTime = -1;
				p.mindState.interactionsToday = 0;
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06001B82 RID: 7042 RVA: 0x000A011C File Offset: 0x0009E31C
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GlowAtPosition()
		{
			Map currentMap = Find.CurrentMap;
			foreach (IntVec3 c in GenRadial.RadialCellsAround(UI.MouseCell(), 10f, true))
			{
				if (c.InBounds(currentMap))
				{
					float num = Find.CurrentMap.glowGrid.GameGlowAt(c, false);
					currentMap.debugDrawer.FlashCell(c, 0f, num.ToString("F1"), 100);
				}
			}
		}

		// Token: 0x06001B83 RID: 7043 RVA: 0x000A01AC File Offset: 0x0009E3AC
		[DebugAction("General", "HSV At Position", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void HSVAtPosition()
		{
			Map currentMap = Find.CurrentMap;
			foreach (IntVec3 c in GenRadial.RadialCellsAround(UI.MouseCell(), 10f, true))
			{
				if (c.InBounds(currentMap))
				{
					float num;
					float num2;
					float num3;
					Color.RGBToHSV(Find.CurrentMap.glowGrid.VisualGlowAt(c), out num, out num2, out num3);
					currentMap.debugDrawer.FlashCell(c, 0.5f, string.Concat(new string[]
					{
						"HSV(",
						num.ToString(".0#"),
						",",
						num2.ToString(".0#"),
						",",
						num3.ToString(".0#"),
						")"
					}), 100);
				}
			}
		}

		// Token: 0x06001B84 RID: 7044 RVA: 0x000A02A0 File Offset: 0x0009E4A0
		[DebugAction("General", null, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FillMapWithTrees()
		{
			Map currentMap = Find.CurrentMap;
			foreach (IntVec3 intVec in currentMap.AllCells)
			{
				if (intVec.Standable(currentMap))
				{
					GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Plant_TreeOak, null), intVec, currentMap, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x06001B85 RID: 7045 RVA: 0x000A030C File Offset: 0x0009E50C
		[DebugAction("General", null, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FlashBlockedLandingCells()
		{
			Map currentMap = Find.CurrentMap;
			foreach (IntVec3 c in currentMap.AllCells)
			{
				if (!c.Fogged(currentMap) && !DropCellFinder.IsGoodDropSpot(c, currentMap, false, false, false))
				{
					currentMap.debugDrawer.FlashCell(c, 0f, "bl", 50);
				}
			}
		}

		// Token: 0x06001B87 RID: 7047 RVA: 0x000A0394 File Offset: 0x0009E594
		[CompilerGenerated]
		internal static string <PawnKindApparelCheck>g__logApparel|46_0(Pawn p)
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

		// Token: 0x06001B88 RID: 7048 RVA: 0x000A0408 File Offset: 0x0009E608
		[CompilerGenerated]
		internal static string <PawnKindApparelCheck>g__apparelOkayToWear|46_1(Pawn pawn, Apparel apparel)
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

		// Token: 0x0400119E RID: 4510
		private static List<Pawn> tmpLentColonists = new List<Pawn>();

		// Token: 0x0400119F RID: 4511
		private const string NoErrorString = "OK";

		// Token: 0x040011A0 RID: 4512
		private const string RoyalApparelTag = "Royal";

		// Token: 0x040011A1 RID: 4513
		private const int PawnsToGenerate = 100;
	}
}
