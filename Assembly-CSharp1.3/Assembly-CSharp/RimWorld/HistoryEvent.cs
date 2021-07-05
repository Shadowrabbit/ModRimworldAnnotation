using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AFB RID: 2811
	public struct HistoryEvent
	{
		// Token: 0x0600422D RID: 16941 RVA: 0x00162679 File Offset: 0x00160879
		public HistoryEvent(HistoryEventDef def)
		{
			this.def = def;
			this.args = default(SignalArgs);
		}

		// Token: 0x0600422E RID: 16942 RVA: 0x0016268E File Offset: 0x0016088E
		public HistoryEvent(HistoryEventDef def, SignalArgs args)
		{
			this.def = def;
			this.args = args;
		}

		// Token: 0x0600422F RID: 16943 RVA: 0x0016269E File Offset: 0x0016089E
		public HistoryEvent(HistoryEventDef def, NamedArgument arg1)
		{
			this.def = def;
			this.args = new SignalArgs(arg1);
		}

		// Token: 0x06004230 RID: 16944 RVA: 0x001626B3 File Offset: 0x001608B3
		public HistoryEvent(HistoryEventDef def, NamedArgument arg1, NamedArgument arg2)
		{
			this.def = def;
			this.args = new SignalArgs(arg1, arg2);
		}

		// Token: 0x06004231 RID: 16945 RVA: 0x001626C9 File Offset: 0x001608C9
		public HistoryEvent(HistoryEventDef def, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			this.def = def;
			this.args = new SignalArgs(arg1, arg2, arg3);
		}

		// Token: 0x06004232 RID: 16946 RVA: 0x001626E1 File Offset: 0x001608E1
		public HistoryEvent(HistoryEventDef def, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			this.def = def;
			this.args = new SignalArgs(arg1, arg2, arg3, arg4);
		}

		// Token: 0x06004233 RID: 16947 RVA: 0x001626FB File Offset: 0x001608FB
		public HistoryEvent(HistoryEventDef def, params NamedArgument[] args)
		{
			this.def = def;
			this.args = new SignalArgs(args);
		}

		// Token: 0x04002852 RID: 10322
		public HistoryEventDef def;

		// Token: 0x04002853 RID: 10323
		public SignalArgs args;
	}
}
