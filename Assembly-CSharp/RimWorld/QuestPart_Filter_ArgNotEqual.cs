using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001065 RID: 4197
	public class QuestPart_Filter_ArgNotEqual : QuestPart_Filter
	{
		// Token: 0x06005B4F RID: 23375 RVA: 0x001D7DA8 File Offset: 0x001D5FA8
		protected override bool Pass(SignalArgs args)
		{
			NamedArgument namedArgument;
			return !args.TryGetArg(this.name, out namedArgument) || !object.Equals(this.obj, namedArgument.arg);
		}

		// Token: 0x06005B50 RID: 23376 RVA: 0x0003F4E1 File Offset: 0x0003D6E1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Universal.Look<object>(ref this.obj, "obj", ref this.objLookMode, ref this.objType, false);
		}

		// Token: 0x06005B51 RID: 23377 RVA: 0x0003F518 File Offset: 0x0003D718
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.name = "test";
			this.obj = "value";
		}

		// Token: 0x04003D4E RID: 15694
		public string name;

		// Token: 0x04003D4F RID: 15695
		public object obj;

		// Token: 0x04003D50 RID: 15696
		private Type objType;

		// Token: 0x04003D51 RID: 15697
		private LookMode objLookMode;
	}
}
