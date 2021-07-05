using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001537 RID: 5431
	public sealed class PlaySettings : IExposable
	{
		// Token: 0x06008122 RID: 33058 RVA: 0x002DAACC File Offset: 0x002D8CCC
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.showLearningHelper, "showLearningHelper", false, false);
			Scribe_Values.Look<bool>(ref this.showZones, "showZones", false, false);
			Scribe_Values.Look<bool>(ref this.showBeauty, "showBeauty", false, false);
			Scribe_Values.Look<bool>(ref this.showRoomStats, "showRoomStats", false, false);
			Scribe_Values.Look<bool>(ref this.showColonistBar, "showColonistBar", false, false);
			Scribe_Values.Look<bool>(ref this.showRoofOverlay, "showRoofOverlay", false, false);
			Scribe_Values.Look<bool>(ref this.showFertilityOverlay, "showFertilityOverlay", false, false);
			Scribe_Values.Look<bool>(ref this.showTerrainAffordanceOverlay, "showTerrainAffordanceOverlay", false, false);
			Scribe_Values.Look<bool>(ref this.autoHomeArea, "autoHomeArea", false, false);
			Scribe_Values.Look<bool>(ref this.autoRebuild, "autoRebuild", false, false);
			Scribe_Values.Look<bool>(ref this.lockNorthUp, "lockNorthUp", false, false);
			Scribe_Values.Look<bool>(ref this.usePlanetDayNightSystem, "usePlanetDayNightSystem", false, false);
			Scribe_Values.Look<bool>(ref this.showExpandingIcons, "showExpandingIcons", false, false);
			Scribe_Values.Look<bool>(ref this.showWorldFeatures, "showWorldFeatures", false, false);
			Scribe_Values.Look<bool>(ref this.useWorkPriorities, "useWorkPriorities", false, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForColonyHumanlike, "defaultCareForHumanlikeColonists", MedicalCareCategory.NoCare, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForColonyAnimal, "defaultCareForAnimalColonists", MedicalCareCategory.NoCare, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForColonyPrisoner, "defaultCareForHumanlikeColonistPrisoners", MedicalCareCategory.NoCare, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForColonySlave, "defaultCareForColonySlave", MedicalCareCategory.HerbalOrWorse, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForNeutralFaction, "defaultCareForHumanlikeNeutrals", MedicalCareCategory.NoCare, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForNeutralAnimal, "defaultCareForAnimalNeutrals", MedicalCareCategory.NoCare, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForHostileFaction, "defaultCareForHumanlikeEnemies", MedicalCareCategory.NoCare, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06008123 RID: 33059 RVA: 0x002DAC6C File Offset: 0x002D8E6C
		public void DoPlaySettingsGlobalControls(WidgetRow row, bool worldView)
		{
			bool flag = this.showColonistBar;
			if (worldView)
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					row.ToggleableIcon(ref this.showColonistBar, TexButton.ShowColonistBar, "ShowColonistBarToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				}
				bool flag2 = this.lockNorthUp;
				row.ToggleableIcon(ref this.lockNorthUp, TexButton.LockNorthUp, "LockNorthUpToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				if (flag2 != this.lockNorthUp && this.lockNorthUp)
				{
					Find.WorldCameraDriver.RotateSoNorthIsUp(true);
				}
				row.ToggleableIcon(ref this.usePlanetDayNightSystem, TexButton.UsePlanetDayNightSystem, "UsePlanetDayNightSystemToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				row.ToggleableIcon(ref this.showExpandingIcons, TexButton.ShowExpandingIcons, "ShowExpandingIconsToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				row.ToggleableIcon(ref this.showWorldFeatures, TexButton.ShowWorldFeatures, "ShowWorldFeaturesToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			}
			else
			{
				row.ToggleableIcon(ref this.showLearningHelper, TexButton.ShowLearningHelper, "ShowLearningHelperWhenEmptyToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				row.ToggleableIcon(ref this.showZones, TexButton.ShowZones, "ZoneVisibilityToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				row.ToggleableIcon(ref this.showBeauty, TexButton.ShowBeauty, "ShowBeautyToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				this.CheckKeyBindingToggle(KeyBindingDefOf.ToggleBeautyDisplay, ref this.showBeauty);
				row.ToggleableIcon(ref this.showRoomStats, TexButton.ShowRoomStats, "ShowRoomStatsToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, "InspectRoomStats");
				this.CheckKeyBindingToggle(KeyBindingDefOf.ToggleRoomStatsDisplay, ref this.showRoomStats);
				row.ToggleableIcon(ref this.showColonistBar, TexButton.ShowColonistBar, "ShowColonistBarToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				row.ToggleableIcon(ref this.showRoofOverlay, TexButton.ShowRoofOverlay, "ShowRoofOverlayToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				row.ToggleableIcon(ref this.showFertilityOverlay, TexButton.ShowFertilityOverlay, "ShowFertilityOverlayToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				row.ToggleableIcon(ref this.showTerrainAffordanceOverlay, TexButton.ShowTerrainAffordanceOverlay, "ShowTerrainAffordanceOverlayToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				row.ToggleableIcon(ref this.autoHomeArea, TexButton.AutoHomeArea, "AutoHomeAreaToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				row.ToggleableIcon(ref this.autoRebuild, TexButton.AutoRebuild, "AutoRebuildButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				bool resourceReadoutCategorized = Prefs.ResourceReadoutCategorized;
				bool flag3 = resourceReadoutCategorized;
				row.ToggleableIcon(ref resourceReadoutCategorized, TexButton.CategorizedResourceReadout, "CategorizedResourceReadoutToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				if (resourceReadoutCategorized != flag3)
				{
					Prefs.ResourceReadoutCategorized = resourceReadoutCategorized;
				}
			}
			if (flag != this.showColonistBar)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
		}

		// Token: 0x06008124 RID: 33060 RVA: 0x002DAF59 File Offset: 0x002D9159
		private void CheckKeyBindingToggle(KeyBindingDef keyBinding, ref bool value)
		{
			if (keyBinding.KeyDownEvent)
			{
				value = !value;
				if (value)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
					return;
				}
				SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x04005093 RID: 20627
		public bool showLearningHelper = true;

		// Token: 0x04005094 RID: 20628
		public bool showZones = true;

		// Token: 0x04005095 RID: 20629
		public bool showBeauty;

		// Token: 0x04005096 RID: 20630
		public bool showRoomStats;

		// Token: 0x04005097 RID: 20631
		public bool showColonistBar = true;

		// Token: 0x04005098 RID: 20632
		public bool showRoofOverlay;

		// Token: 0x04005099 RID: 20633
		public bool showFertilityOverlay;

		// Token: 0x0400509A RID: 20634
		public bool showTerrainAffordanceOverlay;

		// Token: 0x0400509B RID: 20635
		public bool autoHomeArea = true;

		// Token: 0x0400509C RID: 20636
		public bool autoRebuild;

		// Token: 0x0400509D RID: 20637
		public bool lockNorthUp = true;

		// Token: 0x0400509E RID: 20638
		public bool usePlanetDayNightSystem = true;

		// Token: 0x0400509F RID: 20639
		public bool showExpandingIcons = true;

		// Token: 0x040050A0 RID: 20640
		public bool showWorldFeatures = true;

		// Token: 0x040050A1 RID: 20641
		public bool useWorkPriorities;

		// Token: 0x040050A2 RID: 20642
		public MedicalCareCategory defaultCareForColonyHumanlike = MedicalCareCategory.Best;

		// Token: 0x040050A3 RID: 20643
		public MedicalCareCategory defaultCareForColonyAnimal = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x040050A4 RID: 20644
		public MedicalCareCategory defaultCareForColonyPrisoner = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x040050A5 RID: 20645
		public MedicalCareCategory defaultCareForColonySlave = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x040050A6 RID: 20646
		public MedicalCareCategory defaultCareForNeutralFaction = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x040050A7 RID: 20647
		public MedicalCareCategory defaultCareForNeutralAnimal = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x040050A8 RID: 20648
		public MedicalCareCategory defaultCareForHostileFaction = MedicalCareCategory.HerbalOrWorse;
	}
}
