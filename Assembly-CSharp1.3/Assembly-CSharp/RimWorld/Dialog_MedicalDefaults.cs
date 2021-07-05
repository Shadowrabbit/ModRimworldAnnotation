using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012F3 RID: 4851
	public class Dialog_MedicalDefaults : Window
	{
		// Token: 0x1700147A RID: 5242
		// (get) Token: 0x06007498 RID: 29848 RVA: 0x00279E47 File Offset: 0x00278047
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(346f, 350f);
			}
		}

		// Token: 0x06007499 RID: 29849 RVA: 0x00279E58 File Offset: 0x00278058
		public Dialog_MedicalDefaults()
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600749A RID: 29850 RVA: 0x00279E84 File Offset: 0x00278084
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, 170f, 28f);
			Rect rect2 = new Rect(170f, 0f, 140f, 28f);
			Widgets.Label(rect, "MedGroupColonist".Translate());
			MedicalCareUtility.MedicalCareSetter(rect2, ref Find.PlaySettings.defaultCareForColonyHumanlike);
			rect.y += 34f;
			rect2.y += 34f;
			Widgets.Label(rect, "MedGroupImprisonedColonist".Translate());
			MedicalCareUtility.MedicalCareSetter(rect2, ref Find.PlaySettings.defaultCareForColonyPrisoner);
			rect.y += 34f;
			rect2.y += 34f;
			if (ModsConfig.IdeologyActive)
			{
				Widgets.Label(rect, "MedGroupEnslavedColonist".Translate());
				MedicalCareUtility.MedicalCareSetter(rect2, ref Find.PlaySettings.defaultCareForColonySlave);
				rect.y += 34f;
				rect2.y += 34f;
			}
			Widgets.Label(rect, "MedGroupColonyAnimal".Translate());
			MedicalCareUtility.MedicalCareSetter(rect2, ref Find.PlaySettings.defaultCareForColonyAnimal);
			rect.y += 52f;
			rect2.y += 52f;
			Widgets.Label(rect, "MedGroupNeutralAnimal".Translate());
			MedicalCareUtility.MedicalCareSetter(rect2, ref Find.PlaySettings.defaultCareForNeutralAnimal);
			rect.y += 34f;
			rect2.y += 34f;
			Widgets.Label(rect, "MedGroupNeutralFaction".Translate());
			MedicalCareUtility.MedicalCareSetter(rect2, ref Find.PlaySettings.defaultCareForNeutralFaction);
			rect.y += 52f;
			rect2.y += 52f;
			Widgets.Label(rect, "MedGroupHostileFaction".Translate());
			MedicalCareUtility.MedicalCareSetter(rect2, ref Find.PlaySettings.defaultCareForHostileFaction);
		}

		// Token: 0x04004045 RID: 16453
		private const float MedicalCareStartX = 170f;

		// Token: 0x04004046 RID: 16454
		private const float VerticalGap = 6f;

		// Token: 0x04004047 RID: 16455
		private const float VerticalBigGap = 24f;
	}
}
