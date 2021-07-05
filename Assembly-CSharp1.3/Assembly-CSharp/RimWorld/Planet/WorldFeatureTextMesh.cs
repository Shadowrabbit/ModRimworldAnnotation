using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x0200173F RID: 5951
	public abstract class WorldFeatureTextMesh
	{
		// Token: 0x17001645 RID: 5701
		// (get) Token: 0x0600894E RID: 35150
		public abstract bool Active { get; }

		// Token: 0x17001646 RID: 5702
		// (get) Token: 0x0600894F RID: 35151
		public abstract Vector3 Position { get; }

		// Token: 0x17001647 RID: 5703
		// (get) Token: 0x06008950 RID: 35152
		// (set) Token: 0x06008951 RID: 35153
		public abstract Color Color { get; set; }

		// Token: 0x17001648 RID: 5704
		// (get) Token: 0x06008952 RID: 35154
		// (set) Token: 0x06008953 RID: 35155
		public abstract string Text { get; set; }

		// Token: 0x17001649 RID: 5705
		// (set) Token: 0x06008954 RID: 35156
		public abstract float Size { set; }

		// Token: 0x1700164A RID: 5706
		// (get) Token: 0x06008955 RID: 35157
		// (set) Token: 0x06008956 RID: 35158
		public abstract Quaternion Rotation { get; set; }

		// Token: 0x1700164B RID: 5707
		// (get) Token: 0x06008957 RID: 35159
		// (set) Token: 0x06008958 RID: 35160
		public abstract Vector3 LocalPosition { get; set; }

		// Token: 0x06008959 RID: 35161
		public abstract void SetActive(bool active);

		// Token: 0x0600895A RID: 35162
		public abstract void Destroy();

		// Token: 0x0600895B RID: 35163
		public abstract void Init();

		// Token: 0x0600895C RID: 35164
		public abstract void WrapAroundPlanetSurface();
	}
}
