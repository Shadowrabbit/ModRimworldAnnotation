using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A39 RID: 6713
	[StaticConstructorOnStartup]
	public static class HealthCardUtility
	{
		// Token: 0x0600940E RID: 37902 RVA: 0x002ACC40 File Offset: 0x002AAE40
		public static void DrawPawnHealthCard(Rect outRect, Pawn pawn, bool allowOperations, bool showBloodLoss, Thing thingForMedBills)
		{
			if (pawn.Dead && allowOperations)
			{
				Log.Error("Called DrawPawnHealthCard with a dead pawn and allowOperations=true. Operations are disallowed on corpses.", false);
				allowOperations = false;
			}
			outRect = outRect.Rounded();
			Rect rect = new Rect(outRect.x, outRect.y, outRect.width * 0.375f, outRect.height).Rounded();
			Rect rect2 = new Rect(rect.xMax, outRect.y, outRect.width - rect.width, outRect.height);
			rect.yMin += 11f;
			HealthCardUtility.DrawHealthSummary(rect, pawn, allowOperations, thingForMedBills);
			HealthCardUtility.DrawHediffListing(rect2.ContractedBy(10f), pawn, showBloodLoss);
		}

		// Token: 0x0600940F RID: 37903 RVA: 0x002ACCF4 File Offset: 0x002AAEF4
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
			TabDrawer.DrawTabs(rect, list, 200f);
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

		// Token: 0x06009410 RID: 37904 RVA: 0x002ACE5C File Offset: 0x002AB05C
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

		// Token: 0x06009411 RID: 37905 RVA: 0x00063159 File Offset: 0x00061359
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

		// Token: 0x06009412 RID: 37906 RVA: 0x00063170 File Offset: 0x00061370
		private static float GetListPriority(BodyPartRecord rec)
		{
			if (rec == null)
			{
				return 9999999f;
			}
			return (float)((int)rec.height * 10000) + rec.coverageAbsWithChildren;
		}

		// Token: 0x06009413 RID: 37907 RVA: 0x0006318F File Offset: 0x0006138F
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

		// Token: 0x06009414 RID: 37908 RVA: 0x002AD128 File Offset: 0x002AB328
		private static float DrawMedOperationsTab(Rect leftRect, Pawn pawn, Thing thingForMedBills, float curY)
		{
			curY += 2f;
			Func<List<FloatMenuOption>> recipeOptionsMaker = delegate()
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (RecipeDef recipeDef in thingForMedBills.def.AllRecipes)
				{
					if (recipeDef.AvailableNow && recipeDef.AvailableOnNow(pawn))
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
											list.Add(HealthCardUtility.GenerateSurgeryOption(pawn, thingForMedBills, recipeDef, enumerable, part));
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

		// Token: 0x06009415 RID: 37909 RVA: 0x002AD1AC File Offset: 0x002AB3AC
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
				floatMenuOption = new FloatMenuOption(text, null, MenuOptionPriority.Default, null, null, 0f, null, null);
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
					floatMenuOption = new FloatMenuOption(text, action, (ingredientCount != null) ? ingredientCount.FixedIngredient : null, MenuOptionPriority.Default, null, null, 0f, null, null);
				}
				else if (recipe.Worker is Recipe_RemoveBodyPart)
				{
					floatMenuOption = new FloatMenuOption(text, action, part.def.spawnThingOnRemoved, MenuOptionPriority.Default, null, null, 0f, null, null);
				}
				else
				{
					floatMenuOption = new FloatMenuOption(text, action, null, MenuOptionPriority.Default, null, null, 0f, null, null);
				}
			}
			floatMenuOption.extraPartWidth = 29f;
			floatMenuOption.extraPartOnGUI = ((Rect rect) => Widgets.InfoCardButton(rect.x + 5f, rect.y + (rect.height - 24f) / 2f, recipe));
			return floatMenuOption;
		}

		// Token: 0x06009416 RID: 37910 RVA: 0x002AD38C File Offset: 0x002AB58C
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
				Messages.Message("MessageMedicalOperationWillAngerFaction".Translate(medPawn.FactionOrExtraMiniOrHomeFaction), medPawn, MessageTypeDefOf.CautionInput, false);
			}
			ThingDef minRequiredMedicine = HealthCardUtility.GetMinRequiredMedicine(recipe);
			if (minRequiredMedicine != null && medPawn.playerSettings != null && !medPawn.playerSettings.medCare.AllowsMedicine(minRequiredMedicine))
			{
				Messages.Message("MessageTooLowMedCare".Translate(minRequiredMedicine.label, medPawn.LabelShort, medPawn.playerSettings.medCare.GetLabel(), medPawn.Named("PAWN")), medPawn, MessageTypeDefOf.CautionInput, false);
			}
			recipe.Worker.CheckForWarnings(medPawn);
		}

		// Token: 0x06009417 RID: 37911 RVA: 0x002AD628 File Offset: 0x002AB828
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

		// Token: 0x06009418 RID: 37912 RVA: 0x002AD738 File Offset: 0x002AB938
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
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					list.Add(new FloatMenuOption("ManageFoodRestrictions".Translate(), delegate()
					{
						Find.WindowStack.Add(new Dialog_ManageFoodRestrictions(null));
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
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

		// Token: 0x06009419 RID: 37913 RVA: 0x002ADEA8 File Offset: 0x002AC0A8
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

		// Token: 0x0600941A RID: 37914 RVA: 0x002ADF80 File Offset: 0x002AC180
		private static void DrawHediffRow(Rect rect, Pawn pawn, IEnumerable<Hediff> diffs, ref float curY)
		{
			float num = rect.width * 0.375f;
			float width = rect.width - num - HealthCardUtility.lastMaxIconsTotalWidth;
			BodyPartRecord part = diffs.First<Hediff>().Part;
			float a;
			if (part == null)
			{
				a = Text.CalcHeight("WholeBody".Translate(), num);
			}
			else
			{
				a = Text.CalcHeight(part.LabelCap, num);
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
			Rect rect2 = new Rect(0f, curY, rect.width, Mathf.Max(a, b));
			HealthCardUtility.DoRightRowHighlight(rect2);
			if (part != null)
			{
				GUI.color = HealthUtility.GetPartConditionLabel(pawn, part).Second;
				Widgets.Label(new Rect(0f, curY, num, 100f), part.LabelCap);
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
				int num5 = 0;
				Hediff hediff = null;
				Texture2D bleedingIcon = null;
				TextureAndColor stateIcon = null;
				float totalBleedRate = 0f;
				foreach (Hediff hediff2 in grouping)
				{
					if (num5 == 0)
					{
						hediff = hediff2;
					}
					stateIcon = hediff2.StateIcon;
					if (hediff2.Bleeding)
					{
						bleedingIcon = HealthCardUtility.BleedingIcon;
					}
					totalBleedRate += hediff2.BleedRate;
					num5++;
				}
				string text2 = hediff.LabelCap;
				if (num5 != 1)
				{
					text2 = text2 + " x" + num5.ToStringCached();
				}
				GUI.color = hediff.LabelColor;
				float num6 = Text.CalcHeight(text2, width);
				Rect rect3 = new Rect(num, curY, width, num6);
				Widgets.Label(rect3, text2);
				GUI.color = Color.white;
				Rect iconsRect = new Rect(rect3.x + 10f, rect3.y, rect.width - num - 10f, rect3.height);
				List<GenUI.AnonymousStackElement> list = new List<GenUI.AnonymousStackElement>();
				foreach (HediffDef localHediffDef2 in grouping.Select((Hediff h) => h.def).Distinct<HediffDef>())
				{
					HediffDef localHediffDef = localHediffDef2;
					list.Add(new GenUI.AnonymousStackElement
					{
						drawer = delegate(Rect r)
						{
							float num7 = iconsRect.width - (r.x - iconsRect.x) - 20f;
							r = new Rect(iconsRect.x + num7, r.y, 20f, 20f);
							Widgets.InfoCardButton(r.x, r.y, localHediffDef);
						},
						width = 20f
					});
				}
				if (bleedingIcon)
				{
					list.Add(new GenUI.AnonymousStackElement
					{
						drawer = delegate(Rect r)
						{
							float num7 = iconsRect.width - (r.x - iconsRect.x) - 20f;
							r = new Rect(iconsRect.x + num7, r.y, 20f, 20f);
							GUI.DrawTexture(r.ContractedBy(GenMath.LerpDouble(0f, 0.6f, 5f, 0f, Mathf.Min(totalBleedRate, 1f))), bleedingIcon);
						},
						width = 20f
					});
				}
				if (stateIcon.HasValue)
				{
					list.Add(new GenUI.AnonymousStackElement
					{
						drawer = delegate(Rect r)
						{
							float num7 = iconsRect.width - (r.x - iconsRect.x) - 20f;
							r = new Rect(iconsRect.x + num7, r.y, 20f, 20f);
							GUI.color = stateIcon.Color;
							GUI.DrawTexture(r, stateIcon.Texture);
							GUI.color = Color.white;
						},
						width = 20f
					});
				}
				GenUI.DrawElementStack<GenUI.AnonymousStackElement>(iconsRect, num6, list, delegate(Rect r, GenUI.AnonymousStackElement obj)
				{
					obj.drawer(r);
				}, (GenUI.AnonymousStackElement obj) => obj.width, 4f, 5f, true);
				HealthCardUtility.lastMaxIconsTotalWidth = Mathf.Max(HealthCardUtility.lastMaxIconsTotalWidth, list.Sum((GenUI.AnonymousStackElement x) => x.width + 5f) - 5f);
				curY += num6;
			}
			GUI.color = Color.white;
			curY = num2 + Mathf.Max(a, b);
			if (Widgets.ButtonInvisible(rect2, HealthCardUtility.CanEntryBeClicked(diffs, pawn)))
			{
				HealthCardUtility.EntryClicked(diffs, pawn);
			}
			if (Mouse.IsOver(rect2))
			{
				TooltipHandler.TipRegion(rect2, new TipSignal(() => HealthCardUtility.GetTooltip(diffs, pawn, part), (int)curY + 7857));
			}
		}

		// Token: 0x0600941B RID: 37915 RVA: 0x002AE550 File Offset: 0x002AC750
		public static string GetPainTip(Pawn pawn)
		{
			return "PainLevel".Translate() + ": " + (pawn.health.hediffSet.PainTotal * 100f).ToString("F0") + "%";
		}

		// Token: 0x0600941C RID: 37916 RVA: 0x002AE5A8 File Offset: 0x002AC7A8
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

		// Token: 0x0600941D RID: 37917 RVA: 0x002AE774 File Offset: 0x002AC974
		private static string GetTooltip(IEnumerable<Hediff> diffs, Pawn pawn, BodyPartRecord part)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (part != null)
			{
				stringBuilder.Append(part.LabelCap + ": ");
				stringBuilder.AppendLine(" " + pawn.health.hediffSet.GetPartHealth(part).ToString() + " / " + part.def.GetMaxHealth(pawn).ToString());
				float num = PawnCapacityUtility.CalculatePartEfficiency(pawn.health.hediffSet, part, false, null);
				if (num != 1f)
				{
					stringBuilder.AppendLine("Efficiency".Translate() + ": " + num.ToStringPercent());
				}
				stringBuilder.AppendLine("------------------");
			}
			foreach (IGrouping<int, Hediff> grouping in from x in diffs
			group x by x.UIGroupKey)
			{
				foreach (Hediff hediff in grouping)
				{
					string severityLabel = hediff.SeverityLabel;
					bool flag = HealthCardUtility.showHediffsDebugInfo && !hediff.DebugString().NullOrEmpty();
					if (!hediff.Label.NullOrEmpty() || !severityLabel.NullOrEmpty() || !hediff.CapMods.NullOrEmpty<PawnCapacityModifier>() || flag)
					{
						stringBuilder.Append(hediff.LabelCap);
						if (!severityLabel.NullOrEmpty())
						{
							stringBuilder.Append(": " + severityLabel);
						}
						stringBuilder.AppendLine();
						string tipStringExtra = hediff.TipStringExtra;
						if (!tipStringExtra.NullOrEmpty())
						{
							stringBuilder.AppendLine(tipStringExtra.TrimEndNewlines().Indented("    "));
						}
						if (flag)
						{
							stringBuilder.AppendLine(hediff.DebugString().TrimEndNewlines());
						}
					}
				}
			}
			TaggedString taggedString;
			LogEntry logEntry;
			if (HealthCardUtility.GetCombatLogInfo(diffs, out taggedString, out logEntry))
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("Cause".Translate());
				stringBuilder.AppendLine(":");
				stringBuilder.AppendLine(taggedString.Resolve().Indented("    "));
			}
			return stringBuilder.ToString().TrimEnd(Array.Empty<char>());
		}

		// Token: 0x0600941E RID: 37918 RVA: 0x002AEA14 File Offset: 0x002ACC14
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

		// Token: 0x0600941F RID: 37919 RVA: 0x002AEA68 File Offset: 0x002ACC68
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

		// Token: 0x06009420 RID: 37920 RVA: 0x002AEAEC File Offset: 0x002ACCEC
		private static bool GetCombatLogInfo(IEnumerable<Hediff> diffs, out TaggedString combatLogText, out LogEntry combatLogEntry)
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

		// Token: 0x06009421 RID: 37921 RVA: 0x002AEB9C File Offset: 0x002ACD9C
		private static void DoRightRowHighlight(Rect rowRect)
		{
			if (HealthCardUtility.highlight)
			{
				GUI.color = HealthCardUtility.StaticHighlightColor;
				GUI.DrawTexture(rowRect, TexUI.HighlightTex);
			}
			HealthCardUtility.highlight = !HealthCardUtility.highlight;
			if (Mouse.IsOver(rowRect))
			{
				GUI.color = HealthCardUtility.HighlightColor;
				GUI.DrawTexture(rowRect, TexUI.HighlightTex);
			}
		}

		// Token: 0x06009422 RID: 37922 RVA: 0x002AEBF0 File Offset: 0x002ACDF0
		private static void DoDebugOptions(Rect rightRect, Pawn pawn)
		{
			Widgets.CheckboxLabeled(new Rect(rightRect.x, rightRect.y - 25f, 110f, 30f), "Dev: AllDiffs", ref HealthCardUtility.showAllHediffs, false, null, null, false);
			Widgets.CheckboxLabeled(new Rect(rightRect.x + 115f, rightRect.y - 25f, 120f, 30f), "DiffsDebugInfo", ref HealthCardUtility.showHediffsDebugInfo, false, null, null, false);
			if (Widgets.ButtonText(new Rect(rightRect.x + 240f, rightRect.y - 27f, 115f, 25f), "Debug info", true, true, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				list.Add(new FloatMenuOption("Parts hit chance (this part or any child node)", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (BodyPartRecord bodyPartRecord in from x in pawn.RaceProps.body.AllParts
					orderby x.coverageAbsWithChildren descending
					select x)
					{
						stringBuilder.AppendLine(bodyPartRecord.LabelCap + " " + bodyPartRecord.coverageAbsWithChildren.ToStringPercent());
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("Parts hit chance (exactly this part)", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					float num = 0f;
					foreach (BodyPartRecord bodyPartRecord in from x in pawn.RaceProps.body.AllParts
					orderby x.coverageAbs descending
					select x)
					{
						stringBuilder.AppendLine(bodyPartRecord.LabelCap + " " + bodyPartRecord.coverageAbs.ToStringPercent());
						num += bodyPartRecord.coverageAbs;
					}
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("Total " + num.ToStringPercent());
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("Per-part efficiency", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (BodyPartRecord bodyPartRecord in pawn.RaceProps.body.AllParts)
					{
						stringBuilder.AppendLine(bodyPartRecord.LabelCap + " " + PawnCapacityUtility.CalculatePartEfficiency(pawn.health.hediffSet, bodyPartRecord, false, null).ToStringPercent());
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				Func<BodyPartGroupDef, bool> <>9__15;
				list.Add(new FloatMenuOption("BodyPartGroup efficiency (of only natural parts)", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					IEnumerable<BodyPartGroupDef> allDefs = DefDatabase<BodyPartGroupDef>.AllDefs;
					Func<BodyPartGroupDef, bool> predicate;
					if ((predicate = <>9__15) == null)
					{
						predicate = (<>9__15 = ((BodyPartGroupDef x) => pawn.RaceProps.body.AllParts.Any((BodyPartRecord y) => y.groups.Contains(x))));
					}
					foreach (BodyPartGroupDef bodyPartGroupDef in allDefs.Where(predicate))
					{
						stringBuilder.AppendLine(bodyPartGroupDef.LabelCap + " " + PawnCapacityUtility.CalculateNaturalPartsAverageEfficiency(pawn.health.hediffSet, bodyPartGroupDef).ToStringPercent());
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("IsSolid", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (BodyPartRecord bodyPartRecord in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null))
					{
						stringBuilder.AppendLine(bodyPartRecord.LabelCap + " " + bodyPartRecord.def.IsSolid(bodyPartRecord, pawn.health.hediffSet.hediffs).ToString());
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("IsSkinCovered", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (BodyPartRecord bodyPartRecord in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null))
					{
						stringBuilder.AppendLine(bodyPartRecord.LabelCap + " " + bodyPartRecord.def.IsSkinCovered(bodyPartRecord, pawn.health.hediffSet).ToString());
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("Immunities", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (HediffDef def in DefDatabase<HediffDef>.AllDefsListForReading)
					{
						ImmunityRecord immunityRecord = pawn.health.immunity.GetImmunityRecord(def);
						if (immunityRecord != null)
						{
							stringBuilder.AppendLine(string.Concat(new object[]
							{
								"Hediff: ",
								immunityRecord.hediffDef,
								", Source: ",
								immunityRecord.source,
								", Immunity: ",
								immunityRecord.immunity
							}));
						}
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("does have added parts", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (BodyPartRecord bodyPartRecord in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null))
					{
						stringBuilder.AppendLine(bodyPartRecord.LabelCap + " " + pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(bodyPartRecord).ToString());
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("GetNotMissingParts", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (BodyPartRecord bodyPartRecord in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null))
					{
						stringBuilder.AppendLine(bodyPartRecord.LabelCap);
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				Func<BodyPartRecord, float> <>9__17;
				list.Add(new FloatMenuOption("GetCoverageOfNotMissingNaturalParts", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					IEnumerable<BodyPartRecord> allParts = pawn.RaceProps.body.AllParts;
					Func<BodyPartRecord, float> keySelector;
					if ((keySelector = <>9__17) == null)
					{
						keySelector = (<>9__17 = ((BodyPartRecord x) => pawn.health.hediffSet.GetCoverageOfNotMissingNaturalParts(x)));
					}
					foreach (BodyPartRecord bodyPartRecord in allParts.OrderByDescending(keySelector))
					{
						stringBuilder.AppendLine(bodyPartRecord.LabelCap + ": " + pawn.health.hediffSet.GetCoverageOfNotMissingNaturalParts(bodyPartRecord).ToStringPercent());
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("parts nutrition (assuming adult)", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					float totalCorpseNutrition = StatDefOf.Nutrition.Worker.GetValueAbstract(pawn.RaceProps.corpseDef, null);
					IEnumerable<BodyPartRecord> allParts = pawn.RaceProps.body.AllParts;
					Func<BodyPartRecord, float> <>9__18;
					Func<BodyPartRecord, float> keySelector;
					if ((keySelector = <>9__18) == null)
					{
						keySelector = (<>9__18 = ((BodyPartRecord x) => FoodUtility.GetBodyPartNutrition(totalCorpseNutrition, pawn, x)));
					}
					foreach (BodyPartRecord bodyPartRecord in allParts.OrderByDescending(keySelector))
					{
						stringBuilder.AppendLine(bodyPartRecord.LabelCap + ": " + FoodUtility.GetBodyPartNutrition(totalCorpseNutrition, pawn, bodyPartRecord));
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("HediffGiver_Birthday chance at age", delegate()
				{
					List<FloatMenuOption> list2 = new List<FloatMenuOption>();
					foreach (HediffGiverSetDef hediffGiverSetDef in pawn.RaceProps.hediffGiverSets)
					{
						foreach (HediffGiver_Birthday hediffGiver_Birthday in hediffGiverSetDef.hediffGivers.OfType<HediffGiver_Birthday>())
						{
							HediffGiver_Birthday hLocal = hediffGiver_Birthday;
							FloatMenuOption item = new FloatMenuOption(hediffGiverSetDef.defName + " - " + hediffGiver_Birthday.hediff.defName, delegate()
							{
								StringBuilder stringBuilder = new StringBuilder();
								stringBuilder.AppendLine("% of pawns which will have at least 1 " + hLocal.hediff.label + " at age X:");
								stringBuilder.AppendLine();
								int num = 1;
								while ((float)num < pawn.RaceProps.lifeExpectancy + 20f)
								{
									stringBuilder.AppendLine(num + ": " + hLocal.DebugChanceToHaveAtAge(pawn, num).ToStringPercent());
									num++;
								}
								Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
							}, MenuOptionPriority.Default, null, null, 0f, null, null);
							list2.Add(item);
						}
					}
					Find.WindowStack.Add(new FloatMenu(list2));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("HediffGiver_Birthday count at age", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("Average hediffs count (from HediffGiver_Birthday) at age X:");
					stringBuilder.AppendLine();
					int num = 1;
					while ((float)num < pawn.RaceProps.lifeExpectancy + 20f)
					{
						float num2 = 0f;
						foreach (HediffGiverSetDef hediffGiverSetDef in pawn.RaceProps.hediffGiverSets)
						{
							foreach (HediffGiver_Birthday hediffGiver_Birthday in hediffGiverSetDef.hediffGivers.OfType<HediffGiver_Birthday>())
							{
								num2 += hediffGiver_Birthday.DebugChanceToHaveAtAge(pawn, num);
							}
						}
						stringBuilder.AppendLine(num + ": " + num2.ToStringDecimalIfSmall());
						num++;
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x06009423 RID: 37923 RVA: 0x002AEEBC File Offset: 0x002AD0BC
		public static Pair<string, Color> GetEfficiencyLabel(Pawn pawn, PawnCapacityDef activity)
		{
			float level = pawn.health.capacities.GetLevel(activity);
			return new Pair<string, Color>(PawnCapacityUtility.CalculateCapacityLevel(pawn.health.hediffSet, activity, null, false).ToStringPercent(), HealthCardUtility.efficiencyToColor[HealthCardUtility.EfficiencyValueToEstimate(level)]);
		}

		// Token: 0x06009424 RID: 37924 RVA: 0x002AEF08 File Offset: 0x002AD108
		public static string GetEfficiencyEstimateLabel(float eff)
		{
			return HealthCardUtility.EfficiencyValueToEstimate(eff).ToString().Translate();
		}

		// Token: 0x06009425 RID: 37925 RVA: 0x002AEF34 File Offset: 0x002AD134
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

		// Token: 0x06009426 RID: 37926 RVA: 0x002AEF88 File Offset: 0x002AD188
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

		// Token: 0x04005DFF RID: 24063
		private static Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04005E00 RID: 24064
		private static float scrollViewHeight = 0f;

		// Token: 0x04005E01 RID: 24065
		private static bool highlight = true;

		// Token: 0x04005E02 RID: 24066
		private static bool onOperationTab = false;

		// Token: 0x04005E03 RID: 24067
		private static Vector2 billsScrollPosition = Vector2.zero;

		// Token: 0x04005E04 RID: 24068
		private static float billsScrollHeight = 1000f;

		// Token: 0x04005E05 RID: 24069
		private static bool showAllHediffs = false;

		// Token: 0x04005E06 RID: 24070
		private static bool showHediffsDebugInfo = false;

		// Token: 0x04005E07 RID: 24071
		private static float lastMaxIconsTotalWidth;

		// Token: 0x04005E08 RID: 24072
		public const float TopPadding = 20f;

		// Token: 0x04005E09 RID: 24073
		private const float ThoughtLevelHeight = 25f;

		// Token: 0x04005E0A RID: 24074
		private const float ThoughtLevelSpacing = 4f;

		// Token: 0x04005E0B RID: 24075
		private const float IconSize = 20f;

		// Token: 0x04005E0C RID: 24076
		private static readonly Color HighlightColor = new Color(0.5f, 0.5f, 0.5f, 1f);

		// Token: 0x04005E0D RID: 24077
		private static readonly Color StaticHighlightColor = new Color(0.75f, 0.75f, 0.85f, 1f);

		// Token: 0x04005E0E RID: 24078
		private static readonly Color MediumPainColor = new Color(0.9f, 0.9f, 0f);

		// Token: 0x04005E0F RID: 24079
		private static readonly Color SeverePainColor = new Color(0.9f, 0.5f, 0f);

		// Token: 0x04005E10 RID: 24080
		private static readonly Texture2D BleedingIcon = ContentFinder<Texture2D>.Get("UI/Icons/Medical/Bleeding", true);

		// Token: 0x04005E11 RID: 24081
		private static readonly Dictionary<EfficiencyEstimate, Color> efficiencyToColor = new Dictionary<EfficiencyEstimate, Color>
		{
			{
				EfficiencyEstimate.None,
				ColoredText.RedReadable
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

		// Token: 0x04005E12 RID: 24082
		private static List<ThingDef> tmpMedicineBestToWorst = new List<ThingDef>();
	}
}
