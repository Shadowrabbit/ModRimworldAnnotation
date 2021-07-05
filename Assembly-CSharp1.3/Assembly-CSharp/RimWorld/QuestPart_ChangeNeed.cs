using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B68 RID: 2920
	public class QuestPart_ChangeNeed : QuestPart
	{
		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x06004449 RID: 17481 RVA: 0x0016A911 File Offset: 0x00168B11
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.pawn != null)
				{
					yield return this.pawn;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600444A RID: 17482 RVA: 0x0016A924 File Offset: 0x00168B24
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.pawn != null && this.pawn.needs != null)
			{
				Need need = this.pawn.needs.TryGetNeed(this.need);
				if (need != null)
				{
					need.CurLevel += this.offset;
				}
			}
		}

		// Token: 0x0600444B RID: 17483 RVA: 0x0016A990 File Offset: 0x00168B90
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Defs.Look<NeedDef>(ref this.need, "need");
			Scribe_Values.Look<float>(ref this.offset, "offset", 0f, false);
		}

		// Token: 0x0600444C RID: 17484 RVA: 0x0016A9EC File Offset: 0x00168BEC
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.need = NeedDefOf.Food;
			this.offset = 0.5f;
			if (Find.AnyPlayerHomeMap != null)
			{
				Find.RandomPlayerHomeMap.mapPawns.FreeColonists.FirstOrDefault<Pawn>();
			}
		}

		// Token: 0x0600444D RID: 17485 RVA: 0x0016AA26 File Offset: 0x00168C26
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawn == replace)
			{
				this.pawn = with;
			}
		}

		// Token: 0x04002971 RID: 10609
		public string inSignal;

		// Token: 0x04002972 RID: 10610
		public Pawn pawn;

		// Token: 0x04002973 RID: 10611
		public NeedDef need;

		// Token: 0x04002974 RID: 10612
		public float offset;
	}
}
