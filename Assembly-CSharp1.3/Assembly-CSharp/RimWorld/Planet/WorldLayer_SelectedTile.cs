using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001761 RID: 5985
	public class WorldLayer_SelectedTile : WorldLayer_SingleTile
	{
		// Token: 0x17001681 RID: 5761
		// (get) Token: 0x06008A1C RID: 35356 RVA: 0x0031996E File Offset: 0x00317B6E
		protected override int Tile
		{
			get
			{
				return Find.WorldSelector.selectedTile;
			}
		}

		// Token: 0x17001682 RID: 5762
		// (get) Token: 0x06008A1D RID: 35357 RVA: 0x0031997A File Offset: 0x00317B7A
		protected override Material Material
		{
			get
			{
				return WorldMaterials.SelectedTile;
			}
		}
	}
}
