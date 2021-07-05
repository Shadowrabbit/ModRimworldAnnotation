using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017BA RID: 6074
	public class Gizmo_CaravanInfo : Gizmo
	{
		// Token: 0x06008CF8 RID: 36088 RVA: 0x0032B9C4 File Offset: 0x00329BC4
		public Gizmo_CaravanInfo(Caravan caravan)
		{
			this.caravan = caravan;
			this.order = -100f;
		}

		// Token: 0x06008CF9 RID: 36089 RVA: 0x0032B9DE File Offset: 0x00329BDE
		public override float GetWidth(float maxWidth)
		{
			return Mathf.Min(520f, maxWidth);
		}

		// Token: 0x06008CFA RID: 36090 RVA: 0x0032B9EC File Offset: 0x00329BEC
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			if (!this.caravan.Spawned)
			{
				return new GizmoResult(GizmoState.Clear);
			}
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Widgets.DrawWindowBackground(rect);
			GUI.BeginGroup(rect);
			Rect rect2 = rect.AtZero();
			int? ticksToArrive = this.caravan.pather.Moving ? new int?(CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this.caravan, true)) : null;
			StringBuilder stringBuilder = new StringBuilder();
			float tilesPerDay = TilesPerDayCalculator.ApproxTilesPerDay(this.caravan, stringBuilder);
			CaravanUIUtility.DrawCaravanInfo(new CaravanUIUtility.CaravanInfo(this.caravan.MassUsage, this.caravan.MassCapacity, this.caravan.MassCapacityExplanation, tilesPerDay, stringBuilder.ToString(), this.caravan.DaysWorthOfFood, this.caravan.forage.ForagedFoodPerDay, this.caravan.forage.ForagedFoodPerDayExplanation, this.caravan.Visibility, this.caravan.VisibilityExplanation, -1f, -1f, null), null, this.caravan.Tile, ticksToArrive, -9999f, rect2, true, null, true);
			GUI.EndGroup();
			GenUI.AbsorbClicksInRect(rect);
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x04005968 RID: 22888
		private Caravan caravan;
	}
}
