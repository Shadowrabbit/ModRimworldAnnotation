using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001064 RID: 4196
	public class QuestPart_Filter_ArgEqual : QuestPart_Filter
	{
		// Token: 0x06005B4B RID: 23371 RVA: 0x001D7D74 File Offset: 0x001D5F74
		protected override bool Pass(SignalArgs args)
		{
			NamedArgument namedArgument;
			return args.TryGetArg(this.name, out namedArgument) && object.Equals(this.obj, namedArgument.arg);
		}

		// Token: 0x06005B4C RID: 23372 RVA: 0x0003F48C File Offset: 0x0003D68C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Universal.Look<object>(ref this.obj, "obj", ref this.objLookMode, ref this.objType, false);
		}

		// Token: 0x06005B4D RID: 23373 RVA: 0x0003F4C3 File Offset: 0x0003D6C3
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.name = "test";
			this.obj = "value";
		}

		// Token: 0x04003D4A RID: 15690
		public string name;

		// Token: 0x04003D4B RID: 15691
		public object obj;

		// Token: 0x04003D4C RID: 15692
		public LookMode objLookMode;

		// Token: 0x04003D4D RID: 15693
		private Type objType;
	}
}
