using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010CD RID: 4301
	public class ShuttleIncoming : Skyfaller, IActiveDropPod, IThingHolder
	{
		// Token: 0x170011A9 RID: 4521
		// (get) Token: 0x060066E6 RID: 26342 RVA: 0x0022C0AD File Offset: 0x0022A2AD
		// (set) Token: 0x060066E7 RID: 26343 RVA: 0x0022C0C5 File Offset: 0x0022A2C5
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
