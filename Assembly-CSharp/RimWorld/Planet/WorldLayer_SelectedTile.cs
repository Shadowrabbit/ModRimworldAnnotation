using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002056 RID: 8278
	public class WorldLayer_SelectedTile : WorldLayer_SingleTile
	{
		// Token: 0x170019EB RID: 6635
		// (get) Token: 0x0600AF77 RID: 44919 RVA: 0x000722C9 File Offset: 0x000704C9
		protected override int Tile
		{
			get
			{
				return Find.WorldSelector.selectedTile;
			}
		}

		// Token: 0x170019EC RID: 6636
		// (get) Token: 0x0600AF78 RID: 44920 RVA: 0x000722D5 File Offset: 0x000704D5
		protected override Material Material
		{
			get
			{
				return WorldMaterials.SelectedTile;
			}
		}
	}
}
