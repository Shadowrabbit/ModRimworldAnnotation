using System;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200179C RID: 6044
	public static class CaravanAbandonOrBanishUtility
	{
		// Token: 0x06008C12 RID: 35858 RVA: 0x00323BF0 File Offset: 0x00321DF0
		public static void TryAbandonOrBanishViaInterface(Thing t, Caravan caravan)
		{
			Pawn p = t as Pawn;
			if (p == null)
			{
				Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation("ConfirmAbandonItemDialog".Translate(t.Label), delegate
				{
					Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(caravan, t);
					if (ownerOf == null)
					{
						Log.Error("Could not find owner of " + t);
						return;
					}
					ownerOf.inventory.innerContainer.Remove(t);
					t.Destroy(DestroyMode.Vanish);
					caravan.RecacheImmobilizedNow();
					caravan.RecacheDaysWorthOfFood();
				}, true, null);
				Find.WindowStack.Add(window);
				return;
			}
			if (!caravan.PawnsListForReading.Any((Pawn x) => x != p && caravan.IsOwner(x)))
			{
				Messages.Message("MessageCantBanishLastColonist".Translate(), caravan, MessageTypeDefOf.RejectInput, false);
				return;
			}
			PawnBanishUtility.ShowBanishPawnConfirmationDialog(p, null);
		}

		// Token: 0x06008C13 RID: 35859 RVA: 0x00323CB4 File Offset: 0x00321EB4
		public static void TryAbandonOrBanishViaInterface(TransferableImmutable t, Caravan caravan)
		{
			Pawn pawn = t.AnyThing as Pawn;
			if (pawn != null)
			{
				CaravanAbandonOrBanishUtility.TryAbandonOrBanishViaInterface(pawn, caravan);
				return;
			}
			Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation("ConfirmAbandonItemDialog".Translate(t.LabelWithTotalStackCount), delegate
			{
				for (int i = 0; i < t.things.Count; i++)
				{
					Thing thing = t.things[i];
					Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(caravan, thing);
					if (ownerOf == null)
					{
						Log.Error("Could not find owner of " + thing);
						return;
					}
					ownerOf.inventory.innerContainer.Remove(thing);
					thing.Destroy(DestroyMode.Vanish);
				}
				caravan.RecacheImmobilizedNow();
				caravan.RecacheDaysWorthOfFood();
			}, true, null);
			Find.WindowStack.Add(window);
		}

		// Token: 0x06008C14 RID: 35860 RVA: 0x00323D30 File Offset: 0x00321F30
		public static void TryAbandonSpecificCountViaInterface(Thing t, Caravan caravan)
		{
			Find.WindowStack.Add(new Dialog_Slider("AbandonSliderText".Translate(t.LabelNoCount), 1, t.stackCount, delegate(int x)
			{
				Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(caravan, t);
				if (ownerOf == null)
				{
					Log.Error("Could not find owner of " + t);
					return;
				}
				if (x >= t.stackCount)
				{
					ownerOf.inventory.innerContainer.Remove(t);
					t.Destroy(DestroyMode.Vanish);
				}
				else
				{
					t.SplitOff(x).Destroy(DestroyMode.Vanish);
				}
				caravan.RecacheImmobilizedNow();
				caravan.RecacheDaysWorthOfFood();
			}, int.MinValue, 1f));
		}

		// Token: 0x06008C15 RID: 35861 RVA: 0x00323DA4 File Offset: 0x00321FA4
		public static void TryAbandonSpecificCountViaInterface(TransferableImmutable t, Caravan caravan)
		{
			Find.WindowStack.Add(new Dialog_Slider("AbandonSliderText".Translate(t.Label), 1, t.TotalStackCount, delegate(int x)
			{
				int num = x;
				int num2 = 0;
				while (num2 < t.things.Count && num > 0)
				{
					Thing thing = t.things[num2];
					Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(caravan, thing);
					if (ownerOf == null)
					{
						Log.Error("Could not find owner of " + thing);
						return;
					}
					if (num >= thing.stackCount)
					{
						num -= thing.stackCount;
						ownerOf.inventory.innerContainer.Remove(thing);
						thing.Destroy(DestroyMode.Vanish);
					}
					else
					{
						thing.SplitOff(num).Destroy(DestroyMode.Vanish);
						num = 0;
					}
					num2++;
				}
				caravan.RecacheImmobilizedNow();
				caravan.RecacheDaysWorthOfFood();
			}, int.MinValue, 1f));
		}

		// Token: 0x06008C16 RID: 35862 RVA: 0x00323E18 File Offset: 0x00322018
		public static string GetAbandonOrBanishButtonTooltip(Thing t, bool abandonSpecificCount)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				return PawnBanishUtility.GetBanishButtonTip(pawn);
			}
			return CaravanAbandonOrBanishUtility.GetAbandonItemButtonTooltip(t.stackCount, abandonSpecificCount);
		}

		// Token: 0x06008C17 RID: 35863 RVA: 0x00323E44 File Offset: 0x00322044
		public static string GetAbandonOrBanishButtonTooltip(TransferableImmutable t, bool abandonSpecificCount)
		{
			Pawn pawn = t.AnyThing as Pawn;
			if (pawn != null)
			{
				return PawnBanishUtility.GetBanishButtonTip(pawn);
			}
			return CaravanAbandonOrBanishUtility.GetAbandonItemButtonTooltip(t.TotalStackCount, abandonSpecificCount);
		}

		// Token: 0x06008C18 RID: 35864 RVA: 0x00323E74 File Offset: 0x00322074
		private static string GetAbandonItemButtonTooltip(int currentStackCount, bool abandonSpecificCount)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (currentStackCount == 1)
			{
				stringBuilder.AppendLine("AbandonTip".Translate());
			}
			else if (abandonSpecificCount)
			{
				stringBuilder.AppendLine("AbandonSpecificCountTip".Translate());
			}
			else
			{
				stringBuilder.AppendLine("AbandonAllTip".Translate());
			}
			stringBuilder.AppendLine();
			stringBuilder.Append("AbandonItemTipExtraText".Translate());
			return stringBuilder.ToString();
		}
	}
}
