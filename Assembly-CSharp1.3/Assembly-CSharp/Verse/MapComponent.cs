using System;

namespace Verse
{
	// Token: 0x020001C4 RID: 452
	public abstract class MapComponent : IExposable
	{
		// Token: 0x06000D27 RID: 3367 RVA: 0x00046A1F File Offset: 0x00044C1F
		public MapComponent(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void MapComponentUpdate()
		{
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void MapComponentTick()
		{
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void MapComponentOnGUI()
		{
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ExposeData()
		{
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void FinalizeInit()
		{
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void MapGenerated()
		{
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void MapRemoved()
		{
		}

		// Token: 0x04000AD7 RID: 2775
		public Map map;
	}
}
