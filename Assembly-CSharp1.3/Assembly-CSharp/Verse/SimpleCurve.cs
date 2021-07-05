using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200001F RID: 31
	public class SimpleCurve : IEnumerable<CurvePoint>, IEnumerable
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000174 RID: 372 RVA: 0x000077D2 File Offset: 0x000059D2
		public int PointsCount
		{
			get
			{
				return this.points.Count;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000175 RID: 373 RVA: 0x000077DF File Offset: 0x000059DF
		public List<CurvePoint> Points
		{
			get
			{
				return this.points;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000176 RID: 374 RVA: 0x000077E7 File Offset: 0x000059E7
		public bool HasView
		{
			get
			{
				return this.view != null;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000177 RID: 375 RVA: 0x000077F2 File Offset: 0x000059F2
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

		// Token: 0x06000178 RID: 376 RVA: 0x00007819 File Offset: 0x00005A19
		public SimpleCurve(IEnumerable<CurvePoint> points)
		{
			this.SetPoints(points);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00007833 File Offset: 0x00005A33
		public SimpleCurve()
		{
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00007846 File Offset: 0x00005A46
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000784E File Offset: 0x00005A4E
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

		// Token: 0x17000070 RID: 112
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

		// Token: 0x0600017E RID: 382 RVA: 0x0000787C File Offset: 0x00005A7C
		public void SetPoints(IEnumerable<CurvePoint> newPoints)
		{
			this.points.Clear();
			foreach (CurvePoint item in newPoints)
			{
				this.points.Add(item);
			}
			this.SortPoints();
		}

		// Token: 0x0600017F RID: 383 RVA: 0x000078DC File Offset: 0x00005ADC
		public void Add(float x, float y, bool sort = true)
		{
			CurvePoint newPoint = new CurvePoint(x, y);
			this.Add(newPoint, sort);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x000078FA File Offset: 0x00005AFA
		public void Add(CurvePoint newPoint, bool sort = true)
		{
			this.points.Add(newPoint);
			if (sort)
			{
				this.SortPoints();
			}
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00007911 File Offset: 0x00005B11
		public void SortPoints()
		{
			this.points.Sort(SimpleCurve.CurvePointsComparer);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00007924 File Offset: 0x00005B24
		public float ClampToCurve(float value)
		{
			if (this.points.Count == 0)
			{
				Log.Error("Clamping a value to an empty SimpleCurve.");
				return value;
			}
			return Mathf.Clamp(value, this.points[0].y, this.points[this.points.Count - 1].y);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00007984 File Offset: 0x00005B84
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

		// Token: 0x06000184 RID: 388 RVA: 0x000079E4 File Offset: 0x00005BE4
		public float Evaluate(float x)
		{
			if (this.points.Count == 0)
			{
				Log.Error("Evaluating a SimpleCurve with no points.");
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

		// Token: 0x06000185 RID: 389 RVA: 0x00007B2C File Offset: 0x00005D2C
		public float EvaluateInverted(float y)
		{
			if (this.points.Count == 0)
			{
				Log.Error("Evaluating a SimpleCurve with no points.");
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

		// Token: 0x06000186 RID: 390 RVA: 0x00007DB0 File Offset: 0x00005FB0
		public float PeriodProbabilityFromCumulative(float startX, float span)
		{
			if (this.points.Count < 2)
			{
				return 0f;
			}
			if (this.points[0].y != 0f)
			{
				Log.Warning("PeriodProbabilityFromCumulative should only run on curves whose first point is 0.");
			}
			float num = this.Evaluate(startX + span) - this.Evaluate(startX);
			if (num < 0f)
			{
				Log.Error("PeriodicProbability got negative probability from " + this + ": slope should never be negative.");
				num = 0f;
			}
			if (num > 1f)
			{
				num = 1f;
			}
			return num;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00007E3A File Offset: 0x0000603A
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

		// Token: 0x0400004D RID: 77
		private List<CurvePoint> points = new List<CurvePoint>();

		// Token: 0x0400004E RID: 78
		[Unsaved(false)]
		private SimpleCurveView view;

		// Token: 0x0400004F RID: 79
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
