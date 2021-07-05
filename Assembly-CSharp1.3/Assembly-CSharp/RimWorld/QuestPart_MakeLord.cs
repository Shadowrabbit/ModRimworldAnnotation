using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B5A RID: 2906
	public abstract class QuestPart_MakeLord : QuestPart
	{
		// Token: 0x17000BE9 RID: 3049
		// (get) Token: 0x060043F1 RID: 17393 RVA: 0x00169594 File Offset: 0x00167794
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.excludeFromLookTargets)
				{
					yield break;
				}
				int num;
				for (int i = 0; i < this.pawns.Count; i = num + 1)
				{
					yield return this.pawns[i];
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000BEA RID: 3050
		// (get) Token: 0x060043F2 RID: 17394 RVA: 0x001695A4 File Offset: 0x001677A4
		public Map Map
		{
			get
			{
				if (this.mapOfPawn != null)
				{
					return this.mapOfPawn.Map;
				}
				if (this.mapParent != null)
				{
					return this.mapParent.Map;
				}
				return null;
			}
		}

		// Token: 0x060043F3 RID: 17395
		protected abstract Lord MakeLord();

		// Token: 0x060043F4 RID: 17396 RVA: 0x001695D0 File Offset: 0x001677D0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.Map != null)
			{
				this.pawns.RemoveAll((Pawn x) => x.MapHeld != this.Map);
				for (int i = 0; i < this.pawns.Count; i++)
				{
					Lord lord = this.pawns[i].GetLord();
					if (lord != null)
					{
						lord.Notify_PawnLost(this.pawns[i], PawnLostCondition.ForcedByQuest, null);
					}
				}
				Lord lord2 = this.MakeLord();
				for (int j = 0; j < this.pawns.Count; j++)
				{
					if (!this.pawns[j].Dead)
					{
						lord2.AddPawn(this.pawns[j]);
					}
				}
			}
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
		}

		// Token: 0x060043F5 RID: 17397 RVA: 0x001696F2 File Offset: 0x001678F2
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x060043F6 RID: 17398 RVA: 0x00169704 File Offset: 0x00167904
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.inSignalRemovePawn, "inSignalRemovePawn", null, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_References.Look<Pawn>(ref this.mapOfPawn, "mapOfPawn", false);
			Scribe_Values.Look<bool>(ref this.excludeFromLookTargets, "excludeFromLookTargets", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x060043F7 RID: 17399 RVA: 0x001697CC File Offset: 0x001679CC
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.pawns.Add(PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>());
			this.mapParent = this.pawns[0].Map.Parent;
		}

		// Token: 0x0400293B RID: 10555
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x0400293C RID: 10556
		public string inSignal;

		// Token: 0x0400293D RID: 10557
		public Faction faction;

		// Token: 0x0400293E RID: 10558
		public MapParent mapParent;

		// Token: 0x0400293F RID: 10559
		public string inSignalRemovePawn;

		// Token: 0x04002940 RID: 10560
		public Pawn mapOfPawn;

		// Token: 0x04002941 RID: 10561
		public bool excludeFromLookTargets;
	}
}
