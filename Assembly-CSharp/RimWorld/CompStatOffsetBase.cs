using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200186A RID: 6250
	public abstract class CompStatOffsetBase : ThingComp
	{
		// Token: 0x170015C8 RID: 5576
		// (get) Token: 0x06008AAD RID: 35501 RVA: 0x0005CFBE File Offset: 0x0005B1BE
		public CompProperties_StatOffsetBase Props
		{
			get
			{
				return (CompProperties_StatOffsetBase)this.props;
			}
		}

		// Token: 0x170015C9 RID: 5577
		// (get) Token: 0x06008AAE RID: 35502 RVA: 0x0005CFCB File Offset: 0x0005B1CB
		public Pawn LastUser
		{
			get
			{
				return this.lastUser;
			}
		}

		// Token: 0x06008AAF RID: 35503
		public abstract float GetStatOffset(Pawn pawn = null);

		// Token: 0x06008AB0 RID: 35504
		public abstract IEnumerable<string> GetExplanation();

		// Token: 0x06008AB1 RID: 35505 RVA: 0x0005CFD3 File Offset: 0x0005B1D3
		public void Used(Pawn pawn)
		{
			this.lastUser = pawn;
		}

		// Token: 0x06008AB2 RID: 35506 RVA: 0x0005CFDC File Offset: 0x0005B1DC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Pawn>(ref this.lastUser, "lastUser", false);
		}

		// Token: 0x040058F6 RID: 22774
		protected Pawn lastUser;
	}
}
