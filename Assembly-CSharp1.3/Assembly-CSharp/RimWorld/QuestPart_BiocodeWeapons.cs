using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B66 RID: 2918
	public class QuestPart_BiocodeWeapons : QuestPart
	{
		// Token: 0x0600443F RID: 17471 RVA: 0x0016A650 File Offset: 0x00168850
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.pawns[i].equipment != null)
					{
						foreach (ThingWithComps thingWithComps in this.pawns[i].equipment.AllEquipmentListForReading)
						{
							CompBiocodable comp = thingWithComps.GetComp<CompBiocodable>();
							if (comp != null && !comp.Biocoded)
							{
								comp.CodeFor(this.pawns[i]);
							}
						}
					}
				}
			}
		}

		// Token: 0x06004440 RID: 17472 RVA: 0x0016A718 File Offset: 0x00168918
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06004441 RID: 17473 RVA: 0x0016A786 File Offset: 0x00168986
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x0400296B RID: 10603
		public string inSignal;

		// Token: 0x0400296C RID: 10604
		public List<Pawn> pawns = new List<Pawn>();
	}
}
