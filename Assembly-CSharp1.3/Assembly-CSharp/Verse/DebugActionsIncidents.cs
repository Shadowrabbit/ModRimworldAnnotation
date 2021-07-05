using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x0200039A RID: 922
	public static class DebugActionsIncidents
	{
		// Token: 0x06001B38 RID: 6968 RVA: 0x0009DC00 File Offset: 0x0009BE00
		[DebugActionYielder]
		private static IEnumerable<Dialog_DebugActionsMenu.DebugActionOption> IncidentsYielder()
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				yield break;
			}
			IIncidentTarget target = WorldRendererUtility.WorldRenderedNow ? (Find.WorldSelector.SingleSelectedObject as IIncidentTarget) : null;
			if (target == null)
			{
				target = Find.CurrentMap;
			}
			if (target != null)
			{
				yield return DebugActionsIncidents.GetIncidentDebugAction(target);
				yield return DebugActionsIncidents.GetIncidents10DebugAction(target);
				yield return DebugActionsIncidents.GetIncidentWithPointsDebugAction(target);
			}
			if (WorldRendererUtility.WorldRenderedNow)
			{
				yield return DebugActionsIncidents.GetIncidentDebugAction(Find.World);
				yield return DebugActionsIncidents.GetIncidents10DebugAction(Find.World);
				yield return DebugActionsIncidents.GetIncidentWithPointsDebugAction(Find.World);
			}
			yield break;
		}

		// Token: 0x06001B39 RID: 6969 RVA: 0x0009DC0C File Offset: 0x0009BE0C
		[DebugAction("Incidents", "Do trade caravan arrival...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DoTradeCaravanSpecific()
		{
			DebugActionsIncidents.<>c__DisplayClass1_0 CS$<>8__locals1 = new DebugActionsIncidents.<>c__DisplayClass1_0();
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			CS$<>8__locals1.incidentDef = IncidentDefOf.TraderCaravanArrival;
			CS$<>8__locals1.target = Find.CurrentMap;
			using (IEnumerator<Faction> enumerator = Find.FactionManager.AllFactions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DebugActionsIncidents.<>c__DisplayClass1_1 CS$<>8__locals2 = new DebugActionsIncidents.<>c__DisplayClass1_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					CS$<>8__locals2.faction = enumerator.Current;
					if (CS$<>8__locals2.faction.def.caravanTraderKinds != null && CS$<>8__locals2.faction.def.caravanTraderKinds.Any<TraderKindDef>())
					{
						list.Add(new DebugMenuOption(CS$<>8__locals2.faction.Name, DebugMenuOptionMode.Action, delegate()
						{
							List<DebugMenuOption> list2 = new List<DebugMenuOption>();
							using (List<TraderKindDef>.Enumerator enumerator2 = CS$<>8__locals2.faction.def.caravanTraderKinds.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									DebugActionsIncidents.<>c__DisplayClass1_2 CS$<>8__locals3 = new DebugActionsIncidents.<>c__DisplayClass1_2();
									CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals2;
									CS$<>8__locals3.traderKind = enumerator2.Current;
									string text = CS$<>8__locals3.traderKind.label;
									IncidentParms parms = StorytellerUtility.DefaultParmsNow(CS$<>8__locals2.CS$<>8__locals1.incidentDef.category, CS$<>8__locals2.CS$<>8__locals1.target);
									parms.faction = CS$<>8__locals2.faction;
									parms.traderKind = CS$<>8__locals3.traderKind;
									if (!CS$<>8__locals2.CS$<>8__locals1.incidentDef.Worker.CanFireNow(parms))
									{
										text += " [NO]";
									}
									list2.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
									{
										IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.incidentDef.category, CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.target);
										if (CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.incidentDef.pointsScaleable)
										{
											incidentParms = Find.Storyteller.storytellerComps.First((StorytellerComp x) => x is StorytellerComp_OnOffCycle || x is StorytellerComp_RandomMain).GenerateParms(CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.incidentDef.category, parms.target);
										}
										incidentParms.faction = CS$<>8__locals3.CS$<>8__locals2.faction;
										incidentParms.traderKind = CS$<>8__locals3.traderKind;
										CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.incidentDef.Worker.TryExecute(incidentParms);
									}));
								}
							}
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
						}));
					}
				}
			}
			if (list.Count == 0)
			{
				Messages.Message("No valid factions found for trade caravans", MessageTypeDefOf.RejectInput, false);
				return;
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001B3A RID: 6970 RVA: 0x0009DD00 File Offset: 0x0009BF00
		[DebugAction("Incidents", "Execute raid with points...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExecuteRaidWithPoints()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (float localP2 in DebugActionsUtility.PointsOptions(true))
			{
				float localP = localP2;
				list.Add(new FloatMenuOption(localP.ToString() + " points", delegate()
				{
					IncidentParms incidentParms = new IncidentParms();
					incidentParms.target = Find.CurrentMap;
					incidentParms.points = localP;
					IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06001B3B RID: 6971 RVA: 0x0009DDA4 File Offset: 0x0009BFA4
		[DebugAction("Incidents", "Execute raid with faction...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExecuteRaidWithFaction()
		{
			StorytellerComp storytellerComp = Find.Storyteller.storytellerComps.First((StorytellerComp x) => x is StorytellerComp_OnOffCycle || x is StorytellerComp_RandomMain);
			IncidentParms parms = storytellerComp.GenerateParms(IncidentCategoryDefOf.ThreatBig, Find.CurrentMap);
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			Func<RaidStrategyDef, bool> <>9__3;
			Func<PawnsArrivalModeDef, bool> <>9__5;
			foreach (Faction localFac2 in Find.FactionManager.AllFactions)
			{
				Faction localFac = localFac2;
				list.Add(new DebugMenuOption(localFac.Name + " (" + localFac.def.defName + ")", DebugMenuOptionMode.Action, delegate()
				{
					parms.faction = localFac;
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (float num in DebugActionsUtility.PointsOptions(true))
					{
						float localPoints = num;
						list2.Add(new DebugMenuOption(num + " points", DebugMenuOptionMode.Action, delegate()
						{
							parms.points = localPoints;
							IEnumerable<RaidStrategyDef> allDefs = DefDatabase<RaidStrategyDef>.AllDefs;
							Func<RaidStrategyDef, bool> predicate;
							if ((predicate = <>9__3) == null)
							{
								predicate = (<>9__3 = ((RaidStrategyDef s) => s.Worker.CanUseWith(parms, PawnGroupKindDefOf.Combat)));
							}
							List<RaidStrategyDef> source = allDefs.Where(predicate).ToList<RaidStrategyDef>();
							Log.Message("Available strategies: " + string.Join(", ", (from s in source
							select s.defName).ToArray<string>()));
							parms.raidStrategy = source.RandomElement<RaidStrategyDef>();
							if (parms.raidStrategy != null)
							{
								Log.Message("Strategy: " + parms.raidStrategy.defName);
								IEnumerable<PawnsArrivalModeDef> allDefs2 = DefDatabase<PawnsArrivalModeDef>.AllDefs;
								Func<PawnsArrivalModeDef, bool> predicate2;
								if ((predicate2 = <>9__5) == null)
								{
									predicate2 = (<>9__5 = ((PawnsArrivalModeDef a) => a.Worker.CanUseWith(parms) && parms.raidStrategy.arriveModes.Contains(a)));
								}
								List<PawnsArrivalModeDef> source2 = allDefs2.Where(predicate2).ToList<PawnsArrivalModeDef>();
								Log.Message("Available arrival modes: " + string.Join(", ", (from s in source2
								select s.defName).ToArray<string>()));
								parms.raidArrivalMode = source2.RandomElement<PawnsArrivalModeDef>();
								Log.Message("Arrival mode: " + parms.raidArrivalMode.defName);
							}
							DebugActionsIncidents.DoRaid(parms);
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x0009DEB0 File Offset: 0x0009C0B0
		[DebugAction("Incidents", "Execute raid with specifics...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExecuteRaidWithSpecifics()
		{
			StorytellerComp storytellerComp = Find.Storyteller.storytellerComps.First((StorytellerComp x) => x is StorytellerComp_OnOffCycle || x is StorytellerComp_RandomMain);
			IncidentParms parms = storytellerComp.GenerateParms(IncidentCategoryDefOf.ThreatBig, Find.CurrentMap);
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			Action <>9__4;
			foreach (Faction localFac2 in Find.FactionManager.AllFactions)
			{
				Faction localFac = localFac2;
				list.Add(new DebugMenuOption(localFac.Name + " (" + localFac.def.defName + ")", DebugMenuOptionMode.Action, delegate()
				{
					parms.faction = localFac;
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (float num in DebugActionsUtility.PointsOptions(true))
					{
						float localPoints = num;
						list2.Add(new DebugMenuOption(num + " points", DebugMenuOptionMode.Action, delegate()
						{
							parms.points = localPoints;
							List<DebugMenuOption> list3 = new List<DebugMenuOption>();
							foreach (RaidStrategyDef localStrat2 in DefDatabase<RaidStrategyDef>.AllDefs)
							{
								RaidStrategyDef localStrat = localStrat2;
								string text = localStrat.defName;
								if (!localStrat.Worker.CanUseWith(parms, PawnGroupKindDefOf.Combat))
								{
									text += " [NO]";
								}
								list3.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
								{
									parms.raidStrategy = localStrat;
									List<DebugMenuOption> list4 = new List<DebugMenuOption>();
									List<DebugMenuOption> list5 = list4;
									string label = "-Random-";
									DebugMenuOptionMode mode = DebugMenuOptionMode.Action;
									Action method;
									if ((method = <>9__4) == null)
									{
										method = (<>9__4 = delegate()
										{
											DebugActionsIncidents.DoRaid(parms);
										});
									}
									list5.Add(new DebugMenuOption(label, mode, method));
									foreach (PawnsArrivalModeDef localArrival2 in DefDatabase<PawnsArrivalModeDef>.AllDefs)
									{
										PawnsArrivalModeDef localArrival = localArrival2;
										string text2 = localArrival.defName;
										if (!localArrival.Worker.CanUseWith(parms) || !localStrat.arriveModes.Contains(localArrival))
										{
											text2 += " [NO]";
										}
										list4.Add(new DebugMenuOption(text2, DebugMenuOptionMode.Action, delegate()
										{
											parms.raidArrivalMode = localArrival;
											DebugActionsIncidents.DoRaid(parms);
										}));
									}
									Find.WindowStack.Add(new Dialog_DebugOptionListLister(list4));
								}));
							}
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3));
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001B3D RID: 6973 RVA: 0x0009DFBC File Offset: 0x0009C1BC
		private static string GetIncidentTargetLabel(IIncidentTarget target)
		{
			if (target == null)
			{
				return "null target";
			}
			if (target is Map)
			{
				return "Map";
			}
			if (target is World)
			{
				return "World";
			}
			if (target is Caravan)
			{
				return ((Caravan)target).LabelCap;
			}
			return target.ToString();
		}

		// Token: 0x06001B3E RID: 6974 RVA: 0x0009E008 File Offset: 0x0009C208
		private static Dialog_DebugActionsMenu.DebugActionOption GetIncidentDebugAction(IIncidentTarget target)
		{
			return new Dialog_DebugActionsMenu.DebugActionOption
			{
				action = delegate()
				{
					DebugActionsIncidents.DoIncidentDebugAction(target, 1);
				},
				actionType = DebugActionType.Action,
				category = "Incidents",
				label = "Do incident (" + DebugActionsIncidents.GetIncidentTargetLabel(target) + ")..."
			};
		}

		// Token: 0x06001B3F RID: 6975 RVA: 0x0009E074 File Offset: 0x0009C274
		private static Dialog_DebugActionsMenu.DebugActionOption GetIncidents10DebugAction(IIncidentTarget target)
		{
			return new Dialog_DebugActionsMenu.DebugActionOption
			{
				action = delegate()
				{
					DebugActionsIncidents.DoIncidentDebugAction(target, 10);
				},
				actionType = DebugActionType.Action,
				category = "Incidents",
				label = "Do incident x10 (" + DebugActionsIncidents.GetIncidentTargetLabel(target) + ")..."
			};
		}

		// Token: 0x06001B40 RID: 6976 RVA: 0x0009E0E0 File Offset: 0x0009C2E0
		private static void DoIncidentDebugAction(IIncidentTarget target, int iterations = 1)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			IEnumerable<IncidentDef> allDefs = DefDatabase<IncidentDef>.AllDefs;
			Func<IncidentDef, bool> <>9__0;
			Func<IncidentDef, bool> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = ((IncidentDef d) => d.TargetAllowed(target)));
			}
			foreach (IncidentDef localDef2 in from d in allDefs.Where(predicate)
			orderby d.defName
			select d)
			{
				IncidentDef localDef = localDef2;
				string text = localDef.defName;
				IncidentParms parms = StorytellerUtility.DefaultParmsNow(localDef.category, target);
				if (!localDef.Worker.CanFireNow(parms))
				{
					text += " [NO]";
				}
				list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
				{
					for (int i = 0; i < iterations; i++)
					{
						IncidentParms parms = StorytellerUtility.DefaultParmsNow(localDef.category, target);
						if (localDef.pointsScaleable)
						{
							parms = Find.Storyteller.storytellerComps.First((StorytellerComp x) => x is StorytellerComp_OnOffCycle || x is StorytellerComp_RandomMain).GenerateParms(localDef.category, parms.target);
						}
						localDef.Worker.TryExecute(parms);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001B41 RID: 6977 RVA: 0x0009E22C File Offset: 0x0009C42C
		private static Dialog_DebugActionsMenu.DebugActionOption GetIncidentWithPointsDebugAction(IIncidentTarget target)
		{
			return new Dialog_DebugActionsMenu.DebugActionOption
			{
				action = delegate()
				{
					DebugActionsIncidents.DoIncidentWithPointsAction(target);
				},
				actionType = DebugActionType.Action,
				category = "Incidents",
				label = "Do incident w/ points (" + DebugActionsIncidents.GetIncidentTargetLabel(target) + ")..."
			};
		}

		// Token: 0x06001B42 RID: 6978 RVA: 0x0009E298 File Offset: 0x0009C498
		private static void DoIncidentWithPointsAction(IIncidentTarget target)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			IEnumerable<IncidentDef> allDefs = DefDatabase<IncidentDef>.AllDefs;
			Func<IncidentDef, bool> <>9__0;
			Func<IncidentDef, bool> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = ((IncidentDef d) => d.TargetAllowed(target) && d.pointsScaleable));
			}
			foreach (IncidentDef localDef2 in from d in allDefs.Where(predicate)
			orderby d.defName
			select d)
			{
				IncidentDef localDef = localDef2;
				string text = localDef.defName;
				IncidentParms parms = StorytellerUtility.DefaultParmsNow(localDef.category, target);
				if (!localDef.Worker.CanFireNow(parms))
				{
					text += " [NO]";
				}
				list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (float num in DebugActionsUtility.PointsOptions(true))
					{
						float localPoints = num;
						list2.Add(new DebugMenuOption(num + " points", DebugMenuOptionMode.Action, delegate()
						{
							parms.points = localPoints;
							localDef.Worker.TryExecute(parms);
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
				}));
			}
			if (list.Count == 0)
			{
				return;
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001B43 RID: 6979 RVA: 0x0009E3D8 File Offset: 0x0009C5D8
		private static void DoRaid(IncidentParms parms)
		{
			IncidentDef incidentDef;
			if (parms.faction.HostileTo(Faction.OfPlayer))
			{
				incidentDef = IncidentDefOf.RaidEnemy;
			}
			else
			{
				incidentDef = IncidentDefOf.RaidFriendly;
			}
			incidentDef.Worker.TryExecute(parms);
		}
	}
}
