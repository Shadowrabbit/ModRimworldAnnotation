using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200167F RID: 5759
	public abstract class Building_Turret : Building, IAttackTarget, ILoadReferenceable, IAttackTargetSearcher
	{
		// Token: 0x17001357 RID: 4951
		// (get) Token: 0x06007DAA RID: 32170
		public abstract LocalTargetInfo CurrentTarget { get; }

		// Token: 0x17001358 RID: 4952
		// (get) Token: 0x06007DAB RID: 32171
		public abstract Verb AttackVerb { get; }

		// Token: 0x17001359 RID: 4953
		// (get) Token: 0x06007DAC RID: 32172 RVA: 0x000187F7 File Offset: 0x000169F7
		Thing IAttackTarget.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700135A RID: 4954
		// (get) Token: 0x06007DAD RID: 32173 RVA: 0x00054789 File Offset: 0x00052989
		public LocalTargetInfo TargetCurrentlyAimingAt
		{
			get
			{
				return this.CurrentTarget;
			}
		}

		// Token: 0x1700135B RID: 4955
		// (get) Token: 0x06007DAE RID: 32174 RVA: 0x000187F7 File Offset: 0x000169F7
		Thing IAttackTargetSearcher.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700135C RID: 4956
		// (get) Token: 0x06007DAF RID: 32175 RVA: 0x00054791 File Offset: 0x00052991
		public Verb CurrentEffectiveVerb
		{
			get
			{
				return this.AttackVerb;
			}
		}

		// Token: 0x1700135D RID: 4957
		// (get) Token: 0x06007DB0 RID: 32176 RVA: 0x00054799 File Offset: 0x00052999
		public LocalTargetInfo LastAttackedTarget
		{
			get
			{
				return this.lastAttackedTarget;
			}
		}

		// Token: 0x1700135E RID: 4958
		// (get) Token: 0x06007DB1 RID: 32177 RVA: 0x000547A1 File Offset: 0x000529A1
		public int LastAttackTargetTick
		{
			get
			{
				return this.lastAttackTargetTick;
			}
		}

		// Token: 0x1700135F RID: 4959
		// (get) Token: 0x06007DB2 RID: 32178 RVA: 0x0000CE6C File Offset: 0x0000B06C
		public float TargetPriorityFactor
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06007DB3 RID: 32179 RVA: 0x000547A9 File Offset: 0x000529A9
		public Building_Turret()
		{
			this.stunner = new StunHandler(this);
		}

		// Token: 0x06007DB4 RID: 32180 RVA: 0x00257C10 File Offset: 0x00255E10
		public override void Tick()
		{
			base.Tick();
			if (this.forcedTarget.HasThing && (!this.forcedTarget.Thing.Spawned || !base.Spawned || this.forcedTarget.Thing.Map != base.Map))
			{
				this.forcedTarget = LocalTargetInfo.Invalid;
			}
			this.stunner.StunHandlerTick();
		}

		// Token: 0x06007DB5 RID: 32181 RVA: 0x00257C78 File Offset: 0x00255E78
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_TargetInfo.Look(ref this.forcedTarget, "forcedTarget");
			Scribe_TargetInfo.Look(ref this.lastAttackedTarget, "lastAttackedTarget");
			Scribe_Deep.Look<StunHandler>(ref this.stunner, "stunner", new object[]
			{
				this
			});
			Scribe_Values.Look<int>(ref this.lastAttackTargetTick, "lastAttackTargetTick", 0, false);
		}

		// Token: 0x06007DB6 RID: 32182 RVA: 0x000547C8 File Offset: 0x000529C8
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (absorbed)
			{
				return;
			}
			this.stunner.Notify_DamageApplied(dinfo, true);
			absorbed = false;
		}

		// Token: 0x06007DB7 RID: 32183
		public abstract void OrderAttack(LocalTargetInfo targ);

		// Token: 0x06007DB8 RID: 32184 RVA: 0x00257CD8 File Offset: 0x00255ED8
		public bool ThreatDisabled(IAttackTargetSearcher disabledFor)
		{
			CompPowerTrader comp = base.GetComp<CompPowerTrader>();
			if (comp != null && !comp.PowerOn)
			{
				return true;
			}
			CompMannable comp2 = base.GetComp<CompMannable>();
			if (comp2 != null && !comp2.MannedNow)
			{
				return true;
			}
			CompCanBeDormant comp3 = base.GetComp<CompCanBeDormant>();
			if (comp3 != null && !comp3.Awake)
			{
				return true;
			}
			CompInitiatable comp4 = base.GetComp<CompInitiatable>();
			return comp4 != null && !comp4.Initiated;
		}

		// Token: 0x06007DB9 RID: 32185 RVA: 0x000547EC File Offset: 0x000529EC
		protected void OnAttackedTarget(LocalTargetInfo target)
		{
			this.lastAttackTargetTick = Find.TickManager.TicksGame;
			this.lastAttackedTarget = target;
		}

		// Token: 0x040051F1 RID: 20977
		protected StunHandler stunner;

		// Token: 0x040051F2 RID: 20978
		protected LocalTargetInfo forcedTarget = LocalTargetInfo.Invalid;

		// Token: 0x040051F3 RID: 20979
		private LocalTargetInfo lastAttackedTarget;

		// Token: 0x040051F4 RID: 20980
		private int lastAttackTargetTick;

		// Token: 0x040051F5 RID: 20981
		private const float SightRadiusTurret = 13.4f;
	}
}
