using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001491 RID: 5265
	public static class MassUtility
	{
		// Token: 0x06007DD4 RID: 32212 RVA: 0x002C94A7 File Offset: 0x002C76A7
		public static float EncumbrancePercent(Pawn pawn)
		{
			return Mathf.Clamp01(MassUtility.UnboundedEncumbrancePercent(pawn));
		}

		// Token: 0x06007DD5 RID: 32213 RVA: 0x002C94B4 File Offset: 0x002C76B4
		public static float UnboundedEncumbrancePercent(Pawn pawn)
		{
			return MassUtility.GearAndInventoryMass(pawn) / MassUtility.Capacity(pawn, null);
		}

		// Token: 0x06007DD6 RID: 32214 RVA: 0x002C94C4 File Offset: 0x002C76C4
		public static bool IsOverEncumbered(Pawn pawn)
		{
			return MassUtility.UnboundedEncumbrancePercent(pawn) > 1f;
		}

		// Token: 0x06007DD7 RID: 32215 RVA: 0x002C94D3 File Offset: 0x002C76D3
		public static bool WillBeOverEncumberedAfterPickingUp(Pawn pawn, Thing thing, int count)
		{
			return MassUtility.FreeSpace(pawn) < (float)count * thing.GetStatValue(StatDefOf.Mass, true);
		}

		// Token: 0x06007DD8 RID: 32216 RVA: 0x002C94EC File Offset: 0x002C76EC
		public static int CountToPickUpUntilOverEncumbered(Pawn pawn, Thing thing)
		{
			return Mathf.FloorToInt(MassUtility.FreeSpace(pawn) / thing.GetStatValue(StatDefOf.Mass, true));
		}

		// Token: 0x06007DD9 RID: 32217 RVA: 0x002C9506 File Offset: 0x002C7706
		public static float FreeSpace(Pawn pawn)
		{
			return Mathf.Max(MassUtility.Capacity(pawn, null) - MassUtility.GearAndInventoryMass(pawn), 0f);
		}

		// Token: 0x06007DDA RID: 32218 RVA: 0x002C9520 File Offset: 0x002C7720
		public static float GearAndInventoryMass(Pawn pawn)
		{
			return MassUtility.GearMass(pawn) + MassUtility.InventoryMass(pawn);
		}

		// Token: 0x06007DDB RID: 32219 RVA: 0x002C9530 File Offset: 0x002C7730
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

		// Token: 0x06007DDC RID: 32220 RVA: 0x002C95DC File Offset: 0x002C77DC
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

		// Token: 0x06007DDD RID: 32221 RVA: 0x002C9634 File Offset: 0x002C7834
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

		// Token: 0x06007DDE RID: 32222 RVA: 0x002C9692 File Offset: 0x002C7892
		public static bool CanEverCarryAnything(Pawn p)
		{
			return p.RaceProps.ToolUser || p.RaceProps.packAnimal;
		}

		// Token: 0x04004E77 RID: 20087
		public const float MassCapacityPerBodySize = 35f;
	}
}
