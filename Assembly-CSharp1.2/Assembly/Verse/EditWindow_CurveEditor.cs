using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006D8 RID: 1752
	public class EditWindow_CurveEditor : EditWindow
	{
		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x06002CF2 RID: 11506 RVA: 0x000238A2 File Offset: 0x00021AA2
		private bool DraggingView
		{
			get
			{
				return this.draggingButton >= 0;
			}
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06002CF3 RID: 11507 RVA: 0x000238B0 File Offset: 0x00021AB0
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(600f, 400f);
			}
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x06002CF4 RID: 11508 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002CF5 RID: 11509 RVA: 0x000238C1 File Offset: 0x00021AC1
		public EditWindow_CurveEditor(SimpleCurve curve, string title)
		{
			this.curve = curve;
			this.optionalTitle = title;
		}

		// Token: 0x06002CF6 RID: 11510 RVA: 0x00131270 File Offset: 0x0012F470
		public override void DoWindowContents(Rect inRect)
		{
			WidgetRow widgetRow = new WidgetRow(0f, 0f, UIDirection.RightThenUp, 99999f, 4f);
			if (widgetRow.ButtonIcon(TexButton.CenterOnPointsTex, "Center view around points.", null, true))
			{
				this.curve.View.SetViewRectAround(this.curve);
			}
			if (widgetRow.ButtonIcon(TexButton.CurveResetTex, "Reset to growth from 0 to 1.", null, true))
			{
				List<CurvePoint> list = new List<CurvePoint>();
				list.Add(new CurvePoint(0f, 0f));
				list.Add(new CurvePoint(1f, 1f));
				this.curve.SetPoints(list);
				this.curve.View.SetViewRectAround(this.curve);
			}
			if (widgetRow.ButtonIcon(TexButton.QuickZoomHor1Tex, "Reset horizontal zoom to 0-1", null, true))
			{
				this.curve.View.rect.xMin = 0f;
				this.curve.View.rect.xMax = 1f;
			}
			if (widgetRow.ButtonIcon(TexButton.QuickZoomHor100Tex, "Reset horizontal zoom to 0-100", null, true))
			{
				this.curve.View.rect.xMin = 0f;
				this.curve.View.rect.xMax = 100f;
			}
			if (widgetRow.ButtonIcon(TexButton.QuickZoomHor20kTex, "Reset horizontal zoom to 0-20,000", null, true))
			{
				this.curve.View.rect.xMin = 0f;
				this.curve.View.rect.xMax = 20000f;
			}
			if (widgetRow.ButtonIcon(TexButton.QuickZoomVer1Tex, "Reset vertical zoom to 0-1", null, true))
			{
				this.curve.View.rect.yMin = 0f;
				this.curve.View.rect.yMax = 1f;
			}
			if (widgetRow.ButtonIcon(TexButton.QuickZoomVer100Tex, "Reset vertical zoom to 0-100", null, true))
			{
				this.curve.View.rect.yMin = 0f;
				this.curve.View.rect.yMax = 100f;
			}
			if (widgetRow.ButtonIcon(TexButton.QuickZoomVer20kTex, "Reset vertical zoom to 0-20,000", null, true))
			{
				this.curve.View.rect.yMin = 0f;
				this.curve.View.rect.yMax = 20000f;
			}
			Rect screenRect = new Rect(inRect.AtZero());
			screenRect.yMin += 26f;
			screenRect.yMax -= 24f;
			this.DoCurveEditor(screenRect);
		}

		// Token: 0x06002CF7 RID: 11511 RVA: 0x00131550 File Offset: 0x0012F750
		private void DoCurveEditor(Rect screenRect)
		{
			Widgets.DrawMenuSection(screenRect);
			SimpleCurveDrawer.DrawCurve(screenRect, this.curve, null, null, default(Rect));
			Vector2 mousePosition = Event.current.mousePosition;
			if (Mouse.IsOver(screenRect))
			{
				Rect rect = new Rect(mousePosition.x + 8f, mousePosition.y + 18f, 100f, 100f);
				Vector2 v = SimpleCurveDrawer.ScreenToCurveCoords(screenRect, this.curve.View.rect, mousePosition);
				Widgets.Label(rect, v.ToStringTwoDigits());
			}
			Rect rect2 = new Rect(0f, 0f, 50f, 24f)
			{
				x = screenRect.x,
				y = screenRect.y + screenRect.height / 2f - 12f
			};
			float num;
			if (float.TryParse(Widgets.TextField(rect2, this.curve.View.rect.x.ToString()), out num))
			{
				this.curve.View.rect.x = num;
			}
			rect2.x = screenRect.xMax - rect2.width;
			rect2.y = screenRect.y + screenRect.height / 2f - 12f;
			if (float.TryParse(Widgets.TextField(rect2, this.curve.View.rect.xMax.ToString()), out num))
			{
				this.curve.View.rect.xMax = num;
			}
			rect2.x = screenRect.x + screenRect.width / 2f - rect2.width / 2f;
			rect2.y = screenRect.yMax - rect2.height;
			if (float.TryParse(Widgets.TextField(rect2, this.curve.View.rect.y.ToString()), out num))
			{
				this.curve.View.rect.y = num;
			}
			rect2.x = screenRect.x + screenRect.width / 2f - rect2.width / 2f;
			rect2.y = screenRect.y;
			if (float.TryParse(Widgets.TextField(rect2, this.curve.View.rect.yMax.ToString()), out num))
			{
				this.curve.View.rect.yMax = num;
			}
			if (Mouse.IsOver(screenRect))
			{
				if (Event.current.type == EventType.ScrollWheel)
				{
					float num2 = -1f * Event.current.delta.y * 0.025f;
					float num3 = this.curve.View.rect.center.x - this.curve.View.rect.x;
					float num4 = this.curve.View.rect.center.y - this.curve.View.rect.y;
					SimpleCurveView view = this.curve.View;
					view.rect.xMin = view.rect.xMin + num3 * num2;
					SimpleCurveView view2 = this.curve.View;
					view2.rect.xMax = view2.rect.xMax - num3 * num2;
					SimpleCurveView view3 = this.curve.View;
					view3.rect.yMin = view3.rect.yMin + num4 * num2;
					SimpleCurveView view4 = this.curve.View;
					view4.rect.yMax = view4.rect.yMax - num4 * num2;
					Event.current.Use();
				}
				if (Event.current.type == EventType.MouseDown && (Event.current.button == 0 || Event.current.button == 2))
				{
					List<int> list = this.PointsNearMouse(screenRect).ToList<int>();
					if (list.Any<int>())
					{
						this.draggingPointIndex = list[0];
					}
					else
					{
						this.draggingPointIndex = -1;
					}
					if (this.draggingPointIndex < 0)
					{
						this.draggingButton = Event.current.button;
					}
					Event.current.Use();
				}
				if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
				{
					Vector2 mouseCurveCoords = SimpleCurveDrawer.ScreenToCurveCoords(screenRect, this.curve.View.rect, Event.current.mousePosition);
					List<FloatMenuOption> list2 = new List<FloatMenuOption>();
					list2.Add(new FloatMenuOption("Add point at " + mouseCurveCoords.ToString(), delegate()
					{
						this.curve.Add(new CurvePoint(mouseCurveCoords), true);
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
					foreach (int i in this.PointsNearMouse(screenRect))
					{
						CurvePoint point = this.curve[i];
						list2.Add(new FloatMenuOption("Remove point at " + point.ToString(), delegate()
						{
							this.curve.RemovePointNear(point);
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					Find.WindowStack.Add(new FloatMenu(list2));
					Event.current.Use();
				}
			}
			if (this.draggingPointIndex >= 0)
			{
				this.curve[this.draggingPointIndex] = new CurvePoint(SimpleCurveDrawer.ScreenToCurveCoords(screenRect, this.curve.View.rect, Event.current.mousePosition));
				this.curve.SortPoints();
				if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
				{
					this.draggingPointIndex = -1;
					Event.current.Use();
				}
			}
			if (this.DraggingView)
			{
				if (Event.current.type == EventType.MouseDrag)
				{
					Vector2 delta = Event.current.delta;
					SimpleCurveView view5 = this.curve.View;
					view5.rect.x = view5.rect.x - delta.x * this.curve.View.rect.width * 0.002f;
					SimpleCurveView view6 = this.curve.View;
					view6.rect.y = view6.rect.y + delta.y * this.curve.View.rect.height * 0.002f;
					Event.current.Use();
				}
				if (Event.current.type == EventType.MouseUp && Event.current.button == this.draggingButton)
				{
					this.draggingButton = -1;
				}
			}
		}

		// Token: 0x06002CF8 RID: 11512 RVA: 0x000238E5 File Offset: 0x00021AE5
		private IEnumerable<int> PointsNearMouse(Rect screenRect)
		{
			GUI.BeginGroup(screenRect);
			try
			{
				int num;
				for (int i = 0; i < this.curve.PointsCount; i = num + 1)
				{
					if ((SimpleCurveDrawer.CurveToScreenCoordsInsideScreenRect(screenRect, this.curve.View.rect, this.curve[i].Loc) - Event.current.mousePosition).sqrMagnitude < 49f)
					{
						yield return i;
					}
					num = i;
				}
			}
			finally
			{
				GUI.EndGroup();
			}
			yield break;
			yield break;
		}

		// Token: 0x04001E6D RID: 7789
		private SimpleCurve curve;

		// Token: 0x04001E6E RID: 7790
		public List<float> debugInputValues;

		// Token: 0x04001E6F RID: 7791
		private int draggingPointIndex = -1;

		// Token: 0x04001E70 RID: 7792
		private int draggingButton = -1;

		// Token: 0x04001E71 RID: 7793
		private const float ViewDragPanSpeed = 0.002f;

		// Token: 0x04001E72 RID: 7794
		private const float ScrollZoomSpeed = 0.025f;

		// Token: 0x04001E73 RID: 7795
		private const float PointClickDistanceLimit = 7f;
	}
}
