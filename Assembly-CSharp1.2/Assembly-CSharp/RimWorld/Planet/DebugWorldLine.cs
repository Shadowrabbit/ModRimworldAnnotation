using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002043 RID: 8259
	public class DebugWorldLine
	{
		// Token: 0x170019D7 RID: 6615
		// (get) Token: 0x0600AF11 RID: 44817 RVA: 0x00071FA2 File Offset: 0x000701A2
		// (set) Token: 0x0600AF12 RID: 44818 RVA: 0x00071FAA File Offset: 0x000701AA
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

		// Token: 0x0600AF13 RID: 44819 RVA: 0x00071FB3 File Offset: 0x000701B3
		public DebugWorldLine(Vector3 a, Vector3 b, bool onPlanetSurface)
		{
			this.a = a;
			this.b = b;
			this.onPlanetSurface = onPlanetSurface;
			this.ticksLeft = 100;
		}

		// Token: 0x0600AF14 RID: 44820 RVA: 0x00071FD8 File Offset: 0x000701D8
		public DebugWorldLine(Vector3 a, Vector3 b, bool onPlanetSurface, int ticksLeft)
		{
			this.a = a;
			this.b = b;
			this.onPlanetSurface = onPlanetSurface;
			this.ticksLeft = ticksLeft;
		}

		// Token: 0x0600AF15 RID: 44821 RVA: 0x0032E6EC File Offset: 0x0032C8EC
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

		// Token: 0x04007859 RID: 30809
		public Vector3 a;

		// Token: 0x0400785A RID: 30810
		public Vector3 b;

		// Token: 0x0400785B RID: 30811
		public int ticksLeft;

		// Token: 0x0400785C RID: 30812
		private bool onPlanetSurface;
	}
}
