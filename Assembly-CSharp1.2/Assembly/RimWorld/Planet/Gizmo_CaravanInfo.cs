using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002110 RID: 8464
	public class Gizmo_CaravanInfo : Gizmo
	{
		// Token: 0x0600B3C6 RID: 46022 RVA: 0x00074CF2 File Offset: 0x00072EF2
		public Gizmo_CaravanInfo(Caravan caravan)
		{
			this.caravan = caravan;
			this.order = -100f;
		}

		// Token: 0x0600B3C7 RID: 46023 RVA: 0x00074D0C File Offset: 0x00072F0C
		public override float GetWidth(float maxWidth)
		{
			return Mathf.Min(520f, maxWidth);
		}

		// Token: 0x0600B3C8 RID: 46024 RVA: 0x00343238 File Offset: 0x00341438
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
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

		// Token: 0x04007B91 RID: 31633
		private Caravan caravan;
	}
}
