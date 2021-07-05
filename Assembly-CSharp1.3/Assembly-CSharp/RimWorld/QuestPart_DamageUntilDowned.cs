using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B6A RID: 2922
	public class QuestPart_DamageUntilDowned : QuestPart
	{
		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x06004459 RID: 17497 RVA: 0x0016AE14 File Offset: 0x00169014
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

		// Token: 0x0600445A RID: 17498 RVA: 0x0016AE24 File Offset: 0x00169024
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

		// Token: 0x0600445B RID: 17499 RVA: 0x0016AE8C File Offset: 0x0016908C
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

		// Token: 0x0600445C RID: 17500 RVA: 0x0016AF0C File Offset: 0x0016910C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.pawns.Add(PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>());
		}

		// Token: 0x0600445D RID: 17501 RVA: 0x0016AF43 File Offset: 0x00169143
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x04002978 RID: 10616
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04002979 RID: 10617
		public string inSignal;

		// Token: 0x0400297A RID: 10618
		public bool allowBleedingWounds = true;
	}
}
