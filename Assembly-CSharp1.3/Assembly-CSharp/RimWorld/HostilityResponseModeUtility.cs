using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E7E RID: 3710
	[StaticConstructorOnStartup]
	public static class HostilityResponseModeUtility
	{
		// Token: 0x060056D8 RID: 22232 RVA: 0x001D7A53 File Offset: 0x001D5C53
		public static Texture2D GetIcon(this HostilityResponseMode response)
		{
			switch (response)
			{
			case HostilityResponseMode.Ignore:
				return HostilityResponseModeUtility.IgnoreIcon;
			case HostilityResponseMode.Attack:
				return HostilityResponseModeUtility.AttackIcon;
			case HostilityResponseMode.Flee:
				return HostilityResponseModeUtility.FleeIcon;
			default:
				return BaseContent.BadTex;
			}
		}

		// Token: 0x060056D9 RID: 22233 RVA: 0x001D7A80 File Offset: 0x001D5C80
		public static HostilityResponseMode GetNextResponse(Pawn pawn)
		{
			switch (pawn.playerSettings.hostilityResponse)
			{
			case HostilityResponseMode.Ignore:
				if (pawn.WorkTagIsDisabled(WorkTags.Violent))
				{
					return HostilityResponseMode.Flee;
				}
				return HostilityResponseMode.Attack;
			case HostilityResponseMode.Attack:
				return HostilityResponseMode.Flee;
			case HostilityResponseMode.Flee:
				return HostilityResponseMode.Ignore;
			default:
				return HostilityResponseMode.Ignore;
			}
		}

		// Token: 0x060056DA RID: 22234 RVA: 0x001D7ABF File Offset: 0x001D5CBF
		public static string GetLabel(this HostilityResponseMode response)
		{
			return ("HostilityResponseMode_" + response).Translate();
		}

		// Token: 0x060056DB RID: 22235 RVA: 0x001D7ADC File Offset: 0x001D5CDC
		public static void DrawResponseButton(Rect rect, Pawn pawn, bool paintable)
		{
			Widgets.Dropdown<Pawn, HostilityResponseMode>(rect, pawn, HostilityResponseModeUtility.IconColor, new Func<Pawn, HostilityResponseMode>(HostilityResponseModeUtility.DrawResponseButton_GetResponse), new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<HostilityResponseMode>>>(HostilityResponseModeUtility.DrawResponseButton_GenerateMenu), null, pawn.playerSettings.hostilityResponse.GetIcon(), null, null, delegate()
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.HostilityResponse, KnowledgeAmount.SpecificInteraction);
			}, paintable);
			UIHighlighter.HighlightOpportunity(rect, "HostilityResponse");
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, "HostilityReponseTip".Translate() + "\n\n" + "HostilityResponseCurrentMode".Translate() + ": " + pawn.playerSettings.hostilityResponse.GetLabel());
			}
		}

		// Token: 0x060056DC RID: 22236 RVA: 0x001D7BA0 File Offset: 0x001D5DA0
		private static HostilityResponseMode DrawResponseButton_GetResponse(Pawn pawn)
		{
			return pawn.playerSettings.hostilityResponse;
		}

		// Token: 0x060056DD RID: 22237 RVA: 0x001D7BAD File Offset: 0x001D5DAD
		private static IEnumerable<Widgets.DropdownMenuElement<HostilityResponseMode>> DrawResponseButton_GenerateMenu(Pawn p)
		{
			using (IEnumerator enumerator = Enum.GetValues(typeof(HostilityResponseMode)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					HostilityResponseMode response = (HostilityResponseMode)enumerator.Current;
					if (response != HostilityResponseMode.Attack || !p.WorkTagIsDisabled(WorkTags.Violent))
					{
						yield return new Widgets.DropdownMenuElement<HostilityResponseMode>
						{
							option = new FloatMenuOption(response.GetLabel(), delegate()
							{
								p.playerSettings.hostilityResponse = response;
							}, response.GetIcon(), Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
							payload = response
						};
					}
				}
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0400333E RID: 13118
		private static readonly Texture2D IgnoreIcon = ContentFinder<Texture2D>.Get("UI/Icons/HostilityResponse/Ignore", true);

		// Token: 0x0400333F RID: 13119
		private static readonly Texture2D AttackIcon = ContentFinder<Texture2D>.Get("UI/Icons/HostilityResponse/Attack", true);

		// Token: 0x04003340 RID: 13120
		private static readonly Texture2D FleeIcon = ContentFinder<Texture2D>.Get("UI/Icons/HostilityResponse/Flee", true);

		// Token: 0x04003341 RID: 13121
		private static readonly Color IconColor = new Color(0.84f, 0.84f, 0.84f);
	}
}
