using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020011C9 RID: 4553
	[StaticConstructorOnStartup]
	public class Gizmo_PruningConfig : Gizmo
	{
		// Token: 0x06006DE8 RID: 28136 RVA: 0x0024D601 File Offset: 0x0024B801
		public Gizmo_PruningConfig(CompTreeConnection connection)
		{
			this.connection = connection;
			this.order = -100f;
		}

		// Token: 0x06006DE9 RID: 28137 RVA: 0x001D303A File Offset: 0x001D123A
		public override float GetWidth(float maxWidth)
		{
			return 212f;
		}

		// Token: 0x06006DEA RID: 28138 RVA: 0x0024D628 File Offset: 0x0024B828
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return new GizmoResult(GizmoState.Clear);
			}
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Rect rect2 = rect.ContractedBy(6f);
			Widgets.DrawWindowBackground(rect);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperCenter;
			Rect rect3 = rect2;
			rect3.y += 6f;
			rect3.height = Text.LineHeight;
			Widgets.Label(rect3, "DesiredConnectionStrength".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			if (Mouse.IsOver(rect3))
			{
				Widgets.DrawHighlight(rect3);
				TooltipHandler.TipRegion(rect3, "DesiredConnectionStrengthDesc".Translate(this.connection.parent.Named("TREE"), this.connection.ConnectedPawn.Named("CONNECTEDPAWN")));
			}
			this.DrawBar(rect2);
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x06006DEB RID: 28139 RVA: 0x0024D710 File Offset: 0x0024B910
		private void DrawThreshold(Rect rect, float percent, float strValue)
		{
			Rect position = new Rect
			{
				x = rect.x + 3f + (rect.width - 8f) * percent,
				y = rect.y + rect.height - 9f,
				width = 2f,
				height = 6f
			};
			if (strValue < percent)
			{
				GUI.DrawTexture(position, BaseContent.GreyTex);
				return;
			}
			GUI.DrawTexture(position, BaseContent.BlackTex);
		}

		// Token: 0x06006DEC RID: 28140 RVA: 0x0024D79C File Offset: 0x0024B99C
		private void DrawStrengthTarget(Rect rect, float percent)
		{
			float num = Mathf.Round((rect.width - 8f) * percent);
			GUI.DrawTexture(new Rect(rect.x + 3f + num, rect.y, 2f, rect.height), Gizmo_PruningConfig.StrengthTargetTex);
			float num2 = Widgets.AdjustCoordToUIScalingFloor(rect.x + 2f + num);
			float xMax = Widgets.AdjustCoordToUIScalingCeil(num2 + 4f);
			Rect rect2 = new Rect
			{
				y = rect.y - 3f,
				height = 5f,
				xMin = num2,
				xMax = xMax
			};
			GUI.DrawTexture(rect2, Gizmo_PruningConfig.StrengthTargetTex);
			Rect position = rect2;
			position.y = rect.yMax - 2f;
			GUI.DrawTexture(position, Gizmo_PruningConfig.StrengthTargetTex);
		}

		// Token: 0x06006DED RID: 28141 RVA: 0x0024D878 File Offset: 0x0024BA78
		private void DrawBar(Rect inRect)
		{
			Rect rect = inRect;
			rect.xMin += 10f;
			rect.xMax -= 10f;
			rect.yMax = inRect.yMax - 4f;
			rect.yMin = rect.yMax - 22f;
			bool flag = Mouse.IsOver(rect);
			float connectionStrength = this.connection.ConnectionStrength;
			Widgets.FillableBar(rect, connectionStrength, flag ? Gizmo_PruningConfig.StrengthHighlightTex : Gizmo_PruningConfig.StrengthTex, Gizmo_PruningConfig.EmptyBarTex, true);
			foreach (CurvePoint curvePoint in this.connection.Props.maxDryadsPerConnectionStrengthCurve.Points)
			{
				if (curvePoint.x > 0f)
				{
					this.DrawThreshold(rect, curvePoint.x, connectionStrength);
				}
			}
			float num = Mathf.Clamp(Mathf.Round((Event.current.mousePosition.x - (rect.x + 3f)) / (rect.width - 8f) * 20f) / 20f, 0f, 1f);
			Event current = Event.current;
			if (current.type == EventType.MouseDown && current.button == 0 && flag)
			{
				this.selectedStrengthTarget = num;
				this.draggingBar = true;
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				current.Use();
			}
			if (current.type == EventType.MouseDrag && current.button == 0 && this.draggingBar && flag)
			{
				if (Mathf.Abs(num - this.selectedStrengthTarget) > 1E-45f)
				{
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				}
				this.selectedStrengthTarget = num;
				current.Use();
			}
			if (current.type == EventType.MouseUp && current.button == 0 && this.draggingBar)
			{
				if (this.selectedStrengthTarget >= 0f)
				{
					this.connection.DesiredConnectionStrength = this.selectedStrengthTarget;
				}
				this.selectedStrengthTarget = -1f;
				this.draggingBar = false;
				current.Use();
			}
			float num2 = this.draggingBar ? this.selectedStrengthTarget : this.connection.DesiredConnectionStrength;
			this.DrawStrengthTarget(rect, num2);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, num2.ToStringPercent());
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
		}

		// Token: 0x04003D07 RID: 15623
		private CompTreeConnection connection;

		// Token: 0x04003D08 RID: 15624
		private float selectedStrengthTarget = -1f;

		// Token: 0x04003D09 RID: 15625
		private bool draggingBar;

		// Token: 0x04003D0A RID: 15626
		private const float Width = 212f;

		// Token: 0x04003D0B RID: 15627
		private const float BarHeight = 22f;

		// Token: 0x04003D0C RID: 15628
		private static readonly Texture2D StrengthTex = SolidColorMaterials.NewSolidColorTexture(ColorLibrary.Orange);

		// Token: 0x04003D0D RID: 15629
		private static readonly Texture2D StrengthHighlightTex = SolidColorMaterials.NewSolidColorTexture(ColorLibrary.LightOrange);

		// Token: 0x04003D0E RID: 15630
		private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.03f, 0.035f, 0.05f));

		// Token: 0x04003D0F RID: 15631
		private static readonly Texture2D StrengthTargetTex = SolidColorMaterials.NewSolidColorTexture(ColorLibrary.DarkOrange);
	}
}
