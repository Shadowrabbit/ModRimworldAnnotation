using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017C8 RID: 6088
	public class EscapeShip : MapParent
	{
		// Token: 0x17001700 RID: 5888
		// (get) Token: 0x06008D5A RID: 36186 RVA: 0x0032DAA7 File Offset: 0x0032BCA7
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

		// Token: 0x17001701 RID: 5889
		// (get) Token: 0x06008D5B RID: 36187 RVA: 0x0032DAD0 File Offset: 0x0032BCD0
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

		// Token: 0x17001702 RID: 5890
		// (get) Token: 0x06008D5C RID: 36188 RVA: 0x0032DAF4 File Offset: 0x0032BCF4
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

		// Token: 0x06008D5D RID: 36189 RVA: 0x0032DB5C File Offset: 0x0032BD5C
		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			Find.World.renderer.SetDirty<WorldLayer_WorldObjects>();
		}

		// Token: 0x04005987 RID: 22919
		private Material cachedPostGenerateMat;
	}
}
