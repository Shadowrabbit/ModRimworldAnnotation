using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200035B RID: 859
	public class Graphic_Random : Graphic_Collection
	{
		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x0600185C RID: 6236 RVA: 0x0008F270 File Offset: 0x0008D470
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[Rand.Range(0, this.subGraphics.Length)].MatSingle;
			}
		}

		// Token: 0x0600185D RID: 6237 RVA: 0x00090B41 File Offset: 0x0008ED41
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			if (newColorTwo != Color.white)
			{
				Log.ErrorOnce("Cannot use Graphic_Random.GetColoredVersion with a non-white colorTwo.", 9910251);
			}
			return GraphicDatabase.Get<Graphic_Random>(this.path, newShader, this.drawSize, newColor, Color.white, this.data, null);
		}

		// Token: 0x0600185E RID: 6238 RVA: 0x00090B7E File Offset: 0x0008ED7E
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.MatSingleFor(thing);
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x00090B91 File Offset: 0x0008ED91
		public override Material MatSingleFor(Thing thing)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.SubGraphicFor(thing).MatSingle;
		}

		// Token: 0x06001860 RID: 6240 RVA: 0x00090BAC File Offset: 0x0008EDAC
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Graphic graphic;
			if (thing != null)
			{
				graphic = this.SubGraphicFor(thing);
			}
			else
			{
				graphic = this.subGraphics[0];
			}
			graphic.DrawWorker(loc, rot, thingDef, thing, extraRotation);
			if (base.ShadowGraphic != null)
			{
				base.ShadowGraphic.DrawWorker(loc, rot, thingDef, thing, extraRotation);
			}
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x00090BF8 File Offset: 0x0008EDF8
		public Graphic SubGraphicFor(Thing thing)
		{
			if (thing == null)
			{
				return this.subGraphics[0];
			}
			return this.subGraphics[thing.thingIDNumber % this.subGraphics.Length];
		}

		// Token: 0x06001862 RID: 6242 RVA: 0x00090C1C File Offset: 0x0008EE1C
		public Graphic FirstSubgraphic()
		{
			return this.subGraphics[0];
		}

		// Token: 0x06001863 RID: 6243 RVA: 0x00090C28 File Offset: 0x0008EE28
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			Graphic graphic;
			if (thing != null)
			{
				graphic = this.SubGraphicFor(thing);
			}
			else
			{
				graphic = this.subGraphics[0];
			}
			graphic.Print(layer, thing, extraRotation);
			if (base.ShadowGraphic != null && thing != null)
			{
				base.ShadowGraphic.Print(layer, thing, extraRotation);
			}
		}

		// Token: 0x06001864 RID: 6244 RVA: 0x00090C6D File Offset: 0x0008EE6D
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Random(path=",
				this.path,
				", count=",
				this.subGraphics.Length,
				")"
			});
		}
	}
}
