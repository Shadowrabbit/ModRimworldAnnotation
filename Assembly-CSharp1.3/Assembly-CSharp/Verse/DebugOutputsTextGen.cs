using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x020003B4 RID: 948
	public static class DebugOutputsTextGen
	{
		// Token: 0x06001D58 RID: 7512 RVA: 0x000B6EC8 File Offset: 0x000B50C8
		[DebugOutput("Text generation", false)]
		public static void FlavorfulCombatTest()
		{
			DebugOutputsTextGen.<>c__DisplayClass0_0 CS$<>8__locals1 = new DebugOutputsTextGen.<>c__DisplayClass0_0();
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			CS$<>8__locals1.maneuvers = DefDatabase<ManeuverDef>.AllDefsListForReading;
			DebugOutputsTextGen.<>c__DisplayClass0_0 CS$<>8__locals2 = CS$<>8__locals1;
			Func<ManeuverDef, RulePackDef>[] array = new Func<ManeuverDef, RulePackDef>[5];
			array[0] = ((ManeuverDef m) => new RulePackDef[]
			{
				m.combatLogRulesHit,
				m.combatLogRulesDeflect,
				m.combatLogRulesMiss,
				m.combatLogRulesDodge
			}.RandomElement<RulePackDef>());
			array[1] = ((ManeuverDef m) => m.combatLogRulesHit);
			array[2] = ((ManeuverDef m) => m.combatLogRulesDeflect);
			array[3] = ((ManeuverDef m) => m.combatLogRulesMiss);
			array[4] = ((ManeuverDef m) => m.combatLogRulesDodge);
			CS$<>8__locals2.results = array;
			string[] array2 = new string[]
			{
				"(random)",
				"Hit",
				"Deflect",
				"Miss",
				"Dodge"
			};
			using (IEnumerator<Pair<ManeuverDef, int>> enumerator = CS$<>8__locals1.maneuvers.Concat(null).Cross(Enumerable.Range(0, array2.Length)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Pair<ManeuverDef, int> maneuverresult = enumerator.Current;
					Action<Action<List<BodyPartRecord>, List<bool>>> <>9__9;
					DebugMenuOption item = new DebugMenuOption(string.Format("{0}/{1}", (maneuverresult.First == null) ? "(random)" : maneuverresult.First.defName, array2[maneuverresult.Second]), DebugMenuOptionMode.Action, delegate()
					{
						Action<Action<List<BodyPartRecord>, List<bool>>> callback;
						if ((callback = <>9__9) == null)
						{
							callback = (<>9__9 = delegate(Action<List<BodyPartRecord>, List<bool>> bodyPartCreator)
							{
								StringBuilder stringBuilder = new StringBuilder();
								for (int i = 0; i < 100; i++)
								{
									ManeuverDef maneuver = maneuverresult.First;
									if (maneuver == null)
									{
										maneuver = CS$<>8__locals1.maneuvers.RandomElement<ManeuverDef>();
									}
									RulePackDef rulePackDef = CS$<>8__locals1.results[maneuverresult.Second](maneuver);
									List<BodyPartRecord> list2 = null;
									List<bool> list3 = null;
									if (rulePackDef == maneuver.combatLogRulesHit)
									{
										list2 = new List<BodyPartRecord>();
										list3 = new List<bool>();
										bodyPartCreator(list2, list3);
									}
									Pair<ThingDef, Tool> pair;
									ImplementOwnerTypeDef implementOwnerTypeDef;
									string toolLabel;
									if (!(from ttp in (from td in DefDatabase<ThingDef>.AllDefsListForReading
									where td.IsMeleeWeapon && !td.tools.NullOrEmpty<Tool>()
									select td).SelectMany((ThingDef td) => from tool in td.tools
									select new Pair<ThingDef, Tool>(td, tool))
									where ttp.Second.capacities.Contains(maneuver.requiredCapacity)
									select ttp).TryRandomElement(out pair))
									{
										Log.Warning("Melee weapon with tool with capacity " + maneuver.requiredCapacity + " not found.");
										implementOwnerTypeDef = ImplementOwnerTypeDefOf.Bodypart;
										toolLabel = "(" + implementOwnerTypeDef.defName + ")";
									}
									else
									{
										implementOwnerTypeDef = ((pair.Second == null) ? ImplementOwnerTypeDefOf.Bodypart : ImplementOwnerTypeDefOf.Weapon);
										toolLabel = ((pair.Second != null) ? pair.Second.label : ("(" + implementOwnerTypeDef.defName + ")"));
									}
									BattleLogEntry_MeleeCombat battleLogEntry_MeleeCombat = new BattleLogEntry_MeleeCombat(rulePackDef, false, DebugOutputsTextGen.RandomPawnForCombat(), DebugOutputsTextGen.RandomPawnForCombat(), implementOwnerTypeDef, toolLabel, pair.First, null, null);
									battleLogEntry_MeleeCombat.FillTargets(list2, list3, battleLogEntry_MeleeCombat.RuleDef.defName.Contains("Deflect"));
									battleLogEntry_MeleeCombat.Debug_OverrideTicks(Rand.Int);
									stringBuilder.AppendLine(battleLogEntry_MeleeCombat.ToGameStringFromPOV(null, false));
								}
								Log.Message(stringBuilder.ToString());
							});
						}
						DebugOutputsTextGen.CreateDamagedDestroyedMenu(callback);
					});
					list.Add(item);
				}
			}
			int rf;
			int rf2;
			for (rf = 0; rf < 2; rf = rf2)
			{
				list.Add(new DebugMenuOption((rf == 0) ? "Ranged fire singleshot" : "Ranged fire burst", DebugMenuOptionMode.Action, delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = 0; i < 100; i++)
					{
						ThingDef thingDef = (from td in DefDatabase<ThingDef>.AllDefsListForReading
						where td.IsRangedWeapon && td.IsWeaponUsingProjectiles && td.PlayerAcquirable
						select td).RandomElement<ThingDef>();
						bool flag = Rand.Value < 0.2f;
						bool flag2 = !flag && Rand.Value < 0.95f;
						BattleLogEntry_RangedFire battleLogEntry_RangedFire = new BattleLogEntry_RangedFire(DebugOutputsTextGen.RandomPawnForCombat(), flag ? null : DebugOutputsTextGen.RandomPawnForCombat(), flag2 ? null : thingDef, null, rf != 0);
						battleLogEntry_RangedFire.Debug_OverrideTicks(Rand.Int);
						stringBuilder.AppendLine(battleLogEntry_RangedFire.ToGameStringFromPOV(null, false));
					}
					Log.Message(stringBuilder.ToString());
				}));
				rf2 = rf + 1;
			}
			list.Add(new DebugMenuOption("Ranged impact hit", DebugMenuOptionMode.Action, delegate()
			{
				DebugOutputsTextGen.CreateDamagedDestroyedMenu(delegate(Action<List<BodyPartRecord>, List<bool>> bodyPartCreator)
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = 0; i < 100; i++)
					{
						ThingDef weaponDef = (from td in DefDatabase<ThingDef>.AllDefsListForReading
						where td.IsRangedWeapon && td.IsWeaponUsingProjectiles && td.PlayerAcquirable
						select td).RandomElement<ThingDef>();
						List<BodyPartRecord> list2 = new List<BodyPartRecord>();
						List<bool> list3 = new List<bool>();
						bodyPartCreator(list2, list3);
						Pawn pawn = DebugOutputsTextGen.RandomPawnForCombat();
						BattleLogEntry_RangedImpact battleLogEntry_RangedImpact = new BattleLogEntry_RangedImpact(DebugOutputsTextGen.RandomPawnForCombat(), pawn, pawn, weaponDef, null, ThingDefOf.Wall);
						battleLogEntry_RangedImpact.FillTargets(list2, list3, Rand.Chance(0.5f));
						battleLogEntry_RangedImpact.Debug_OverrideTicks(Rand.Int);
						stringBuilder.AppendLine(battleLogEntry_RangedImpact.ToGameStringFromPOV(null, false));
					}
					Log.Message(stringBuilder.ToString());
				});
			}));
			list.Add(new DebugMenuOption("Ranged impact miss", DebugMenuOptionMode.Action, delegate()
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < 100; i++)
				{
					ThingDef weaponDef = (from td in DefDatabase<ThingDef>.AllDefsListForReading
					where td.IsRangedWeapon && td.IsWeaponUsingProjectiles && td.PlayerAcquirable
					select td).RandomElement<ThingDef>();
					BattleLogEntry_RangedImpact battleLogEntry_RangedImpact = new BattleLogEntry_RangedImpact(DebugOutputsTextGen.RandomPawnForCombat(), null, DebugOutputsTextGen.RandomPawnForCombat(), weaponDef, null, ThingDefOf.Wall);
					battleLogEntry_RangedImpact.Debug_OverrideTicks(Rand.Int);
					stringBuilder.AppendLine(battleLogEntry_RangedImpact.ToGameStringFromPOV(null, false));
				}
				Log.Message(stringBuilder.ToString());
			}));
			list.Add(new DebugMenuOption("Ranged impact hit incorrect", DebugMenuOptionMode.Action, delegate()
			{
				DebugOutputsTextGen.CreateDamagedDestroyedMenu(delegate(Action<List<BodyPartRecord>, List<bool>> bodyPartCreator)
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = 0; i < 100; i++)
					{
						ThingDef weaponDef = (from td in DefDatabase<ThingDef>.AllDefsListForReading
						where td.IsRangedWeapon && td.IsWeaponUsingProjectiles && td.PlayerAcquirable
						select td).RandomElement<ThingDef>();
						List<BodyPartRecord> list2 = new List<BodyPartRecord>();
						List<bool> list3 = new List<bool>();
						bodyPartCreator(list2, list3);
						BattleLogEntry_RangedImpact battleLogEntry_RangedImpact = new BattleLogEntry_RangedImpact(DebugOutputsTextGen.RandomPawnForCombat(), DebugOutputsTextGen.RandomPawnForCombat(), DebugOutputsTextGen.RandomPawnForCombat(), weaponDef, null, ThingDefOf.Wall);
						battleLogEntry_RangedImpact.FillTargets(list2, list3, Rand.Chance(0.5f));
						battleLogEntry_RangedImpact.Debug_OverrideTicks(Rand.Int);
						stringBuilder.AppendLine(battleLogEntry_RangedImpact.ToGameStringFromPOV(null, false));
					}
					Log.Message(stringBuilder.ToString());
				});
			}));
			using (IEnumerator<RulePackDef> enumerator2 = (from def in DefDatabase<RulePackDef>.AllDefsListForReading
			where def.defName.Contains("Transition") && !def.defName.Contains("Include")
			select def).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					RulePackDef transition = enumerator2.Current;
					list.Add(new DebugMenuOption(transition.defName, DebugMenuOptionMode.Action, delegate()
					{
						StringBuilder stringBuilder = new StringBuilder();
						for (int i = 0; i < 100; i++)
						{
							Pawn pawn = DebugOutputsTextGen.RandomPawnForCombat();
							Pawn initiator = DebugOutputsTextGen.RandomPawnForCombat();
							BodyPartRecord partRecord = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).RandomElement<BodyPartRecord>();
							BattleLogEntry_StateTransition battleLogEntry_StateTransition = new BattleLogEntry_StateTransition(pawn, transition, initiator, HediffMaker.MakeHediff(DefDatabase<HediffDef>.AllDefsListForReading.RandomElement<HediffDef>(), pawn, partRecord), pawn.RaceProps.body.AllParts.RandomElement<BodyPartRecord>());
							battleLogEntry_StateTransition.Debug_OverrideTicks(Rand.Int);
							stringBuilder.AppendLine(battleLogEntry_StateTransition.ToGameStringFromPOV(null, false));
						}
						Log.Message(stringBuilder.ToString());
					}));
				}
			}
			using (IEnumerator<RulePackDef> enumerator2 = (from def in DefDatabase<RulePackDef>.AllDefsListForReading
			where def.defName.Contains("DamageEvent") && !def.defName.Contains("Include")
			select def).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					RulePackDef damageEvent = enumerator2.Current;
					Action<Action<List<BodyPartRecord>, List<bool>>> <>9__25;
					list.Add(new DebugMenuOption(damageEvent.defName, DebugMenuOptionMode.Action, delegate()
					{
						Action<Action<List<BodyPartRecord>, List<bool>>> callback;
						if ((callback = <>9__25) == null)
						{
							callback = (<>9__25 = delegate(Action<List<BodyPartRecord>, List<bool>> bodyPartCreator)
							{
								StringBuilder stringBuilder = new StringBuilder();
								for (int i = 0; i < 100; i++)
								{
									List<BodyPartRecord> list2 = new List<BodyPartRecord>();
									List<bool> list3 = new List<bool>();
									bodyPartCreator(list2, list3);
									BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = new BattleLogEntry_DamageTaken(DebugOutputsTextGen.RandomPawnForCombat(), damageEvent, null);
									battleLogEntry_DamageTaken.FillTargets(list2, list3, false);
									battleLogEntry_DamageTaken.Debug_OverrideTicks(Rand.Int);
									stringBuilder.AppendLine(battleLogEntry_DamageTaken.ToGameStringFromPOV(null, false));
								}
								Log.Message(stringBuilder.ToString());
							});
						}
						DebugOutputsTextGen.CreateDamagedDestroyedMenu(callback);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001D59 RID: 7513 RVA: 0x000B72AC File Offset: 0x000B54AC
		public static Pawn RandomPawnForCombat()
		{
			PawnKindDef pawnKindDef = DefDatabase<PawnKindDef>.AllDefsListForReading.RandomElementByWeight(delegate(PawnKindDef pawnkind)
			{
				if (pawnkind.RaceProps.Humanlike)
				{
					return 8f;
				}
				if (pawnkind.RaceProps.IsMechanoid)
				{
					return 8f;
				}
				return 1f;
			});
			Faction faction = FactionUtility.DefaultFactionFrom(pawnKindDef.defaultFactionType);
			return PawnGenerator.GeneratePawn(pawnKindDef, faction);
		}

		// Token: 0x06001D5A RID: 7514 RVA: 0x000B72F4 File Offset: 0x000B54F4
		private static void CreateDamagedDestroyedMenu(Action<Action<List<BodyPartRecord>, List<bool>>> callback)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			IEnumerable<int> damagedes = Enumerable.Range(0, 5);
			IEnumerable<int> destroyedes = Enumerable.Range(0, 5);
			using (IEnumerator<Pair<int, int>> enumerator = damagedes.Concat(-1).Cross(destroyedes.Concat(-1)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Pair<int, int> damageddestroyed = enumerator.Current;
					Action<List<BodyPartRecord>, List<bool>> <>9__1;
					DebugMenuOption item = new DebugMenuOption(string.Format("{0} damaged/{1} destroyed", (damageddestroyed.First == -1) ? "(random)" : damageddestroyed.First.ToString(), (damageddestroyed.Second == -1) ? "(random)" : damageddestroyed.Second.ToString()), DebugMenuOptionMode.Action, delegate()
					{
						Action<Action<List<BodyPartRecord>, List<bool>>> callback2 = callback;
						Action<List<BodyPartRecord>, List<bool>> obj;
						if ((obj = <>9__1) == null)
						{
							obj = (<>9__1 = delegate(List<BodyPartRecord> bodyparts, List<bool> flags)
							{
								int num = damageddestroyed.First;
								int destroyed = damageddestroyed.Second;
								if (num == -1)
								{
									num = damagedes.RandomElement<int>();
								}
								if (destroyed == -1)
								{
									destroyed = destroyedes.RandomElement<int>();
								}
								Pair<BodyPartRecord, bool>[] source = (from idx in Enumerable.Range(0, num + destroyed)
								select new Pair<BodyPartRecord, bool>(BodyDefOf.Human.AllParts.RandomElement<BodyPartRecord>(), idx < destroyed)).InRandomOrder(null).ToArray<Pair<BodyPartRecord, bool>>();
								bodyparts.Clear();
								flags.Clear();
								bodyparts.AddRange(from part in source
								select part.First);
								flags.AddRange(from part in source
								select part.Second);
							});
						}
						callback2(obj);
					});
					list.Add(item);
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001D5B RID: 7515 RVA: 0x000B7420 File Offset: 0x000B5620
		[DebugOutput("Text generation", false)]
		public static void ArtDescsSpecificTale()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (TaleDef localDef2 in from def in DefDatabase<TaleDef>.AllDefs
			orderby def.defName
			select def)
			{
				TaleDef localDef = localDef2;
				FloatMenuOption item = new FloatMenuOption(localDef.defName, delegate()
				{
					DebugOutputsTextGen.LogSpecificTale(localDef, 40);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06001D5C RID: 7516 RVA: 0x000B74E0 File Offset: 0x000B56E0
		[DebugOutput("Text generation", false)]
		public static void NamesFromRulepack()
		{
			IEnumerable<RulePackDef> enumerable = from d in DefDatabase<RulePackDef>.AllDefsListForReading
			where d.directTestable || d.defName.StartsWith("Namer")
			orderby d.defName
			select d;
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (RulePackDef localNamer2 in enumerable)
			{
				RulePackDef localNamer = localNamer2;
				FloatMenuOption item = new FloatMenuOption(localNamer.defName, delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("Testing RulePack " + localNamer.defName + " as a name generator:");
					for (int i = 0; i < 200; i++)
					{
						string testPawnNameSymbol = (i % 2 == 0) ? "Smithee" : null;
						stringBuilder.AppendLine(NameGenerator.GenerateName(localNamer, null, false, localNamer.FirstRuleKeyword, testPawnNameSymbol));
					}
					Log.Message(stringBuilder.ToString());
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06001D5D RID: 7517 RVA: 0x000B75C4 File Offset: 0x000B57C4
		[DebugOutput("Text generation", true)]
		public static void DatabaseTalesList()
		{
			Find.TaleManager.LogTales();
		}

		// Token: 0x06001D5E RID: 7518 RVA: 0x000B75D0 File Offset: 0x000B57D0
		[DebugOutput("Text generation", true)]
		public static void DatabaseTalesInterest()
		{
			Find.TaleManager.LogTaleInterestSummary();
		}

		// Token: 0x06001D5F RID: 7519 RVA: 0x000B75DC File Offset: 0x000B57DC
		[DebugOutput("Text generation", true)]
		public static void ArtDescsDatabaseTales()
		{
			DebugOutputsTextGen.LogTales(from t in Find.TaleManager.AllTalesListForReading
			where t.def.usableForArt
			select t);
		}

		// Token: 0x06001D60 RID: 7520 RVA: 0x000B7614 File Offset: 0x000B5814
		[DebugOutput("Text generation", true)]
		public static void ArtDescsRandomTales()
		{
			int num = 40;
			List<Tale> list = new List<Tale>();
			for (int i = 0; i < num; i++)
			{
				list.Add(TaleFactory.MakeRandomTestTale(null));
			}
			DebugOutputsTextGen.LogTales(list);
		}

		// Token: 0x06001D61 RID: 7521 RVA: 0x000B7648 File Offset: 0x000B5848
		[DebugOutput("Text generation", true)]
		public static void ArtDescsTaleless()
		{
			List<Tale> list = new List<Tale>();
			for (int i = 0; i < 20; i++)
			{
				list.Add(null);
			}
			DebugOutputsTextGen.LogTales(list);
		}

		// Token: 0x06001D62 RID: 7522 RVA: 0x000B7678 File Offset: 0x000B5878
		[DebugOutput("Text generation", false)]
		public static void InteractionLogs()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (List<InteractionDef>.Enumerator enumerator = DefDatabase<InteractionDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					InteractionDef def = enumerator.Current;
					list.Add(new DebugMenuOption(def.defName, DebugMenuOptionMode.Action, delegate()
					{
						StringBuilder stringBuilder = new StringBuilder();
						Pawn pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.Colonist, null);
						Pawn recipient = PawnGenerator.GeneratePawn(PawnKindDefOf.Colonist, null);
						for (int i = 0; i < 100; i++)
						{
							PlayLogEntry_Interaction playLogEntry_Interaction = new PlayLogEntry_Interaction(def, pawn, recipient, null);
							stringBuilder.AppendLine(playLogEntry_Interaction.ToGameStringFromPOV(pawn, false));
						}
						Log.Message(stringBuilder.ToString());
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001D63 RID: 7523 RVA: 0x000B7708 File Offset: 0x000B5908
		private static void LogSpecificTale(TaleDef def, int count)
		{
			List<Tale> list = new List<Tale>();
			for (int i = 0; i < count; i++)
			{
				list.Add(TaleFactory.MakeRandomTestTale(def));
			}
			DebugOutputsTextGen.LogTales(list);
		}

		// Token: 0x06001D64 RID: 7524 RVA: 0x000B773C File Offset: 0x000B593C
		private static void LogTales(IEnumerable<Tale> tales)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (Tale tale in tales)
			{
				TaleReference tr = new TaleReference(tale);
				stringBuilder.AppendLine(DebugOutputsTextGen.RandomArtworkName(tr));
				stringBuilder.AppendLine(DebugOutputsTextGen.RandomArtworkDescription(tr));
				stringBuilder.AppendLine();
				num++;
				if (num % 20 == 0)
				{
					Log.Message(stringBuilder.ToString());
					stringBuilder = new StringBuilder();
				}
			}
			if (!stringBuilder.ToString().NullOrEmpty())
			{
				Log.Message(stringBuilder.ToString());
			}
		}

		// Token: 0x06001D65 RID: 7525 RVA: 0x000B77E0 File Offset: 0x000B59E0
		private static string RandomArtworkName(TaleReference tr)
		{
			RulePackDef extraInclude = null;
			switch (Rand.RangeInclusive(0, 4))
			{
			case 0:
				extraInclude = RulePackDefOf.NamerArtSculpture;
				break;
			case 1:
				extraInclude = RulePackDefOf.NamerArtWeaponMelee;
				break;
			case 2:
				extraInclude = RulePackDefOf.NamerArtWeaponGun;
				break;
			case 3:
				extraInclude = RulePackDefOf.NamerArtFurniture;
				break;
			case 4:
				extraInclude = RulePackDefOf.NamerArtSarcophagusPlate;
				break;
			}
			return tr.GenerateText(TextGenerationPurpose.ArtName, extraInclude);
		}

		// Token: 0x06001D66 RID: 7526 RVA: 0x000B7848 File Offset: 0x000B5A48
		private static string RandomArtworkDescription(TaleReference tr)
		{
			RulePackDef extraInclude = null;
			switch (Rand.RangeInclusive(0, 4))
			{
			case 0:
				extraInclude = RulePackDefOf.ArtDescription_Sculpture;
				break;
			case 1:
				extraInclude = RulePackDefOf.ArtDescription_WeaponMelee;
				break;
			case 2:
				extraInclude = RulePackDefOf.ArtDescription_WeaponGun;
				break;
			case 3:
				extraInclude = RulePackDefOf.ArtDescription_Furniture;
				break;
			case 4:
				extraInclude = RulePackDefOf.ArtDescription_SarcophagusPlate;
				break;
			}
			return tr.GenerateText(TextGenerationPurpose.ArtDescription, extraInclude);
		}
	}
}
