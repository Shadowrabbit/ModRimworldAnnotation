using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200135E RID: 4958
	public abstract class AbilityComp
	{
		// Token: 0x06006BD6 RID: 27606 RVA: 0x000495C8 File Offset: 0x000477C8
		public virtual void Initialize(AbilityCompProperties props)
		{
			this.props = props;
		}

		// Token: 0x06006BD7 RID: 27607 RVA: 0x00213010 File Offset: 0x00211210
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

		// Token: 0x06006BD8 RID: 27608 RVA: 0x0001C8FF File Offset: 0x0001AAFF
		public virtual bool GizmoDisabled(out string reason)
		{
			reason = null;
			return false;
		}

		// Token: 0x06006BD9 RID: 27609 RVA: 0x00016647 File Offset: 0x00014847
		public virtual float PsyfocusCostForTarget(LocalTargetInfo target)
		{
			return 0f;
		}

		// Token: 0x040047A4 RID: 18340
		public Ability parent;

		// Token: 0x040047A5 RID: 18341
		public AbilityCompProperties props;
	}
}
