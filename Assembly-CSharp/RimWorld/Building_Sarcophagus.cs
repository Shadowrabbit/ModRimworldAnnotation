using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016CE RID: 5838
	public class Building_Sarcophagus : Building_Grave
	{
		// Token: 0x0600802E RID: 32814 RVA: 0x000560F0 File Offset: 0x000542F0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.everNonEmpty, "everNonEmpty", false, false);
		}

		// Token: 0x0600802F RID: 32815 RVA: 0x0005610A File Offset: 0x0005430A
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

		// Token: 0x06008030 RID: 32816 RVA: 0x0025F370 File Offset: 0x0025D570
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
						pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowBuriedInSarcophagus, null);
					}
				}
			}
		}

		// Token: 0x0400530C RID: 21260
		private bool everNonEmpty;

		// Token: 0x0400530D RID: 21261
		private bool thisIsFirstBodyEver;
	}
}
