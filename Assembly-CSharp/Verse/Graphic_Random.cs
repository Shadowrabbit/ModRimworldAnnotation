using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004E2 RID: 1250
	public class Graphic_Random : Graphic_Collection
	{
		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06001F31 RID: 7985 RVA: 0x0001B5F3 File Offset: 0x000197F3
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[Rand.Range(0, this.subGraphics.Length)].MatSingle;
			}
		}

		// Token: 0x06001F32 RID: 7986 RVA: 0x0001B7E6 File Offset: 0x000199E6
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			if (newColorTwo != Color.white)
			{
				Log.ErrorOnce("Cannot use Graphic_Random.GetColoredVersion with a non-white colorTwo.", 9910251, false);
			}
			return GraphicDatabase.Get<Graphic_Random>(this.path, newShader, this.drawSize, newColor, Color.white, this.data);
		}

		// Token: 0x06001F33 RID: 7987 RVA: 0x0001B823 File Offset: 0x00019A23
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.MatSingleFor(thing);
		}

		// Token: 0x06001F34 RID: 7988 RVA: 0x0001B836 File Offset: 0x00019A36
		public override Material MatSingleFor(Thing thing)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.SubGraphicFor(thing).MatSingle;
		}

		// Token: 0x06001F35 RID: 7989 RVA: 0x000FF534 File Offset: 0x000FD734
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
		}

		// Token: 0x06001F36 RID: 7990 RVA: 0x0001B84E File Offset: 0x00019A4E
		public Graphic SubGraphicFor(Thing thing)
		{
			if (thing == null)
			{
				return this.subGraphics[0];
			}
			return this.subGraphics[thing.thingIDNumber % this.subGraphics.Length];
		}

		// Token: 0x06001F37 RID: 7991 RVA: 0x0001B872 File Offset: 0x00019A72
		public Graphic FirstSubgraphic()
		{
			return this.subGraphics[0];
		}

		// Token: 0x06001F38 RID: 7992 RVA: 0x0001B87C File Offset: 0x00019A7C
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
