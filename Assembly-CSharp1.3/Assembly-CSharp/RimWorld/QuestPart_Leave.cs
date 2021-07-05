using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B81 RID: 2945
	public class QuestPart_Leave : QuestPart
	{
		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x060044DA RID: 17626 RVA: 0x0016C9AA File Offset: 0x0016ABAA
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

		// Token: 0x060044DB RID: 17627 RVA: 0x0016C9BC File Offset: 0x0016ABBC
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
				LeaveQuestPartUtility.MakePawnsLeave(this.pawns, this.sendStandardLetter, this.quest, this.wakeUp);
			}
		}

		// Token: 0x060044DC RID: 17628 RVA: 0x0016CA43 File Offset: 0x0016AC43
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.leaveOnCleanup)
			{
				LeaveQuestPartUtility.MakePawnsLeave(this.pawns, this.sendStandardLetter, this.quest, this.wakeUp);
			}
		}

		// Token: 0x060044DD RID: 17629 RVA: 0x0016CA70 File Offset: 0x0016AC70
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", true, false);
			Scribe_Values.Look<bool>(ref this.leaveOnCleanup, "leaveOnCleanup", false, false);
			Scribe_Values.Look<string>(ref this.inSignalRemovePawn, "inSignalRemovePawn", null, false);
			Scribe_Values.Look<bool>(ref this.wakeUp, "wakeUp", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x060044DE RID: 17630 RVA: 0x0016CB28 File Offset: 0x0016AD28
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

		// Token: 0x060044DF RID: 17631 RVA: 0x0016CB8A File Offset: 0x0016AD8A
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x040029C5 RID: 10693
		public string inSignal;

		// Token: 0x040029C6 RID: 10694
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x040029C7 RID: 10695
		public bool sendStandardLetter = true;

		// Token: 0x040029C8 RID: 10696
		public bool leaveOnCleanup = true;

		// Token: 0x040029C9 RID: 10697
		public string inSignalRemovePawn;

		// Token: 0x040029CA RID: 10698
		public bool wakeUp;
	}
}
