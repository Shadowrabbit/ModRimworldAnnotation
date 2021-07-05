using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200033F RID: 831
	public abstract class PawnTrigger : Thing
	{
		// Token: 0x060017B6 RID: 6070 RVA: 0x0008DBE4 File Offset: 0x0008BDE4
		protected bool TriggeredBy(Thing thing)
		{
			return thing.def.category == ThingCategory.Pawn && thing.def.race.intelligence == Intelligence.Humanlike && thing.Faction == Faction.OfPlayer;
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x0008DC16 File Offset: 0x0008BE16
		public void ActivatedBy(Pawn p)
		{
			Find.SignalManager.SendSignal(new Signal(this.signalTag, p.Named("SUBJECT")));
			if (!base.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x0008DC47 File Offset: 0x0008BE47
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
		}

		// Token: 0x04001054 RID: 4180
		public string signalTag;
	}
}
