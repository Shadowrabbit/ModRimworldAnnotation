using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000026 RID: 38
	public class SimpleCurveView
	{
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00008364 File Offset: 0x00006564
		public IEnumerable<float> DebugInputValues
		{
			get
			{
				if (this.debugInputValues == null)
				{
					yield break;
				}
				foreach (float num in this.debugInputValues.Values)
				{
					yield return num;
				}
				Dictionary<object, float>.ValueCollection.Enumerator enumerator = default(Dictionary<object, float>.ValueCollection.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00008374 File Offset: 0x00006574
		public void SetDebugInput(object key, float value)
		{
			this.debugInputValues[key] = value;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00008383 File Offset: 0x00006583
		public void ClearDebugInputFrom(object key)
		{
			if (this.debugInputValues.ContainsKey(key))
			{
				this.debugInputValues.Remove(key);
			}
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0007D9A4 File Offset: 0x0007BBA4
		public void SetViewRectAround(SimpleCurve curve)
		{
			if (curve.PointsCount == 0)
			{
				this.rect = SimpleCurveView.identityRect;
				return;
			}
			this.rect.xMin = (from pt in curve.Points
			select pt.Loc.x).Min();
			this.rect.xMax = (from pt in curve.Points
			select pt.Loc.x).Max();
			this.rect.yMin = (from pt in curve.Points
			select pt.Loc.y).Min();
			this.rect.yMax = (from pt in curve.Points
			select pt.Loc.y).Max();
			if (Mathf.Approximately(this.rect.width, 0f))
			{
				this.rect.width = this.rect.xMin * 2f;
			}
			if (Mathf.Approximately(this.rect.height, 0f))
			{
				this.rect.height = this.rect.yMin * 2f;
			}
			if (Mathf.Approximately(this.rect.width, 0f))
			{
				this.rect.width = 1f;
			}
			if (Mathf.Approximately(this.rect.height, 0f))
			{
				this.rect.height = 1f;
			}
			float width = this.rect.width;
			float height = this.rect.height;
			this.rect.xMin = this.rect.xMin - width * 0.1f;
			this.rect.xMax = this.rect.xMax + width * 0.1f;
			this.rect.yMin = this.rect.yMin - height * 0.1f;
			this.rect.yMax = this.rect.yMax + height * 0.1f;
		}

		// Token: 0x04000084 RID: 132
		public Rect rect;

		// Token: 0x04000085 RID: 133
		private Dictionary<object, float> debugInputValues = new Dictionary<object, float>();

		// Token: 0x04000086 RID: 134
		private const float ResetZoomBuffer = 0.1f;

		// Token: 0x04000087 RID: 135
		private static Rect identityRect = new Rect(0f, 0f, 1f, 1f);
	}
}
