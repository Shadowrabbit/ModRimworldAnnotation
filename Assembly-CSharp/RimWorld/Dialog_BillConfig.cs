using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020016DB RID: 5851
	public class Dialog_BillConfig : Window
	{
		// Token: 0x170013FC RID: 5116
		// (get) Token: 0x06008083 RID: 32899 RVA: 0x00056465 File Offset: 0x00054665
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(800f, 634f);
			}
		}

		// Token: 0x06008084 RID: 32900 RVA: 0x00056476 File Offset: 0x00054676
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

		// Token: 0x06008085 RID: 32901 RVA: 0x000564AF File Offset: 0x000546AF
		private void AdjustCount(int offset)
		{
			SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			this.bill.repeatCount += offset;
			if (this.bill.repeatCount < 1)
			{
				this.bill.repeatCount = 1;
			}
		}

		// Token: 0x06008086 RID: 32902 RVA: 0x000564E9 File Offset: 0x000546E9
		public override void WindowUpdate()
		{
			this.bill.TryDrawIngredientSearchRadiusOnMap(this.billGiverPos);
		}

		// Token: 0x06008087 RID: 32903 RVA: 0x00260788 File Offset: 0x0025E988
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Medium;
			Rect rect = new Rect(40f, 0f, 400f, 34f);
			Widgets.Label(rect, this.bill.LabelCap);
			Widgets.DefIcon(new Rect(0f, rect.y, 34f, 34f), this.bill.recipe, null, 1f, true);
			float width = (float)((int)((inRect.width - 34f) / 3f));
			Rect rect2 = new Rect(0f, 80f, width, inRect.height - 80f);
			Rect rect3 = new Rect(rect2.xMax + 17f, 50f, width, inRect.height - 50f - this.CloseButSize.y);
			Rect rect4 = new Rect(rect3.xMax + 17f, 50f, 0f, inRect.height - 50f - this.CloseButSize.y);
			rect4.xMax = inRect.xMax;
			Text.Font = GameFont.Small;
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.Begin(rect3);
			Listing_Standard listing_Standard2 = listing_Standard.BeginSection((float)Dialog_BillConfig.RepeatModeSubdialogHeight);
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
			Listing_Standard listing_Standard3 = listing_Standard.BeginSection((float)Dialog_BillConfig.StoreModeSubdialogHeight);
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
									list.Add(new FloatMenuOption(string.Format("{0} ({1})", string.Format(billStoreModeDef.LabelCap, group.parent.SlotYielderLabel()), "IncompatibleLower".Translate()), null, MenuOptionPriority.Default, null, null, 0f, null, null));
								}
								else
								{
									list.Add(new FloatMenuOption(string.Format(billStoreModeDef.LabelCap, group.parent.SlotYielderLabel()), delegate()
									{
										this.bill.SetStoreMode(BillStoreModeDefOf.SpecificStockpile, (Zone_Stockpile)group.parent);
									}, MenuOptionPriority.Default, null, null, 0f, null, null));
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
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			Text.Font = GameFont.Small;
			listing_Standard.EndSection(listing_Standard3);
			listing_Standard.Gap(12f);
			Listing_Standard listing_Standard4 = listing_Standard.BeginSection((float)Dialog_BillConfig.WorkerSelectionSubdialogHeight);
			Widgets.Dropdown<Bill_Production, Pawn>(listing_Standard4.GetRect(30f), this.bill, (Bill_Production b) => b.pawnRestriction, (Bill_Production b) => this.GeneratePawnRestrictionOptions(), (this.bill.pawnRestriction == null) ? "AnyWorker".TranslateSimple() : this.bill.pawnRestriction.LabelShortCap, null, null, null, null, false);
			if (this.bill.pawnRestriction == null && this.bill.recipe.workSkill != null)
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
				ThingFilterUI.DoThingFilterConfigWindow(rect4, ref this.thingFilterScrollPosition, this.bill.ingredientFilter, this.bill.recipe.fixedIngredientFilter, 4, null, this.bill.recipe.forceHiddenSpecialFilters, false, this.bill.recipe.GetPremultipliedSmallIngredients(), this.bill.Map);
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

		// Token: 0x06008088 RID: 32904 RVA: 0x000564FC File Offset: 0x000546FC
		private IEnumerable<Widgets.DropdownMenuElement<Pawn>> GeneratePawnRestrictionOptions()
		{
			yield return new Widgets.DropdownMenuElement<Pawn>
			{
				option = new FloatMenuOption("AnyWorker".Translate(), delegate()
				{
					this.bill.pawnRestriction = null;
				}, MenuOptionPriority.Default, null, null, 0f, null, null),
				payload = null
			};
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
				Log.ErrorOnce("Generating pawn restrictions for a BillGiver without a Workgiver", 96455148, false);
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
							option = new FloatMenuOption(string.Format("{0} ({1})", pawn.LabelShortCap, "WillNever".Translate(workGiver.verb)), null, MenuOptionPriority.Default, null, null, 0f, null, null),
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
								this.bill.pawnRestriction = pawn;
							}, MenuOptionPriority.Default, null, null, 0f, null, null),
							payload = pawn
						};
					}
					else if (!pawn.workSettings.WorkIsActive(workGiver.workType))
					{
						yield return new Widgets.DropdownMenuElement<Pawn>
						{
							option = new FloatMenuOption(string.Format("{0} ({1})", pawn.LabelShortCap, "NotAssigned".Translate()), delegate()
							{
								this.bill.pawnRestriction = pawn;
							}, MenuOptionPriority.Default, null, null, 0f, null, null),
							payload = pawn
						};
					}
					else if (this.bill.recipe.workSkill != null)
					{
						yield return new Widgets.DropdownMenuElement<Pawn>
						{
							option = new FloatMenuOption(string.Format("{0} ({1} {2})", pawn.LabelShortCap, pawn.skills.GetSkill(this.bill.recipe.workSkill).Level, this.bill.recipe.workSkill.label), delegate()
							{
								this.bill.pawnRestriction = pawn;
							}, MenuOptionPriority.Default, null, null, 0f, null, null),
							payload = pawn
						};
					}
					else
					{
						yield return new Widgets.DropdownMenuElement<Pawn>
						{
							option = new FloatMenuOption(string.Format("{0}", pawn.LabelShortCap), delegate()
							{
								this.bill.pawnRestriction = pawn;
							}, MenuOptionPriority.Default, null, null, 0f, null, null),
							payload = pawn
						};
					}
				}
			}
			IEnumerator<Pawn> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06008089 RID: 32905 RVA: 0x0005650C File Offset: 0x0005470C
		private IEnumerable<Widgets.DropdownMenuElement<Zone_Stockpile>> GenerateStockpileInclusion()
		{
			yield return new Widgets.DropdownMenuElement<Zone_Stockpile>
			{
				option = new FloatMenuOption("IncludeFromAll".Translate(), delegate()
				{
					this.bill.includeFromZone = null;
				}, MenuOptionPriority.Default, null, null, 0f, null, null),
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
							option = new FloatMenuOption(string.Format("{0} ({1})", "IncludeSpecific".Translate(slotGroup.parent.SlotYielderLabel()), "IncompatibleLower".Translate()), null, MenuOptionPriority.Default, null, null, 0f, null, null),
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
							}, MenuOptionPriority.Default, null, null, 0f, null, null),
							payload = stockpile
						};
					}
				}
				num = i + 1;
			}
			yield break;
		}

		// Token: 0x04005331 RID: 21297
		private IntVec3 billGiverPos;

		// Token: 0x04005332 RID: 21298
		private Bill_Production bill;

		// Token: 0x04005333 RID: 21299
		private Vector2 thingFilterScrollPosition;

		// Token: 0x04005334 RID: 21300
		private string repeatCountEditBuffer;

		// Token: 0x04005335 RID: 21301
		private string targetCountEditBuffer;

		// Token: 0x04005336 RID: 21302
		private string unpauseCountEditBuffer;

		// Token: 0x04005337 RID: 21303
		[TweakValue("Interface", 0f, 400f)]
		private static int RepeatModeSubdialogHeight = 324;

		// Token: 0x04005338 RID: 21304
		[TweakValue("Interface", 0f, 400f)]
		private static int StoreModeSubdialogHeight = 30;

		// Token: 0x04005339 RID: 21305
		[TweakValue("Interface", 0f, 400f)]
		private static int WorkerSelectionSubdialogHeight = 85;

		// Token: 0x0400533A RID: 21306
		[TweakValue("Interface", 0f, 400f)]
		private static int IngredientRadiusSubdialogHeight = 50;
	}
}
