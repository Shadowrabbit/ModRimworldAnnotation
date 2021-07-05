using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EDD RID: 3805
	public class PreceptComp_BedThought : PreceptComp_Thought
	{
		// Token: 0x06005A7D RID: 23165 RVA: 0x001F4E30 File Offset: 0x001F3030
		public override void Notify_AddBedThoughts(Pawn pawn, Precept precept)
		{
			base.Notify_AddBedThoughts(pawn, precept);
			pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDef(this.thought);
			if (this.requireFacility != null && !this.HasActiveLinkedFacility(pawn))
			{
				return;
			}
			Thought_Memory newThought = ThoughtMaker.MakeThought(this.thought, precept);
			pawn.needs.mood.thoughts.memories.TryGainMemory(newThought, null);
		}

		// Token: 0x06005A7E RID: 23166 RVA: 0x001F4EA0 File Offset: 0x001F30A0
		private bool HasActiveLinkedFacility(Pawn pawn)
		{
			Building_Bed building_Bed = pawn.CurrentBed();
			CompAffectedByFacilities compAffectedByFacilities = (building_Bed != null) ? building_Bed.TryGetComp<CompAffectedByFacilities>() : null;
			if (compAffectedByFacilities == null)
			{
				return false;
			}
			foreach (Thing thing in compAffectedByFacilities.LinkedFacilitiesListForReading)
			{
				if (thing.def == this.requireFacility.def && compAffectedByFacilities.IsFacilityActive(thing))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005A7F RID: 23167 RVA: 0x001F4F28 File Offset: 0x001F3128
		public override IEnumerable<string> ConfigErrors(PreceptDef parent)
		{
			foreach (string text in base.ConfigErrors(parent))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.requireFacility != null && this.requireFacility.def == null)
			{
				yield return "<requireFacility> has null <def>";
			}
			yield break;
			yield break;
		}

		// Token: 0x040034FC RID: 13564
		public PreceptComp_BedThought.FacilityData requireFacility;

		// Token: 0x02002376 RID: 9078
		public class FacilityData
		{
			// Token: 0x040086F5 RID: 34549
			public ThingDef def;
		}
	}
}
