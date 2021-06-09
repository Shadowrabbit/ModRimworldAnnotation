using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002048 RID: 8264
	public class WorldLayer_CurrentMapTile : WorldLayer_SingleTile
	{
		// Token: 0x170019DF RID: 6623
		// (get) Token: 0x0600AF33 RID: 44851 RVA: 0x0007212B File Offset: 0x0007032B
		protected override int Tile
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing)
				{
					return -1;
				}
				if (Find.CurrentMap == null)
				{
					return -1;
				}
				return Find.CurrentMap.Tile;
			}
		}

		// Token: 0x170019E0 RID: 6624
		// (get) Token: 0x0600AF34 RID: 44852 RVA: 0x0007214A File Offset: 0x0007034A
		protected override Material Material
		{
			get
			{
				return WorldMaterials.CurrentMapTile;
			}
		}
	}
}
