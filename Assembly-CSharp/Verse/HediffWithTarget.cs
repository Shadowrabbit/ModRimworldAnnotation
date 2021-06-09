using System;

namespace Verse
{
	// Token: 0x02000402 RID: 1026
	public class HediffWithTarget : HediffWithComps
	{
		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06001905 RID: 6405 RVA: 0x000E0ED8 File Offset: 0x000DF0D8
		public override string LabelBase
		{
			get
			{
				string[] array = new string[5];
				array[0] = base.LabelBase;
				array[1] = " ";
				array[2] = this.def.targetPrefix;
				array[3] = " ";
				int num = 4;
				Thing thing = this.target;
				array[num] = ((thing != null) ? thing.LabelShortCap : null);
				return string.Concat(array);
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06001906 RID: 6406 RVA: 0x000E0F2C File Offset: 0x000DF12C
		public override bool ShouldRemove
		{
			get
			{
				Pawn pawn;
				return this.target == null || ((pawn = (this.target as Pawn)) != null && pawn.Dead) || base.ShouldRemove;
			}
		}

		// Token: 0x06001907 RID: 6407 RVA: 0x00017B54 File Offset: 0x00015D54
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
		}

		// Token: 0x040012C0 RID: 4800
		public Thing target;
	}
}
