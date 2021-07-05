using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004DC RID: 1244
	public class Graphic_Linked : Graphic
	{
		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06001F06 RID: 7942 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual LinkDrawerType LinkerType
		{
			get
			{
				return LinkDrawerType.Basic;
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06001F07 RID: 7943 RVA: 0x0001B662 File Offset: 0x00019862
		public override Material MatSingle
		{
			get
			{
				return MaterialAtlasPool.SubMaterialFromAtlas(this.subGraphic.MatSingle, LinkDirections.None);
			}
		}

		// Token: 0x06001F08 RID: 7944 RVA: 0x0001B5D3 File Offset: 0x000197D3
		public Graphic_Linked()
		{
		}

		// Token: 0x06001F09 RID: 7945 RVA: 0x0001B675 File Offset: 0x00019875
		public Graphic_Linked(Graphic subGraphic)
		{
			this.subGraphic = subGraphic;
		}

		// Token: 0x06001F0A RID: 7946 RVA: 0x0001B684 File Offset: 0x00019884
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return new Graphic_Linked(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo))
			{
				data = this.data
			};
		}

		// Token: 0x06001F0B RID: 7947 RVA: 0x000FEA70 File Offset: 0x000FCC70
		public override void Print(SectionLayer layer, Thing thing)
		{
			Material mat = this.LinkedDrawMatFrom(thing, thing.Position);
			Printer_Plane.PrintPlane(layer, thing.TrueCenter(), new Vector2(1f, 1f), mat, 0f, false, null, null, 0.01f, 0f);
		}

		// Token: 0x06001F0C RID: 7948 RVA: 0x0001B6A5 File Offset: 0x000198A5
		public override Material MatSingleFor(Thing thing)
		{
			return this.LinkedDrawMatFrom(thing, thing.Position);
		}

		// Token: 0x06001F0D RID: 7949 RVA: 0x000FEABC File Offset: 0x000FCCBC
		protected Material LinkedDrawMatFrom(Thing parent, IntVec3 cell)
		{
			int num = 0;
			int num2 = 1;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = cell + GenAdj.CardinalDirections[i];
				if (this.ShouldLinkWith(c, parent))
				{
					num += num2;
				}
				num2 *= 2;
			}
			LinkDirections linkSet = (LinkDirections)num;
			return MaterialAtlasPool.SubMaterialFromAtlas(this.subGraphic.MatSingleFor(parent), linkSet);
		}

		// Token: 0x06001F0E RID: 7950 RVA: 0x000FEB14 File Offset: 0x000FCD14
		public virtual bool ShouldLinkWith(IntVec3 c, Thing parent)
		{
			if (!parent.Spawned)
			{
				return false;
			}
			if (!c.InBounds(parent.Map))
			{
				return (parent.def.graphicData.linkFlags & LinkFlags.MapEdge) > LinkFlags.None;
			}
			return (parent.Map.linkGrid.LinkFlagsAt(c) & parent.def.graphicData.linkFlags) > LinkFlags.None;
		}

		// Token: 0x040015ED RID: 5613
		protected Graphic subGraphic;
	}
}
