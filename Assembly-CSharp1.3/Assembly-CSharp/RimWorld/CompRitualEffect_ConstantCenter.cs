using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000FB5 RID: 4021
	public class CompRitualEffect_ConstantCenter : CompRitualEffect_Constant
	{
		// Token: 0x06005EED RID: 24301 RVA: 0x00207E84 File Offset: 0x00206084
		protected override Vector3 SpawnPos(LordJob_Ritual ritual)
		{
			return ritual.selectedTarget.Cell.ToVector3Shifted();
		}
	}
}
