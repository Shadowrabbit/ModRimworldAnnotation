using System;

namespace Verse
{
	// Token: 0x020002C5 RID: 709
	public class HediffWithTarget : HediffWithComps
	{
		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06001334 RID: 4916 RVA: 0x0006D244 File Offset: 0x0006B444
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

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06001335 RID: 4917 RVA: 0x0006D298 File Offset: 0x0006B498
		public override bool ShouldRemove
		{
			get
			{
				Pawn pawn;
				return this.target == null || ((pawn = (this.target as Pawn)) != null && pawn.Dead) || base.ShouldRemove;
			}
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x0006D2CC File Offset: 0x0006B4CC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
		}

		// Token: 0x04000E50 RID: 3664
		public Thing target;
	}
}
