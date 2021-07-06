using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001517 RID: 5399
	[StaticConstructorOnStartup]
	public class PsychicEntropyGizmo : Gizmo
	{
		// Token: 0x060074A3 RID: 29859 RVA: 0x00237C30 File Offset: 0x00235E30
		public PsychicEntropyGizmo(Pawn_PsychicEntropyTracker tracker)
		{
			this.tracker = tracker;
			this.order = -100f;
			this.LimitedTex = ContentFinder<Texture2D>.Get("UI/Icons/EntropyLimit/Limited", true);
			this.UnlimitedTex = ContentFinder<Texture2D>.Get("UI/Icons/EntropyLimit/Unlimited", true);
		}

		// Token: 0x060074A4 RID: 29860 RVA: 0x00237C84 File Offset: 0x00235E84
		private void DrawThreshold(Rect rect, float percent, float entropyValue)
		{
			Rect position = new Rect
			{
				x = rect.x + 3f + (rect.width - 8f) * percent,
				y = rect.y + rect.height - 9f,
				width = 2f,
				height = 6f
			};
			if (entropyValue < percent)
			{
				GUI.DrawTexture(position, BaseContent.GreyTex);
				return;
			}
			GUI.DrawTexture(position, BaseContent.BlackTex);
		}

		// Token: 0x060074A5 RID: 29861 RVA: 0x00237D10 File Offset: 0x00235F10
		private void DrawPsyfocusTarget(Rect rect, float percent)
		{
			float num = Mathf.Round((rect.width - 8f) * percent);
			GUI.DrawTexture(new Rect
			{
				x = rect.x + 3f + num,
				y = rect.y,
				width = 2f,
				height = rect.height
			}, PsychicEntropyGizmo.PsyfocusTargetTex);
			float num2 = Widgets.AdjustCoordToUIScalingFloor(rect.x + 2f + num);
			float xMax = Widgets.AdjustCoordToUIScalingCeil(num2 + 4f);
			Rect rect2 = new Rect
			{
				y = rect.y - 3f,
				height = 5f,
				xMin = num2,
				xMax = xMax
			};
			GUI.DrawTexture(rect2, PsychicEntropyGizmo.PsyfocusTargetTex);
			Rect position = rect2;
			position.y = rect.yMax - 2f;
			GUI.DrawTexture(position, PsychicEntropyGizmo.PsyfocusTargetTex);
		}

		// Token: 0x060074A6 RID: 29862 RVA: 0x00237E0C File Offset: 0x0023600C
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Rect rect2 = rect.ContractedBy(6f);
			MainTabWindow_Inspect mainTabWindow_Inspect = (MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow;
			Command_Psycast command_Psycast = ((mainTabWindow_Inspect != null) ? mainTabWindow_Inspect.LastMouseoverGizmo : null) as Command_Psycast;
			float num = Mathf.Repeat(Time.time, 0.85f);
			float num2 = 1f;
			if (num < 0.1f)
			{
				num2 = num / 0.1f;
			}
			else if (num >= 0.25f)
			{
				num2 = 1f - (num - 0.25f) / 0.6f;
			}
			Widgets.DrawWindowBackground(rect);
			Text.Font = GameFont.Small;
			Rect rect3 = rect2;
			rect3.y += 6f;
			rect3.height = Text.LineHeight;
			Widgets.Label(rect3, "PsychicEntropyShort".Translate());
			Rect rect4 = rect2;
			rect4.y += 38f;
			rect4.height = Text.LineHeight;
			Widgets.Label(rect4, "PsyfocusLabelGizmo".Translate());
			Rect rect5 = rect2;
			rect5.x += 63f;
			rect5.y += 6f;
			rect5.width = 100f;
			rect5.height = 22f;
			float entropyRelativeValue = this.tracker.EntropyRelativeValue;
			Widgets.FillableBar(rect5, Mathf.Min(entropyRelativeValue, 1f), PsychicEntropyGizmo.EntropyBarTex, PsychicEntropyGizmo.EmptyBarTex, true);
			if (this.tracker.EntropyValue > this.tracker.MaxEntropy)
			{
				Widgets.FillableBar(rect5, Mathf.Min(entropyRelativeValue - 1f, 1f), PsychicEntropyGizmo.OverLimitBarTex, PsychicEntropyGizmo.EntropyBarTex, true);
			}
			if (command_Psycast != null)
			{
				Ability ability = command_Psycast.Ability;
				if (ability.def.EntropyGain > 1E-45f)
				{
					Rect rect6 = rect5.ContractedBy(3f);
					float width = rect6.width;
					float num3 = this.tracker.EntropyToRelativeValue(this.tracker.EntropyValue + ability.def.EntropyGain);
					float num4 = entropyRelativeValue;
					if (num4 > 1f)
					{
						num4 -= 1f;
						num3 -= 1f;
					}
					rect6.xMin = Widgets.AdjustCoordToUIScalingFloor(rect6.xMin + num4 * width);
					rect6.width = Widgets.AdjustCoordToUIScalingFloor(Mathf.Max(Mathf.Min(num3, 1f) - num4, 0f) * width);
					GUI.color = new Color(1f, 1f, 1f, num2 * 0.7f);
					GenUI.DrawTextureWithMaterial(rect6, PsychicEntropyGizmo.EntropyBarTexAdd, null, default(Rect));
					GUI.color = Color.white;
				}
			}
			if (this.tracker.EntropyValue > this.tracker.MaxEntropy)
			{
				foreach (KeyValuePair<PsychicEntropySeverity, float> keyValuePair in Pawn_PsychicEntropyTracker.EntropyThresholds)
				{
					if (keyValuePair.Value > 1f && keyValuePair.Value < 2f)
					{
						this.DrawThreshold(rect5, keyValuePair.Value - 1f, entropyRelativeValue);
					}
				}
			}
			string label = this.tracker.EntropyValue.ToString("F0") + " / " + this.tracker.MaxEntropy.ToString("F0");
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect5, label);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Tiny;
			GUI.color = Color.white;
			Rect rect7 = rect2;
			rect7.width = 175f;
			rect7.height = 38f;
			TooltipHandler.TipRegion(rect7, delegate()
			{
				float f = this.tracker.EntropyValue / this.tracker.RecoveryRate;
				return string.Format("PawnTooltipPsychicEntropyStats".Translate(), new object[]
				{
					Mathf.Round(this.tracker.EntropyValue),
					Mathf.Round(this.tracker.MaxEntropy),
					this.tracker.RecoveryRate.ToString("0.#"),
					Mathf.Round(f)
				}) + "\n\n" + "PawnTooltipPsychicEntropyDesc".Translate();
			}, Gen.HashCombineInt(this.tracker.GetHashCode(), 133858));
			Rect rect8 = rect2;
			rect8.x += 63f;
			rect8.y += 38f;
			rect8.width = 100f;
			rect8.height = 22f;
			bool flag = Mouse.IsOver(rect8);
			Widgets.FillableBar(rect8, Mathf.Min(this.tracker.CurrentPsyfocus, 1f), flag ? PsychicEntropyGizmo.PsyfocusBarHighlightTex : PsychicEntropyGizmo.PsyfocusBarTex, PsychicEntropyGizmo.EmptyBarTex, true);
			if (command_Psycast != null)
			{
				float min = command_Psycast.Ability.def.PsyfocusCostRange.min;
				if (min > 1E-45f)
				{
					Rect rect9 = rect8.ContractedBy(3f);
					float num5 = Mathf.Max(this.tracker.CurrentPsyfocus - min, 0f);
					float width2 = rect9.width;
					rect9.xMin = Widgets.AdjustCoordToUIScalingFloor(rect9.xMin + num5 * width2);
					rect9.width = Widgets.AdjustCoordToUIScalingCeil((this.tracker.CurrentPsyfocus - num5) * width2);
					GUI.color = new Color(1f, 1f, 1f, num2);
					GenUI.DrawTextureWithMaterial(rect9, PsychicEntropyGizmo.PsyfocusBarTexReduce, null, default(Rect));
					GUI.color = Color.white;
				}
			}
			for (int i = 1; i < Pawn_PsychicEntropyTracker.PsyfocusBandPercentages.Count - 1; i++)
			{
				this.DrawThreshold(rect8, Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[i], this.tracker.CurrentPsyfocus);
			}
			float num6 = Mathf.Clamp(Mathf.Round((Event.current.mousePosition.x - (rect8.x + 3f)) / (rect8.width - 8f) * 16f) / 16f, 0f, 1f);
			Event current = Event.current;
			if (current.type == EventType.MouseDown && current.button == 0 && flag)
			{
				this.selectedPsyfocusTarget = num6;
				this.draggingPsyfocusBar = true;
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.MeditationDesiredPsyfocus, KnowledgeAmount.Total);
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				current.Use();
			}
			if (current.type == EventType.MouseDrag && current.button == 0 && this.draggingPsyfocusBar && flag)
			{
				if (Math.Abs(num6 - this.selectedPsyfocusTarget) > 1E-45f)
				{
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				}
				this.selectedPsyfocusTarget = num6;
				current.Use();
			}
			if (current.type == EventType.MouseUp && current.button == 0 && this.draggingPsyfocusBar)
			{
				if (this.selectedPsyfocusTarget >= 0f)
				{
					this.tracker.SetPsyfocusTarget(this.selectedPsyfocusTarget);
				}
				this.selectedPsyfocusTarget = -1f;
				this.draggingPsyfocusBar = false;
				current.Use();
			}
			UIHighlighter.HighlightOpportunity(rect8, "PsyfocusBar");
			this.DrawPsyfocusTarget(rect8, this.draggingPsyfocusBar ? this.selectedPsyfocusTarget : this.tracker.TargetPsyfocus);
			GUI.color = Color.white;
			Rect rect10 = rect2;
			rect10.y += 38f;
			rect10.width = 175f;
			rect10.height = 38f;
			TooltipHandler.TipRegion(rect10, () => this.tracker.PsyfocusTipString_NewTemp(this.selectedPsyfocusTarget), Gen.HashCombineInt(this.tracker.GetHashCode(), 133873));
			if (this.tracker.Pawn.IsColonistPlayerControlled)
			{
				float num7 = 32f;
				float num8 = 4f;
				float num9 = rect2.height / 2f - num7 + num8;
				float num10 = rect2.width - num7;
				Rect rect11 = new Rect(rect2.x + num10, rect2.y + num9, num7, num7);
				if (Widgets.ButtonImage(rect11, this.tracker.limitEntropyAmount ? this.LimitedTex : this.UnlimitedTex, true))
				{
					this.tracker.limitEntropyAmount = !this.tracker.limitEntropyAmount;
					if (this.tracker.limitEntropyAmount)
					{
						SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					}
					else
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}
				}
				TooltipHandler.TipRegionByKey(rect11, "PawnTooltipPsychicEntropyLimit");
			}
			if (this.tracker.PainMultiplier > 1f)
			{
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.MiddleCenter;
				string recoveryBonus = (this.tracker.PainMultiplier - 1f).ToStringPercent("F0");
				string recoveryBonus2 = recoveryBonus;
				float widthCached = recoveryBonus2.GetWidthCached();
				Rect rect12 = rect2;
				rect12.x += rect2.width - widthCached / 2f - 16f;
				rect12.y += 38f;
				rect12.width = widthCached;
				rect12.height = Text.LineHeight;
				GUI.color = PsychicEntropyGizmo.PainBoostColor;
				Widgets.Label(rect12, recoveryBonus2);
				GUI.color = Color.white;
				Text.Font = GameFont.Tiny;
				Text.Anchor = TextAnchor.UpperLeft;
				TooltipHandler.TipRegion(rect12.ContractedBy(-1f), () => "PawnTooltipPsychicEntropyPainFocus".Translate(this.tracker.Pawn.health.hediffSet.PainTotal.ToStringPercent("F0"), recoveryBonus), Gen.HashCombineInt(this.tracker.GetHashCode(), 133878));
			}
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x060074A7 RID: 29863 RVA: 0x0004EC7F File Offset: 0x0004CE7F
		public override float GetWidth(float maxWidth)
		{
			return 212f;
		}

		// Token: 0x04004CE6 RID: 19686
		private Pawn_PsychicEntropyTracker tracker;

		// Token: 0x04004CE7 RID: 19687
		private float selectedPsyfocusTarget = -1f;

		// Token: 0x04004CE8 RID: 19688
		private bool draggingPsyfocusBar;

		// Token: 0x04004CE9 RID: 19689
		private Texture2D LimitedTex;

		// Token: 0x04004CEA RID: 19690
		private Texture2D UnlimitedTex;

		// Token: 0x04004CEB RID: 19691
		private const string LimitedIconPath = "UI/Icons/EntropyLimit/Limited";

		// Token: 0x04004CEC RID: 19692
		private const string UnlimitedIconPath = "UI/Icons/EntropyLimit/Unlimited";

		// Token: 0x04004CED RID: 19693
		private const float CostPreviewFadeIn = 0.1f;

		// Token: 0x04004CEE RID: 19694
		private const float CostPreviewSolid = 0.15f;

		// Token: 0x04004CEF RID: 19695
		private const float CostPreviewFadeOut = 0.6f;

		// Token: 0x04004CF0 RID: 19696
		private static readonly Color PainBoostColor = new Color(0.2f, 0.65f, 0.35f);

		// Token: 0x04004CF1 RID: 19697
		private static readonly Texture2D EntropyBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.46f, 0.34f, 0.35f));

		// Token: 0x04004CF2 RID: 19698
		private static readonly Texture2D EntropyBarTexAdd = SolidColorMaterials.NewSolidColorTexture(new Color(0.78f, 0.72f, 0.66f));

		// Token: 0x04004CF3 RID: 19699
		private static readonly Texture2D OverLimitBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.75f, 0.2f, 0.15f));

		// Token: 0x04004CF4 RID: 19700
		private static readonly Texture2D PsyfocusBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.34f, 0.42f, 0.43f));

		// Token: 0x04004CF5 RID: 19701
		private static readonly Texture2D PsyfocusBarTexReduce = SolidColorMaterials.NewSolidColorTexture(new Color(0.65f, 0.83f, 0.83f));

		// Token: 0x04004CF6 RID: 19702
		private static readonly Texture2D PsyfocusBarHighlightTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.43f, 0.54f, 0.55f));

		// Token: 0x04004CF7 RID: 19703
		private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.03f, 0.035f, 0.05f));

		// Token: 0x04004CF8 RID: 19704
		private static readonly Texture2D PsyfocusTargetTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.74f, 0.97f, 0.8f));
	}
}
