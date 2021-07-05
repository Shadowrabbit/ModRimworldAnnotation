using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200177C RID: 6012
	public abstract class WorldComponent : IExposable
	{
		// Token: 0x06008AB9 RID: 35513 RVA: 0x0031C729 File Offset: 0x0031A929
		public WorldComponent(World world)
		{
			this.world = world;
		}

		// Token: 0x06008ABA RID: 35514 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void WorldComponentUpdate()
		{
		}

		// Token: 0x06008ABB RID: 35515 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void WorldComponentTick()
		{
		}

		// Token: 0x06008ABC RID: 35516 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ExposeData()
		{
		}

		// Token: 0x06008ABD RID: 35517 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void FinalizeInit()
		{
		}

		// Token: 0x04005858 RID: 22616
		public World world;
	}
}
