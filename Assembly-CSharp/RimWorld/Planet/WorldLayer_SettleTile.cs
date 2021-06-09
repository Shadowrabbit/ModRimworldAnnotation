using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002057 RID: 8279
	public class WorldLayer_SettleTile : WorldLayer_SingleTile
	{
		// Token: 0x170019ED RID: 6637
		// (get) Token: 0x0600AF7A RID: 44922 RVA: 0x00330190 File Offset: 0x0032E390
		protected override int Tile
		{
			get
			{
				if (!(Find.WorldInterface.inspectPane.mouseoverGizmo is Command_Settle))
				{
					return -1;
				}
				Caravan caravan = Find.WorldSelector.SingleSelectedObject as Caravan;
				if (caravan == null)
				{
					return -1;
				}
				return caravan.Tile;
			}
		}

		// Token: 0x170019EE RID: 6638
		// (get) Token: 0x0600AF7B RID: 44923 RVA: 0x0007214A File Offset: 0x0007034A
		protected override Material Material
		{
			get
			{
				return WorldMaterials.CurrentMapTile;
			}
		}

		// Token: 0x170019EF RID: 6639
		// (get) Token: 0x0600AF7C RID: 44924 RVA: 0x000722DC File Offset: 0x000704DC
		protected override float Alpha
		{
			get
			{
				return Mathf.Abs(Time.time % 2f - 1f);
			}
		}
	}
}
