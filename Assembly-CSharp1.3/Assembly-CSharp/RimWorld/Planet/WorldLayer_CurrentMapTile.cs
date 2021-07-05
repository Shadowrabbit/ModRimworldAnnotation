using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200175A RID: 5978
	public class WorldLayer_CurrentMapTile : WorldLayer_SingleTile
	{
		// Token: 0x1700167D RID: 5757
		// (get) Token: 0x06008A02 RID: 35330 RVA: 0x00318ECB File Offset: 0x003170CB
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

		// Token: 0x1700167E RID: 5758
		// (get) Token: 0x06008A03 RID: 35331 RVA: 0x00318EEA File Offset: 0x003170EA
		protected override Material Material
		{
			get
			{
				return WorldMaterials.CurrentMapTile;
			}
		}
	}
}
