using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010AE RID: 4270
	public class QuestPart_DamageUntilDowned : QuestPart
	{
		// Token: 0x17000E74 RID: 3700
		// (get) Token: 0x06005D1C RID: 23836 RVA: 0x00040947 File Offset: 0x0003EB47
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
					yield return this.pawns[i];
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06005D1D RID: 23837 RVA: 0x001DC1BC File Offset: 0x001DA3BC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (!this.pawns[i].DestroyedOrNull())
					{
						HealthUtility.DamageUntilDowned(this.pawns[i], this.allowBleedingWounds);
					}
				}
			}
		}

		// Token: 0x06005D1E RID: 23838 RVA: 0x001DC224 File Offset: 0x001DA424
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.allowBleedingWounds, "allowBleedingWounds", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06005D1F RID: 23839 RVA: 0x00040957 File Offset: 0x0003EB57
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.pawns.Add(PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>());
		}

		// Token: 0x06005D20 RID: 23840 RVA: 0x0004098E File Offset: 0x0003EB8E
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x04003E51 RID: 15953
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04003E52 RID: 15954
		public string inSignal;

		// Token: 0x04003E53 RID: 15955
		public bool allowBleedingWounds = true;
	}
}
