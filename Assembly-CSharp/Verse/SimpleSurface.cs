using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007E6 RID: 2022
	public class SimpleSurface : IEnumerable<SurfaceColumn>, IEnumerable
	{
		// Token: 0x06003309 RID: 13065 RVA: 0x0014F000 File Offset: 0x0014D200
		public float Evaluate(float x, float y)
		{
			if (this.columns.Count == 0)
			{
				Log.Error("Evaluating a SimpleCurve2D with no columns.", false);
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

		// Token: 0x0600330A RID: 13066 RVA: 0x00027FE2 File Offset: 0x000261E2
		public void Add(SurfaceColumn newColumn)
		{
			this.columns.Add(newColumn);
		}

		// Token: 0x0600330B RID: 13067 RVA: 0x00027FF0 File Offset: 0x000261F0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600330C RID: 13068 RVA: 0x00027FF8 File Offset: 0x000261F8
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

		// Token: 0x0600330D RID: 13069 RVA: 0x00028007 File Offset: 0x00026207
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

		// Token: 0x0400232D RID: 9005
		private List<SurfaceColumn> columns = new List<SurfaceColumn>();
	}
}
