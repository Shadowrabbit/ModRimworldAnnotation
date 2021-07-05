using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EE7 RID: 3815
	public class PreceptComp_Apparel : PreceptComp
	{
		// Token: 0x06005A9B RID: 23195 RVA: 0x001F55DC File Offset: 0x001F37DC
		public Gender AffectedGender(Ideo ideo)
		{
			switch (this.gender)
			{
			case IdeoApparelGender.Any:
				return Gender.None;
			case IdeoApparelGender.SupremeGender:
				return ideo.SupremeGender;
			case IdeoApparelGender.SubordinateGender:
				return ideo.SupremeGender.Opposite();
			default:
				Log.Error("Unimplemented gender: " + this.gender.ToString());
				return Gender.None;
			}
		}

		// Token: 0x06005A9C RID: 23196 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool CanApplyToApparel(ThingDef apparelDef)
		{
			return true;
		}

		// Token: 0x06005A9D RID: 23197 RVA: 0x001F563C File Offset: 0x001F383C
		protected bool AppliesToPawn(Pawn pawn, Precept precept)
		{
			Precept_Apparel precept_Apparel;
			if (!ModsConfig.IdeologyActive || pawn.Ideo == null || (precept_Apparel = (precept as Precept_Apparel)) == null)
			{
				return false;
			}
			Gender targetGender = precept_Apparel.TargetGender;
			if (targetGender != Gender.None && pawn.gender != targetGender)
			{
				return false;
			}
			if (pawn.royalty != null)
			{
				foreach (RoyalTitle royalTitle in pawn.royalty.AllTitlesForReading)
				{
					if (!royalTitle.def.requiredApparel.NullOrEmpty<ApparelRequirement>())
					{
						using (List<ApparelRequirement>.Enumerator enumerator2 = royalTitle.def.requiredApparel.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								string text;
								if (ApparelUtility.IsRequirementActive(enumerator2.Current, ApparelRequirementSource.Title, pawn, out text))
								{
									return false;
								}
							}
						}
					}
				}
			}
			using (List<Apparel>.Enumerator enumerator3 = pawn.apparel.WornApparel.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					if (enumerator3.Current.def == precept_Apparel.apparelDef)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06005A9E RID: 23198 RVA: 0x001F5780 File Offset: 0x001F3980
		protected void GiveApparelToPawn(Pawn pawn, Precept_Apparel precept)
		{
			Apparel apparel = PawnApparelGenerator.GenerateApparelOfDefFor(pawn, precept.apparelDef);
			if (apparel != null)
			{
				PawnApparelGenerator.PostProcessApparel(apparel, pawn);
				PawnGenerator.PostProcessGeneratedGear(apparel, pawn);
				apparel.SetColor(pawn.Ideo.ApparelColor, false);
				pawn.apparel.Wear(apparel, false, false);
			}
		}

		// Token: 0x04003510 RID: 13584
		private IdeoApparelGender gender;
	}
}
