using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010A1 RID: 4257
	public class SignalAction_OpenDoor : SignalAction
	{
		// Token: 0x06006575 RID: 25973 RVA: 0x002242D8 File Offset: 0x002224D8
		protected override void DoAction(SignalArgs args)
		{
			if (this.door != null)
			{
				Pawn opener;
				if (args.TryGetArg<Pawn>("SUBJECT", out opener))
				{
					this.door.StartManualOpenBy(opener);
					return;
				}
				this.door.StartManualOpenBy(null);
			}
		}

		// Token: 0x06006576 RID: 25974 RVA: 0x00224316 File Offset: 0x00222516
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Building_Door>(ref this.door, "door", false);
		}

		// Token: 0x04003927 RID: 14631
		public Building_Door door;
	}
}
