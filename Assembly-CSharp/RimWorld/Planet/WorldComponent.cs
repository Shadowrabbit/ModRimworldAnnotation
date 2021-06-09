using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002089 RID: 8329
	public abstract class WorldComponent : IExposable
	{
		// Token: 0x0600B09E RID: 45214 RVA: 0x00072D38 File Offset: 0x00070F38
		public WorldComponent(World world)
		{
			this.world = world;
		}

		// Token: 0x0600B09F RID: 45215 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void WorldComponentUpdate()
		{
		}

		// Token: 0x0600B0A0 RID: 45216 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void WorldComponentTick()
		{
		}

		// Token: 0x0600B0A1 RID: 45217 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void ExposeData()
		{
		}

		// Token: 0x0600B0A2 RID: 45218 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void FinalizeInit()
		{
		}

		// Token: 0x04007995 RID: 31125
		public World world;
	}
}
