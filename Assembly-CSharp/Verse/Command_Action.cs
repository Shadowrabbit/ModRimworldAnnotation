using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000710 RID: 1808
	public class Command_Action : Command
	{
		// Token: 0x06002DC7 RID: 11719 RVA: 0x00024116 File Offset: 0x00022316
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			this.action();
		}

		// Token: 0x04001F34 RID: 7988
		public Action action;
	}
}
