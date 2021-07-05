using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020012DB RID: 4827
	[StaticConstructorOnStartup]
	public class Dialog_BeginRitual : Window
	{
		// Token: 0x17001435 RID: 5173
		// (get) Token: 0x06007360 RID: 29536 RVA: 0x00269857 File Offset: 0x00267A57
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(845f, 740f);
			}
		}

		// Token: 0x17001436 RID: 5174
		// (get) Token: 0x06007361 RID: 29537 RVA: 0x00269868 File Offset: 0x00267A68
		protected Vector2 ButSize
		{
			get
			{
				return new Vector2(200f, 40f);
			}
		}

		// Token: 0x17001437 RID: 5175
		// (get) Token: 0x06007362 RID: 29538 RVA: 0x0026987C File Offset: 0x00267A7C
		protected string WarningText
		{
			get
			{
				string result = "";
				using (IEnumerator<string> enumerator = this.BlockingIssues().GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						result = enumerator.Current;
					}
				}
				return result;
			}
		}

		// Token: 0x17001438 RID: 5176
		// (get) Token: 0x06007363 RID: 29539 RVA: 0x002698CC File Offset: 0x00267ACC
		public string SleepingWarning
		{
			get
			{
				if (this.sleepingMessage.NullOrEmpty())
				{
					this.sleepingMessage = "RitualBeginSleepingWarning".Translate();
				}
				if (this.assignments.Participants.Any((Pawn p) => !p.Awake()))
				{
					return this.sleepingMessage;
				}
				return null;
			}
		}

		// Token: 0x06007364 RID: 29540 RVA: 0x00269934 File Offset: 0x00267B34
		public Dialog_BeginRitual(string header, string ritualLabel, Precept_Ritual ritual, TargetInfo target, Map map, Dialog_BeginRitual.ActionCallback action, Pawn organizer, RitualObligation obligation, Func<Pawn, bool, bool, bool> filter = null, string confirmText = null, List<Pawn> requiredPawns = null, Dictionary<string, Pawn> forcedForRole = null, string ritualName = null, RitualOutcomeEffectDef outcome = null, List<string> extraInfoText = null, Pawn selectedPawn = null)
		{
			if (!ModLister.CheckRoyaltyOrIdeology("Ritual"))
			{
				return;
			}
			this.ritual = ritual;
			this.target = target;
			this.obligation = obligation;
			this.extraInfos = extraInfoText;
			this.selectedPawn = selectedPawn;
			this.assignments = new RitualRoleAssignments(ritual);
			List<Pawn> list = new List<Pawn>(map.mapPawns.FreeColonistsAndPrisonersSpawned);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				Pawn pawn = list[i];
				if (filter != null && !filter(pawn, true, true))
				{
					list.RemoveAt(i);
				}
				else
				{
					bool flag2;
					bool flag = RitualRoleAssignments.PawnUnavailableReason(pawn, null, ritual, this.assignments, out flag2) == null || flag2;
					if (!flag && ritual != null)
					{
						foreach (RitualRole ritualRole in ritual.behavior.def.roles)
						{
							if ((RitualRoleAssignments.PawnUnavailableReason(pawn, ritualRole, ritual, this.assignments, out flag2) == null || flag2) && (filter == null || filter(pawn, !(ritualRole is RitualRoleForced), ritualRole.allowOtherIdeos)) && (ritualRole.maxCount > 1 || forcedForRole == null || !forcedForRole.ContainsKey(ritualRole.id)))
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						list.RemoveAt(i);
					}
				}
			}
			if (requiredPawns != null)
			{
				foreach (Pawn item in requiredPawns)
				{
					if (!list.Contains(item))
					{
						list.Add(item);
					}
				}
			}
			if (forcedForRole != null)
			{
				foreach (KeyValuePair<string, Pawn> keyValuePair in forcedForRole)
				{
					list.AddDistinct(keyValuePair.Value);
				}
			}
			if (ritual != null)
			{
				using (List<RitualRole>.Enumerator enumerator = ritual.behavior.def.roles.GetEnumerator())
				{
					Func<Pawn, bool> <>9__0;
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Animal)
						{
							List<Pawn> list2 = list;
							IEnumerable<Pawn> spawnedColonyAnimals = map.mapPawns.SpawnedColonyAnimals;
							Func<Pawn, bool> predicate;
							if ((predicate = <>9__0) == null)
							{
								predicate = (<>9__0 = ((Pawn p) => filter == null || filter(p, true, true)));
							}
							list2.AddRange(spawnedColonyAnimals.Where(predicate));
							break;
						}
					}
				}
			}
			this.assignments.Setup(list, forcedForRole, requiredPawns, selectedPawn);
			this.ritualExplanation = ((ritual != null) ? ritual.ritualExplanation : null);
			this.action = action;
			this.filter = filter;
			this.map = map;
			this.ritualLabel = ritualLabel;
			this.headerText = header;
			this.confirmText = confirmText;
			this.organizer = organizer;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
			this.forcePause = true;
			this.outcome = ((ritual != null && ritual.outcomeEffect != null) ? ritual.outcomeEffect.def : outcome);
		}

		// Token: 0x06007365 RID: 29541 RVA: 0x00269C90 File Offset: 0x00267E90
		public override void PostOpen()
		{
			this.assignments.FillPawns(this.filter);
		}

		// Token: 0x06007366 RID: 29542 RVA: 0x00269CA4 File Offset: 0x00267EA4
		public override void DoWindowContents(Rect inRect)
		{
			float num = 0f;
			Text.Font = GameFont.Medium;
			Rect rect = new Rect(inRect)
			{
				height = Text.CalcHeight(this.ritualLabel, inRect.width) + 4f
			};
			Widgets.Label(rect, this.ritualLabel);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.LowerRight;
			GUI.color = Color.gray;
			if (this.ritual != null && !this.ritual.Label.EqualsIgnoreCase(this.ritual.UIInfoFirstLine))
			{
				Widgets.Label(new Rect(inRect)
				{
					height = Text.CalcHeight(this.ritual.UIInfoFirstLine, inRect.width) + 4f
				}, this.ritual.UIInfoFirstLine);
			}
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
			num += rect.height;
			num += 4f;
			Precept_Ritual precept_Ritual = this.ritual;
			string text = (precept_Ritual != null) ? precept_Ritual.Description : null;
			if (!text.NullOrEmpty())
			{
				text = text.Formatted(this.organizer.Named("ORGANIZER"));
				float num2 = Text.CalcHeight(text, inRect.width);
				Rect rect2 = inRect;
				rect2.x += 10f;
				rect2.width -= 20f;
				rect2.yMin = num + 10f;
				rect2.height = num2;
				Widgets.Label(rect2, text);
				num += num2 + 17f;
			}
			Precept_Ritual precept_Ritual2 = this.ritual;
			string text2;
			if (precept_Ritual2 == null)
			{
				text2 = null;
			}
			else
			{
				RitualBehaviorWorker behavior = precept_Ritual2.behavior;
				float num3;
				text2 = ((behavior != null) ? behavior.GetExplanation(this.ritual, this.assignments, this.PredictedQuality(out num3)) : null);
			}
			string text3 = text2;
			if (!this.ritualExplanation.NullOrEmpty() || !text3.NullOrEmpty())
			{
				string text4 = this.ritualExplanation;
				if (!text3.NullOrEmpty())
				{
					if (!text4.NullOrEmpty())
					{
						text4 += "\n\n";
					}
					text4 += text3;
				}
				float num4 = Text.CalcHeight(text4, inRect.width);
				Rect rect3 = inRect;
				rect3.x += 10f;
				rect3.width -= 20f;
				rect3.yMin = num + 10f;
				rect3.height = num4;
				Widgets.Label(rect3, text4);
				num += num4 + 17f;
			}
			Rect source = new Rect(inRect);
			source.yMin = num + 10f;
			source.yMax -= this.ButSize.y + 10f + 6f;
			Rect rect4 = new Rect(source);
			rect4.width = 320f;
			rect4.x += 20f;
			rect4.height -= 10f;
			Rect viewRect = new Rect(0f, 0f, rect4.width - ((this.listScrollViewHeight > rect4.height) ? 16f : 0f), this.listScrollViewHeight);
			Widgets.BeginScrollView(rect4, ref this.scrollPositionPawns, viewRect, true);
			try
			{
				this.DrawPawnList(viewRect, rect4);
			}
			finally
			{
				Widgets.EndScrollView();
			}
			this.DrawQualityFactors(new Rect(source)
			{
				x = rect4.xMax + 28f,
				width = 402f
			});
			Rect rect5 = default(Rect);
			rect5.xMin = inRect.xMin;
			rect5.xMax = inRect.xMax;
			rect5.y = source.y + 17f - 2f;
			rect5.height = source.height;
			Rect rect6 = new Rect(rect5.xMax - this.ButSize.x - 250f - 10f, rect5.yMax, 250f, this.ButSize.y);
			GUI.color = ColorLibrary.RedReadable;
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect6, this.WarningText);
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
			if (this.target.Thing != null)
			{
				TaggedString taggedString = "RitualTakesPlaceAt".Translate() + ": ";
				TaggedString taggedString2 = taggedString + this.target.Thing.LabelShortCap;
				float x = Text.CalcSize(taggedString).x;
				float x2 = Text.CalcSize(this.target.Thing.LabelShortCap).x;
				float num5 = Text.CalcSize(taggedString2).x + 4f + 32f;
				Rect rect7 = new Rect(rect5.xMax - (x2 + 4f + 32f), rect5.yMax - 34f, 32f, 32f);
				Rect rect8 = new Rect(rect5.xMax - num5, rect5.yMax - 28f, x, 24f);
				Rect rect9 = new Rect(rect7.xMax + 4f, rect5.yMax - 28f, x2, 24f);
				Widgets.Label(rect8, taggedString);
				Widgets.Label(rect9, this.target.Thing.LabelShortCap);
				Widgets.ThingIcon(rect7, this.target.Thing, 1f, null);
				if (Mouse.IsOver(rect8) || Mouse.IsOver(rect9) || Mouse.IsOver(rect7))
				{
					Find.WindowStack.ImmediateWindow(738453, new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight), WindowLayer.Super, delegate
					{
						GenUI.DrawArrowPointingAtWorldspace(this.target.Cell.ToVector3(), Find.Camera);
					}, false, false, 0f, null);
				}
			}
			Rect rect10 = new Rect(rect5.xMax - this.ButSize.x, rect5.yMax, this.ButSize.x, this.ButSize.y);
			Rect rect11 = new Rect(rect5.x, rect5.yMax, this.ButSize.x, this.ButSize.y);
			bool flag = !this.BlockingIssues().Any<string>();
			if (!flag)
			{
				GUI.color = Color.gray;
			}
			if (Widgets.ButtonText(rect10, this.confirmText ?? "OK".Translate(), true, true, flag))
			{
				float num3;
				if (this.PredictedQuality(out num3) < 0.25f && this.outcome.warnOnLowQuality)
				{
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("RitualQualityLowWarning".Translate(this.ritualLabel, 0.25f.ToStringPercent()), new Action(this.<DoWindowContents>g__Start|52_1), true, null));
				}
				else
				{
					this.<DoWindowContents>g__Start|52_1();
				}
			}
			GUI.color = Color.white;
			if (Widgets.ButtonText(rect11, "CancelButton".Translate(), true, true, true))
			{
				this.Close(true);
			}
		}

		// Token: 0x06007367 RID: 29543 RVA: 0x0026A3A4 File Offset: 0x002685A4
		protected void DrawQualityFactors(Rect viewRect)
		{
			Dialog_BeginRitual.<>c__DisplayClass54_0 CS$<>8__locals1;
			CS$<>8__locals1.viewRect = viewRect;
			if (this.outcome == null)
			{
				return;
			}
			float num = CS$<>8__locals1.viewRect.y;
			CS$<>8__locals1.totalInfoHeight = 0f;
			CS$<>8__locals1.even = true;
			this.expectedOutcomeEffects.Clear();
			float startingQuality = this.outcome.startingQuality;
			float num3;
			float num2 = this.PredictedQuality(out num3);
			if (startingQuality > 0f)
			{
				this.expectedOutcomeEffects.Add(new ExpectedOutcomeDesc
				{
					label = "StartingQuality".Translate(),
					effect = "+" + startingQuality.ToStringPercent("F0"),
					quality = startingQuality,
					noMiddleColumnInfo = true,
					positive = true,
					priority = 5f
				});
			}
			if (this.ritual != null && this.ritual.RepeatPenaltyActive)
			{
				float repeatQualityPenalty = this.ritual.RepeatQualityPenalty;
				this.expectedOutcomeEffects.Add(new ExpectedOutcomeDesc
				{
					label = "RitualOutcomePerformedRecently".Translate(),
					effect = repeatQualityPenalty.ToStringPercent(),
					quality = repeatQualityPenalty,
					noMiddleColumnInfo = true,
					positive = false,
					priority = 5f
				});
			}
			Map map = this.map;
			Precept_Ritual precept_Ritual = this.ritual;
			Tuple<ExpectationDef, float> expectationsOffset = RitualOutcomeEffectWorker_FromQuality.GetExpectationsOffset(map, (precept_Ritual != null) ? precept_Ritual.def : null);
			if (expectationsOffset != null)
			{
				this.expectedOutcomeEffects.Add(new ExpectedOutcomeDesc
				{
					label = "RitualQualityExpectations".Translate(expectationsOffset.Item1.LabelCap),
					effect = "+" + expectationsOffset.Item2.ToStringPercent(),
					quality = expectationsOffset.Item2,
					noMiddleColumnInfo = true,
					positive = true,
					priority = 5f
				});
			}
			if (this.expectedOutcomeEffects.NullOrEmpty<ExpectedOutcomeDesc>())
			{
				return;
			}
			Widgets.Label(new Rect(CS$<>8__locals1.viewRect.x, num + 3f, CS$<>8__locals1.viewRect.width, 32f), "QualityFactors".Translate());
			num += 32f;
			CS$<>8__locals1.totalInfoHeight += 32f;
			foreach (ExpectedOutcomeDesc expectedOutcomeDesc in from e in this.expectedOutcomeEffects
			orderby e.priority descending
			select e)
			{
				Dialog_BeginRitual.<DrawQualityFactors>g__DrawQualityFactor|54_0(expectedOutcomeDesc, ref num, ref CS$<>8__locals1);
				CS$<>8__locals1.even = !CS$<>8__locals1.even;
			}
			num += 2f;
			if (num2 < 0.25f)
			{
				GUI.color = ColorLibrary.RedReadable;
			}
			Rect rect = new Rect(CS$<>8__locals1.viewRect.x, num + 4f, CS$<>8__locals1.viewRect.width, 25f);
			string str;
			if (this.ritual != null && this.ritual.outcomeEffect != null && this.ritual.outcomeEffect.ExpectedQualityLabel() != null)
			{
				str = this.ritual.outcomeEffect.ExpectedQualityLabel();
			}
			else
			{
				str = "ExpectedRitualQuality".Translate();
			}
			Widgets.Label(rect, str + ":");
			Text.Font = GameFont.Medium;
			string text = (num2 == num3) ? num2.ToStringPercent("F0") : (num2.ToStringPercent("F0") + "-" + num3.ToStringPercent("F0"));
			float x = Text.CalcSize(text).x;
			Widgets.Label(new Rect(CS$<>8__locals1.viewRect.xMax - x, num - 2f, CS$<>8__locals1.viewRect.width, 32f), text);
			Text.Font = GameFont.Small;
			num += 28f;
			CS$<>8__locals1.totalInfoHeight += 28f;
			Rect rect2 = CS$<>8__locals1.viewRect;
			rect2.width += 10f;
			rect2.height = CS$<>8__locals1.totalInfoHeight;
			rect2 = rect2.ExpandedBy(9f);
			GUI.color = new Color(0.25f, 0.25f, 0.25f);
			Widgets.DrawBox(rect2, 2, null);
			GUI.color = Color.white;
			num += 28f;
			CS$<>8__locals1.totalInfoHeight += 28f;
			if (this.ritual != null)
			{
				int num4 = this.ritual.behavior.ExpectedDurationOverride(this.ritual, this.assignments, num2);
				string label = "ExpectedRitualDuration".Translate() + ": " + ((num4 > 0) ? num4 : this.ritual.behavior.def.durationTicks.max).ToStringTicksToPeriod(false, false, true, true);
				Widgets.Label(new Rect(CS$<>8__locals1.viewRect.x, num - 4f, CS$<>8__locals1.viewRect.width, 32f), label);
				num += 17f;
				CS$<>8__locals1.totalInfoHeight += 17f;
			}
			if (!this.outcome.outcomeChances.NullOrEmpty<OutcomeChance>())
			{
				Widgets.Label(new Rect(CS$<>8__locals1.viewRect.x, num, CS$<>8__locals1.viewRect.width, 32f), "RitualOutcomeChances".Translate(text) + ": ");
				num += 28f;
				CS$<>8__locals1.totalInfoHeight += 28f;
				float num5 = 0f;
				foreach (OutcomeChance outcomeChance in this.outcome.outcomeChances)
				{
					num5 += (outcomeChance.Positive ? (outcomeChance.chance * num2) : outcomeChance.chance);
				}
				foreach (OutcomeChance outcomeChance2 in this.outcome.outcomeChances)
				{
					float f = outcomeChance2.Positive ? (outcomeChance2.chance * num2 / num5) : (outcomeChance2.chance / num5);
					string text2 = "  - " + outcomeChance2.label + ": " + f.ToStringPercent();
					Rect rect3 = new Rect(CS$<>8__locals1.viewRect.x, num, Text.CalcSize(text2).x, 32f);
					Rect rect4 = new Rect(rect3)
					{
						width = rect3.width + 8f,
						height = 22f
					};
					if (Mouse.IsOver(rect4))
					{
						string desc = this.outcome.OutcomeMoodBreakdown(outcomeChance2);
						if (!outcomeChance2.potentialExtraOutcomeDesc.NullOrEmpty())
						{
							if (!desc.NullOrEmpty())
							{
								desc += "\n\n";
							}
							desc += outcomeChance2.potentialExtraOutcomeDesc;
						}
						Widgets.DrawLightHighlight(rect4);
						if (!desc.NullOrEmpty())
						{
							TooltipHandler.TipRegion(rect4, () => desc, 231134347);
						}
					}
					Widgets.Label(rect3, text2);
					num += Text.LineHeight;
					CS$<>8__locals1.totalInfoHeight += Text.LineHeight;
				}
			}
			num += 10f;
			CS$<>8__locals1.totalInfoHeight += 10f;
			if (this.extraInfos != null)
			{
				foreach (string text3 in this.extraInfos)
				{
					float num6 = Math.Max(Text.CalcHeight(text3, CS$<>8__locals1.viewRect.width) + 3f, 28f);
					Widgets.Label(new Rect(CS$<>8__locals1.viewRect.x, num + 4f, CS$<>8__locals1.viewRect.width, num6), text3);
					num += num6;
					CS$<>8__locals1.totalInfoHeight += num6;
				}
			}
			string sleepingWarning = this.SleepingWarning;
			if (!sleepingWarning.NullOrEmpty())
			{
				float num7 = Math.Max(Text.CalcHeight(sleepingWarning, CS$<>8__locals1.viewRect.width) + 3f, 28f);
				Widgets.Label(new Rect(CS$<>8__locals1.viewRect.x, num + 4f, CS$<>8__locals1.viewRect.width, num7), sleepingWarning);
				num += num7;
			}
			GUI.color = Color.white;
		}

		// Token: 0x06007368 RID: 29544 RVA: 0x0026ACBC File Offset: 0x00268EBC
		private float PredictedQuality(out float potentialMax)
		{
			float num = this.outcome.startingQuality;
			potentialMax = 0f;
			foreach (RitualOutcomeComp ritualOutcomeComp in this.outcome.comps)
			{
				ExpectedOutcomeDesc expectedOutcomeDesc = ritualOutcomeComp.GetExpectedOutcomeDesc(this.ritual, this.target, this.obligation, this.assignments);
				if (expectedOutcomeDesc != null)
				{
					this.expectedOutcomeEffects.Add(expectedOutcomeDesc);
					if (expectedOutcomeDesc.uncertainOutcome)
					{
						potentialMax += expectedOutcomeDesc.quality;
					}
					else
					{
						num += expectedOutcomeDesc.quality;
					}
				}
			}
			if (this.ritual != null && this.ritual.RepeatPenaltyActive)
			{
				num += this.ritual.RepeatQualityPenalty;
			}
			Map map = this.map;
			Precept_Ritual precept_Ritual = this.ritual;
			Tuple<ExpectationDef, float> expectationsOffset = RitualOutcomeEffectWorker_FromQuality.GetExpectationsOffset(map, (precept_Ritual != null) ? precept_Ritual.def : null);
			if (expectationsOffset != null)
			{
				num += expectationsOffset.Item2;
			}
			num = Mathf.Clamp(num, this.outcome.minQuality, this.outcome.maxQuality);
			potentialMax += num;
			potentialMax = Mathf.Clamp(potentialMax, this.outcome.minQuality, this.outcome.maxQuality);
			return num;
		}

		// Token: 0x06007369 RID: 29545 RVA: 0x0026ADFC File Offset: 0x00268FFC
		private void CalculatePawnPortraitIcons(Pawn pawn)
		{
			Ideo ideo = pawn.Ideo;
			if (this.assignments.Required(pawn))
			{
				Dialog_BeginRitual.tmpPortraitIcons.Add(new Dialog_BeginRitual.PawnPortraitIcon
				{
					color = Color.white,
					icon = IdeoUIUtility.LockedTex,
					tooltip = "Required".Translate()
				});
			}
			if (ModsConfig.IdeologyActive && ideo != null)
			{
				Dialog_BeginRitual.tmpPortraitIcons.Add(new Dialog_BeginRitual.PawnPortraitIcon
				{
					color = ideo.Color,
					icon = ideo.Icon,
					tooltip = ideo.memberName
				});
				Precept_Role role = ideo.GetRole(pawn);
				if (role != null)
				{
					Dialog_BeginRitual.tmpPortraitIcons.Add(new Dialog_BeginRitual.PawnPortraitIcon
					{
						color = ideo.Color,
						icon = role.Icon,
						tooltip = role.TipLabel
					});
				}
				GUI.color = Color.white;
				Faction homeFaction = pawn.HomeFaction;
				if (homeFaction != null && !homeFaction.IsPlayer)
				{
					Dialog_BeginRitual.tmpPortraitIcons.Add(new Dialog_BeginRitual.PawnPortraitIcon
					{
						color = homeFaction.Color,
						icon = homeFaction.def.FactionIcon,
						tooltip = "Faction".Translate() + ": " + homeFaction.Name + "\n" + homeFaction.def.LabelCap
					});
				}
				if (pawn.IsSlave)
				{
					Dialog_BeginRitual.tmpPortraitIcons.Add(new Dialog_BeginRitual.PawnPortraitIcon
					{
						color = Color.white,
						icon = Dialog_BeginRitual.slaveryIcon,
						tooltip = "RitualBeginSlaveDesc".Translate()
					});
				}
				if (pawn.IsPrisoner)
				{
					Dialog_BeginRitual.tmpPortraitIcons.Add(new Dialog_BeginRitual.PawnPortraitIcon
					{
						color = Color.white,
						icon = Dialog_BeginRitual.prisonerIcon,
						tooltip = null
					});
				}
				if (!pawn.Awake())
				{
					Dialog_BeginRitual.tmpPortraitIcons.Add(new Dialog_BeginRitual.PawnPortraitIcon
					{
						color = Color.white,
						icon = Dialog_BeginRitual.sleepingIcon,
						tooltip = "RitualBeginSleepingDesc".Translate(pawn)
					});
				}
			}
		}

		// Token: 0x0600736A RID: 29546 RVA: 0x0026B054 File Offset: 0x00269254
		private void DrawPawnPortrait(Rect rect, Pawn pawn, Action clickHandler = null, Action rightClickHandler = null)
		{
			Dialog_BeginRitual.<>c__DisplayClass59_0 CS$<>8__locals1 = new Dialog_BeginRitual.<>c__DisplayClass59_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.rect = rect;
			CS$<>8__locals1.pawn = pawn;
			if (Mouse.IsOver(CS$<>8__locals1.rect) && Event.current.type == EventType.MouseDown && Event.current.button == 1)
			{
				rightClickHandler();
			}
			CS$<>8__locals1.mp = Event.current.mousePosition;
			if (!this.assignments.Required(CS$<>8__locals1.pawn) && DragAndDropWidget.Draggable(this.dragAndDropGroup, CS$<>8__locals1.rect, CS$<>8__locals1.pawn, clickHandler, delegate
			{
				CS$<>8__locals1.<>4__this.lastHoveredDropArea = DragAndDropWidget.HoveringDropAreaRect(CS$<>8__locals1.<>4__this.dragAndDropGroup, new Vector3?(CS$<>8__locals1.mp));
			}))
			{
				CS$<>8__locals1.rect.position = Event.current.mousePosition;
				Dialog_BeginRitual.tmpDelayedGuiCalls.Add(delegate
				{
					base.<DrawPawnPortrait>g__DrawInternal|0(CS$<>8__locals1.rect, CS$<>8__locals1.pawn, 0.9f);
				});
				return;
			}
			CS$<>8__locals1.<DrawPawnPortrait>g__DrawInternal|0(CS$<>8__locals1.rect, CS$<>8__locals1.pawn, 1f);
			Widgets.DrawHighlightIfMouseover(CS$<>8__locals1.rect);
		}

		// Token: 0x0600736B RID: 29547 RVA: 0x0026B140 File Offset: 0x00269340
		private string ExtraPawnAssignmentInfo(IEnumerable<RitualRole> roleGroup, Pawn pawnToBeAssigned = null)
		{
			RitualRole role = (roleGroup != null) ? roleGroup.First<RitualRole>() : null;
			IEnumerable<Pawn> enumerable = this.assignments.AssignedPawns(role);
			if (pawnToBeAssigned != null)
			{
				enumerable = enumerable.Concat(new Pawn[]
				{
					pawnToBeAssigned
				}).Distinct<Pawn>();
			}
			string text = (pawnToBeAssigned == null) ? role.ExtraInfoForDialog(enumerable) : null;
			Pawn pawn = pawnToBeAssigned ?? enumerable.FirstOrDefault<Pawn>();
			PreceptDef preceptDef;
			if (pawn == null)
			{
				preceptDef = null;
			}
			else
			{
				Ideo ideo = pawn.Ideo;
				if (ideo == null)
				{
					preceptDef = null;
				}
				else
				{
					Precept_Role role2 = ideo.GetRole(pawn);
					preceptDef = ((role2 != null) ? role2.def : null);
				}
			}
			PreceptDef preceptDef2 = preceptDef;
			if (pawn != null && preceptDef2 != role.precept && role.substitutable && role.precept != null)
			{
				if (text != null)
				{
					text += "\n\n";
				}
				Precept precept = this.ritual.ideo.PreceptsListForReading.First((Precept p) => p.def == role.precept);
				text += "RitualRoleRequiresSocialRole".Translate(precept.Label);
				string text2 = null;
				bool flag = false;
				if (this.ritual.outcomeEffect != null && !this.ritual.outcomeEffect.def.comps.NullOrEmpty<RitualOutcomeComp>())
				{
					foreach (RitualOutcomeComp ritualOutcomeComp in this.ritual.outcomeEffect.def.comps)
					{
						if (ritualOutcomeComp is RitualOutcomeComp_RolePresentNotSubstituted)
						{
							if (flag)
							{
								text2 += ", ";
							}
							text2 += ritualOutcomeComp.GetBonusDescShort();
							flag = true;
						}
					}
				}
				text = text + ": " + (flag ? text2 : "None".Translate().CapitalizeFirst().Resolve());
			}
			if (role.required && pawnToBeAssigned == null && this.assignments.FirstAssignedPawn(role) == null)
			{
				int num = 0;
				foreach (RitualRole ritualRole in roleGroup)
				{
					num += ritualRole.maxCount;
				}
				if (num > 1)
				{
					text += "MessageRitualNeedsAtLeastNumRolePawn".Translate(Find.ActiveLanguageWorker.Pluralize(role.Label, -1), num);
				}
				else
				{
					text += "MessageRitualNeedsAtLeastOneRolePawn".Translate(role.Label);
				}
			}
			return text;
		}

		// Token: 0x0600736C RID: 29548 RVA: 0x0026B41C File Offset: 0x0026961C
		private string CannotAssignReason(Pawn draggable, IEnumerable<RitualRole> roles, out RitualRole firstRole, out bool mustReplace, bool isReplacing = false)
		{
			int num = 0;
			int num2 = 0;
			firstRole = ((roles == null) ? null : roles.FirstOrDefault<RitualRole>());
			mustReplace = false;
			string text = this.assignments.PawnUnavailableReason(draggable, firstRole).CapitalizeFirst();
			if (text == null && roles != null && !roles.Any((RitualRole r) => this.assignments.RoleForPawn(draggable, true) == r))
			{
				foreach (RitualRole ritualRole in roles)
				{
					if (ritualRole.maxCount <= 0)
					{
						num = -1;
					}
					if (num != -1)
					{
						num += ritualRole.maxCount;
					}
					num2 += this.assignments.AssignedPawns(ritualRole).Count<Pawn>();
				}
				if (num >= 0 && num <= num2)
				{
					mustReplace = true;
					if (!isReplacing)
					{
						text = "MaxPawnsPerRole".Translate(firstRole.Label, num);
					}
				}
			}
			return text;
		}

		// Token: 0x0600736D RID: 29549 RVA: 0x0026B52C File Offset: 0x0026972C
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			this.scrollPositionPawns.y = this.scrollPositionPawns.y + (float)(this.pawnsListEdgeScrollDirection * 1000) * Time.deltaTime;
		}

		// Token: 0x0600736E RID: 29550 RVA: 0x0026B558 File Offset: 0x00269758
		protected void DrawPawnList(Rect viewRect, Rect listRect)
		{
			Dialog_BeginRitual.<>c__DisplayClass70_0 CS$<>8__locals1 = new Dialog_BeginRitual.<>c__DisplayClass70_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.viewRect = viewRect;
			try
			{
				Dialog_BeginRitual.<>c__DisplayClass70_2 CS$<>8__locals2 = new Dialog_BeginRitual.<>c__DisplayClass70_2();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				int num = DragAndDropWidget.NewGroup(null);
				this.dragAndDropGroup = ((num == -1) ? this.dragAndDropGroup : num);
				CS$<>8__locals2.maxPawnsPerRow = Mathf.FloorToInt((CS$<>8__locals2.CS$<>8__locals1.viewRect.width - 8f) / 54f);
				CS$<>8__locals2.rowHeight = 0f;
				CS$<>8__locals2.curY = 0f;
				CS$<>8__locals2.curX = 0f;
				string text6;
				foreach (IGrouping<string, RitualRole> grouping in from r in this.assignments.AllRolesForReading
				group r by r.mergeId ?? r.id)
				{
					IGrouping<string, RitualRole> localRoleGroup = grouping;
					RitualRole ritualRole = grouping.First<RitualRole>();
					this.assignments.CandidatesForRole(ritualRole.id, !ritualRole.substitutable, ritualRole.substitutable, true);
					int num2 = 0;
					foreach (RitualRole ritualRole2 in grouping)
					{
						num2 += ritualRole2.maxCount;
					}
					text6 = this.ExtraPawnAssignmentInfo(localRoleGroup, null);
					IEnumerable<RitualRole> source = grouping;
					Func<RitualRole, IEnumerable<Pawn>> selector;
					if ((selector = CS$<>8__locals2.CS$<>8__locals1.<>9__23) == null)
					{
						selector = (CS$<>8__locals2.CS$<>8__locals1.<>9__23 = ((RitualRole r) => CS$<>8__locals2.CS$<>8__locals1.<>4__this.assignments.AssignedPawns(r)));
					}
					IEnumerable<Pawn> enumerable = source.SelectMany(selector);
					string text2 = ritualRole.CategoryLabelCap ?? ritualRole.LabelCap;
					Vector2 mp = Event.current.mousePosition;
					Dialog_BeginRitual.<>c__DisplayClass70_2 CS$<>8__locals4 = CS$<>8__locals2;
					IEnumerable<Pawn> selectedPawns = enumerable;
					string headline = text2;
					int maxPawns = num2;
					Action<Pawn, Vector2> assignAction = delegate(Pawn p, Vector2 dropPos)
					{
						Pawn pawn2 = (Pawn)DragAndDropWidget.DraggableAt(CS$<>8__locals2.CS$<>8__locals1.<>4__this.dragAndDropGroup, mp);
						if (pawn2 != null)
						{
							CS$<>8__locals2.CS$<>8__locals1.<DrawPawnList>g__TryAssignReplace|4(p, localRoleGroup, pawn2, false);
							return;
						}
						CS$<>8__locals2.CS$<>8__locals1.<DrawPawnList>g__TryAssign|3(p, localRoleGroup, true, (Pawn)DragAndDropWidget.GetDraggableAfter(CS$<>8__locals2.CS$<>8__locals1.<>4__this.dragAndDropGroup, dropPos), true, true);
					};
					object dropAreaContext = grouping;
					string extraInfo = text6;
					bool locked;
					if (enumerable.Any<Pawn>())
					{
						IEnumerable<Pawn> source2 = enumerable;
						Func<Pawn, bool> predicate;
						if ((predicate = CS$<>8__locals2.CS$<>8__locals1.<>9__25) == null)
						{
							predicate = (CS$<>8__locals2.CS$<>8__locals1.<>9__25 = ((Pawn p) => CS$<>8__locals2.CS$<>8__locals1.<>4__this.assignments.Required(p)));
						}
						locked = source2.All(predicate);
					}
					else
					{
						locked = false;
					}
					Texture2D warningIcon = Dialog_BeginRitual.WarningIcon;
					Action<Pawn> clickHandler = null;
					Action<Pawn> rightClickHandler;
					if ((rightClickHandler = CS$<>8__locals2.CS$<>8__locals1.<>9__26) == null)
					{
						rightClickHandler = (CS$<>8__locals2.CS$<>8__locals1.<>9__26 = delegate(Pawn p)
						{
							if (CS$<>8__locals2.CS$<>8__locals1.<>4__this.assignments.Required(p))
							{
								return;
							}
							if (!CS$<>8__locals2.CS$<>8__locals1.<>4__this.assignments.TryAssignSpectate(p, null))
							{
								CS$<>8__locals2.CS$<>8__locals1.<>4__this.assignments.RemoveParticipant(p);
							}
							SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						});
					}
					CS$<>8__locals4.<DrawPawnList>g__DrawRoleGroup|7(selectedPawns, headline, maxPawns, assignAction, dropAreaContext, extraInfo, locked, warningIcon, clickHandler, rightClickHandler);
				}
				List<Pawn> spectatorsForReading = this.assignments.SpectatorsForReading;
				List<Pawn> allPawns = this.assignments.AllPawns;
				Dialog_BeginRitual.<>c__DisplayClass70_2 CS$<>8__locals5 = CS$<>8__locals2;
				Precept_Ritual precept_Ritual = this.ritual;
				string text3;
				if (precept_Ritual == null)
				{
					text3 = null;
				}
				else
				{
					RitualBehaviorWorker behavior = precept_Ritual.behavior;
					text3 = ((behavior != null) ? behavior.def.spectatorsLabel : null);
				}
				CS$<>8__locals5.spectatorLabel = (text3 ?? "Spectators".Translate());
				Dialog_BeginRitual.<>c__DisplayClass70_2 CS$<>8__locals6 = CS$<>8__locals2;
				IEnumerable<Pawn> selectedPawns2 = spectatorsForReading;
				string spectatorLabel = CS$<>8__locals2.spectatorLabel;
				int count = allPawns.Count;
				Action<Pawn, Vector2> assignAction2 = delegate(Pawn p, Vector2 dropPos)
				{
					string text6 = CS$<>8__locals2.CS$<>8__locals1.<>4__this.assignments.PawnUnavailableReason(p, null);
					if (text6 != null)
					{
						Messages.Message(text6, LookTargets.Invalid, MessageTypeDefOf.RejectInput, false);
						return;
					}
					CS$<>8__locals2.CS$<>8__locals1.<>4__this.assignments.TryAssignSpectate(p, (Pawn)DragAndDropWidget.GetDraggableAfter(CS$<>8__locals2.CS$<>8__locals1.<>4__this.dragAndDropGroup, dropPos));
					SoundDefOf.DropElement.PlayOneShotOnCamera(null);
				};
				object dropContextSpectator = Dialog_BeginRitual.DropContextSpectator;
				Precept_Ritual precept_Ritual2 = this.ritual;
				string extraInfo2;
				if (precept_Ritual2 == null)
				{
					extraInfo2 = null;
				}
				else
				{
					RitualBehaviorWorker behavior2 = precept_Ritual2.behavior;
					if (behavior2 == null)
					{
						extraInfo2 = null;
					}
					else
					{
						RitualSpectatorFilter spectatorFilter = behavior2.def.spectatorFilter;
						extraInfo2 = ((spectatorFilter != null) ? spectatorFilter.description : null);
					}
				}
				CS$<>8__locals6.<DrawPawnList>g__DrawRoleGroup|7(selectedPawns2, spectatorLabel, count, assignAction2, dropContextSpectator, extraInfo2, false, null, new Action<Pawn>(CS$<>8__locals2.CS$<>8__locals1.<DrawPawnList>g__TryAssignAnyRole|8), delegate(Pawn p)
				{
					if (CS$<>8__locals2.CS$<>8__locals1.<>4__this.assignments.Required(p))
					{
						return;
					}
					CS$<>8__locals2.CS$<>8__locals1.<>4__this.assignments.RemoveParticipant(p);
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				});
				CS$<>8__locals2.<DrawPawnList>g__DrawRoleGroup|7(from p in allPawns
				where !CS$<>8__locals2.CS$<>8__locals1.<>4__this.assignments.PawnParticipating(p)
				select p, "NotParticipating".Translate(), allPawns.Count, delegate(Pawn p, Vector2 dropPos)
				{
					CS$<>8__locals2.CS$<>8__locals1.<>4__this.assignments.RemoveParticipant(p);
					SoundDefOf.DropElement.PlayOneShotOnCamera(null);
				}, Dialog_BeginRitual.DropContextNotParticipating, null, false, null, delegate(Pawn p)
				{
					if (CS$<>8__locals2.CS$<>8__locals1.<>4__this.assignments.PawnUnavailableReason(p, null) != null)
					{
						base.<DrawPawnList>g__TryAssignAnyRole|8(p);
						return;
					}
					CS$<>8__locals2.CS$<>8__locals1.<>4__this.assignments.TryAssignSpectate(p, null);
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}, delegate(Pawn p)
				{
					Dialog_BeginRitual.<>c__DisplayClass70_6 CS$<>8__locals8 = new Dialog_BeginRitual.<>c__DisplayClass70_6();
					CS$<>8__locals8.CS$<>8__locals5 = CS$<>8__locals2;
					CS$<>8__locals8.p = p;
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					RitualRole ritualRole4;
					bool flag2;
					string text6 = CS$<>8__locals2.CS$<>8__locals1.<>4__this.CannotAssignReason(CS$<>8__locals8.p, null, out ritualRole4, out flag2, false);
					Action action2 = (text6 != null) ? null : new Action(delegate()
					{
						CS$<>8__locals8.CS$<>8__locals5.CS$<>8__locals1.<>4__this.assignments.TryAssignSpectate(CS$<>8__locals8.p, null);
					});
					list.Add(new FloatMenuOption(Dialog_BeginRitual.<DrawPawnList>g__PostProcessFloatLabel|70_0(CS$<>8__locals2.spectatorLabel, text6, null), action2, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					foreach (IGrouping<string, RitualRole> grouping3 in CS$<>8__locals2.CS$<>8__locals1.<>4__this.assignments.AllRolesForReading.GroupBy((RitualRole r) => r.mergeId ?? r.id))
					{
						Dialog_BeginRitual.<>c__DisplayClass70_7 CS$<>8__locals9 = new Dialog_BeginRitual.<>c__DisplayClass70_7();
						CS$<>8__locals9.CS$<>8__locals6 = CS$<>8__locals8;
						CS$<>8__locals9.localRoleGroup = grouping3;
						RitualRole ritualRole5 = grouping3.First<RitualRole>();
						text6 = CS$<>8__locals2.CS$<>8__locals1.<>4__this.CannotAssignReason(CS$<>8__locals9.CS$<>8__locals6.p, CS$<>8__locals9.localRoleGroup, out ritualRole4, out CS$<>8__locals9.mustReplace, true);
						Dialog_BeginRitual.<>c__DisplayClass70_7 CS$<>8__locals10 = CS$<>8__locals9;
						Pawn replacing;
						if (!CS$<>8__locals9.mustReplace)
						{
							replacing = null;
						}
						else
						{
							IEnumerable<RitualRole> localRoleGroup = CS$<>8__locals9.localRoleGroup;
							Func<RitualRole, IEnumerable<Pawn>> selector2;
							if ((selector2 = CS$<>8__locals2.CS$<>8__locals1.<>9__32) == null)
							{
								selector2 = (CS$<>8__locals2.CS$<>8__locals1.<>9__32 = ((RitualRole role) => CS$<>8__locals2.CS$<>8__locals1.<>4__this.assignments.AssignedPawns(role)));
							}
							replacing = localRoleGroup.SelectMany(selector2).Last<Pawn>();
						}
						CS$<>8__locals10.replacing = replacing;
						action2 = ((text6 != null) ? null : new Action(delegate()
						{
							if (CS$<>8__locals9.mustReplace)
							{
								CS$<>8__locals9.CS$<>8__locals6.CS$<>8__locals5.CS$<>8__locals1.<DrawPawnList>g__TryAssignReplace|4(CS$<>8__locals9.CS$<>8__locals6.p, CS$<>8__locals9.localRoleGroup, CS$<>8__locals9.replacing, false);
								return;
							}
							CS$<>8__locals9.CS$<>8__locals6.CS$<>8__locals5.CS$<>8__locals1.<DrawPawnList>g__TryAssign|3(CS$<>8__locals9.CS$<>8__locals6.p, CS$<>8__locals9.localRoleGroup, true, null, true, false);
						}));
						list.Add(new FloatMenuOption(Dialog_BeginRitual.<DrawPawnList>g__PostProcessFloatLabel|70_0(ritualRole5.LabelCap, text6, CS$<>8__locals9.replacing), action2, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					Find.WindowStack.Add(new FloatMenu(list));
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				});
				CS$<>8__locals2.curY += CS$<>8__locals2.rowHeight + 4f;
				Text.Font = GameFont.Tiny;
				GUI.color = ColorLibrary.Grey;
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(new Rect(CS$<>8__locals2.CS$<>8__locals1.viewRect.x, CS$<>8__locals2.curY, CS$<>8__locals2.CS$<>8__locals1.viewRect.width, 20f), "DragPawnsToRolesInfo".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				Text.Font = GameFont.Small;
				CS$<>8__locals2.curY += 20f;
				if (Event.current.type == EventType.Layout)
				{
					this.listScrollViewHeight = CS$<>8__locals2.curY;
				}
				foreach (Action action in Dialog_BeginRitual.tmpDelayedGuiCalls)
				{
					action();
				}
				object obj = DragAndDropWidget.CurrentlyDraggedDraggable();
				Pawn pawn = (Pawn)DragAndDropWidget.DraggableAt(this.dragAndDropGroup, Event.current.mousePosition);
				if (obj != null)
				{
					object obj2 = DragAndDropWidget.HoveringDropArea(this.dragAndDropGroup);
					if (obj2 != null)
					{
						Rect? rect = DragAndDropWidget.HoveringDropAreaRect(this.dragAndDropGroup, null);
						if (this.lastHoveredDropArea != null && rect != null && rect != this.lastHoveredDropArea)
						{
							SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
						}
						this.lastHoveredDropArea = rect;
					}
					if (obj2 != null && obj2 != Dialog_BeginRitual.DropContextNotParticipating)
					{
						IGrouping<string, RitualRole> grouping2 = obj2 as IGrouping<string, RitualRole>;
						RitualRole ritualRole3;
						bool flag;
						string text4 = this.CannotAssignReason((Pawn)obj, grouping2, out ritualRole3, out flag, grouping2 != null && pawn != null);
						float num3 = (float)UI.screenHeight - Input.mousePosition.y - 50f;
						string text5 = (ritualRole3 == null) ? null : this.ExtraPawnAssignmentInfo(grouping2, (Pawn)obj);
						if (!string.IsNullOrWhiteSpace(text4) || !string.IsNullOrWhiteSpace(text5))
						{
							string text = string.IsNullOrWhiteSpace(text4) ? text5 : text4;
							Color color = string.IsNullOrWhiteSpace(text4) ? ColorLibrary.Yellow : ColorLibrary.RedReadable;
							Text.Font = GameFont.Small;
							Vector2 vector = Text.CalcSize(text);
							Rect r = new Rect(Input.mousePosition.x - vector.x / 2f, num3, vector.x, vector.y).ExpandedBy(5f);
							num3 -= r.height - 10f;
							Find.WindowStack.ImmediateWindow(47839543, r, WindowLayer.Super, delegate
							{
								Text.Font = GameFont.Small;
								GUI.color = color;
								Widgets.Label(r.AtZero().ContractedBy(5f), text);
								GUI.color = Color.white;
							}, true, false, 1f, null);
						}
					}
				}
			}
			finally
			{
				Dialog_BeginRitual.tmpDelayedGuiCalls.Clear();
			}
			this.pawnsListEdgeScrollDirection = 0;
			if (DragAndDropWidget.CurrentlyDraggedDraggable() != null)
			{
				Rect rect2 = new Rect(CS$<>8__locals1.viewRect.x, this.scrollPositionPawns.y, CS$<>8__locals1.viewRect.width, 30f);
				Rect rect3 = new Rect(CS$<>8__locals1.viewRect.x, this.scrollPositionPawns.y + (listRect.height - 30f), CS$<>8__locals1.viewRect.width, 30f);
				if (Mouse.IsOver(rect2))
				{
					this.pawnsListEdgeScrollDirection = -1;
					return;
				}
				if (Mouse.IsOver(rect3))
				{
					this.pawnsListEdgeScrollDirection = 1;
				}
			}
		}

		// Token: 0x0600736F RID: 29551 RVA: 0x0026BD40 File Offset: 0x00269F40
		protected IEnumerable<string> BlockingIssues()
		{
			if (this.assignments.Participants.Count<Pawn>() == 0)
			{
				yield return "MessageRitualNeedsAtLeastOnePerson".Translate();
			}
			if (this.ritual != null)
			{
				if (this.ritual.behavior.SpectatorsRequired() && this.assignments.SpectatorsForReading.Count == 0)
				{
					yield return "MessageRitualNeedsAtLeastOneSpectator".Translate();
				}
				if (this.ritual.obligationTargetFilter != null)
				{
					foreach (string text in this.ritual.obligationTargetFilter.GetBlockingIssues(this.target, this.assignments.Participants))
					{
						yield return text;
					}
					IEnumerator<string> enumerator = null;
				}
				if (!this.ritual.behavior.def.roles.NullOrEmpty<RitualRole>())
				{
					foreach (IGrouping<string, RitualRole> source in from r in this.ritual.behavior.def.roles
					group r by r.mergeId ?? r.id)
					{
						RitualRole firstRole = source.First<RitualRole>();
						int requiredPawnCount = source.Count((RitualRole r) => r.required);
						if (requiredPawnCount > 0)
						{
							IEnumerable<Pawn> selectedPawns = source.SelectMany((RitualRole r) => this.assignments.AssignedPawns(r));
							foreach (Pawn p in selectedPawns)
							{
								string text2 = this.assignments.PawnUnavailableReason(p, firstRole);
								if (text2 != null)
								{
									yield return text2;
								}
							}
							IEnumerator<Pawn> enumerator3 = null;
							if (requiredPawnCount == 1 && !selectedPawns.Any<Pawn>())
							{
								yield return "MessageRitualNeedsAtLeastOneRolePawn".Translate(firstRole.Label);
							}
							else if (requiredPawnCount > 1 && selectedPawns.Count<Pawn>() < requiredPawnCount)
							{
								yield return "MessageRitualNeedsAtLeastNumRolePawn".Translate(Find.ActiveLanguageWorker.Pluralize(firstRole.Label, -1), requiredPawnCount);
							}
							selectedPawns = null;
						}
						firstRole = null;
					}
					IEnumerator<IGrouping<string, RitualRole>> enumerator2 = null;
					if (!this.assignments.ExtraRequiredPawnsForReading.NullOrEmpty<Pawn>())
					{
						foreach (Pawn pawn in this.assignments.ExtraRequiredPawnsForReading)
						{
							string text3 = this.assignments.PawnUnavailableReason(pawn, this.assignments.RoleForPawn(pawn, true));
							if (text3 != null)
							{
								yield return text3;
							}
						}
						List<Pawn>.Enumerator enumerator4 = default(List<Pawn>.Enumerator);
					}
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06007372 RID: 29554 RVA: 0x0026BE32 File Offset: 0x0026A032
		[CompilerGenerated]
		private void <DoWindowContents>g__Start|52_1()
		{
			Dialog_BeginRitual.ActionCallback actionCallback = this.action;
			if (actionCallback != null && actionCallback(this.assignments))
			{
				this.Close(true);
			}
		}

		// Token: 0x06007373 RID: 29555 RVA: 0x0026BE58 File Offset: 0x0026A058
		[CompilerGenerated]
		internal static void <DrawQualityFactors>g__DrawQualityFactor|54_0(ExpectedOutcomeDesc expectedOutcomeDesc, ref float y, ref Dialog_BeginRitual.<>c__DisplayClass54_0 A_2)
		{
			Rect rect = new Rect(A_2.viewRect.x, y, A_2.viewRect.width, 25f);
			Rect rect2 = new Rect
			{
				x = A_2.viewRect.x,
				width = A_2.viewRect.width + 10f,
				y = y - 3f,
				height = 28f
			};
			if (A_2.even)
			{
				Widgets.DrawLightHighlight(rect2);
			}
			GUI.color = (expectedOutcomeDesc.uncertainOutcome ? ColorLibrary.Yellow : (expectedOutcomeDesc.positive ? ColorLibrary.Green : ColorLibrary.RedReadable));
			Widgets.Label(rect, "  " + expectedOutcomeDesc.label);
			Text.Anchor = TextAnchor.UpperRight;
			Widgets.Label(rect, expectedOutcomeDesc.effect);
			Text.Anchor = TextAnchor.UpperLeft;
			if (!expectedOutcomeDesc.noMiddleColumnInfo)
			{
				if (!expectedOutcomeDesc.count.NullOrEmpty())
				{
					float x = Text.CalcSize(expectedOutcomeDesc.count).x;
					Rect rect3 = new Rect(rect);
					rect3.xMin += 220f - x / 2f;
					rect3.width = x;
					Widgets.Label(rect3, expectedOutcomeDesc.count);
				}
				else
				{
					GUI.color = Color.white;
					Texture2D image = expectedOutcomeDesc.present ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex;
					if (expectedOutcomeDesc.uncertainOutcome)
					{
						image = Dialog_BeginRitual.questionMark;
					}
					Rect rect4 = new Rect(rect);
					rect4.x += 208f;
					rect4.y -= 1f;
					rect4.width = 24f;
					rect4.height = 24f;
					if (!expectedOutcomeDesc.present)
					{
						if (expectedOutcomeDesc.uncertainOutcome)
						{
							TooltipHandler.TipRegion(rect4, () => "QualityFactorTooltipUncertain".Translate(), 238934347);
						}
						else
						{
							TooltipHandler.TipRegion(rect4, () => "QualityFactorTooltipNotFulfilled".Translate(), 238934347);
						}
					}
					GUI.DrawTexture(rect4, image);
				}
			}
			GUI.color = Color.white;
			if (expectedOutcomeDesc.tip != null && Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect2);
				TooltipHandler.TipRegion(rect, () => expectedOutcomeDesc.tip, 976091152);
			}
			y += 28f;
			A_2.totalInfoHeight += 28f;
		}

		// Token: 0x06007374 RID: 29556 RVA: 0x0026C12C File Offset: 0x0026A32C
		[CompilerGenerated]
		internal static string <DrawPawnList>g__PostProcessFloatLabel|70_0(string label, string unavailableReason, Pawn replacing)
		{
			string text = label;
			if (unavailableReason != null)
			{
				text += " (" + "DisabledLower".Translate().CapitalizeFirst() + ": " + unavailableReason + ")";
			}
			if (replacing != null)
			{
				text += " (" + "RitualRoleReplaces".Translate(replacing.Named("PAWN")) + ")";
			}
			return "AssignToRole".Translate(text);
		}

		// Token: 0x04003EF9 RID: 16121
		private Precept_Ritual ritual;

		// Token: 0x04003EFA RID: 16122
		private TargetInfo target;

		// Token: 0x04003EFB RID: 16123
		private RitualObligation obligation;

		// Token: 0x04003EFC RID: 16124
		private RitualOutcomeEffectDef outcome;

		// Token: 0x04003EFD RID: 16125
		private string ritualExplanation;

		// Token: 0x04003EFE RID: 16126
		private List<string> extraInfos;

		// Token: 0x04003EFF RID: 16127
		protected Dialog_BeginRitual.ActionCallback action;

		// Token: 0x04003F00 RID: 16128
		protected Func<Pawn, bool, bool, bool> filter;

		// Token: 0x04003F01 RID: 16129
		protected Map map;

		// Token: 0x04003F02 RID: 16130
		protected string ritualLabel;

		// Token: 0x04003F03 RID: 16131
		protected string headerText;

		// Token: 0x04003F04 RID: 16132
		protected string confirmText;

		// Token: 0x04003F05 RID: 16133
		protected Vector2 scrollPositionPawns;

		// Token: 0x04003F06 RID: 16134
		protected float listScrollViewHeight;

		// Token: 0x04003F07 RID: 16135
		protected Pawn organizer;

		// Token: 0x04003F08 RID: 16136
		protected Pawn selectedPawn;

		// Token: 0x04003F09 RID: 16137
		private RitualRoleAssignments assignments;

		// Token: 0x04003F0A RID: 16138
		private int pawnsListEdgeScrollDirection;

		// Token: 0x04003F0B RID: 16139
		private static Texture2D slaveryIcon = ContentFinder<Texture2D>.Get("UI/Icons/Slavery", true);

		// Token: 0x04003F0C RID: 16140
		private static Texture2D prisonerIcon = ContentFinder<Texture2D>.Get("UI/Icons/Prisoner", true);

		// Token: 0x04003F0D RID: 16141
		private static Texture2D sleepingIcon = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Sleeping", true);

		// Token: 0x04003F0E RID: 16142
		private static readonly Texture2D questionMark = ContentFinder<Texture2D>.Get("UI/Overlays/QuestionMark", true);

		// Token: 0x04003F0F RID: 16143
		private static readonly Texture2D ErrorIcon = Resources.Load<Texture2D>("Textures/UI/Widgets/Error");

		// Token: 0x04003F10 RID: 16144
		private static readonly Texture2D WarningIcon = Resources.Load<Texture2D>("Textures/UI/Widgets/Warning");

		// Token: 0x04003F11 RID: 16145
		private static readonly Texture2D InfoIcon = Resources.Load<Texture2D>("Textures/UI/Widgets/Info");

		// Token: 0x04003F12 RID: 16146
		private string sleepingMessage;

		// Token: 0x04003F13 RID: 16147
		protected const float CategoryCaptionHeight = 32f;

		// Token: 0x04003F14 RID: 16148
		protected const float EntryHeight = 28f;

		// Token: 0x04003F15 RID: 16149
		protected const float ListWidth = 320f;

		// Token: 0x04003F16 RID: 16150
		protected const float TargetIconSize = 32f;

		// Token: 0x04003F17 RID: 16151
		protected const float QualityOffsetListWidth = 402f;

		// Token: 0x04003F18 RID: 16152
		private const int HeadlineIconSize = 20;

		// Token: 0x04003F19 RID: 16153
		private const int PawnPortraitHeightTotal = 70;

		// Token: 0x04003F1A RID: 16154
		private const int PawnPortraitHeight = 50;

		// Token: 0x04003F1B RID: 16155
		private const int PawnPortraitWidth = 50;

		// Token: 0x04003F1C RID: 16156
		private const int PawnPortraitLabelHeight = 20;

		// Token: 0x04003F1D RID: 16157
		private const int PawnPortraitMargin = 4;

		// Token: 0x04003F1E RID: 16158
		private const int PawnsListPadding = 4;

		// Token: 0x04003F1F RID: 16159
		private const int PawnsListHorizontalGap = 26;

		// Token: 0x04003F20 RID: 16160
		private const int PawnPortraitIconSize = 20;

		// Token: 0x04003F21 RID: 16161
		private const int EdgeScrollSpeedWhileDragging = 1000;

		// Token: 0x04003F22 RID: 16162
		private List<ExpectedOutcomeDesc> expectedOutcomeEffects = new List<ExpectedOutcomeDesc>();

		// Token: 0x04003F23 RID: 16163
		private static List<Dialog_BeginRitual.PawnPortraitIcon> tmpPortraitIcons = new List<Dialog_BeginRitual.PawnPortraitIcon>();

		// Token: 0x04003F24 RID: 16164
		private static List<Action> tmpDelayedGuiCalls = new List<Action>();

		// Token: 0x04003F25 RID: 16165
		private int dragAndDropGroup;

		// Token: 0x04003F26 RID: 16166
		private Rect? lastHoveredDropArea;

		// Token: 0x04003F27 RID: 16167
		private static List<Pawn> tmpSelectedPawns = new List<Pawn>();

		// Token: 0x04003F28 RID: 16168
		private static List<Pawn> tmpAssignedPawns = new List<Pawn>();

		// Token: 0x04003F29 RID: 16169
		private static readonly object DropContextSpectator = new object();

		// Token: 0x04003F2A RID: 16170
		private static readonly object DropContextNotParticipating = new object();

		// Token: 0x0200262A RID: 9770
		// (Invoke) Token: 0x0600D569 RID: 54633
		public delegate bool ActionCallback(RitualRoleAssignments assignments);

		// Token: 0x0200262B RID: 9771
		private struct PawnPortraitIcon
		{
			// Token: 0x0400916D RID: 37229
			public Color color;

			// Token: 0x0400916E RID: 37230
			public Texture2D icon;

			// Token: 0x0400916F RID: 37231
			public string tooltip;
		}
	}
}
