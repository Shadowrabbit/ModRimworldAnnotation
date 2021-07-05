using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D9D RID: 3485
	public class Recipe_RemoveBodyPart_Cut : Recipe_RemoveBodyPart
	{
		// Token: 0x17000E00 RID: 3584
		// (get) Token: 0x060050D0 RID: 20688 RVA: 0x0001276E File Offset: 0x0001096E
		protected override bool SpawnPartsWhenRemoved
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060050D1 RID: 20689 RVA: 0x001B0974 File Offset: 0x001AEB74
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			return MedicalRecipesUtility.GetFixedPartsToApplyOn(recipe, pawn, (BodyPartRecord record) => !pawn.health.hediffSet.PartIsMissing(record));
		}

		// Token: 0x060050D2 RID: 20690 RVA: 0x001B09A6 File Offset: 0x001AEBA6
		public override void ApplyThoughts(Pawn pawn, Pawn billDoer)
		{
			if (pawn.Dead)
			{
				ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, billDoer, PawnExecutionKind.GenericBrutal);
			}
		}

		// Token: 0x060050D3 RID: 20691 RVA: 0x001B09B8 File Offset: 0x001AEBB8
		public override string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			return "Cut".Translate();
		}
	}
}
