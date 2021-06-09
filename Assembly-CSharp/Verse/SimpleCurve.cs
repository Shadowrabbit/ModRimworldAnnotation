using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000022 RID: 34
	public class SimpleCurve : IEnumerable<CurvePoint>, IEnumerable
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600019C RID: 412 RVA: 0x000081CA File Offset: 0x000063CA
		public int PointsCount
		{
			get
			{
				return this.points.Count;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600019D RID: 413 RVA: 0x000081D7 File Offset: 0x000063D7
		public List<CurvePoint> Points
		{
			get
			{
				return this.points;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600019E RID: 414 RVA: 0x000081DF File Offset: 0x000063DF
		public bool HasView
		{
			get
			{
				return this.view != null;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600019F RID: 415 RVA: 0x000081EA File Offset: 0x000063EA
		public SimpleCurveView View
		{
			get
			{
				if (this.view == null)
				{
					this.view = new SimpleCurveView();
					this.view.SetViewRectAround(this);
				}
				return this.view;
			}
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00008211 File Offset: 0x00006411
		public SimpleCurve(IEnumerable<CurvePoint> points)
		{
			this.SetPoints(points);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000822B File Offset: 0x0000642B
		public SimpleCurve()
		{
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000823E File Offset: 0x0000643E
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00008246 File Offset: 0x00006446
		public IEnumerator<CurvePoint> GetEnumerator()
		{
			foreach (CurvePoint curvePoint in this.points)
			{
				yield return curvePoint;
			}
			List<CurvePoint>.Enumerator enumerator = default(List<CurvePoint>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x17000083 RID: 131
		public CurvePoint this[int i]
		{
			get
			{
				return this.points[i];
			}
			set
			{
				this.points[i] = value;
			}
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0007D238 File Offset: 0x0007B438
		public void SetPoints(IEnumerable<CurvePoint> newPoints)
		{
			this.points.Clear();
			foreach (CurvePoint item in newPoints)
			{
				this.points.Add(item);
			}
			this.SortPoints();
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0007D298 File Offset: 0x0007B498
		public void Add(float x, float y, bool sort = true)
		{
			CurvePoint newPoint = new CurvePoint(x, y);
			this.Add(newPoint, sort);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00008272 File Offset: 0x00006472
		public void Add(CurvePoint newPoint, bool sort = true)
		{
			this.points.Add(newPoint);
			if (sort)
			{
				this.SortPoints();
			}
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00008289 File Offset: 0x00006489
		public void SortPoints()
		{
			this.points.Sort(SimpleCurve.CurvePointsComparer);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0007D2B8 File Offset: 0x0007B4B8
		public float ClampToCurve(float value)
		{
			if (this.points.Count == 0)
			{
				Log.Error("Clamping a value to an empty SimpleCurve.", false);
				return value;
			}
			return Mathf.Clamp(value, this.points[0].y, this.points[this.points.Count - 1].y);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0007D31C File Offset: 0x0007B51C
		public void RemovePointNear(CurvePoint point)
		{
			for (int i = 0; i < this.points.Count; i++)
			{
				if ((this.points[i].Loc - point.Loc).sqrMagnitude < 0.001f)
				{
					this.points.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0007D37C File Offset: 0x0007B57C
		public float Evaluate(float x)
		{
			if (this.points.Count == 0)
			{
				Log.Error("Evaluating a SimpleCurve with no points.", false);
				return 0f;
			}
			if (x <= this.points[0].x)
			{
				return this.points[0].y;
			}
			if (x >= this.points[this.points.Count - 1].x)
			{
				return this.points[this.points.Count - 1].y;
			}
			CurvePoint curvePoint = this.points[0];
			CurvePoint curvePoint2 = this.points[this.points.Count - 1];
			int i = 0;
			while (i < this.points.Count)
			{
				if (x <= this.points[i].x)
				{
					curvePoint2 = this.points[i];
					if (i > 0)
					{
						curvePoint = this.points[i - 1];
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			float t = (x - curvePoint.x) / (curvePoint2.x - curvePoint.x);
			return Mathf.Lerp(curvePoint.y, curvePoint2.y, t);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0007D4C4 File Offset: 0x0007B6C4
		public float EvaluateInverted(float y)
		{
			if (this.points.Count == 0)
			{
				Log.Error("Evaluating a SimpleCurve with no points.", false);
				return 0f;
			}
			if (this.points.Count == 1)
			{
				return this.points[0].x;
			}
			int i = 0;
			while (i < this.points.Count - 1)
			{
				if ((y >= this.points[i].y && y <= this.points[i + 1].y) || (y <= this.points[i].y && y >= this.points[i + 1].y))
				{
					if (y == this.points[i].y)
					{
						return this.points[i].x;
					}
					if (y == this.points[i + 1].y)
					{
						return this.points[i + 1].x;
					}
					return GenMath.LerpDouble(this.points[i].y, this.points[i + 1].y, this.points[i].x, this.points[i + 1].x, y);
				}
				else
				{
					i++;
				}
			}
			if (y < this.points[0].y)
			{
				float result = 0f;
				float num = 0f;
				for (int j = 0; j < this.points.Count; j++)
				{
					if (j == 0 || this.points[j].y < num)
					{
						num = this.points[j].y;
						result = this.points[j].x;
					}
				}
				return result;
			}
			float result2 = 0f;
			float num2 = 0f;
			for (int k = 0; k < this.points.Count; k++)
			{
				if (k == 0 || this.points[k].y > num2)
				{
					num2 = this.points[k].y;
					result2 = this.points[k].x;
				}
			}
			return result2;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0007D74C File Offset: 0x0007B94C
		public float PeriodProbabilityFromCumulative(float startX, float span)
		{
			if (this.points.Count < 2)
			{
				return 0f;
			}
			if (this.points[0].y != 0f)
			{
				Log.Warning("PeriodProbabilityFromCumulative should only run on curves whose first point is 0.", false);
			}
			float num = this.Evaluate(startX + span) - this.Evaluate(startX);
			if (num < 0f)
			{
				Log.Error("PeriodicProbability got negative probability from " + this + ": slope should never be negative.", false);
				num = 0f;
			}
			if (num > 1f)
			{
				num = 1f;
			}
			return num;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000829B File Offset: 0x0000649B
		public IEnumerable<string> ConfigErrors(string prefix)
		{
			for (int i = 0; i < this.points.Count - 1; i++)
			{
				if (this.points[i + 1].x < this.points[i].x)
				{
					yield return prefix + ": points are out of order";
					break;
				}
			}
			yield break;
		}

		// Token: 0x04000076 RID: 118
		private List<CurvePoint> points = new List<CurvePoint>();

		// Token: 0x04000077 RID: 119
		[Unsaved(false)]
		private SimpleCurveView view;

		// Token: 0x04000078 RID: 120
		private static Comparison<CurvePoint> CurvePointsComparer = delegate(CurvePoint a, CurvePoint b)
		{
			if (a.x < b.x)
			{
				return -1;
			}
			if (b.x < a.x)
			{
				return 1;
			}
			return 0;
		};
	}
}
