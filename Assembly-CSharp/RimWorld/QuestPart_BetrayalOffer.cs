using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200106A RID: 4202
	public class QuestPart_BetrayalOffer : QuestPartActivable
	{
		// Token: 0x17000E1C RID: 3612
		// (get) Token: 0x06005B5B RID: 23387 RVA: 0x0003F536 File Offset: 0x0003D736
		public override string ExpiryInfoPart
		{
			get
			{
				return "QuestBetrayalOffer".Translate(this.PawnsAliveCount, this.extraFaction.faction.Name);
			}
		}

		// Token: 0x17000E1D RID: 3613
		// (get) Token: 0x06005B5C RID: 23388 RVA: 0x0003F567 File Offset: 0x0003D767
		public override string ExpiryInfoPartTip
		{
			get
			{
				return "QuestBetrayalOfferTip".Translate(this.asker.NameFullColored, this.extraFaction.faction.Name);
			}
		}

		// Token: 0x17000E1E RID: 3614
		// (get) Token: 0x06005B5D RID: 23389 RVA: 0x0003F59D File Offset: 0x0003D79D
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

		// Token: 0x17000E1F RID: 3615
		// (get) Token: 0x06005B5E RID: 23390 RVA: 0x001D7E94 File Offset: 0x001D6094
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

		// Token: 0x17000E20 RID: 3616
		// (get) Token: 0x06005B5F RID: 23391 RVA: 0x001D7F00 File Offset: 0x001D6100
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

		// Token: 0x06005B60 RID: 23392 RVA: 0x001D7F7C File Offset: 0x001D617C
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

		// Token: 0x06005B61 RID: 23393 RVA: 0x001D7FCC File Offset: 0x001D61CC
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

		// Token: 0x06005B62 RID: 23394 RVA: 0x0003F5AD File Offset: 0x0003D7AD
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.extraFaction.faction == f)
			{
				this.extraFaction.faction = null;
			}
		}

		// Token: 0x06005B63 RID: 23395 RVA: 0x001D8048 File Offset: 0x001D6248
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
			}
		}

		// Token: 0x04003D52 RID: 15698
		public ExtraFaction extraFaction;

		// Token: 0x04003D53 RID: 15699
		public Pawn asker;

		// Token: 0x04003D54 RID: 15700
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04003D55 RID: 15701
		public List<string> inSignals = new List<string>();

		// Token: 0x04003D56 RID: 15702
		public string outSignalEnabled;

		// Token: 0x04003D57 RID: 15703
		public string outSignalSuccess;

		// Token: 0x04003D58 RID: 15704
		public string outSignalFailure;
	}
}
