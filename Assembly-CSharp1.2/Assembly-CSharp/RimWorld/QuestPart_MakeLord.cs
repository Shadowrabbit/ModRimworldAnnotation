using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001090 RID: 4240
	public abstract class QuestPart_MakeLord : QuestPart
	{
		// Token: 0x17000E52 RID: 3666
		// (get) Token: 0x06005C58 RID: 23640 RVA: 0x00040185 File Offset: 0x0003E385
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

		// Token: 0x17000E53 RID: 3667
		// (get) Token: 0x06005C59 RID: 23641 RVA: 0x00040195 File Offset: 0x0003E395
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

		// Token: 0x06005C5A RID: 23642
		protected abstract Lord MakeLord();

		// Token: 0x06005C5B RID: 23643 RVA: 0x001DA300 File Offset: 0x001D8500
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

		// Token: 0x06005C5C RID: 23644 RVA: 0x000401C0 File Offset: 0x0003E3C0
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x06005C5D RID: 23645 RVA: 0x001DA424 File Offset: 0x001D8624
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

		// Token: 0x06005C5E RID: 23646 RVA: 0x001DA4EC File Offset: 0x001D86EC
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.pawns.Add(PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>());
			this.mapParent = this.pawns[0].Map.Parent;
		}

		// Token: 0x04003DDA RID: 15834
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04003DDB RID: 15835
		public string inSignal;

		// Token: 0x04003DDC RID: 15836
		public Faction faction;

		// Token: 0x04003DDD RID: 15837
		public MapParent mapParent;

		// Token: 0x04003DDE RID: 15838
		public string inSignalRemovePawn;

		// Token: 0x04003DDF RID: 15839
		public Pawn mapOfPawn;

		// Token: 0x04003DE0 RID: 15840
		public bool excludeFromLookTargets;
	}
}
