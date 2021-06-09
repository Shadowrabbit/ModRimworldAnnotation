using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200212C RID: 8492
	public class EscapeShip : MapParent
	{
		// Token: 0x17001A8E RID: 6798
		// (get) Token: 0x0600B467 RID: 46183 RVA: 0x00075294 File Offset: 0x00073494
		public override Texture2D ExpandingIcon
		{
			get
			{
				if (!base.HasMap || base.Faction == null)
				{
					return base.ExpandingIcon;
				}
				return base.Faction.def.FactionIcon;
			}
		}

		// Token: 0x17001A8F RID: 6799
		// (get) Token: 0x0600B468 RID: 46184 RVA: 0x000752BD File Offset: 0x000734BD
		public override Color ExpandingIconColor
		{
			get
			{
				if (!base.HasMap || base.Faction == null)
				{
					return base.ExpandingIconColor;
				}
				return base.Faction.Color;
			}
		}

		// Token: 0x17001A90 RID: 6800
		// (get) Token: 0x0600B469 RID: 46185 RVA: 0x00345640 File Offset: 0x00343840
		public override Material Material
		{
			get
			{
				if (!base.HasMap || base.Faction == null)
				{
					return base.Material;
				}
				if (this.cachedPostGenerateMat == null)
				{
					this.cachedPostGenerateMat = MaterialPool.MatFrom(base.Faction.def.settlementTexturePath, ShaderDatabase.WorldOverlayTransparentLit, base.Faction.Color, WorldMaterials.WorldObjectRenderQueue);
				}
				return this.cachedPostGenerateMat;
			}
		}

		// Token: 0x0600B46A RID: 46186 RVA: 0x000752E1 File Offset: 0x000734E1
		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			Find.World.renderer.SetDirty<WorldLayer_WorldObjects>();
		}

		// Token: 0x04007BDD RID: 31709
		private Material cachedPostGenerateMat;
	}
}
