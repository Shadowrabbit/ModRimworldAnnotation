using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200130B RID: 4875
	public static class NeedsCardUtility
	{
		// Token: 0x0600753B RID: 30011 RVA: 0x0028371C File Offset: 0x0028191C
		public static Vector2 GetSize(Pawn pawn)
		{
			NeedsCardUtility.UpdateDisplayNeeds(pawn);
			if (pawn.needs.mood != null)
			{
				return NeedsCardUtility.FullSize;
			}
			return new Vector2(225f, (float)NeedsCardUtility.displayNeeds.Count * Mathf.Min(70f, NeedsCardUtility.FullSize.y / (float)NeedsCardUtility.displayNeeds.Count));
		}

		// Token: 0x0600753C RID: 30012 RVA: 0x00283778 File Offset: 0x00281978
		public static void DoNeedsMoodAndThoughts(Rect rect, Pawn pawn, ref Vector2 thoughtScrollPosition)
		{
			Rect rect2 = new Rect(rect.x, rect.y, 225f, rect.height);
			NeedsCardUtility.DoNeeds(rect2, pawn);
			if (pawn.needs.mood != null)
			{
				NeedsCardUtility.DoMoodAndThoughts(new Rect(rect2.xMax, rect.y, rect.width - rect2.width, rect.height), pawn, ref thoughtScrollPosition);
			}
		}

		// Token: 0x0600753D RID: 30013 RVA: 0x002837EC File Offset: 0x002819EC
		public static void DoNeeds(Rect rect, Pawn pawn)
		{
			NeedsCardUtility.UpdateDisplayNeeds(pawn);
			float num = 0f;
			for (int i = 0; i < NeedsCardUtility.displayNeeds.Count; i++)
			{
				Need need = NeedsCardUtility.displayNeeds[i];
				Rect rect2 = new Rect(rect.x, rect.y + num, rect.width, Mathf.Min(70f, rect.height / (float)NeedsCardUtility.displayNeeds.Count));
				if (!need.def.major)
				{
					if (i > 0 && NeedsCardUtility.displayNeeds[i - 1].def.major)
					{
						rect2.y += 10f;
					}
					rect2.width *= 0.73f;
					rect2.height = Mathf.Max(rect2.height * 0.666f, 30f);
				}
				need.DrawOnGUI(rect2, int.MaxValue, -1f, true, true, null);
				num = rect2.yMax;
			}
		}

		// Token: 0x0600753E RID: 30014 RVA: 0x002838F8 File Offset: 0x00281AF8
		private static void DoMoodAndThoughts(Rect rect, Pawn pawn, ref Vector2 thoughtScrollPosition)
		{
			GUI.BeginGroup(rect);
			Rect rect2 = new Rect(0f, 0f, rect.width * 0.8f, 70f);
			pawn.needs.mood.DrawOnGUI(rect2, int.MaxValue, -1f, true, true, null);
			NeedsCardUtility.DrawThoughtListing(new Rect(0f, 80f, rect.width, rect.height - 70f - 10f).ContractedBy(10f), pawn, ref thoughtScrollPosition);
			GUI.EndGroup();
		}

		// Token: 0x0600753F RID: 30015 RVA: 0x00283994 File Offset: 0x00281B94
		private static void UpdateDisplayNeeds(Pawn pawn)
		{
			NeedsCardUtility.displayNeeds.Clear();
			List<Need> allNeeds = pawn.needs.AllNeeds;
			for (int i = 0; i < allNeeds.Count; i++)
			{
				if (allNeeds[i].ShowOnNeedList)
				{
					NeedsCardUtility.displayNeeds.Add(allNeeds[i]);
				}
			}
			PawnNeedsUIUtility.SortInDisplayOrder(NeedsCardUtility.displayNeeds);
		}

		// Token: 0x06007540 RID: 30016 RVA: 0x002839F4 File Offset: 0x00281BF4
		private static void DrawThoughtListing(Rect listingRect, Pawn pawn, ref Vector2 thoughtScrollPosition)
		{
			if (Event.current.type == EventType.Layout)
			{
				return;
			}
			Text.Font = GameFont.Small;
			IdeoUIUtility.DrawExtraThoughtInfoFromIdeo(pawn, ref listingRect);
			PawnNeedsUIUtility.GetThoughtGroupsInDisplayOrder(pawn.needs.mood, NeedsCardUtility.thoughtGroupsPresent);
			float height = (float)NeedsCardUtility.thoughtGroupsPresent.Count * 24f;
			Widgets.BeginScrollView(listingRect, ref thoughtScrollPosition, new Rect(0f, 0f, listingRect.width - 16f, height), true);
			Text.Anchor = TextAnchor.MiddleLeft;
			float num = 0f;
			for (int i = 0; i < NeedsCardUtility.thoughtGroupsPresent.Count; i++)
			{
				if (NeedsCardUtility.DrawThoughtGroup(new Rect(0f, num, listingRect.width - 16f, 20f), NeedsCardUtility.thoughtGroupsPresent[i], pawn))
				{
					num += 24f;
				}
			}
			Widgets.EndScrollView();
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06007541 RID: 30017 RVA: 0x00283AD0 File Offset: 0x00281CD0
		private static bool DrawThoughtGroup(Rect rect, Thought group, Pawn pawn)
		{
			try
			{
				pawn.needs.mood.thoughts.GetMoodThoughts(group, NeedsCardUtility.thoughtGroup);
				Thought leadingThoughtInGroup = PawnNeedsUIUtility.GetLeadingThoughtInGroup(NeedsCardUtility.thoughtGroup);
				if (!leadingThoughtInGroup.VisibleInNeedsTab)
				{
					NeedsCardUtility.thoughtGroup.Clear();
					return false;
				}
				if (leadingThoughtInGroup != NeedsCardUtility.thoughtGroup[0])
				{
					NeedsCardUtility.thoughtGroup.Remove(leadingThoughtInGroup);
					NeedsCardUtility.thoughtGroup.Insert(0, leadingThoughtInGroup);
				}
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
				}
				if (Mouse.IsOver(rect))
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(leadingThoughtInGroup.Description);
					int durationTicks = group.DurationTicks;
					if (durationTicks > 5)
					{
						stringBuilder.AppendLine();
						stringBuilder.AppendLine();
						Thought_Memory thought_Memory = leadingThoughtInGroup as Thought_Memory;
						if (thought_Memory != null)
						{
							if (NeedsCardUtility.thoughtGroup.Count == 1)
							{
								stringBuilder.Append("ThoughtExpiresIn".Translate((durationTicks - thought_Memory.age).ToStringTicksToPeriod(true, false, true, true)));
							}
							else
							{
								int num = int.MaxValue;
								int num2 = int.MinValue;
								foreach (Thought thought in NeedsCardUtility.thoughtGroup)
								{
									Thought_Memory thought_Memory2 = (Thought_Memory)thought;
									num = Mathf.Min(num, thought_Memory2.age);
									num2 = Mathf.Max(num2, thought_Memory2.age);
								}
								stringBuilder.Append("ThoughtStartsExpiringIn".Translate((durationTicks - num2).ToStringTicksToPeriod(true, false, true, true)));
								stringBuilder.AppendLine();
								stringBuilder.Append("ThoughtFinishesExpiringIn".Translate((durationTicks - num).ToStringTicksToPeriod(true, false, true, true)));
							}
						}
					}
					if (NeedsCardUtility.thoughtGroup.Count > 1)
					{
						bool flag = false;
						for (int i = 1; i < NeedsCardUtility.thoughtGroup.Count; i++)
						{
							bool flag2 = false;
							for (int j = 0; j < i; j++)
							{
								if (NeedsCardUtility.thoughtGroup[i].LabelCap == NeedsCardUtility.thoughtGroup[j].LabelCap)
								{
									flag2 = true;
									break;
								}
							}
							if (!flag2)
							{
								if (!flag)
								{
									stringBuilder.AppendLine();
									stringBuilder.AppendLine();
									flag = true;
								}
								stringBuilder.AppendLine("+ " + NeedsCardUtility.thoughtGroup[i].LabelCap);
							}
						}
					}
					TooltipHandler.TipRegion(rect, new TipSignal(stringBuilder.ToString(), 7291));
				}
				Text.WordWrap = false;
				Text.Anchor = TextAnchor.MiddleLeft;
				Rect rect2 = new Rect(rect.x + 10f, rect.y, 225f, rect.height);
				rect2.yMin -= 3f;
				rect2.yMax += 3f;
				string text = leadingThoughtInGroup.LabelCap;
				if (NeedsCardUtility.thoughtGroup.Count > 1)
				{
					text = text + " x" + NeedsCardUtility.thoughtGroup.Count;
				}
				Widgets.Label(rect2, text);
				Text.Anchor = TextAnchor.MiddleCenter;
				float num3 = pawn.needs.mood.thoughts.MoodOffsetOfGroup(group);
				if (num3 == 0f)
				{
					GUI.color = NeedsCardUtility.NoEffectColor;
				}
				else if (num3 > 0f)
				{
					GUI.color = NeedsCardUtility.MoodColor;
				}
				else
				{
					GUI.color = NeedsCardUtility.MoodColorNegative;
				}
				Widgets.Label(new Rect(rect.x + 235f, rect.y, 32f, rect.height), num3.ToString("##0"));
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				Text.WordWrap = true;
				if (ModsConfig.IdeologyActive && leadingThoughtInGroup.sourcePrecept != null)
				{
					IdeoUIUtility.DoIdeoIcon(new Rect(rect.x + 235f + 32f + 10f, rect.y, 20f, 20f), leadingThoughtInGroup.sourcePrecept.ideo, false, null);
				}
			}
			catch (Exception ex)
			{
				Log.ErrorOnce(string.Concat(new object[]
				{
					"Exception in DrawThoughtGroup for ",
					group.def,
					" on ",
					pawn,
					": ",
					ex.ToString()
				}), 3452698);
			}
			return true;
		}

		// Token: 0x040040D2 RID: 16594
		private static List<Need> displayNeeds = new List<Need>();

		// Token: 0x040040D3 RID: 16595
		private static readonly Color MoodColor = new Color(0.1f, 1f, 0.1f);

		// Token: 0x040040D4 RID: 16596
		private static readonly Color MoodColorNegative = new Color(0.8f, 0.4f, 0.4f);

		// Token: 0x040040D5 RID: 16597
		private static readonly Color NoEffectColor = new Color(0.5f, 0.5f, 0.5f, 0.75f);

		// Token: 0x040040D6 RID: 16598
		private const float ThoughtHeight = 20f;

		// Token: 0x040040D7 RID: 16599
		private const float ThoughtSpacing = 4f;

		// Token: 0x040040D8 RID: 16600
		private const float ThoughtIntervalY = 24f;

		// Token: 0x040040D9 RID: 16601
		private const float MoodX = 235f;

		// Token: 0x040040DA RID: 16602
		private const float MoodNumberWidth = 32f;

		// Token: 0x040040DB RID: 16603
		private const float NeedsColumnWidth = 225f;

		// Token: 0x040040DC RID: 16604
		public static readonly Vector2 FullSize = new Vector2(580f, 520f);

		// Token: 0x040040DD RID: 16605
		private static List<Thought> thoughtGroupsPresent = new List<Thought>();

		// Token: 0x040040DE RID: 16606
		private static List<Thought> thoughtGroup = new List<Thought>();
	}
}
