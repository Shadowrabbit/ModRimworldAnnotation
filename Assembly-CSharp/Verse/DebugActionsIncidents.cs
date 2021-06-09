using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x0200054C RID: 1356
	public static class DebugActionsIncidents
	{
		// Token: 0x060022D7 RID: 8919 RVA: 0x0001DD7E File Offset: 0x0001BF7E
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
				yield return DebugActionsIncidents.GetIncidentWithPointsDebugAction(Find.World);
			}
			yield break;
		}

		// Token: 0x060022D8 RID: 8920 RVA: 0x0010B438 File Offset: 0x00109638
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
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x060022D9 RID: 8921 RVA: 0x0010B4D8 File Offset: 0x001096D8
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
							select s.defName).ToArray<string>()), false);
							parms.raidStrategy = source.RandomElement<RaidStrategyDef>();
							if (parms.raidStrategy != null)
							{
								Log.Message("Strategy: " + parms.raidStrategy.defName, false);
								IEnumerable<PawnsArrivalModeDef> allDefs2 = DefDatabase<PawnsArrivalModeDef>.AllDefs;
								Func<PawnsArrivalModeDef, bool> predicate2;
								if ((predicate2 = <>9__5) == null)
								{
									predicate2 = (<>9__5 = ((PawnsArrivalModeDef a) => a.Worker.CanUseWith(parms) && parms.raidStrategy.arriveModes.Contains(a)));
								}
								List<PawnsArrivalModeDef> source2 = allDefs2.Where(predicate2).ToList<PawnsArrivalModeDef>();
								Log.Message("Available arrival modes: " + string.Join(", ", (from s in source2
								select s.defName).ToArray<string>()), false);
								parms.raidArrivalMode = source2.RandomElement<PawnsArrivalModeDef>();
								Log.Message("Arrival mode: " + parms.raidArrivalMode.defName, false);
							}
							DebugActionsIncidents.DoRaid(parms);
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060022DA RID: 8922 RVA: 0x0010B5E4 File Offset: 0x001097E4
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

		// Token: 0x060022DB RID: 8923 RVA: 0x0010B6F0 File Offset: 0x001098F0
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

		// Token: 0x060022DC RID: 8924 RVA: 0x0010B73C File Offset: 0x0010993C
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

		// Token: 0x060022DD RID: 8925 RVA: 0x0010B7A8 File Offset: 0x001099A8
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

		// Token: 0x060022DE RID: 8926 RVA: 0x0010B814 File Offset: 0x00109A14
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
				if (!localDef.Worker.CanFireNow(parms, false))
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

		// Token: 0x060022DF RID: 8927 RVA: 0x0010B960 File Offset: 0x00109B60
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

		// Token: 0x060022E0 RID: 8928 RVA: 0x0010B9CC File Offset: 0x00109BCC
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
				if (!localDef.Worker.CanFireNow(parms, false))
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
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060022E1 RID: 8929 RVA: 0x0010BB04 File Offset: 0x00109D04
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
