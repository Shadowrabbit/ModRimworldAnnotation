using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001084 RID: 4228
	public class Building_Sarcophagus : Building_Grave
	{
		// Token: 0x060064A3 RID: 25763 RVA: 0x0021E802 File Offset: 0x0021CA02
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.everNonEmpty, "everNonEmpty", false, false);
		}

		// Token: 0x060064A4 RID: 25764 RVA: 0x0021E81C File Offset: 0x0021CA1C
		public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			if (base.TryAcceptThing(thing, allowSpecialEffects))
			{
				this.thisIsFirstBodyEver = !this.everNonEmpty;
				this.everNonEmpty = true;
				return true;
			}
			return false;
		}

		// Token: 0x060064A5 RID: 25765 RVA: 0x0021E844 File Offset: 0x0021CA44
		public override void Notify_CorpseBuried(Pawn worker)
		{
			base.Notify_CorpseBuried(worker);
			if (this.thisIsFirstBodyEver && worker.IsColonist && base.Corpse.InnerPawn.def.race.Humanlike && !base.Corpse.everBuriedInSarcophagus)
			{
				base.Corpse.everBuriedInSarcophagus = true;
				foreach (Pawn pawn in base.Map.mapPawns.FreeColonists)
				{
					if (pawn.needs.mood != null)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowBuriedInSarcophagus, null, null);
					}
				}
			}
		}

		// Token: 0x040038A9 RID: 14505
		private bool everNonEmpty;

		// Token: 0x040038AA RID: 14506
		private bool thisIsFirstBodyEver;
	}
}
