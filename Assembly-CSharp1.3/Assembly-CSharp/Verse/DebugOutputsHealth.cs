using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020003AB RID: 939
	public static class DebugOutputsHealth
	{
		// Token: 0x06001CDB RID: 7387 RVA: 0x000AE4DC File Offset: 0x000AC6DC
		[DebugOutput]
		public static void Bodies()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (BodyDef localBd2 in DefDatabase<BodyDef>.AllDefs)
			{
				BodyDef localBd = localBd2;
				list.Add(new FloatMenuOption(localBd.defName, delegate()
				{
					IEnumerable<BodyPartRecord> dataSources = from d in localBd.AllParts
					orderby d.height descending
					select d;
					TableDataGetter<BodyPartRecord>[] array = new TableDataGetter<BodyPartRecord>[7];
					array[0] = new TableDataGetter<BodyPartRecord>("defName", (BodyPartRecord d) => d.def.defName);
					array[1] = new TableDataGetter<BodyPartRecord>("hitPoints\n(non-adjusted)", (BodyPartRecord d) => d.def.hitPoints);
					array[2] = new TableDataGetter<BodyPartRecord>("coverage", (BodyPartRecord d) => d.coverage.ToStringPercent());
					array[3] = new TableDataGetter<BodyPartRecord>("coverageAbsWithChildren", (BodyPartRecord d) => d.coverageAbsWithChildren.ToStringPercent());
					array[4] = new TableDataGetter<BodyPartRecord>("coverageAbs", (BodyPartRecord d) => d.coverageAbs.ToStringPercent());
					array[5] = new TableDataGetter<BodyPartRecord>("depth", (BodyPartRecord d) => d.depth.ToString());
					array[6] = new TableDataGetter<BodyPartRecord>("height", (BodyPartRecord d) => d.height.ToString());
					DebugTables.MakeTablesDialog<BodyPartRecord>(dataSources, array);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06001CDC RID: 7388 RVA: 0x000AE574 File Offset: 0x000AC774
		[DebugOutput]
		public static void InstallableBodyParts()
		{
			Func<RecipeDef, ThingDef> getThingDef = (RecipeDef r) => r.fixedIngredientFilter.AllowedThingDefs.FirstOrDefault<ThingDef>();
			Func<ThingDef, RecipeDef> recipeToMakeThing = (ThingDef t) => (from x in DefDatabase<RecipeDef>.AllDefs
			where x.ProducedThingDef == t
			select x).FirstOrDefault<RecipeDef>();
			Func<RecipeDef, bool> installsBodyPart = (RecipeDef r) => r.addsHediff != null && getThingDef(r) != null;
			IEnumerable<string> enumerable = (from r in DefDatabase<RecipeDef>.AllDefs
			where installsBodyPart(r) && !getThingDef(r).tradeTags.NullOrEmpty<string>()
			select r).SelectMany((RecipeDef x) => getThingDef(x).tradeTags).Distinct<string>();
			IEnumerable<string> enumerable2 = (from r in DefDatabase<RecipeDef>.AllDefs
			where installsBodyPart(r) && !getThingDef(r).techHediffsTags.NullOrEmpty<string>()
			select r).SelectMany((RecipeDef x) => getThingDef(x).techHediffsTags).Distinct<string>();
			Func<ThingDef, string> getMinCraftingSkill = delegate(ThingDef t)
			{
				if (recipeToMakeThing(t) == null || recipeToMakeThing(t).skillRequirements.NullOrEmpty<SkillRequirement>())
				{
					return string.Empty;
				}
				SkillRequirement skillRequirement = (from x in recipeToMakeThing(t).skillRequirements
				where x.skill == SkillDefOf.Crafting
				select x).FirstOrDefault<SkillRequirement>();
				if (skillRequirement != null)
				{
					return skillRequirement.minLevel.ToString();
				}
				return string.Empty;
			};
			List<TableDataGetter<RecipeDef>> list = new List<TableDataGetter<RecipeDef>>();
			list.Add(new TableDataGetter<RecipeDef>("thingDef", (RecipeDef r) => getThingDef(r).defName));
			list.Add(new TableDataGetter<RecipeDef>("hediffDef", delegate(RecipeDef r)
			{
				if (r.addsHediff != null)
				{
					return r.addsHediff.defName;
				}
				return "";
			}));
			list.Add(new TableDataGetter<RecipeDef>("mkt val", (RecipeDef r) => getThingDef(r).BaseMarketValue.ToStringMoney(null)));
			list.Add(new TableDataGetter<RecipeDef>("tech lvl", (RecipeDef r) => getThingDef(r).techLevel.ToString()));
			list.Add(new TableDataGetter<RecipeDef>("mass", (RecipeDef r) => getThingDef(r).BaseMass));
			list.Add(new TableDataGetter<RecipeDef>("work to\nmake", (RecipeDef r) => r.workAmount.ToString()));
			list.Add(new TableDataGetter<RecipeDef>("min skill\ncrft", (RecipeDef r) => getMinCraftingSkill(getThingDef(r))));
			list.Add(new TableDataGetter<RecipeDef>("stuff costs", delegate(RecipeDef r)
			{
				if (!getThingDef(r).CostList.NullOrEmpty<ThingDefCountClass>())
				{
					return (from x in getThingDef(r).CostList
					select x.Summary).ToCommaList(false, false);
				}
				return "";
			}));
			list.Add(new TableDataGetter<RecipeDef>("tradeable", (RecipeDef r) => getThingDef(r).tradeability.ToString()));
			list.Add(new TableDataGetter<RecipeDef>("recipeDef", (RecipeDef r) => r.defName));
			list.Add(new TableDataGetter<RecipeDef>("death on\nfail %", (RecipeDef r) => r.deathOnFailedSurgeryChance.ToStringPercent()));
			list.Add(new TableDataGetter<RecipeDef>("surg sccss\nfctr", (RecipeDef r) => r.surgerySuccessChanceFactor.ToString()));
			list.Add(new TableDataGetter<RecipeDef>("min skill", (RecipeDef r) => r.MinSkillString.TrimEndNewlines().TrimStart(new char[]
			{
				' '
			})));
			list.Add(new TableDataGetter<RecipeDef>("research\nprereq", delegate(RecipeDef r)
			{
				if (recipeToMakeThing(getThingDef(r)) == null)
				{
					return "";
				}
				if (recipeToMakeThing(getThingDef(r)).researchPrerequisite != null)
				{
					return recipeToMakeThing(getThingDef(r)).researchPrerequisite.defName;
				}
				return "";
			}));
			list.Add(new TableDataGetter<RecipeDef>("research\nprereqs", delegate(RecipeDef r)
			{
				if (recipeToMakeThing(getThingDef(r)) == null)
				{
					return "";
				}
				if (!recipeToMakeThing(getThingDef(r)).researchPrerequisites.NullOrEmpty<ResearchProjectDef>())
				{
					return (from x in recipeToMakeThing(getThingDef(r)).researchPrerequisites
					select x.defName).ToCommaList(false, false);
				}
				return "";
			}));
			list.Add(new TableDataGetter<RecipeDef>("recipe\nusers", delegate(RecipeDef r)
			{
				if (recipeToMakeThing(getThingDef(r)) != null)
				{
					return (from x in recipeToMakeThing(getThingDef(r)).AllRecipeUsers
					select x.defName).ToCommaList(false, false);
				}
				return "";
			}));
			List<TableDataGetter<RecipeDef>> list2 = list;
			using (IEnumerator<string> enumerator = enumerable2.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string c = enumerator.Current;
					TableDataGetter<RecipeDef> item = new TableDataGetter<RecipeDef>("techHediff\n" + c.Shorten(), (RecipeDef r) => (!getThingDef(r).techHediffsTags.NullOrEmpty<string>() && getThingDef(r).techHediffsTags.Contains(c)).ToStringCheckBlank());
					list2.Add(item);
				}
			}
			using (IEnumerator<string> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string c = enumerator.Current;
					TableDataGetter<RecipeDef> item2 = new TableDataGetter<RecipeDef>("trade\n" + c.Shorten(), (RecipeDef r) => (!getThingDef(r).tradeTags.NullOrEmpty<string>() && getThingDef(r).tradeTags.Contains(c)).ToStringCheckBlank());
					list2.Add(item2);
				}
			}
			DebugTables.MakeTablesDialog<RecipeDef>(from r in DefDatabase<RecipeDef>.AllDefs
			where getThingDef(r) != null && installsBodyPart(r)
			select r, list2.ToArray());
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x000AE998 File Offset: 0x000ACB98
		[DebugOutput]
		public static void BodyParts()
		{
			IEnumerable<BodyPartDef> allDefs = DefDatabase<BodyPartDef>.AllDefs;
			TableDataGetter<BodyPartDef>[] array = new TableDataGetter<BodyPartDef>[16];
			array[0] = new TableDataGetter<BodyPartDef>("defName", (BodyPartDef d) => d.defName);
			array[1] = new TableDataGetter<BodyPartDef>("hit\npoints", (BodyPartDef d) => d.hitPoints);
			array[2] = new TableDataGetter<BodyPartDef>("bleeding\nate\nmultiplier", (BodyPartDef d) => d.bleedRate.ToStringPercent());
			array[3] = new TableDataGetter<BodyPartDef>("perm injury\nchance factor", (BodyPartDef d) => d.permanentInjuryChanceFactor.ToStringPercent());
			array[4] = new TableDataGetter<BodyPartDef>("frostbite\nvulnerability", (BodyPartDef d) => d.frostbiteVulnerability);
			array[5] = new TableDataGetter<BodyPartDef>("solid", delegate(BodyPartDef d)
			{
				if (!d.IsSolidInDefinition_Debug)
				{
					return "";
				}
				return "S";
			});
			array[6] = new TableDataGetter<BodyPartDef>("beauty\nrelated", delegate(BodyPartDef d)
			{
				if (!d.beautyRelated)
				{
					return "";
				}
				return "B";
			});
			array[7] = new TableDataGetter<BodyPartDef>("alive", delegate(BodyPartDef d)
			{
				if (!d.alive)
				{
					return "";
				}
				return "A";
			});
			array[8] = new TableDataGetter<BodyPartDef>("conceptual", delegate(BodyPartDef d)
			{
				if (!d.conceptual)
				{
					return "";
				}
				return "C";
			});
			array[9] = new TableDataGetter<BodyPartDef>("can\nsuggest\namputation", delegate(BodyPartDef d)
			{
				if (!d.canSuggestAmputation)
				{
					return "no A";
				}
				return "";
			});
			array[10] = new TableDataGetter<BodyPartDef>("socketed", delegate(BodyPartDef d)
			{
				if (!d.socketed)
				{
					return "";
				}
				return "DoL";
			});
			array[11] = new TableDataGetter<BodyPartDef>("skin covered", delegate(BodyPartDef d)
			{
				if (!d.IsSkinCoveredInDefinition_Debug)
				{
					return "";
				}
				return "skin";
			});
			array[12] = new TableDataGetter<BodyPartDef>("pawn generator\ncan amputate", delegate(BodyPartDef d)
			{
				if (!d.pawnGeneratorCanAmputate)
				{
					return "";
				}
				return "amp";
			});
			array[13] = new TableDataGetter<BodyPartDef>("spawn thing\non removed", (BodyPartDef d) => d.spawnThingOnRemoved);
			array[14] = new TableDataGetter<BodyPartDef>("hitChanceFactors", delegate(BodyPartDef d)
			{
				if (d.hitChanceFactors != null)
				{
					return (from kvp in d.hitChanceFactors
					select kvp.ToString()).ToCommaList(false, false);
				}
				return "";
			});
			array[15] = new TableDataGetter<BodyPartDef>("tags", delegate(BodyPartDef d)
			{
				if (d.tags != null)
				{
					return (from t in d.tags
					select t.defName).ToCommaList(false, false);
				}
				return "";
			});
			DebugTables.MakeTablesDialog<BodyPartDef>(allDefs, array);
		}

		// Token: 0x06001CDE RID: 7390 RVA: 0x000AEC80 File Offset: 0x000ACE80
		[DebugOutput]
		public static void Surgeries()
		{
			IEnumerable<RecipeDef> dataSources = from d in DefDatabase<RecipeDef>.AllDefs
			where d.IsSurgery
			orderby d.WorkAmountTotal(null) descending
			select d;
			TableDataGetter<RecipeDef>[] array = new TableDataGetter<RecipeDef>[6];
			array[0] = new TableDataGetter<RecipeDef>("defName", (RecipeDef d) => d.defName);
			array[1] = new TableDataGetter<RecipeDef>("work", (RecipeDef d) => d.WorkAmountTotal(null).ToString("F0"));
			array[2] = new TableDataGetter<RecipeDef>("ingredients", (RecipeDef d) => (from ing in d.ingredients
			select ing.ToString()).ToCommaList(false, false));
			array[3] = new TableDataGetter<RecipeDef>("skillRequirements", delegate(RecipeDef d)
			{
				if (d.skillRequirements != null)
				{
					return (from ing in d.skillRequirements
					select ing.ToString()).ToCommaList(false, false);
				}
				return "-";
			});
			array[4] = new TableDataGetter<RecipeDef>("surgerySuccessChanceFactor", (RecipeDef d) => d.surgerySuccessChanceFactor.ToStringPercent());
			array[5] = new TableDataGetter<RecipeDef>("deathOnFailChance", (RecipeDef d) => d.deathOnFailedSurgeryChance.ToStringPercent());
			DebugTables.MakeTablesDialog<RecipeDef>(dataSources, array);
		}

		// Token: 0x06001CDF RID: 7391 RVA: 0x000AEDF0 File Offset: 0x000ACFF0
		[DebugOutput]
		public static void HitsToKill()
		{
			Dictionary<ThingDef, <>f__AnonymousType1<ThingDef, float, int>> data = (from d in DefDatabase<ThingDef>.AllDefs
			where d.race != null
			select d).Select(delegate(ThingDef x)
			{
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < 15; i++)
				{
					Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(x.race.AnyPawnKind, null, PawnGenerationContext.NonPlayer, -1, true, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, null, false, false));
					for (int j = 0; j < 1000; j++)
					{
						DamageInfo dinfo = new DamageInfo(DamageDefOf.Crush, 10f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true);
						dinfo.SetIgnoreInstantKillProtection(true);
						pawn.TakeDamage(dinfo);
						if (pawn.Destroyed)
						{
							num += j + 1;
							break;
						}
					}
					if (!pawn.Destroyed)
					{
						Log.Error("Could not kill pawn " + pawn.ToStringSafe<Pawn>());
					}
					if (pawn.health.ShouldBeDeadFromLethalDamageThreshold())
					{
						num2++;
					}
					if (Find.WorldPawns.Contains(pawn))
					{
						Find.WorldPawns.RemovePawn(pawn);
					}
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
				}
				float hits = (float)num / 15f;
				return new
				{
					Race = x,
					Hits = hits,
					DiedDueToDamageThreshold = num2
				};
			}).ToDictionary(x => x.Race);
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.race != null
			orderby d.race.baseHealthScale descending
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[4];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("10 damage hits", (ThingDef d) => data[d].Hits.ToString("F0"));
			array[2] = new TableDataGetter<ThingDef>("died due to\ndam. thresh.", (ThingDef d) => data[d].DiedDueToDamageThreshold + "/" + 15);
			array[3] = new TableDataGetter<ThingDef>("mech", delegate(ThingDef d)
			{
				if (!d.race.IsMechanoid)
				{
					return "";
				}
				return "mech";
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06001CE0 RID: 7392 RVA: 0x000AEF5C File Offset: 0x000AD15C
		[DebugOutput]
		public static void Prosthetics()
		{
			PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.Colonist, Faction.OfPlayer, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, 0f, null, 1f, (Pawn p) => p.health.hediffSet.hediffs.Count == 0, null, null, null, null, null, null, null, null, null, null, null, null, false, false);
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			Action refreshPawn = delegate()
			{
				while (pawn.health.hediffSet.hediffs.Count > 0)
				{
					pawn.health.RemoveHediff(pawn.health.hediffSet.hediffs[0]);
				}
			};
			Func<BodyPartDef, IEnumerable<BodyPartRecord>> <>9__13;
			Func<BodyPartGroupDef, IEnumerable<BodyPartRecord>> <>9__14;
			Func<RecipeDef, BodyPartRecord> getApplicationPoint = delegate(RecipeDef recipe)
			{
				IEnumerable<BodyPartDef> appliedOnFixedBodyParts = recipe.appliedOnFixedBodyParts;
				Func<BodyPartDef, IEnumerable<BodyPartRecord>> selector;
				if ((selector = <>9__13) == null)
				{
					selector = (<>9__13 = ((BodyPartDef bpd) => pawn.def.race.body.GetPartsWithDef(bpd)));
				}
				IEnumerable<BodyPartRecord> first = appliedOnFixedBodyParts.SelectMany(selector);
				IEnumerable<BodyPartGroupDef> appliedOnFixedBodyPartGroups = recipe.appliedOnFixedBodyPartGroups;
				Func<BodyPartGroupDef, IEnumerable<BodyPartRecord>> selector2;
				if ((selector2 = <>9__14) == null)
				{
					selector2 = (<>9__14 = ((BodyPartGroupDef g) => from r in pawn.def.race.body.AllParts
					where r.groups != null && r.groups.Contains(g)
					select r));
				}
				return first.Concat(appliedOnFixedBodyPartGroups.SelectMany(selector2)).FirstOrDefault<BodyPartRecord>();
			};
			Func<RecipeDef, ThingDef> getProstheticItem = (RecipeDef recipe) => (from ic in recipe.ingredients
			select ic.filter.AnyAllowedDef).FirstOrDefault((ThingDef td) => !td.IsMedicine);
			List<TableDataGetter<RecipeDef>> list = new List<TableDataGetter<RecipeDef>>();
			list.Add(new TableDataGetter<RecipeDef>("defName", (RecipeDef r) => r.defName));
			list.Add(new TableDataGetter<RecipeDef>("price", delegate(RecipeDef r)
			{
				ThingDef thingDef = getProstheticItem(r);
				if (thingDef == null)
				{
					return 0f;
				}
				return thingDef.BaseMarketValue;
			}));
			list.Add(new TableDataGetter<RecipeDef>("install time", (RecipeDef r) => r.workAmount));
			list.Add(new TableDataGetter<RecipeDef>("install total cost", delegate(RecipeDef r)
			{
				float num = r.ingredients.Sum((IngredientCount ic) => ic.filter.AnyAllowedDef.BaseMarketValue * ic.GetBaseCount());
				float num2 = r.workAmount * 0.0036f;
				return num + num2;
			}));
			list.Add(new TableDataGetter<RecipeDef>("install skill", (RecipeDef r) => (from sr in r.skillRequirements
			select sr.minLevel).Max()));
			using (IEnumerator<PawnCapacityDef> enumerator = (from pc in DefDatabase<PawnCapacityDef>.AllDefs
			orderby pc.listOrder
			select pc).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PawnCapacityDef cap = enumerator.Current;
					list.Add(new TableDataGetter<RecipeDef>(cap.defName, delegate(RecipeDef r)
					{
						refreshPawn();
						r.Worker.ApplyOnPawn(pawn, getApplicationPoint(r), null, null, null);
						float num = pawn.health.capacities.GetLevel(cap) - 1f;
						if ((double)Math.Abs(num) > 0.001)
						{
							return num.ToStringPercent();
						}
						refreshPawn();
						BodyPartRecord bodyPartRecord = getApplicationPoint(r);
						pawn.TakeDamage(new DamageInfo(DamageDefOf.ExecutionCut, pawn.health.hediffSet.GetPartHealth(bodyPartRecord) / 2f, 999f, -1f, null, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
						List<PawnCapacityUtility.CapacityImpactor> list2 = new List<PawnCapacityUtility.CapacityImpactor>();
						PawnCapacityUtility.CalculateCapacityLevel(pawn.health.hediffSet, cap, list2, false);
						if (list2.Any((PawnCapacityUtility.CapacityImpactor imp) => imp.IsDirect))
						{
							return 0f.ToStringPercent();
						}
						return "";
					}));
				}
			}
			list.Add(new TableDataGetter<RecipeDef>("tech level", delegate(RecipeDef r)
			{
				if (getProstheticItem(r) != null)
				{
					return getProstheticItem(r).techLevel.ToStringHuman();
				}
				return "";
			}));
			list.Add(new TableDataGetter<RecipeDef>("thingSetMakerTags", delegate(RecipeDef r)
			{
				if (getProstheticItem(r) != null)
				{
					return getProstheticItem(r).thingSetMakerTags.ToCommaList(false, false);
				}
				return "";
			}));
			list.Add(new TableDataGetter<RecipeDef>("techHediffsTags", delegate(RecipeDef r)
			{
				if (getProstheticItem(r) != null)
				{
					return getProstheticItem(r).techHediffsTags.ToCommaList(false, false);
				}
				return "";
			}));
			DebugTables.MakeTablesDialog<RecipeDef>(from r in ThingDefOf.Human.AllRecipes
			where r.workerClass == typeof(Recipe_InstallArtificialBodyPart) || r.workerClass == typeof(Recipe_InstallNaturalBodyPart)
			select r, list.ToArray());
			Messages.Clear();
		}

		// Token: 0x06001CE1 RID: 7393 RVA: 0x000AF25C File Offset: 0x000AD45C
		[DebugOutput]
		public static void TranshumanistBodyParts()
		{
			IEnumerable<HediffDef> allDefs = DefDatabase<HediffDef>.AllDefs;
			List<TableDataGetter<HediffDef>> list = new List<TableDataGetter<HediffDef>>();
			list.Add(new TableDataGetter<HediffDef>("defName", (HediffDef h) => h.defName));
			list.Add(new TableDataGetter<HediffDef>("cares", (HediffDef h) => h.countsAsAddedPartOrImplant.ToStringCheckBlank()));
			DebugTables.MakeTablesDialog<HediffDef>(allDefs, list.ToArray());
		}
	}
}
