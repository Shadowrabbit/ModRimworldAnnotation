using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001167 RID: 4455
	public struct Signal
	{
		// Token: 0x060061F6 RID: 25078 RVA: 0x00043608 File Offset: 0x00041808
		public Signal(string tag)
		{
			this.tag = tag;
			this.args = default(SignalArgs);
		}

		// Token: 0x060061F7 RID: 25079 RVA: 0x0004361D File Offset: 0x0004181D
		public Signal(string tag, SignalArgs args)
		{
			this.tag = tag;
			this.args = args;
		}

		// Token: 0x060061F8 RID: 25080 RVA: 0x0004362D File Offset: 0x0004182D
		public Signal(string tag, NamedArgument arg1)
		{
			this.tag = tag;
			this.args = new SignalArgs(arg1);
		}

		// Token: 0x060061F9 RID: 25081 RVA: 0x00043642 File Offset: 0x00041842
		public Signal(string tag, NamedArgument arg1, NamedArgument arg2)
		{
			this.tag = tag;
			this.args = new SignalArgs(arg1, arg2);
		}

		// Token: 0x060061FA RID: 25082 RVA: 0x00043658 File Offset: 0x00041858
		public Signal(string tag, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			this.tag = tag;
			this.args = new SignalArgs(arg1, arg2, arg3);
		}

		// Token: 0x060061FB RID: 25083 RVA: 0x00043670 File Offset: 0x00041870
		public Signal(string tag, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			this.tag = tag;
			this.args = new SignalArgs(arg1, arg2, arg3, arg4);
		}

		// Token: 0x060061FC RID: 25084 RVA: 0x0004368A File Offset: 0x0004188A
		public Signal(string tag, params NamedArgument[] args)
		{
			this.tag = tag;
			this.args = new SignalArgs(args);
		}

		// Token: 0x04004192 RID: 16786
		public string tag;

		// Token: 0x04004193 RID: 16787
		public SignalArgs args;
	}
}
