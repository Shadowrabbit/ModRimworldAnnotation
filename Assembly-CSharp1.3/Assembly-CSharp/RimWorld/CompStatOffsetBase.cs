using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020011AA RID: 4522
	public abstract class CompStatOffsetBase : ThingComp
	{
		// Token: 0x170012DE RID: 4830
		// (get) Token: 0x06006CEC RID: 27884 RVA: 0x00249156 File Offset: 0x00247356
		public CompProperties_StatOffsetBase Props
		{
			get
			{
				return (CompProperties_StatOffsetBase)this.props;
			}
		}

		// Token: 0x170012DF RID: 4831
		// (get) Token: 0x06006CED RID: 27885 RVA: 0x00249163 File Offset: 0x00247363
		public Pawn LastUser
		{
			get
			{
				return this.lastUser;
			}
		}

		// Token: 0x06006CEE RID: 27886
		public abstract float GetStatOffset(Pawn pawn = null);

		// Token: 0x06006CEF RID: 27887
		public abstract IEnumerable<string> GetExplanation();

		// Token: 0x06006CF0 RID: 27888 RVA: 0x0024916B File Offset: 0x0024736B
		public void Used(Pawn pawn)
		{
			this.lastUser = pawn;
		}

		// Token: 0x06006CF1 RID: 27889 RVA: 0x00249174 File Offset: 0x00247374
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Pawn>(ref this.lastUser, "lastUser", false);
		}

		// Token: 0x04003C9C RID: 15516
		protected Pawn lastUser;
	}
}
