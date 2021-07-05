using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200035F RID: 863
	public class Graphic_StackCount : Graphic_Collection
	{
		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x0600187F RID: 6271 RVA: 0x00091129 File Offset: 0x0008F329
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[this.subGraphics.Length - 1].MatSingle;
			}
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x00091141 File Offset: 0x0008F341
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_StackCount>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data, null);
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x00090B7E File Offset: 0x0008ED7E
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.MatSingleFor(thing);
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x0009115E File Offset: 0x0008F35E
		public override Material MatSingleFor(Thing thing)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.SubGraphicFor(thing).MatSingle;
		}

		// Token: 0x06001883 RID: 6275 RVA: 0x00091176 File Offset: 0x0008F376
		public Graphic SubGraphicFor(Thing thing)
		{
			return this.SubGraphicForStackCount(thing.stackCount, thing.def);
		}

		// Token: 0x06001884 RID: 6276 RVA: 0x0009118C File Offset: 0x0008F38C
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

		// Token: 0x06001885 RID: 6277 RVA: 0x000911C0 File Offset: 0x0008F3C0
		public Graphic SubGraphicForStackCount(int stackCount, ThingDef def)
		{
			switch (this.subGraphics.Length)
			{
			case 1:
				return this.subGraphics[0];
			case 2:
				if (stackCount == 1)
				{
					return this.subGraphics[0];
				}
				return this.subGraphics[1];
			case 3:
				if (stackCount == 1)
				{
					return this.subGraphics[0];
				}
				if (stackCount == def.stackLimit)
				{
					return this.subGraphics[2];
				}
				return this.subGraphics[1];
			default:
			{
				if (stackCount == 1)
				{
					return this.subGraphics[0];
				}
				if (stackCount == def.stackLimit)
				{
					return this.subGraphics[this.subGraphics.Length - 1];
				}
				int num = Mathf.Min(1 + Mathf.RoundToInt((float)stackCount / (float)def.stackLimit * ((float)this.subGraphics.Length - 3f) + 1E-05f), this.subGraphics.Length - 2);
				return this.subGraphics[num];
			}
			}
		}

		// Token: 0x06001886 RID: 6278 RVA: 0x0009129A File Offset: 0x0008F49A
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"StackCount(path=",
				this.path,
				", count=",
				this.subGraphics.Length,
				")"
			});
		}
	}
}
