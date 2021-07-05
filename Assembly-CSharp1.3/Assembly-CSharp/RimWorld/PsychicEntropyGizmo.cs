using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000E72 RID: 3698
	[StaticConstructorOnStartup]
	public class PsychicEntropyGizmo : Gizmo
	{
		// Token: 0x06005618 RID: 22040 RVA: 0x001D24F4 File Offset: 0x001D06F4
		public PsychicEntropyGizmo(Pawn_PsychicEntropyTracker tracker)
		{
			this.tracker = tracker;
			this.order = -100f;
			this.LimitedTex = ContentFinder<Texture2D>.Get("UI/Icons/EntropyLimit/Limited", true);
			this.UnlimitedTex = ContentFinder<Texture2D>.Get("UI/Icons/EntropyLimit/Unlimited", true);
		}

		// Token: 0x06005619 RID: 22041 RVA: 0x001D2548 File Offset: 0x001D0748
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

		// Token: 0x0600561A RID: 22042 RVA: 0x001D25D4 File Offset: 0x001D07D4
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

		// Token: 0x0600561B RID: 22043 RVA: 0x001D26D0 File Offset: 0x001D08D0
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
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
			TooltipHandler.TipRegion(rect10, () => this.tracker.PsyfocusTipString(this.selectedPsyfocusTarget), Gen.HashCombineInt(this.tracker.GetHashCode(), 133873));
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
			float num11;
			if (this.TryGetPainMultiplier(this.tracker.Pawn, out num11))
			{
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.MiddleCenter;
				string recoveryBonus = (num11 - 1f).ToStringPercent("F0");
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

		// Token: 0x0600561C RID: 22044 RVA: 0x001D2FE4 File Offset: 0x001D11E4
		private bool TryGetPainMultiplier(Pawn pawn, out float painMultiplier)
		{
			List<StatPart> parts = StatDefOf.PsychicEntropyRecoveryRate.parts;
			for (int i = 0; i < parts.Count; i++)
			{
				StatPart_Pain statPart_Pain;
				if ((statPart_Pain = (parts[i] as StatPart_Pain)) != null)
				{
					painMultiplier = statPart_Pain.PainFactor(this.tracker.Pawn);
					return true;
				}
			}
			painMultiplier = 0f;
			return false;
		}

		// Token: 0x0600561D RID: 22045 RVA: 0x001D303A File Offset: 0x001D123A
		public override float GetWidth(float maxWidth)
		{
			return 212f;
		}

		// Token: 0x040032E1 RID: 13025
		private Pawn_PsychicEntropyTracker tracker;

		// Token: 0x040032E2 RID: 13026
		private float selectedPsyfocusTarget = -1f;

		// Token: 0x040032E3 RID: 13027
		private bool draggingPsyfocusBar;

		// Token: 0x040032E4 RID: 13028
		private Texture2D LimitedTex;

		// Token: 0x040032E5 RID: 13029
		private Texture2D UnlimitedTex;

		// Token: 0x040032E6 RID: 13030
		private const string LimitedIconPath = "UI/Icons/EntropyLimit/Limited";

		// Token: 0x040032E7 RID: 13031
		private const string UnlimitedIconPath = "UI/Icons/EntropyLimit/Unlimited";

		// Token: 0x040032E8 RID: 13032
		private const float CostPreviewFadeIn = 0.1f;

		// Token: 0x040032E9 RID: 13033
		private const float CostPreviewSolid = 0.15f;

		// Token: 0x040032EA RID: 13034
		private const float CostPreviewFadeOut = 0.6f;

		// Token: 0x040032EB RID: 13035
		private static readonly Color PainBoostColor = new Color(0.2f, 0.65f, 0.35f);

		// Token: 0x040032EC RID: 13036
		private static readonly Texture2D EntropyBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.46f, 0.34f, 0.35f));

		// Token: 0x040032ED RID: 13037
		private static readonly Texture2D EntropyBarTexAdd = SolidColorMaterials.NewSolidColorTexture(new Color(0.78f, 0.72f, 0.66f));

		// Token: 0x040032EE RID: 13038
		private static readonly Texture2D OverLimitBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.75f, 0.2f, 0.15f));

		// Token: 0x040032EF RID: 13039
		private static readonly Texture2D PsyfocusBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.34f, 0.42f, 0.43f));

		// Token: 0x040032F0 RID: 13040
		private static readonly Texture2D PsyfocusBarTexReduce = SolidColorMaterials.NewSolidColorTexture(new Color(0.65f, 0.83f, 0.83f));

		// Token: 0x040032F1 RID: 13041
		private static readonly Texture2D PsyfocusBarHighlightTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.43f, 0.54f, 0.55f));

		// Token: 0x040032F2 RID: 13042
		private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.03f, 0.035f, 0.05f));

		// Token: 0x040032F3 RID: 13043
		private static readonly Texture2D PsyfocusTargetTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.74f, 0.97f, 0.8f));
	}
}
