using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000635 RID: 1589
	public static class DebugOutputsIncidents
	{
		// Token: 0x060028CD RID: 10445 RVA: 0x00121A4C File Offset: 0x0011FC4C
		[DebugOutput("Incidents", false)]
		public static void IncidentChances()
		{
			List<StorytellerComp> storytellerComps = Find.Storyteller.storytellerComps;
			for (int i = 0; i < storytellerComps.Count; i++)
			{
				StorytellerComp_CategoryMTB storytellerComp_CategoryMTB = storytellerComps[i] as StorytellerComp_CategoryMTB;
				if (storytellerComp_CategoryMTB != null && ((StorytellerCompProperties_CategoryMTB)storytellerComp_CategoryMTB.props).category == IncidentCategoryDefOf.Misc)
				{
					storytellerComp_CategoryMTB.DebugTablesIncidentChances();
				}
			}
		}

		// Token: 0x060028CE RID: 10446 RVA: 0x00020FD1 File Offset: 0x0001F1D1
		[DebugOutput("Incidents", true)]
		public static void FutureIncidents()
		{
			StorytellerUtility.ShowFutureIncidentsDebugLogFloatMenu(false);
		}

		// Token: 0x060028CF RID: 10447 RVA: 0x00020FD9 File Offset: 0x0001F1D9
		[DebugOutput("Incidents", true)]
		public static void FutureIncidentsCurrentMap()
		{
			StorytellerUtility.ShowFutureIncidentsDebugLogFloatMenu(true);
		}

		// Token: 0x060028D0 RID: 10448 RVA: 0x00020FE1 File Offset: 0x0001F1E1
		[DebugOutput("Incidents", true)]
		public static void IncidentTargetsList()
		{
			StorytellerUtility.DebugLogTestIncidentTargets();
		}

		// Token: 0x060028D1 RID: 10449 RVA: 0x00121AA4 File Offset: 0x0011FCA4
		[DebugOutput("Incidents", false)]
		public static void PawnArrivalCandidates()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(IncidentDefOf.RaidEnemy.defName);
			stringBuilder.AppendLine(((IncidentWorker_PawnsArrive)IncidentDefOf.RaidEnemy.Worker).DebugListingOfGroupSources());
			stringBuilder.AppendLine(IncidentDefOf.RaidFriendly.defName);
			stringBuilder.AppendLine(((IncidentWorker_PawnsArrive)IncidentDefOf.RaidFriendly.Worker).DebugListingOfGroupSources());
			stringBuilder.AppendLine(IncidentDefOf.VisitorGroup.defName);
			stringBuilder.AppendLine(((IncidentWorker_PawnsArrive)IncidentDefOf.VisitorGroup.Worker).DebugListingOfGroupSources());
			stringBuilder.AppendLine(IncidentDefOf.TravelerGroup.defName);
			stringBuilder.AppendLine(((IncidentWorker_PawnsArrive)IncidentDefOf.TravelerGroup.Worker).DebugListingOfGroupSources());
			stringBuilder.AppendLine(IncidentDefOf.TraderCaravanArrival.defName);
			stringBuilder.AppendLine(((IncidentWorker_PawnsArrive)IncidentDefOf.TraderCaravanArrival.Worker).DebugListingOfGroupSources());
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x060028D2 RID: 10450 RVA: 0x00121BA0 File Offset: 0x0011FDA0
		[DebugOutput("Incidents", false)]
		public static void TraderKinds()
		{
			IEnumerable<TraderKindDef> allDefs = DefDatabase<TraderKindDef>.AllDefs;
			TableDataGetter<TraderKindDef>[] array = new TableDataGetter<TraderKindDef>[8];
			array[0] = new TableDataGetter<TraderKindDef>("defName", (TraderKindDef d) => d.defName);
			array[1] = new TableDataGetter<TraderKindDef>("orbital", (TraderKindDef d) => d.orbital.ToStringCheckBlank());
			array[2] = new TableDataGetter<TraderKindDef>("requestable", (TraderKindDef d) => d.requestable.ToStringCheckBlank());
			array[3] = new TableDataGetter<TraderKindDef>("commonality\nbase", (TraderKindDef d) => d.commonality.ToString("F2"));
			array[4] = new TableDataGetter<TraderKindDef>("commonality\nnow", (TraderKindDef d) => d.CalculatedCommonality.ToString("F2"));
			array[5] = new TableDataGetter<TraderKindDef>("faction", delegate(TraderKindDef d)
			{
				if (d.faction == null)
				{
					return "";
				}
				return d.faction.defName;
			});
			array[6] = new TableDataGetter<TraderKindDef>("permit\nrequired", delegate(TraderKindDef d)
			{
				if (d.permitRequiredForTrading == null)
				{
					return "";
				}
				return d.permitRequiredForTrading.defName;
			});
			array[7] = new TableDataGetter<TraderKindDef>("average\nvalue", (TraderKindDef d) => ((ThingSetMaker_TraderStock)ThingSetMakerDefOf.TraderStock.root).DebugAverageTotalStockValue(d).ToString("F0"));
			DebugTables.MakeTablesDialog<TraderKindDef>(allDefs, array);
		}

		// Token: 0x060028D3 RID: 10451 RVA: 0x00121D20 File Offset: 0x0011FF20
		[DebugOutput("Incidents", false)]
		public static void TraderKindThings()
		{
			List<TableDataGetter<ThingDef>> list = new List<TableDataGetter<ThingDef>>();
			list.Add(new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName));
			foreach (TraderKindDef localTk2 in DefDatabase<TraderKindDef>.AllDefs)
			{
				TraderKindDef localTk = localTk2;
				string text = localTk.defName;
				text = text.Replace("_", "\n");
				text = text.Shorten();
				list.Add(new TableDataGetter<ThingDef>(text, (ThingDef td) => localTk.WillTrade(td).ToStringCheckBlank()));
			}
			DebugTables.MakeTablesDialog<ThingDef>((from d in DefDatabase<ThingDef>.AllDefs
			where (d.category == ThingCategory.Item && d.BaseMarketValue > 0.001f && !d.isUnfinishedThing && !d.IsCorpse && !d.destroyOnDrop && d != ThingDefOf.Silver && !d.thingCategories.NullOrEmpty<ThingCategoryDef>()) || (d.category == ThingCategory.Building && d.Minifiable) || d.category == ThingCategory.Pawn
			select d).OrderBy(delegate(ThingDef d)
			{
				if (d.thingCategories.NullOrEmpty<ThingCategoryDef>())
				{
					return "zzzzzzz";
				}
				return d.thingCategories[0].defName;
			}).ThenBy((ThingDef d) => d.BaseMarketValue), list.ToArray());
		}

		// Token: 0x060028D4 RID: 10452 RVA: 0x00121E68 File Offset: 0x00120068
		[DebugOutput("Incidents", false)]
		public static void TraderStockMarketValues()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (TraderKindDef traderKindDef in DefDatabase<TraderKindDef>.AllDefs)
			{
				stringBuilder.AppendLine(traderKindDef.defName + " : " + ((ThingSetMaker_TraderStock)ThingSetMakerDefOf.TraderStock.root).DebugAverageTotalStockValue(traderKindDef).ToString("F0"));
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x060028D5 RID: 10453 RVA: 0x00121EF8 File Offset: 0x001200F8
		[DebugOutput("Incidents", false)]
		public static void TraderStockGeneration()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (TraderKindDef localDef2 in DefDatabase<TraderKindDef>.AllDefs)
			{
				TraderKindDef localDef = localDef2;
				FloatMenuOption item = new FloatMenuOption(localDef.defName, delegate()
				{
					Log.Message(((ThingSetMaker_TraderStock)ThingSetMakerDefOf.TraderStock.root).DebugGenerationDataFor(localDef), false);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x060028D6 RID: 10454 RVA: 0x00121F90 File Offset: 0x00120190
		[DebugOutput("Incidents", false)]
		public static void TraderStockGeneratorsDefs()
		{
			if (Find.CurrentMap == null)
			{
				Log.Error("Requires visible map.", false);
				return;
			}
			StringBuilder sb = new StringBuilder();
			Action<StockGenerator> action = delegate(StockGenerator gen)
			{
				sb.AppendLine(gen.GetType().ToString());
				sb.AppendLine("ALLOWED DEFS:");
				IEnumerable<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefs;
				Func<ThingDef, bool> <>9__1;
				Func<ThingDef, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((ThingDef d) => gen.HandlesThingDef(d)));
				}
				foreach (ThingDef thingDef in allDefs.Where(predicate))
				{
					sb.AppendLine(string.Concat(new object[]
					{
						thingDef.defName,
						" [",
						thingDef.BaseMarketValue,
						"]"
					}));
				}
				sb.AppendLine();
				sb.AppendLine("GENERATION TEST:");
				gen.countRange = IntRange.one;
				for (int i = 0; i < 30; i++)
				{
					foreach (Thing thing in gen.GenerateThings(Find.CurrentMap.Tile, null))
					{
						sb.AppendLine(string.Concat(new object[]
						{
							thing.Label,
							" [",
							thing.MarketValue,
							"]"
						}));
					}
				}
				sb.AppendLine("---------------------------------------------------------");
			};
			action(new StockGenerator_Armor());
			action(new StockGenerator_WeaponsRanged());
			action(new StockGenerator_Clothes());
			action(new StockGenerator_Art());
			Log.Message(sb.ToString(), false);
		}

		// Token: 0x060028D7 RID: 10455 RVA: 0x0012200C File Offset: 0x0012020C
		[DebugOutput("Incidents", false)]
		public static void PawnGroupGenSampled()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (faction.def.pawnGroupMakers != null)
				{
					if (faction.def.pawnGroupMakers.Any((PawnGroupMaker x) => x.kindDef == PawnGroupKindDefOf.Combat))
					{
						Faction localFac = faction;
						list.Add(new DebugMenuOption(localFac.Name + " (" + localFac.def.defName + ")", DebugMenuOptionMode.Action, delegate()
						{
							List<DebugMenuOption> list2 = new List<DebugMenuOption>();
							foreach (float localP2 in DebugActionsUtility.PointsOptions(true))
							{
								float localP = localP2;
								float maxPawnCost = PawnGroupMakerUtility.MaxPawnCost(localFac, localP, null, PawnGroupKindDefOf.Combat);
								string defName = (from op in localFac.def.pawnGroupMakers.SelectMany((PawnGroupMaker gm) => gm.options)
								where op.Cost <= maxPawnCost
								select op).MaxBy((PawnGenOption op) => op.Cost).kind.defName;
								string label = string.Concat(new string[]
								{
									localP.ToString(),
									", max ",
									maxPawnCost.ToString("F0"),
									" ",
									defName
								});
								list2.Add(new DebugMenuOption(label, DebugMenuOptionMode.Action, delegate()
								{
									Dictionary<ThingDef, int>[] weaponsCount = new Dictionary<ThingDef, int>[20];
									string[] pawnKinds = new string[20];
									for (int i = 0; i < 20; i++)
									{
										weaponsCount[i] = new Dictionary<ThingDef, int>();
										List<Pawn> list3 = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
										{
											groupKind = PawnGroupKindDefOf.Combat,
											tile = Find.CurrentMap.Tile,
											points = localP,
											faction = localFac
										}, false).ToList<Pawn>();
										pawnKinds[i] = PawnUtility.PawnKindsToCommaList(list3, true);
										foreach (Pawn pawn in list3)
										{
											if (pawn.equipment.Primary != null)
											{
												if (!weaponsCount[i].ContainsKey(pawn.equipment.Primary.def))
												{
													weaponsCount[i].Add(pawn.equipment.Primary.def, 0);
												}
												Dictionary<ThingDef, int> dictionary = weaponsCount[i];
												ThingDef def = pawn.equipment.Primary.def;
												int num = dictionary[def];
												dictionary[def] = num + 1;
											}
											pawn.Destroy(DestroyMode.Vanish);
										}
									}
									int totalPawns = weaponsCount.Sum((Dictionary<ThingDef, int> x) => x.Sum((KeyValuePair<ThingDef, int> y) => y.Value));
									List<TableDataGetter<int>> list4 = new List<TableDataGetter<int>>();
									list4.Add(new TableDataGetter<int>("", delegate(int x)
									{
										if (x != 20)
										{
											return (x + 1).ToString();
										}
										return "avg";
									}));
									list4.Add(new TableDataGetter<int>("pawns", delegate(int x)
									{
										string str = " ";
										string str2;
										if (x != 20)
										{
											str2 = weaponsCount[x].Sum((KeyValuePair<ThingDef, int> y) => y.Value).ToString();
										}
										else
										{
											str2 = ((float)totalPawns / 20f).ToString("0.#");
										}
										return str + str2;
									}));
									list4.Add(new TableDataGetter<int>("kinds", delegate(int x)
									{
										if (x != 20)
										{
											return pawnKinds[x];
										}
										return "";
									}));
									list4.AddRange((from x in DefDatabase<ThingDef>.AllDefs
									where x.IsWeapon && !x.weaponTags.NullOrEmpty<string>() && weaponsCount.Any((Dictionary<ThingDef, int> wc) => wc.ContainsKey(x))
									orderby x.IsMeleeWeapon descending, x.techLevel, x.BaseMarketValue
									select x).Select(delegate(ThingDef x)
									{
										Func<Dictionary<ThingDef, int>, int> <>9__19;
										return new TableDataGetter<int>(x.label.Shorten(), delegate(int y)
										{
											IEnumerable<Dictionary<ThingDef, int>> weaponsCount;
											if (y == 20)
											{
												string str = " ";
												weaponsCount = weaponsCount;
												Func<Dictionary<ThingDef, int>, int> selector;
												if ((selector = <>9__19) == null)
												{
													selector = (<>9__19 = delegate(Dictionary<ThingDef, int> z)
													{
														if (!z.ContainsKey(x))
														{
															return 0;
														}
														return z[x];
													});
												}
												return str + ((float)weaponsCount.Sum(selector) / 20f).ToString("0.#");
											}
											if (!weaponsCount[y].ContainsKey(x))
											{
												return "";
											}
											object[] array = new object[5];
											array[0] = " ";
											array[1] = weaponsCount[y][x];
											array[2] = " (";
											array[3] = ((float)weaponsCount[y][x] / (float)weaponsCount[y].Sum((KeyValuePair<ThingDef, int> z) => z.Value)).ToStringPercent("F0");
											array[4] = ")";
											return string.Concat(array);
										});
									}));
									DebugTables.MakeTablesDialog<int>(Enumerable.Range(0, 21), list4.ToArray());
								}));
							}
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
						}));
					}
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060028D8 RID: 10456 RVA: 0x00020FE8 File Offset: 0x0001F1E8
		[DebugOutput("Incidents", false)]
		public static void RaidFactionSampled()
		{
			((IncidentWorker_Raid)IncidentDefOf.RaidEnemy.Worker).DoTable_RaidFactionSampled();
		}

		// Token: 0x060028D9 RID: 10457 RVA: 0x00122100 File Offset: 0x00120300
		[DebugOutput("Incidents", false)]
		public static void RaidStrategySampled()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			list.Add(new FloatMenuOption("Choose factions randomly like a real raid", delegate()
			{
				((IncidentWorker_Raid)IncidentDefOf.RaidEnemy.Worker).DoTable_RaidStrategySampled(null);
			}, MenuOptionPriority.Default, null, null, 0f, null, null));
			using (IEnumerator<Faction> enumerator = Find.FactionManager.AllFactions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Faction f = enumerator.Current;
					Faction f2 = f;
					list.Add(new FloatMenuOption(f2.Name + " (" + f2.def.defName + ")", delegate()
					{
						((IncidentWorker_Raid)IncidentDefOf.RaidEnemy.Worker).DoTable_RaidStrategySampled(f);
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x060028DA RID: 10458 RVA: 0x001221EC File Offset: 0x001203EC
		[DebugOutput("Incidents", false)]
		public static void RaidArrivemodeSampled()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			list.Add(new FloatMenuOption("Choose factions randomly like a real raid", delegate()
			{
				((IncidentWorker_Raid)IncidentDefOf.RaidEnemy.Worker).DoTable_RaidArrivalModeSampled(null);
			}, MenuOptionPriority.Default, null, null, 0f, null, null));
			using (IEnumerator<Faction> enumerator = Find.FactionManager.AllFactions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Faction f = enumerator.Current;
					Faction f2 = f;
					list.Add(new FloatMenuOption(f2.Name + " (" + f2.def.defName + ")", delegate()
					{
						((IncidentWorker_Raid)IncidentDefOf.RaidEnemy.Worker).DoTable_RaidArrivalModeSampled(f);
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x060028DB RID: 10459 RVA: 0x001222D8 File Offset: 0x001204D8
		[DebugOutput("Incidents", false)]
		public static void ThreatsGenerator()
		{
			StorytellerUtility.DebugLogTestFutureIncidents(new ThreatsGeneratorParams
			{
				allowedThreats = AllowedThreatsGeneratorThreats.All,
				randSeed = Rand.Int,
				onDays = 1f,
				offDays = 0.5f,
				minSpacingDays = 0.04f,
				numIncidentsRange = new FloatRange(1f, 2f)
			});
		}
	}
}
