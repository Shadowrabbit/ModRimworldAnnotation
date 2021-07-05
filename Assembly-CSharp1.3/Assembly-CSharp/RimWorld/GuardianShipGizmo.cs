using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B7C RID: 2940
	[StaticConstructorOnStartup]
	public class GuardianShipGizmo : Gizmo
	{
		// Token: 0x060044BC RID: 17596 RVA: 0x0016C4A8 File Offset: 0x0016A6A8
		public GuardianShipGizmo(QuestPart_GuardianShipDelay delay)
		{
			this.delay = delay;
			this.order = -100f;
		}

		// Token: 0x060044BD RID: 17597 RVA: 0x0016C4C2 File Offset: 0x0016A6C2
		public override float GetWidth(float maxWidth)
		{
			return 110f;
		}

		// Token: 0x060044BE RID: 17598 RVA: 0x0016C4CC File Offset: 0x0016A6CC
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Rect rect2 = rect.ContractedBy(6f);
			Widgets.DrawWindowBackground(rect);
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect2, "GuardianShipPayment".Translate(this.delay.TicksLeft.ToStringTicksToPeriod(true, false, true, true)));
			GenUI.ResetLabelAlign();
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x040029B6 RID: 10678
		private QuestPart_GuardianShipDelay delay;
	}
}
