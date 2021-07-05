using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000F66 RID: 3942
	public struct WornGraphicBodyTypeData
	{
		// Token: 0x17000D42 RID: 3394
		// (get) Token: 0x0600569B RID: 22171 RVA: 0x001CAB58 File Offset: 0x001C8D58
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

		// Token: 0x040037D3 RID: 14291
		public Vector2 offset;

		// Token: 0x040037D4 RID: 14292
		public Vector2? scale;
	}
}
