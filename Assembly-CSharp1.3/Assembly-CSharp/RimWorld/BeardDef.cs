using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AD1 RID: 2769
	public class BeardDef : StyleItemDef
	{
		// Token: 0x06004155 RID: 16725 RVA: 0x0015F394 File Offset: 0x0015D594
		public Vector3 GetOffset(CrownType crownType, Rot4 rot)
		{
			if (crownType != CrownType.Narrow || rot == Rot4.North)
			{
				return Vector3.zero;
			}
			if (rot == Rot4.South)
			{
				return this.offsetNarrowSouth;
			}
			if (rot == Rot4.East)
			{
				return this.offsetNarrowEast;
			}
			return new Vector3(-this.offsetNarrowEast.x, 0f, this.offsetNarrowEast.z);
		}

		// Token: 0x04002728 RID: 10024
		public Vector3 offsetNarrowEast;

		// Token: 0x04002729 RID: 10025
		public Vector3 offsetNarrowSouth;
	}
}
