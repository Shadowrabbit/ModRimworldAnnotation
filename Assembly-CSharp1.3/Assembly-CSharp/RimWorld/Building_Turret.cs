using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001053 RID: 4179
	public abstract class Building_Turret : Building, IAttackTarget, ILoadReferenceable, IAttackTargetSearcher
	{
		// Token: 0x170010CB RID: 4299
		// (get) Token: 0x060062DA RID: 25306
		public abstract LocalTargetInfo CurrentTarget { get; }

		// Token: 0x170010CC RID: 4300
		// (get) Token: 0x060062DB RID: 25307
		public abstract Verb AttackVerb { get; }

		// Token: 0x170010CD RID: 4301
		// (get) Token: 0x060062DC RID: 25308 RVA: 0x00072AAA File Offset: 0x00070CAA
		Thing IAttackTarget.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170010CE RID: 4302
		// (get) Token: 0x060062DD RID: 25309 RVA: 0x00217D8E File Offset: 0x00215F8E
		public LocalTargetInfo TargetCurrentlyAimingAt
		{
			get
			{
				return this.CurrentTarget;
			}
		}

		// Token: 0x170010CF RID: 4303
		// (get) Token: 0x060062DE RID: 25310 RVA: 0x00072AAA File Offset: 0x00070CAA
		Thing IAttackTargetSearcher.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170010D0 RID: 4304
		// (get) Token: 0x060062DF RID: 25311 RVA: 0x00217D96 File Offset: 0x00215F96
		public Verb CurrentEffectiveVerb
		{
			get
			{
				return this.AttackVerb;
			}
		}

		// Token: 0x170010D1 RID: 4305
		// (get) Token: 0x060062E0 RID: 25312 RVA: 0x00217D9E File Offset: 0x00215F9E
		public LocalTargetInfo LastAttackedTarget
		{
			get
			{
				return this.lastAttackedTarget;
			}
		}

		// Token: 0x170010D2 RID: 4306
		// (get) Token: 0x060062E1 RID: 25313 RVA: 0x00217DA6 File Offset: 0x00215FA6
		public int LastAttackTargetTick
		{
			get
			{
				return this.lastAttackTargetTick;
			}
		}

		// Token: 0x170010D3 RID: 4307
		// (get) Token: 0x060062E2 RID: 25314 RVA: 0x0001F15E File Offset: 0x0001D35E
		public float TargetPriorityFactor
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x060062E3 RID: 25315 RVA: 0x00217DAE File Offset: 0x00215FAE
		public Building_Turret()
		{
			this.stunner = new StunHandler(this);
		}

		// Token: 0x060062E4 RID: 25316 RVA: 0x00217DD0 File Offset: 0x00215FD0
		public override void Tick()
		{
			base.Tick();
			if (this.forcedTarget.HasThing && (!this.forcedTarget.Thing.Spawned || !base.Spawned || this.forcedTarget.Thing.Map != base.Map))
			{
				this.forcedTarget = LocalTargetInfo.Invalid;
			}
			this.stunner.StunHandlerTick();
		}

		// Token: 0x060062E5 RID: 25317 RVA: 0x00217E38 File Offset: 0x00216038
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

		// Token: 0x060062E6 RID: 25318 RVA: 0x00217E97 File Offset: 0x00216097
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (absorbed)
			{
				return;
			}
			this.stunner.Notify_DamageApplied(dinfo);
			absorbed = false;
		}

		// Token: 0x060062E7 RID: 25319
		public abstract void OrderAttack(LocalTargetInfo targ);

		// Token: 0x060062E8 RID: 25320 RVA: 0x00217EBC File Offset: 0x002160BC
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

		// Token: 0x060062E9 RID: 25321 RVA: 0x00217F1A File Offset: 0x0021611A
		protected void OnAttackedTarget(LocalTargetInfo target)
		{
			this.lastAttackTargetTick = Find.TickManager.TicksGame;
			this.lastAttackedTarget = target;
		}

		// Token: 0x04003824 RID: 14372
		protected StunHandler stunner;

		// Token: 0x04003825 RID: 14373
		protected LocalTargetInfo forcedTarget = LocalTargetInfo.Invalid;

		// Token: 0x04003826 RID: 14374
		private LocalTargetInfo lastAttackedTarget;

		// Token: 0x04003827 RID: 14375
		private int lastAttackTargetTick;

		// Token: 0x04003828 RID: 14376
		private const float SightRadiusTurret = 13.4f;
	}
}
