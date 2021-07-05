using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000053 RID: 83
	public class RitualStageOnTickActions : IExposable
	{
		// Token: 0x060003D7 RID: 983 RVA: 0x00014F43 File Offset: 0x00013143
		public void ExposeData()
		{
			Scribe_Collections.Look<ActionOnTick>(ref this.actions, "actions", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x04000124 RID: 292
		public List<ActionOnTick> actions = new List<ActionOnTick>();
	}
}
