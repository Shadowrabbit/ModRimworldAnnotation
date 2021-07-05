using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D9E RID: 3486
	public class Recipe_RemoveBodyPart_CutMany : Recipe_RemoveBodyPart_Cut
	{
		// Token: 0x060050D5 RID: 20693 RVA: 0x001B09D1 File Offset: 0x001AEBD1
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			IEnumerable<BodyPartRecord> source = this.FixedPartsToApplyOn(pawn, recipe);
			if (source.Any<BodyPartRecord>() && recipe.minPartCount <= source.Count<BodyPartRecord>())
			{
				yield return source.First<BodyPartRecord>();
			}
			yield break;
		}

		// Token: 0x060050D6 RID: 20694 RVA: 0x001B09F0 File Offset: 0x001AEBF0
		public IEnumerable<BodyPartRecord> FixedPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			return MedicalRecipesUtility.GetFixedPartsToApplyOn(recipe, pawn, (BodyPartRecord record) => !pawn.health.hediffSet.PartIsMissing(record));
		}

		// Token: 0x060050D7 RID: 20695 RVA: 0x001B0A24 File Offset: 0x001AEC24
		public override void DamagePart(Pawn pawn, BodyPartRecord part)
		{
			foreach (BodyPartRecord hitPart in this.FixedPartsToApplyOn(pawn, this.recipe))
			{
				pawn.TakeDamage(new DamageInfo(DamageDefOf.SurgicalCut, 99999f, 999f, -1f, null, hitPart, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
			}
		}

		// Token: 0x060050D8 RID: 20696 RVA: 0x0001C72F File Offset: 0x0001A92F
		public override string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			return this.recipe.label;
		}
	}
}
