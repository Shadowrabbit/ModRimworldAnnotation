using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CD0 RID: 7376
	public static class MassUtility
	{
		// Token: 0x0600A050 RID: 41040 RVA: 0x0006AD8E File Offset: 0x00068F8E
		public static float EncumbrancePercent(Pawn pawn)
		{
			return Mathf.Clamp01(MassUtility.UnboundedEncumbrancePercent(pawn));
		}

		// Token: 0x0600A051 RID: 41041 RVA: 0x0006AD9B File Offset: 0x00068F9B
		public static float UnboundedEncumbrancePercent(Pawn pawn)
		{
			return MassUtility.GearAndInventoryMass(pawn) / MassUtility.Capacity(pawn, null);
		}

		// Token: 0x0600A052 RID: 41042 RVA: 0x0006ADAB File Offset: 0x00068FAB
		public static bool IsOverEncumbered(Pawn pawn)
		{
			return MassUtility.UnboundedEncumbrancePercent(pawn) > 1f;
		}

		// Token: 0x0600A053 RID: 41043 RVA: 0x0006ADBA File Offset: 0x00068FBA
		public static bool WillBeOverEncumberedAfterPickingUp(Pawn pawn, Thing thing, int count)
		{
			return MassUtility.FreeSpace(pawn) < (float)count * thing.GetStatValue(StatDefOf.Mass, true);
		}

		// Token: 0x0600A054 RID: 41044 RVA: 0x0006ADD3 File Offset: 0x00068FD3
		public static int CountToPickUpUntilOverEncumbered(Pawn pawn, Thing thing)
		{
			return Mathf.FloorToInt(MassUtility.FreeSpace(pawn) / thing.GetStatValue(StatDefOf.Mass, true));
		}

		// Token: 0x0600A055 RID: 41045 RVA: 0x0006ADED File Offset: 0x00068FED
		public static float FreeSpace(Pawn pawn)
		{
			return Mathf.Max(MassUtility.Capacity(pawn, null) - MassUtility.GearAndInventoryMass(pawn), 0f);
		}

		// Token: 0x0600A056 RID: 41046 RVA: 0x0006AE07 File Offset: 0x00069007
		public static float GearAndInventoryMass(Pawn pawn)
		{
			return MassUtility.GearMass(pawn) + MassUtility.InventoryMass(pawn);
		}

		// Token: 0x0600A057 RID: 41047 RVA: 0x002EE930 File Offset: 0x002ECB30
		public static float GearMass(Pawn p)
		{
			float num = 0f;
			if (p.apparel != null)
			{
				List<Apparel> wornApparel = p.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					num += wornApparel[i].GetStatValue(StatDefOf.Mass, true);
				}
			}
			if (p.equipment != null)
			{
				foreach (ThingWithComps thing in p.equipment.AllEquipmentListForReading)
				{
					num += thing.GetStatValue(StatDefOf.Mass, true);
				}
			}
			return num;
		}

		// Token: 0x0600A058 RID: 41048 RVA: 0x002EE9DC File Offset: 0x002ECBDC
		public static float InventoryMass(Pawn p)
		{
			float num = 0f;
			for (int i = 0; i < p.inventory.innerContainer.Count; i++)
			{
				Thing thing = p.inventory.innerContainer[i];
				num += (float)thing.stackCount * thing.GetStatValue(StatDefOf.Mass, true);
			}
			return num;
		}

		// Token: 0x0600A059 RID: 41049 RVA: 0x002EEA34 File Offset: 0x002ECC34
		public static float Capacity(Pawn p, StringBuilder explanation = null)
		{
			if (!MassUtility.CanEverCarryAnything(p))
			{
				return 0f;
			}
			float num = p.BodySize * 35f;
			if (explanation != null)
			{
				if (explanation.Length > 0)
				{
					explanation.AppendLine();
				}
				explanation.Append("  - " + p.LabelShortCap + ": " + num.ToStringMassOffset());
			}
			return num;
		}

		// Token: 0x0600A05A RID: 41050 RVA: 0x0006AE16 File Offset: 0x00069016
		public static bool CanEverCarryAnything(Pawn p)
		{
			return p.RaceProps.ToolUser || p.RaceProps.packAnimal;
		}

		// Token: 0x04006CF3 RID: 27891
		public const float MassCapacityPerBodySize = 35f;
	}
}
