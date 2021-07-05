using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AA8 RID: 6824
	[StaticConstructorOnStartup]
	public static class StorytellerUI
	{
		// Token: 0x060096C1 RID: 38593 RVA: 0x00064AA2 File Offset: 0x00062CA2
		public static void ResetStorytellerSelectionInterface()
		{
			StorytellerUI.scrollPosition = default(Vector2);
			StorytellerUI.explanationScrollPosition = default(Vector2);
			StorytellerUI.explanationScrollPositionAnimated = null;
		}

		// Token: 0x060096C2 RID: 38594 RVA: 0x002BE4CC File Offset: 0x002BC6CC
		[Obsolete]
		public static void DrawStorytellerSelectionInterface(Rect rect, ref StorytellerDef chosenStoryteller, ref DifficultyDef difficulty, Listing_Standard infoListing)
		{
			Difficulty difficulty2 = new Difficulty();
			StorytellerUI.DrawStorytellerSelectionInterface_NewTemp(rect, ref chosenStoryteller, ref difficulty, ref difficulty2, infoListing);
		}

		// Token: 0x060096C3 RID: 38595 RVA: 0x002BE4EC File Offset: 0x002BC6EC
		public static void DrawStorytellerSelectionInterface_NewTemp(Rect rect, ref StorytellerDef chosenStoryteller, ref DifficultyDef difficulty, ref Difficulty difficultyValues, Listing_Standard infoListing)
		{
			GUI.BeginGroup(rect);
			Rect outRect = new Rect(0f, 0f, Storyteller.PortraitSizeTiny.x + 16f, rect.height);
			Rect viewRect = new Rect(0f, 0f, Storyteller.PortraitSizeTiny.x, (float)DefDatabase<StorytellerDef>.AllDefs.Count<StorytellerDef>() * (Storyteller.PortraitSizeTiny.y + 10f));
			Widgets.BeginScrollView(outRect, ref StorytellerUI.scrollPosition, viewRect, true);
			Rect rect2 = new Rect(0f, 0f, Storyteller.PortraitSizeTiny.x, Storyteller.PortraitSizeTiny.y);
			foreach (StorytellerDef storytellerDef in from tel in DefDatabase<StorytellerDef>.AllDefs
			orderby tel.listOrder
			select tel)
			{
				if (storytellerDef.listVisible)
				{
					if (Widgets.ButtonImage(rect2, storytellerDef.portraitTinyTex, true))
					{
						TutorSystem.Notify_Event("ChooseStoryteller");
						chosenStoryteller = storytellerDef;
					}
					if (chosenStoryteller == storytellerDef)
					{
						GUI.DrawTexture(rect2, StorytellerUI.StorytellerHighlightTex);
					}
					rect2.y += rect2.height + 8f;
				}
			}
			Widgets.EndScrollView();
			Rect outRect2 = new Rect(outRect.xMax + 8f, 0f, rect.width - outRect.width - 8f, rect.height);
			StorytellerUI.explanationInnerRect.width = outRect2.width - 16f;
			Widgets.BeginScrollView(outRect2, ref StorytellerUI.explanationScrollPosition, StorytellerUI.explanationInnerRect, true);
			Text.Font = GameFont.Small;
			Widgets.Label(new Rect(0f, 0f, 300f, 999f), "HowStorytellersWork".Translate());
			Rect rect3 = new Rect(0f, 120f, 290f, 9999f);
			float num = 300f;
			if (chosenStoryteller != null && chosenStoryteller.listVisible)
			{
				Rect position = new Rect(390f - outRect2.x, rect.height - Storyteller.PortraitSizeLarge.y - 1f, Storyteller.PortraitSizeLarge.x, Storyteller.PortraitSizeLarge.y);
				GUI.DrawTexture(position, chosenStoryteller.portraitLargeTex);
				Text.Anchor = TextAnchor.UpperLeft;
				infoListing.Begin(rect3);
				Text.Font = GameFont.Medium;
				infoListing.Indent(15f);
				infoListing.Label(chosenStoryteller.label, -1f, null);
				infoListing.Outdent(15f);
				Text.Font = GameFont.Small;
				infoListing.Gap(8f);
				infoListing.Label(chosenStoryteller.description, 160f, null);
				infoListing.Gap(6f);
				foreach (DifficultyDef difficultyDef in DefDatabase<DifficultyDef>.AllDefs)
				{
					TaggedString taggedString = difficultyDef.LabelCap;
					if (difficultyDef.isCustom)
					{
						taggedString += "...";
					}
					if (infoListing.RadioButton_NewTemp(taggedString, difficulty == difficultyDef, 0f, difficultyDef.description, new float?(0f)))
					{
						if (!difficultyDef.isCustom)
						{
							difficultyValues.CopyFrom(difficultyDef);
						}
						else if (difficultyDef != difficulty)
						{
							difficultyValues.CopyFrom(DifficultyDefOf.Rough);
							float time = Time.time;
							float num2 = 0.6f;
							StorytellerUI.explanationScrollPositionAnimated = AnimationCurve.EaseInOut(time, StorytellerUI.explanationScrollPosition.y, time + num2, StorytellerUI.explanationInnerRect.height);
						}
						difficulty = difficultyDef;
					}
					infoListing.Gap(3f);
				}
				if (Current.ProgramState == ProgramState.Entry)
				{
					infoListing.Gap(25f);
					bool active = Find.GameInitData.permadeathChosen && Find.GameInitData.permadeath;
					bool active2 = Find.GameInitData.permadeathChosen && !Find.GameInitData.permadeath;
					if (infoListing.RadioButton("ReloadAnytimeMode".Translate(), active2, 0f, "ReloadAnytimeModeInfo".Translate()))
					{
						Find.GameInitData.permadeathChosen = true;
						Find.GameInitData.permadeath = false;
					}
					infoListing.Gap(3f);
					if (infoListing.RadioButton("CommitmentMode".TranslateWithBackup("PermadeathMode"), active, 0f, "PermadeathModeInfo".Translate()))
					{
						Find.GameInitData.permadeathChosen = true;
						Find.GameInitData.permadeath = true;
					}
				}
				num = rect3.y + infoListing.CurHeight;
				infoListing.End();
				if (difficulty != null && difficulty.isCustom)
				{
					if (StorytellerUI.explanationScrollPositionAnimated != null)
					{
						float time2 = Time.time;
						if (time2 < StorytellerUI.explanationScrollPositionAnimated.keys.Last<Keyframe>().time)
						{
							StorytellerUI.explanationScrollPosition.y = StorytellerUI.explanationScrollPositionAnimated.Evaluate(time2);
						}
						else
						{
							StorytellerUI.explanationScrollPositionAnimated = null;
						}
					}
					Listing_Standard listing_Standard = new Listing_Standard();
					float num3 = position.xMax - StorytellerUI.explanationInnerRect.x;
					listing_Standard.ColumnWidth = num3 / 2f - 17f;
					Rect rect4 = new Rect(0f, Math.Max(position.yMax, num) - 45f, num3, 9999f);
					listing_Standard.Begin(rect4);
					Text.Font = GameFont.Medium;
					listing_Standard.Indent(15f);
					listing_Standard.Label("DifficultyCustomSectionLabel".Translate(), -1f, null);
					listing_Standard.Outdent(15f);
					Text.Font = GameFont.Small;
					listing_Standard.Gap(12f);
					if (listing_Standard.ButtonText("DifficultyReset".Translate(), null))
					{
						StorytellerUI.MakeResetDifficultyFloatMenu(difficultyValues);
					}
					float curHeight = listing_Standard.CurHeight;
					float gapHeight = outRect2.height / 2f;
					StorytellerUI.DrawCustomLeft(listing_Standard, difficultyValues);
					listing_Standard.Gap(gapHeight);
					listing_Standard.NewColumn();
					listing_Standard.Gap(curHeight);
					StorytellerUI.DrawCustomRight(listing_Standard, difficultyValues);
					listing_Standard.Gap(gapHeight);
					num = rect4.y + listing_Standard.MaxColumnHeightSeen;
					listing_Standard.End();
				}
			}
			StorytellerUI.explanationInnerRect.height = num;
			Widgets.EndScrollView();
			GUI.EndGroup();
		}

		// Token: 0x060096C4 RID: 38596 RVA: 0x002BEB6C File Offset: 0x002BCD6C
		private static void DrawCustomLeft(Listing_Standard listing, Difficulty difficulty)
		{
			Listing_Standard listing_Standard = StorytellerUI.DrawCustomSectionStart(listing, StorytellerUI.sectionHeightThreats, "DifficultyThreatSection".Translate(), null);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "threatScale", ref difficulty.threatScale, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultyCheckbox(listing_Standard, "allowBigThreats", ref difficulty.allowBigThreats, false, true);
			StorytellerUI.DrawCustomDifficultyCheckbox(listing_Standard, "allowViolentQuests", ref difficulty.allowViolentQuests, false, true);
			StorytellerUI.DrawCustomDifficultyCheckbox(listing_Standard, "allowIntroThreats", ref difficulty.allowIntroThreats, false, true);
			StorytellerUI.DrawCustomDifficultyCheckbox(listing_Standard, "predatorsHuntHumanlikes", ref difficulty.predatorsHuntHumanlikes, false, true);
			StorytellerUI.DrawCustomDifficultyCheckbox(listing_Standard, "allowExtremeWeatherIncidents", ref difficulty.allowExtremeWeatherIncidents, false, true);
			StorytellerUI.DrawCustomSectionEnd(listing, listing_Standard, out StorytellerUI.sectionHeightThreats);
			listing_Standard = StorytellerUI.DrawCustomSectionStart(listing, StorytellerUI.sectionHeightEconomy, "DifficultyEconomySection".Translate(), null);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "cropYieldFactor", ref difficulty.cropYieldFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "mineYieldFactor", ref difficulty.mineYieldFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "butcherYieldFactor", ref difficulty.butcherYieldFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "researchSpeedFactor", ref difficulty.researchSpeedFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "questRewardValueFactor", ref difficulty.questRewardValueFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "raidLootPointsFactor", ref difficulty.raidLootPointsFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "tradePriceFactorLoss", ref difficulty.tradePriceFactorLoss, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 0.5f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "maintenanceCostFactor", ref difficulty.maintenanceCostFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0.01f, 1f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "scariaRotChance", ref difficulty.scariaRotChance, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 1f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "enemyDeathOnDownedChanceFactor", ref difficulty.enemyDeathOnDownedChanceFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 1f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomSectionEnd(listing, listing_Standard, out StorytellerUI.sectionHeightEconomy);
		}

		// Token: 0x060096C5 RID: 38597 RVA: 0x002BEDE0 File Offset: 0x002BCFE0
		private static void DrawCustomRight(Listing_Standard listing, Difficulty difficulty)
		{
			Listing_Standard listing_Standard = StorytellerUI.DrawCustomSectionStart(listing, StorytellerUI.sectionHeightGeneral, "DifficultyGeneralSection".Translate(), null);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "colonistMoodOffset", ref difficulty.colonistMoodOffset, ToStringStyle.Integer, ToStringNumberSense.Offset, -20f, 20f, 1f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "foodPoisonChanceFactor", ref difficulty.foodPoisonChanceFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "manhunterChanceOnDamageFactor", ref difficulty.manhunterChanceOnDamageFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "playerPawnInfectionChanceFactor", ref difficulty.playerPawnInfectionChanceFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "diseaseIntervalFactor", ref difficulty.diseaseIntervalFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, true, 100f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "enemyReproductionRateFactor", ref difficulty.enemyReproductionRateFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "deepDrillInfestationChanceFactor", ref difficulty.deepDrillInfestationChanceFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "friendlyFireChanceFactor", ref difficulty.friendlyFireChanceFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 1f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "allowInstantKillChance", ref difficulty.allowInstantKillChance, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 1f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultyCheckbox(listing_Standard, "peacefulTemples", ref difficulty.peacefulTemples, true, true);
			StorytellerUI.DrawCustomDifficultyCheckbox(listing_Standard, "allowCaveHives", ref difficulty.allowCaveHives, false, true);
			StorytellerUI.DrawCustomSectionEnd(listing, listing_Standard, out StorytellerUI.sectionHeightGeneral);
			listing_Standard = StorytellerUI.DrawCustomSectionStart(listing, StorytellerUI.sectionHeightPlayerTools, "DifficultyPlayerToolsSection".Translate(), null);
			StorytellerUI.DrawCustomDifficultyCheckbox(listing_Standard, "allowTraps", ref difficulty.allowTraps, false, true);
			StorytellerUI.DrawCustomDifficultyCheckbox(listing_Standard, "allowTurrets", ref difficulty.allowTurrets, false, true);
			StorytellerUI.DrawCustomDifficultyCheckbox(listing_Standard, "allowMortars", ref difficulty.allowMortars, false, true);
			StorytellerUI.DrawCustomSectionEnd(listing, listing_Standard, out StorytellerUI.sectionHeightPlayerTools);
			listing_Standard = StorytellerUI.DrawCustomSectionStart(listing, StorytellerUI.sectionHeightAdaptation, "DifficultyAdaptationSection".Translate(), null);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "adaptationGrowthRateFactorOverZero", ref difficulty.adaptationGrowthRateFactorOverZero, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 1f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "adaptationEffectFactor", ref difficulty.adaptationEffectFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 1f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultyCheckbox(listing_Standard, "fixedWealthMode", ref difficulty.fixedWealthMode, false, true);
			GUI.enabled = difficulty.fixedWealthMode;
			float num = Mathf.Round(12f / difficulty.fixedWealthTimeFactor);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "fixedWealthTimeFactor", ref num, ToStringStyle.Integer, ToStringNumberSense.Absolute, 1f, 20f, 1f, false, 1000f);
			difficulty.fixedWealthTimeFactor = 12f / num;
			GUI.enabled = true;
			StorytellerUI.DrawCustomSectionEnd(listing, listing_Standard, out StorytellerUI.sectionHeightAdaptation);
		}

		// Token: 0x060096C6 RID: 38598 RVA: 0x00064AC0 File Offset: 0x00062CC0
		private static Listing_Standard DrawCustomSectionStart(Listing_Standard listing, float height, string label, string tooltip = null)
		{
			listing.Gap(12f);
			listing.Label(label, -1f, tooltip);
			Listing_Standard listing_Standard = listing.BeginSection_NewTemp(height, 8f, 6f);
			listing_Standard.maxOneColumn = true;
			return listing_Standard;
		}

		// Token: 0x060096C7 RID: 38599 RVA: 0x00064AF3 File Offset: 0x00062CF3
		private static void DrawCustomSectionEnd(Listing_Standard listing, Listing_Standard section, out float height)
		{
			listing.EndSection(section);
			height = section.CurHeight;
		}

		// Token: 0x060096C8 RID: 38600 RVA: 0x002BF0E4 File Offset: 0x002BD2E4
		private static void MakeResetDifficultyFloatMenu(Difficulty difficultyValues)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			using (IEnumerator<DifficultyDef> enumerator = DefDatabase<DifficultyDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DifficultyDef d = enumerator.Current;
					if (!d.isCustom)
					{
						list.Add(new FloatMenuOption(d.LabelCap, delegate()
						{
							difficultyValues.CopyFrom(d);
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x060096C9 RID: 38601 RVA: 0x002BF19C File Offset: 0x002BD39C
		private static void DrawCustomDifficultySlider(Listing_Standard listing, string optionName, ref float value, ToStringStyle style, ToStringNumberSense numberSense, float min, float max, float precision = 0.01f, bool reciprocate = false, float reciprocalCutoff = 1000f)
		{
			string str = reciprocate ? "_Inverted" : "";
			string str2 = optionName.CapitalizeFirst();
			string key = "Difficulty_" + str2 + str + "_Label";
			string key2 = "Difficulty_" + str2 + str + "_Info";
			float num = value;
			if (reciprocate)
			{
				num = StorytellerUI.Reciprocal(num, reciprocalCutoff);
			}
			TaggedString label = key.Translate() + ": " + num.ToStringByStyle(style, numberSense);
			listing.Label(label, -1f, key2.Translate());
			float num2 = listing.Slider(num, min, max);
			if (num2 != num)
			{
				num = GenMath.RoundTo(num2, precision);
			}
			if (reciprocate)
			{
				num = StorytellerUI.Reciprocal(num, reciprocalCutoff);
			}
			value = num;
		}

		// Token: 0x060096CA RID: 38602 RVA: 0x002BF258 File Offset: 0x002BD458
		private static void DrawCustomDifficultyCheckbox(Listing_Standard listing, string optionName, ref bool value, bool invert = false, bool showTooltip = true)
		{
			string str = invert ? "_Inverted" : "";
			string str2 = optionName.CapitalizeFirst();
			string key = "Difficulty_" + str2 + str + "_Label";
			string key2 = "Difficulty_" + str2 + str + "_Info";
			bool flag = invert ? (!value) : value;
			listing.CheckboxLabeled(key.Translate(), ref flag, showTooltip ? key2.Translate() : null);
			value = (invert ? (!flag) : flag);
		}

		// Token: 0x060096CB RID: 38603 RVA: 0x00064B04 File Offset: 0x00062D04
		private static float Reciprocal(float f, float cutOff)
		{
			cutOff *= 10f;
			if (Mathf.Abs(f) < 0.01f)
			{
				return cutOff;
			}
			if (f >= 0.99f * cutOff)
			{
				return 0f;
			}
			return 1f / f;
		}

		// Token: 0x04006025 RID: 24613
		private static Vector2 scrollPosition = default(Vector2);

		// Token: 0x04006026 RID: 24614
		private static Vector2 explanationScrollPosition = default(Vector2);

		// Token: 0x04006027 RID: 24615
		private static AnimationCurve explanationScrollPositionAnimated;

		// Token: 0x04006028 RID: 24616
		private static Rect explanationInnerRect = default(Rect);

		// Token: 0x04006029 RID: 24617
		private static float sectionHeightThreats = 0f;

		// Token: 0x0400602A RID: 24618
		private static float sectionHeightGeneral = 0f;

		// Token: 0x0400602B RID: 24619
		private static float sectionHeightPlayerTools = 0f;

		// Token: 0x0400602C RID: 24620
		private static float sectionHeightEconomy = 0f;

		// Token: 0x0400602D RID: 24621
		private static float sectionHeightAdaptation = 0f;

		// Token: 0x0400602E RID: 24622
		private static readonly Texture2D StorytellerHighlightTex = ContentFinder<Texture2D>.Get("UI/HeroArt/Storytellers/Highlight", true);

		// Token: 0x0400602F RID: 24623
		private const float CustomSettingsPrecision = 0.01f;
	}
}
