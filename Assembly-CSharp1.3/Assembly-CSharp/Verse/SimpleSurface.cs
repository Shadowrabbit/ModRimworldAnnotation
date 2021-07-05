using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000485 RID: 1157
	public class SimpleSurface : IEnumerable<SurfaceColumn>, IEnumerable
	{
		// Token: 0x0600233E RID: 9022 RVA: 0x000DD318 File Offset: 0x000DB518
		public float Evaluate(float x, float y)
		{
			if (this.columns.Count == 0)
			{
				Log.Error("Evaluating a SimpleCurve2D with no columns.");
				return 0f;
			}
			if (x <= this.columns[0].x)
			{
				return this.columns[0].y.Evaluate(y);
			}
			if (x >= this.columns[this.columns.Count - 1].x)
			{
				return this.columns[this.columns.Count - 1].y.Evaluate(y);
			}
			SurfaceColumn surfaceColumn = this.columns[0];
			SurfaceColumn surfaceColumn2 = this.columns[this.columns.Count - 1];
			int i = 0;
			while (i < this.columns.Count)
			{
				if (x <= this.columns[i].x)
				{
					surfaceColumn2 = this.columns[i];
					if (i > 0)
					{
						surfaceColumn = this.columns[i - 1];
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			float t = (x - surfaceColumn.x) / (surfaceColumn2.x - surfaceColumn.x);
			return Mathf.Lerp(surfaceColumn.y.Evaluate(y), surfaceColumn2.y.Evaluate(y), t);
		}

		// Token: 0x0600233F RID: 9023 RVA: 0x000DD459 File Offset: 0x000DB659
		public void Add(SurfaceColumn newColumn)
		{
			this.columns.Add(newColumn);
		}

		// Token: 0x06002340 RID: 9024 RVA: 0x000DD467 File Offset: 0x000DB667
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06002341 RID: 9025 RVA: 0x000DD46F File Offset: 0x000DB66F
		public IEnumerator<SurfaceColumn> GetEnumerator()
		{
			foreach (SurfaceColumn surfaceColumn in this.columns)
			{
				yield return surfaceColumn;
			}
			List<SurfaceColumn>.Enumerator enumerator = default(List<SurfaceColumn>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06002342 RID: 9026 RVA: 0x000DD47E File Offset: 0x000DB67E
		public IEnumerable<string> ConfigErrors(string prefix)
		{
			for (int i = 0; i < this.columns.Count - 1; i++)
			{
				if (this.columns[i + 1].x < this.columns[i].x)
				{
					yield return prefix + ": columns are out of order";
					break;
				}
			}
			yield break;
		}

		// Token: 0x04001603 RID: 5635
		private List<SurfaceColumn> columns = new List<SurfaceColumn>();
	}
}
