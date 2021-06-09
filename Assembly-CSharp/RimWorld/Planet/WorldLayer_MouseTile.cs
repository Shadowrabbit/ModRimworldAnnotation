using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200204D RID: 8269
	public class WorldLayer_MouseTile : WorldLayer_SingleTile
	{
		// Token: 0x170019E5 RID: 6629
		// (get) Token: 0x0600AF4F RID: 44879 RVA: 0x0032F0D4 File Offset: 0x0032D2D4
		protected override int Tile
		{
			get
			{
				if (Find.World.UI.selector.dragBox.IsValidAndActive)
				{
					return -1;
				}
				if (Find.WorldTargeter.IsTargeting)
				{
					return -1;
				}
				if (Find.ScreenshotModeHandler.Active)
				{
					return -1;
				}
				return GenWorld.MouseTile(false);
			}
		}

		// Token: 0x170019E6 RID: 6630
		// (get) Token: 0x0600AF50 RID: 44880 RVA: 0x000721DD File Offset: 0x000703DD
		protected override Material Material
		{
			get
			{
				return WorldMaterials.MouseTile;
			}
		}
	}
}
