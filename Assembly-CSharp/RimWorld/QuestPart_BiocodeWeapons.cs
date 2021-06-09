using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010A5 RID: 4261
	public class QuestPart_BiocodeWeapons : QuestPart
	{
		// Token: 0x06005CE9 RID: 23785 RVA: 0x001DB898 File Offset: 0x001D9A98
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
							CompBiocodableWeapon comp = thingWithComps.GetComp<CompBiocodableWeapon>();
							if (comp != null && !comp.Biocoded)
							{
								comp.CodeFor(this.pawns[i]);
							}
						}
					}
				}
			}
		}

		// Token: 0x06005CEA RID: 23786 RVA: 0x001DB960 File Offset: 0x001D9B60
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

		// Token: 0x06005CEB RID: 23787 RVA: 0x00040765 File Offset: 0x0003E965
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x04003E34 RID: 15924
		public string inSignal;

		// Token: 0x04003E35 RID: 15925
		public List<Pawn> pawns = new List<Pawn>();
	}
}
