using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000E80 RID: 3712
	[StaticConstructorOnStartup]
	public static class MedicalCareUtility
	{
		// Token: 0x060056DF RID: 22239 RVA: 0x001D7C16 File Offset: 0x001D5E16
		public static void Reset()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				MedicalCareUtility.careTextures = new Texture2D[5];
				MedicalCareUtility.careTextures[0] = ContentFinder<Texture2D>.Get("UI/Icons/Medical/NoCare", true);
				MedicalCareUtility.careTextures[1] = ContentFinder<Texture2D>.Get("UI/Icons/Medical/NoMeds", true);
				MedicalCareUtility.careTextures[2] = ThingDefOf.MedicineHerbal.uiIcon;
				MedicalCareUtility.careTextures[3] = ThingDefOf.MedicineIndustrial.uiIcon;
				MedicalCareUtility.careTextures[4] = ThingDefOf.MedicineUltratech.uiIcon;
			});
		}

		// Token: 0x060056E0 RID: 22240 RVA: 0x001D7C3C File Offset: 0x001D5E3C
		public static void MedicalCareSetter(Rect rect, ref MedicalCareCategory medCare)
		{
			Rect rect2 = new Rect(rect.x, rect.y, rect.width / 5f, rect.height);
			for (int i = 0; i < 5; i++)
			{
				MedicalCareCategory mc = (MedicalCareCategory)i;
				Widgets.DrawHighlightIfMouseover(rect2);
				MouseoverSounds.DoRegion(rect2);
				GUI.DrawTexture(rect2, MedicalCareUtility.careTextures[i]);
				Widgets.DraggableResult draggableResult = Widgets.ButtonInvisibleDraggable(rect2, false);
				if (draggableResult == Widgets.DraggableResult.Dragged)
				{
					MedicalCareUtility.medicalCarePainting = true;
				}
				if ((MedicalCareUtility.medicalCarePainting && Mouse.IsOver(rect2) && medCare != mc) || draggableResult.AnyPressed())
				{
					medCare = mc;
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				if (medCare == mc)
				{
					Widgets.DrawBox(rect2, 3, null);
				}
				if (Mouse.IsOver(rect2))
				{
					TooltipHandler.TipRegion(rect2, () => mc.GetLabel(), 632165 + i * 17);
				}
				rect2.x += rect2.width;
			}
			if (!Input.GetMouseButton(0))
			{
				MedicalCareUtility.medicalCarePainting = false;
			}
		}

		// Token: 0x060056E1 RID: 22241 RVA: 0x001D7D4A File Offset: 0x001D5F4A
		public static string GetLabel(this MedicalCareCategory cat)
		{
			return ("MedicalCareCategory_" + cat).Translate();
		}

		// Token: 0x060056E2 RID: 22242 RVA: 0x001D7D68 File Offset: 0x001D5F68
		public static bool AllowsMedicine(this MedicalCareCategory cat, ThingDef meds)
		{
			switch (cat)
			{
			case MedicalCareCategory.NoCare:
				return false;
			case MedicalCareCategory.NoMeds:
				return false;
			case MedicalCareCategory.HerbalOrWorse:
				return meds.GetStatValueAbstract(StatDefOf.MedicalPotency, null) <= ThingDefOf.MedicineHerbal.GetStatValueAbstract(StatDefOf.MedicalPotency, null);
			case MedicalCareCategory.NormalOrWorse:
				return meds.GetStatValueAbstract(StatDefOf.MedicalPotency, null) <= ThingDefOf.MedicineIndustrial.GetStatValueAbstract(StatDefOf.MedicalPotency, null);
			case MedicalCareCategory.Best:
				return true;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x060056E3 RID: 22243 RVA: 0x001D7DE0 File Offset: 0x001D5FE0
		public static void MedicalCareSelectButton(Rect rect, Pawn pawn)
		{
			Widgets.Dropdown<Pawn, MedicalCareCategory>(rect, pawn, new Func<Pawn, MedicalCareCategory>(MedicalCareUtility.MedicalCareSelectButton_GetMedicalCare), new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<MedicalCareCategory>>>(MedicalCareUtility.MedicalCareSelectButton_GenerateMenu), null, MedicalCareUtility.careTextures[(int)pawn.playerSettings.medCare], null, null, null, true);
		}

		// Token: 0x060056E4 RID: 22244 RVA: 0x001D7E22 File Offset: 0x001D6022
		private static MedicalCareCategory MedicalCareSelectButton_GetMedicalCare(Pawn pawn)
		{
			return pawn.playerSettings.medCare;
		}

		// Token: 0x060056E5 RID: 22245 RVA: 0x001D7E2F File Offset: 0x001D602F
		private static IEnumerable<Widgets.DropdownMenuElement<MedicalCareCategory>> MedicalCareSelectButton_GenerateMenu(Pawn p)
		{
			int num;
			for (int i = 0; i < 5; i = num + 1)
			{
				MedicalCareCategory mc = (MedicalCareCategory)i;
				yield return new Widgets.DropdownMenuElement<MedicalCareCategory>
				{
					option = new FloatMenuOption(mc.GetLabel(), delegate()
					{
						p.playerSettings.medCare = mc;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
					payload = mc
				};
				num = i;
			}
			yield break;
		}

		// Token: 0x04003348 RID: 13128
		private static Texture2D[] careTextures;

		// Token: 0x04003349 RID: 13129
		public const float CareSetterHeight = 28f;

		// Token: 0x0400334A RID: 13130
		public const float CareSetterWidth = 140f;

		// Token: 0x0400334B RID: 13131
		private static bool medicalCarePainting;
	}
}
