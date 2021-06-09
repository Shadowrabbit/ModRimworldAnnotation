using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200219F RID: 8607
	public class TimedDetectionRaids : WorldObjectComp
	{
		// Token: 0x17001B3E RID: 6974
		// (get) Token: 0x0600B7D2 RID: 47058 RVA: 0x000773AF File Offset: 0x000755AF
		public bool NextRaidCountdownActiveAndVisible
		{
			get
			{
				return this.ticksLeftToSendRaid >= 0 && this.ticksLeftTillNotifyPlayer == 0;
			}
		}

		// Token: 0x17001B3F RID: 6975
		// (get) Token: 0x0600B7D3 RID: 47059 RVA: 0x000773C5 File Offset: 0x000755C5
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

		// Token: 0x17001B40 RID: 6976
		// (get) Token: 0x0600B7D4 RID: 47060 RVA: 0x000773E0 File Offset: 0x000755E0
		private Faction RaidFaction
		{
			get
			{
				return this.parent.Faction ?? Faction.OfMechanoids;
			}
		}

		// Token: 0x0600B7D5 RID: 47061 RVA: 0x000773F6 File Offset: 0x000755F6
		public void StartDetectionCountdown(int ticks, int notifyTicks = -1)
		{
			this.ticksLeftToSendRaid = ticks;
			this.ticksLeftTillNotifyPlayer = ((notifyTicks == -1) ? Mathf.Min((int)(60000f * Rand.Range(0.8f, 1.2f)), ticks / 2) : notifyTicks);
		}

		// Token: 0x0600B7D6 RID: 47062 RVA: 0x0034F5B8 File Offset: 0x0034D7B8
		public void ResetCountdown()
		{
			this.ticksLeftTillNotifyPlayer = (this.ticksLeftToSendRaid = -1);
		}

		// Token: 0x0600B7D7 RID: 47063 RVA: 0x0007742A File Offset: 0x0007562A
		public void SetNotifiedSilently()
		{
			this.ticksLeftTillNotifyPlayer = 0;
		}

		// Token: 0x0600B7D8 RID: 47064 RVA: 0x00077433 File Offset: 0x00075633
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeftToSendRaid, "ticksLeftToForceExitAndRemoveMap", -1, false);
			Scribe_Values.Look<int>(ref this.ticksLeftTillNotifyPlayer, "ticksLeftTillNotifyPlayer", -1, false);
		}

		// Token: 0x0600B7D9 RID: 47065 RVA: 0x0034F5D8 File Offset: 0x0034D7D8
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

		// Token: 0x0600B7DA RID: 47066 RVA: 0x0034F680 File Offset: 0x0034D880
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
						this.ticksLeftToSendRaid = (int)(Rand.Range(18f, 24f) * 2500f);
						Messages.Message("MessageCaravanDetectedRaidArrived".Translate(incidentParms.faction.def.pawnsPlural, incidentParms.faction, this.ticksLeftToSendRaid.ToStringTicksToDays("F1")), MessageTypeDefOf.ThreatBig, true);
						return;
					}
				}
			}
			else
			{
				this.ResetCountdown();
			}
		}

		// Token: 0x0600B7DB RID: 47067 RVA: 0x0034F7A4 File Offset: 0x0034D9A4
		private void NotifyPlayer()
		{
			Find.LetterStack.ReceiveLetter("LetterLabelSiteCountdownStarted".Translate(), "LetterTextSiteCountdownStarted".Translate(this.ticksLeftToSendRaid.ToStringTicksToDays("F1"), this.RaidFaction.def.pawnsPlural, this.RaidFaction), LetterDefOf.ThreatBig, this.parent, null, null, null, null);
		}

		// Token: 0x0600B7DC RID: 47068 RVA: 0x0007745F File Offset: 0x0007565F
		public static string GetDetectionCountdownTimeLeftString(int ticksLeft)
		{
			if (ticksLeft < 0)
			{
				return "";
			}
			return ticksLeft.ToStringTicksToPeriod(true, false, true, true);
		}

		// Token: 0x0600B7DD RID: 47069 RVA: 0x00077475 File Offset: 0x00075675
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

		// Token: 0x0600B7DE RID: 47070 RVA: 0x00077485 File Offset: 0x00075685
		public void CopyFrom(TimedDetectionRaids other)
		{
			this.ticksLeftToSendRaid = other.ticksLeftToSendRaid;
			this.ticksLeftTillNotifyPlayer = other.ticksLeftTillNotifyPlayer;
		}

		// Token: 0x04007DAA RID: 32170
		public const float RaidThreatPointsMultiplier = 2.5f;

		// Token: 0x04007DAB RID: 32171
		private int ticksLeftToSendRaid = -1;

		// Token: 0x04007DAC RID: 32172
		private int ticksLeftTillNotifyPlayer = -1;

		// Token: 0x04007DAD RID: 32173
		private static List<Pawn> tmpPawns = new List<Pawn>();
	}
}
