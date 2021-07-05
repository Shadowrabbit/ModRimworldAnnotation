using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200034C RID: 844
	public class Graphic_Cluster : Graphic_Collection
	{
		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06001812 RID: 6162 RVA: 0x0008F270 File Offset: 0x0008D470
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[Rand.Range(0, this.subGraphics.Length)].MatSingle;
			}
		}

		// Token: 0x06001813 RID: 6163 RVA: 0x0008F28C File Offset: 0x0008D48C
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Log.ErrorOnce("Graphic_Scatter cannot draw realtime.", 9432243);
		}

		// Token: 0x06001814 RID: 6164 RVA: 0x0008F2A0 File Offset: 0x0008D4A0
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
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
				float rot = (float)Rand.RangeInclusive(0, 360) + extraRotation;
				bool flipUv = Rand.Value < 0.5f;
				Vector2[] uvs;
				Color32 color;
				Graphic.TryGetTextureAtlasReplacementInfo(matSingle, thing.def.category.ToAtlasGroup(), flipUv, true, out matSingle, out uvs, out color);
				Printer_Plane.PrintPlane(layer, center, size, matSingle, rot, flipUv, uvs, new Color32[]
				{
					color,
					color,
					color,
					color
				}, 0.01f, 0f);
			}
			Rand.PopState();
		}

		// Token: 0x06001815 RID: 6165 RVA: 0x0008F428 File Offset: 0x0008D628
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

		// Token: 0x0400107D RID: 4221
		private const float PositionVariance = 0.45f;

		// Token: 0x0400107E RID: 4222
		private const float SizeVariance = 0.2f;

		// Token: 0x0400107F RID: 4223
		private const float SizeFactorMin = 0.8f;

		// Token: 0x04001080 RID: 4224
		private const float SizeFactorMax = 1.2f;
	}
}
