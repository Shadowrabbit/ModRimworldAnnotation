using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B35 RID: 2869
	public class QuestPart_Filter_ArgNotEqual : QuestPart_Filter
	{
		// Token: 0x06004338 RID: 17208 RVA: 0x00167000 File Offset: 0x00165200
		protected override bool Pass(SignalArgs args)
		{
			NamedArgument namedArgument;
			return !args.TryGetArg(this.name, out namedArgument) || !object.Equals(this.obj, namedArgument.arg);
		}

		// Token: 0x06004339 RID: 17209 RVA: 0x00167034 File Offset: 0x00165234
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Universal.Look<object>(ref this.obj, "obj", ref this.objLookMode, ref this.objType, false);
		}

		// Token: 0x0600433A RID: 17210 RVA: 0x0016706B File Offset: 0x0016526B
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.name = "test";
			this.obj = "value";
		}

		// Token: 0x040028E2 RID: 10466
		public string name;

		// Token: 0x040028E3 RID: 10467
		public object obj;

		// Token: 0x040028E4 RID: 10468
		private Type objType;

		// Token: 0x040028E5 RID: 10469
		private LookMode objLookMode;
	}
}
