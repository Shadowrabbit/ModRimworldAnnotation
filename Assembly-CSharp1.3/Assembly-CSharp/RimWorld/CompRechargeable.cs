using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200117A RID: 4474
	public class CompRechargeable : ThingComp
	{
		// Token: 0x17001283 RID: 4739
		// (get) Token: 0x06006B7E RID: 27518 RVA: 0x002417C6 File Offset: 0x0023F9C6
		private CompProperties_Rechargeable Props
		{
			get
			{
				return (CompProperties_Rechargeable)this.props;
			}
		}

		// Token: 0x17001284 RID: 4740
		// (get) Token: 0x06006B7F RID: 27519 RVA: 0x002417D3 File Offset: 0x0023F9D3
		public bool Charged
		{
			get
			{
				return this.ticksUntilCharged == 0;
			}
		}

		// Token: 0x06006B80 RID: 27520 RVA: 0x002417DE File Offset: 0x0023F9DE
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.ticksUntilCharged = this.Props.ticksToRecharge;
		}

		// Token: 0x06006B81 RID: 27521 RVA: 0x002417F8 File Offset: 0x0023F9F8
		public override void CompTick()
		{
			base.CompTick();
			if (this.compPowerCached == null)
			{
				this.compPowerCached = this.parent.TryGetComp<CompPowerTrader>();
			}
			if (this.ticksUntilCharged > 0 && this.compPowerCached.PowerOn)
			{
				if (this.ticksUntilCharged == 1 && this.Props.chargedSoundDef != null)
				{
					this.Props.chargedSoundDef.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
				}
				this.ticksUntilCharged--;
			}
			if (!this.Charged)
			{
				if (this.progressBar == null)
				{
					this.progressBar = EffecterDefOf.ProgressBarAlwaysVisible.Spawn();
				}
				this.progressBar.EffectTick(this.parent, TargetInfo.Invalid);
				MoteProgressBar mote = ((SubEffecter_ProgressBar)this.progressBar.children[0]).mote;
				if (mote != null)
				{
					mote.progress = 1f - Mathf.Clamp01((float)this.ticksUntilCharged / (float)this.Props.ticksToRecharge);
					mote.offsetZ = -0.5f;
					return;
				}
			}
			else if (this.progressBar != null)
			{
				this.progressBar.Cleanup();
				this.progressBar = null;
			}
		}

		// Token: 0x06006B82 RID: 27522 RVA: 0x00241938 File Offset: 0x0023FB38
		public void Discharge()
		{
			this.ticksUntilCharged = this.Props.ticksToRecharge;
			if (this.Props.dischargeSoundDef != null)
			{
				this.Props.dischargeSoundDef.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
			}
		}

		// Token: 0x06006B83 RID: 27523 RVA: 0x00241994 File Offset: 0x0023FB94
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (Prefs.DevMode)
			{
				if (!this.Charged)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEBUG: Recharge",
						action = delegate()
						{
							if (this.Props.chargedSoundDef != null)
							{
								this.Props.chargedSoundDef.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
							}
							this.ticksUntilCharged = 0;
						}
					};
				}
				else
				{
					yield return new Command_Action
					{
						defaultLabel = "DEBUG: Discharge",
						action = new Action(this.Discharge)
					};
				}
			}
			yield break;
		}

		// Token: 0x06006B84 RID: 27524 RVA: 0x002419A4 File Offset: 0x0023FBA4
		public override string CompInspectStringExtra()
		{
			if (this.Charged)
			{
				return "RechargeableReady".Translate() + ".";
			}
			return "RechargeableCharging".Translate() + ": " + "DurationLeft".Translate(this.ticksUntilCharged.ToStringTicksToPeriod(true, true, true, true)) + ".";
		}

		// Token: 0x06006B85 RID: 27525 RVA: 0x00241A19 File Offset: 0x0023FC19
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksUntilCharged, "ticksUntilCharged", 0, false);
		}

		// Token: 0x04003BCD RID: 15309
		private int ticksUntilCharged;

		// Token: 0x04003BCE RID: 15310
		private Effecter progressBar;

		// Token: 0x04003BCF RID: 15311
		private CompPowerTrader compPowerCached;
	}
}
