using System;

namespace Verse
{
	// Token: 0x02000284 RID: 644
	public abstract class MapComponent : IExposable
	{
		// Token: 0x060010D3 RID: 4307 RVA: 0x000126BB File Offset: 0x000108BB
		public MapComponent(Map map)
		{
			this.map = map;
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void MapComponentUpdate()
		{
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void MapComponentTick()
		{
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void MapComponentOnGUI()
		{
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void ExposeData()
		{
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void FinalizeInit()
		{
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void MapGenerated()
		{
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void MapRemoved()
		{
		}

		// Token: 0x04000DB3 RID: 3507
		public Map map;
	}
}
