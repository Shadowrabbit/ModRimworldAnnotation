using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B34 RID: 2868
	public class QuestPart_Filter_ArgEqual : QuestPart_Filter
	{
		// Token: 0x06004334 RID: 17204 RVA: 0x00166F78 File Offset: 0x00165178
		protected override bool Pass(SignalArgs args)
		{
			NamedArgument namedArgument;
			return args.TryGetArg(this.name, out namedArgument) && object.Equals(this.obj, namedArgument.arg);
		}

		// Token: 0x06004335 RID: 17205 RVA: 0x00166FA9 File Offset: 0x001651A9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Universal.Look<object>(ref this.obj, "obj", ref this.objLookMode, ref this.objType, false);
		}

		// Token: 0x06004336 RID: 17206 RVA: 0x00166FE0 File Offset: 0x001651E0
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.name = "test";
			this.obj = "value";
		}

		// Token: 0x040028DE RID: 10462
		public string name;

		// Token: 0x040028DF RID: 10463
		public object obj;

		// Token: 0x040028E0 RID: 10464
		public LookMode objLookMode;

		// Token: 0x040028E1 RID: 10465
		private Type objType;
	}
}
