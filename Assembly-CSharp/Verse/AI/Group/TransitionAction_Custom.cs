using System;

namespace Verse.AI.Group
{
	// Token: 0x02000AE1 RID: 2785
	public class TransitionAction_Custom : TransitionAction
	{
		// Token: 0x060041D1 RID: 16849 RVA: 0x00030F91 File Offset: 0x0002F191
		public TransitionAction_Custom(Action action)
		{
			this.action = action;
		}

		// Token: 0x060041D2 RID: 16850 RVA: 0x00030FA0 File Offset: 0x0002F1A0
		public TransitionAction_Custom(Action<Transition> actionWithArg)
		{
			this.actionWithArg = actionWithArg;
		}

		// Token: 0x060041D3 RID: 16851 RVA: 0x00030FAF File Offset: 0x0002F1AF
		public override void DoAction(Transition trans)
		{
			if (this.actionWithArg != null)
			{
				this.actionWithArg(trans);
			}
			if (this.action != null)
			{
				this.action();
			}
		}

		// Token: 0x04002D42 RID: 11586
		public Action action;

		// Token: 0x04002D43 RID: 11587
		public Action<Transition> actionWithArg;
	}
}
