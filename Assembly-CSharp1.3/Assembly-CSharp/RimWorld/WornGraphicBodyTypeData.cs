using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000A44 RID: 2628
	public struct WornGraphicBodyTypeData
	{
		// Token: 0x17000B0A RID: 2826
		// (get) Token: 0x06003F77 RID: 16247 RVA: 0x00158DD8 File Offset: 0x00156FD8
		public Vector2 Scale
		{
			get
			{
				Vector2? vector = this.scale;
				if (vector == null)
				{
					return Vector2.one;
				}
				return vector.GetValueOrDefault();
			}
		}

		// Token: 0x040022DA RID: 8922
		public Vector2 offset;

		// Token: 0x040022DB RID: 8923
		public Vector2? scale;
	}
}
