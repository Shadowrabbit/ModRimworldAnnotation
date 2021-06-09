using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010F6 RID: 4342
	public class QuestPart_RemoveEquipmentFromPawns : QuestPart
	{
		// Token: 0x06005EE0 RID: 24288 RVA: 0x001E12D8 File Offset: 0x001DF4D8
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

		// Token: 0x06005EE1 RID: 24289 RVA: 0x001E1350 File Offset: 0x001DF550
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

		// Token: 0x04003F88 RID: 16264
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04003F89 RID: 16265
		public string inSignal;
	}
}
