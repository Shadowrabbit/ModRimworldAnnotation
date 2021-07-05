using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200175D RID: 5981
	public class WorldLayer_MouseTile : WorldLayer_SingleTile
	{
		// Token: 0x1700167F RID: 5759
		// (get) Token: 0x06008A0C RID: 35340 RVA: 0x00318FAC File Offset: 0x003171AC
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

		// Token: 0x17001680 RID: 5760
		// (get) Token: 0x06008A0D RID: 35341 RVA: 0x00318FF8 File Offset: 0x003171F8
		protected override Material Material
		{
			get
			{
				return WorldMaterials.MouseTile;
			}
		}
	}
}
