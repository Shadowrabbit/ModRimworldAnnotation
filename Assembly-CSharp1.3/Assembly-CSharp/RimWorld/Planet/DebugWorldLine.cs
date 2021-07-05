using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001756 RID: 5974
	public class DebugWorldLine
	{
		// Token: 0x17001677 RID: 5751
		// (get) Token: 0x060089E8 RID: 35304 RVA: 0x003188D8 File Offset: 0x00316AD8
		// (set) Token: 0x060089E9 RID: 35305 RVA: 0x003188E0 File Offset: 0x00316AE0
		public int TicksLeft
		{
			get
			{
				return this.ticksLeft;
			}
			set
			{
				this.ticksLeft = value;
			}
		}

		// Token: 0x060089EA RID: 35306 RVA: 0x003188E9 File Offset: 0x00316AE9
		public DebugWorldLine(Vector3 a, Vector3 b, bool onPlanetSurface)
		{
			this.a = a;
			this.b = b;
			this.onPlanetSurface = onPlanetSurface;
			this.ticksLeft = 100;
		}

		// Token: 0x060089EB RID: 35307 RVA: 0x0031890E File Offset: 0x00316B0E
		public DebugWorldLine(Vector3 a, Vector3 b, bool onPlanetSurface, int ticksLeft)
		{
			this.a = a;
			this.b = b;
			this.onPlanetSurface = onPlanetSurface;
			this.ticksLeft = ticksLeft;
		}

		// Token: 0x060089EC RID: 35308 RVA: 0x00318934 File Offset: 0x00316B34
		public void Draw()
		{
			float num = Vector3.Distance(this.a, this.b);
			if (num < 0.001f)
			{
				return;
			}
			if (this.onPlanetSurface)
			{
				float averageTileSize = Find.WorldGrid.averageTileSize;
				int num2 = Mathf.Max(Mathf.RoundToInt(num / averageTileSize), 0);
				float num3 = 0.05f;
				for (int i = 0; i < num2; i++)
				{
					Vector3 vector = Vector3.Lerp(this.a, this.b, (float)i / (float)num2);
					Vector3 vector2 = Vector3.Lerp(this.a, this.b, (float)(i + 1) / (float)num2);
					vector = vector.normalized * (100f + num3);
					vector2 = vector2.normalized * (100f + num3);
					GenDraw.DrawWorldLineBetween(vector, vector2);
				}
				return;
			}
			GenDraw.DrawWorldLineBetween(this.a, this.b);
		}

		// Token: 0x040057AC RID: 22444
		public Vector3 a;

		// Token: 0x040057AD RID: 22445
		public Vector3 b;

		// Token: 0x040057AE RID: 22446
		public int ticksLeft;

		// Token: 0x040057AF RID: 22447
		private bool onPlanetSurface;
	}
}
