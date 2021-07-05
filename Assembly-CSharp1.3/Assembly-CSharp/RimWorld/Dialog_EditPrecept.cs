using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020012E4 RID: 4836
	public class Dialog_EditPrecept : Window
	{
		// Token: 0x17001447 RID: 5191
		// (get) Token: 0x060073C6 RID: 29638 RVA: 0x002702E4 File Offset: 0x0026E4E4
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(700f, this.windowHeight);
			}
		}

		// Token: 0x060073C7 RID: 29639 RVA: 0x002702F8 File Offset: 0x0026E4F8
		public Dialog_EditPrecept(Precept precept)
		{
			this.precept = precept;
			this.absorbInputAroundWindow = true;
			this.newPreceptName = precept.Label;
			Precept_Ritual ritual;
			if ((ritual = (precept as Precept_Ritual)) != null)
			{
				this.dateTrigger = ritual.obligationTriggers.OfType<RitualObligationTrigger_Date>().FirstOrDefault<RitualObligationTrigger_Date>();
				if (this.dateTrigger != null)
				{
					this.newTriggerDaysSinceStartOfYear = this.dateTrigger.triggerDaysSinceStartOfYear;
					this.RecalculateQuadrumAndDay();
				}
				this.newCanStartAnytime = ritual.isAnytime;
				this.attachableOutcomeEffects = DefDatabase<RitualAttachableOutcomeEffectDef>.AllDefs.ToList<RitualAttachableOutcomeEffectDef>();
				this.attachableUsableOutcomeEffects = (from e in this.attachableOutcomeEffects
				where e.CanAttachToRitual(ritual)
				select e).ToList<RitualAttachableOutcomeEffectDef>();
				this.selectedReward = ritual.attachableOutcomeEffect;
			}
			if (precept.def.leaderRole)
			{
				this.newPreceptNameFemale = precept.ideo.leaderTitleFemale;
			}
			this.apparelRequirements = ((precept.ApparelRequirements != null) ? precept.ApparelRequirements.ToList<PreceptApparelRequirement>() : null);
			Precept_Building precept_Building;
			if ((precept_Building = (precept as Precept_Building)) != null)
			{
				this.selectedStyle = precept_Building.ideo.GetStyleAndCategoryFor(precept_Building.ThingDef);
			}
			this.UpdateWindowHeight();
		}

		// Token: 0x060073C8 RID: 29640 RVA: 0x00270440 File Offset: 0x0026E640
		private void UpdateWindowHeight()
		{
			this.windowHeight = 170f;
			if (this.precept.def.leaderRole || this.dateTrigger != null)
			{
				this.windowHeight += 10f + Dialog_EditPrecept.EditFieldHeight;
			}
			if (this.apparelRequirements != null)
			{
				this.windowHeight += 39f;
				this.windowHeight += 40f;
				this.windowHeight += 30f;
				if (!this.apparelRequirements.NullOrEmpty<PreceptApparelRequirement>())
				{
					foreach (PreceptApparelRequirement preceptApparelRequirement in this.apparelRequirements)
					{
						int num = preceptApparelRequirement.requirement.AllRequiredApparel(Gender.None).EnumerableCount();
						this.windowHeight += (float)num * 30f;
					}
					this.windowHeight += (float)this.apparelRequirements.Count * 10f;
				}
			}
			Precept_Ritual precept_Ritual;
			if ((precept_Ritual = (this.precept as Precept_Ritual)) != null)
			{
				this.windowHeight += 10f + Dialog_EditPrecept.EditFieldHeight;
				this.windowHeight += 10f + Dialog_EditPrecept.EditFieldHeight;
				this.windowHeight += 10f + Dialog_EditPrecept.EditFieldHeight;
				if (precept_Ritual.attachableOutcomeEffect != null)
				{
					float x = Text.CalcSize(precept_Ritual.attachableOutcomeEffect.LabelCap).x;
					this.attachedRewardDescWidth = x + Dialog_EditPrecept.ButSize.x + 4f;
					this.attachedRewardDescHeight = Text.CalcHeight(precept_Ritual.attachableOutcomeEffect.effectDesc, this.attachedRewardDescWidth);
					this.windowHeight += this.attachedRewardDescHeight;
				}
			}
			Precept_Building building;
			if ((building = (this.precept as Precept_Building)) != null && this.StylesForBuilding(building).Count > 1)
			{
				int val = this.NumStyleRows(building);
				this.windowHeight += (float)Math.Min(val, 2) * 74f + 14f;
			}
			Precept_Relic precept_Relic;
			if ((precept_Relic = (this.precept as Precept_Relic)) != null && precept_Relic.ThingDef.MadeFromStuff)
			{
				this.windowHeight += 10f + Dialog_EditPrecept.EditFieldHeight;
			}
			this.SetInitialSizeAndPosition();
		}

		// Token: 0x060073C9 RID: 29641 RVA: 0x002706A8 File Offset: 0x0026E8A8
		private int NumStyleRows(Precept_Building building)
		{
			int num = (int)(this.InitialSize.x - this.Margin * 2f - this.InitialSize.x / 3f) / 68;
			return this.StylesForBuilding(building).Count / num + 1;
		}

		// Token: 0x060073CA RID: 29642 RVA: 0x002706F4 File Offset: 0x0026E8F4
		public override void OnAcceptKeyPressed()
		{
			this.ApplyChanges();
			Event.current.Use();
		}

		// Token: 0x060073CB RID: 29643 RVA: 0x00270708 File Offset: 0x0026E908
		public override void DoWindowContents(Rect rect)
		{
			Dialog_EditPrecept.<>c__DisplayClass32_0 CS$<>8__locals1 = new Dialog_EditPrecept.<>c__DisplayClass32_0();
			CS$<>8__locals1.<>4__this = this;
			float num = rect.x + rect.width / 3f;
			float num2 = rect.xMax - num;
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(rect.x, rect.y, rect.width, 35f), "EditName".Translate());
			Text.Font = GameFont.Small;
			float num3 = rect.y + 35f + 10f;
			if (this.precept.def.leaderRole)
			{
				Widgets.Label(new Rect(rect.x, num3, num2, Dialog_EditPrecept.EditFieldHeight), "LeaderTitle".Translate() + " (" + Gender.Male.GetLabel(false) + ")");
				this.newPreceptName = Widgets.TextField(new Rect(num, num3, num2, Dialog_EditPrecept.EditFieldHeight), this.newPreceptName);
				num3 += Dialog_EditPrecept.EditFieldHeight + 10f;
				Widgets.Label(new Rect(rect.x, num3, num2, Dialog_EditPrecept.EditFieldHeight), "LeaderTitle".Translate() + " (" + Gender.Female.GetLabel(false) + ")");
				this.newPreceptNameFemale = Widgets.TextField(new Rect(num, num3, num2, Dialog_EditPrecept.EditFieldHeight), this.newPreceptNameFemale);
				num3 += Dialog_EditPrecept.EditFieldHeight + 10f;
			}
			else
			{
				Dialog_EditPrecept.<>c__DisplayClass32_1 CS$<>8__locals2 = new Dialog_EditPrecept.<>c__DisplayClass32_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				Widgets.Label(new Rect(rect.x, num3, num2, Dialog_EditPrecept.EditFieldHeight), "Name".Translate());
				this.newPreceptName = Widgets.TextField(new Rect(num, num3, num2, Dialog_EditPrecept.EditFieldHeight), this.newPreceptName);
				num3 += Dialog_EditPrecept.EditFieldHeight + 10f;
				CS$<>8__locals2.ritual = (this.precept as Precept_Ritual);
				if (CS$<>8__locals2.ritual != null)
				{
					if (CS$<>8__locals2.ritual.canBeAnytime && !CS$<>8__locals2.ritual.sourcePattern.alwaysStartAnytime)
					{
						Widgets.Label(new Rect(rect.x, num3, num2, Dialog_EditPrecept.EditFieldHeight), "StartingCondition".Translate());
						bool flag = this.newCanStartAnytime;
						if (Widgets.RadioButton(num, num3, flag))
						{
							flag = !flag;
						}
						Widgets.Label(new Rect(num + 10f + 24f, num3, num2, Dialog_EditPrecept.EditFieldHeight), "StartingCondition_Anytime".Translate());
						num3 += Dialog_EditPrecept.EditFieldHeight + 10f;
						if (Widgets.RadioButton(num, num3, !flag))
						{
							flag = !flag;
						}
						Widgets.Label(new Rect(num + 10f + 24f, num3, num2, Dialog_EditPrecept.EditFieldHeight), "StartingCondition_Date".Translate());
						num3 += Dialog_EditPrecept.EditFieldHeight + 10f;
						if (flag != this.newCanStartAnytime)
						{
							this.newCanStartAnytime = flag;
							this.UpdateWindowHeight();
						}
						if (this.dateTrigger != null && !this.newCanStartAnytime)
						{
							Widgets.Label(new Rect(rect.x, num3, num2, Dialog_EditPrecept.EditFieldHeight), "Date".Translate());
							if (Widgets.ButtonText(new Rect(num, num3, num2 / 4f, Dialog_EditPrecept.EditFieldHeight), Find.ActiveLanguageWorker.OrdinalNumber(this.day + 1, Gender.None), true, true, true))
							{
								List<FloatMenuOption> list = new List<FloatMenuOption>();
								for (int i = 0; i < 15; i++)
								{
									int d = i;
									list.Add(new FloatMenuOption(Find.ActiveLanguageWorker.OrdinalNumber(d + 1, Gender.None), delegate()
									{
										CS$<>8__locals2.CS$<>8__locals1.<>4__this.day = d;
										CS$<>8__locals2.CS$<>8__locals1.<>4__this.newTriggerDaysSinceStartOfYear = (int)(CS$<>8__locals2.CS$<>8__locals1.<>4__this.quadrum * (Quadrum)15 + (byte)CS$<>8__locals2.CS$<>8__locals1.<>4__this.day);
									}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
								}
								Find.WindowStack.Add(new FloatMenu(list));
							}
							if (Widgets.ButtonText(new Rect(num + num2 / 4f + 10f, num3, num2 / 4f, Dialog_EditPrecept.EditFieldHeight), this.quadrum.Label(), true, true, true))
							{
								List<FloatMenuOption> list2 = new List<FloatMenuOption>();
								using (List<Quadrum>.Enumerator enumerator = QuadrumUtility.QuadrumsInChronologicalOrder.GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										Quadrum q = enumerator.Current;
										list2.Add(new FloatMenuOption(q.Label(), delegate()
										{
											CS$<>8__locals2.CS$<>8__locals1.<>4__this.quadrum = q;
											CS$<>8__locals2.CS$<>8__locals1.<>4__this.newTriggerDaysSinceStartOfYear = (int)(CS$<>8__locals2.CS$<>8__locals1.<>4__this.quadrum * (Quadrum)15 + (byte)CS$<>8__locals2.CS$<>8__locals1.<>4__this.day);
										}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
									}
								}
								Find.WindowStack.Add(new FloatMenu(list2));
							}
							num3 += Dialog_EditPrecept.EditFieldHeight + 10f;
						}
					}
					if (CS$<>8__locals2.ritual.SupportsAttachableOutcomeEffect)
					{
						Widgets.Label(new Rect(rect.x, num3, num2 / 2f, Dialog_EditPrecept.EditFieldHeight), "RitualAttachedReward".Translate());
						if (this.selectedReward != null)
						{
							GUI.color = Color.gray;
							string value = this.selectedReward.AppliesToOutcomesString(CS$<>8__locals2.ritual.outcomeEffect.def);
							Widgets.Label(new Rect(rect.x, num3 + Dialog_EditPrecept.EditFieldHeight, num2 / 2f, Dialog_EditPrecept.EditFieldHeight * 2f), (this.selectedReward.AppliesToSeveralOutcomes(CS$<>8__locals2.ritual) ? "RitualAttachedApplliesForOutcomes" : "RitualAttachedApplliesForOutcome").Translate(value));
							Widgets.Label(new Rect(num, num3 + Dialog_EditPrecept.EditFieldHeight, this.attachedRewardDescWidth, this.attachedRewardDescHeight), this.selectedReward.effectDesc.CapitalizeFirst());
							GUI.color = Color.white;
						}
						TaggedString taggedString = (this.selectedReward != null) ? this.selectedReward.LabelCap : "None".Translate();
						Widgets.Label(new Rect(num, num3, num2, Dialog_EditPrecept.EditFieldHeight), taggedString);
						float num4 = num + Text.CalcSize(taggedString).x + 10f;
						if (this.selectedReward != null)
						{
							TooltipHandler.TipRegion(new Rect(rect.x, num3, num4 - rect.x, Dialog_EditPrecept.EditFieldHeight), new TipSignal(() => CS$<>8__locals2.CS$<>8__locals1.<>4__this.selectedReward.TooltipForRitual(CS$<>8__locals2.ritual), CS$<>8__locals2.ritual.Id));
						}
						if (Widgets.ButtonText(new Rect(num4, num3 - 4f, num2 / 4f, Dialog_EditPrecept.EditFieldHeight), "RitualAttachedRewardChoose".Translate() + "...", true, true, this.attachableOutcomeEffects.Any<RitualAttachableOutcomeEffectDef>()))
						{
							List<FloatMenuOption> list3 = new List<FloatMenuOption>();
							if (this.selectedReward != null)
							{
								list3.Add(new FloatMenuOption("None".Translate(), delegate()
								{
									CS$<>8__locals2.CS$<>8__locals1.<>4__this.selectedReward = null;
									CS$<>8__locals2.CS$<>8__locals1.<>4__this.UpdateWindowHeight();
								}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
							}
							using (List<RitualAttachableOutcomeEffectDef>.Enumerator enumerator2 = this.attachableOutcomeEffects.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									Dialog_EditPrecept.<>c__DisplayClass32_4 CS$<>8__locals5 = new Dialog_EditPrecept.<>c__DisplayClass32_4();
									CS$<>8__locals5.CS$<>8__locals4 = CS$<>8__locals2;
									CS$<>8__locals5.eff = enumerator2.Current;
									if (CS$<>8__locals5.eff != this.selectedReward)
									{
										AcceptanceReport report = CS$<>8__locals5.eff.CanAttachToRitual(CS$<>8__locals5.CS$<>8__locals4.ritual);
										if (report)
										{
											list3.Add(new FloatMenuOption(CS$<>8__locals5.eff.LabelCap, delegate()
											{
												CS$<>8__locals5.CS$<>8__locals4.CS$<>8__locals1.<>4__this.selectedReward = CS$<>8__locals5.eff;
												CS$<>8__locals5.CS$<>8__locals4.CS$<>8__locals1.<>4__this.UpdateWindowHeight();
											}, MenuOptionPriority.Default, new Action<Rect>(CS$<>8__locals5.<DoWindowContents>g__DrawTooltip|4), null, 0f, null, null, true, 0));
										}
										else
										{
											list3.Add(new FloatMenuOption(CS$<>8__locals5.eff.LabelCap + " (" + report.Reason + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
										}
									}
								}
							}
							if (list3.Any<FloatMenuOption>())
							{
								Find.WindowStack.Add(new FloatMenu(list3));
							}
						}
					}
				}
			}
			if (this.apparelRequirements != null)
			{
				string text = "EditApparelRequirement".Translate();
				Text.Font = GameFont.Medium;
				Vector2 vector = Text.CalcSize(text);
				Widgets.Label(new Rect(rect.x, num3, rect.width, 35f), text);
				Text.Font = GameFont.Small;
				num3 += 39f;
				text = "NoteRoleApparelRequirementEffects".Translate();
				Rect rect2 = new Rect(rect.x, num3, rect.width, 30f);
				Color color = GUI.color;
				GUI.color = Color.gray;
				Widgets.Label(rect2, text);
				GUI.color = color;
				num3 += 40f;
				int num5 = -1;
				for (int j = 0; j < this.apparelRequirements.Count; j++)
				{
					PreceptApparelRequirement preceptApparelRequirement = this.apparelRequirements[j];
					if (Widgets.ButtonText(new Rect(rect.x + rect.width - 100f, num3, 100f, 30f), "Remove".Translate(), true, true, true))
					{
						num5 = j;
					}
					foreach (ThingDef thingDef in preceptApparelRequirement.requirement.AllRequiredApparel(Gender.None))
					{
						Rect rect3 = new Rect(rect.x + 26f + 10f + 24f, num3 + 3f, 24f, 30f);
						Rect rect4 = new Rect(rect3.x + 32f, num3, rect.width - (rect3.x - rect.x), 30f);
						text = thingDef.LabelCap;
						vector = Text.CalcSize(text);
						rect4.y += (rect4.height - vector.y) / 2f;
						Widgets.Label(rect4, text);
						Widgets.DefIcon(rect3, thingDef, null, 1f, null, false, null);
						num3 += rect4.height;
					}
					num3 += 10f;
				}
				if (num5 != -1)
				{
					this.apparelRequirements.RemoveAt(num5);
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					this.UpdateWindowHeight();
				}
				if (Widgets.ButtonText(new Rect(rect.x + 26f, num3, rect.width - 26f, 30f), "Add".Translate().CapitalizeFirst() + "...", true, true, true))
				{
					List<FloatMenuOption> list4 = new List<FloatMenuOption>();
					foreach (PreceptApparelRequirement preceptApparelRequirement2 in this.precept.def.roleApparelRequirements)
					{
						PreceptApparelRequirement localReq = preceptApparelRequirement2;
						List<ThingDef> list5 = preceptApparelRequirement2.requirement.AllRequiredApparel(Gender.None).ToList<ThingDef>();
						if (list5.Count > 0)
						{
							FloatMenuOption floatMenuOption = new FloatMenuOption(string.Join<TaggedString>(", ", from ap in list5
							select ap.LabelCap), delegate()
							{
								CS$<>8__locals1.<>4__this.apparelRequirements.Add(localReq);
								CS$<>8__locals1.<>4__this.UpdateWindowHeight();
							}, list5[0].uiIcon, list5[0].uiIconColor, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
							string str;
							floatMenuOption.Disabled = !preceptApparelRequirement2.CanAddRequirement(this.precept, this.apparelRequirements, out str, null);
							if (floatMenuOption.Disabled)
							{
								FloatMenuOption floatMenuOption2 = floatMenuOption;
								floatMenuOption2.Label = floatMenuOption2.Label + " (" + str + ")";
							}
							list4.Add(floatMenuOption);
						}
					}
					Find.WindowStack.Add(new FloatMenu(list4));
				}
			}
			if ((CS$<>8__locals1.building = (this.precept as Precept_Building)) != null && this.StylesForBuilding(CS$<>8__locals1.building).Count > 1)
			{
				ThingDef thingDef2 = CS$<>8__locals1.building.ThingDef;
				Rect rect5 = new Rect(rect);
				rect5.y += num3;
				float num6 = 8f;
				Widgets.Label(rect5, "Appearance".Translate() + ":");
				List<StyleCategoryPair> elements = this.StylesForBuilding(CS$<>8__locals1.building);
				int val = this.NumStyleRows(CS$<>8__locals1.building);
				Rect outRect = new Rect
				{
					x = rect.x + this.InitialSize.x / 3f,
					y = rect.y + num3,
					width = rect.width - this.InitialSize.x / 3f,
					height = (float)Math.Min(val, 2) * 68f + num6
				};
				Widgets.BeginScrollView(outRect, ref this.styleListScrollPos, new Rect(0f, 0f, outRect.width - 16f, this.styleListLastHeight + num6), true);
				this.styleListLastHeight = GenUI.DrawElementStack<StyleCategoryPair>(new Rect(num6, num6, outRect.width - num6, 99999f), 64f, elements, delegate(Rect r, StyleCategoryPair obj)
				{
					if (CS$<>8__locals1.<>4__this.selectedStyle != null && CS$<>8__locals1.<>4__this.selectedStyle.styleDef == obj.styleDef)
					{
						Widgets.DrawBoxSolid(r, new Color(1f, 0.8f, 0.2f, 0.2f));
					}
					if (Mouse.IsOver(r))
					{
						if (Widgets.ButtonInvisible(r, true))
						{
							CS$<>8__locals1.<>4__this.selectedStyle = obj;
							SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						}
						TooltipHandler.TipRegion(r, "UnlockedByMeme".Translate() + " " + obj.category.LabelCap + ".");
						Widgets.DrawHighlight(r);
					}
					Widgets.DefIcon(r, CS$<>8__locals1.building.ThingDef, GenStuff.DefaultStuffFor(CS$<>8__locals1.building.ThingDef), 1f, obj.styleDef, false, null);
				}, (StyleCategoryPair obj) => 64f, 6f, 5f, true).height;
				Widgets.EndScrollView();
			}
			if ((CS$<>8__locals1.relic = (this.precept as Precept_Relic)) != null && CS$<>8__locals1.relic.ThingDef.MadeFromStuff)
			{
				Widgets.Label(new Rect(rect.x, num3, num2, Dialog_EditPrecept.EditFieldHeight), "RelicStuff".Translate() + ":");
				Vector2 vector2 = Text.CalcSize(CS$<>8__locals1.relic.stuff.LabelCap);
				Text.Anchor = TextAnchor.MiddleCenter;
				Rect rect6 = new Rect(num, num3, vector2.x, Dialog_EditPrecept.EditFieldHeight);
				Widgets.Label(rect6, CS$<>8__locals1.relic.stuff.LabelCap);
				Text.Anchor = TextAnchor.UpperLeft;
				Rect rect7 = new Rect(rect6.xMax + 4f, num3, Dialog_EditPrecept.EditFieldHeight, Dialog_EditPrecept.EditFieldHeight);
				Widgets.DefIcon(rect7, CS$<>8__locals1.relic.stuff, null, 1f, null, false, null);
				TaggedString taggedString2 = "ChooseStuffForRelic".Translate() + "...";
				Vector2 vector3 = Text.CalcSize(taggedString2);
				if (Widgets.ButtonText(new Rect(rect7.xMax + 4f, num3, vector3.x + 20f, Dialog_EditPrecept.EditFieldHeight), taggedString2, true, true, true))
				{
					IEnumerable<ThingDef> enumerable = GenStuff.AllowedStuffsFor(CS$<>8__locals1.relic.ThingDef, TechLevel.Undefined);
					if (enumerable.Count<ThingDef>() > 0)
					{
						List<FloatMenuOption> list6 = new List<FloatMenuOption>();
						foreach (ThingDef thingDef3 in enumerable)
						{
							ThingDef localStuff = thingDef3;
							list6.Add(new FloatMenuOption(thingDef3.LabelCap, delegate()
							{
								CS$<>8__locals1.relic.stuff = localStuff;
							}, thingDef3, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
						Find.WindowStack.Add(new FloatMenu(list6));
					}
				}
				num3 += Dialog_EditPrecept.EditFieldHeight + 10f;
			}
			if (Widgets.ButtonText(new Rect(0f, rect.height - Dialog_EditPrecept.ButSize.y, Dialog_EditPrecept.ButSize.x, Dialog_EditPrecept.ButSize.y), "Back".Translate(), true, true, true))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(rect.width / 2f - Dialog_EditPrecept.ButSize.x / 2f, rect.height - Dialog_EditPrecept.ButSize.y, Dialog_EditPrecept.ButSize.x, Dialog_EditPrecept.ButSize.y), "Randomize".Translate(), true, true, true))
			{
				this.newPreceptName = (this.newPreceptNameFemale = this.precept.GenerateNewName());
				this.apparelRequirements = this.precept.GenerateNewApparelRequirements(null);
				if (this.dateTrigger != null)
				{
					this.newTriggerDaysSinceStartOfYear = this.dateTrigger.RandomDate();
					this.RecalculateQuadrumAndDay();
				}
				Precept_Ritual precept_Ritual;
				if ((precept_Ritual = (this.precept as Precept_Ritual)) != null)
				{
					if (precept_Ritual.canBeAnytime && !precept_Ritual.sourcePattern.alwaysStartAnytime)
					{
						this.newCanStartAnytime = Rand.Bool;
					}
					if (this.attachableUsableOutcomeEffects.Any<RitualAttachableOutcomeEffectDef>())
					{
						this.selectedReward = this.attachableUsableOutcomeEffects.RandomElement<RitualAttachableOutcomeEffectDef>();
					}
				}
				Precept_Building building;
				if ((building = (this.precept as Precept_Building)) != null)
				{
					this.selectedStyle = this.StylesForBuilding(building).RandomElement<StyleCategoryPair>();
				}
				Precept_Relic precept_Relic;
				if ((precept_Relic = (this.precept as Precept_Relic)) != null)
				{
					precept_Relic.SetRandomStuff();
				}
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				this.UpdateWindowHeight();
			}
			if (Widgets.ButtonText(new Rect(rect.width - Dialog_EditPrecept.ButSize.x, rect.height - Dialog_EditPrecept.ButSize.y, Dialog_EditPrecept.ButSize.x, Dialog_EditPrecept.ButSize.y), "DoneButton".Translate(), true, true, true))
			{
				this.ApplyChanges();
			}
		}

		// Token: 0x060073CC RID: 29644 RVA: 0x00271918 File Offset: 0x0026FB18
		private void RecalculateQuadrumAndDay()
		{
			Map map = (Current.ProgramState == ProgramState.Playing) ? Find.AnyPlayerHomeMap : null;
			float longitude = (map != null) ? Find.WorldGrid.LongLatOf(map.Tile).x : 0f;
			this.quadrum = GenDate.Quadrum((long)(this.newTriggerDaysSinceStartOfYear * 60000), longitude);
			this.day = GenDate.DayOfQuadrum((long)(this.newTriggerDaysSinceStartOfYear * 60000), longitude);
		}

		// Token: 0x060073CD RID: 29645 RVA: 0x00271988 File Offset: 0x0026FB88
		private void ApplyChanges()
		{
			this.precept.SetName(this.newPreceptName);
			if (this.precept.def.leaderRole)
			{
				this.precept.ideo.leaderTitleMale = this.newPreceptName;
				this.precept.ideo.leaderTitleFemale = this.newPreceptNameFemale;
			}
			Precept_Ritual precept_Ritual;
			if ((precept_Ritual = (this.precept as Precept_Ritual)) != null)
			{
				precept_Ritual.isAnytime = this.newCanStartAnytime;
				precept_Ritual.attachableOutcomeEffect = this.selectedReward;
			}
			if (this.dateTrigger != null && this.newTriggerDaysSinceStartOfYear != this.dateTrigger.triggerDaysSinceStartOfYear)
			{
				this.dateTrigger.triggerDaysSinceStartOfYear = this.newTriggerDaysSinceStartOfYear;
			}
			foreach (Precept precept in this.precept.ideo.PreceptsListForReading)
			{
				precept.ClearTipCache();
			}
			if (this.apparelRequirements != null)
			{
				this.precept.ApparelRequirements = this.apparelRequirements;
			}
			Precept_Building precept_Building;
			if ((precept_Building = (this.precept as Precept_Building)) != null && this.selectedStyle != null)
			{
				this.precept.ideo.style.SetStyleForThingDef(precept_Building.ThingDef, this.selectedStyle);
			}
			this.Close(true);
		}

		// Token: 0x060073CE RID: 29646 RVA: 0x00271ADC File Offset: 0x0026FCDC
		private List<StyleCategoryPair> StylesForBuilding(Precept_Building building)
		{
			ThingDef thingDef = building.ThingDef;
			if (thingDef != null && thingDef.canEditAnyStyle)
			{
				return Precept_ThingDef.AllPossibleStylesForBuilding(building.ThingDef);
			}
			Dialog_EditPrecept.stylesForBuildingTmp.Clear();
			foreach (ThingStyleCategoryWithPriority thingStyleCategoryWithPriority in this.precept.ideo.thingStyleCategories)
			{
				foreach (ThingDefStyle thingDefStyle in thingStyleCategoryWithPriority.category.thingDefStyles)
				{
					if (thingDefStyle.ThingDef == building.ThingDef)
					{
						Dialog_EditPrecept.stylesForBuildingTmp.Add(new StyleCategoryPair
						{
							category = thingStyleCategoryWithPriority.category,
							styleDef = thingDefStyle.StyleDef
						});
					}
				}
			}
			return Dialog_EditPrecept.stylesForBuildingTmp;
		}

		// Token: 0x04003F82 RID: 16258
		private Precept precept;

		// Token: 0x04003F83 RID: 16259
		private string newPreceptName;

		// Token: 0x04003F84 RID: 16260
		private string newPreceptNameFemale;

		// Token: 0x04003F85 RID: 16261
		private float windowHeight = 170f;

		// Token: 0x04003F86 RID: 16262
		private float attachedRewardDescWidth;

		// Token: 0x04003F87 RID: 16263
		private float attachedRewardDescHeight;

		// Token: 0x04003F88 RID: 16264
		private int newTriggerDaysSinceStartOfYear = -1;

		// Token: 0x04003F89 RID: 16265
		private bool newCanStartAnytime;

		// Token: 0x04003F8A RID: 16266
		private Quadrum quadrum;

		// Token: 0x04003F8B RID: 16267
		private int day;

		// Token: 0x04003F8C RID: 16268
		private StyleCategoryPair selectedStyle;

		// Token: 0x04003F8D RID: 16269
		private float styleListLastHeight;

		// Token: 0x04003F8E RID: 16270
		private Vector2 styleListScrollPos;

		// Token: 0x04003F8F RID: 16271
		private RitualObligationTrigger_Date dateTrigger;

		// Token: 0x04003F90 RID: 16272
		private static readonly Vector2 ButSize = new Vector2(150f, 38f);

		// Token: 0x04003F91 RID: 16273
		private static readonly float EditFieldHeight = 30f;

		// Token: 0x04003F92 RID: 16274
		private const float ApparelRequirementThingHeight = 30f;

		// Token: 0x04003F93 RID: 16275
		private const float ApparelRequirementBtnHeight = 30f;

		// Token: 0x04003F94 RID: 16276
		private const float WindowBaseHeight = 170f;

		// Token: 0x04003F95 RID: 16277
		private const float HeaderHeight = 35f;

		// Token: 0x04003F96 RID: 16278
		private const float ApparelRequirementNoteHeight = 30f;

		// Token: 0x04003F97 RID: 16279
		private const float BuildingStyleIconSize = 64f;

		// Token: 0x04003F98 RID: 16280
		private List<PreceptApparelRequirement> apparelRequirements;

		// Token: 0x04003F99 RID: 16281
		private RitualAttachableOutcomeEffectDef selectedReward;

		// Token: 0x04003F9A RID: 16282
		private List<RitualAttachableOutcomeEffectDef> attachableOutcomeEffects;

		// Token: 0x04003F9B RID: 16283
		private List<RitualAttachableOutcomeEffectDef> attachableUsableOutcomeEffects;

		// Token: 0x04003F9C RID: 16284
		private static List<StyleCategoryPair> stylesForBuildingTmp = new List<StyleCategoryPair>();
	}
}
