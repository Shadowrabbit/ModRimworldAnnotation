using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02001B7A RID: 7034
	public class PawnColumnWorker_AllowedAreaWide : PawnColumnWorker_AllowedArea
	{
		// Token: 0x06009B03 RID: 39683 RVA: 0x00067296 File Offset: 0x00065496
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(350, this.GetMinWidth(table), this.GetMaxWidth(table));
		}
	}
}
