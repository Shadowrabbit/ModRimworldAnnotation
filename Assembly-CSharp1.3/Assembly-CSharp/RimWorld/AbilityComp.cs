using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D16 RID: 3350
	public abstract class AbilityComp
	{
		// Token: 0x06004E9E RID: 20126 RVA: 0x001A598A File Offset: 0x001A3B8A
		public virtual void Initialize(AbilityCompProperties props)
		{
			this.props = props;
		}

		// Token: 0x06004E9F RID: 20127 RVA: 0x001A5994 File Offset: 0x001A3B94
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				base.GetType().Name,
				"(parent=",
				this.parent,
				" at=",
				(this.parent != null) ? this.parent.pawn.Position : IntVec3.Invalid,
				")"
			});
		}

		// Token: 0x06004EA0 RID: 20128 RVA: 0x00095CB9 File Offset: 0x00093EB9
		public virtual bool GizmoDisabled(out string reason)
		{
			reason = null;
			return false;
		}

		// Token: 0x06004EA1 RID: 20129 RVA: 0x000682C5 File Offset: 0x000664C5
		public virtual float PsyfocusCostForTarget(LocalTargetInfo target)
		{
			return 0f;
		}

		// Token: 0x04002F57 RID: 12119
		public Ability parent;

		// Token: 0x04002F58 RID: 12120
		public AbilityCompProperties props;
	}
}
