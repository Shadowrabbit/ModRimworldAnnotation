using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004E6 RID: 1254
	public class Graphic_StackCount : Graphic_Collection
	{
		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06001F50 RID: 8016 RVA: 0x0001B9C2 File Offset: 0x00019BC2
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[this.subGraphics.Length - 1].MatSingle;
			}
		}

		// Token: 0x06001F51 RID: 8017 RVA: 0x0001B9DA File Offset: 0x00019BDA
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_StackCount>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data);
		}

		// Token: 0x06001F52 RID: 8018 RVA: 0x0001B823 File Offset: 0x00019A23
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.MatSingleFor(thing);
		}

		// Token: 0x06001F53 RID: 8019 RVA: 0x0001B9F6 File Offset: 0x00019BF6
		public override Material MatSingleFor(Thing thing)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.SubGraphicFor(thing).MatSingle;
		}

		// Token: 0x06001F54 RID: 8020 RVA: 0x0001BA0E File Offset: 0x00019C0E
		public Graphic SubGraphicFor(Thing thing)
		{
			return this.SubGraphicForStackCount(thing.stackCount, thing.def);
		}

		// Token: 0x06001F55 RID: 8021 RVA: 0x000FF810 File Offset: 0x000FDA10
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

		// Token: 0x06001F56 RID: 8022 RVA: 0x000FF844 File Offset: 0x000FDA44
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

		// Token: 0x06001F57 RID: 8023 RVA: 0x0001BA22 File Offset: 0x00019C22
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
