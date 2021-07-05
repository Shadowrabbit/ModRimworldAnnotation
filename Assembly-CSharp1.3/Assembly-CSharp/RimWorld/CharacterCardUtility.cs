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
	// Token: 0x02001306 RID: 4870
	[StaticConstructorOnStartup]
	public static class CharacterCardUtility
	{
		// Token: 0x0600750A RID: 29962 RVA: 0x0027DF4C File Offset: 0x0027C14C
		public static void DrawCharacterCard(Rect rect, Pawn pawn, Action randomizeCallback = null, Rect creationRect = default(Rect))
		{
			CharacterCardUtility.<>c__DisplayClass15_0 CS$<>8__locals1 = new CharacterCardUtility.<>c__DisplayClass15_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.creationMode = (randomizeCallback != null);
			GUI.BeginGroup(CS$<>8__locals1.creationMode ? creationRect : rect);
			CharacterCardUtility.<>c__DisplayClass15_1 CS$<>8__locals2 = new CharacterCardUtility.<>c__DisplayClass15_1();
			CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
			Rect rect2 = new Rect(0f, 0f, 300f, 30f);
			NameTriple nameTriple = CS$<>8__locals2.CS$<>8__locals1.pawn.Name as NameTriple;
			float x;
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
				string text = CS$<>8__locals2.CS$<>8__locals1.pawn.Name.ToStringFull.CapitalizeFirst();
				Widgets.Label(rect2, text);
				if (CS$<>8__locals2.CS$<>8__locals1.pawn.guilt != null && CS$<>8__locals2.CS$<>8__locals1.pawn.guilt.IsGuilty)
				{
					x = Text.CalcSize(text).x;
					Rect rect6 = new Rect(x + 10f, 0f, 32f, 32f);
					GUI.DrawTexture(rect6, TexUI.GuiltyTex);
					TooltipHandler.TipRegion(rect6, () => CS$<>8__locals2.CS$<>8__locals1.pawn.guilt.Tip, 6321623);
				}
				Text.Font = GameFont.Small;
			}
			if (randomizeCallback != null)
			{
				Rect rect7 = new Rect(creationRect.width - 24f - 100f, 0f, 100f, rect2.height);
				if (Widgets.ButtonText(rect7, "Randomize".Translate(), true, true, true))
				{
					SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
					randomizeCallback();
				}
				UIHighlighter.HighlightOpportunity(rect7, "RandomizePawn");
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
					Rect rect8 = new Rect(num, 0f, 30f, 30f);
					if (Mouse.IsOver(rect8))
					{
						TooltipHandler.TipRegion(rect8, PawnBanishUtility.GetBanishButtonTip(CS$<>8__locals2.CS$<>8__locals1.pawn));
					}
					if (Widgets.ButtonImage(rect8, TexButton.Banish, true))
					{
						if (CS$<>8__locals2.CS$<>8__locals1.pawn.Downed)
						{
							Messages.Message("MessageCantBanishDownedPawn".Translate(CS$<>8__locals2.CS$<>8__locals1.pawn.LabelShort, CS$<>8__locals2.CS$<>8__locals1.pawn).AdjustedFor(CS$<>8__locals2.CS$<>8__locals1.pawn, "PAWN", true), CS$<>8__locals2.CS$<>8__locals1.pawn, MessageTypeDefOf.RejectInput, false);
						}
						else
						{
							PawnBanishUtility.ShowBanishPawnConfirmationDialog(CS$<>8__locals2.CS$<>8__locals1.pawn, null);
						}
					}
					num -= 40f;
				}
				if (CS$<>8__locals2.CS$<>8__locals1.pawn.IsColonist)
				{
					Rect rect9 = new Rect(num, 0f, 30f, 30f);
					TooltipHandler.TipRegionByKey(rect9, "RenameColonist");
					if (Widgets.ButtonImage(rect9, TexButton.Rename, true))
					{
						Find.WindowStack.Add(new Dialog_NamePawn(CS$<>8__locals2.CS$<>8__locals1.pawn));
					}
					num -= 40f;
				}
				if (CS$<>8__locals2.CS$<>8__locals1.pawn.IsFreeColonist && !CS$<>8__locals2.CS$<>8__locals1.pawn.IsQuestLodger() && CS$<>8__locals2.CS$<>8__locals1.pawn.royalty != null && CS$<>8__locals2.CS$<>8__locals1.pawn.royalty.AllTitlesForReading.Count > 0)
				{
					Rect rect10 = new Rect(num, 0f, 30f, 30f);
					TooltipHandler.TipRegionByKey(rect10, "RenounceTitle");
					if (Widgets.ButtonImage(rect10, TexButton.RenounceTitle, true))
					{
						FloatMenuUtility.MakeMenu<RoyalTitle>(CS$<>8__locals2.CS$<>8__locals1.pawn.royalty.AllTitlesForReading, (RoyalTitle title) => "RenounceTitle".Translate() + ": " + "TitleOfFaction".Translate(title.def.GetLabelCapFor(CS$<>8__locals2.CS$<>8__locals1.pawn), title.faction.GetCallLabel()), delegate(RoyalTitle title)
						{
							Action <>9__7;
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
										stringBuilder.AppendLine("- " + royalTitlePermitDef.LabelCap + " (" + base.<DrawCharacterCard>g__FirstTitleWithPermit|6(royalTitlePermitDef).GetLabelFor(CS$<>8__locals2.CS$<>8__locals1.pawn) + ")");
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
								if ((confirmedAct = <>9__7) == null)
								{
									confirmedAct = (<>9__7 = delegate()
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
				if (CS$<>8__locals2.CS$<>8__locals1.pawn.guilt != null && CS$<>8__locals2.CS$<>8__locals1.pawn.guilt.IsGuilty && CS$<>8__locals2.CS$<>8__locals1.pawn.IsFreeColonist && !CS$<>8__locals2.CS$<>8__locals1.pawn.IsQuestLodger())
				{
					Rect rect11 = new Rect(num + 5f, 0f, 30f, 30f);
					TooltipHandler.TipRegionByKey(rect11, "ExecuteColonist");
					if (Widgets.ButtonImage(rect11, TexButton.ExecuteColonist, true))
					{
						CS$<>8__locals2.CS$<>8__locals1.pawn.guilt.awaitingExecution = !CS$<>8__locals2.CS$<>8__locals1.pawn.guilt.awaitingExecution;
						if (CS$<>8__locals2.CS$<>8__locals1.pawn.guilt.awaitingExecution)
						{
							Messages.Message("MessageColonistMarkedForExecution".Translate(CS$<>8__locals2.CS$<>8__locals1.pawn), CS$<>8__locals2.CS$<>8__locals1.pawn, MessageTypeDefOf.SilentInput, false);
						}
					}
					if (CS$<>8__locals2.CS$<>8__locals1.pawn.guilt.awaitingExecution)
					{
						Rect position = default(Rect);
						position.x += rect11.x + 22f;
						position.width = 15f;
						position.height = 15f;
						GUI.DrawTexture(position, Widgets.CheckboxOnTex);
					}
				}
			}
			CS$<>8__locals2.stackElements = new List<GenUI.AnonymousStackElement>();
			float num2 = rect.width - 10f;
			float width = CS$<>8__locals2.CS$<>8__locals1.creationMode ? (num2 - 20f - Page_ConfigureStartingPawns.PawnPortraitSize.x) : num2;
			Text.Font = GameFont.Small;
			string mainDesc = CS$<>8__locals2.CS$<>8__locals1.pawn.MainDesc(false);
			CS$<>8__locals2.stackElements.Add(new GenUI.AnonymousStackElement
			{
				drawer = delegate(Rect r)
				{
					Widgets.Label(r, mainDesc);
					if (Mouse.IsOver(r))
					{
						Func<string> textGetter;
						if ((textGetter = CS$<>8__locals2.CS$<>8__locals1.<>9__10) == null)
						{
							textGetter = (CS$<>8__locals2.CS$<>8__locals1.<>9__10 = (() => CS$<>8__locals2.CS$<>8__locals1.pawn.ageTracker.AgeTooltipString));
						}
						TooltipHandler.TipRegion(r, textGetter, 6873641);
					}
				},
				width = Text.CalcSize(mainDesc).x + 5f
			});
			if (CS$<>8__locals2.CS$<>8__locals1.pawn.Faction != null && !CS$<>8__locals2.CS$<>8__locals1.pawn.Faction.Hidden)
			{
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
						Rect position3 = new Rect(r.x + 1f, r.y + 1f, 20f, 20f);
						GUI.color = CS$<>8__locals2.CS$<>8__locals1.pawn.Faction.Color;
						GUI.DrawTexture(position3, CS$<>8__locals2.CS$<>8__locals1.pawn.Faction.def.FactionIcon);
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
							string text2 = string.Concat(new string[]
							{
								"Faction".Translate().Colorize(ColoredText.TipSectionTitleColor),
								"\n\n",
								"FactionDesc".Translate(CS$<>8__locals2.CS$<>8__locals1.pawn.Named("PAWN")).Resolve(),
								"\n\n",
								"ClickToViewFactions".Translate().Resolve()
							});
							TipSignal tip = new TipSignal(text2, CS$<>8__locals2.CS$<>8__locals1.pawn.Faction.loadID * 37);
							TooltipHandler.TipRegion(rect12, tip);
						}
					},
					width = Text.CalcSize(CS$<>8__locals2.CS$<>8__locals1.pawn.Faction.Name).x + 22f + 15f
				});
			}
			CharacterCardUtility.tmpExtraFactions.Clear();
			QuestUtility.GetExtraFactionsFromQuestParts(CS$<>8__locals2.CS$<>8__locals1.pawn, CharacterCardUtility.tmpExtraFactions, null);
			GuestUtility.GetExtraFactionsFromGuestStatus(CS$<>8__locals2.CS$<>8__locals1.pawn, CharacterCardUtility.tmpExtraFactions);
			foreach (ExtraFaction extraFaction in CharacterCardUtility.tmpExtraFactions)
			{
				if (CS$<>8__locals2.CS$<>8__locals1.pawn.Faction != extraFaction.faction)
				{
					ExtraFaction localExtraFaction = extraFaction;
					string factionName = localExtraFaction.faction.Name;
					bool drawExtraFactionIcon = localExtraFaction.factionType == ExtraFactionType.HomeFaction || localExtraFaction.factionType == ExtraFactionType.MiniFaction;
					CS$<>8__locals2.stackElements.Add(new GenUI.AnonymousStackElement
					{
						drawer = delegate(Rect r)
						{
							Rect rect12 = new Rect(r.x, r.y, r.width, r.height);
							Rect rect13 = drawExtraFactionIcon ? rect12 : r;
							Color color = GUI.color;
							GUI.color = CharacterCardUtility.StackElementBackground;
							GUI.DrawTexture(rect13, BaseContent.WhiteTex);
							GUI.color = color;
							Widgets.DrawHighlightIfMouseover(rect13);
							if (drawExtraFactionIcon)
							{
								Rect rect14 = new Rect(r.x, r.y, r.width, r.height);
								Rect position3 = new Rect(r.x + 1f, r.y + 1f, 20f, 20f);
								GUI.color = localExtraFaction.faction.Color;
								GUI.DrawTexture(position3, localExtraFaction.faction.def.FactionIcon);
								GUI.color = color;
								Widgets.Label(new Rect(rect14.x + rect14.height + 5f, rect14.y, rect14.width - 10f, rect14.height), factionName);
							}
							else
							{
								Widgets.Label(new Rect(r.x + 5f, r.y, r.width - 10f, r.height), factionName);
							}
							if (Widgets.ButtonInvisible(rect12, true))
							{
								Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Factions, true);
							}
							if (Mouse.IsOver(rect13))
							{
								TaggedString taggedString = localExtraFaction.factionType.GetLabel().CapitalizeFirst() + "\n\n" + "ExtraFactionDesc".Translate(CS$<>8__locals2.CS$<>8__locals1.pawn.Named("PAWN")) + "\n\n" + "ClickToViewFactions".Translate();
								TipSignal tip = new TipSignal(taggedString, localExtraFaction.faction.loadID ^ 1938473043);
								TooltipHandler.TipRegion(rect13, tip);
							}
						},
						width = Text.CalcSize(factionName).x + (float)(drawExtraFactionIcon ? 22 : 0) + 15f
					});
				}
			}
			if (CS$<>8__locals2.CS$<>8__locals1.pawn.Ideo != null && ModsConfig.IdeologyActive)
			{
				float width2 = Text.CalcSize(CS$<>8__locals2.CS$<>8__locals1.pawn.Ideo.name).x + 22f + 15f;
				CS$<>8__locals2.stackElements.Add(new GenUI.AnonymousStackElement
				{
					drawer = delegate(Rect r)
					{
						GUI.color = CharacterCardUtility.StackElementBackground;
						GUI.DrawTexture(r, BaseContent.WhiteTex);
						GUI.color = Color.white;
						Widgets.DrawHighlightIfMouseover(r);
						Rect rect12 = new Rect(r.x, r.y, r.width, r.height);
						Rect position3 = new Rect(r.x + 1f, r.y + 1f, 20f, 20f);
						GUI.color = CS$<>8__locals2.CS$<>8__locals1.pawn.Ideo.Color;
						GUI.DrawTexture(position3, CS$<>8__locals2.CS$<>8__locals1.pawn.Ideo.Icon);
						GUI.color = Color.white;
						Widgets.Label(new Rect(rect12.x + rect12.height + 5f, rect12.y, rect12.width - 10f, rect12.height), CS$<>8__locals2.CS$<>8__locals1.pawn.Ideo.name);
						if (Widgets.ButtonInvisible(r, true))
						{
							IdeoUIUtility.OpenIdeoInfo(CS$<>8__locals2.CS$<>8__locals1.pawn.Ideo);
						}
						if (Mouse.IsOver(r))
						{
							string text2 = string.Concat(new string[]
							{
								CS$<>8__locals2.CS$<>8__locals1.pawn.Ideo.name.Colorize(ColoredText.TipSectionTitleColor),
								"\n",
								"Certainty".Translate().CapitalizeFirst().Resolve(),
								": ",
								CS$<>8__locals2.CS$<>8__locals1.pawn.ideo.Certainty.ToStringPercent(),
								"\n\n",
								"ClickForMoreInfo".Translate().Resolve()
							});
							if (CS$<>8__locals2.CS$<>8__locals1.pawn.ideo.PreviousIdeos.Any<Ideo>())
							{
								text2 += "\n\n" + "Formerly".Translate().CapitalizeFirst() + ": \n" + (from x in CS$<>8__locals2.CS$<>8__locals1.pawn.ideo.PreviousIdeos
								select x.name).ToLineList("  - ", false);
							}
							TooltipHandler.TipRegion(r, text2);
						}
					},
					width = width2
				});
			}
			CS$<>8__locals2.curY = 48f;
			if (ModsConfig.IdeologyActive)
			{
				CharacterCardUtility.<>c__DisplayClass15_6 CS$<>8__locals5 = new CharacterCardUtility.<>c__DisplayClass15_6();
				CS$<>8__locals5.CS$<>8__locals5 = CS$<>8__locals2;
				CharacterCardUtility.<>c__DisplayClass15_6 CS$<>8__locals6 = CS$<>8__locals5;
				Ideo ideo = CS$<>8__locals5.CS$<>8__locals5.CS$<>8__locals1.pawn.Ideo;
				CS$<>8__locals6.role = ((ideo != null) ? ideo.GetRole(CS$<>8__locals5.CS$<>8__locals5.CS$<>8__locals1.pawn) : null);
				if (CS$<>8__locals5.role != null)
				{
					string roleLabel = CS$<>8__locals5.role.LabelForPawn(CS$<>8__locals5.CS$<>8__locals5.CS$<>8__locals1.pawn);
					CS$<>8__locals5.CS$<>8__locals5.stackElements.Add(new GenUI.AnonymousStackElement
					{
						drawer = delegate(Rect r)
						{
							Color color = GUI.color;
							Rect rect12 = new Rect(r.x, r.y, r.width, r.height);
							GUI.color = CharacterCardUtility.StackElementBackground;
							GUI.DrawTexture(rect12, BaseContent.WhiteTex);
							GUI.color = color;
							if (Mouse.IsOver(rect12))
							{
								Widgets.DrawHighlight(rect12);
							}
							Rect rect13 = new Rect(r.x, r.y, r.width + 22f + 9f, r.height);
							Rect position3 = new Rect(r.x + 1f, r.y + 1f, 20f, 20f);
							GUI.color = CS$<>8__locals5.CS$<>8__locals5.CS$<>8__locals1.pawn.Ideo.Color;
							GUI.DrawTexture(position3, CS$<>8__locals5.role.Icon);
							GUI.color = Color.white;
							Widgets.Label(new Rect(rect13.x + 22f + 5f, rect13.y, rect13.width - 10f, rect13.height), roleLabel);
							if (Widgets.ButtonInvisible(rect12, true))
							{
								InspectPaneUtility.OpenTab(typeof(ITab_Pawn_Social));
							}
							if (Mouse.IsOver(rect12))
							{
								Func<string> textGetter;
								if ((textGetter = CS$<>8__locals5.<>9__16) == null)
								{
									textGetter = (CS$<>8__locals5.<>9__16 = (() => CS$<>8__locals5.role.GetTip()));
								}
								TipSignal tip = new TipSignal(textGetter, (int)CS$<>8__locals5.CS$<>8__locals5.curY * 39);
								TooltipHandler.TipRegion(rect12, tip);
							}
						},
						width = Text.CalcSize(roleLabel).x + 22f + 14f
					});
				}
			}
			if (CS$<>8__locals2.CS$<>8__locals1.pawn.royalty != null && CS$<>8__locals2.CS$<>8__locals1.pawn.royalty.AllTitlesForReading.Count > 0)
			{
				using (List<RoyalTitle>.Enumerator enumerator2 = CS$<>8__locals2.CS$<>8__locals1.pawn.royalty.AllTitlesForReading.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						CharacterCardUtility.<>c__DisplayClass15_8 CS$<>8__locals8 = new CharacterCardUtility.<>c__DisplayClass15_8();
						CS$<>8__locals8.CS$<>8__locals7 = CS$<>8__locals2;
						CS$<>8__locals8.title = enumerator2.Current;
						RoyalTitle localTitle = CS$<>8__locals8.title;
						string titleLabel = string.Concat(new object[]
						{
							localTitle.def.GetLabelCapFor(CS$<>8__locals8.CS$<>8__locals7.CS$<>8__locals1.pawn),
							" (",
							CS$<>8__locals8.CS$<>8__locals7.CS$<>8__locals1.pawn.royalty.GetFavor(localTitle.faction),
							")"
						});
						CS$<>8__locals8.CS$<>8__locals7.stackElements.Add(new GenUI.AnonymousStackElement
						{
							drawer = delegate(Rect r)
							{
								Color color = GUI.color;
								Rect rect12 = new Rect(r.x, r.y, r.width, r.height);
								GUI.color = CharacterCardUtility.StackElementBackground;
								GUI.DrawTexture(rect12, BaseContent.WhiteTex);
								GUI.color = color;
								int favor = CS$<>8__locals8.CS$<>8__locals7.CS$<>8__locals1.pawn.royalty.GetFavor(localTitle.faction);
								if (Mouse.IsOver(rect12))
								{
									Widgets.DrawHighlight(rect12);
								}
								Rect rect13 = new Rect(r.x, r.y, r.width + 22f + 9f, r.height);
								Rect position3 = new Rect(r.x + 1f, r.y + 1f, 20f, 20f);
								GUI.color = CS$<>8__locals8.title.faction.Color;
								GUI.DrawTexture(position3, localTitle.faction.def.FactionIcon);
								GUI.color = color;
								Widgets.Label(new Rect(rect13.x + 22f + 5f, rect13.y, rect13.width - 10f, rect13.height), titleLabel);
								if (Widgets.ButtonInvisible(rect12, true))
								{
									Find.WindowStack.Add(new Dialog_InfoCard(localTitle.def, localTitle.faction, CS$<>8__locals8.CS$<>8__locals7.CS$<>8__locals1.pawn));
								}
								if (Mouse.IsOver(rect12))
								{
									TipSignal tip = new TipSignal(() => CharacterCardUtility.GetTitleTipString(CS$<>8__locals8.CS$<>8__locals7.CS$<>8__locals1.pawn, localTitle.faction, localTitle, favor), (int)CS$<>8__locals8.CS$<>8__locals7.curY * 37);
									TooltipHandler.TipRegion(rect12, tip);
								}
							},
							width = Text.CalcSize(titleLabel).x + 22f + 14f
						});
					}
				}
			}
			if (ModsConfig.IdeologyActive && CS$<>8__locals2.CS$<>8__locals1.pawn.story != null && CS$<>8__locals2.CS$<>8__locals1.pawn.story.favoriteColor != null)
			{
				CS$<>8__locals2.stackElements.Add(new GenUI.AnonymousStackElement
				{
					drawer = delegate(Rect r)
					{
						Widgets.DrawRectFast(r, CS$<>8__locals2.CS$<>8__locals1.pawn.story.favoriteColor.Value, null);
						Func<string> textGetter;
						if ((textGetter = CS$<>8__locals2.CS$<>8__locals1.<>9__20) == null)
						{
							textGetter = (CS$<>8__locals2.CS$<>8__locals1.<>9__20 = (() => "FavoriteColorTooltip".Translate(CS$<>8__locals2.CS$<>8__locals1.pawn.Named("PAWN"), 0.6f.ToStringPercent().Named("PERCENTAGE"))));
						}
						TooltipHandler.TipRegion(r, textGetter, 837472764);
					},
					width = 22f
				});
			}
			int num3;
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
			}, CS$<>8__locals2.CS$<>8__locals1.pawn, out num3);
			CS$<>8__locals2.curY += GenUI.DrawElementStack<GenUI.AnonymousStackElement>(new Rect(0f, CS$<>8__locals2.curY, width, 50f), 22f, CS$<>8__locals2.stackElements, delegate(Rect r, GenUI.AnonymousStackElement obj)
			{
				obj.drawer(r);
			}, (GenUI.AnonymousStackElement obj) => obj.width, 4f, 5f, false).height;
			if (CS$<>8__locals2.stackElements.Any<GenUI.AnonymousStackElement>())
			{
				CS$<>8__locals2.curY += 10f;
			}
			CS$<>8__locals2.leftRect = new Rect(0f, CS$<>8__locals2.curY, 250f, 355f);
			Rect position2 = new Rect(CS$<>8__locals2.leftRect.xMax, CS$<>8__locals2.curY, 258f, 355f);
			GUI.BeginGroup(CS$<>8__locals2.leftRect);
			CS$<>8__locals2.curY = 0f;
			Pawn pawnLocal = CS$<>8__locals2.CS$<>8__locals1.pawn;
			List<Ability> abilities = (from a in CS$<>8__locals2.CS$<>8__locals1.pawn.abilities.abilities
			orderby a.def.level, a.def.EntropyGain
			select a).ToList<Ability>();
			int numSections = abilities.Any<Ability>() ? 5 : 4;
			float num4 = (float)Enum.GetValues(typeof(BackstorySlot)).Length * 22f;
			if (CS$<>8__locals2.CS$<>8__locals1.pawn.story != null && CS$<>8__locals2.CS$<>8__locals1.pawn.story.title != null)
			{
				num4 += 22f;
			}
			List<CharacterCardUtility.LeftRectSection> list = new List<CharacterCardUtility.LeftRectSection>();
			list.Add(new CharacterCardUtility.LeftRectSection
			{
				rect = new Rect(0f, 0f, CS$<>8__locals2.leftRect.width, num4),
				drawer = delegate(Rect sectionRect)
				{
					float num9 = sectionRect.y;
					Text.Font = GameFont.Small;
					foreach (object obj in Enum.GetValues(typeof(BackstorySlot)))
					{
						BackstorySlot backstorySlot = (BackstorySlot)obj;
						Backstory backstory = CS$<>8__locals2.CS$<>8__locals1.pawn.story.GetBackstory(backstorySlot);
						if (backstory != null)
						{
							Rect rect12 = new Rect(sectionRect.x, num9, CS$<>8__locals2.leftRect.width, 22f);
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
							num9 += rect12.height;
						}
					}
					if (CS$<>8__locals2.CS$<>8__locals1.pawn.story != null && CS$<>8__locals2.CS$<>8__locals1.pawn.story.title != null)
					{
						Rect rect14 = new Rect(sectionRect.x, num9, CS$<>8__locals2.leftRect.width, 22f);
						Text.Anchor = TextAnchor.MiddleLeft;
						Widgets.Label(rect14, "Current".Translate() + ":");
						Text.Anchor = TextAnchor.UpperLeft;
						Rect rect15 = new Rect(rect14);
						rect15.x += 90f;
						rect15.width -= 90f;
						Widgets.Label(rect15, CS$<>8__locals2.CS$<>8__locals1.pawn.story.title);
						num9 += rect14.height;
					}
				}
			});
			num4 = 30f;
			WorkTags disabledTags = CS$<>8__locals2.CS$<>8__locals1.pawn.CombinedDisabledWorkTags;
			List<WorkTags> disabledTagsList = CharacterCardUtility.WorkTagsFrom(disabledTags).ToList<WorkTags>();
			bool allowWorkTagVerticalLayout = false;
			GenUI.StackElementWidthGetter<WorkTags> workTagWidthGetter = (WorkTags tag) => Text.CalcSize(tag.LabelTranslated().CapitalizeFirst()).x + 10f;
			if (disabledTags == WorkTags.None)
			{
				num4 += 22f;
			}
			else
			{
				disabledTagsList.Sort(delegate(WorkTags a, WorkTags b)
				{
					int num9 = CharacterCardUtility.GetWorkTypeDisableCauses(CS$<>8__locals2.CS$<>8__locals1.pawn, a).Any((object c) => c is RoyalTitleDef) ? 1 : -1;
					int value2 = CharacterCardUtility.GetWorkTypeDisableCauses(CS$<>8__locals2.CS$<>8__locals1.pawn, b).Any((object c) => c is RoyalTitleDef) ? 1 : -1;
					return num9.CompareTo(value2);
				});
				num4 += GenUI.DrawElementStack<WorkTags>(new Rect(0f, 0f, CS$<>8__locals2.leftRect.width - 5f, CS$<>8__locals2.leftRect.height), 22f, disabledTagsList, delegate(Rect r, WorkTags tag)
				{
				}, workTagWidthGetter, 4f, 5f, false).height;
				num4 += 12f;
				allowWorkTagVerticalLayout = (GenUI.DrawElementStackVertical<WorkTags>(new Rect(0f, 0f, rect.width, CS$<>8__locals2.leftRect.height / (float)numSections), 22f, disabledTagsList, delegate(Rect r, WorkTags tag)
				{
				}, workTagWidthGetter, 5f).width <= CS$<>8__locals2.leftRect.width);
			}
			list.Add(new CharacterCardUtility.LeftRectSection
			{
				rect = new Rect(0f, 0f, CS$<>8__locals2.leftRect.width, num4),
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
			num4 = 30f;
			List<Trait> traits = CS$<>8__locals2.CS$<>8__locals1.pawn.story.traits.allTraits;
			if (traits == null || traits.Count == 0)
			{
				num4 += 22f;
			}
			else
			{
				num4 += GenUI.DrawElementStack<Trait>(new Rect(0f, 0f, CS$<>8__locals2.leftRect.width - 5f, CS$<>8__locals2.leftRect.height), 22f, CS$<>8__locals2.CS$<>8__locals1.pawn.story.traits.allTraits, delegate(Rect r, Trait trait)
				{
				}, (Trait trait) => Text.CalcSize(trait.LabelCap).x + 10f, 4f, 5f, true).height;
			}
			num4 += 12f;
			list.Add(new CharacterCardUtility.LeftRectSection
			{
				rect = new Rect(0f, 0f, CS$<>8__locals2.leftRect.width, num4),
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
				num4 = 30f;
				num4 += GenUI.DrawElementStack<Ability>(new Rect(0f, 0f, CS$<>8__locals2.leftRect.width - 5f, CS$<>8__locals2.leftRect.height), 32f, abilities, delegate(Rect r, Ability abil)
				{
				}, (Ability abil) => 32f, 4f, 5f, true).height;
				list.Add(new CharacterCardUtility.LeftRectSection
				{
					rect = new Rect(0f, 0f, CS$<>8__locals2.leftRect.width, num4),
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
								Find.WindowStack.Add(new Dialog_InfoCard(abil.def, null));
							}
							if (Mouse.IsOver(r))
							{
								Ability abilCapture = abil;
								TipSignal tip = new TipSignal(() => abilCapture.Tooltip + "\n\n" + "ClickToLearnMore".Translate(), (int)currentY * 37);
								TooltipHandler.TipRegion(r, tip);
							}
						}, (Ability abil) => 32f, 4f, 5f, true);
						GUI.color = Color.white;
					}
				});
			}
			float num5 = CS$<>8__locals2.leftRect.height / (float)list.Count;
			float num6 = 0f;
			for (int i = 0; i < list.Count; i++)
			{
				CharacterCardUtility.LeftRectSection value = list[i];
				if (value.rect.height > num5)
				{
					num6 += value.rect.height - num5;
					value.calculatedSize = value.rect.height;
				}
				else
				{
					value.calculatedSize = num5;
				}
				list[i] = value;
			}
			bool flag = false;
			float num7 = 0f;
			if (num6 > 0f)
			{
				CharacterCardUtility.LeftRectSection leftRectSection = list[0];
				float num8 = leftRectSection.rect.height + 12f;
				num6 -= leftRectSection.calculatedSize - num8;
				leftRectSection.calculatedSize = num8;
				list[0] = leftRectSection;
			}
			while (num6 > 0f)
			{
				bool flag2 = true;
				for (int j = 0; j < list.Count; j++)
				{
					CharacterCardUtility.LeftRectSection leftRectSection2 = list[j];
					if (leftRectSection2.calculatedSize - leftRectSection2.rect.height > 0f)
					{
						leftRectSection2.calculatedSize -= 1f;
						num6 -= 1f;
						flag2 = false;
					}
					list[j] = leftRectSection2;
				}
				if (flag2)
				{
					for (int k = 0; k < list.Count; k++)
					{
						CharacterCardUtility.LeftRectSection leftRectSection3 = list[k];
						if (k > 0)
						{
							leftRectSection3.calculatedSize = Mathf.Max(leftRectSection3.rect.height, num5);
						}
						else
						{
							leftRectSection3.calculatedSize = leftRectSection3.rect.height + 22f;
						}
						num7 += leftRectSection3.calculatedSize;
						list[k] = leftRectSection3;
					}
					flag = true;
					break;
				}
			}
			if (flag)
			{
				Widgets.BeginScrollView(new Rect(0f, 0f, CS$<>8__locals2.leftRect.width, CS$<>8__locals2.leftRect.height), ref CharacterCardUtility.leftRectScrollPos, new Rect(0f, 0f, CS$<>8__locals2.leftRect.width - 16f, num7), true);
			}
			CS$<>8__locals2.curY = 0f;
			for (int l = 0; l < list.Count; l++)
			{
				CharacterCardUtility.LeftRectSection leftRectSection4 = list[l];
				leftRectSection4.drawer(new Rect(0f, CS$<>8__locals2.curY, CS$<>8__locals2.leftRect.width, leftRectSection4.rect.height));
				CS$<>8__locals2.curY += leftRectSection4.calculatedSize;
			}
			if (flag)
			{
				Widgets.EndScrollView();
			}
			GUI.EndGroup();
			GUI.BeginGroup(position2);
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

		// Token: 0x0600750B RID: 29963 RVA: 0x0027F720 File Offset: 0x0027D920
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

		// Token: 0x0600750C RID: 29964 RVA: 0x0027F9E8 File Offset: 0x0027DBE8
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
			if (ModsConfig.IdeologyActive && pawn.Ideo != null)
			{
				Precept_Role role = pawn.Ideo.GetRole(pawn);
				if (role != null && (role.def.roleDisabledWorkTags & workTag) != WorkTags.None)
				{
					list.Add(role);
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

		// Token: 0x0600750D RID: 29965 RVA: 0x0027FC54 File Offset: 0x0027DE54
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

		// Token: 0x0600750E RID: 29966 RVA: 0x0027FCB8 File Offset: 0x0027DEB8
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
				else if (obj is Precept_Role)
				{
					stringBuilder.AppendLine("IncapableOfTooltipRole".Translate((obj as Precept_Role).LabelForPawn(pawn)));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600750F RID: 29967 RVA: 0x0027FE58 File Offset: 0x0027E058
		private static string GetWorkTypesDisabledByWorkTag(WorkTags workTag)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("IncapableOfTooltipWorkTypes".Translate().Colorize(ColoredText.TipSectionTitleColor));
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

		// Token: 0x06007510 RID: 29968 RVA: 0x0027FEE8 File Offset: 0x0027E0E8
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

		// Token: 0x06007511 RID: 29969 RVA: 0x0027FF2C File Offset: 0x0027E12C
		public static void DoNameInputRect(Rect rect, ref string name, int maxLength)
		{
			string text = Widgets.TextField(rect, name);
			if (text.Length <= maxLength && CharacterCardUtility.ValidNameRegex.IsMatch(text))
			{
				name = text;
			}
		}

		// Token: 0x06007512 RID: 29970 RVA: 0x0027FF5B File Offset: 0x0027E15B
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

		// Token: 0x06007513 RID: 29971 RVA: 0x0027FF6C File Offset: 0x0027E16C
		private static Vector2 GetQuestLineSize(string line, Quest quest)
		{
			Vector2 vector = Text.CalcSize(line);
			return new Vector2(17f + vector.x + 10f, Mathf.Max(24f, vector.y));
		}

		// Token: 0x06007514 RID: 29972 RVA: 0x0027FFA8 File Offset: 0x0027E1A8
		private static void DoQuestLine(Rect rect, string line, Quest quest)
		{
			Rect rect2 = rect;
			rect2.xMin += 22f;
			rect2.height = Text.CalcSize(line).y;
			float x = Text.CalcSize(line).x;
			Rect rect3 = new Rect(rect.x, rect.y, Mathf.Min(x, rect2.width) + 24f + -7f + 5f, rect.height);
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

		// Token: 0x0400408E RID: 16526
		private static Vector2 leftRectScrollPos = Vector2.zero;

		// Token: 0x0400408F RID: 16527
		public const int MainRectsY = 100;

		// Token: 0x04004090 RID: 16528
		private const float MainRectsHeight = 355f;

		// Token: 0x04004091 RID: 16529
		private const int ConfigRectTitlesHeight = 40;

		// Token: 0x04004092 RID: 16530
		private const int FactionIconSize = 22;

		// Token: 0x04004093 RID: 16531
		private const int IdeoIconSize = 22;

		// Token: 0x04004094 RID: 16532
		public static Vector2 BasePawnCardSize = new Vector2(480f, 455f);

		// Token: 0x04004095 RID: 16533
		private const int MaxNameLength = 12;

		// Token: 0x04004096 RID: 16534
		public const int MaxNickLength = 16;

		// Token: 0x04004097 RID: 16535
		public const int MaxTitleLength = 25;

		// Token: 0x04004098 RID: 16536
		public const int QuestLineHeight = 20;

		// Token: 0x04004099 RID: 16537
		private static readonly Texture2D QuestIcon = ContentFinder<Texture2D>.Get("UI/Icons/Quest", true);

		// Token: 0x0400409A RID: 16538
		public static readonly Color StackElementBackground = new Color(1f, 1f, 1f, 0.1f);

		// Token: 0x0400409B RID: 16539
		private static List<ExtraFaction> tmpExtraFactions = new List<ExtraFaction>();

		// Token: 0x0400409C RID: 16540
		private static readonly Color TitleCausedWorkTagDisableColor = new Color(0.67f, 0.84f, 0.9f);

		// Token: 0x0400409D RID: 16541
		private static StringBuilder tmpInspectStrings = new StringBuilder();

		// Token: 0x0400409E RID: 16542
		public static Regex ValidNameRegex = new Regex("^[\\p{L}0-9 '\\-.]*$");

		// Token: 0x0400409F RID: 16543
		private const int QuestIconSize = 24;

		// Token: 0x040040A0 RID: 16544
		private const int QuestIconExtraPaddingLeft = -7;

		// Token: 0x0200268D RID: 9869
		private struct LeftRectSection
		{
			// Token: 0x04009287 RID: 37511
			public Rect rect;

			// Token: 0x04009288 RID: 37512
			public Action<Rect> drawer;

			// Token: 0x04009289 RID: 37513
			public float calculatedSize;
		}
	}
}
