using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200108D RID: 4237
	public class Dialog_BillConfig : Window
	{
		// Token: 0x17001149 RID: 4425
		// (get) Token: 0x060064DF RID: 25823 RVA: 0x0021FBB8 File Offset: 0x0021DDB8
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(800f, 634f);
			}
		}

		// Token: 0x060064E0 RID: 25824 RVA: 0x0021FBCC File Offset: 0x0021DDCC
		public Dialog_BillConfig(Bill_Production bill, IntVec3 billGiverPos)
		{
			this.billGiverPos = billGiverPos;
			this.bill = bill;
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.absorbInputAroundWindow = true;
			this.closeOnClickedOutside = true;
		}

		// Token: 0x060064E1 RID: 25825 RVA: 0x0021FC1B File Offset: 0x0021DE1B
		public override void PreOpen()
		{
			base.PreOpen();
			this.thingFilterState.quickSearch.Reset();
		}

		// Token: 0x060064E2 RID: 25826 RVA: 0x0021FC33 File Offset: 0x0021DE33
		private void AdjustCount(int offset)
		{
			SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			this.bill.repeatCount += offset;
			if (this.bill.repeatCount < 1)
			{
				this.bill.repeatCount = 1;
			}
		}

		// Token: 0x060064E3 RID: 25827 RVA: 0x0021FC6D File Offset: 0x0021DE6D
		public override void WindowUpdate()
		{
			this.bill.TryDrawIngredientSearchRadiusOnMap(this.billGiverPos);
		}

		// Token: 0x060064E4 RID: 25828 RVA: 0x0021FC80 File Offset: 0x0021DE80
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Medium;
			Rect rect = new Rect(40f, 0f, 400f, 34f);
			Widgets.Label(rect, this.bill.LabelCap);
			Widgets.DefIcon(new Rect(0f, rect.y, 34f, 34f), this.bill.recipe, null, 1f, null, true, null);
			float width = (float)((int)((inRect.width - 34f) / 3f));
			Rect rect2 = new Rect(0f, 80f, width, inRect.height - 80f);
			Rect rect3 = new Rect(rect2.xMax + 17f, 50f, width, inRect.height - 50f - Window.CloseButSize.y);
			Rect rect4 = new Rect(rect3.xMax + 17f, 50f, 0f, inRect.height - 50f - Window.CloseButSize.y);
			rect4.xMax = inRect.xMax;
			Text.Font = GameFont.Small;
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.Begin(rect3);
			Listing_Standard listing_Standard2 = listing_Standard.BeginSection((float)Dialog_BillConfig.RepeatModeSubdialogHeight, 4f, 4f);
			if (listing_Standard2.ButtonText(this.bill.repeatMode.LabelCap, null))
			{
				BillRepeatModeUtility.MakeConfigFloatMenu(this.bill);
			}
			listing_Standard2.Gap(12f);
			if (this.bill.repeatMode == BillRepeatModeDefOf.RepeatCount)
			{
				listing_Standard2.Label("RepeatCount".Translate(this.bill.repeatCount), -1f, null);
				listing_Standard2.IntEntry(ref this.bill.repeatCount, ref this.repeatCountEditBuffer, 1);
			}
			else if (this.bill.repeatMode == BillRepeatModeDefOf.TargetCount)
			{
				string text = "CurrentlyHave".Translate() + ": ";
				text += this.bill.recipe.WorkerCounter.CountProducts(this.bill);
				text += " / ";
				text += ((this.bill.targetCount < 999999) ? this.bill.targetCount.ToString() : "Infinite".Translate().ToLower().ToString());
				string str = this.bill.recipe.WorkerCounter.ProductsDescription(this.bill);
				if (!str.NullOrEmpty())
				{
					text += "\n" + "CountingProducts".Translate() + ": " + str.CapitalizeFirst();
				}
				listing_Standard2.Label(text, -1f, null);
				int targetCount = this.bill.targetCount;
				listing_Standard2.IntEntry(ref this.bill.targetCount, ref this.targetCountEditBuffer, this.bill.recipe.targetCountAdjustment);
				this.bill.unpauseWhenYouHave = Mathf.Max(0, this.bill.unpauseWhenYouHave + (this.bill.targetCount - targetCount));
				ThingDef producedThingDef = this.bill.recipe.ProducedThingDef;
				if (producedThingDef != null)
				{
					if (producedThingDef.IsWeapon || producedThingDef.IsApparel)
					{
						listing_Standard2.CheckboxLabeled("IncludeEquipped".Translate(), ref this.bill.includeEquipped, null);
					}
					if (producedThingDef.IsApparel && producedThingDef.apparel.careIfWornByCorpse)
					{
						listing_Standard2.CheckboxLabeled("IncludeTainted".Translate(), ref this.bill.includeTainted, null);
					}
					Widgets.Dropdown<Bill_Production, Zone_Stockpile>(listing_Standard2.GetRect(30f), this.bill, (Bill_Production b) => b.includeFromZone, (Bill_Production b) => this.GenerateStockpileInclusion(), (this.bill.includeFromZone == null) ? "IncludeFromAll".Translate() : "IncludeSpecific".Translate(this.bill.includeFromZone.label), null, null, null, null, false);
					if (this.bill.recipe.products.Any((ThingDefCountClass prod) => prod.thingDef.useHitPoints))
					{
						Widgets.FloatRange(listing_Standard2.GetRect(28f), 975643279, ref this.bill.hpRange, 0f, 1f, "HitPoints", ToStringStyle.PercentZero);
						this.bill.hpRange.min = Mathf.Round(this.bill.hpRange.min * 100f) / 100f;
						this.bill.hpRange.max = Mathf.Round(this.bill.hpRange.max * 100f) / 100f;
					}
					if (producedThingDef.HasComp(typeof(CompQuality)))
					{
						Widgets.QualityRange(listing_Standard2.GetRect(28f), 1098906561, ref this.bill.qualityRange);
					}
					if (producedThingDef.MadeFromStuff)
					{
						listing_Standard2.CheckboxLabeled("LimitToAllowedStuff".Translate(), ref this.bill.limitToAllowedStuff, null);
					}
				}
			}
			if (this.bill.repeatMode == BillRepeatModeDefOf.TargetCount)
			{
				listing_Standard2.CheckboxLabeled("PauseWhenSatisfied".Translate(), ref this.bill.pauseWhenSatisfied, null);
				if (this.bill.pauseWhenSatisfied)
				{
					listing_Standard2.Label("UnpauseWhenYouHave".Translate() + ": " + this.bill.unpauseWhenYouHave.ToString("F0"), -1f, null);
					listing_Standard2.IntEntry(ref this.bill.unpauseWhenYouHave, ref this.unpauseCountEditBuffer, this.bill.recipe.targetCountAdjustment);
					if (this.bill.unpauseWhenYouHave >= this.bill.targetCount)
					{
						this.bill.unpauseWhenYouHave = this.bill.targetCount - 1;
						this.unpauseCountEditBuffer = this.bill.unpauseWhenYouHave.ToStringCached();
					}
				}
			}
			listing_Standard.EndSection(listing_Standard2);
			listing_Standard.Gap(12f);
			Listing_Standard listing_Standard3 = listing_Standard.BeginSection((float)Dialog_BillConfig.StoreModeSubdialogHeight, 4f, 4f);
			string text2 = string.Format(this.bill.GetStoreMode().LabelCap, (this.bill.GetStoreZone() != null) ? this.bill.GetStoreZone().SlotYielderLabel() : "");
			if (this.bill.GetStoreZone() != null && !this.bill.recipe.WorkerCounter.CanPossiblyStoreInStockpile(this.bill, this.bill.GetStoreZone()))
			{
				text2 += string.Format(" ({0})", "IncompatibleLower".Translate());
				Text.Font = GameFont.Tiny;
			}
			if (listing_Standard3.ButtonText(text2, null))
			{
				Text.Font = GameFont.Small;
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (BillStoreModeDef billStoreModeDef in from bsm in DefDatabase<BillStoreModeDef>.AllDefs
				orderby bsm.listOrder
				select bsm)
				{
					if (billStoreModeDef == BillStoreModeDefOf.SpecificStockpile)
					{
						List<SlotGroup> allGroupsListInPriorityOrder = this.bill.billStack.billGiver.Map.haulDestinationManager.AllGroupsListInPriorityOrder;
						int count = allGroupsListInPriorityOrder.Count;
						for (int i = 0; i < count; i++)
						{
							SlotGroup group = allGroupsListInPriorityOrder[i];
							Zone_Stockpile zone_Stockpile = group.parent as Zone_Stockpile;
							if (zone_Stockpile != null)
							{
								if (!this.bill.recipe.WorkerCounter.CanPossiblyStoreInStockpile(this.bill, zone_Stockpile))
								{
									list.Add(new FloatMenuOption(string.Format("{0} ({1})", string.Format(billStoreModeDef.LabelCap, group.parent.SlotYielderLabel()), "IncompatibleLower".Translate()), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
								}
								else
								{
									list.Add(new FloatMenuOption(string.Format(billStoreModeDef.LabelCap, group.parent.SlotYielderLabel()), delegate()
									{
										this.bill.SetStoreMode(BillStoreModeDefOf.SpecificStockpile, (Zone_Stockpile)group.parent);
									}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
								}
							}
						}
					}
					else
					{
						BillStoreModeDef smLocal = billStoreModeDef;
						list.Add(new FloatMenuOption(smLocal.LabelCap, delegate()
						{
							this.bill.SetStoreMode(smLocal, null);
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			Text.Font = GameFont.Small;
			listing_Standard.EndSection(listing_Standard3);
			listing_Standard.Gap(12f);
			Listing_Standard listing_Standard4 = listing_Standard.BeginSection((float)Dialog_BillConfig.WorkerSelectionSubdialogHeight, 4f, 4f);
			string buttonLabel;
			if (this.bill.PawnRestriction != null)
			{
				buttonLabel = this.bill.PawnRestriction.LabelShortCap;
			}
			else if (ModsConfig.IdeologyActive && this.bill.SlavesOnly)
			{
				buttonLabel = "AnySlave".Translate();
			}
			else
			{
				buttonLabel = "AnyWorker".Translate();
			}
			Widgets.Dropdown<Bill_Production, Pawn>(listing_Standard4.GetRect(30f), this.bill, (Bill_Production b) => b.PawnRestriction, (Bill_Production b) => this.GeneratePawnRestrictionOptions(), buttonLabel, null, null, null, null, false);
			if (this.bill.PawnRestriction == null && this.bill.recipe.workSkill != null)
			{
				listing_Standard4.Label("AllowedSkillRange".Translate(this.bill.recipe.workSkill.label), -1f, null);
				listing_Standard4.IntRange(ref this.bill.allowedSkillRange, 0, 20);
			}
			listing_Standard.EndSection(listing_Standard4);
			listing_Standard.End();
			Rect rect5 = rect4;
			bool flag = true;
			for (int j = 0; j < this.bill.recipe.ingredients.Count; j++)
			{
				if (!this.bill.recipe.ingredients[j].IsFixedIngredient)
				{
					flag = false;
					break;
				}
			}
			if (!flag)
			{
				rect5.yMin = rect5.yMax - (float)Dialog_BillConfig.IngredientRadiusSubdialogHeight;
				rect4.yMax = rect5.yMin - 17f;
				bool flag2 = this.bill.GetStoreZone() == null || this.bill.recipe.WorkerCounter.CanPossiblyStoreInStockpile(this.bill, this.bill.GetStoreZone());
				ThingFilterUI.DoThingFilterConfigWindow(rect4, this.thingFilterState, this.bill.ingredientFilter, this.bill.recipe.fixedIngredientFilter, 4, null, this.bill.recipe.forceHiddenSpecialFilters, false, this.bill.recipe.GetPremultipliedSmallIngredients(), this.bill.Map);
				bool flag3 = this.bill.GetStoreZone() == null || this.bill.recipe.WorkerCounter.CanPossiblyStoreInStockpile(this.bill, this.bill.GetStoreZone());
				if (flag2 && !flag3)
				{
					Messages.Message("MessageBillValidationStoreZoneInsufficient".Translate(this.bill.LabelCap, this.bill.billStack.billGiver.LabelShort.CapitalizeFirst(), this.bill.GetStoreZone().label), this.bill.billStack.billGiver as Thing, MessageTypeDefOf.RejectInput, false);
				}
			}
			else
			{
				rect5.yMin = 50f;
			}
			Listing_Standard listing_Standard5 = new Listing_Standard();
			listing_Standard5.Begin(rect5);
			string str2 = "IngredientSearchRadius".Translate().Truncate(rect5.width * 0.6f, null);
			string str3 = (this.bill.ingredientSearchRadius == 999f) ? "Unlimited".TranslateSimple().Truncate(rect5.width * 0.3f, null) : this.bill.ingredientSearchRadius.ToString("F0");
			listing_Standard5.Label(str2 + ": " + str3, -1f, null);
			this.bill.ingredientSearchRadius = listing_Standard5.Slider((this.bill.ingredientSearchRadius > 100f) ? 100f : this.bill.ingredientSearchRadius, 3f, 100f);
			if (this.bill.ingredientSearchRadius >= 100f)
			{
				this.bill.ingredientSearchRadius = 999f;
			}
			listing_Standard5.End();
			Listing_Standard listing_Standard6 = new Listing_Standard();
			listing_Standard6.Begin(rect2);
			if (this.bill.suspended)
			{
				if (listing_Standard6.ButtonText("Suspended".Translate(), null))
				{
					this.bill.suspended = false;
					SoundDefOf.Click.PlayOneShotOnCamera(null);
				}
			}
			else if (listing_Standard6.ButtonText("NotSuspended".Translate(), null))
			{
				this.bill.suspended = true;
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (this.bill.recipe.description != null)
			{
				stringBuilder.AppendLine(this.bill.recipe.description);
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine("WorkAmount".Translate() + ": " + this.bill.recipe.WorkAmountTotal(null).ToStringWorkAmount());
			for (int k = 0; k < this.bill.recipe.ingredients.Count; k++)
			{
				IngredientCount ingredientCount = this.bill.recipe.ingredients[k];
				if (!ingredientCount.filter.Summary.NullOrEmpty())
				{
					stringBuilder.AppendLine(this.bill.recipe.IngredientValueGetter.BillRequirementsDescription(this.bill.recipe, ingredientCount));
				}
			}
			stringBuilder.AppendLine();
			string text3 = this.bill.recipe.IngredientValueGetter.ExtraDescriptionLine(this.bill.recipe);
			if (text3 != null)
			{
				stringBuilder.AppendLine(text3);
				stringBuilder.AppendLine();
			}
			if (!this.bill.recipe.skillRequirements.NullOrEmpty<SkillRequirement>())
			{
				stringBuilder.AppendLine("MinimumSkills".Translate());
				stringBuilder.AppendLine(this.bill.recipe.MinSkillString);
			}
			Text.Font = GameFont.Small;
			string text4 = stringBuilder.ToString();
			if (Text.CalcHeight(text4, rect2.width) > rect2.height)
			{
				Text.Font = GameFont.Tiny;
			}
			listing_Standard6.Label(text4, -1f, null);
			Text.Font = GameFont.Small;
			listing_Standard6.End();
			if (this.bill.recipe.products.Count == 1)
			{
				ThingDef thingDef = this.bill.recipe.products[0].thingDef;
				Widgets.InfoCardButton(rect2.x, rect4.y, thingDef, GenStuff.DefaultStuffFor(thingDef));
			}
		}

		// Token: 0x060064E5 RID: 25829 RVA: 0x00220CBC File Offset: 0x0021EEBC
		private IEnumerable<Widgets.DropdownMenuElement<Pawn>> GeneratePawnRestrictionOptions()
		{
			yield return new Widgets.DropdownMenuElement<Pawn>
			{
				option = new FloatMenuOption("AnyWorker".Translate(), delegate()
				{
					this.bill.SetAnyPawnRestriction();
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
				payload = null
			};
			if (ModsConfig.IdeologyActive)
			{
				yield return new Widgets.DropdownMenuElement<Pawn>
				{
					option = new FloatMenuOption("AnySlave".Translate(), delegate()
					{
						this.bill.SetAnySlaveRestriction();
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
					payload = null
				};
			}
			bool workSkill = this.bill.recipe.workSkill != null;
			IEnumerable<Pawn> enumerable = PawnsFinder.AllMaps_FreeColonists;
			enumerable = from pawn in enumerable
			orderby pawn.LabelShortCap
			select pawn;
			if (workSkill)
			{
				enumerable = from pawn in enumerable
				orderby pawn.skills.GetSkill(this.bill.recipe.workSkill).Level descending
				select pawn;
			}
			WorkGiverDef workGiver = this.bill.billStack.billGiver.GetWorkgiver();
			if (workGiver == null)
			{
				Log.ErrorOnce("Generating pawn restrictions for a BillGiver without a Workgiver", 96455148);
				yield break;
			}
			enumerable = from pawn in enumerable
			orderby pawn.workSettings.WorkIsActive(workGiver.workType) descending
			select pawn;
			enumerable = from pawn in enumerable
			orderby pawn.WorkTypeIsDisabled(workGiver.workType)
			select pawn;
			using (IEnumerator<Pawn> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Pawn pawn = enumerator.Current;
					if (pawn.WorkTypeIsDisabled(workGiver.workType))
					{
						yield return new Widgets.DropdownMenuElement<Pawn>
						{
							option = new FloatMenuOption(string.Format("{0} ({1})", pawn.LabelShortCap, "WillNever".Translate(workGiver.verb)), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
							payload = pawn
						};
					}
					else if (this.bill.recipe.workSkill != null && !pawn.workSettings.WorkIsActive(workGiver.workType))
					{
						yield return new Widgets.DropdownMenuElement<Pawn>
						{
							option = new FloatMenuOption(string.Format("{0} ({1} {2}, {3})", new object[]
							{
								pawn.LabelShortCap,
								pawn.skills.GetSkill(this.bill.recipe.workSkill).Level,
								this.bill.recipe.workSkill.label,
								"NotAssigned".Translate()
							}), delegate()
							{
								this.bill.SetPawnRestriction(pawn);
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
							payload = pawn
						};
					}
					else if (!pawn.workSettings.WorkIsActive(workGiver.workType))
					{
						yield return new Widgets.DropdownMenuElement<Pawn>
						{
							option = new FloatMenuOption(string.Format("{0} ({1})", pawn.LabelShortCap, "NotAssigned".Translate()), delegate()
							{
								this.bill.SetPawnRestriction(pawn);
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
							payload = pawn
						};
					}
					else if (this.bill.recipe.workSkill != null)
					{
						yield return new Widgets.DropdownMenuElement<Pawn>
						{
							option = new FloatMenuOption(string.Format("{0} ({1} {2})", pawn.LabelShortCap, pawn.skills.GetSkill(this.bill.recipe.workSkill).Level, this.bill.recipe.workSkill.label), delegate()
							{
								this.bill.SetPawnRestriction(pawn);
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
							payload = pawn
						};
					}
					else
					{
						yield return new Widgets.DropdownMenuElement<Pawn>
						{
							option = new FloatMenuOption(string.Format("{0}", pawn.LabelShortCap), delegate()
							{
								this.bill.SetPawnRestriction(pawn);
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
							payload = pawn
						};
					}
				}
			}
			IEnumerator<Pawn> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060064E6 RID: 25830 RVA: 0x00220CCC File Offset: 0x0021EECC
		private IEnumerable<Widgets.DropdownMenuElement<Zone_Stockpile>> GenerateStockpileInclusion()
		{
			yield return new Widgets.DropdownMenuElement<Zone_Stockpile>
			{
				option = new FloatMenuOption("IncludeFromAll".Translate(), delegate()
				{
					this.bill.includeFromZone = null;
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
				payload = null
			};
			List<SlotGroup> groupList = this.bill.billStack.billGiver.Map.haulDestinationManager.AllGroupsListInPriorityOrder;
			int groupCount = groupList.Count;
			int num;
			for (int i = 0; i < groupCount; i = num)
			{
				SlotGroup slotGroup = groupList[i];
				Zone_Stockpile stockpile = slotGroup.parent as Zone_Stockpile;
				if (stockpile != null)
				{
					if (!this.bill.recipe.WorkerCounter.CanPossiblyStoreInStockpile(this.bill, stockpile))
					{
						yield return new Widgets.DropdownMenuElement<Zone_Stockpile>
						{
							option = new FloatMenuOption(string.Format("{0} ({1})", "IncludeSpecific".Translate(slotGroup.parent.SlotYielderLabel()), "IncompatibleLower".Translate()), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
							payload = stockpile
						};
					}
					else
					{
						yield return new Widgets.DropdownMenuElement<Zone_Stockpile>
						{
							option = new FloatMenuOption("IncludeSpecific".Translate(slotGroup.parent.SlotYielderLabel()), delegate()
							{
								this.bill.includeFromZone = stockpile;
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
							payload = stockpile
						};
					}
				}
				num = i + 1;
			}
			yield break;
		}

		// Token: 0x040038BF RID: 14527
		private IntVec3 billGiverPos;

		// Token: 0x040038C0 RID: 14528
		private Bill_Production bill;

		// Token: 0x040038C1 RID: 14529
		private ThingFilterUI.UIState thingFilterState = new ThingFilterUI.UIState();

		// Token: 0x040038C2 RID: 14530
		private string repeatCountEditBuffer;

		// Token: 0x040038C3 RID: 14531
		private string targetCountEditBuffer;

		// Token: 0x040038C4 RID: 14532
		private string unpauseCountEditBuffer;

		// Token: 0x040038C5 RID: 14533
		[TweakValue("Interface", 0f, 400f)]
		private static int RepeatModeSubdialogHeight = 324;

		// Token: 0x040038C6 RID: 14534
		[TweakValue("Interface", 0f, 400f)]
		private static int StoreModeSubdialogHeight = 30;

		// Token: 0x040038C7 RID: 14535
		[TweakValue("Interface", 0f, 400f)]
		private static int WorkerSelectionSubdialogHeight = 85;

		// Token: 0x040038C8 RID: 14536
		[TweakValue("Interface", 0f, 400f)]
		private static int IngredientRadiusSubdialogHeight = 50;
	}
}
