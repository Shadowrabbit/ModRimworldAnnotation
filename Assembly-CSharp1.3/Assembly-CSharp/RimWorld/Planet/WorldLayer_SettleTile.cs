using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001762 RID: 5986
	public class WorldLayer_SettleTile : WorldLayer_SingleTile
	{
		// Token: 0x17001683 RID: 5763
		// (get) Token: 0x06008A1F RID: 35359 RVA: 0x00319984 File Offset: 0x00317B84
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

		// Token: 0x17001684 RID: 5764
		// (get) Token: 0x06008A20 RID: 35360 RVA: 0x00318EEA File Offset: 0x003170EA
		protected override Material Material
		{
			get
			{
				return WorldMaterials.CurrentMapTile;
			}
		}

		// Token: 0x17001685 RID: 5765
		// (get) Token: 0x06008A21 RID: 35361 RVA: 0x003199C4 File Offset: 0x00317BC4
		protected override float Alpha
		{
			get
			{
				return Mathf.Abs(Time.time % 2f - 1f);
			}
		}
	}
}
