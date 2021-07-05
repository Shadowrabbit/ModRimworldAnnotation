using System;

namespace Verse.AI.Group
{
	// Token: 0x0200067A RID: 1658
	public class TransitionAction_Custom : TransitionAction
	{
		// Token: 0x06002EF0 RID: 12016 RVA: 0x00117AC9 File Offset: 0x00115CC9
		public TransitionAction_Custom(Action action)
		{
			this.action = action;
		}

		// Token: 0x06002EF1 RID: 12017 RVA: 0x00117AD8 File Offset: 0x00115CD8
		public TransitionAction_Custom(Action<Transition> actionWithArg)
		{
			this.actionWithArg = actionWithArg;
		}

		// Token: 0x06002EF2 RID: 12018 RVA: 0x00117AE7 File Offset: 0x00115CE7
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

		// Token: 0x04001CB1 RID: 7345
		public Action action;

		// Token: 0x04001CB2 RID: 7346
		public Action<Transition> actionWithArg;
	}
}
