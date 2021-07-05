using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200117D RID: 4477
	[StaticConstructorOnStartup]
	public class Gizmo_RefuelableFuelStatus : Gizmo
	{
		// Token: 0x06006BB0 RID: 27568 RVA: 0x0022B5AC File Offset: 0x002297AC
		public Gizmo_RefuelableFuelStatus()
		{
			this.order = -100f;
		}

		// Token: 0x06006BB1 RID: 27569 RVA: 0x0022B5BF File Offset: 0x002297BF
		public override float GetWidth(float maxWidth)
		{
			return 140f;
		}

		// Token: 0x06006BB2 RID: 27570 RVA: 0x00242588 File Offset: 0x00240788
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			Rect overRect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Find.WindowStack.ImmediateWindow(1523289473, overRect, WindowLayer.GameUI, delegate
			{
				Rect rect2;
				Rect rect = rect2 = overRect.AtZero().ContractedBy(6f);
				rect2.height = overRect.height / 2f;
				Text.Font = GameFont.Tiny;
				Widgets.Label(rect2, this.refuelable.Props.FuelGizmoLabel);
				Rect rect3 = rect;
				rect3.yMin = overRect.height / 2f;
				float fillPercent = this.refuelable.Fuel / this.refuelable.Props.fuelCapacity;
				Widgets.FillableBar(rect3, fillPercent, Gizmo_RefuelableFuelStatus.FullBarTex, Gizmo_RefuelableFuelStatus.EmptyBarTex, false);
				if (this.refuelable.Props.targetFuelLevelConfigurable)
				{
					float num = this.refuelable.TargetFuelLevel / this.refuelable.Props.fuelCapacity;
					float x = rect3.x + num * rect3.width - (float)Gizmo_RefuelableFuelStatus.TargetLevelArrow.width * 0.5f / 2f;
					float y = rect3.y - (float)Gizmo_RefuelableFuelStatus.TargetLevelArrow.height * 0.5f;
					GUI.DrawTexture(new Rect(x, y, (float)Gizmo_RefuelableFuelStatus.TargetLevelArrow.width * 0.5f, (float)Gizmo_RefuelableFuelStatus.TargetLevelArrow.height * 0.5f), Gizmo_RefuelableFuelStatus.TargetLevelArrow);
				}
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect3, this.refuelable.Fuel.ToString("F0") + " / " + this.refuelable.Props.fuelCapacity.ToString("F0"));
				Text.Anchor = TextAnchor.UpperLeft;
			}, true, false, 1f, null);
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x04003BF5 RID: 15349
		public CompRefuelable refuelable;

		// Token: 0x04003BF6 RID: 15350
		private static readonly Texture2D FullBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.35f, 0.35f, 0.2f));

		// Token: 0x04003BF7 RID: 15351
		private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(Color.black);

		// Token: 0x04003BF8 RID: 15352
		private static readonly Texture2D TargetLevelArrow = ContentFinder<Texture2D>.Get("UI/Misc/BarInstantMarkerRotated", true);

		// Token: 0x04003BF9 RID: 15353
		private const float ArrowScale = 0.5f;
	}
}
