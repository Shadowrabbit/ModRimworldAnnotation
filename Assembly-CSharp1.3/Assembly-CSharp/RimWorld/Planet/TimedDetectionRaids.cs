using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017F8 RID: 6136
	public class TimedDetectionRaids : WorldObjectComp
	{
		// Token: 0x1700176E RID: 5998
		// (get) Token: 0x06008F23 RID: 36643 RVA: 0x00334C92 File Offset: 0x00332E92
		public bool NextRaidCountdownActiveAndVisible
		{
			get
			{
				return this.ticksLeftToSendRaid >= 0 && this.ticksLeftTillNotifyPlayer == 0;
			}
		}

		// Token: 0x1700176F RID: 5999
		// (get) Token: 0x06008F24 RID: 36644 RVA: 0x00334CA8 File Offset: 0x00332EA8
		public string DetectionCountdownTimeLeftString
		{
			get
			{
				if (!this.NextRaidCountdownActiveAndVisible)
				{
					return "";
				}
				return TimedDetectionRaids.GetDetectionCountdownTimeLeftString(this.ticksLeftToSendRaid);
			}
		}

		// Token: 0x17001770 RID: 6000
		// (get) Token: 0x06008F25 RID: 36645 RVA: 0x00334CC3 File Offset: 0x00332EC3
		private Faction RaidFaction
		{
			get
			{
				return this.parent.Faction ?? Faction.OfMechanoids;
			}
		}

		// Token: 0x17001771 RID: 6001
		// (get) Token: 0x06008F26 RID: 36646 RVA: 0x00334CD9 File Offset: 0x00332ED9
		public int TicksLeftToSendRaids
		{
			get
			{
				return this.ticksLeftToSendRaid;
			}
		}

		// Token: 0x17001772 RID: 6002
		// (get) Token: 0x06008F27 RID: 36647 RVA: 0x00334CE1 File Offset: 0x00332EE1
		public int RaidsSentCount
		{
			get
			{
				return this.raidsSentCount;
			}
		}

		// Token: 0x17001773 RID: 6003
		// (get) Token: 0x06008F28 RID: 36648 RVA: 0x00334CE9 File Offset: 0x00332EE9
		public bool DetectionCountdownStarted
		{
			get
			{
				return this.ticksLeftToSendRaid >= 0;
			}
		}

		// Token: 0x06008F29 RID: 36649 RVA: 0x00334CF7 File Offset: 0x00332EF7
		public void StartDetectionCountdown(int ticks, int notifyTicks = -1)
		{
			this.ticksLeftToSendRaid = ticks;
			this.ticksLeftTillNotifyPlayer = ((notifyTicks == -1) ? Mathf.Min((int)(60000f * Rand.Range(0.8f, 1.2f)), ticks / 2) : notifyTicks);
		}

		// Token: 0x06008F2A RID: 36650 RVA: 0x00334D2C File Offset: 0x00332F2C
		public void ResetCountdown()
		{
			this.ticksLeftTillNotifyPlayer = (this.ticksLeftToSendRaid = -1);
		}

		// Token: 0x06008F2B RID: 36651 RVA: 0x00334D49 File Offset: 0x00332F49
		public void SetNotifiedSilently()
		{
			this.ticksLeftTillNotifyPlayer = 0;
		}

		// Token: 0x06008F2C RID: 36652 RVA: 0x00334D54 File Offset: 0x00332F54
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeftToSendRaid, "ticksLeftToForceExitAndRemoveMap", -1, false);
			Scribe_Values.Look<int>(ref this.ticksLeftTillNotifyPlayer, "ticksLeftTillNotifyPlayer", -1, false);
			Scribe_Values.Look<bool>(ref this.alertRaidsArrivingIn, "alertRaidsArrivingIn", false, false);
			Scribe_Values.Look<int>(ref this.raidsSentCount, "raidsSentCount", 0, false);
			Scribe_Values.Look<FloatRange>(ref this.delayRangeHours, "delayRangeHours", TimedDetectionRaids.DefaultDelayRangeHours, false);
		}

		// Token: 0x06008F2D RID: 36653 RVA: 0x00334DC8 File Offset: 0x00332FC8
		public override string CompInspectStringExtra()
		{
			string text = null;
			if (this.NextRaidCountdownActiveAndVisible)
			{
				text += "CaravanDetectedRaidCountdown".Translate(this.DetectionCountdownTimeLeftString) + ".\n";
			}
			if (Prefs.DevMode)
			{
				if (this.ticksLeftToSendRaid != -1)
				{
					text = text + "[DEV]: Time left to send raid: " + this.ticksLeftToSendRaid.ToStringTicksToPeriod(true, false, true, true) + "\n";
				}
				if (this.ticksLeftTillNotifyPlayer != -1)
				{
					text = text + "[DEV]: Time left till notify player about incoming raid: " + this.ticksLeftTillNotifyPlayer.ToStringTicksToPeriod(true, false, true, true) + "\n";
				}
			}
			if (text != null)
			{
				text = text.TrimEndNewlines();
			}
			return text;
		}

		// Token: 0x06008F2E RID: 36654 RVA: 0x00334E70 File Offset: 0x00333070
		public override void CompTick()
		{
			MapParent mapParent = (MapParent)this.parent;
			if (mapParent.HasMap)
			{
				if (this.ticksLeftTillNotifyPlayer > 0)
				{
					int num = this.ticksLeftTillNotifyPlayer - 1;
					this.ticksLeftTillNotifyPlayer = num;
					if (num == 0)
					{
						this.NotifyPlayer();
					}
				}
				if (this.ticksLeftToSendRaid > 0)
				{
					this.ticksLeftToSendRaid--;
					if (this.ticksLeftToSendRaid == 0)
					{
						IncidentParms incidentParms = new IncidentParms();
						incidentParms.target = mapParent.Map;
						incidentParms.points = StorytellerUtility.DefaultThreatPointsNow(incidentParms.target) * 2.5f;
						incidentParms.faction = this.RaidFaction;
						IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms);
						this.ticksLeftToSendRaid = (int)(this.delayRangeHours.RandomInRange * 2500f);
						Messages.Message("MessageCaravanDetectedRaidArrived".Translate(incidentParms.faction.def.pawnsPlural, incidentParms.faction, this.ticksLeftToSendRaid.ToStringTicksToDays("F1")), MessageTypeDefOf.ThreatBig, true);
						this.raidsSentCount++;
						return;
					}
				}
			}
			else
			{
				this.ResetCountdown();
			}
		}

		// Token: 0x06008F2F RID: 36655 RVA: 0x00334FA0 File Offset: 0x003331A0
		private void NotifyPlayer()
		{
			Find.LetterStack.ReceiveLetter("LetterLabelSiteCountdownStarted".Translate(), "LetterTextSiteCountdownStarted".Translate(this.ticksLeftToSendRaid.ToStringTicksToDays("F1"), this.RaidFaction.def.pawnsPlural, this.RaidFaction), LetterDefOf.ThreatBig, this.parent, null, null, null, null);
		}

		// Token: 0x06008F30 RID: 36656 RVA: 0x00335014 File Offset: 0x00333214
		public static string GetDetectionCountdownTimeLeftString(int ticksLeft)
		{
			if (ticksLeft < 0)
			{
				return "";
			}
			return ticksLeft.ToStringTicksToPeriod(true, false, true, true);
		}

		// Token: 0x06008F31 RID: 36657 RVA: 0x0033502A File Offset: 0x0033322A
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "Dev: Set raid timer to 1 hour",
					action = delegate()
					{
						this.ticksLeftToSendRaid = 2500;
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Dev: Set notify raid timer to 1 hour",
					action = delegate()
					{
						this.ticksLeftTillNotifyPlayer = 2500;
					}
				};
			}
			yield break;
		}

		// Token: 0x06008F32 RID: 36658 RVA: 0x0033503A File Offset: 0x0033323A
		public void CopyFrom(TimedDetectionRaids other)
		{
			this.ticksLeftToSendRaid = other.ticksLeftToSendRaid;
			this.ticksLeftTillNotifyPlayer = other.ticksLeftTillNotifyPlayer;
			this.delayRangeHours = other.delayRangeHours;
		}

		// Token: 0x04005A0E RID: 23054
		public bool alertRaidsArrivingIn;

		// Token: 0x04005A0F RID: 23055
		public const float RaidThreatPointsMultiplier = 2.5f;

		// Token: 0x04005A10 RID: 23056
		private int ticksLeftToSendRaid = -1;

		// Token: 0x04005A11 RID: 23057
		private int ticksLeftTillNotifyPlayer = -1;

		// Token: 0x04005A12 RID: 23058
		private int raidsSentCount;

		// Token: 0x04005A13 RID: 23059
		public FloatRange delayRangeHours = TimedDetectionRaids.DefaultDelayRangeHours;

		// Token: 0x04005A14 RID: 23060
		private static readonly FloatRange DefaultDelayRangeHours = new FloatRange(18f, 24f);

		// Token: 0x04005A15 RID: 23061
		private static List<Pawn> tmpPawns = new List<Pawn>();
	}
}
