using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004D6 RID: 1238
	public class Graphic_Cluster : Graphic_Collection
	{
		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06001EF3 RID: 7923 RVA: 0x0001B5F3 File Offset: 0x000197F3
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[Rand.Range(0, this.subGraphics.Length)].MatSingle;
			}
		}

		// Token: 0x06001EF4 RID: 7924 RVA: 0x0001B60F File Offset: 0x0001980F
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Log.ErrorOnce("Graphic_Scatter cannot draw realtime.", 9432243, false);
		}

		// Token: 0x06001EF5 RID: 7925 RVA: 0x000FE3EC File Offset: 0x000FC5EC
		public override void Print(SectionLayer layer, Thing thing)
		{
			Vector3 a = thing.TrueCenter();
			Rand.PushState();
			Rand.Seed = thing.Position.GetHashCode();
			Filth filth = thing as Filth;
			int num;
			if (filth == null)
			{
				num = 3;
			}
			else
			{
				num = filth.thickness;
			}
			for (int i = 0; i < num; i++)
			{
				Material matSingle = this.MatSingle;
				Vector3 center = a + new Vector3(Rand.Range(-0.45f, 0.45f), 0f, Rand.Range(-0.45f, 0.45f));
				Vector2 size = new Vector2(Rand.Range(this.data.drawSize.x * 0.8f, this.data.drawSize.x * 1.2f), Rand.Range(this.data.drawSize.y * 0.8f, this.data.drawSize.y * 1.2f));
				float rot = (float)Rand.RangeInclusive(0, 360);
				bool flipUv = Rand.Value < 0.5f;
				Printer_Plane.PrintPlane(layer, center, size, matSingle, rot, flipUv, null, null, 0.01f, 0f);
			}
			Rand.PopState();
		}

		// Token: 0x06001EF6 RID: 7926 RVA: 0x000FE528 File Offset: 0x000FC728
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Scatter(subGraphic[0]=",
				this.subGraphics[0].ToString(),
				", count=",
				this.subGraphics.Length,
				")"
			});
		}

		// Token: 0x040015E0 RID: 5600
		private const float PositionVariance = 0.45f;

		// Token: 0x040015E1 RID: 5601
		private const float SizeVariance = 0.2f;

		// Token: 0x040015E2 RID: 5602
		private const float SizeFactorMin = 0.8f;

		// Token: 0x040015E3 RID: 5603
		private const float SizeFactorMax = 1.2f;
	}
}
