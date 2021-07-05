using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BD1 RID: 3025
	public struct Signal
	{
		// Token: 0x060046FF RID: 18175 RVA: 0x00177B7E File Offset: 0x00175D7E
		public Signal(string tag)
		{
			this.tag = tag;
			this.args = default(SignalArgs);
		}

		// Token: 0x06004700 RID: 18176 RVA: 0x00177B93 File Offset: 0x00175D93
		public Signal(string tag, SignalArgs args)
		{
			this.tag = tag;
			this.args = args;
		}

		// Token: 0x06004701 RID: 18177 RVA: 0x00177BA3 File Offset: 0x00175DA3
		public Signal(string tag, NamedArgument arg1)
		{
			this.tag = tag;
			this.args = new SignalArgs(arg1);
		}

		// Token: 0x06004702 RID: 18178 RVA: 0x00177BB8 File Offset: 0x00175DB8
		public Signal(string tag, NamedArgument arg1, NamedArgument arg2)
		{
			this.tag = tag;
			this.args = new SignalArgs(arg1, arg2);
		}

		// Token: 0x06004703 RID: 18179 RVA: 0x00177BCE File Offset: 0x00175DCE
		public Signal(string tag, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			this.tag = tag;
			this.args = new SignalArgs(arg1, arg2, arg3);
		}

		// Token: 0x06004704 RID: 18180 RVA: 0x00177BE6 File Offset: 0x00175DE6
		public Signal(string tag, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			this.tag = tag;
			this.args = new SignalArgs(arg1, arg2, arg3, arg4);
		}

		// Token: 0x06004705 RID: 18181 RVA: 0x00177C00 File Offset: 0x00175E00
		public Signal(string tag, params NamedArgument[] args)
		{
			this.tag = tag;
			this.args = new SignalArgs(args);
		}

		// Token: 0x04002B82 RID: 11138
		public string tag;

		// Token: 0x04002B83 RID: 11139
		public SignalArgs args;
	}
}
