using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02001390 RID: 5008
	public class PawnColumnWorker_AllowedAreaWide : PawnColumnWorker_AllowedArea
	{
		// Token: 0x060079CD RID: 31181 RVA: 0x002B0558 File Offset: 0x002AE758
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(350, this.GetMinWidth(table), this.GetMaxWidth(table));
		}
	}
}
