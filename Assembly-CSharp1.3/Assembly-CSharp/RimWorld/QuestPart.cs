using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BB8 RID: 3000
	public abstract class QuestPart : IExposable, ILoadReferenceable
	{
		// Token: 0x17000C38 RID: 3128
		// (get) Token: 0x060045F8 RID: 17912 RVA: 0x00172691 File Offset: 0x00170891
		public virtual string DescriptionPart { get; }

		// Token: 0x17000C39 RID: 3129
		// (get) Token: 0x060045F9 RID: 17913 RVA: 0x00172699 File Offset: 0x00170899
		public int Index
		{
			get
			{
				return this.quest.PartsListForReading.IndexOf(this);
			}
		}

		// Token: 0x17000C3A RID: 3130
		// (get) Token: 0x060045FA RID: 17914 RVA: 0x001726AC File Offset: 0x001708AC
		public virtual IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17000C3B RID: 3131
		// (get) Token: 0x060045FB RID: 17915 RVA: 0x00002688 File Offset: 0x00000888
		public virtual string QuestSelectTargetsLabel
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000C3C RID: 3132
		// (get) Token: 0x060045FC RID: 17916 RVA: 0x001726B5 File Offset: 0x001708B5
		public virtual IEnumerable<GlobalTargetInfo> QuestSelectTargets
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17000C3D RID: 3133
		// (get) Token: 0x060045FD RID: 17917 RVA: 0x001726BE File Offset: 0x001708BE
		public virtual IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17000C3E RID: 3134
		// (get) Token: 0x060045FE RID: 17918 RVA: 0x001726C7 File Offset: 0x001708C7
		public virtual IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17000C3F RID: 3135
		// (get) Token: 0x060045FF RID: 17919 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool IncreasesPopulation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C40 RID: 3136
		// (get) Token: 0x06004600 RID: 17920 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool RequiresAccepter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x06004601 RID: 17921 RVA: 0x001726D0 File Offset: 0x001708D0
		public virtual bool PreventsAutoAccept
		{
			get
			{
				return this.RequiresAccepter;
			}
		}

		// Token: 0x06004602 RID: 17922 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool QuestPartReserves(Pawn p)
		{
			return false;
		}

		// Token: 0x06004603 RID: 17923 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool QuestPartReserves(Faction f)
		{
			return false;
		}

		// Token: 0x06004604 RID: 17924 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Cleanup()
		{
		}

		// Token: 0x06004605 RID: 17925 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void AssignDebugData()
		{
		}

		// Token: 0x06004606 RID: 17926 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PreQuestAccept()
		{
		}

		// Token: 0x06004607 RID: 17927 RVA: 0x001726D8 File Offset: 0x001708D8
		public virtual void ExposeData()
		{
			Scribe_Values.Look<QuestPart.SignalListenMode>(ref this.signalListenMode, "signalListenMode", QuestPart.SignalListenMode.OngoingOnly, false);
			Scribe_Values.Look<string>(ref this.debugLabel, "debugLabel", null, false);
		}

		// Token: 0x06004608 RID: 17928 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_QuestSignalReceived(Signal signal)
		{
		}

		// Token: 0x06004609 RID: 17929 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_ThingsProduced(Pawn worker, List<Thing> things)
		{
		}

		// Token: 0x0600460A RID: 17930 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_PlantHarvested(Pawn worker, Thing harvested)
		{
		}

		// Token: 0x0600460B RID: 17931 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
		}

		// Token: 0x0600460C RID: 17932 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_FactionRemoved(Faction faction)
		{
		}

		// Token: 0x0600460D RID: 17933 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_PawnDiscarded(Pawn pawn)
		{
		}

		// Token: 0x0600460E RID: 17934 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_PreCleanup()
		{
		}

		// Token: 0x0600460F RID: 17935 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostQuestAdded()
		{
		}

		// Token: 0x06004610 RID: 17936 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ReplacePawnReferences(Pawn replace, Pawn with)
		{
		}

		// Token: 0x06004611 RID: 17937 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
		}

		// Token: 0x06004612 RID: 17938 RVA: 0x00172700 File Offset: 0x00170900
		public override string ToString()
		{
			string str = base.GetType().Name + " (index=" + this.Index;
			if (!this.debugLabel.NullOrEmpty())
			{
				str = str + ", debugLabel=" + this.debugLabel;
			}
			return str + ")";
		}

		// Token: 0x06004613 RID: 17939 RVA: 0x0017275A File Offset: 0x0017095A
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

		// Token: 0x04002AA3 RID: 10915
		public Quest quest;

		// Token: 0x04002AA4 RID: 10916
		public QuestPart.SignalListenMode signalListenMode;

		// Token: 0x04002AA5 RID: 10917
		public string debugLabel;

		// Token: 0x020020D4 RID: 8404
		public enum SignalListenMode
		{
			// Token: 0x04007DC7 RID: 32199
			OngoingOnly,
			// Token: 0x04007DC8 RID: 32200
			NotYetAcceptedOnly,
			// Token: 0x04007DC9 RID: 32201
			OngoingOrNotYetAccepted,
			// Token: 0x04007DCA RID: 32202
			HistoricalOnly,
			// Token: 0x04007DCB RID: 32203
			Always
		}
	}
}
