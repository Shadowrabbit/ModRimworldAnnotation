using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000357 RID: 855
	public class Graphic_LinkedCornerFiller : Graphic_Linked
	{
		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x0600183D RID: 6205 RVA: 0x0009007E File Offset: 0x0008E27E
		public override LinkDrawerType LinkerType
		{
			get
			{
				return LinkDrawerType.CornerFiller;
			}
		}

		// Token: 0x0600183E RID: 6206 RVA: 0x0008FE34 File Offset: 0x0008E034
		public Graphic_LinkedCornerFiller(Graphic subGraphic) : base(subGraphic)
		{
		}

		// Token: 0x0600183F RID: 6207 RVA: 0x00090081 File Offset: 0x0008E281
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return new Graphic_LinkedCornerFiller(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo))
			{
				data = this.data
			};
		}

		// Token: 0x06001840 RID: 6208 RVA: 0x000900A4 File Offset: 0x0008E2A4
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			base.Print(layer, thing, extraRotation);
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
					Printer_Plane.PrintPlane(layer, center, size, this.LinkedDrawMatFrom(thing, thing.Position), extraRotation, false, Graphic_LinkedCornerFiller.CornerFillUVs, null, 0.01f, 0f);
				}
			}
		}

		// Token: 0x04001092 RID: 4242
		private const float ShiftUp = 0.09f;

		// Token: 0x04001093 RID: 4243
		private const float CoverSize = 0.5f;

		// Token: 0x04001094 RID: 4244
		private static readonly float CoverSizeCornerCorner = new Vector2(0.5f, 0.5f).magnitude;

		// Token: 0x04001095 RID: 4245
		private static readonly float DistCenterCorner = new Vector2(0.5f, 0.5f).magnitude;

		// Token: 0x04001096 RID: 4246
		private static readonly float CoverOffsetDist = Graphic_LinkedCornerFiller.DistCenterCorner - Graphic_LinkedCornerFiller.CoverSizeCornerCorner * 0.5f;

		// Token: 0x04001097 RID: 4247
		private static readonly Vector2[] CornerFillUVs = new Vector2[]
		{
			new Vector2(0.5f, 0.6f),
			new Vector2(0.5f, 0.6f),
			new Vector2(0.5f, 0.6f),
			new Vector2(0.5f, 0.6f)
		};
	}
}
