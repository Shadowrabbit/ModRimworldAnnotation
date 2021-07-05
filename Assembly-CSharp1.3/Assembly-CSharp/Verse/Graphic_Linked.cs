using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000355 RID: 853
	public class Graphic_Linked : Graphic
	{
		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x0600182E RID: 6190 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual LinkDrawerType LinkerType
		{
			get
			{
				return LinkDrawerType.Basic;
			}
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x0600182F RID: 6191 RVA: 0x0008FCB8 File Offset: 0x0008DEB8
		public override Material MatSingle
		{
			get
			{
				return MaterialAtlasPool.SubMaterialFromAtlas(this.subGraphic.MatSingle, LinkDirections.None);
			}
		}

		// Token: 0x06001830 RID: 6192 RVA: 0x0008F268 File Offset: 0x0008D468
		public Graphic_Linked()
		{
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x0008FCCB File Offset: 0x0008DECB
		public Graphic_Linked(Graphic subGraphic)
		{
			this.subGraphic = subGraphic;
			this.data = subGraphic.data;
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x0008FCE6 File Offset: 0x0008DEE6
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return new Graphic_Linked(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo))
			{
				data = this.data
			};
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x0008FD08 File Offset: 0x0008DF08
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			Material mat = this.LinkedDrawMatFrom(thing, thing.Position);
			Printer_Plane.PrintPlane(layer, thing.TrueCenter(), new Vector2(1f, 1f), mat, extraRotation, false, null, null, 0.01f, 0f);
			if (base.ShadowGraphic != null && thing != null)
			{
				base.ShadowGraphic.Print(layer, thing, 0f);
			}
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x0008FD6A File Offset: 0x0008DF6A
		public override Material MatSingleFor(Thing thing)
		{
			return this.LinkedDrawMatFrom(thing, thing.Position);
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x0008FD7C File Offset: 0x0008DF7C
		protected virtual Material LinkedDrawMatFrom(Thing parent, IntVec3 cell)
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

		// Token: 0x06001836 RID: 6198 RVA: 0x0008FDD4 File Offset: 0x0008DFD4
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

		// Token: 0x04001091 RID: 4241
		protected Graphic subGraphic;
	}
}
