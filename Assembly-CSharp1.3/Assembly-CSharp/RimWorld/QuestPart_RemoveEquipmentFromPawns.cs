using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B93 RID: 2963
	public class QuestPart_RemoveEquipmentFromPawns : QuestPart
	{
		// Token: 0x06004544 RID: 17732 RVA: 0x0016FA20 File Offset: 0x0016DC20
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.pawns[i] != null && this.pawns[i].equipment != null)
					{
						this.pawns[i].equipment.DestroyAllEquipment(DestroyMode.Vanish);
					}
				}
			}
		}

		// Token: 0x06004545 RID: 17733 RVA: 0x0016FA98 File Offset: 0x0016DC98
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04002A38 RID: 10808
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04002A39 RID: 10809
		public string inSignal;
	}
}
