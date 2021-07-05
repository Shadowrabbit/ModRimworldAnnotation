using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001118 RID: 4376
	public class CompCanBeDormant : ThingComp
	{
		// Token: 0x170011F7 RID: 4599
		// (get) Token: 0x06006910 RID: 26896 RVA: 0x00237122 File Offset: 0x00235322
		private CompProperties_CanBeDormant Props
		{
			get
			{
				return (CompProperties_CanBeDormant)this.props;
			}
		}

		// Token: 0x170011F8 RID: 4600
		// (get) Token: 0x06006911 RID: 26897 RVA: 0x0023712F File Offset: 0x0023532F
		private bool WaitingToWakeUp
		{
			get
			{
				return this.wakeUpOnTick != int.MinValue;
			}
		}

		// Token: 0x170011F9 RID: 4601
		// (get) Token: 0x06006912 RID: 26898 RVA: 0x00237141 File Offset: 0x00235341
		public bool Awake
		{
			get
			{
				return this.wokeUpTick != int.MinValue && this.wokeUpTick <= Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06006913 RID: 26899 RVA: 0x00237167 File Offset: 0x00235367
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.makeTick = GenTicks.TicksGame;
			if (!this.Props.startsDormant)
			{
				this.WakeUp();
			}
		}

		// Token: 0x06006914 RID: 26900 RVA: 0x0023718D File Offset: 0x0023538D
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.wakeUpSignalTag = this.Props.wakeUpSignalTag;
		}

		// Token: 0x06006915 RID: 26901 RVA: 0x002371A7 File Offset: 0x002353A7
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!Prefs.DevMode)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = "DEV: Wake Up",
				action = delegate()
				{
					this.WakeUp();
				}
			};
			yield break;
		}

		// Token: 0x06006916 RID: 26902 RVA: 0x002371B8 File Offset: 0x002353B8
		public override string CompInspectStringExtra()
		{
			if (!this.Awake)
			{
				return this.Props.dormantStateLabelKey.Translate();
			}
			if (this.makeTick != this.wokeUpTick)
			{
				return this.Props.awakeStateLabelKey.Translate((GenTicks.TicksGame - this.wokeUpTick).TicksToDays().ToString("0.#"));
			}
			return null;
		}

		// Token: 0x06006917 RID: 26903 RVA: 0x0023722B File Offset: 0x0023542B
		public void WakeUpWithDelay()
		{
			if (!this.Awake)
			{
				this.wakeUpOnTick = Find.TickManager.TicksGame + Rand.Range(60, 300);
			}
		}

		// Token: 0x06006918 RID: 26904 RVA: 0x00237254 File Offset: 0x00235454
		public void WakeUp()
		{
			if (this.Awake)
			{
				return;
			}
			this.wokeUpTick = GenTicks.TicksGame;
			this.wakeUpOnTick = int.MinValue;
			Pawn pawn = this.parent as Pawn;
			Building building = this.parent as Building;
			Lord lord = ((pawn != null) ? pawn.GetLord() : null) ?? ((building != null) ? building.GetLord() : null);
			if (lord != null)
			{
				lord.Notify_DormancyWakeup();
			}
			if (this.parent.Spawned)
			{
				IAttackTarget attackTarget = this.parent as IAttackTarget;
				if (attackTarget != null)
				{
					this.parent.Map.attackTargetsCache.UpdateTarget(attackTarget);
				}
			}
		}

		// Token: 0x06006919 RID: 26905 RVA: 0x002372F0 File Offset: 0x002354F0
		public void ToSleep()
		{
			if (!this.Awake)
			{
				return;
			}
			this.wokeUpTick = int.MinValue;
			if (this.parent.Spawned)
			{
				IAttackTarget attackTarget = this.parent as IAttackTarget;
				if (attackTarget != null)
				{
					this.parent.Map.attackTargetsCache.UpdateTarget(attackTarget);
				}
			}
		}

		// Token: 0x0600691A RID: 26906 RVA: 0x00237343 File Offset: 0x00235543
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (this.wakeUpOnTick != -2147483648 && Find.TickManager.TicksGame >= this.wakeUpOnTick)
			{
				this.WakeUp();
			}
			this.TickRareWorker();
		}

		// Token: 0x0600691B RID: 26907 RVA: 0x00237378 File Offset: 0x00235578
		public override void CompTick()
		{
			base.CompTick();
			if (this.wakeUpOnTick != -2147483648 && Find.TickManager.TicksGame >= this.wakeUpOnTick)
			{
				this.WakeUp();
			}
			if (this.parent.IsHashIntervalTick(250))
			{
				this.TickRareWorker();
			}
		}

		// Token: 0x0600691C RID: 26908 RVA: 0x002373C8 File Offset: 0x002355C8
		public void TickRareWorker()
		{
			if (!this.parent.Spawned || this.Awake)
			{
				return;
			}
			if (!(this.parent is Pawn) && !this.parent.Position.Fogged(this.parent.Map))
			{
				FleckMaker.ThrowMetaIcon(this.parent.Position, this.parent.Map, FleckDefOf.SleepZ, 0.42f);
			}
		}

		// Token: 0x0600691D RID: 26909 RVA: 0x0023743C File Offset: 0x0023563C
		public override void Notify_SignalReceived(Signal signal)
		{
			if (string.IsNullOrEmpty(this.wakeUpSignalTag) || this.Awake)
			{
				return;
			}
			Thing thing;
			Faction faction;
			if ((signal.tag == this.wakeUpSignalTag || (this.wakeUpSignalTags != null && this.wakeUpSignalTags.Contains(signal.tag))) && signal.args.TryGetArg<Thing>("SUBJECT", out thing) && thing != this.parent && thing != null && thing.Map == this.parent.Map && this.parent.Position.DistanceTo(thing.Position) <= this.Props.maxDistAwakenByOther && (!signal.args.TryGetArg<Faction>("FACTION", out faction) || faction == null || faction == this.parent.Faction) && (this.Props.canWakeUpFogged || !this.parent.Fogged()) && !this.WaitingToWakeUp)
			{
				this.WakeUpWithDelay();
			}
		}

		// Token: 0x0600691E RID: 26910 RVA: 0x00237544 File Offset: 0x00235744
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.wokeUpTick, "wokeUpTick", int.MinValue, false);
			Scribe_Values.Look<int>(ref this.wakeUpOnTick, "wakeUpOnTick", int.MinValue, false);
			Scribe_Values.Look<string>(ref this.wakeUpSignalTag, "wakeUpSignalTag", null, false);
			Scribe_Collections.Look<string>(ref this.wakeUpSignalTags, "wakeUpSignalTags", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.makeTick, "makeTick", 0, false);
		}

		// Token: 0x04003AD5 RID: 15061
		public int makeTick;

		// Token: 0x04003AD6 RID: 15062
		public int wokeUpTick = int.MinValue;

		// Token: 0x04003AD7 RID: 15063
		public int wakeUpOnTick = int.MinValue;

		// Token: 0x04003AD8 RID: 15064
		public string wakeUpSignalTag;

		// Token: 0x04003AD9 RID: 15065
		public List<string> wakeUpSignalTags;

		// Token: 0x04003ADA RID: 15066
		public const string DefaultWakeUpSignal = "CompCanBeDormant.WakeUp";
	}
}
