using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200165B RID: 5723
	public class QuestPart_SetupTransportShip : QuestPart
	{
		// Token: 0x0600857A RID: 34170 RVA: 0x002FE7CA File Offset: 0x002FC9CA
		public override bool QuestPartReserves(Pawn p)
		{
			return (this.pawns != null && this.pawns.Contains(p)) || (this.transportShip != null && this.transportShip.TransporterComp.innerContainer.Contains(p));
		}

		// Token: 0x0600857B RID: 34171 RVA: 0x002FE804 File Offset: 0x002FCA04
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			List<Pawn> list = this.pawns;
			if (list == null)
			{
				return;
			}
			list.Replace(replace, with);
		}

		// Token: 0x0600857C RID: 34172 RVA: 0x002FE81C File Offset: 0x002FCA1C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				if (!this.pawns.NullOrEmpty<Pawn>())
				{
					using (List<Pawn>.Enumerator enumerator = this.pawns.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (!enumerator.Current.IsWorldPawn())
							{
								Log.Error("Trying to transfer a non-world pawn to a transportShip.");
							}
						}
					}
					this.transportShip.TransporterComp.innerContainer.TryAddRangeOrTransfer(this.pawns, true, true);
				}
				if (!this.items.NullOrEmpty<Thing>())
				{
					this.transportShip.TransporterComp.innerContainer.TryAddRangeOrTransfer(this.items, true, true);
				}
				this.transportShip.Start();
			}
		}

		// Token: 0x0600857D RID: 34173 RVA: 0x002FE8F0 File Offset: 0x002FCAF0
		public override void Cleanup()
		{
			base.Cleanup();
			this.transportShip = null;
			this.items = null;
			this.pawns = null;
		}

		// Token: 0x0600857E RID: 34174 RVA: 0x002FE910 File Offset: 0x002FCB10
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<TransportShip>(ref this.transportShip, "transportShip", false);
			Scribe_Collections.Look<Thing>(ref this.items, "items", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (!this.items.NullOrEmpty<Thing>())
				{
					this.items.RemoveAll((Thing x) => x == null);
				}
				if (!this.pawns.NullOrEmpty<Pawn>())
				{
					this.pawns.RemoveAll((Pawn x) => x == null);
				}
			}
		}

		// Token: 0x04005359 RID: 21337
		public TransportShip transportShip;

		// Token: 0x0400535A RID: 21338
		public List<Pawn> pawns;

		// Token: 0x0400535B RID: 21339
		public List<Thing> items;

		// Token: 0x0400535C RID: 21340
		public string inSignal;
	}
}
