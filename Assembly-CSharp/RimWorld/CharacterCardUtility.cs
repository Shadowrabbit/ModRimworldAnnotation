using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001A21 RID: 6689
	[StaticConstructorOnStartup]
	public static class CharacterCardUtility
	{
		// Token: 0x060093B4 RID: 37812 RVA: 0x002A90C0 File Offset: 0x002A72C0
		public static void DrawCharacterCard(Rect rect, Pawn pawn, Action randomizeCallback = null, Rect creationRect = default(Rect))
		{
			CharacterCardUtility.<>c__DisplayClass14_0 CS$<>8__locals1 = new CharacterCardUtility.<>c__DisplayClass14_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.creationMode = (randomizeCallback != null);
			GUI.BeginGroup(CS$<>8__locals1.creationMode ? creationRect : rect);
			CharacterCardUtility.<>c__DisplayClass14_1 CS$<>8__locals2 = new CharacterCardUtility.<>c__DisplayClass14_1();
			CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
			Rect rect2 = new Rect(0f, 0f, 300f, 30f);
			NameTriple nameTriple = CS$<>8__locals2.CS$<>8__locals1.pawn.Name as NameTriple;
			if (CS$<>8__locals2.CS$<>8__locals1.creationMode && nameTriple != null)
			{
				Rect rect3 = new Rect(rect2);
				rect3.width *= 0.333f;
				Rect rect4 = new Rect(rect2);
				rect4.width *= 0.333f;
				rect4.x += rect4.width;
				Rect rect5 = new Rect(rect2);
				rect5.width *= 0.333f;
				rect5.x += rect4.width * 2f;
				string first = nameTriple.First;
				string nick = nameTriple.Nick;
				string last = nameTriple.Last;
				CharacterCardUtility.DoNameInputRect(rect3, ref first, 12);
				if (nameTriple.Nick == nameTriple.First || nameTriple.Nick == nameTriple.Last)
				{
					GUI.color = new Color(1f, 1f, 1f, 0.5f);
				}
				CharacterCardUtility.DoNameInputRect(rect4, ref nick, 16);
				GUI.color = Color.white;
				CharacterCardUtility.DoNameInputRect(rect5, ref last, 12);
				if (nameTriple.First != first || nameTriple.Nick != nick || nameTriple.Last != last)
				{
					CS$<>8__locals2.CS$<>8__locals1.pawn.Name = new NameTriple(first, string.IsNullOrEmpty(nick) ? first : nick, last);
				}
				TooltipHandler.TipRegionByKey(rect3, "FirstNameDesc");
				TooltipHandler.TipRegionByKey(rect4, "ShortIdentifierDesc");
				TooltipHandler.TipRegionByKey(rect5, "LastNameDesc");
			}
			else
			{
				rect2.width = 999f;
				Text.Font = GameFont.Medium;
				Widgets.Label(rect2, CS$<>8__locals2.CS$<>8__locals1.pawn.Name.ToStringFull);
				Text.Font = GameFont.Small;
			}
			if (randomizeCallback != null)
			{
				Rect rect6 = new Rect(creationRect.width - 24f - 100f, 0f, 100f, rect2.height);
				if (Widgets.ButtonText(rect6, "Randomize".Translate(), true, true, true))
				{
					SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
					randomizeCallback();
				}
				UIHighlighter.HighlightOpportunity(rect6, "RandomizePawn");
			}
			if (CS$<>8__locals2.CS$<>8__locals1.creationMode)
			{
				Widgets.InfoCardButton(creationRect.width - 24f, 0f, CS$<>8__locals2.CS$<>8__locals1.pawn);
			}
			else if (!CS$<>8__locals2.CS$<>8__locals1.pawn.health.Dead)
			{
				float num = CharacterCardUtility.PawnCardSize(CS$<>8__locals2.CS$<>8__locals1.pawn).x - 85f;
				if (CS$<>8__locals2.CS$<>8__locals1.pawn.IsFreeColonist && CS$<>8__locals2.CS$<>8__locals1.pawn.Spawned && !CS$<>8__locals2.CS$<>8__locals1.pawn.IsQuestLodger())
				{
					Rect rect7 = new Rect(num, 0f, 30f, 30f);
					if (Mouse.IsOver(rect7))
					{
						TooltipHandler.TipRegion(rect7, PawnBanishUtility.GetBanishButtonTip(CS$<>8__locals2.CS$<>8__locals1.pawn));
					}
					if (Widgets.ButtonImage(rect7, TexButton.Banish, true))
					{
						if (CS$<>8__locals2.CS$<>8__locals1.pawn.Downed)
						{
							Messages.Message("MessageCantBanishDownedPawn".Translate(CS$<>8__locals2.CS$<>8__locals1.pawn.LabelShort, CS$<>8__locals2.CS$<>8__locals1.pawn).AdjustedFor(CS$<>8__locals2.CS$<>8__locals1.pawn, "PAWN", true), CS$<>8__locals2.CS$<>8__locals1.pawn, MessageTypeDefOf.RejectInput, false);
						}
						else
						{
							PawnBanishUtility.ShowBanishPawnConfirmationDialog(CS$<>8__locals2.CS$<>8__locals1.pawn);
						}
					}
					num -= 40f;
				}
				if (CS$<>8__locals2.CS$<>8__locals1.pawn.IsColonist)
				{
					Rect rect8 = new Rect(num, 0f, 30f, 30f);
					TooltipHandler.TipRegionByKey(rect8, "RenameColonist");
					if (Widgets.ButtonImage(rect8, TexButton.Rename, true))
					{
						Find.WindowStack.Add(new Dialog_NamePawn(CS$<>8__locals2.CS$<>8__locals1.pawn));
					}
					num -= 40f;
				}
				if (CS$<>8__locals2.CS$<>8__locals1.pawn.IsFreeColonist && !CS$<>8__locals2.CS$<>8__locals1.pawn.IsQuestLodger() && CS$<>8__locals2.CS$<>8__locals1.pawn.royalty != null && CS$<>8__locals2.CS$<>8__locals1.pawn.royalty.AllTitlesForReading.Count > 0)
				{
					Rect rect9 = new Rect(num, 0f, 30f, 30f);
					TooltipHandler.TipRegionByKey(rect9, "RenounceTitle");
					if (Widgets.ButtonImage(rect9, TexButton.RenounceTitle, true))
					{
						FloatMenuUtility.MakeMenu<RoyalTitle>(CS$<>8__locals2.CS$<>8__locals1.pawn.royalty.AllTitlesForReading, (RoyalTitle title) => "RenounceTitle".Translate() + ": " + "TitleOfFaction".Translate(title.def.GetLabelCapFor(CS$<>8__locals2.CS$<>8__locals1.pawn), title.faction.GetCallLabel()), delegate(RoyalTitle title)
						{
							Action <>9__6;
							return delegate()
							{
								List<RoyalTitlePermitDef> list2;
								List<RoyalTitlePermitDef> list3;
								RoyalTitleUtility.FindLostAndGainedPermits(title.def, null, out list2, out list3);
								StringBuilder stringBuilder = new StringBuilder();
								if (list3.Count > 0)
								{
									stringBuilder.AppendLine("RenounceTitleWillLoosePermits".Translate(CS$<>8__locals2.CS$<>8__locals1.pawn.Named("PAWN")) + ":");
									foreach (RoyalTitlePermitDef royalTitlePermitDef in list3)
									{
										stringBuilder.AppendLine("- " + royalTitlePermitDef.LabelCap + " (" + base.<DrawCharacterCard>g__FirstTitleWithPermit|5(royalTitlePermitDef).GetLabelFor(CS$<>8__locals2.CS$<>8__locals1.pawn) + ")");
									}
									stringBuilder.AppendLine();
								}
								if (!title.faction.def.renounceTitleMessage.NullOrEmpty())
								{
									stringBuilder.AppendLine(title.faction.def.renounceTitleMessage);
								}
								WindowStack windowStack = Find.WindowStack;
								TaggedString text2 = "RenounceTitleDescription".Translate(CS$<>8__locals2.CS$<>8__locals1.pawn.Named("PAWN"), "TitleOfFaction".Translate(title.def.GetLabelCapFor(CS$<>8__locals2.CS$<>8__locals1.pawn), title.faction.GetCallLabel()).Named("TITLE"), stringBuilder.ToString().TrimEndNewlines().Named("EFFECTS"));
								Action confirmedAct;
								if ((confirmedAct = <>9__6) == null)
								{
									confirmedAct = (<>9__6 = delegate()
									{
										CS$<>8__locals2.CS$<>8__locals1.pawn.royalty.SetTitle(title.faction, null, false, false, true);
										CS$<>8__locals2.CS$<>8__locals1.pawn.royalty.ResetPermitsAndPoints(title.faction, title.def);
									});
								}
								windowStack.Add(Dialog_MessageBox.CreateConfirmation(text2, confirmedAct, true, null));
							};
						});
					}
					num -= 40f;
				}
			}
			CS$<>8__locals2.stackElements = new List<GenUI.AnonymousStackElement>();
			Text.Font = GameFont.Small;
			string text = CS$<>8__locals2.CS$<>8__locals1.pawn.MainDesc(false);
			Vector2 vector = Text.CalcSize(text);
			Rect rect10 = new Rect(0f, 45f, vector.x + 5f, 24f);
			Widgets.Label(rect10, text);
			float height = Text.CalcHeight(text, rect10.width);
			Rect rect11 = new Rect(rect10.x, rect10.y, rect10.width, height);
			if (Mouse.IsOver(rect11))
			{
				TooltipHandler.TipRegion(rect11, () => CS$<>8__locals2.CS$<>8__locals1.pawn.ageTracker.AgeTooltipString, 6873641);
			}
			float num2 = 0f;
			if (CS$<>8__locals2.CS$<>8__locals1.pawn.Faction != null && !CS$<>8__locals2.CS$<>8__locals1.pawn.Faction.Hidden)
			{
				float num3 = Text.CalcSize(CS$<>8__locals2.CS$<>8__locals1.pawn.Faction.Name).x + 22f + 15f;
				CS$<>8__locals2.stackElements.Add(new GenUI.AnonymousStackElement
				{
					drawer = delegate(Rect r)
					{
						Rect rect12 = new Rect(r.x, r.y, r.width, r.height);
						Color color = GUI.color;
						GUI.color = CharacterCardUtility.StackElementBackground;
						GUI.DrawTexture(rect12, BaseContent.WhiteTex);
						GUI.color = color;
						Widgets.DrawHighlightIfMouseover(rect12);
						Rect rect13 = new Rect(r.x, r.y, r.width, r.height);
						Rect position2 = new Rect(r.x + 1f, r.y + 1f, 20f, 20f);
						GUI.color = CS$<>8__locals2.CS$<>8__locals1.pawn.Faction.Color;
						GUI.DrawTexture(position2, CS$<>8__locals2.CS$<>8__locals1.pawn.Faction.def.FactionIcon);
						GUI.color = color;
						Widgets.Label(new Rect(rect13.x + rect13.height + 5f, rect13.y, rect13.width - 10f, rect13.height), CS$<>8__locals2.CS$<>8__locals1.pawn.Faction.Name);
						if (Widgets.ButtonInvisible(rect12, true))
						{
							if (CS$<>8__locals2.CS$<>8__locals1.creationMode)
							{
								Find.WindowStack.Add(new Dialog_FactionDuringLanding());
							}
							else
							{
								Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Factions, true);
							}
						}
						if (Mouse.IsOver(rect12))
						{
							TaggedString taggedString = "Faction".Translate() + "\n\n" + "FactionDesc".Translate(CS$<>8__locals2.CS$<>8__locals1.pawn.Named("PAWN")) + "\n\n" + "ClickToViewFactions".Translate();
							TipSignal tip = new TipSignal(taggedString, CS$<>8__locals2.CS$<>8__locals1.pawn.Faction.loadID * 37);
							TooltipHandler.TipRegion(rect12, tip);
						}
					},
					width = num3
				});
				num2 += num3;
			}
			bool flag = false;
			float num4 = rect.width - vector.x - 10f;
			CharacterCardUtility.tmpExtraFactions.Clear();
			QuestUtility.GetExtraFactionsFromQuestParts(CS$<>8__locals2.CS$<>8__locals1.pawn, CharacterCardUtility.tmpExtraFactions, null);
			foreach (ExtraFaction extraFaction in CharacterCardUtility.tmpExtraFactions)
			{
				CharacterCardUtility.<>c__DisplayClass14_4 CS$<>8__locals3 = new CharacterCardUtility.<>c__DisplayClass14_4();
				CS$<>8__locals3.CS$<>8__locals3 = CS$<>8__locals2;
				if (CS$<>8__locals3.CS$<>8__locals3.CS$<>8__locals1.pawn.Faction != extraFaction.faction)
				{
					CS$<>8__locals3.localExtraFaction = extraFaction;
					CS$<>8__locals3.factionName = CS$<>8__locals3.localExtraFaction.faction.Name;
					CS$<>8__locals3.drawExtraFactionIcon = (CS$<>8__locals3.localExtraFaction.factionType == ExtraFactionType.HomeFaction || CS$<>8__locals3.localExtraFaction.factionType == ExtraFactionType.MiniFaction);
					float num5 = CS$<>8__locals3.<DrawCharacterCard>g__ElementWidth|12();
					if (flag || num2 + num5 >= num4)
					{
						CS$<>8__locals3.factionName = "...";
						num5 = CS$<>8__locals3.<DrawCharacterCard>g__ElementWidth|12();
						flag = true;
					}
					CS$<>8__locals3.CS$<>8__locals3.stackElements.Add(new GenUI.AnonymousStackElement
					{
						drawer = delegate(Rect r)
						{
							Rect rect12 = new Rect(r.x, r.y, r.width, r.height);
							Rect rect13 = CS$<>8__locals3.drawExtraFactionIcon ? rect12 : r;
							Color color = GUI.color;
							GUI.color = CharacterCardUtility.StackElementBackground;
							GUI.DrawTexture(rect13, BaseContent.WhiteTex);
							GUI.color = color;
							Widgets.DrawHighlightIfMouseover(rect13);
							if (CS$<>8__locals3.drawExtraFactionIcon)
							{
								Rect rect14 = new Rect(r.x, r.y, r.width, r.height);
								Rect position2 = new Rect(r.x + 1f, r.y + 1f, 20f, 20f);
								GUI.color = CS$<>8__locals3.localExtraFaction.faction.Color;
								GUI.DrawTexture(position2, CS$<>8__locals3.localExtraFaction.faction.def.FactionIcon);
								GUI.color = color;
								Widgets.Label(new Rect(rect14.x + rect14.height + 5f, rect14.y, rect14.width - 10f, rect14.height), CS$<>8__locals3.factionName);
							}
							else
							{
								Widgets.Label(new Rect(r.x + 5f, r.y, r.width - 10f, r.height), CS$<>8__locals3.factionName);
							}
							if (Widgets.ButtonInvisible(rect12, true))
							{
								Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Factions, true);
							}
							if (Mouse.IsOver(rect13))
							{
								TaggedString taggedString = CS$<>8__locals3.localExtraFaction.factionType.GetLabel().CapitalizeFirst() + "\n\n" + "ExtraFactionDesc".Translate(CS$<>8__locals3.CS$<>8__locals3.CS$<>8__locals1.pawn.Named("PAWN")) + "\n\n" + "ClickToViewFactions".Translate();
								TipSignal tip = new TipSignal(taggedString, CS$<>8__locals3.localExtraFaction.faction.loadID ^ 1938473043);
								TooltipHandler.TipRegion(rect13, tip);
							}
						},
						width = num5
					});
					num2 += num5;
				}
			}
			GenUI.DrawElementStack<GenUI.AnonymousStackElement>(new Rect(vector.x + 10f, 45f, 999f, 24f), 22f, CS$<>8__locals2.stackElements, delegate(Rect r, GenUI.AnonymousStackElement obj)
			{
				obj.drawer(r);
			}, (GenUI.AnonymousStackElement obj) => obj.width, 4f, 5f, false);
			CS$<>8__locals2.stackElements.Clear();
			CS$<>8__locals2.curY = 72f;
			if (CS$<>8__locals2.CS$<>8__locals1.pawn.royalty != null && CS$<>8__locals2.CS$<>8__locals1.pawn.royalty.AllTitlesForReading.Count > 0)
			{
				using (List<RoyalTitle>.Enumerator enumerator2 = CS$<>8__locals2.CS$<>8__locals1.pawn.royalty.AllTitlesForReading.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						CharacterCardUtility.<>c__DisplayClass14_5 CS$<>8__locals4 = new CharacterCardUtility.<>c__DisplayClass14_5();
						CS$<>8__locals4.CS$<>8__locals4 = CS$<>8__locals2;
						CS$<>8__locals4.title = enumerator2.Current;
						RoyalTitle localTitle = CS$<>8__locals4.title;
						string titleLabel = string.Concat(new object[]
						{
							localTitle.def.GetLabelCapFor(CS$<>8__locals4.CS$<>8__locals4.CS$<>8__locals1.pawn),
							" (",
							CS$<>8__locals4.CS$<>8__locals4.CS$<>8__locals1.pawn.royalty.GetFavor(localTitle.faction),
							")"
						});
						CS$<>8__locals4.CS$<>8__locals4.stackElements.Add(new GenUI.AnonymousStackElement
						{
							drawer = delegate(Rect r)
							{
								Color color = GUI.color;
								Rect rect12 = new Rect(r.x, r.y, r.width + 22f, r.height);
								GUI.color = CharacterCardUtility.StackElementBackground;
								GUI.DrawTexture(rect12, BaseContent.WhiteTex);
								GUI.color = color;
								int favor = CS$<>8__locals4.CS$<>8__locals4.CS$<>8__locals1.pawn.royalty.GetFavor(localTitle.faction);
								if (Mouse.IsOver(rect12))
								{
									Widgets.DrawHighlight(rect12);
								}
								Rect rect13 = new Rect(r.x, r.y, r.width + 22f, r.height);
								Rect position2 = new Rect(r.x + 1f, r.y + 1f, 20f, 20f);
								GUI.color = CS$<>8__locals4.title.faction.Color;
								GUI.DrawTexture(position2, localTitle.faction.def.FactionIcon);
								GUI.color = color;
								Widgets.Label(new Rect(rect13.x + rect13.height + 5f, rect13.y, rect13.width - 10f, rect13.height), titleLabel);
								if (Widgets.ButtonInvisible(rect12, true))
								{
									Find.WindowStack.Add(new Dialog_InfoCard(localTitle.def, localTitle.faction));
								}
								if (Mouse.IsOver(rect12))
								{
									TipSignal tip = new TipSignal(() => CharacterCardUtility.GetTitleTipString(CS$<>8__locals4.CS$<>8__locals4.CS$<>8__locals1.pawn, localTitle.faction, localTitle, favor), (int)CS$<>8__locals4.CS$<>8__locals4.curY * 37);
									TooltipHandler.TipRegion(rect12, tip);
								}
							},
							width = Text.CalcSize(titleLabel).x + 15f
						});
					}
				}
			}
			int num6;
			QuestUtility.AppendInspectStringsFromQuestParts(delegate(string str, Quest quest)
			{
				CS$<>8__locals2.stackElements.Add(new GenUI.AnonymousStackElement
				{
					drawer = delegate(Rect r)
					{
						Color color = GUI.color;
						GUI.color = CharacterCardUtility.StackElementBackground;
						GUI.DrawTexture(r, BaseContent.WhiteTex);
						GUI.color = color;
						CharacterCardUtility.DoQuestLine(r, str, quest);
					},
					width = CharacterCardUtility.GetQuestLineSize(str, quest).x
				});
			}, CS$<>8__locals2.CS$<>8__locals1.pawn, out num6);
			CS$<>8__locals2.curY += GenUI.DrawElementStack<GenUI.AnonymousStackElement>(new Rect(0f, CS$<>8__locals2.curY, rect.width - 5f, 50f), 22f, CS$<>8__locals2.stackElements, delegate(Rect r, GenUI.AnonymousStackElement obj)
			{
				obj.drawer(r);
			}, (GenUI.AnonymousStackElement obj) => obj.width, 4f, 5f, true).height;
			if (CS$<>8__locals2.stackElements.Any<GenUI.AnonymousStackElement>())
			{
				CS$<>8__locals2.curY += 10f;
			}
			CS$<>8__locals2.leftRect = new Rect(0f, CS$<>8__locals2.curY, 250f, 355f);
			Rect position = new Rect(CS$<>8__locals2.leftRect.xMax, CS$<>8__locals2.curY, 258f, 355f);
			GUI.BeginGroup(CS$<>8__locals2.leftRect);
			CS$<>8__locals2.curY = 0f;
			Pawn pawnLocal = CS$<>8__locals2.CS$<>8__locals1.pawn;
			List<Ability> abilities = (from a in CS$<>8__locals2.CS$<>8__locals1.pawn.abilities.abilities
			orderby a.def.level, a.def.EntropyGain
			select a).ToList<Ability>();
			int numSections = abilities.Any<Ability>() ? 5 : 4;
			float num7 = (float)Enum.GetValues(typeof(BackstorySlot)).Length * 22f;
			if (CS$<>8__locals2.CS$<>8__locals1.pawn.story != null && CS$<>8__locals2.CS$<>8__locals1.pawn.story.title != null)
			{
				num7 += 22f;
			}
			List<CharacterCardUtility.LeftRectSection> list = new List<CharacterCardUtility.LeftRectSection>();
			list.Add(new CharacterCardUtility.LeftRectSection
			{
				rect = new Rect(0f, 0f, CS$<>8__locals2.leftRect.width, num7),
				drawer = delegate(Rect sectionRect)
				{
					float num12 = sectionRect.y;
					Text.Font = GameFont.Small;
					foreach (object obj in Enum.GetValues(typeof(BackstorySlot)))
					{
						BackstorySlot backstorySlot = (BackstorySlot)obj;
						Backstory backstory = CS$<>8__locals2.CS$<>8__locals1.pawn.story.GetBackstory(backstorySlot);
						if (backstory != null)
						{
							Rect rect12 = new Rect(sectionRect.x, num12, CS$<>8__locals2.leftRect.width, 22f);
							if (Mouse.IsOver(rect12))
							{
								Widgets.DrawHighlight(rect12);
							}
							if (Mouse.IsOver(rect12))
							{
								TooltipHandler.TipRegion(rect12, backstory.FullDescriptionFor(CS$<>8__locals2.CS$<>8__locals1.pawn).Resolve());
							}
							Text.Anchor = TextAnchor.MiddleLeft;
							string str = (backstorySlot == BackstorySlot.Adulthood) ? "Adulthood".Translate() : "Childhood".Translate();
							Widgets.Label(rect12, str + ":");
							Text.Anchor = TextAnchor.UpperLeft;
							Rect rect13 = new Rect(rect12);
							rect13.x += 90f;
							rect13.width -= 90f;
							string str2 = backstory.TitleCapFor(CS$<>8__locals2.CS$<>8__locals1.pawn.gender);
							Widgets.Label(rect13, str2.Truncate(rect13.width, null));
							num12 += rect12.height;
						}
					}
					if (CS$<>8__locals2.CS$<>8__locals1.pawn.story != null && CS$<>8__locals2.CS$<>8__locals1.pawn.story.title != null)
					{
						Rect rect14 = new Rect(sectionRect.x, num12, CS$<>8__locals2.leftRect.width, 22f);
						Text.Anchor = TextAnchor.MiddleLeft;
						Widgets.Label(rect14, "Current".Translate() + ":");
						Text.Anchor = TextAnchor.UpperLeft;
						Rect rect15 = new Rect(rect14);
						rect15.x += 90f;
						rect15.width -= 90f;
						Widgets.Label(rect15, CS$<>8__locals2.CS$<>8__locals1.pawn.story.title);
						num12 += rect14.height;
					}
				}
			});
			num7 = 30f;
			WorkTags disabledTags = CS$<>8__locals2.CS$<>8__locals1.pawn.CombinedDisabledWorkTags;
			List<WorkTags> disabledTagsList = CharacterCardUtility.WorkTagsFrom(disabledTags).ToList<WorkTags>();
			bool allowWorkTagVerticalLayout = false;
			GenUI.StackElementWidthGetter<WorkTags> workTagWidthGetter = (WorkTags tag) => Text.CalcSize(tag.LabelTranslated().CapitalizeFirst()).x + 10f;
			if (disabledTags == WorkTags.None)
			{
				num7 += 22f;
			}
			else
			{
				disabledTagsList.Sort(delegate(WorkTags a, WorkTags b)
				{
					int num12 = CharacterCardUtility.GetWorkTypeDisableCauses(CS$<>8__locals2.CS$<>8__locals1.pawn, a).Any((object c) => c is RoyalTitleDef) ? 1 : -1;
					int value2 = CharacterCardUtility.GetWorkTypeDisableCauses(CS$<>8__locals2.CS$<>8__locals1.pawn, b).Any((object c) => c is RoyalTitleDef) ? 1 : -1;
					return num12.CompareTo(value2);
				});
				num7 += GenUI.DrawElementStack<WorkTags>(new Rect(0f, 0f, CS$<>8__locals2.leftRect.width - 5f, CS$<>8__locals2.leftRect.height), 22f, disabledTagsList, delegate(Rect r, WorkTags tag)
				{
				}, workTagWidthGetter, 4f, 5f, false).height;
				num7 += 12f;
				allowWorkTagVerticalLayout = (GenUI.DrawElementStackVertical<WorkTags>(new Rect(0f, 0f, rect.width, CS$<>8__locals2.leftRect.height / (float)numSections), 22f, disabledTagsList, delegate(Rect r, WorkTags tag)
				{
				}, workTagWidthGetter, 5f).width <= CS$<>8__locals2.leftRect.width);
			}
			list.Add(new CharacterCardUtility.LeftRectSection
			{
				rect = new Rect(0f, 0f, CS$<>8__locals2.leftRect.width, num7),
				drawer = delegate(Rect sectionRect)
				{
					Text.Font = GameFont.Medium;
					float currentY = sectionRect.y;
					Widgets.Label(new Rect(sectionRect.x, currentY, 200f, 30f), "IncapableOf".Translate(CS$<>8__locals2.CS$<>8__locals1.pawn));
					currentY += 30f;
					Text.Font = GameFont.Small;
					if (disabledTags == WorkTags.None)
					{
						GUI.color = Color.gray;
						Rect rect12 = new Rect(sectionRect.x, currentY, CS$<>8__locals2.leftRect.width, 24f);
						if (Mouse.IsOver(rect12))
						{
							Widgets.DrawHighlight(rect12);
						}
						Widgets.Label(rect12, "None".Translate());
						TooltipHandler.TipRegionByKey(rect12, "None");
					}
					else
					{
						GenUI.StackElementDrawer<WorkTags> drawer = delegate(Rect r, WorkTags tag)
						{
							Color color = GUI.color;
							GUI.color = CharacterCardUtility.StackElementBackground;
							GUI.DrawTexture(r, BaseContent.WhiteTex);
							GUI.color = color;
							GUI.color = CharacterCardUtility.GetDisabledWorkTagLabelColor(CS$<>8__locals2.CS$<>8__locals1.pawn, tag);
							if (Mouse.IsOver(r))
							{
								Widgets.DrawHighlight(r);
							}
							Widgets.Label(new Rect(r.x + 5f, r.y, r.width - 10f, r.height), tag.LabelTranslated().CapitalizeFirst());
							if (Mouse.IsOver(r))
							{
								WorkTags tagLocal = tag;
								TipSignal tip = new TipSignal(() => CharacterCardUtility.GetWorkTypeDisabledCausedBy(pawnLocal, tagLocal) + "\n" + CharacterCardUtility.GetWorkTypesDisabledByWorkTag(tagLocal), (int)currentY * 32);
								TooltipHandler.TipRegion(r, tip);
							}
						};
						if (allowWorkTagVerticalLayout)
						{
							GenUI.DrawElementStackVertical<WorkTags>(new Rect(sectionRect.x, currentY, CS$<>8__locals2.leftRect.width - 5f, CS$<>8__locals2.leftRect.height / (float)numSections), 22f, disabledTagsList, drawer, workTagWidthGetter, 5f);
						}
						else
						{
							GenUI.DrawElementStack<WorkTags>(new Rect(sectionRect.x, currentY, CS$<>8__locals2.leftRect.width - 5f, CS$<>8__locals2.leftRect.height / (float)numSections), 22f, disabledTagsList, drawer, workTagWidthGetter, 5f, 5f, true);
						}
					}
					GUI.color = Color.white;
				}
			});
			num7 = 30f;
			List<Trait> traits = CS$<>8__locals2.CS$<>8__locals1.pawn.story.traits.allTraits;
			if (traits == null || traits.Count == 0)
			{
				num7 += 22f;
			}
			else
			{
				num7 += GenUI.DrawElementStack<Trait>(new Rect(0f, 0f, CS$<>8__locals2.leftRect.width - 5f, CS$<>8__locals2.leftRect.height), 22f, CS$<>8__locals2.CS$<>8__locals1.pawn.story.traits.allTraits, delegate(Rect r, Trait trait)
				{
				}, (Trait trait) => Text.CalcSize(trait.LabelCap).x + 10f, 4f, 5f, true).height;
			}
			num7 += 12f;
			list.Add(new CharacterCardUtility.LeftRectSection
			{
				rect = new Rect(0f, 0f, CS$<>8__locals2.leftRect.width, num7),
				drawer = delegate(Rect sectionRect)
				{
					Text.Font = GameFont.Medium;
					float currentY = sectionRect.y;
					Widgets.Label(new Rect(sectionRect.x, currentY, 200f, 30f), "Traits".Translate());
					currentY += 30f;
					Text.Font = GameFont.Small;
					if (traits == null || traits.Count == 0)
					{
						Color color = GUI.color;
						GUI.color = Color.gray;
						Rect rect12 = new Rect(sectionRect.x, currentY, CS$<>8__locals2.leftRect.width, 24f);
						if (Mouse.IsOver(rect12))
						{
							Widgets.DrawHighlight(rect12);
						}
						Widgets.Label(rect12, "None".Translate());
						currentY += rect12.height + 2f;
						TooltipHandler.TipRegionByKey(rect12, "None");
						GUI.color = color;
						return;
					}
					GenUI.DrawElementStack<Trait>(new Rect(sectionRect.x, currentY, CS$<>8__locals2.leftRect.width - 5f, CS$<>8__locals2.leftRect.height / (float)numSections), 22f, CS$<>8__locals2.CS$<>8__locals1.pawn.story.traits.allTraits, delegate(Rect r, Trait trait)
					{
						Color color2 = GUI.color;
						GUI.color = CharacterCardUtility.StackElementBackground;
						GUI.DrawTexture(r, BaseContent.WhiteTex);
						GUI.color = color2;
						if (Mouse.IsOver(r))
						{
							Widgets.DrawHighlight(r);
						}
						Widgets.Label(new Rect(r.x + 5f, r.y, r.width - 10f, r.height), trait.LabelCap);
						if (Mouse.IsOver(r))
						{
							Trait trLocal = trait;
							TipSignal tip = new TipSignal(() => trLocal.TipString(CS$<>8__locals2.CS$<>8__locals1.pawn), (int)currentY * 37);
							TooltipHandler.TipRegion(r, tip);
						}
					}, (Trait trait) => Text.CalcSize(trait.LabelCap).x + 10f, 4f, 5f, true);
				}
			});
			if (abilities.Any<Ability>())
			{
				num7 = 30f;
				num7 += GenUI.DrawElementStack<Ability>(new Rect(0f, 0f, CS$<>8__locals2.leftRect.width - 5f, CS$<>8__locals2.leftRect.height), 32f, abilities, delegate(Rect r, Ability abil)
				{
				}, (Ability abil) => 32f, 4f, 5f, true).height;
				list.Add(new CharacterCardUtility.LeftRectSection
				{
					rect = new Rect(0f, 0f, CS$<>8__locals2.leftRect.width, num7),
					drawer = delegate(Rect sectionRect)
					{
						Text.Font = GameFont.Medium;
						float currentY = sectionRect.y;
						Widgets.Label(new Rect(sectionRect.x, currentY, 200f, 30f), "Abilities".Translate(CS$<>8__locals2.CS$<>8__locals1.pawn));
						currentY += 30f;
						Text.Font = GameFont.Small;
						GenUI.DrawElementStack<Ability>(new Rect(sectionRect.x, currentY, CS$<>8__locals2.leftRect.width - 5f, CS$<>8__locals2.leftRect.height), 32f, abilities, delegate(Rect r, Ability abil)
						{
							GUI.DrawTexture(r, BaseContent.ClearTex);
							if (Mouse.IsOver(r))
							{
								Widgets.DrawHighlight(r);
							}
							if (Widgets.ButtonImage(r, abil.def.uiIcon, false))
							{
								Find.WindowStack.Add(new Dialog_InfoCard(abil.def));
							}
							if (Mouse.IsOver(r))
							{
								Ability abilCapture = abil;
								TipSignal tip = new TipSignal(() => abilCapture.def.GetTooltip(null) + "\n\n" + "ClickToLearnMore".Translate(), (int)currentY * 37);
								TooltipHandler.TipRegion(r, tip);
							}
						}, (Ability abil) => 32f, 4f, 5f, true);
						GUI.color = Color.white;
					}
				});
			}
			float num8 = CS$<>8__locals2.leftRect.height / (float)list.Count;
			float num9 = 0f;
			for (int i = 0; i < list.Count; i++)
			{
				CharacterCardUtility.LeftRectSection value = list[i];
				if (value.rect.height > num8)
				{
					num9 += value.rect.height - num8;
					value.calculatedSize = value.rect.height;
				}
				else
				{
					value.calculatedSize = num8;
				}
				list[i] = value;
			}
			bool flag2 = false;
			float num10 = 0f;
			if (num9 > 0f)
			{
				CharacterCardUtility.LeftRectSection leftRectSection = list[0];
				float num11 = leftRectSection.rect.height + 12f;
				num9 -= leftRectSection.calculatedSize - num11;
				leftRectSection.calculatedSize = num11;
				list[0] = leftRectSection;
			}
			while (num9 > 0f)
			{
				bool flag3 = true;
				for (int j = 0; j < list.Count; j++)
				{
					CharacterCardUtility.LeftRectSection leftRectSection2 = list[j];
					if (leftRectSection2.calculatedSize - leftRectSection2.rect.height > 0f)
					{
						leftRectSection2.calculatedSize -= 1f;
						num9 -= 1f;
						flag3 = false;
					}
					list[j] = leftRectSection2;
				}
				if (flag3)
				{
					for (int k = 0; k < list.Count; k++)
					{
						CharacterCardUtility.LeftRectSection leftRectSection3 = list[k];
						if (k > 0)
						{
							leftRectSection3.calculatedSize = Mathf.Max(leftRectSection3.rect.height, num8);
						}
						else
						{
							leftRectSection3.calculatedSize = leftRectSection3.rect.height + 22f;
						}
						num10 += leftRectSection3.calculatedSize;
						list[k] = leftRectSection3;
					}
					flag2 = true;
					break;
				}
			}
			if (flag2)
			{
				Widgets.BeginScrollView(new Rect(0f, 0f, CS$<>8__locals2.leftRect.width, CS$<>8__locals2.leftRect.height), ref CharacterCardUtility.leftRectScrollPos, new Rect(0f, 0f, CS$<>8__locals2.leftRect.width - 16f, num10), true);
			}
			CS$<>8__locals2.curY = 0f;
			for (int l = 0; l < list.Count; l++)
			{
				CharacterCardUtility.LeftRectSection leftRectSection4 = list[l];
				leftRectSection4.drawer(new Rect(0f, CS$<>8__locals2.curY, CS$<>8__locals2.leftRect.width, leftRectSection4.rect.height));
				CS$<>8__locals2.curY += leftRectSection4.calculatedSize;
			}
			if (flag2)
			{
				Widgets.EndScrollView();
			}
			GUI.EndGroup();
			GUI.BeginGroup(position);
			SkillUI.SkillDrawMode mode;
			if (Current.ProgramState == ProgramState.Playing)
			{
				mode = SkillUI.SkillDrawMode.Gameplay;
			}
			else
			{
				mode = SkillUI.SkillDrawMode.Menu;
			}
			SkillUI.DrawSkillsOf(CS$<>8__locals2.CS$<>8__locals1.pawn, new Vector2(0f, 0f), mode);
			GUI.EndGroup();
			GUI.EndGroup();
		}

		// Token: 0x060093B5 RID: 37813 RVA: 0x002AA52C File Offset: 0x002A872C
		private static string GetTitleTipString(Pawn pawn, Faction faction, RoyalTitle title, int favor)
		{
			RoyalTitleDef def = title.def;
			TaggedString t = "RoyalTitleTooltipHasTitle".Translate(pawn.Named("PAWN"), faction.Named("FACTION"), def.GetLabelCapFor(pawn).Named("TITLE"));
			t += "\n\n" + faction.def.royalFavorLabel.CapitalizeFirst() + ": " + favor.ToString();
			RoyalTitleDef nextTitle = def.GetNextTitle(faction);
			if (nextTitle != null)
			{
				t += "\n" + "RoyalTitleTooltipNextTitle".Translate() + ": " + nextTitle.GetLabelCapFor(pawn) + " (" + "RoyalTitleTooltipNextTitleFavorCost".Translate(nextTitle.favorCost.ToString(), faction.Named("FACTION")) + ")";
			}
			else
			{
				t += "\n" + "RoyalTitleTooltipFinalTitle".Translate();
			}
			if (title.def.canBeInherited)
			{
				Pawn heir = pawn.royalty.GetHeir(faction);
				if (heir != null)
				{
					t += "\n\n" + "RoyalTitleTooltipInheritance".Translate(pawn.Named("PAWN"), heir.Named("HEIR"));
					if (heir.Faction == null)
					{
						t += " " + "RoyalTitleTooltipHeirNoFaction".Translate(heir.Named("HEIR"));
					}
					else if (heir.Faction != faction)
					{
						t += " " + "RoyalTitleTooltipHeirDifferentFaction".Translate(heir.Named("HEIR"), heir.Faction.Named("FACTION"));
					}
				}
				else
				{
					t += "\n\n" + "RoyalTitleTooltipNoHeir".Translate(pawn.Named("PAWN"));
				}
			}
			else
			{
				t += "\n\n" + "LetterRoyalTitleCantBeInherited".Translate(title.def.Named("TITLE")).CapitalizeFirst() + " " + "LetterRoyalTitleNoHeir".Translate(pawn.Named("PAWN"));
			}
			t += "\n\n" + (title.conceited ? "RoyalTitleTooltipConceited" : "RoyalTitleTooltipNonConceited").Translate(pawn.Named("PAWN"));
			t += "\n\n" + RoyalTitleUtility.GetTitleProgressionInfo(faction, pawn);
			return (t + ("\n\n" + "ClickToLearnMore".Translate())).Resolve();
		}

		// Token: 0x060093B6 RID: 37814 RVA: 0x002AA7F4 File Offset: 0x002A89F4
		private static List<object> GetWorkTypeDisableCauses(Pawn pawn, WorkTags workTag)
		{
			List<object> list = new List<object>();
			if (pawn.story != null && pawn.story.childhood != null && (pawn.story.childhood.workDisables & workTag) != WorkTags.None)
			{
				list.Add(pawn.story.childhood);
			}
			if (pawn.story != null && pawn.story.adulthood != null && (pawn.story.adulthood.workDisables & workTag) != WorkTags.None)
			{
				list.Add(pawn.story.adulthood);
			}
			if (pawn.health != null && pawn.health.hediffSet != null)
			{
				foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
				{
					HediffStage curStage = hediff.CurStage;
					if (curStage != null && (curStage.disabledWorkTags & workTag) != WorkTags.None)
					{
						list.Add(hediff);
					}
				}
			}
			if (pawn.story.traits != null)
			{
				for (int i = 0; i < pawn.story.traits.allTraits.Count; i++)
				{
					Trait trait = pawn.story.traits.allTraits[i];
					if ((trait.def.disabledWorkTags & workTag) != WorkTags.None)
					{
						list.Add(trait);
					}
				}
			}
			if (pawn.royalty != null)
			{
				foreach (RoyalTitle royalTitle in pawn.royalty.AllTitlesForReading)
				{
					if (royalTitle.conceited && (royalTitle.def.disabledWorkTags & workTag) != WorkTags.None)
					{
						list.Add(royalTitle);
					}
				}
			}
			foreach (QuestPart_WorkDisabled questPart_WorkDisabled in QuestUtility.GetWorkDisabledQuestPart(pawn))
			{
				if ((questPart_WorkDisabled.disabledWorkTags & workTag) != WorkTags.None && !list.Contains(questPart_WorkDisabled.quest))
				{
					list.Add(questPart_WorkDisabled.quest);
				}
			}
			return list;
		}

		// Token: 0x060093B7 RID: 37815 RVA: 0x002AAA24 File Offset: 0x002A8C24
		private static Color GetDisabledWorkTagLabelColor(Pawn pawn, WorkTags workTag)
		{
			using (List<object>.Enumerator enumerator = CharacterCardUtility.GetWorkTypeDisableCauses(pawn, workTag).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current is RoyalTitleDef)
					{
						return CharacterCardUtility.TitleCausedWorkTagDisableColor;
					}
				}
			}
			return Color.white;
		}

		// Token: 0x060093B8 RID: 37816 RVA: 0x002AAA88 File Offset: 0x002A8C88
		private static string GetWorkTypeDisabledCausedBy(Pawn pawn, WorkTags workTag)
		{
			List<object> workTypeDisableCauses = CharacterCardUtility.GetWorkTypeDisableCauses(pawn, workTag);
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object obj in workTypeDisableCauses)
			{
				if (obj is Backstory)
				{
					stringBuilder.AppendLine("IncapableOfTooltipBackstory".Translate((obj as Backstory).TitleFor(pawn.gender)));
				}
				else if (obj is Trait)
				{
					stringBuilder.AppendLine("IncapableOfTooltipTrait".Translate((obj as Trait).LabelCap));
				}
				else if (obj is Hediff)
				{
					stringBuilder.AppendLine("IncapableOfTooltipHediff".Translate((obj as Hediff).LabelCap));
				}
				else if (obj is RoyalTitle)
				{
					stringBuilder.AppendLine("IncapableOfTooltipTitle".Translate((obj as RoyalTitle).def.GetLabelFor(pawn)));
				}
				else if (obj is Quest)
				{
					stringBuilder.AppendLine("IncapableOfTooltipQuest".Translate((obj as Quest).name));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060093B9 RID: 37817 RVA: 0x002AABF4 File Offset: 0x002A8DF4
		private static string GetWorkTypesDisabledByWorkTag(WorkTags workTag)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("IncapableOfTooltipWorkTypes".Translate());
			foreach (WorkTypeDef workTypeDef in DefDatabase<WorkTypeDef>.AllDefs)
			{
				if ((workTypeDef.workTags & workTag) > WorkTags.None)
				{
					stringBuilder.Append("- ");
					stringBuilder.AppendLine(workTypeDef.pawnLabel);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060093BA RID: 37818 RVA: 0x002AAC80 File Offset: 0x002A8E80
		public static Vector2 PawnCardSize(Pawn pawn)
		{
			Vector2 basePawnCardSize = CharacterCardUtility.BasePawnCardSize;
			CharacterCardUtility.tmpInspectStrings.Length = 0;
			int num;
			QuestUtility.AppendInspectStringsFromQuestParts(CharacterCardUtility.tmpInspectStrings, pawn, out num);
			if (num >= 2)
			{
				basePawnCardSize.y += (float)((num - 1) * 20);
			}
			return basePawnCardSize;
		}

		// Token: 0x060093BB RID: 37819 RVA: 0x002AACC4 File Offset: 0x002A8EC4
		public static void DoNameInputRect(Rect rect, ref string name, int maxLength)
		{
			string text = Widgets.TextField(rect, name);
			if (text.Length <= maxLength && CharacterCardUtility.ValidNameRegex.IsMatch(text))
			{
				name = text;
			}
		}

		// Token: 0x060093BC RID: 37820 RVA: 0x00062EF7 File Offset: 0x000610F7
		private static IEnumerable<WorkTags> WorkTagsFrom(WorkTags tags)
		{
			foreach (WorkTags workTags in tags.GetAllSelectedItems<WorkTags>())
			{
				if (workTags != WorkTags.None)
				{
					yield return workTags;
				}
			}
			IEnumerator<WorkTags> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060093BD RID: 37821 RVA: 0x002AACF4 File Offset: 0x002A8EF4
		private static Vector2 GetQuestLineSize(string line, Quest quest)
		{
			Vector2 vector = Text.CalcSize(line);
			return new Vector2(17f + vector.x + 10f, Mathf.Max(24f, vector.y));
		}

		// Token: 0x060093BE RID: 37822 RVA: 0x002AAD30 File Offset: 0x002A8F30
		private static void DoQuestLine(Rect rect, string line, Quest quest)
		{
			Rect rect2 = rect;
			rect2.xMin += 22f;
			rect2.height = Text.CalcSize(line).y;
			float x = Text.CalcSize(line).x;
			Rect rect3 = new Rect(rect.x, rect.y, Mathf.Min(x, rect2.width) + 24f + -7f, rect.height);
			if (!quest.hidden)
			{
				Widgets.DrawHighlightIfMouseover(rect3);
				TooltipHandler.TipRegionByKey(rect3, "ClickToViewInQuestsTab");
			}
			GUI.DrawTexture(new Rect(rect.x + -7f, rect.y - 2f, 24f, 24f), CharacterCardUtility.QuestIcon);
			Widgets.Label(rect2, line.Truncate(rect2.width, null));
			if (!quest.hidden && Widgets.ButtonInvisible(rect3, true))
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
				((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(quest);
			}
		}

		// Token: 0x04005D97 RID: 23959
		private static Vector2 leftRectScrollPos = Vector2.zero;

		// Token: 0x04005D98 RID: 23960
		public const int MainRectsY = 100;

		// Token: 0x04005D99 RID: 23961
		private const float MainRectsHeight = 355f;

		// Token: 0x04005D9A RID: 23962
		private const int ConfigRectTitlesHeight = 40;

		// Token: 0x04005D9B RID: 23963
		private const int FactionIconSize = 22;

		// Token: 0x04005D9C RID: 23964
		public static Vector2 BasePawnCardSize = new Vector2(480f, 455f);

		// Token: 0x04005D9D RID: 23965
		private const int MaxNameLength = 12;

		// Token: 0x04005D9E RID: 23966
		public const int MaxNickLength = 16;

		// Token: 0x04005D9F RID: 23967
		public const int MaxTitleLength = 25;

		// Token: 0x04005DA0 RID: 23968
		public const int QuestLineHeight = 20;

		// Token: 0x04005DA1 RID: 23969
		private static readonly Texture2D QuestIcon = ContentFinder<Texture2D>.Get("UI/Icons/Quest", true);

		// Token: 0x04005DA2 RID: 23970
		public static readonly Color StackElementBackground = new Color(1f, 1f, 1f, 0.1f);

		// Token: 0x04005DA3 RID: 23971
		private static List<ExtraFaction> tmpExtraFactions = new List<ExtraFaction>();

		// Token: 0x04005DA4 RID: 23972
		private static readonly Color TitleCausedWorkTagDisableColor = new Color(0.67f, 0.84f, 0.9f);

		// Token: 0x04005DA5 RID: 23973
		private static StringBuilder tmpInspectStrings = new StringBuilder();

		// Token: 0x04005DA6 RID: 23974
		public static Regex ValidNameRegex = new Regex("^[\\p{L}0-9 '\\-.]*$");

		// Token: 0x04005DA7 RID: 23975
		private const int QuestIconSize = 24;

		// Token: 0x04005DA8 RID: 23976
		private const int QuestIconExtraPaddingLeft = -7;

		// Token: 0x02001A22 RID: 6690
		private struct LeftRectSection
		{
			// Token: 0x04005DA9 RID: 23977
			public Rect rect;

			// Token: 0x04005DAA RID: 23978
			public Action<Rect> drawer;

			// Token: 0x04005DAB RID: 23979
			public float calculatedSize;
		}
	}
}
