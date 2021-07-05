using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010DD RID: 4317
	public class QuestPart_Leave : QuestPart
	{
		// Token: 0x17000EA5 RID: 3749
		// (get) Token: 0x06005E36 RID: 24118 RVA: 0x0004147C File Offset: 0x0003F67C
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				foreach (Pawn t in PawnsArriveQuestPartUtility.GetQuestLookTargets(this.pawns))
				{
					yield return t;
				}
				IEnumerator<Pawn> enumerator2 = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x06005E37 RID: 24119 RVA: 0x001DE8C0 File Offset: 0x001DCAC0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
			if (signal.tag == this.inSignal)
			{
				LeaveQuestPartUtility.MakePawnsLeave(this.pawns, this.sendStandardLetter, this.quest);
			}
		}

		// Token: 0x06005E38 RID: 24120 RVA: 0x0004148C File Offset: 0x0003F68C
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.leaveOnCleanup)
			{
				LeaveQuestPartUtility.MakePawnsLeave(this.pawns, this.sendStandardLetter, this.quest);
			}
		}

		// Token: 0x06005E39 RID: 24121 RVA: 0x001DE944 File Offset: 0x001DCB44
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", true, false);
			Scribe_Values.Look<bool>(ref this.leaveOnCleanup, "leaveOnCleanup", false, false);
			Scribe_Values.Look<string>(ref this.inSignalRemovePawn, "inSignalRemovePawn", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06005E3A RID: 24122 RVA: 0x001DE9E8 File Offset: 0x001DCBE8
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			if (Find.AnyPlayerHomeMap != null)
			{
				Map randomPlayerHomeMap = Find.RandomPlayerHomeMap;
				if (randomPlayerHomeMap.mapPawns.FreeColonistsCount != 0)
				{
					this.pawns.Add(randomPlayerHomeMap.mapPawns.FreeColonists.First<Pawn>());
				}
			}
		}

		// Token: 0x06005E3B RID: 24123 RVA: 0x000414B3 File Offset: 0x0003F6B3
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x04003EF9 RID: 16121
		public string inSignal;

		// Token: 0x04003EFA RID: 16122
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04003EFB RID: 16123
		public bool sendStandardLetter = true;

		// Token: 0x04003EFC RID: 16124
		public bool leaveOnCleanup = true;

		// Token: 0x04003EFD RID: 16125
		public string inSignalRemovePawn;
	}
}
