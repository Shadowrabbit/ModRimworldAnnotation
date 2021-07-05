using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010C7 RID: 4295
	[StaticConstructorOnStartup]
	public class Gizmo_EnergyShieldStatus : Gizmo
	{
		// Token: 0x060066C0 RID: 26304 RVA: 0x0022B5AC File Offset: 0x002297AC
		public Gizmo_EnergyShieldStatus()
		{
			this.order = -100f;
		}

		// Token: 0x060066C1 RID: 26305 RVA: 0x0022B5BF File Offset: 0x002297BF
		public override float GetWidth(float maxWidth)
		{
			return 140f;
		}

		// Token: 0x060066C2 RID: 26306 RVA: 0x0022B5C8 File Offset: 0x002297C8
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Rect rect2 = rect.ContractedBy(6f);
			Widgets.DrawWindowBackground(rect);
			Rect rect3 = rect2;
			rect3.height = rect.height / 2f;
			Text.Font = GameFont.Tiny;
			Widgets.Label(rect3, this.shield.LabelCap);
			Rect rect4 = rect2;
			rect4.yMin = rect2.y + rect2.height / 2f;
			float fillPercent = this.shield.Energy / Mathf.Max(1f, this.shield.GetStatValue(StatDefOf.EnergyShieldEnergyMax, true));
			Widgets.FillableBar(rect4, fillPercent, Gizmo_EnergyShieldStatus.FullShieldBarTex, Gizmo_EnergyShieldStatus.EmptyShieldBarTex, false);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect4, (this.shield.Energy * 100f).ToString("F0") + " / " + (this.shield.GetStatValue(StatDefOf.EnergyShieldEnergyMax, true) * 100f).ToString("F0"));
			Text.Anchor = TextAnchor.UpperLeft;
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x040039FE RID: 14846
		public ShieldBelt shield;

		// Token: 0x040039FF RID: 14847
		private static readonly Texture2D FullShieldBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.2f, 0.24f));

		// Token: 0x04003A00 RID: 14848
		private static readonly Texture2D EmptyShieldBarTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);
	}
}
