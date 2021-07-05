using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001309 RID: 4873
	[StaticConstructorOnStartup]
	public static class HealthCardUtility
	{
		// Token: 0x0600751F RID: 29983 RVA: 0x00281180 File Offset: 0x0027F380
		public static void DrawPawnHealthCard(Rect outRect, Pawn pawn, bool allowOperations, bool showBloodLoss, Thing thingForMedBills)
		{
			if (pawn.Dead && allowOperations)
			{
				Log.Error("Called DrawPawnHealthCard with a dead pawn and allowOperations=true. Operations are disallowed on corpses.");
				allowOperations = false;
			}
			outRect = outRect.Rounded();
			Rect rect = new Rect(outRect.x, outRect.y, outRect.width * 0.375f, outRect.height).Rounded();
			Rect rect2 = new Rect(rect.xMax, outRect.y, outRect.width - rect.width, outRect.height);
			rect.yMin += 11f;
			HealthCardUtility.DrawHealthSummary(rect, pawn, allowOperations, thingForMedBills);
			HealthCardUtility.DrawHediffListing(rect2.ContractedBy(10f), pawn, showBloodLoss);
		}

		// Token: 0x06007520 RID: 29984 RVA: 0x00281230 File Offset: 0x0027F430
		public static void DrawHealthSummary(Rect rect, Pawn pawn, bool allowOperations, Thing thingForMedBills)
		{
			GUI.color = Color.white;
			if (!allowOperations)
			{
				HealthCardUtility.onOperationTab = false;
			}
			Widgets.DrawMenuSection(rect);
			List<TabRecord> list = new List<TabRecord>();
			list.Add(new TabRecord("HealthOverview".Translate(), delegate()
			{
				HealthCardUtility.onOperationTab = false;
			}, !HealthCardUtility.onOperationTab));
			if (allowOperations)
			{
				string label = pawn.RaceProps.IsMechanoid ? "MedicalOperationsMechanoidsShort".Translate(pawn.BillStack.Count) : "MedicalOperationsShort".Translate(pawn.BillStack.Count);
				list.Add(new TabRecord(label, delegate()
				{
					HealthCardUtility.onOperationTab = true;
				}, HealthCardUtility.onOperationTab));
			}
			TabDrawer.DrawTabs<TabRecord>(rect, list, 200f);
			rect = rect.ContractedBy(9f);
			GUI.BeginGroup(rect);
			float curY = 0f;
			Text.Font = GameFont.Medium;
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperCenter;
			if (HealthCardUtility.onOperationTab)
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.MedicalOperations, KnowledgeAmount.FrameDisplayed);
				curY = HealthCardUtility.DrawMedOperationsTab(rect, pawn, thingForMedBills, curY);
			}
			else
			{
				curY = HealthCardUtility.DrawOverviewTab(rect, pawn, curY);
			}
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.EndGroup();
		}

		// Token: 0x06007521 RID: 29985 RVA: 0x00281398 File Offset: 0x0027F598
		public static void DrawHediffListing(Rect rect, Pawn pawn, bool showBloodLoss)
		{
			GUI.color = Color.white;
			if (Prefs.DevMode && Current.ProgramState == ProgramState.Playing)
			{
				HealthCardUtility.DoDebugOptions(rect, pawn);
			}
			GUI.BeginGroup(rect);
			float lineHeight = Text.LineHeight;
			Rect outRect = new Rect(0f, 0f, rect.width, rect.height - lineHeight);
			Rect viewRect = new Rect(0f, 0f, rect.width - 16f, HealthCardUtility.scrollViewHeight);
			Rect rect2 = rect;
			if (viewRect.height > outRect.height)
			{
				rect2.width -= 16f;
			}
			Widgets.BeginScrollView(outRect, ref HealthCardUtility.scrollPosition, viewRect, true);
			GUI.color = Color.white;
			float b = 0f;
			HealthCardUtility.highlight = true;
			bool flag = false;
			if (Event.current.type == EventType.Layout)
			{
				HealthCardUtility.lastMaxIconsTotalWidth = 0f;
			}
			foreach (IGrouping<BodyPartRecord, Hediff> diffs in HealthCardUtility.VisibleHediffGroupsInOrder(pawn, showBloodLoss))
			{
				flag = true;
				HealthCardUtility.DrawHediffRow(rect2, pawn, diffs, ref b);
			}
			if (!flag)
			{
				Widgets.NoneLabelCenteredVertically(new Rect(0f, 0f, viewRect.width, outRect.height), "(" + "NoHealthConditions".Translate() + ")");
				b = outRect.height - 1f;
			}
			if (Event.current.type == EventType.Repaint)
			{
				HealthCardUtility.scrollViewHeight = b;
			}
			else if (Event.current.type == EventType.Layout)
			{
				HealthCardUtility.scrollViewHeight = Mathf.Max(HealthCardUtility.scrollViewHeight, b);
			}
			Widgets.EndScrollView();
			float bleedRateTotal = pawn.health.hediffSet.BleedRateTotal;
			if (bleedRateTotal > 0.01f)
			{
				Rect rect3 = new Rect(0f, rect.height - lineHeight, rect.width, lineHeight);
				string text = "BleedingRate".Translate() + ": " + bleedRateTotal.ToStringPercent() + "/" + "LetterDay".Translate();
				int num = HealthUtility.TicksUntilDeathDueToBloodLoss(pawn);
				if (num < 60000)
				{
					text += " (" + "TimeToDeath".Translate(num.ToStringTicksToPeriod(true, false, true, true)) + ")";
				}
				else
				{
					text += " (" + "WontBleedOutSoon".Translate() + ")";
				}
				Widgets.Label(rect3, text);
			}
			GUI.EndGroup();
			GUI.color = Color.white;
		}

		// Token: 0x06007522 RID: 29986 RVA: 0x00281664 File Offset: 0x0027F864
		private static IEnumerable<IGrouping<BodyPartRecord, Hediff>> VisibleHediffGroupsInOrder(Pawn pawn, bool showBloodLoss)
		{
			foreach (IGrouping<BodyPartRecord, Hediff> grouping in from x in HealthCardUtility.VisibleHediffs(pawn, showBloodLoss)
			group x by x.Part into x
			orderby HealthCardUtility.GetListPriority(x.First<Hediff>().Part) descending
			select x)
			{
				yield return grouping;
			}
			IEnumerator<IGrouping<BodyPartRecord, Hediff>> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06007523 RID: 29987 RVA: 0x0028167B File Offset: 0x0027F87B
		private static float GetListPriority(BodyPartRecord rec)
		{
			if (rec == null)
			{
				return 9999999f;
			}
			return (float)((int)rec.height * 10000) + rec.coverageAbsWithChildren;
		}

		// Token: 0x06007524 RID: 29988 RVA: 0x0028169A File Offset: 0x0027F89A
		private static IEnumerable<Hediff> VisibleHediffs(Pawn pawn, bool showBloodLoss)
		{
			if (!HealthCardUtility.showAllHediffs)
			{
				List<Hediff_MissingPart> mpca = pawn.health.hediffSet.GetMissingPartsCommonAncestors();
				int num;
				for (int i = 0; i < mpca.Count; i = num + 1)
				{
					yield return mpca[i];
					num = i;
				}
				IEnumerable<Hediff> enumerable = from d in pawn.health.hediffSet.hediffs
				where !(d is Hediff_MissingPart) && d.Visible && (showBloodLoss || d.def != HediffDefOf.BloodLoss)
				select d;
				foreach (Hediff hediff in enumerable)
				{
					yield return hediff;
				}
				IEnumerator<Hediff> enumerator = null;
				mpca = null;
			}
			else
			{
				foreach (Hediff hediff2 in pawn.health.hediffSet.hediffs)
				{
					yield return hediff2;
				}
				List<Hediff>.Enumerator enumerator2 = default(List<Hediff>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x06007525 RID: 29989 RVA: 0x002816B4 File Offset: 0x0027F8B4
		private static float DrawMedOperationsTab(Rect leftRect, Pawn pawn, Thing thingForMedBills, float curY)
		{
			curY += 2f;
			Func<List<FloatMenuOption>> recipeOptionsMaker = delegate()
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (RecipeDef recipeDef in thingForMedBills.def.AllRecipes)
				{
					if (recipeDef.AvailableNow && recipeDef.AvailableOnNow(pawn, null))
					{
						IEnumerable<ThingDef> enumerable = recipeDef.PotentiallyMissingIngredients(null, thingForMedBills.Map);
						if (!enumerable.Any((ThingDef x) => x.isTechHediff))
						{
							if (!enumerable.Any((ThingDef x) => x.IsDrug) && (!enumerable.Any<ThingDef>() || !recipeDef.dontShowIfAnyIngredientMissing))
							{
								if (recipeDef.targetsBodyPart)
								{
									using (IEnumerator<BodyPartRecord> enumerator2 = recipeDef.Worker.GetPartsToApplyOn(pawn, recipeDef).GetEnumerator())
									{
										while (enumerator2.MoveNext())
										{
											BodyPartRecord part = enumerator2.Current;
											if (recipeDef.AvailableOnNow(pawn, part))
											{
												list.Add(HealthCardUtility.GenerateSurgeryOption(pawn, thingForMedBills, recipeDef, enumerable, part));
											}
										}
										continue;
									}
								}
								list.Add(HealthCardUtility.GenerateSurgeryOption(pawn, thingForMedBills, recipeDef, enumerable, null));
							}
						}
					}
				}
				return list;
			};
			Rect rect = new Rect(leftRect.x - 9f, curY, leftRect.width, leftRect.height - curY - 20f);
			((IBillGiver)thingForMedBills).BillStack.DoListing(rect, recipeOptionsMaker, ref HealthCardUtility.billsScrollPosition, ref HealthCardUtility.billsScrollHeight);
			return curY;
		}

		// Token: 0x06007526 RID: 29990 RVA: 0x00281738 File Offset: 0x0027F938
		private static FloatMenuOption GenerateSurgeryOption(Pawn pawn, Thing thingForMedBills, RecipeDef recipe, IEnumerable<ThingDef> missingIngredients, BodyPartRecord part = null)
		{
			string text = recipe.Worker.GetLabelWhenUsedOn(pawn, part).CapitalizeFirst();
			if (part != null && !recipe.hideBodyPartNames)
			{
				text = text + " (" + part.Label + ")";
			}
			FloatMenuOption floatMenuOption;
			if (missingIngredients.Any<ThingDef>())
			{
				text += " (";
				bool flag = true;
				foreach (ThingDef thingDef in missingIngredients)
				{
					if (!flag)
					{
						text += ", ";
					}
					flag = false;
					text += "MissingMedicalBillIngredient".Translate(thingDef.label);
				}
				text += ")";
				floatMenuOption = new FloatMenuOption(text, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			else
			{
				Action action = delegate()
				{
					Pawn medPawn = thingForMedBills as Pawn;
					if (medPawn == null)
					{
						return;
					}
					HediffDef hediffDef = recipe.addsHediff ?? recipe.changesHediffLevel;
					if (hediffDef == null)
					{
						HealthCardUtility.CreateSurgeryBill(medPawn, recipe, part);
						return;
					}
					TaggedString text2 = CompRoyalImplant.CheckForViolations(medPawn, hediffDef, recipe.hediffLevelOffset);
					if (!text2.NullOrEmpty())
					{
						Find.WindowStack.Add(new Dialog_MessageBox(text2, "Yes".Translate(), delegate()
						{
							HealthCardUtility.CreateSurgeryBill(medPawn, recipe, part);
						}, "No".Translate(), null, null, false, null, null));
						return;
					}
					HealthCardUtility.CreateSurgeryBill(medPawn, recipe, part);
				};
				if (recipe.Worker is Recipe_AdministerIngestible)
				{
					IngredientCount ingredientCount = recipe.ingredients.FirstOrDefault<IngredientCount>();
					floatMenuOption = new FloatMenuOption(text, action, (ingredientCount != null) ? ingredientCount.FixedIngredient : null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				else if (recipe.Worker is Recipe_RemoveBodyPart)
				{
					floatMenuOption = new FloatMenuOption(text, action, part.def.spawnThingOnRemoved, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				else
				{
					floatMenuOption = new FloatMenuOption(text, action, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
			}
			floatMenuOption.extraPartWidth = 29f;
			floatMenuOption.extraPartOnGUI = ((Rect rect) => Widgets.InfoCardButton(rect.x + 5f, rect.y + (rect.height - 24f) / 2f, recipe));
			return floatMenuOption;
		}

		// Token: 0x06007527 RID: 29991 RVA: 0x00281920 File Offset: 0x0027FB20
		private static void CreateSurgeryBill(Pawn medPawn, RecipeDef recipe, BodyPartRecord part)
		{
			Bill_Medical bill_Medical = new Bill_Medical(recipe);
			medPawn.BillStack.AddBill(bill_Medical);
			bill_Medical.Part = part;
			if (recipe.conceptLearned != null)
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(recipe.conceptLearned, KnowledgeAmount.Total);
			}
			Map map = medPawn.Map;
			if (!map.mapPawns.FreeColonists.Any((Pawn col) => recipe.PawnSatisfiesSkillRequirements(col)))
			{
				Bill.CreateNoPawnsWithSkillDialog(recipe);
			}
			if (!medPawn.InBed() && medPawn.RaceProps.IsFlesh)
			{
				if (medPawn.RaceProps.Humanlike)
				{
					if (!map.listerBuildings.allBuildingsColonist.Any((Building x) => x is Building_Bed && RestUtility.CanUseBedEver(medPawn, x.def) && ((Building_Bed)x).Medical))
					{
						Messages.Message("MessageNoMedicalBeds".Translate(), medPawn, MessageTypeDefOf.CautionInput, false);
					}
				}
				else if (!map.listerBuildings.allBuildingsColonist.Any((Building x) => x is Building_Bed && RestUtility.CanUseBedEver(medPawn, x.def)))
				{
					Messages.Message("MessageNoAnimalBeds".Translate(), medPawn, MessageTypeDefOf.CautionInput, false);
				}
			}
			if (medPawn.Faction != null && !medPawn.Faction.Hidden && !medPawn.Faction.HostileTo(Faction.OfPlayer) && recipe.Worker.IsViolationOnPawn(medPawn, part, Faction.OfPlayer))
			{
				Messages.Message("MessageMedicalOperationWillAngerFaction".Translate(medPawn.HomeFaction), medPawn, MessageTypeDefOf.CautionInput, false);
			}
			ThingDef minRequiredMedicine = HealthCardUtility.GetMinRequiredMedicine(recipe);
			if (minRequiredMedicine != null && medPawn.playerSettings != null && !medPawn.playerSettings.medCare.AllowsMedicine(minRequiredMedicine))
			{
				Messages.Message("MessageTooLowMedCare".Translate(minRequiredMedicine.label, medPawn.LabelShort, medPawn.playerSettings.medCare.GetLabel(), medPawn.Named("PAWN")), medPawn, MessageTypeDefOf.CautionInput, false);
			}
			recipe.Worker.CheckForWarnings(medPawn);
		}

		// Token: 0x06007528 RID: 29992 RVA: 0x00281BBC File Offset: 0x0027FDBC
		private static ThingDef GetMinRequiredMedicine(RecipeDef recipe)
		{
			HealthCardUtility.tmpMedicineBestToWorst.Clear();
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].IsMedicine)
				{
					HealthCardUtility.tmpMedicineBestToWorst.Add(allDefsListForReading[i]);
				}
			}
			HealthCardUtility.tmpMedicineBestToWorst.SortByDescending((ThingDef x) => x.GetStatValueAbstract(StatDefOf.MedicalPotency, null));
			ThingDef thingDef = null;
			for (int j = 0; j < recipe.ingredients.Count; j++)
			{
				ThingDef thingDef2 = null;
				for (int k = 0; k < HealthCardUtility.tmpMedicineBestToWorst.Count; k++)
				{
					if (recipe.ingredients[j].filter.Allows(HealthCardUtility.tmpMedicineBestToWorst[k]))
					{
						thingDef2 = HealthCardUtility.tmpMedicineBestToWorst[k];
					}
				}
				if (thingDef2 != null && (thingDef == null || thingDef2.GetStatValueAbstract(StatDefOf.MedicalPotency, null) > thingDef.GetStatValueAbstract(StatDefOf.MedicalPotency, null)))
				{
					thingDef = thingDef2;
				}
			}
			HealthCardUtility.tmpMedicineBestToWorst.Clear();
			return thingDef;
		}

		// Token: 0x06007529 RID: 29993 RVA: 0x00281CCC File Offset: 0x0027FECC
		private static float DrawOverviewTab(Rect leftRect, Pawn pawn, float curY)
		{
			curY += 4f;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = new Color(0.9f, 0.9f, 0.9f);
			string str;
			if (pawn.gender != Gender.None)
			{
				str = "PawnSummaryWithGender".Translate(pawn.Named("PAWN"));
			}
			else
			{
				str = "PawnSummary".Translate(pawn.Named("PAWN"));
			}
			Rect rect = new Rect(0f, curY, leftRect.width, 34f);
			Widgets.Label(rect, str.CapitalizeFirst());
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, () => pawn.ageTracker.AgeTooltipString, 73412);
				Widgets.DrawHighlight(rect);
			}
			GUI.color = Color.white;
			curY += 34f;
			bool flag = pawn.RaceProps.IsFlesh && (pawn.Faction == Faction.OfPlayer || pawn.HostFaction == Faction.OfPlayer || (pawn.NonHumanlikeOrWildMan() && pawn.InBed() && pawn.CurrentBed().Faction == Faction.OfPlayer));
			if (pawn.foodRestriction != null && pawn.foodRestriction.Configurable)
			{
				Rect rect2 = new Rect(0f, curY, leftRect.width * 0.42f, 23f);
				Text.Anchor = TextAnchor.MiddleLeft;
				Widgets.Label(rect2, "FoodRestriction".Translate());
				GenUI.ResetLabelAlign();
				if (Widgets.ButtonText(new Rect(rect2.width, curY, leftRect.width - rect2.width, 23f), pawn.foodRestriction.CurrentFoodRestriction.label, true, true, true))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					List<FoodRestriction> allFoodRestrictions = Current.Game.foodRestrictionDatabase.AllFoodRestrictions;
					for (int i = 0; i < allFoodRestrictions.Count; i++)
					{
						FoodRestriction localRestriction = allFoodRestrictions[i];
						list.Add(new FloatMenuOption(localRestriction.label, delegate()
						{
							pawn.foodRestriction.CurrentFoodRestriction = localRestriction;
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					list.Add(new FloatMenuOption("ManageFoodRestrictions".Translate(), delegate()
					{
						Find.WindowStack.Add(new Dialog_ManageFoodRestrictions(null));
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					Find.WindowStack.Add(new FloatMenu(list));
				}
				curY += 23f;
			}
			if (pawn.IsColonist && !pawn.Dead)
			{
				bool selfTend = pawn.playerSettings.selfTend;
				Rect rect3 = new Rect(0f, curY, leftRect.width, 24f);
				Widgets.CheckboxLabeled(rect3, "SelfTend".Translate(), ref pawn.playerSettings.selfTend, false, null, null, false);
				if (pawn.playerSettings.selfTend && !selfTend)
				{
					if (pawn.WorkTypeIsDisabled(WorkTypeDefOf.Doctor))
					{
						pawn.playerSettings.selfTend = false;
						Messages.Message("MessageCannotSelfTendEver".Translate(pawn.LabelShort, pawn), MessageTypeDefOf.RejectInput, false);
					}
					else if (pawn.workSettings.GetPriority(WorkTypeDefOf.Doctor) == 0)
					{
						Messages.Message("MessageSelfTendUnsatisfied".Translate(pawn.LabelShort, pawn), MessageTypeDefOf.CautionInput, false);
					}
				}
				if (Mouse.IsOver(rect3))
				{
					TooltipHandler.TipRegion(rect3, "SelfTendTip".Translate(Faction.OfPlayer.def.pawnsPlural, 0.7f.ToStringPercent()).CapitalizeFirst());
				}
				curY += 28f;
			}
			if (flag && pawn.playerSettings != null && !pawn.Dead && Current.ProgramState == ProgramState.Playing)
			{
				MedicalCareUtility.MedicalCareSetter(new Rect(0f, curY, 140f, 28f), ref pawn.playerSettings.medCare);
				if (Widgets.ButtonText(new Rect(leftRect.width - 70f, curY, 70f, 28f), "MedGroupDefaults".Translate(), true, true, true))
				{
					Find.WindowStack.Add(new Dialog_MedicalDefaults());
				}
				curY += 32f;
			}
			Text.Font = GameFont.Small;
			if (pawn.def.race.IsFlesh)
			{
				Pair<string, Color> painLabel = HealthCardUtility.GetPainLabel(pawn);
				string painTip = HealthCardUtility.GetPainTip(pawn);
				curY = HealthCardUtility.DrawLeftRow(leftRect, curY, "PainLevel".Translate(), painLabel.First, painLabel.Second, painTip);
			}
			if (!pawn.Dead)
			{
				IEnumerable<PawnCapacityDef> source;
				if (pawn.def.race.Humanlike)
				{
					source = from x in DefDatabase<PawnCapacityDef>.AllDefs
					where x.showOnHumanlikes
					select x;
				}
				else if (pawn.def.race.Animal)
				{
					source = from x in DefDatabase<PawnCapacityDef>.AllDefs
					where x.showOnAnimals
					select x;
				}
				else
				{
					source = from x in DefDatabase<PawnCapacityDef>.AllDefs
					where x.showOnMechanoids
					select x;
				}
				foreach (PawnCapacityDef pawnCapacityDef in from act in source
				orderby act.listOrder
				select act)
				{
					if (PawnCapacityUtility.BodyCanEverDoCapacity(pawn.RaceProps.body, pawnCapacityDef))
					{
						PawnCapacityDef activityLocal = pawnCapacityDef;
						Pair<string, Color> efficiencyLabel = HealthCardUtility.GetEfficiencyLabel(pawn, pawnCapacityDef);
						Func<string> textGetter = delegate()
						{
							if (!pawn.Dead)
							{
								return HealthCardUtility.GetPawnCapacityTip(pawn, activityLocal);
							}
							return "";
						};
						curY = HealthCardUtility.DrawLeftRow(leftRect, curY, pawnCapacityDef.GetLabelFor(pawn.RaceProps.IsFlesh, pawn.RaceProps.Humanlike).CapitalizeFirst(), efficiencyLabel.First, efficiencyLabel.Second, new TipSignal(textGetter, pawn.thingIDNumber ^ (int)pawnCapacityDef.index));
					}
				}
			}
			return curY;
		}

		// Token: 0x0600752A RID: 29994 RVA: 0x00282440 File Offset: 0x00280640
		private static float DrawLeftRow(Rect leftRect, float curY, string leftLabel, string rightLabel, Color rightLabelColor, TipSignal tipSignal)
		{
			Rect rect = new Rect(0f, curY, leftRect.width, 20f);
			if (Mouse.IsOver(rect))
			{
				GUI.color = HealthCardUtility.HighlightColor;
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			GUI.color = Color.white;
			Widgets.Label(new Rect(0f, curY, leftRect.width * 0.65f, 30f), leftLabel);
			GUI.color = rightLabelColor;
			Widgets.Label(new Rect(leftRect.width * 0.65f, curY, leftRect.width * 0.35f, 30f), rightLabel);
			Rect rect2 = new Rect(0f, curY, leftRect.width, 20f);
			if (Mouse.IsOver(rect2))
			{
				TooltipHandler.TipRegion(rect2, tipSignal);
			}
			curY += 20f;
			return curY;
		}

		// Token: 0x0600752B RID: 29995 RVA: 0x00282518 File Offset: 0x00280718
		private static void DrawHediffRow(Rect rect, Pawn pawn, IEnumerable<Hediff> diffs, ref float curY)
		{
			HealthCardUtility.<>c__DisplayClass32_0 CS$<>8__locals1 = new HealthCardUtility.<>c__DisplayClass32_0();
			CS$<>8__locals1.pawn = pawn;
			float num = rect.width * 0.375f;
			float width = rect.width - num - HealthCardUtility.lastMaxIconsTotalWidth;
			CS$<>8__locals1.part = diffs.First<Hediff>().Part;
			float a;
			if (CS$<>8__locals1.part == null)
			{
				a = Text.CalcHeight("WholeBody".Translate(), num);
			}
			else
			{
				a = Text.CalcHeight(CS$<>8__locals1.part.LabelCap, num);
			}
			float b = 0f;
			float num2 = curY;
			float num3 = 0f;
			foreach (IGrouping<int, Hediff> source in from x in diffs
			group x by x.UIGroupKey)
			{
				int num4 = source.Count<Hediff>();
				string text = source.First<Hediff>().LabelCap;
				if (num4 != 1)
				{
					text = text + " x" + num4.ToString();
				}
				num3 += Text.CalcHeight(text, width);
			}
			b = num3;
			HealthCardUtility.DoRightRowHighlight(new Rect(0f, curY, rect.width, Mathf.Max(a, b)));
			if (CS$<>8__locals1.part != null)
			{
				GUI.color = HealthUtility.GetPartConditionLabel(CS$<>8__locals1.pawn, CS$<>8__locals1.part).Second;
				Widgets.Label(new Rect(0f, curY, num, 100f), CS$<>8__locals1.part.LabelCap);
			}
			else
			{
				GUI.color = HealthUtility.RedColor;
				Widgets.Label(new Rect(0f, curY, num, 100f), "WholeBody".Translate());
			}
			GUI.color = Color.white;
			foreach (IGrouping<int, Hediff> grouping in from x in diffs
			group x by x.UIGroupKey)
			{
				HealthCardUtility.<>c__DisplayClass32_1 CS$<>8__locals2 = new HealthCardUtility.<>c__DisplayClass32_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				int num5 = 0;
				Hediff hediff3 = null;
				CS$<>8__locals2.bleedingIcon = null;
				CS$<>8__locals2.stateIcon = null;
				CS$<>8__locals2.totalBleedRate = 0f;
				foreach (Hediff hediff2 in grouping)
				{
					if (num5 == 0)
					{
						hediff3 = hediff2;
					}
					CS$<>8__locals2.stateIcon = hediff2.StateIcon;
					if (hediff2.Bleeding)
					{
						CS$<>8__locals2.bleedingIcon = HealthCardUtility.BleedingIcon;
					}
					CS$<>8__locals2.totalBleedRate += hediff2.BleedRate;
					num5++;
				}
				string text2 = hediff3.LabelCap;
				if (num5 != 1)
				{
					text2 = text2 + " x" + num5.ToStringCached();
				}
				float num6 = Text.CalcHeight(text2, width);
				Rect rect2 = new Rect(num, curY, width, num6);
				Widgets.DrawHighlightIfMouseover(rect2);
				GUI.color = hediff3.LabelColor;
				Widgets.Label(rect2, text2);
				GUI.color = Color.white;
				CS$<>8__locals2.iconsRect = new Rect(rect2.x + 10f, rect2.y, rect.width - num - 10f, rect2.height);
				List<GenUI.AnonymousStackElement> list = new List<GenUI.AnonymousStackElement>();
				Hediff localHediff = hediff3;
				list.Add(new GenUI.AnonymousStackElement
				{
					drawer = delegate(Rect r)
					{
						float num8 = CS$<>8__locals2.iconsRect.width - (r.x - CS$<>8__locals2.iconsRect.x) - 20f;
						r = new Rect(CS$<>8__locals2.iconsRect.x + num8, r.y, 20f, 20f);
						Widgets.InfoCardButton(r, localHediff.def);
					},
					width = 20f
				});
				if (CS$<>8__locals2.bleedingIcon)
				{
					list.Add(new GenUI.AnonymousStackElement
					{
						drawer = delegate(Rect r)
						{
							float num8 = CS$<>8__locals2.iconsRect.width - (r.x - CS$<>8__locals2.iconsRect.x) - 20f;
							r = new Rect(CS$<>8__locals2.iconsRect.x + num8, r.y, 20f, 20f);
							GUI.DrawTexture(r.ContractedBy(GenMath.LerpDouble(0f, 0.6f, 5f, 0f, Mathf.Min(CS$<>8__locals2.totalBleedRate, 1f))), CS$<>8__locals2.bleedingIcon);
						},
						width = 20f
					});
				}
				if (CS$<>8__locals2.stateIcon.HasValue)
				{
					list.Add(new GenUI.AnonymousStackElement
					{
						drawer = delegate(Rect r)
						{
							float num8 = CS$<>8__locals2.iconsRect.width - (r.x - CS$<>8__locals2.iconsRect.x) - 20f;
							r = new Rect(CS$<>8__locals2.iconsRect.x + num8, r.y, 20f, 20f);
							GUI.color = CS$<>8__locals2.stateIcon.Color;
							GUI.DrawTexture(r, CS$<>8__locals2.stateIcon.Texture);
							GUI.color = Color.white;
						},
						width = 20f
					});
				}
				GenUI.DrawElementStack<GenUI.AnonymousStackElement>(CS$<>8__locals2.iconsRect, num6, list, delegate(Rect r, GenUI.AnonymousStackElement obj)
				{
					obj.drawer(r);
				}, (GenUI.AnonymousStackElement obj) => obj.width, 4f, 5f, true);
				HealthCardUtility.lastMaxIconsTotalWidth = Mathf.Max(HealthCardUtility.lastMaxIconsTotalWidth, list.Sum((GenUI.AnonymousStackElement x) => x.width + 5f) - 5f);
				if (Mouse.IsOver(rect2))
				{
					int num7 = 0;
					using (IEnumerator<Hediff> enumerator2 = grouping.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Hediff hediff = enumerator2.Current;
							TooltipHandler.TipRegion(rect2, new TipSignal(() => hediff.GetTooltip(CS$<>8__locals2.CS$<>8__locals1.pawn, HealthCardUtility.showHediffsDebugInfo), (int)curY + 7857 + num7++, TooltipPriority.Default));
						}
					}
					if (CS$<>8__locals2.CS$<>8__locals1.part != null)
					{
						Rect rect3 = rect2;
						Func<string> textGetter;
						if ((textGetter = CS$<>8__locals2.CS$<>8__locals1.<>9__8) == null)
						{
							textGetter = (CS$<>8__locals2.CS$<>8__locals1.<>9__8 = (() => HealthCardUtility.GetTooltip(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.CS$<>8__locals1.part)));
						}
						TooltipHandler.TipRegion(rect3, new TipSignal(textGetter, (int)curY + 7857 + ++num7, TooltipPriority.Pawn));
					}
				}
				if (Widgets.ButtonInvisible(rect2, HealthCardUtility.CanEntryBeClicked(grouping, CS$<>8__locals2.CS$<>8__locals1.pawn)))
				{
					HealthCardUtility.EntryClicked(grouping, CS$<>8__locals2.CS$<>8__locals1.pawn);
				}
				curY += num6;
			}
			GUI.color = Color.white;
			curY = num2 + Mathf.Max(a, b);
		}

		// Token: 0x0600752C RID: 29996 RVA: 0x00282B50 File Offset: 0x00280D50
		public static string GetPainTip(Pawn pawn)
		{
			return "PainLevel".Translate() + ": " + (pawn.health.hediffSet.PainTotal * 100f).ToString("F0") + "%";
		}

		// Token: 0x0600752D RID: 29997 RVA: 0x00282BA8 File Offset: 0x00280DA8
		private static string GetTooltip(Pawn pawn, BodyPartRecord part)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(part.LabelCap + ": ");
			stringBuilder.AppendLine(" " + pawn.health.hediffSet.GetPartHealth(part).ToString() + " / " + part.def.GetMaxHealth(pawn).ToString());
			float num = PawnCapacityUtility.CalculatePartEfficiency(pawn.health.hediffSet, part, false, null);
			if (num != 1f)
			{
				stringBuilder.AppendLine("Efficiency".Translate() + ": " + num.ToStringPercent());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600752E RID: 29998 RVA: 0x00282C64 File Offset: 0x00280E64
		public static string GetPawnCapacityTip(Pawn pawn, PawnCapacityDef capacity)
		{
			List<PawnCapacityUtility.CapacityImpactor> list = new List<PawnCapacityUtility.CapacityImpactor>();
			float eff = PawnCapacityUtility.CalculateCapacityLevel(pawn.health.hediffSet, capacity, list, false);
			list.RemoveAll(delegate(PawnCapacityUtility.CapacityImpactor x)
			{
				PawnCapacityUtility.CapacityImpactorCapacity capacityImpactorCapacity;
				return (capacityImpactorCapacity = (x as PawnCapacityUtility.CapacityImpactorCapacity)) != null && !capacityImpactorCapacity.capacity.CanShowOnPawn(pawn);
			});
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(capacity.GetLabelFor(pawn).CapitalizeFirst() + ": " + HealthCardUtility.GetEfficiencyEstimateLabel(eff));
			if (list.Count > 0)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("AffectedBy".Translate());
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] is PawnCapacityUtility.CapacityImpactorHediff)
					{
						stringBuilder.AppendLine(string.Format("  {0}", list[i].Readable(pawn)));
					}
				}
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j] is PawnCapacityUtility.CapacityImpactorBodyPartHealth)
					{
						stringBuilder.AppendLine(string.Format("  {0}", list[j].Readable(pawn)));
					}
				}
				for (int k = 0; k < list.Count; k++)
				{
					if (list[k] is PawnCapacityUtility.CapacityImpactorCapacity)
					{
						stringBuilder.AppendLine(string.Format("  {0}", list[k].Readable(pawn)));
					}
				}
				for (int l = 0; l < list.Count; l++)
				{
					if (list[l] is PawnCapacityUtility.CapacityImpactorPain)
					{
						stringBuilder.AppendLine(string.Format("  {0}", list[l].Readable(pawn)));
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600752F RID: 29999 RVA: 0x00282E30 File Offset: 0x00281030
		private static bool CanEntryBeClicked(IEnumerable<Hediff> diffs, Pawn pawn)
		{
			LogEntry combatLogEntry;
			Predicate<LogEntry> <>9__1;
			TaggedString taggedString;
			return HealthCardUtility.GetCombatLogInfo(diffs, out taggedString, out combatLogEntry) && combatLogEntry != null && Find.BattleLog.Battles.Any(delegate(Battle b)
			{
				if (b.Concerns(pawn))
				{
					List<LogEntry> entries = b.Entries;
					Predicate<LogEntry> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = ((LogEntry e) => e == combatLogEntry));
					}
					return entries.Any(predicate);
				}
				return false;
			});
		}

		// Token: 0x06007530 RID: 30000 RVA: 0x00282E84 File Offset: 0x00281084
		private static void EntryClicked(IEnumerable<Hediff> diffs, Pawn pawn)
		{
			LogEntry combatLogEntry;
			TaggedString taggedString;
			if (!HealthCardUtility.GetCombatLogInfo(diffs, out taggedString, out combatLogEntry) || combatLogEntry == null)
			{
				return;
			}
			Predicate<LogEntry> <>9__1;
			if (!Find.BattleLog.Battles.Any(delegate(Battle b)
			{
				if (b.Concerns(pawn))
				{
					List<LogEntry> entries = b.Entries;
					Predicate<LogEntry> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = ((LogEntry e) => e == combatLogEntry));
					}
					return entries.Any(predicate);
				}
				return false;
			}))
			{
				return;
			}
			ITab_Pawn_Log tab_Pawn_Log = InspectPaneUtility.OpenTab(typeof(ITab_Pawn_Log)) as ITab_Pawn_Log;
			if (tab_Pawn_Log == null)
			{
				return;
			}
			tab_Pawn_Log.SeekTo(combatLogEntry);
			tab_Pawn_Log.Highlight(combatLogEntry);
		}

		// Token: 0x06007531 RID: 30001 RVA: 0x00282F08 File Offset: 0x00281108
		public static bool GetCombatLogInfo(IEnumerable<Hediff> diffs, out TaggedString combatLogText, out LogEntry combatLogEntry)
		{
			combatLogText = null;
			combatLogEntry = null;
			foreach (Hediff hediff in diffs)
			{
				if ((hediff.combatLogEntry != null && hediff.combatLogEntry.Target != null) || (combatLogText.NullOrEmpty() && !hediff.combatLogText.NullOrEmpty()))
				{
					combatLogEntry = ((hediff.combatLogEntry != null) ? hediff.combatLogEntry.Target : null);
					combatLogText = hediff.combatLogText;
				}
				if (combatLogEntry != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007532 RID: 30002 RVA: 0x00282FB8 File Offset: 0x002811B8
		private static void DoRightRowHighlight(Rect rowRect)
		{
			if (HealthCardUtility.highlight)
			{
				GUI.color = HealthCardUtility.StaticHighlightColor;
				GUI.DrawTexture(rowRect, TexUI.HighlightTex);
			}
			HealthCardUtility.highlight = !HealthCardUtility.highlight;
		}

		// Token: 0x06007533 RID: 30003 RVA: 0x00282FE4 File Offset: 0x002811E4
		private static void DoDebugOptions(Rect rightRect, Pawn pawn)
		{
			Widgets.CheckboxLabeled(new Rect(rightRect.x, rightRect.y - 25f, 110f, 30f), "Dev: AllDiffs", ref HealthCardUtility.showAllHediffs, false, null, null, false);
			Widgets.CheckboxLabeled(new Rect(rightRect.x + 115f, rightRect.y - 25f, 120f, 30f), "DiffsDebugInfo", ref HealthCardUtility.showHediffsDebugInfo, false, null, null, false);
			if (Widgets.ButtonText(new Rect(rightRect.x + 240f, rightRect.y - 27f, 115f, 25f), "Debug info", true, true, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				Func<BodyPartRecord, string> <>9__5;
				Func<BodyPartRecord, string> <>9__8;
				Func<BodyPartRecord, string> <>9__10;
				Func<BodyPartRecord, string> <>9__11;
				Func<BodyPartRecord, string> <>9__12;
				Func<BodyPartRecord, string> <>9__13;
				list.Add(new FloatMenuOption("BodyPartRecord info", delegate()
				{
					float totalCorpseNutrition = StatDefOf.Nutrition.Worker.GetValueAbstract(pawn.RaceProps.corpseDef, null);
					IEnumerable<BodyPartRecord> allParts = pawn.RaceProps.body.AllParts;
					TableDataGetter<BodyPartRecord>[] array = new TableDataGetter<BodyPartRecord>[10];
					array[0] = new TableDataGetter<BodyPartRecord>("defName", (BodyPartRecord b) => b.def.defName);
					int num = 1;
					string label = "Coverage";
					Func<BodyPartRecord, string> getter;
					if ((getter = <>9__5) == null)
					{
						getter = (<>9__5 = ((BodyPartRecord b) => pawn.health.hediffSet.GetCoverageOfNotMissingNaturalParts(b).ToStringPercent()));
					}
					array[num] = new TableDataGetter<BodyPartRecord>(label, getter);
					array[2] = new TableDataGetter<BodyPartRecord>("Hit chance\n(this or any child)", (BodyPartRecord b) => b.coverageAbsWithChildren.ToStringPercent());
					array[3] = new TableDataGetter<BodyPartRecord>("Hit chance\n(this part)", (BodyPartRecord b) => b.coverageAbs.ToStringPercent());
					int num2 = 4;
					string label2 = "Efficiency";
					Func<BodyPartRecord, string> getter2;
					if ((getter2 = <>9__8) == null)
					{
						getter2 = (<>9__8 = ((BodyPartRecord b) => PawnCapacityUtility.CalculatePartEfficiency(pawn.health.hediffSet, b, false, null).ToStringPercent()));
					}
					array[num2] = new TableDataGetter<BodyPartRecord>(label2, getter2);
					array[5] = new TableDataGetter<BodyPartRecord>("Nutrition", (BodyPartRecord b) => FoodUtility.GetBodyPartNutrition(totalCorpseNutrition, pawn, b));
					int num3 = 6;
					string label3 = "Solid";
					Func<BodyPartRecord, string> getter3;
					if ((getter3 = <>9__10) == null)
					{
						getter3 = (<>9__10 = delegate(BodyPartRecord b)
						{
							if (!pawn.health.hediffSet.PartIsMissing(b))
							{
								return b.def.IsSolid(b, pawn.health.hediffSet.hediffs).ToStringCheckBlank();
							}
							return "";
						});
					}
					array[num3] = new TableDataGetter<BodyPartRecord>(label3, getter3);
					int num4 = 7;
					string label4 = "Skin covered";
					Func<BodyPartRecord, string> getter4;
					if ((getter4 = <>9__11) == null)
					{
						getter4 = (<>9__11 = delegate(BodyPartRecord b)
						{
							if (!pawn.health.hediffSet.PartIsMissing(b))
							{
								return b.def.IsSkinCovered(b, pawn.health.hediffSet).ToStringCheckBlank();
							}
							return "";
						});
					}
					array[num4] = new TableDataGetter<BodyPartRecord>(label4, getter4);
					int num5 = 8;
					string label5 = "Is missing";
					Func<BodyPartRecord, string> getter5;
					if ((getter5 = <>9__12) == null)
					{
						getter5 = (<>9__12 = ((BodyPartRecord b) => pawn.health.hediffSet.PartIsMissing(b).ToStringCheckBlank()));
					}
					array[num5] = new TableDataGetter<BodyPartRecord>(label5, getter5);
					int num6 = 9;
					string label6 = "Is missing parts";
					Func<BodyPartRecord, string> getter6;
					if ((getter6 = <>9__13) == null)
					{
						getter6 = (<>9__13 = ((BodyPartRecord b) => pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(b).ToStringCheckBlank()));
					}
					array[num6] = new TableDataGetter<BodyPartRecord>(label6, getter6);
					DebugTables.MakeTablesDialog<BodyPartRecord>(allParts, array);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				Func<BodyPartGroupDef, bool> <>9__14;
				Func<BodyPartGroupDef, string> <>9__16;
				list.Add(new FloatMenuOption("BodyPartGroupDef info", delegate()
				{
					IEnumerable<BodyPartGroupDef> allDefs = DefDatabase<BodyPartGroupDef>.AllDefs;
					Func<BodyPartGroupDef, bool> predicate;
					if ((predicate = <>9__14) == null)
					{
						predicate = (<>9__14 = ((BodyPartGroupDef x) => pawn.RaceProps.body.AllParts.Any((BodyPartRecord y) => y.groups.Contains(x))));
					}
					IEnumerable<BodyPartGroupDef> dataSources = allDefs.Where(predicate);
					TableDataGetter<BodyPartGroupDef>[] array = new TableDataGetter<BodyPartGroupDef>[2];
					array[0] = new TableDataGetter<BodyPartGroupDef>("defName", (BodyPartGroupDef b) => b.defName);
					int num = 1;
					string label = "Efficiency";
					Func<BodyPartGroupDef, string> getter;
					if ((getter = <>9__16) == null)
					{
						getter = (<>9__16 = ((BodyPartGroupDef b) => PawnCapacityUtility.CalculateNaturalPartsAverageEfficiency(pawn.health.hediffSet, b).ToStringPercent()));
					}
					array[num] = new TableDataGetter<BodyPartGroupDef>(label, getter);
					DebugTables.MakeTablesDialog<BodyPartGroupDef>(dataSources, array);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				list.Add(new FloatMenuOption("HediffGiver_Birthday", delegate()
				{
					List<TableDataGetter<HediffGiver_Birthday>> list2 = new List<TableDataGetter<HediffGiver_Birthday>>();
					list2.Add(new TableDataGetter<HediffGiver_Birthday>("label", (HediffGiver_Birthday b) => b.hediff.LabelCap));
					int num = 1;
					while ((float)num < pawn.RaceProps.lifeExpectancy + 20f)
					{
						int age = num;
						list2.Add(new TableDataGetter<HediffGiver_Birthday>("Chance at\n" + num, (HediffGiver_Birthday h) => h.DebugChanceToHaveAtAge(pawn, age).ToStringPercent()));
						num++;
					}
					list2.Add(new TableDataGetter<HediffGiver_Birthday>("Spacing", (HediffGiver_Birthday h) => ""));
					int num2 = 1;
					while ((float)num2 < pawn.RaceProps.lifeExpectancy + 20f)
					{
						int age = num2;
						list2.Add(new TableDataGetter<HediffGiver_Birthday>("Count at\n" + num2, delegate(HediffGiver_Birthday h)
						{
							float num3 = 0f;
							foreach (HediffGiverSetDef hediffGiverSetDef in pawn.RaceProps.hediffGiverSets)
							{
								foreach (HediffGiver_Birthday hediffGiver_Birthday in hediffGiverSetDef.hediffGivers.OfType<HediffGiver_Birthday>())
								{
									num3 += hediffGiver_Birthday.DebugChanceToHaveAtAge(pawn, age);
								}
							}
							return num3.ToStringDecimalIfSmall();
						}));
						num2++;
					}
					DebugTables.MakeTablesDialog<HediffGiver_Birthday>(pawn.RaceProps.hediffGiverSets.SelectMany((HediffGiverSetDef x) => x.hediffGivers.OfType<HediffGiver_Birthday>()), list2.ToArray());
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				if (pawn.health.immunity.ImmunityListForReading.Any<ImmunityRecord>())
				{
					list.Add(new FloatMenuOption("Immunities", delegate()
					{
						List<TableDataGetter<ImmunityRecord>> list2 = new List<TableDataGetter<ImmunityRecord>>();
						list2.Add(new TableDataGetter<ImmunityRecord>("hediff", (ImmunityRecord i) => i.hediffDef.LabelCap));
						list2.Add(new TableDataGetter<ImmunityRecord>("source", (ImmunityRecord i) => i.source.LabelCap));
						list2.Add(new TableDataGetter<ImmunityRecord>("immunity", (ImmunityRecord i) => i.immunity.ToStringPercent()));
						DebugTables.MakeTablesDialog<ImmunityRecord>(pawn.health.immunity.ImmunityListForReading, list2.ToArray());
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x06007534 RID: 30004 RVA: 0x00283180 File Offset: 0x00281380
		public static Pair<string, Color> GetEfficiencyLabel(Pawn pawn, PawnCapacityDef activity)
		{
			float level = pawn.health.capacities.GetLevel(activity);
			return new Pair<string, Color>(PawnCapacityUtility.CalculateCapacityLevel(pawn.health.hediffSet, activity, null, false).ToStringPercent(), HealthCardUtility.efficiencyToColor[HealthCardUtility.EfficiencyValueToEstimate(level)]);
		}

		// Token: 0x06007535 RID: 30005 RVA: 0x002831CC File Offset: 0x002813CC
		public static string GetEfficiencyEstimateLabel(float eff)
		{
			return HealthCardUtility.EfficiencyValueToEstimate(eff).ToString().Translate();
		}

		// Token: 0x06007536 RID: 30006 RVA: 0x002831F8 File Offset: 0x002813F8
		public static EfficiencyEstimate EfficiencyValueToEstimate(float eff)
		{
			if (eff <= 0f)
			{
				return EfficiencyEstimate.None;
			}
			if (eff < 0.4f)
			{
				return EfficiencyEstimate.VeryPoor;
			}
			if (eff < 0.7f)
			{
				return EfficiencyEstimate.Poor;
			}
			if (eff < 1f && !Mathf.Approximately(eff, 1f))
			{
				return EfficiencyEstimate.Weakened;
			}
			if (Mathf.Approximately(eff, 1f))
			{
				return EfficiencyEstimate.GoodCondition;
			}
			return EfficiencyEstimate.Enhanced;
		}

		// Token: 0x06007537 RID: 30007 RVA: 0x0028324C File Offset: 0x0028144C
		public static Pair<string, Color> GetPainLabel(Pawn pawn)
		{
			float painTotal = pawn.health.hediffSet.PainTotal;
			Color second = Color.white;
			string first;
			if (Mathf.Approximately(painTotal, 0f))
			{
				first = "NoPain".Translate();
				second = HealthUtility.GoodConditionColor;
			}
			else if (painTotal < 0.15f)
			{
				first = "LittlePain".Translate();
				second = Color.gray;
			}
			else if (painTotal < 0.4f)
			{
				first = "MediumPain".Translate();
				second = HealthCardUtility.MediumPainColor;
			}
			else if (painTotal < 0.8f)
			{
				first = "SeverePain".Translate();
				second = HealthCardUtility.SeverePainColor;
			}
			else
			{
				first = "ExtremePain".Translate();
				second = HealthUtility.RedColor;
			}
			return new Pair<string, Color>(first, second);
		}

		// Token: 0x040040B9 RID: 16569
		private static Vector2 scrollPosition = Vector2.zero;

		// Token: 0x040040BA RID: 16570
		private static float scrollViewHeight = 0f;

		// Token: 0x040040BB RID: 16571
		private static bool highlight = true;

		// Token: 0x040040BC RID: 16572
		private static bool onOperationTab = false;

		// Token: 0x040040BD RID: 16573
		private static Vector2 billsScrollPosition = Vector2.zero;

		// Token: 0x040040BE RID: 16574
		private static float billsScrollHeight = 1000f;

		// Token: 0x040040BF RID: 16575
		private static bool showAllHediffs = false;

		// Token: 0x040040C0 RID: 16576
		private static bool showHediffsDebugInfo = false;

		// Token: 0x040040C1 RID: 16577
		private static float lastMaxIconsTotalWidth;

		// Token: 0x040040C2 RID: 16578
		public const float TopPadding = 20f;

		// Token: 0x040040C3 RID: 16579
		private const float ThoughtLevelHeight = 25f;

		// Token: 0x040040C4 RID: 16580
		private const float ThoughtLevelSpacing = 4f;

		// Token: 0x040040C5 RID: 16581
		private const float IconSize = 20f;

		// Token: 0x040040C6 RID: 16582
		private static readonly Color HighlightColor = new Color(0.5f, 0.5f, 0.5f, 1f);

		// Token: 0x040040C7 RID: 16583
		private static readonly Color StaticHighlightColor = new Color(0.75f, 0.75f, 0.85f, 1f);

		// Token: 0x040040C8 RID: 16584
		private static readonly Color MediumPainColor = new Color(0.9f, 0.9f, 0f);

		// Token: 0x040040C9 RID: 16585
		private static readonly Color SeverePainColor = new Color(0.9f, 0.5f, 0f);

		// Token: 0x040040CA RID: 16586
		private static readonly Texture2D BleedingIcon = ContentFinder<Texture2D>.Get("UI/Icons/Medical/Bleeding", true);

		// Token: 0x040040CB RID: 16587
		private static readonly Dictionary<EfficiencyEstimate, Color> efficiencyToColor = new Dictionary<EfficiencyEstimate, Color>
		{
			{
				EfficiencyEstimate.None,
				ColorLibrary.RedReadable
			},
			{
				EfficiencyEstimate.VeryPoor,
				new Color(0.75f, 0.45f, 0.45f)
			},
			{
				EfficiencyEstimate.Poor,
				new Color(0.55f, 0.55f, 0.55f)
			},
			{
				EfficiencyEstimate.Weakened,
				new Color(0.7f, 0.7f, 0.7f)
			},
			{
				EfficiencyEstimate.GoodCondition,
				HealthUtility.GoodConditionColor
			},
			{
				EfficiencyEstimate.Enhanced,
				new Color(0.5f, 0.5f, 0.9f)
			}
		};

		// Token: 0x040040CC RID: 16588
		private static List<ThingDef> tmpMedicineBestToWorst = new List<ThingDef>();
	}
}
