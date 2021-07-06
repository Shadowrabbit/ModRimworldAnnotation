using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001711 RID: 5905
	public class StunHandler : IExposable
	{
		// Token: 0x17001429 RID: 5161
		// (get) Token: 0x060081F9 RID: 33273 RVA: 0x000574CD File Offset: 0x000556CD
		public bool Stunned
		{
			get
			{
				return this.stunTicksLeft > 0;
			}
		}

		// Token: 0x1700142A RID: 5162
		// (get) Token: 0x060081FA RID: 33274 RVA: 0x0026854C File Offset: 0x0026674C
		private int EMPAdaptationTicksDuration
		{
			get
			{
				Pawn pawn = this.parent as Pawn;
				if (pawn != null && pawn.RaceProps.IsMechanoid)
				{
					return 2200;
				}
				return 0;
			}
		}

		// Token: 0x1700142B RID: 5163
		// (get) Token: 0x060081FB RID: 33275 RVA: 0x0026857C File Offset: 0x0026677C
		private bool AffectedByEMP
		{
			get
			{
				Pawn pawn;
				return (pawn = (this.parent as Pawn)) == null || !pawn.RaceProps.IsFlesh;
			}
		}

		// Token: 0x1700142C RID: 5164
		// (get) Token: 0x060081FC RID: 33276 RVA: 0x000574D8 File Offset: 0x000556D8
		public int StunTicksLeft
		{
			get
			{
				return this.stunTicksLeft;
			}
		}

		// Token: 0x060081FD RID: 33277 RVA: 0x000574E0 File Offset: 0x000556E0
		public StunHandler(Thing parent)
		{
			this.parent = parent;
		}

		// Token: 0x060081FE RID: 33278 RVA: 0x002685A8 File Offset: 0x002667A8
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.stunTicksLeft, "stunTicksLeft", 0, false);
			Scribe_Values.Look<bool>(ref this.showStunMote, "showStunMote", false, false);
			Scribe_Values.Look<int>(ref this.EMPAdaptedTicksLeft, "EMPAdaptedTicksLeft", 0, false);
			Scribe_Values.Look<bool>(ref this.stunFromEMP, "stunFromEMP", false, false);
		}

		// Token: 0x060081FF RID: 33279 RVA: 0x00268600 File Offset: 0x00266800
		public void StunHandlerTick()
		{
			if (this.EMPAdaptedTicksLeft > 0)
			{
				this.EMPAdaptedTicksLeft--;
			}
			if (this.stunTicksLeft > 0)
			{
				this.stunTicksLeft--;
				if (this.showStunMote && (this.moteStun == null || this.moteStun.Destroyed))
				{
					this.moteStun = MoteMaker.MakeStunOverlay(this.parent);
				}
				Pawn pawn = this.parent as Pawn;
				if (pawn != null && pawn.Downed)
				{
					this.stunTicksLeft = 0;
				}
				if (this.moteStun != null)
				{
					this.moteStun.Maintain();
				}
				if (this.AffectedByEMP && this.stunFromEMP)
				{
					if (this.empEffecter == null)
					{
						this.empEffecter = EffecterDefOf.DisabledByEMP.Spawn();
					}
					this.empEffecter.EffectTick(this.parent, this.parent);
					return;
				}
			}
			else if (this.empEffecter != null)
			{
				this.empEffecter.Cleanup();
				this.empEffecter = null;
				this.stunFromEMP = false;
			}
		}

		// Token: 0x06008200 RID: 33280 RVA: 0x00268708 File Offset: 0x00266908
		public void Notify_DamageApplied(DamageInfo dinfo, bool affectedByEMP)
		{
			Pawn pawn = this.parent as Pawn;
			if (pawn != null && (pawn.Downed || pawn.Dead))
			{
				return;
			}
			if (dinfo.Def == DamageDefOf.Stun)
			{
				this.StunFor_NewTmp(Mathf.RoundToInt(dinfo.Amount * 30f), dinfo.Instigator, true, true);
				return;
			}
			if (dinfo.Def == DamageDefOf.EMP && this.AffectedByEMP)
			{
				if (this.EMPAdaptedTicksLeft <= 0)
				{
					this.StunFor_NewTmp(Mathf.RoundToInt(dinfo.Amount * 30f), dinfo.Instigator, true, true);
					this.EMPAdaptedTicksLeft = this.EMPAdaptationTicksDuration;
					this.stunFromEMP = true;
					return;
				}
				MoteMaker.ThrowText(new Vector3((float)this.parent.Position.x + 1f, (float)this.parent.Position.y, (float)this.parent.Position.z + 1f), this.parent.Map, "Adapted".Translate(), Color.white, -1f);
			}
		}

		// Token: 0x06008201 RID: 33281 RVA: 0x000574F6 File Offset: 0x000556F6
		[Obsolete("Only need this overload to not break mod compatibility.")]
		public void StunFor(int ticks, Thing instigator, bool addBattleLog = true)
		{
			this.StunFor_NewTmp(ticks, instigator, addBattleLog, true);
		}

		// Token: 0x06008202 RID: 33282 RVA: 0x00057502 File Offset: 0x00055702
		public void StunFor_NewTmp(int ticks, Thing instigator, bool addBattleLog = true, bool showMote = true)
		{
			this.stunTicksLeft = Mathf.Max(this.stunTicksLeft, ticks);
			this.showStunMote = showMote;
			if (addBattleLog)
			{
				Find.BattleLog.Add(new BattleLogEntry_Event(this.parent, RulePackDefOf.Event_Stun, instigator));
			}
		}

		// Token: 0x0400545A RID: 21594
		public Thing parent;

		// Token: 0x0400545B RID: 21595
		private int stunTicksLeft;

		// Token: 0x0400545C RID: 21596
		private Mote moteStun;

		// Token: 0x0400545D RID: 21597
		private bool showStunMote = true;

		// Token: 0x0400545E RID: 21598
		private int EMPAdaptedTicksLeft;

		// Token: 0x0400545F RID: 21599
		private Effecter empEffecter;

		// Token: 0x04005460 RID: 21600
		private bool stunFromEMP;

		// Token: 0x04005461 RID: 21601
		public const float StunDurationTicksPerDamage = 30f;
	}
}
