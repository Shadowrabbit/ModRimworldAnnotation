using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200103F RID: 4159
	public class Quest : IExposable, ILoadReferenceable, ISignalReceiver
	{
		// Token: 0x17000DFF RID: 3583
		// (get) Token: 0x06005A7E RID: 23166 RVA: 0x0003EB24 File Offset: 0x0003CD24
		public List<QuestPart> PartsListForReading
		{
			get
			{
				return this.parts;
			}
		}

		// Token: 0x17000E00 RID: 3584
		// (get) Token: 0x06005A7F RID: 23167 RVA: 0x0003EB2C File Offset: 0x0003CD2C
		public int TicksSinceAppeared
		{
			get
			{
				return Find.TickManager.TicksGame - this.appearanceTick;
			}
		}

		// Token: 0x17000E01 RID: 3585
		// (get) Token: 0x06005A80 RID: 23168 RVA: 0x0003EB3F File Offset: 0x0003CD3F
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

		// Token: 0x17000E02 RID: 3586
		// (get) Token: 0x06005A81 RID: 23169 RVA: 0x0003EB5D File Offset: 0x0003CD5D
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

		// Token: 0x17000E03 RID: 3587
		// (get) Token: 0x06005A82 RID: 23170 RVA: 0x0003EB7A File Offset: 0x0003CD7A
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

		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x06005A83 RID: 23171 RVA: 0x0003EB96 File Offset: 0x0003CD96
		public string InitiateSignal
		{
			get
			{
				return "Quest" + this.id + ".Initiate";
			}
		}

		// Token: 0x17000E05 RID: 3589
		// (get) Token: 0x06005A84 RID: 23172 RVA: 0x0003EBB2 File Offset: 0x0003CDB2
		public bool EverAccepted
		{
			get
			{
				return this.initiallyAccepted || this.acceptanceTick >= 0;
			}
		}

		// Token: 0x17000E06 RID: 3590
		// (get) Token: 0x06005A85 RID: 23173 RVA: 0x0003EBCA File Offset: 0x0003CDCA
		public Pawn AccepterPawn
		{
			get
			{
				return this.accepterPawn;
			}
		}

		// Token: 0x17000E07 RID: 3591
		// (get) Token: 0x06005A86 RID: 23174 RVA: 0x001D59D4 File Offset: 0x001D3BD4
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

		// Token: 0x17000E08 RID: 3592
		// (get) Token: 0x06005A87 RID: 23175 RVA: 0x001D5A10 File Offset: 0x001D3C10
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

		// Token: 0x17000E09 RID: 3593
		// (get) Token: 0x06005A88 RID: 23176 RVA: 0x0003EBD2 File Offset: 0x0003CDD2
		public IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				return this.parts.SelectMany((QuestPart x) => x.QuestLookTargets).Distinct<GlobalTargetInfo>();
			}
		}

		// Token: 0x17000E0A RID: 3594
		// (get) Token: 0x06005A89 RID: 23177 RVA: 0x0003EC03 File Offset: 0x0003CE03
		public IEnumerable<GlobalTargetInfo> QuestSelectTargets
		{
			get
			{
				return this.parts.SelectMany((QuestPart x) => x.QuestSelectTargets).Distinct<GlobalTargetInfo>();
			}
		}

		// Token: 0x17000E0B RID: 3595
		// (get) Token: 0x06005A8A RID: 23178 RVA: 0x0003EC34 File Offset: 0x0003CE34
		public IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				return this.parts.SelectMany((QuestPart x) => x.InvolvedFactions).Distinct<Faction>();
			}
		}

		// Token: 0x17000E0C RID: 3596
		// (get) Token: 0x06005A8B RID: 23179 RVA: 0x0003EC65 File Offset: 0x0003CE65
		public IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				return this.parts.SelectMany((QuestPart x) => x.Hyperlinks).Distinct<Dialog_InfoCard.Hyperlink>();
			}
		}

		// Token: 0x17000E0D RID: 3597
		// (get) Token: 0x06005A8C RID: 23180 RVA: 0x0003EC96 File Offset: 0x0003CE96
		public bool Historical
		{
			get
			{
				return this.State != QuestState.NotYetAccepted && this.State != QuestState.Ongoing;
			}
		}

		// Token: 0x17000E0E RID: 3598
		// (get) Token: 0x06005A8D RID: 23181 RVA: 0x001D5A60 File Offset: 0x001D3C60
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

		// Token: 0x06005A8E RID: 23182 RVA: 0x0003ECAE File Offset: 0x0003CEAE
		public static Quest MakeRaw()
		{
			return new Quest
			{
				id = Find.UniqueIDsManager.GetNextQuestID(),
				appearanceTick = Find.TickManager.TicksGame,
				name = "Unnamed quest"
			};
		}

		// Token: 0x06005A8F RID: 23183 RVA: 0x001D5A9C File Offset: 0x001D3C9C
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
							Log.Error("Exception ticking QuestPart: " + arg, false);
						}
						if (this.Historical)
						{
							break;
						}
					}
				}
			}
		}

		// Token: 0x06005A90 RID: 23184 RVA: 0x001D5B80 File Offset: 0x001D3D80
		public void AddPart(QuestPart part)
		{
			if (this.parts.Contains(part))
			{
				Log.Error("Tried to add the same QuestPart twice: " + part.ToStringSafe<QuestPart>() + ", quest=" + this.ToStringSafe<Quest>(), false);
				return;
			}
			part.quest = this;
			this.parts.Add(part);
		}

		// Token: 0x06005A91 RID: 23185 RVA: 0x001D5BD0 File Offset: 0x001D3DD0
		public void RemovePart(QuestPart part)
		{
			if (!this.parts.Contains(part))
			{
				Log.Error("Tried to remove QuestPart which doesn't exist: " + part.ToStringSafe<QuestPart>() + ", quest=" + this.ToStringSafe<Quest>(), false);
				return;
			}
			part.quest = null;
			this.parts.Remove(part);
		}

		// Token: 0x06005A92 RID: 23186 RVA: 0x001D5C24 File Offset: 0x001D3E24
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

		// Token: 0x06005A93 RID: 23187 RVA: 0x001D5C88 File Offset: 0x001D3E88
		public void End(QuestEndOutcome outcome, bool sendLetter = true)
		{
			if (this.Historical)
			{
				Log.Error("Tried to resolve a historical quest. id=" + this.id, false);
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

		// Token: 0x06005A94 RID: 23188 RVA: 0x001D5DA0 File Offset: 0x001D3FA0
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

		// Token: 0x06005A95 RID: 23189 RVA: 0x001D5DE4 File Offset: 0x001D3FE4
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

		// Token: 0x06005A96 RID: 23190 RVA: 0x0003ECE0 File Offset: 0x0003CEE0
		public void SetInitiallyAccepted()
		{
			this.acceptanceTick = Find.TickManager.TicksGame;
			this.ticksUntilAcceptanceExpiry = -1;
			this.initiallyAccepted = true;
		}

		// Token: 0x06005A97 RID: 23191 RVA: 0x001D5E28 File Offset: 0x001D4028
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
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.parts.RemoveAll((QuestPart x) => x == null) != 0)
				{
					Log.Error("Some quest parts were null after loading.", false);
				}
				for (int i = 0; i < this.parts.Count; i++)
				{
					this.parts[i].quest = this;
				}
			}
		}

		// Token: 0x06005A98 RID: 23192 RVA: 0x0003ED00 File Offset: 0x0003CF00
		public void Notify_PawnDiscarded(Pawn pawn)
		{
			if (this.accepterPawn == pawn)
			{
				this.accepterPawn = null;
				this.accepterPawnLabel = pawn.LabelCap;
			}
		}

		// Token: 0x06005A99 RID: 23193 RVA: 0x001D603C File Offset: 0x001D423C
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
					Log.Error("Error while processing a quest signal: " + arg, false);
				}
			}
		}

		// Token: 0x06005A9A RID: 23194 RVA: 0x0003ED1E File Offset: 0x0003CF1E
		public void Initiate()
		{
			Find.SignalManager.SendSignal(new Signal(this.InitiateSignal));
		}

		// Token: 0x06005A9B RID: 23195 RVA: 0x001D6138 File Offset: 0x001D4338
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
					Log.Error("Error in QuestPart Notify_PreCleanup: " + arg, false);
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
					Log.Error("Error in QuestPart cleanup: " + arg2, false);
				}
			}
			this.cleanedUp = true;
			Find.FactionManager.Notify_QuestCleanedUp(this);
		}

		// Token: 0x06005A9C RID: 23196 RVA: 0x001D6208 File Offset: 0x001D4408
		public void Notify_ThingsProduced(Pawn worker, List<Thing> things)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_ThingsProduced(worker, things);
			}
		}

		// Token: 0x06005A9D RID: 23197 RVA: 0x001D6240 File Offset: 0x001D4440
		public void Notify_PlantHarvested(Pawn worker, Thing harvested)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_PlantHarvested(worker, harvested);
			}
		}

		// Token: 0x06005A9E RID: 23198 RVA: 0x001D6278 File Offset: 0x001D4478
		public void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_PawnKilled(pawn, dinfo);
			}
		}

		// Token: 0x06005A9F RID: 23199 RVA: 0x001D62B0 File Offset: 0x001D44B0
		public void Notify_FactionRemoved(Faction faction)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_FactionRemoved(faction);
			}
		}

		// Token: 0x06005AA0 RID: 23200 RVA: 0x0003ED35 File Offset: 0x0003CF35
		public string GetUniqueLoadID()
		{
			return "Quest_" + this.id;
		}

		// Token: 0x04003CDB RID: 15579
		public int id;

		// Token: 0x04003CDC RID: 15580
		private List<QuestPart> parts = new List<QuestPart>();

		// Token: 0x04003CDD RID: 15581
		public string name;

		// Token: 0x04003CDE RID: 15582
		public TaggedString description;

		// Token: 0x04003CDF RID: 15583
		public float points;

		// Token: 0x04003CE0 RID: 15584
		public int challengeRating = -1;

		// Token: 0x04003CE1 RID: 15585
		public List<string> tags = new List<string>();

		// Token: 0x04003CE2 RID: 15586
		public string lastSlateStateDebug;

		// Token: 0x04003CE3 RID: 15587
		public QuestScriptDef root;

		// Token: 0x04003CE4 RID: 15588
		public bool hidden;

		// Token: 0x04003CE5 RID: 15589
		public int appearanceTick = -1;

		// Token: 0x04003CE6 RID: 15590
		public int acceptanceTick = -1;

		// Token: 0x04003CE7 RID: 15591
		public bool initiallyAccepted;

		// Token: 0x04003CE8 RID: 15592
		public bool dismissed;

		// Token: 0x04003CE9 RID: 15593
		public bool hiddenInUI;

		// Token: 0x04003CEA RID: 15594
		public int ticksUntilAcceptanceExpiry = -1;

		// Token: 0x04003CEB RID: 15595
		private Pawn accepterPawn;

		// Token: 0x04003CEC RID: 15596
		private string accepterPawnLabel;

		// Token: 0x04003CED RID: 15597
		public List<string> signalsReceivedDebug;

		// Token: 0x04003CEE RID: 15598
		private bool ended;

		// Token: 0x04003CEF RID: 15599
		private QuestEndOutcome endOutcome;

		// Token: 0x04003CF0 RID: 15600
		private bool cleanedUp;

		// Token: 0x04003CF1 RID: 15601
		public int cleanupTick = -1;

		// Token: 0x04003CF2 RID: 15602
		public const int MaxSignalsReceivedDebugCount = 25;

		// Token: 0x04003CF3 RID: 15603
		private const int RemoveAllQuestPartsAfterTicksSinceCleanup = 1800000;
	}
}
