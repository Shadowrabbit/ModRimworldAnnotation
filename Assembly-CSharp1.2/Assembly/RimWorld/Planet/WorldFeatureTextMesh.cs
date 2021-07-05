using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x0200202D RID: 8237
	public abstract class WorldFeatureTextMesh
	{
		// Token: 0x170019A5 RID: 6565
		// (get) Token: 0x0600AE84 RID: 44676
		public abstract bool Active { get; }

		// Token: 0x170019A6 RID: 6566
		// (get) Token: 0x0600AE85 RID: 44677
		public abstract Vector3 Position { get; }

		// Token: 0x170019A7 RID: 6567
		// (get) Token: 0x0600AE86 RID: 44678
		// (set) Token: 0x0600AE87 RID: 44679
		public abstract Color Color { get; set; }

		// Token: 0x170019A8 RID: 6568
		// (get) Token: 0x0600AE88 RID: 44680
		// (set) Token: 0x0600AE89 RID: 44681
		public abstract string Text { get; set; }

		// Token: 0x170019A9 RID: 6569
		// (set) Token: 0x0600AE8A RID: 44682
		public abstract float Size { set; }

		// Token: 0x170019AA RID: 6570
		// (get) Token: 0x0600AE8B RID: 44683
		// (set) Token: 0x0600AE8C RID: 44684
		public abstract Quaternion Rotation { get; set; }

		// Token: 0x170019AB RID: 6571
		// (get) Token: 0x0600AE8D RID: 44685
		// (set) Token: 0x0600AE8E RID: 44686
		public abstract Vector3 LocalPosition { get; set; }

		// Token: 0x0600AE8F RID: 44687
		public abstract void SetActive(bool active);

		// Token: 0x0600AE90 RID: 44688
		public abstract void Destroy();

		// Token: 0x0600AE91 RID: 44689
		public abstract void Init();

		// Token: 0x0600AE92 RID: 44690
		public abstract void WrapAroundPlanetSurface();
	}
}
