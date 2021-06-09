using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001731 RID: 5937
	public class ShuttleIncoming : Skyfaller, IActiveDropPod, IThingHolder
	{
		// Token: 0x1700145E RID: 5214
		// (get) Token: 0x060082F1 RID: 33521 RVA: 0x00057E59 File Offset: 0x00056059
		// (set) Token: 0x060082F2 RID: 33522 RVA: 0x00057E71 File Offset: 0x00056071
		public ActiveDropPodInfo Contents
		{
			get
			{
				return ((ActiveDropPod)this.innerContainer[0]).Contents;
			}
			set
			{
				((ActiveDropPod)this.innerContainer[0]).Contents = value;
			}
		}
	}
}
