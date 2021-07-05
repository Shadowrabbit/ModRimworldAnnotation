using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010AA RID: 4266
	public class QuestPart_ChangeNeed : QuestPart
	{
		// Token: 0x17000E70 RID: 3696
		// (get) Token: 0x06005D02 RID: 23810 RVA: 0x00040826 File Offset: 0x0003EA26
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

		// Token: 0x06005D03 RID: 23811 RVA: 0x001DBC70 File Offset: 0x001D9E70
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

		// Token: 0x06005D04 RID: 23812 RVA: 0x001DBCDC File Offset: 0x001D9EDC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Defs.Look<NeedDef>(ref this.need, "need");
			Scribe_Values.Look<float>(ref this.offset, "offset", 0f, false);
		}

		// Token: 0x06005D05 RID: 23813 RVA: 0x00040836 File Offset: 0x0003EA36
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

		// Token: 0x06005D06 RID: 23814 RVA: 0x00040870 File Offset: 0x0003EA70
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawn == replace)
			{
				this.pawn = with;
			}
		}

		// Token: 0x04003E43 RID: 15939
		public string inSignal;

		// Token: 0x04003E44 RID: 15940
		public Pawn pawn;

		// Token: 0x04003E45 RID: 15941
		public NeedDef need;

		// Token: 0x04003E46 RID: 15942
		public float offset;
	}
}
