using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001A5A RID: 6746
	public static class SocialCardUtility
	{
		// Token: 0x060094B4 RID: 38068 RVA: 0x002B2214 File Offset: 0x002B0414
		public static void DrawSocialCard(Rect rect, Pawn pawn)
		{
			GUI.BeginGroup(rect);
			Text.Font = GameFont.Small;
			float num = Prefs.DevMode ? 20f : 15f;
			Rect rect2 = new Rect(0f, num, rect.width, rect.height - num).ContractedBy(10f);
			Rect rect3 = rect2;
			Rect rect4 = rect2;
			rect3.height *= 0.63f;
			rect4.y = rect3.yMax + 17f;
			rect4.yMax = rect2.yMax;
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			Widgets.DrawLineHorizontal(0f, (rect3.yMax + rect4.y) / 2f, rect.width);
			GUI.color = Color.white;
			if (Prefs.DevMode && !pawn.Dead)
			{
				SocialCardUtility.DrawDebugOptions(new Rect(5f, 5f, rect.width, 22f), pawn);
			}
			SocialCardUtility.DrawRelationsAndOpinions(rect3, pawn);
			InteractionCardUtility.DrawInteractionsLog(rect4, pawn, Find.PlayLog.AllEntries, 12);
			GUI.EndGroup();
		}

		// Token: 0x060094B5 RID: 38069 RVA: 0x000635CB File Offset: 0x000617CB
		private static void CheckRecache(Pawn selPawnForSocialInfo)
		{
			if (SocialCardUtility.cachedForPawn != selPawnForSocialInfo || Time.frameCount % 20 == 0)
			{
				SocialCardUtility.Recache(selPawnForSocialInfo);
			}
		}

		// Token: 0x060094B6 RID: 38070 RVA: 0x002B2340 File Offset: 0x002B0540
		private static void Recache(Pawn selPawnForSocialInfo)
		{
			SocialCardUtility.cachedForPawn = selPawnForSocialInfo;
			SocialCardUtility.tmpToCache.Clear();
			foreach (Pawn pawn in selPawnForSocialInfo.relations.RelatedPawns)
			{
				if (SocialCardUtility.ShouldShowPawnRelations(pawn, selPawnForSocialInfo))
				{
					SocialCardUtility.RecacheEntry(pawn, selPawnForSocialInfo, null, null);
					SocialCardUtility.tmpToCache.Add(pawn);
				}
			}
			List<Pawn> list = null;
			if (selPawnForSocialInfo.MapHeld != null)
			{
				list = selPawnForSocialInfo.MapHeld.mapPawns.AllPawns;
			}
			else if (selPawnForSocialInfo.IsCaravanMember())
			{
				list = selPawnForSocialInfo.GetCaravan().PawnsListForReading;
			}
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					Pawn pawn2 = list[i];
					if (pawn2.RaceProps.Humanlike && pawn2 != selPawnForSocialInfo && SocialCardUtility.ShouldShowPawnRelations(pawn2, selPawnForSocialInfo) && !SocialCardUtility.tmpToCache.Contains(pawn2) && (pawn2.relations.OpinionOf(selPawnForSocialInfo) != 0 || selPawnForSocialInfo.relations.OpinionOf(pawn2) != 0))
					{
						SocialCardUtility.RecacheEntry(pawn2, selPawnForSocialInfo, null, null);
						SocialCardUtility.tmpToCache.Add(pawn2);
					}
				}
			}
			SocialCardUtility.cachedEntries.RemoveAll((SocialCardUtility.CachedSocialTabEntry x) => !SocialCardUtility.tmpToCache.Contains(x.otherPawn));
			SocialCardUtility.cachedEntries.Sort(SocialCardUtility.CachedEntriesComparer);
		}

		// Token: 0x060094B7 RID: 38071 RVA: 0x002B24D0 File Offset: 0x002B06D0
		private static bool ShouldShowPawnRelations(Pawn pawn, Pawn selPawnForSocialInfo)
		{
			return SocialCardUtility.showAllRelations || ((!pawn.RaceProps.Animal || !pawn.Dead || pawn.Corpse != null) && pawn.Name != null && !pawn.Name.Numerical && pawn.relations.everSeenByPlayer);
		}

		// Token: 0x060094B8 RID: 38072 RVA: 0x002B252C File Offset: 0x002B072C
		private static void RecacheEntry(Pawn pawn, Pawn selPawnForSocialInfo, int? opinionOfMe = null, int? opinionOfOtherPawn = null)
		{
			bool flag = false;
			foreach (SocialCardUtility.CachedSocialTabEntry cachedSocialTabEntry in SocialCardUtility.cachedEntries)
			{
				if (cachedSocialTabEntry.otherPawn == pawn)
				{
					SocialCardUtility.RecacheEntryInt(cachedSocialTabEntry, selPawnForSocialInfo, opinionOfMe, opinionOfOtherPawn);
					flag = true;
				}
			}
			if (flag)
			{
				return;
			}
			SocialCardUtility.CachedSocialTabEntry cachedSocialTabEntry2 = new SocialCardUtility.CachedSocialTabEntry();
			cachedSocialTabEntry2.otherPawn = pawn;
			SocialCardUtility.RecacheEntryInt(cachedSocialTabEntry2, selPawnForSocialInfo, opinionOfMe, opinionOfOtherPawn);
			SocialCardUtility.cachedEntries.Add(cachedSocialTabEntry2);
		}

		// Token: 0x060094B9 RID: 38073 RVA: 0x002B25B4 File Offset: 0x002B07B4
		private static void RecacheEntryInt(SocialCardUtility.CachedSocialTabEntry entry, Pawn selPawnForSocialInfo, int? opinionOfMe = null, int? opinionOfOtherPawn = null)
		{
			entry.opinionOfMe = ((opinionOfMe != null) ? opinionOfMe.Value : entry.otherPawn.relations.OpinionOf(selPawnForSocialInfo));
			entry.opinionOfOtherPawn = ((opinionOfOtherPawn != null) ? opinionOfOtherPawn.Value : selPawnForSocialInfo.relations.OpinionOf(entry.otherPawn));
			entry.relations.Clear();
			foreach (PawnRelationDef item in selPawnForSocialInfo.GetRelations(entry.otherPawn))
			{
				entry.relations.Add(item);
			}
			entry.relations.Sort((PawnRelationDef a, PawnRelationDef b) => b.importance.CompareTo(a.importance));
		}

		// Token: 0x060094BA RID: 38074 RVA: 0x002B2694 File Offset: 0x002B0894
		public static void DrawRelationsAndOpinions(Rect rect, Pawn selPawnForSocialInfo)
		{
			SocialCardUtility.CheckRecache(selPawnForSocialInfo);
			if (Current.ProgramState != ProgramState.Playing)
			{
				SocialCardUtility.showAllRelations = false;
			}
			GUI.BeginGroup(rect);
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			Rect outRect = new Rect(0f, 0f, rect.width, rect.height);
			Rect viewRect = new Rect(0f, 0f, rect.width - 16f, SocialCardUtility.listScrollViewHeight);
			Rect rect2 = rect;
			if (viewRect.height > outRect.height)
			{
				rect2.width -= 16f;
			}
			Widgets.BeginScrollView(outRect, ref SocialCardUtility.listScrollPosition, viewRect, true);
			float num = 0f;
			float y = SocialCardUtility.listScrollPosition.y;
			float num2 = SocialCardUtility.listScrollPosition.y + outRect.height;
			for (int i = 0; i < SocialCardUtility.cachedEntries.Count; i++)
			{
				float rowHeight = SocialCardUtility.GetRowHeight(SocialCardUtility.cachedEntries[i], rect2.width, selPawnForSocialInfo);
				if (num > y - rowHeight && num < num2)
				{
					SocialCardUtility.DrawPawnRow(num, rect2.width, SocialCardUtility.cachedEntries[i], selPawnForSocialInfo);
				}
				num += rowHeight;
			}
			if (!SocialCardUtility.cachedEntries.Any<SocialCardUtility.CachedSocialTabEntry>())
			{
				GUI.color = Color.gray;
				Text.Anchor = TextAnchor.UpperCenter;
				Widgets.Label(new Rect(0f, 0f, rect2.width, 30f), "NoRelationships".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
			}
			if (Event.current.type == EventType.Layout)
			{
				SocialCardUtility.listScrollViewHeight = num + 30f;
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
			GUI.color = Color.white;
		}

		// Token: 0x060094BB RID: 38075 RVA: 0x002B2840 File Offset: 0x002B0A40
		private static void DrawPawnRow(float y, float width, SocialCardUtility.CachedSocialTabEntry entry, Pawn selPawnForSocialInfo)
		{
			float rowHeight = SocialCardUtility.GetRowHeight(entry, width, selPawnForSocialInfo);
			Rect rect = new Rect(0f, y, width, rowHeight);
			Pawn otherPawn = entry.otherPawn;
			if (Mouse.IsOver(rect))
			{
				GUI.color = SocialCardUtility.HighlightColor;
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, () => SocialCardUtility.GetPawnRowTooltip(entry, selPawnForSocialInfo), entry.otherPawn.thingIDNumber * 13 + selPawnForSocialInfo.thingIDNumber);
			}
			if (Widgets.ButtonInvisible(rect, true))
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					if (otherPawn.Dead)
					{
						Messages.Message("MessageCantSelectDeadPawn".Translate(otherPawn.LabelShort, otherPawn).CapitalizeFirst(), MessageTypeDefOf.RejectInput, false);
					}
					else if (otherPawn.SpawnedOrAnyParentSpawned || otherPawn.IsCaravanMember())
					{
						CameraJumper.TryJumpAndSelect(otherPawn);
					}
					else
					{
						Messages.Message("MessageCantSelectOffMapPawn".Translate(otherPawn.LabelShort, otherPawn).CapitalizeFirst(), MessageTypeDefOf.RejectInput, false);
					}
				}
				else if (Find.GameInitData.startingAndOptionalPawns.Contains(otherPawn))
				{
					Page_ConfigureStartingPawns page_ConfigureStartingPawns = Find.WindowStack.WindowOfType<Page_ConfigureStartingPawns>();
					if (page_ConfigureStartingPawns != null)
					{
						page_ConfigureStartingPawns.SelectPawn(otherPawn);
						SoundDefOf.RowTabSelect.PlayOneShotOnCamera(null);
					}
				}
			}
			float width2;
			float width3;
			float width4;
			float width5;
			float width6;
			SocialCardUtility.CalculateColumnsWidths(width, out width2, out width3, out width4, out width5, out width6);
			Rect rect2 = new Rect(5f, y + 3f, width2, rowHeight - 3f);
			SocialCardUtility.DrawRelationLabel(entry, rect2, selPawnForSocialInfo);
			Rect rect3 = new Rect(rect2.xMax, y + 3f, width3, rowHeight - 3f);
			SocialCardUtility.DrawPawnLabel(otherPawn, rect3);
			Rect rect4 = new Rect(rect3.xMax, y + 3f, width4, rowHeight - 3f);
			SocialCardUtility.DrawMyOpinion(entry, rect4, selPawnForSocialInfo);
			Rect rect5 = new Rect(rect4.xMax, y + 3f, width5, rowHeight - 3f);
			SocialCardUtility.DrawHisOpinion(entry, rect5, selPawnForSocialInfo);
			Rect rect6 = new Rect(rect5.xMax, y + 3f, width6, rowHeight - 3f);
			SocialCardUtility.DrawPawnSituationLabel(entry.otherPawn, rect6, selPawnForSocialInfo);
		}

		// Token: 0x060094BC RID: 38076 RVA: 0x002B2AC4 File Offset: 0x002B0CC4
		private static float GetRowHeight(SocialCardUtility.CachedSocialTabEntry entry, float rowWidth, Pawn selPawnForSocialInfo)
		{
			float width;
			float width2;
			float num;
			float num2;
			float num3;
			SocialCardUtility.CalculateColumnsWidths(rowWidth, out width, out width2, out num, out num2, out num3);
			return Mathf.Max(Mathf.Max(0f, Text.CalcHeight(SocialCardUtility.GetRelationsString(entry, selPawnForSocialInfo), width)), Text.CalcHeight(SocialCardUtility.GetPawnLabel(entry.otherPawn), width2)) + 3f;
		}

		// Token: 0x060094BD RID: 38077 RVA: 0x002B2B14 File Offset: 0x002B0D14
		private static void CalculateColumnsWidths(float rowWidth, out float relationsWidth, out float pawnLabelWidth, out float myOpinionWidth, out float hisOpinionWidth, out float pawnSituationLabelWidth)
		{
			float num = rowWidth - 10f;
			relationsWidth = num * 0.23f;
			pawnLabelWidth = num * 0.41f;
			myOpinionWidth = num * 0.075f;
			hisOpinionWidth = num * 0.085f;
			pawnSituationLabelWidth = num * 0.2f;
			if (myOpinionWidth < 25f)
			{
				pawnLabelWidth -= 25f - myOpinionWidth;
				myOpinionWidth = 25f;
			}
			if (hisOpinionWidth < 35f)
			{
				pawnLabelWidth -= 35f - hisOpinionWidth;
				hisOpinionWidth = 35f;
			}
		}

		// Token: 0x060094BE RID: 38078 RVA: 0x002B2B98 File Offset: 0x002B0D98
		private static void DrawRelationLabel(SocialCardUtility.CachedSocialTabEntry entry, Rect rect, Pawn selPawnForSocialInfo)
		{
			string relationsString = SocialCardUtility.GetRelationsString(entry, selPawnForSocialInfo);
			if (!relationsString.NullOrEmpty())
			{
				GUI.color = SocialCardUtility.RelationLabelColor;
				Widgets.Label(rect, relationsString);
			}
		}

		// Token: 0x060094BF RID: 38079 RVA: 0x000635E5 File Offset: 0x000617E5
		private static void DrawPawnLabel(Pawn pawn, Rect rect)
		{
			GUI.color = SocialCardUtility.PawnLabelColor;
			Widgets.Label(rect, SocialCardUtility.GetPawnLabel(pawn));
		}

		// Token: 0x060094C0 RID: 38080 RVA: 0x002B2BC8 File Offset: 0x002B0DC8
		private static void DrawMyOpinion(SocialCardUtility.CachedSocialTabEntry entry, Rect rect, Pawn selPawnForSocialInfo)
		{
			if (!entry.otherPawn.RaceProps.Humanlike || !selPawnForSocialInfo.RaceProps.Humanlike)
			{
				return;
			}
			int opinionOfOtherPawn = entry.opinionOfOtherPawn;
			GUI.color = SocialCardUtility.OpinionLabelColor(opinionOfOtherPawn);
			Widgets.Label(rect, opinionOfOtherPawn.ToStringWithSign());
		}

		// Token: 0x060094C1 RID: 38081 RVA: 0x002B2C14 File Offset: 0x002B0E14
		private static void DrawHisOpinion(SocialCardUtility.CachedSocialTabEntry entry, Rect rect, Pawn selPawnForSocialInfo)
		{
			if (!entry.otherPawn.RaceProps.Humanlike || !selPawnForSocialInfo.RaceProps.Humanlike)
			{
				return;
			}
			int opinionOfMe = entry.opinionOfMe;
			Color color = SocialCardUtility.OpinionLabelColor(opinionOfMe);
			GUI.color = new Color(color.r, color.g, color.b, 0.4f);
			Widgets.Label(rect, "(" + opinionOfMe.ToStringWithSign() + ")");
		}

		// Token: 0x060094C2 RID: 38082 RVA: 0x002B2C8C File Offset: 0x002B0E8C
		private static void DrawPawnSituationLabel(Pawn pawn, Rect rect, Pawn selPawnForSocialInfo)
		{
			GUI.color = Color.gray;
			string label = SocialCardUtility.GetPawnSituationLabel(pawn, selPawnForSocialInfo).Truncate(rect.width, null);
			Widgets.Label(rect, label);
		}

		// Token: 0x060094C3 RID: 38083 RVA: 0x000635FD File Offset: 0x000617FD
		private static Color OpinionLabelColor(int opinion)
		{
			if (Mathf.Abs(opinion) < 10)
			{
				return Color.gray;
			}
			if (opinion < 0)
			{
				return ColoredText.RedReadable;
			}
			return Color.green;
		}

		// Token: 0x060094C4 RID: 38084 RVA: 0x0006361E File Offset: 0x0006181E
		private static string GetPawnLabel(Pawn pawn)
		{
			if (pawn.Name != null)
			{
				return pawn.Name.ToStringFull;
			}
			return pawn.LabelCapNoCount;
		}

		// Token: 0x060094C5 RID: 38085 RVA: 0x002B2CC0 File Offset: 0x002B0EC0
		public static string GetPawnSituationLabel(Pawn pawn, Pawn fromPOV)
		{
			if (pawn.Dead)
			{
				return "Dead".Translate();
			}
			if (pawn.Destroyed)
			{
				return "Missing".Translate();
			}
			if (PawnUtility.IsKidnappedPawn(pawn))
			{
				return "Kidnapped".Translate();
			}
			QuestPart_LendColonistsToFaction questPart_LendColonistsToFaction = QuestUtility.GetAllQuestPartsOfType<QuestPart_LendColonistsToFaction>(true).FirstOrDefault((QuestPart_LendColonistsToFaction p) => p.LentColonistsListForReading.Contains(pawn));
			if (questPart_LendColonistsToFaction != null)
			{
				return "Lent".Translate(questPart_LendColonistsToFaction.lendColonistsToFaction.Named("FACTION"), questPart_LendColonistsToFaction.returnLentColonistsInTicks.ToStringTicksToDays("0.0"));
			}
			if (pawn.kindDef == PawnKindDefOf.Slave)
			{
				return "Slave".Translate();
			}
			if (PawnUtility.IsFactionLeader(pawn))
			{
				return "FactionLeader".Translate();
			}
			Faction faction = pawn.Faction;
			if (faction == fromPOV.Faction)
			{
				return "";
			}
			if (faction == null || fromPOV.Faction == null)
			{
				return "Neutral".Translate();
			}
			switch (faction.RelationKindWith(fromPOV.Faction))
			{
			case FactionRelationKind.Hostile:
				return "Hostile".Translate() + ", " + faction.Name;
			case FactionRelationKind.Neutral:
				return "Neutral".Translate() + ", " + faction.Name;
			case FactionRelationKind.Ally:
				return "Ally".Translate() + ", " + faction.Name;
			default:
				return "";
			}
		}

		// Token: 0x060094C6 RID: 38086 RVA: 0x002B2E90 File Offset: 0x002B1090
		private static string GetRelationsString(SocialCardUtility.CachedSocialTabEntry entry, Pawn selPawnForSocialInfo)
		{
			string text = "";
			if (entry.relations.Count != 0)
			{
				for (int i = 0; i < entry.relations.Count; i++)
				{
					PawnRelationDef pawnRelationDef = entry.relations[i];
					if (!text.NullOrEmpty())
					{
						text = text + ", " + pawnRelationDef.GetGenderSpecificLabel(entry.otherPawn);
					}
					else
					{
						text = pawnRelationDef.GetGenderSpecificLabelCap(entry.otherPawn);
					}
				}
				return text;
			}
			if (entry.opinionOfOtherPawn < -20)
			{
				return "Rival".Translate();
			}
			if (entry.opinionOfOtherPawn > 20)
			{
				return "Friend".Translate();
			}
			return "Acquaintance".Translate();
		}

		// Token: 0x060094C7 RID: 38087 RVA: 0x002B2F48 File Offset: 0x002B1148
		private static string GetPawnRowTooltip(SocialCardUtility.CachedSocialTabEntry entry, Pawn selPawnForSocialInfo)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (entry.otherPawn.RaceProps.Humanlike && selPawnForSocialInfo.RaceProps.Humanlike)
			{
				stringBuilder.AppendLine(selPawnForSocialInfo.relations.OpinionExplanation(entry.otherPawn));
				stringBuilder.AppendLine();
				stringBuilder.Append("SomeonesOpinionOfMe".Translate(entry.otherPawn.LabelShort, entry.otherPawn));
				stringBuilder.Append(": ");
				stringBuilder.Append(entry.opinionOfMe.ToStringWithSign());
			}
			else
			{
				stringBuilder.AppendLine(entry.otherPawn.LabelCapNoCount);
				string pawnSituationLabel = SocialCardUtility.GetPawnSituationLabel(entry.otherPawn, selPawnForSocialInfo);
				if (!pawnSituationLabel.NullOrEmpty())
				{
					stringBuilder.AppendLine(pawnSituationLabel);
				}
				stringBuilder.AppendLine("--------------");
				stringBuilder.Append(SocialCardUtility.GetRelationsString(entry, selPawnForSocialInfo));
			}
			if (Prefs.DevMode)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("(debug) Compatibility: " + selPawnForSocialInfo.relations.CompatibilityWith(entry.otherPawn).ToString("F2"));
				stringBuilder.Append("(debug) RomanceChanceFactor: " + selPawnForSocialInfo.relations.SecondaryRomanceChanceFactor(entry.otherPawn).ToString("F2"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060094C8 RID: 38088 RVA: 0x002B30A8 File Offset: 0x002B12A8
		private static void DrawDebugOptions(Rect rect, Pawn pawn)
		{
			GUI.BeginGroup(rect);
			Widgets.CheckboxLabeled(new Rect(0f, 0f, 145f, 22f), "Dev: AllRelations", ref SocialCardUtility.showAllRelations, false, null, null, false);
			if (Widgets.ButtonText(new Rect(150f, 0f, 115f, 22f), "Debug info", true, true, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				Func<Pawn, bool> <>9__5;
				Func<Pawn, float> <>9__6;
				list.Add(new FloatMenuOption("RomanceChance", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("My gender: " + pawn.gender);
					stringBuilder.AppendLine("My age: " + pawn.ageTracker.AgeBiologicalYears);
					stringBuilder.AppendLine();
					IEnumerable<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
					Func<Pawn, bool> predicate;
					if ((predicate = <>9__5) == null)
					{
						predicate = (<>9__5 = ((Pawn x) => x.def == pawn.def));
					}
					IEnumerable<Pawn> source = allPawnsSpawned.Where(predicate);
					Func<Pawn, float> keySelector;
					if ((keySelector = <>9__6) == null)
					{
						keySelector = (<>9__6 = ((Pawn x) => pawn.relations.SecondaryRomanceChanceFactor(x)));
					}
					foreach (Pawn pawn2 in source.OrderByDescending(keySelector))
					{
						if (pawn2 != pawn)
						{
							stringBuilder.AppendLine(string.Concat(new object[]
							{
								pawn2.LabelShort,
								" (",
								pawn2.gender,
								", age: ",
								pawn2.ageTracker.AgeBiologicalYears,
								", compat: ",
								pawn.relations.CompatibilityWith(pawn2).ToString("F2"),
								"): ",
								pawn.relations.SecondaryRomanceChanceFactor(pawn2).ToStringPercent("F0"),
								"        [vs ",
								pawn2.relations.SecondaryRomanceChanceFactor(pawn).ToStringPercent("F0"),
								"]"
							}));
						}
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				Func<Pawn, bool> <>9__7;
				Func<Pawn, float> <>9__8;
				list.Add(new FloatMenuOption("CompatibilityTo", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("My age: " + pawn.ageTracker.AgeBiologicalYears);
					stringBuilder.AppendLine();
					IEnumerable<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
					Func<Pawn, bool> predicate;
					if ((predicate = <>9__7) == null)
					{
						predicate = (<>9__7 = ((Pawn x) => x.def == pawn.def));
					}
					IEnumerable<Pawn> source = allPawnsSpawned.Where(predicate);
					Func<Pawn, float> keySelector;
					if ((keySelector = <>9__8) == null)
					{
						keySelector = (<>9__8 = ((Pawn x) => pawn.relations.CompatibilityWith(x)));
					}
					foreach (Pawn pawn2 in source.OrderByDescending(keySelector))
					{
						if (pawn2 != pawn)
						{
							stringBuilder.AppendLine(string.Concat(new object[]
							{
								pawn2.LabelShort,
								" (",
								pawn2.KindLabel,
								", age: ",
								pawn2.ageTracker.AgeBiologicalYears,
								"): ",
								pawn.relations.CompatibilityWith(pawn2).ToString("0.##")
							}));
						}
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				if (pawn.RaceProps.Humanlike)
				{
					list.Add(new FloatMenuOption("Interaction chance", delegate()
					{
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.AppendLine("(selected pawn is the initiator)");
						stringBuilder.AppendLine("(\"fight chance\" is the chance that the receiver will start social fight)");
						stringBuilder.AppendLine("Interaction chance (real chance, not just weights):");
						using (IEnumerator<Pawn> enumerator = (from x in pawn.Map.mapPawns.AllPawnsSpawned
						where x.RaceProps.Humanlike
						select x).OrderBy(delegate(Pawn x)
						{
							if (x.Faction != null)
							{
								return x.Faction.loadID;
							}
							return -1;
						}).GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Pawn c = enumerator.Current;
								if (c != pawn)
								{
									stringBuilder.AppendLine();
									stringBuilder.AppendLine(string.Concat(new object[]
									{
										c.LabelShort,
										" (",
										c.KindLabel,
										", ",
										c.gender,
										", age: ",
										c.ageTracker.AgeBiologicalYears,
										", compat: ",
										pawn.relations.CompatibilityWith(c).ToString("F2"),
										", romCh: ",
										pawn.relations.SecondaryRomanceChanceFactor(c).ToStringPercent("F0"),
										"):"
									}));
									List<InteractionDef> list2 = (from x in DefDatabase<InteractionDef>.AllDefs
									where x.Worker.RandomSelectionWeight(pawn, c) > 0f
									orderby x.Worker.RandomSelectionWeight(pawn, c) descending
									select x).ToList<InteractionDef>();
									float num = list2.Sum((InteractionDef x) => x.Worker.RandomSelectionWeight(pawn, c));
									foreach (InteractionDef interactionDef in list2)
									{
										float f = c.interactions.SocialFightChance(interactionDef, pawn);
										float f2 = interactionDef.Worker.RandomSelectionWeight(pawn, c) / num;
										stringBuilder.AppendLine(string.Concat(new string[]
										{
											"  ",
											interactionDef.defName,
											": ",
											f2.ToStringPercent(),
											" (fight chance: ",
											f.ToStringPercent("F2"),
											")"
										}));
										if (interactionDef == InteractionDefOf.RomanceAttempt)
										{
											stringBuilder.AppendLine("    success chance: " + ((InteractionWorker_RomanceAttempt)interactionDef.Worker).SuccessChance(pawn, c).ToStringPercent());
										}
										else if (interactionDef == InteractionDefOf.MarriageProposal)
										{
											stringBuilder.AppendLine("    acceptance chance: " + ((InteractionWorker_MarriageProposal)interactionDef.Worker).AcceptanceChance(pawn, c).ToStringPercent());
										}
									}
								}
							}
						}
						Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
					Func<Pawn, bool> <>9__14;
					Func<Pawn, float> <>9__15;
					list.Add(new FloatMenuOption("Lovin' MTB", delegate()
					{
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.AppendLine("Lovin' MTB hours with pawn X.");
						stringBuilder.AppendLine("Assuming both pawns are in bed and are partners.");
						stringBuilder.AppendLine();
						IEnumerable<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
						Func<Pawn, bool> predicate;
						if ((predicate = <>9__14) == null)
						{
							predicate = (<>9__14 = ((Pawn x) => x.def == pawn.def));
						}
						IEnumerable<Pawn> source = allPawnsSpawned.Where(predicate);
						Func<Pawn, float> keySelector;
						if ((keySelector = <>9__15) == null)
						{
							keySelector = (<>9__15 = ((Pawn x) => pawn.relations.SecondaryRomanceChanceFactor(x)));
						}
						foreach (Pawn pawn2 in source.OrderByDescending(keySelector))
						{
							if (pawn2 != pawn)
							{
								stringBuilder.AppendLine(string.Concat(new object[]
								{
									pawn2.LabelShort,
									" (",
									pawn2.KindLabel,
									", age: ",
									pawn2.ageTracker.AgeBiologicalYears,
									"): ",
									LovePartnerRelationUtility.GetLovinMtbHours(pawn, pawn2).ToString("F1"),
									" h"
								}));
							}
						}
						Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				list.Add(new FloatMenuOption("Test per pawns pair compatibility factor probability", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					int num = 0;
					int num2 = 0;
					int num3 = 0;
					int num4 = 0;
					int num5 = 0;
					int num6 = 0;
					int num7 = 0;
					int num8 = 0;
					float num9 = -999999f;
					float num10 = 999999f;
					for (int i = 0; i < 10000; i++)
					{
						int otherPawnID = Rand.RangeInclusive(0, 30000);
						float num11 = pawn.relations.ConstantPerPawnsPairCompatibilityOffset(otherPawnID);
						if (num11 < -3f)
						{
							num++;
						}
						else if (num11 < -2f)
						{
							num2++;
						}
						else if (num11 < -1f)
						{
							num3++;
						}
						else if (num11 < 0f)
						{
							num4++;
						}
						else if (num11 < 1f)
						{
							num5++;
						}
						else if (num11 < 2f)
						{
							num6++;
						}
						else if (num11 < 3f)
						{
							num7++;
						}
						else
						{
							num8++;
						}
						if (num11 > num9)
						{
							num9 = num11;
						}
						else if (num11 < num10)
						{
							num10 = num11;
						}
					}
					stringBuilder.AppendLine("< -3: " + ((float)num / 10000f).ToStringPercent("F2"));
					stringBuilder.AppendLine("< -2: " + ((float)num2 / 10000f).ToStringPercent("F2"));
					stringBuilder.AppendLine("< -1: " + ((float)num3 / 10000f).ToStringPercent("F2"));
					stringBuilder.AppendLine("< 0: " + ((float)num4 / 10000f).ToStringPercent("F2"));
					stringBuilder.AppendLine("< 1: " + ((float)num5 / 10000f).ToStringPercent("F2"));
					stringBuilder.AppendLine("< 2: " + ((float)num6 / 10000f).ToStringPercent("F2"));
					stringBuilder.AppendLine("< 3: " + ((float)num7 / 10000f).ToStringPercent("F2"));
					stringBuilder.AppendLine("> 3: " + ((float)num8 / 10000f).ToStringPercent("F2"));
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("trials: " + 10000);
					stringBuilder.AppendLine("min: " + num10);
					stringBuilder.AppendLine("max: " + num9);
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				Find.WindowStack.Add(new FloatMenu(list));
			}
			GUI.EndGroup();
		}

		// Token: 0x04005E91 RID: 24209
		private static Vector2 listScrollPosition = Vector2.zero;

		// Token: 0x04005E92 RID: 24210
		private static float listScrollViewHeight = 0f;

		// Token: 0x04005E93 RID: 24211
		private static bool showAllRelations;

		// Token: 0x04005E94 RID: 24212
		private static List<SocialCardUtility.CachedSocialTabEntry> cachedEntries = new List<SocialCardUtility.CachedSocialTabEntry>();

		// Token: 0x04005E95 RID: 24213
		private static Pawn cachedForPawn;

		// Token: 0x04005E96 RID: 24214
		private const float TopPadding = 15f;

		// Token: 0x04005E97 RID: 24215
		private const float TopPaddingDevMode = 20f;

		// Token: 0x04005E98 RID: 24216
		private static readonly Color RelationLabelColor = new Color(0.6f, 0.6f, 0.6f);

		// Token: 0x04005E99 RID: 24217
		private static readonly Color PawnLabelColor = new Color(0.9f, 0.9f, 0.9f, 1f);

		// Token: 0x04005E9A RID: 24218
		private static readonly Color HighlightColor = new Color(0.5f, 0.5f, 0.5f, 1f);

		// Token: 0x04005E9B RID: 24219
		private const float RowTopPadding = 3f;

		// Token: 0x04005E9C RID: 24220
		private const float RowLeftRightPadding = 5f;

		// Token: 0x04005E9D RID: 24221
		private static SocialCardUtility.CachedSocialTabEntryComparer CachedEntriesComparer = new SocialCardUtility.CachedSocialTabEntryComparer();

		// Token: 0x04005E9E RID: 24222
		private static HashSet<Pawn> tmpToCache = new HashSet<Pawn>();

		// Token: 0x02001A5B RID: 6747
		private class CachedSocialTabEntry
		{
			// Token: 0x04005E9F RID: 24223
			public Pawn otherPawn;

			// Token: 0x04005EA0 RID: 24224
			public int opinionOfOtherPawn;

			// Token: 0x04005EA1 RID: 24225
			public int opinionOfMe;

			// Token: 0x04005EA2 RID: 24226
			public List<PawnRelationDef> relations = new List<PawnRelationDef>();
		}

		// Token: 0x02001A5C RID: 6748
		private class CachedSocialTabEntryComparer : IComparer<SocialCardUtility.CachedSocialTabEntry>
		{
			// Token: 0x060094CB RID: 38091 RVA: 0x002B32A0 File Offset: 0x002B14A0
			public int Compare(SocialCardUtility.CachedSocialTabEntry a, SocialCardUtility.CachedSocialTabEntry b)
			{
				bool flag = a.relations.Any<PawnRelationDef>();
				bool flag2 = b.relations.Any<PawnRelationDef>();
				if (flag != flag2)
				{
					return flag2.CompareTo(flag);
				}
				if (flag && flag2)
				{
					float num = float.MinValue;
					for (int i = 0; i < a.relations.Count; i++)
					{
						if (a.relations[i].importance > num)
						{
							num = a.relations[i].importance;
						}
					}
					float num2 = float.MinValue;
					for (int j = 0; j < b.relations.Count; j++)
					{
						if (b.relations[j].importance > num2)
						{
							num2 = b.relations[j].importance;
						}
					}
					if (num != num2)
					{
						return num2.CompareTo(num);
					}
				}
				if (a.opinionOfOtherPawn != b.opinionOfOtherPawn)
				{
					return b.opinionOfOtherPawn.CompareTo(a.opinionOfOtherPawn);
				}
				return a.otherPawn.thingIDNumber.CompareTo(b.otherPawn.thingIDNumber);
			}
		}
	}
}
