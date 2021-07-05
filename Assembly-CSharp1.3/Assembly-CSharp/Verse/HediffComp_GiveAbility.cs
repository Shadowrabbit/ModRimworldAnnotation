using System;

namespace Verse
{
	// Token: 0x02000299 RID: 665
	public class HediffComp_GiveAbility : HediffComp
	{
		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06001278 RID: 4728 RVA: 0x0006A691 File Offset: 0x00068891
		private HediffCompProperties_GiveAbility Props
		{
			get
			{
				return (HediffCompProperties_GiveAbility)this.props;
			}
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x0006A69E File Offset: 0x0006889E
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			this.parent.pawn.abilities.GainAbility(this.Props.abilityDef);
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x0006A6C0 File Offset: 0x000688C0
		public override void CompPostPostRemoved()
		{
			this.parent.pawn.abilities.RemoveAbility(this.Props.abilityDef);
		}
	}
}
