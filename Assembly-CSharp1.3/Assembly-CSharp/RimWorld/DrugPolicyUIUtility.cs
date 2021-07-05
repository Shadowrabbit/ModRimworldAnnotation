using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001393 RID: 5011
	public static class DrugPolicyUIUtility
	{
		// Token: 0x060079E0 RID: 31200 RVA: 0x002B0870 File Offset: 0x002AEA70
		public static void DoAssignDrugPolicyButtons(Rect rect, Pawn pawn)
		{
			int num = Mathf.FloorToInt((rect.width - 4f) * 0.71428573f);
			int num2 = Mathf.FloorToInt((rect.width - 4f) * 0.2857143f);
			float num3 = rect.x;
			Rect rect2 = new Rect(num3, rect.y + 2f, (float)num, rect.height - 4f);
			string text = pawn.drugs.CurrentPolicy.label;
			if (pawn.story != null && pawn.story.traits != null)
			{
				Trait trait = pawn.story.traits.GetTrait(TraitDefOf.DrugDesire);
				if (trait != null)
				{
					text = text + " (" + trait.Label + ")";
				}
			}
			Widgets.Dropdown<Pawn, DrugPolicy>(rect2, pawn, (Pawn p) => p.drugs.CurrentPolicy, new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<DrugPolicy>>>(DrugPolicyUIUtility.Button_GenerateMenu), text.Truncate(rect2.width, null), null, pawn.drugs.CurrentPolicy.label, null, delegate()
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.DrugPolicies, KnowledgeAmount.Total);
			}, true);
			num3 += (float)num;
			num3 += 4f;
			Rect rect3 = new Rect(num3, rect.y + 2f, (float)num2, rect.height - 4f);
			if (Widgets.ButtonText(rect3, "AssignTabEdit".Translate(), true, true, true))
			{
				Find.WindowStack.Add(new Dialog_ManageDrugPolicies(pawn.drugs.CurrentPolicy));
			}
			UIHighlighter.HighlightOpportunity(rect2, "ButtonAssignDrugs");
			UIHighlighter.HighlightOpportunity(rect3, "ButtonAssignDrugs");
			num3 += (float)num2;
		}

		// Token: 0x060079E1 RID: 31201 RVA: 0x002B0A27 File Offset: 0x002AEC27
		private static IEnumerable<Widgets.DropdownMenuElement<DrugPolicy>> Button_GenerateMenu(Pawn pawn)
		{
			using (List<DrugPolicy>.Enumerator enumerator = Current.Game.drugPolicyDatabase.AllPolicies.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DrugPolicy assignedDrugs = enumerator.Current;
					yield return new Widgets.DropdownMenuElement<DrugPolicy>
					{
						option = new FloatMenuOption(assignedDrugs.label, delegate()
						{
							pawn.drugs.CurrentPolicy = assignedDrugs;
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
						payload = assignedDrugs
					};
				}
			}
			List<DrugPolicy>.Enumerator enumerator = default(List<DrugPolicy>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0400438B RID: 17291
		public const string AssigningDrugsTutorHighlightTag = "ButtonAssignDrugs";
	}
}
