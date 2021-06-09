using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004DD RID: 1245
	public class Graphic_LinkedCornerFiller : Graphic_Linked
	{
		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06001F0F RID: 7951 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override LinkDrawerType LinkerType
		{
			get
			{
				return LinkDrawerType.CornerFiller;
			}
		}

		// Token: 0x06001F10 RID: 7952 RVA: 0x0001B6B7 File Offset: 0x000198B7
		public Graphic_LinkedCornerFiller(Graphic subGraphic) : base(subGraphic)
		{
		}

		// Token: 0x06001F11 RID: 7953 RVA: 0x0001B6C0 File Offset: 0x000198C0
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return new Graphic_LinkedCornerFiller(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo))
			{
				data = this.data
			};
		}

		// Token: 0x06001F12 RID: 7954 RVA: 0x000FEB74 File Offset: 0x000FCD74
		public override void Print(SectionLayer layer, Thing thing)
		{
			base.Print(layer, thing);
			IntVec3 position = thing.Position;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec = thing.Position + GenAdj.DiagonalDirectionsAround[i];
				if (this.ShouldLinkWith(intVec, thing) && (i != 0 || (this.ShouldLinkWith(position + IntVec3.West, thing) && this.ShouldLinkWith(position + IntVec3.South, thing))) && (i != 1 || (this.ShouldLinkWith(position + IntVec3.West, thing) && this.ShouldLinkWith(position + IntVec3.North, thing))) && (i != 2 || (this.ShouldLinkWith(position + IntVec3.East, thing) && this.ShouldLinkWith(position + IntVec3.North, thing))) && (i != 3 || (this.ShouldLinkWith(position + IntVec3.East, thing) && this.ShouldLinkWith(position + IntVec3.South, thing))))
				{
					Vector3 center = thing.DrawPos + GenAdj.DiagonalDirectionsAround[i].ToVector3().normalized * Graphic_LinkedCornerFiller.CoverOffsetDist + Altitudes.AltIncVect + new Vector3(0f, 0f, 0.09f);
					Vector2 size = new Vector2(0.5f, 0.5f);
					if (!intVec.InBounds(thing.Map))
					{
						if (intVec.x == -1)
						{
							center.x -= 1f;
							size.x *= 5f;
						}
						if (intVec.z == -1)
						{
							center.z -= 1f;
							size.y *= 5f;
						}
						if (intVec.x == thing.Map.Size.x)
						{
							center.x += 1f;
							size.x *= 5f;
						}
						if (intVec.z == thing.Map.Size.z)
						{
							center.z += 1f;
							size.y *= 5f;
						}
					}
					Printer_Plane.PrintPlane(layer, center, size, base.LinkedDrawMatFrom(thing, thing.Position), 0f, false, Graphic_LinkedCornerFiller.CornerFillUVs, null, 0.01f, 0f);
				}
			}
		}

		// Token: 0x040015EE RID: 5614
		private const float ShiftUp = 0.09f;

		// Token: 0x040015EF RID: 5615
		private const float CoverSize = 0.5f;

		// Token: 0x040015F0 RID: 5616
		private static readonly float CoverSizeCornerCorner = new Vector2(0.5f, 0.5f).magnitude;

		// Token: 0x040015F1 RID: 5617
		private static readonly float DistCenterCorner = new Vector2(0.5f, 0.5f).magnitude;

		// Token: 0x040015F2 RID: 5618
		private static readonly float CoverOffsetDist = Graphic_LinkedCornerFiller.DistCenterCorner - Graphic_LinkedCornerFiller.CoverSizeCornerCorner * 0.5f;

		// Token: 0x040015F3 RID: 5619
		private static readonly Vector2[] CornerFillUVs = new Vector2[]
		{
			new Vector2(0.5f, 0.6f),
			new Vector2(0.5f, 0.6f),
			new Vector2(0.5f, 0.6f),
			new Vector2(0.5f, 0.6f)
		};
	}
}
