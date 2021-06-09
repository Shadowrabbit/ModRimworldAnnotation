using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001122 RID: 4386
	public abstract class QuestPart : IExposable, ILoadReferenceable
	{
		// Token: 0x17000EEF RID: 3823
		// (get) Token: 0x06005FE2 RID: 24546 RVA: 0x000424BA File Offset: 0x000406BA
		public virtual string DescriptionPart { get; }

		// Token: 0x17000EF0 RID: 3824
		// (get) Token: 0x06005FE3 RID: 24547 RVA: 0x000424C2 File Offset: 0x000406C2
		public int Index
		{
			get
			{
				return this.quest.PartsListForReading.IndexOf(this);
			}
		}

		// Token: 0x17000EF1 RID: 3825
		// (get) Token: 0x06005FE4 RID: 24548 RVA: 0x000424D5 File Offset: 0x000406D5
		public virtual IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17000EF2 RID: 3826
		// (get) Token: 0x06005FE5 RID: 24549 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual string QuestSelectTargetsLabel
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000EF3 RID: 3827
		// (get) Token: 0x06005FE6 RID: 24550 RVA: 0x000424DE File Offset: 0x000406DE
		public virtual IEnumerable<GlobalTargetInfo> QuestSelectTargets
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17000EF4 RID: 3828
		// (get) Token: 0x06005FE7 RID: 24551 RVA: 0x000424E7 File Offset: 0x000406E7
		public virtual IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17000EF5 RID: 3829
		// (get) Token: 0x06005FE8 RID: 24552 RVA: 0x000424F0 File Offset: 0x000406F0
		public virtual IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17000EF6 RID: 3830
		// (get) Token: 0x06005FE9 RID: 24553 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool IncreasesPopulation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000EF7 RID: 3831
		// (get) Token: 0x06005FEA RID: 24554 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool RequiresAccepter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000EF8 RID: 3832
		// (get) Token: 0x06005FEB RID: 24555 RVA: 0x000424F9 File Offset: 0x000406F9
		public virtual bool PreventsAutoAccept
		{
			get
			{
				return this.RequiresAccepter;
			}
		}

		// Token: 0x06005FEC RID: 24556 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool QuestPartReserves(Pawn p)
		{
			return false;
		}

		// Token: 0x06005FED RID: 24557 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool QuestPartReserves(Faction f)
		{
			return false;
		}

		// Token: 0x06005FEE RID: 24558 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Cleanup()
		{
		}

		// Token: 0x06005FEF RID: 24559 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void AssignDebugData()
		{
		}

		// Token: 0x06005FF0 RID: 24560 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PreQuestAccept()
		{
		}

		// Token: 0x06005FF1 RID: 24561 RVA: 0x00042501 File Offset: 0x00040701
		public virtual void ExposeData()
		{
			Scribe_Values.Look<QuestPart.SignalListenMode>(ref this.signalListenMode, "signalListenMode", QuestPart.SignalListenMode.OngoingOnly, false);
			Scribe_Values.Look<string>(ref this.debugLabel, "debugLabel", null, false);
		}

		// Token: 0x06005FF2 RID: 24562 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_QuestSignalReceived(Signal signal)
		{
		}

		// Token: 0x06005FF3 RID: 24563 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_ThingsProduced(Pawn worker, List<Thing> things)
		{
		}

		// Token: 0x06005FF4 RID: 24564 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_PlantHarvested(Pawn worker, Thing harvested)
		{
		}

		// Token: 0x06005FF5 RID: 24565 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
		}

		// Token: 0x06005FF6 RID: 24566 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_FactionRemoved(Faction faction)
		{
		}

		// Token: 0x06005FF7 RID: 24567 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_PreCleanup()
		{
		}

		// Token: 0x06005FF8 RID: 24568 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostQuestAdded()
		{
		}

		// Token: 0x06005FF9 RID: 24569 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void ReplacePawnReferences(Pawn replace, Pawn with)
		{
		}

		// Token: 0x06005FFA RID: 24570 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
		}

		// Token: 0x06005FFB RID: 24571 RVA: 0x001E35D4 File Offset: 0x001E17D4
		public override string ToString()
		{
			string str = base.GetType().Name + " (index=" + this.Index;
			if (!this.debugLabel.NullOrEmpty())
			{
				str = str + ", debugLabel=" + this.debugLabel;
			}
			return str + ")";
		}

		// Token: 0x06005FFC RID: 24572 RVA: 0x00042527 File Offset: 0x00040727
		public string GetUniqueLoadID()
		{
			return string.Concat(new object[]
			{
				"QuestPart_",
				this.quest.id,
				"_",
				this.Index
			});
		}

		// Token: 0x0400401A RID: 16410
		public Quest quest;

		// Token: 0x0400401B RID: 16411
		public QuestPart.SignalListenMode signalListenMode;

		// Token: 0x0400401C RID: 16412
		public string debugLabel;

		// Token: 0x02001123 RID: 4387
		public enum SignalListenMode
		{
			// Token: 0x0400401F RID: 16415
			OngoingOnly,
			// Token: 0x04004020 RID: 16416
			NotYetAcceptedOnly,
			// Token: 0x04004021 RID: 16417
			OngoingOrNotYetAccepted,
			// Token: 0x04004022 RID: 16418
			HistoricalOnly,
			// Token: 0x04004023 RID: 16419
			Always
		}
	}
}
