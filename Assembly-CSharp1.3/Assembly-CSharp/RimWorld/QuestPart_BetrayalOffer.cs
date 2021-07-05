using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B41 RID: 2881
	public class QuestPart_BetrayalOffer : QuestPartActivable
	{
		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x0600435B RID: 17243 RVA: 0x001674F4 File Offset: 0x001656F4
		public override string ExpiryInfoPart
		{
			get
			{
				return "QuestBetrayalOffer".Translate(this.PawnsAliveCount, this.extraFaction.faction.Name);
			}
		}

		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x0600435C RID: 17244 RVA: 0x00167525 File Offset: 0x00165725
		public override string ExpiryInfoPartTip
		{
			get
			{
				return "QuestBetrayalOfferTip".Translate(this.asker.NameFullColored, this.extraFaction.faction.Name);
			}
		}

		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x0600435D RID: 17245 RVA: 0x0016755B File Offset: 0x0016575B
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				int num;
				for (int i = 0; i < this.pawns.Count; i = num + 1)
				{
					if (this.pawns[i].GetExtraFaction(this.extraFaction.factionType, this.quest) == this.extraFaction.faction && !this.pawns[i].Destroyed)
					{
						yield return this.pawns[i];
					}
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x0600435E RID: 17246 RVA: 0x0016756C File Offset: 0x0016576C
		private int PawnsAliveCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.pawns[i].GetExtraFaction(this.extraFaction.factionType, this.quest) == this.extraFaction.faction && !this.pawns[i].Destroyed)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x0600435F RID: 17247 RVA: 0x001675D8 File Offset: 0x001657D8
		private bool AnyPawnDespawnedButAlive
		{
			get
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.pawns[i].GetExtraFaction(this.extraFaction.factionType, this.quest) == this.extraFaction.faction && !this.pawns[i].Spawned && !this.pawns[i].Dead)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06004360 RID: 17248 RVA: 0x00167654 File Offset: 0x00165854
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			if (!this.AnyPawnDespawnedButAlive && this.PawnsAliveCount > 0)
			{
				if (!this.outSignalEnabled.NullOrEmpty())
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalEnabled, receivedArgs));
					return;
				}
			}
			else
			{
				base.Complete();
			}
		}

		// Token: 0x06004361 RID: 17249 RVA: 0x001676A4 File Offset: 0x001658A4
		protected override void ProcessQuestSignal(Signal signal)
		{
			base.ProcessQuestSignal(signal);
			if (this.inSignals.Contains(signal.tag))
			{
				if (this.AnyPawnDespawnedButAlive)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalFailure, signal.args));
					base.Complete();
					return;
				}
				if (this.PawnsAliveCount == 0)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalSuccess, signal.args));
					base.Complete();
				}
			}
		}

		// Token: 0x06004362 RID: 17250 RVA: 0x0016771E File Offset: 0x0016591E
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.extraFaction.faction == f)
			{
				this.extraFaction.faction = null;
			}
		}

		// Token: 0x06004363 RID: 17251 RVA: 0x0016773A File Offset: 0x0016593A
		public override void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			base.Notify_PawnKilled(pawn, dinfo);
			if (pawn == this.asker && base.State == QuestPartState.Enabled)
			{
				this.Disable();
			}
		}

		// Token: 0x06004364 RID: 17252 RVA: 0x0016775C File Offset: 0x0016595C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Deep.Look<ExtraFaction>(ref this.extraFaction, "extraFaction", Array.Empty<object>());
			Scribe_References.Look<Pawn>(ref this.asker, "asker", false);
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignalSuccess, "outSignalSuccess", null, false);
			Scribe_Values.Look<string>(ref this.outSignalFailure, "outSignalFailure", null, false);
			Scribe_Values.Look<string>(ref this.outSignalEnabled, "outSignalEnabled", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
				if (this.asker == null && base.State == QuestPartState.Enabled)
				{
					this.Disable();
				}
			}
		}

		// Token: 0x040028EF RID: 10479
		public ExtraFaction extraFaction;

		// Token: 0x040028F0 RID: 10480
		public Pawn asker;

		// Token: 0x040028F1 RID: 10481
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x040028F2 RID: 10482
		public List<string> inSignals = new List<string>();

		// Token: 0x040028F3 RID: 10483
		public string outSignalEnabled;

		// Token: 0x040028F4 RID: 10484
		public string outSignalSuccess;

		// Token: 0x040028F5 RID: 10485
		public string outSignalFailure;
	}
}
