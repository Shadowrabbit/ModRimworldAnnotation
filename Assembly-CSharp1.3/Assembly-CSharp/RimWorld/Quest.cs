using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000B0F RID: 2831
	public class Quest : IExposable, ILoadReferenceable, ISignalReceiver
	{
		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x06004267 RID: 16999 RVA: 0x00163C2A File Offset: 0x00161E2A
		public List<QuestPart> PartsListForReading
		{
			get
			{
				return this.parts;
			}
		}

		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x06004268 RID: 17000 RVA: 0x00163C32 File Offset: 0x00161E32
		public int TicksSinceAppeared
		{
			get
			{
				return Find.TickManager.TicksGame - this.appearanceTick;
			}
		}

		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x06004269 RID: 17001 RVA: 0x00163C45 File Offset: 0x00161E45
		public int TicksSinceAccepted
		{
			get
			{
				if (this.acceptanceTick >= 0)
				{
					return Find.TickManager.TicksGame - this.acceptanceTick;
				}
				return -1;
			}
		}

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x0600426A RID: 17002 RVA: 0x00163C63 File Offset: 0x00161E63
		public int TicksSinceCleanup
		{
			get
			{
				if (!this.cleanedUp)
				{
					return -1;
				}
				return Find.TickManager.TicksGame - this.cleanupTick;
			}
		}

		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x0600426B RID: 17003 RVA: 0x00163C80 File Offset: 0x00161E80
		public string AccepterPawnLabelCap
		{
			get
			{
				if (this.accepterPawn == null)
				{
					return this.accepterPawnLabel;
				}
				return this.accepterPawn.LabelCap;
			}
		}

		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x0600426C RID: 17004 RVA: 0x00163C9C File Offset: 0x00161E9C
		public string AddedSignal
		{
			get
			{
				return "Quest" + this.id + ".Added";
			}
		}

		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x0600426D RID: 17005 RVA: 0x00163CB8 File Offset: 0x00161EB8
		public string InitiateSignal
		{
			get
			{
				return "Quest" + this.id + ".Initiate";
			}
		}

		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x0600426E RID: 17006 RVA: 0x00163CD4 File Offset: 0x00161ED4
		public bool EverAccepted
		{
			get
			{
				return this.initiallyAccepted || this.acceptanceTick >= 0;
			}
		}

		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x0600426F RID: 17007 RVA: 0x00163CEC File Offset: 0x00161EEC
		public Pawn AccepterPawn
		{
			get
			{
				return this.accepterPawn;
			}
		}

		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x06004270 RID: 17008 RVA: 0x00163CF4 File Offset: 0x00161EF4
		public bool RequiresAccepter
		{
			get
			{
				for (int i = 0; i < this.parts.Count; i++)
				{
					if (this.parts[i].RequiresAccepter)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x06004271 RID: 17009 RVA: 0x00163D30 File Offset: 0x00161F30
		public QuestState State
		{
			get
			{
				if (this.ticksUntilAcceptanceExpiry == 0)
				{
					return QuestState.EndedOfferExpired;
				}
				if (this.ended)
				{
					if (this.endOutcome == QuestEndOutcome.Success)
					{
						return QuestState.EndedSuccess;
					}
					if (this.endOutcome == QuestEndOutcome.Fail)
					{
						return QuestState.EndedFailed;
					}
					if (this.endOutcome == QuestEndOutcome.InvalidPreAcceptance)
					{
						return QuestState.EndedInvalid;
					}
					return QuestState.EndedUnknownOutcome;
				}
				else
				{
					if (this.acceptanceTick < 0)
					{
						return QuestState.NotYetAccepted;
					}
					return QuestState.Ongoing;
				}
			}
		}

		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x06004272 RID: 17010 RVA: 0x00163D7E File Offset: 0x00161F7E
		public IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				return this.parts.SelectMany((QuestPart x) => x.QuestLookTargets).Distinct<GlobalTargetInfo>();
			}
		}

		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x06004273 RID: 17011 RVA: 0x00163DAF File Offset: 0x00161FAF
		public IEnumerable<GlobalTargetInfo> QuestSelectTargets
		{
			get
			{
				return this.parts.SelectMany((QuestPart x) => x.QuestSelectTargets).Distinct<GlobalTargetInfo>();
			}
		}

		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x06004274 RID: 17012 RVA: 0x00163DE0 File Offset: 0x00161FE0
		public IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				return this.parts.SelectMany((QuestPart x) => x.InvolvedFactions).Distinct<Faction>();
			}
		}

		// Token: 0x17000BB2 RID: 2994
		// (get) Token: 0x06004275 RID: 17013 RVA: 0x00163E11 File Offset: 0x00162011
		public IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				if (this.parent != null)
				{
					yield return new Dialog_InfoCard.Hyperlink(this.parent, -1);
				}
				foreach (Quest quest in from q in this.GetSubquests(null)
				orderby q.Historical
				select q)
				{
					yield return new Dialog_InfoCard.Hyperlink(quest, -1);
				}
				IEnumerator<Quest> enumerator = null;
				foreach (Dialog_InfoCard.Hyperlink hyperlink in this.parts.SelectMany((QuestPart x) => x.Hyperlinks).Distinct<Dialog_InfoCard.Hyperlink>())
				{
					yield return hyperlink;
				}
				IEnumerator<Dialog_InfoCard.Hyperlink> enumerator2 = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000BB3 RID: 2995
		// (get) Token: 0x06004276 RID: 17014 RVA: 0x00163E21 File Offset: 0x00162021
		public bool Historical
		{
			get
			{
				return this.State != QuestState.NotYetAccepted && this.State != QuestState.Ongoing;
			}
		}

		// Token: 0x17000BB4 RID: 2996
		// (get) Token: 0x06004277 RID: 17015 RVA: 0x00163E3C File Offset: 0x0016203C
		public bool IncreasesPopulation
		{
			get
			{
				for (int i = 0; i < this.parts.Count; i++)
				{
					if (this.parts[i].IncreasesPopulation)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06004278 RID: 17016 RVA: 0x00163E75 File Offset: 0x00162075
		public static Quest MakeRaw()
		{
			return new Quest
			{
				id = Find.UniqueIDsManager.GetNextQuestID(),
				appearanceTick = Find.TickManager.TicksGame,
				name = "Unnamed quest"
			};
		}

		// Token: 0x06004279 RID: 17017 RVA: 0x00163EA8 File Offset: 0x001620A8
		public void QuestTick()
		{
			if (this.Historical)
			{
				if (!this.cleanedUp)
				{
					this.CleanupQuestParts();
				}
				if (this.TicksSinceCleanup >= 1800000)
				{
					this.parts.Clear();
				}
				return;
			}
			if (this.ticksUntilAcceptanceExpiry > 0 && this.State == QuestState.NotYetAccepted)
			{
				this.ticksUntilAcceptanceExpiry--;
				if (this.ticksUntilAcceptanceExpiry == 0 && !this.cleanedUp)
				{
					this.CleanupQuestParts();
				}
			}
			if (!this.Historical)
			{
				for (int i = 0; i < this.parts.Count; i++)
				{
					QuestPartActivable questPartActivable = this.parts[i] as QuestPartActivable;
					if (questPartActivable != null && questPartActivable.State == QuestPartState.Enabled)
					{
						try
						{
							questPartActivable.QuestPartTick();
						}
						catch (Exception arg)
						{
							Log.Error("Exception ticking QuestPart: " + arg);
						}
						if (this.Historical)
						{
							break;
						}
					}
				}
			}
		}

		// Token: 0x0600427A RID: 17018 RVA: 0x00163F8C File Offset: 0x0016218C
		public void AddPart(QuestPart part)
		{
			if (this.parts.Contains(part))
			{
				Log.Error("Tried to add the same QuestPart twice: " + part.ToStringSafe<QuestPart>() + ", quest=" + this.ToStringSafe<Quest>());
				return;
			}
			part.quest = this;
			this.parts.Add(part);
		}

		// Token: 0x0600427B RID: 17019 RVA: 0x00163FDC File Offset: 0x001621DC
		public void RemovePart(QuestPart part)
		{
			if (!this.parts.Contains(part))
			{
				Log.Error("Tried to remove QuestPart which doesn't exist: " + part.ToStringSafe<QuestPart>() + ", quest=" + this.ToStringSafe<Quest>());
				return;
			}
			part.quest = null;
			this.parts.Remove(part);
		}

		// Token: 0x0600427C RID: 17020 RVA: 0x0016402C File Offset: 0x0016222C
		public void Accept(Pawn by)
		{
			if (this.State != QuestState.NotYetAccepted)
			{
				return;
			}
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].PreQuestAccept();
			}
			this.acceptanceTick = Find.TickManager.TicksGame;
			this.accepterPawn = by;
			this.dismissed = false;
			this.Initiate();
		}

		// Token: 0x0600427D RID: 17021 RVA: 0x00164090 File Offset: 0x00162290
		public void End(QuestEndOutcome outcome, bool sendLetter = true)
		{
			if (this.Historical)
			{
				Log.Error("Tried to resolve a historical quest. id=" + this.id);
				return;
			}
			this.ended = true;
			this.endOutcome = outcome;
			this.CleanupQuestParts();
			if (!this.EverAccepted && this.State == QuestState.EndedOfferExpired)
			{
				return;
			}
			if (sendLetter && !this.hidden)
			{
				string key = null;
				string key2 = null;
				LetterDef textLetterDef = null;
				switch (this.State)
				{
				case QuestState.EndedUnknownOutcome:
					key2 = "LetterQuestConcludedLabel";
					key = "LetterQuestCompletedConcluded";
					textLetterDef = LetterDefOf.NeutralEvent;
					SoundDefOf.Quest_Concluded.PlayOneShotOnCamera(null);
					break;
				case QuestState.EndedSuccess:
					key2 = "LetterQuestCompletedLabel";
					key = "LetterQuestCompletedSuccess";
					textLetterDef = LetterDefOf.PositiveEvent;
					SoundDefOf.Quest_Succeded.PlayOneShotOnCamera(null);
					break;
				case QuestState.EndedFailed:
					key2 = "LetterQuestFailedLabel";
					key = "LetterQuestCompletedFail";
					textLetterDef = LetterDefOf.NegativeEvent;
					SoundDefOf.Quest_Failed.PlayOneShotOnCamera(null);
					break;
				}
				Find.LetterStack.ReceiveLetter(key2.Translate(), key.Translate(this.name.CapitalizeFirst()), textLetterDef, null, null, this, null, null);
			}
		}

		// Token: 0x0600427E RID: 17022 RVA: 0x001641A8 File Offset: 0x001623A8
		public bool QuestReserves(Pawn p)
		{
			if (this.Historical)
			{
				return false;
			}
			for (int i = 0; i < this.parts.Count; i++)
			{
				if (this.parts[i].QuestPartReserves(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600427F RID: 17023 RVA: 0x001641EC File Offset: 0x001623EC
		public bool QuestReserves(Faction f)
		{
			if (this.Historical)
			{
				return false;
			}
			for (int i = 0; i < this.parts.Count; i++)
			{
				if (this.parts[i].QuestPartReserves(f))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004280 RID: 17024 RVA: 0x00164230 File Offset: 0x00162430
		public void SetInitiallyAccepted()
		{
			this.acceptanceTick = Find.TickManager.TicksGame;
			this.ticksUntilAcceptanceExpiry = -1;
			this.initiallyAccepted = true;
		}

		// Token: 0x06004281 RID: 17025 RVA: 0x00164250 File Offset: 0x00162450
		public void SetNotYetAccepted()
		{
			this.acceptanceTick = -1;
		}

		// Token: 0x06004282 RID: 17026 RVA: 0x0016425C File Offset: 0x0016245C
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.id, "id", 0, false);
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<int>(ref this.appearanceTick, "appearanceTick", -1, false);
			Scribe_Values.Look<int>(ref this.acceptanceTick, "acceptanceTick", -1, false);
			Scribe_Values.Look<int>(ref this.ticksUntilAcceptanceExpiry, "ticksUntilAcceptanceExpiry", -1, false);
			Scribe_References.Look<Pawn>(ref this.accepterPawn, "acceptedBy", false);
			Scribe_Values.Look<string>(ref this.accepterPawnLabel, "acceptedByLabel", null, false);
			Scribe_Values.Look<bool>(ref this.ended, "ended", false, false);
			Scribe_Values.Look<QuestEndOutcome>(ref this.endOutcome, "endOutcome", QuestEndOutcome.Unknown, false);
			Scribe_Values.Look<bool>(ref this.cleanedUp, "cleanedUp", false, false);
			Scribe_Values.Look<int>(ref this.cleanupTick, "cleanupTick", -1, false);
			Scribe_Values.Look<bool>(ref this.initiallyAccepted, "initiallyAccepted", false, false);
			Scribe_Values.Look<bool>(ref this.dismissed, "dismissed", false, false);
			Scribe_Values.Look<bool>(ref this.hiddenInUI, "hiddenInUI", false, false);
			Scribe_Values.Look<int>(ref this.challengeRating, "challengeRating", 0, false);
			Scribe_Values.Look<TaggedString>(ref this.description, "description", default(TaggedString), false);
			Scribe_Values.Look<string>(ref this.lastSlateStateDebug, "lastSlateStateDebug", null, false);
			Scribe_Defs.Look<QuestScriptDef>(ref this.root, "root");
			Scribe_Values.Look<bool>(ref this.hidden, "hidden", false, false);
			Scribe_Collections.Look<string>(ref this.signalsReceivedDebug, "signalsReceivedDebug", LookMode.Undefined, Array.Empty<object>());
			Scribe_Collections.Look<QuestPart>(ref this.parts, "parts", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.tags, "tags", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.charity, "charity", false, false);
			Scribe_References.Look<Quest>(ref this.parent, "parent", false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.parts.RemoveAll((QuestPart x) => x == null) != 0)
				{
					Log.Error("Some quest parts were null after loading.");
				}
				for (int i = 0; i < this.parts.Count; i++)
				{
					this.parts[i].quest = this;
				}
			}
		}

		// Token: 0x06004283 RID: 17027 RVA: 0x00164490 File Offset: 0x00162690
		public void Notify_PawnDiscarded(Pawn pawn)
		{
			if (this.accepterPawn == pawn)
			{
				this.accepterPawn = null;
				this.accepterPawnLabel = pawn.LabelCap;
			}
			foreach (QuestPart questPart in this.PartsListForReading)
			{
				questPart.Notify_PawnDiscarded(pawn);
			}
		}

		// Token: 0x06004284 RID: 17028 RVA: 0x00164500 File Offset: 0x00162700
		public void Notify_SignalReceived(Signal signal)
		{
			if (!signal.tag.StartsWith("Quest" + this.id + "."))
			{
				return;
			}
			for (int i = 0; i < this.parts.Count; i++)
			{
				try
				{
					bool flag;
					switch (this.parts[i].signalListenMode)
					{
					case QuestPart.SignalListenMode.OngoingOnly:
						flag = (this.State == QuestState.Ongoing);
						break;
					case QuestPart.SignalListenMode.NotYetAcceptedOnly:
						flag = (this.State == QuestState.NotYetAccepted);
						break;
					case QuestPart.SignalListenMode.OngoingOrNotYetAccepted:
						flag = (this.State == QuestState.Ongoing || this.State == QuestState.NotYetAccepted);
						break;
					case QuestPart.SignalListenMode.HistoricalOnly:
						flag = this.Historical;
						break;
					case QuestPart.SignalListenMode.Always:
						flag = true;
						break;
					default:
						flag = false;
						break;
					}
					if (flag)
					{
						this.parts[i].Notify_QuestSignalReceived(signal);
					}
				}
				catch (Exception arg)
				{
					Log.Error("Error while processing a quest signal: " + arg);
				}
			}
		}

		// Token: 0x06004285 RID: 17029 RVA: 0x001645FC File Offset: 0x001627FC
		public void PostAdded()
		{
			Find.SignalManager.SendSignal(new Signal(this.AddedSignal));
		}

		// Token: 0x06004286 RID: 17030 RVA: 0x00164613 File Offset: 0x00162813
		public void Initiate()
		{
			Find.SignalManager.SendSignal(new Signal(this.InitiateSignal));
		}

		// Token: 0x06004287 RID: 17031 RVA: 0x0016462C File Offset: 0x0016282C
		public void CleanupQuestParts()
		{
			if (this.cleanedUp)
			{
				return;
			}
			this.cleanupTick = Find.TickManager.TicksGame;
			for (int i = 0; i < this.parts.Count; i++)
			{
				try
				{
					this.parts[i].Notify_PreCleanup();
				}
				catch (Exception arg)
				{
					Log.Error("Error in QuestPart Notify_PreCleanup: " + arg);
				}
			}
			for (int j = 0; j < this.parts.Count; j++)
			{
				try
				{
					this.parts[j].Cleanup();
				}
				catch (Exception arg2)
				{
					Log.Error("Error in QuestPart cleanup: " + arg2);
				}
			}
			this.cleanedUp = true;
			Find.FactionManager.Notify_QuestCleanedUp(this);
			IdeoUtility.Notify_QuestCleanedUp(this, this.State);
			if (this.root.hideOnCleanup)
			{
				this.hiddenInUI = true;
			}
		}

		// Token: 0x06004288 RID: 17032 RVA: 0x0016471C File Offset: 0x0016291C
		public void Notify_ThingsProduced(Pawn worker, List<Thing> things)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_ThingsProduced(worker, things);
			}
		}

		// Token: 0x06004289 RID: 17033 RVA: 0x00164754 File Offset: 0x00162954
		public void Notify_PlantHarvested(Pawn worker, Thing harvested)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_PlantHarvested(worker, harvested);
			}
		}

		// Token: 0x0600428A RID: 17034 RVA: 0x0016478C File Offset: 0x0016298C
		public void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_PawnKilled(pawn, dinfo);
			}
		}

		// Token: 0x0600428B RID: 17035 RVA: 0x001647C4 File Offset: 0x001629C4
		public void Notify_FactionRemoved(Faction faction)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_FactionRemoved(faction);
			}
		}

		// Token: 0x0600428C RID: 17036 RVA: 0x001647F9 File Offset: 0x001629F9
		public string GetUniqueLoadID()
		{
			return "Quest_" + this.id;
		}

		// Token: 0x04002876 RID: 10358
		public int id;

		// Token: 0x04002877 RID: 10359
		private List<QuestPart> parts = new List<QuestPart>();

		// Token: 0x04002878 RID: 10360
		public string name;

		// Token: 0x04002879 RID: 10361
		public TaggedString description;

		// Token: 0x0400287A RID: 10362
		public float points;

		// Token: 0x0400287B RID: 10363
		public int challengeRating = -1;

		// Token: 0x0400287C RID: 10364
		public List<string> tags = new List<string>();

		// Token: 0x0400287D RID: 10365
		public string lastSlateStateDebug;

		// Token: 0x0400287E RID: 10366
		public QuestScriptDef root;

		// Token: 0x0400287F RID: 10367
		public bool hidden;

		// Token: 0x04002880 RID: 10368
		public Quest parent;

		// Token: 0x04002881 RID: 10369
		public int appearanceTick = -1;

		// Token: 0x04002882 RID: 10370
		public int acceptanceTick = -1;

		// Token: 0x04002883 RID: 10371
		public bool initiallyAccepted;

		// Token: 0x04002884 RID: 10372
		public bool dismissed;

		// Token: 0x04002885 RID: 10373
		public bool hiddenInUI;

		// Token: 0x04002886 RID: 10374
		public int ticksUntilAcceptanceExpiry = -1;

		// Token: 0x04002887 RID: 10375
		private Pawn accepterPawn;

		// Token: 0x04002888 RID: 10376
		private string accepterPawnLabel;

		// Token: 0x04002889 RID: 10377
		public bool charity;

		// Token: 0x0400288A RID: 10378
		public List<string> signalsReceivedDebug;

		// Token: 0x0400288B RID: 10379
		private bool ended;

		// Token: 0x0400288C RID: 10380
		private QuestEndOutcome endOutcome;

		// Token: 0x0400288D RID: 10381
		private bool cleanedUp;

		// Token: 0x0400288E RID: 10382
		public int cleanupTick = -1;

		// Token: 0x0400288F RID: 10383
		public const int MaxSignalsReceivedDebugCount = 25;

		// Token: 0x04002890 RID: 10384
		private const int RemoveAllQuestPartsAfterTicksSinceCleanup = 1800000;
	}
}
