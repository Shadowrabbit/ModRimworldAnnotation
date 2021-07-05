using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FD0 RID: 4048
	public abstract class RoleEffect
	{
		// Token: 0x17001063 RID: 4195
		// (get) Token: 0x06005F70 RID: 24432 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool IsBad
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005F71 RID: 24433 RVA: 0x0020AAFC File Offset: 0x00208CFC
		public virtual string Label(Pawn pawn, Precept_Role role)
		{
			if (!this.labelKey.NullOrEmpty())
			{
				return this.labelKey.Translate(pawn.Named("PAWN"), role.LabelCap);
			}
			return this.label.Formatted(pawn.Named("PAWN"), role.LabelCap);
		}

		// Token: 0x06005F72 RID: 24434 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool CanEquip(Pawn pawn, Thing thing)
		{
			return true;
		}

		// Token: 0x06005F73 RID: 24435 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_Tended(Pawn doctor, Pawn target)
		{
		}

		// Token: 0x040036DD RID: 14045
		[MustTranslate]
		public string label;

		// Token: 0x040036DE RID: 14046
		[NoTranslate]
		public string labelKey;
	}
}
