using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200134B RID: 4939
	public static class ITab_Pawn_Log_Utility
	{
		// Token: 0x0600779B RID: 30619 RVA: 0x002A237D File Offset: 0x002A057D
		public static IEnumerable<ITab_Pawn_Log_Utility.LogLineDisplayable> GenerateLogLinesFor(Pawn pawn, bool showAll, bool showCombat, bool showSocial)
		{
			LogEntry[] nonCombatLines = showSocial ? (from e in Find.PlayLog.AllEntries
			where e.Concerns(pawn)
			select e).ToArray<LogEntry>() : new LogEntry[0];
			int nonCombatIndex = 0;
			Battle currentBattle = null;
			if (showCombat)
			{
				bool atTop = true;
				foreach (Battle battle in Find.BattleLog.Battles)
				{
					if (battle.Concerns(pawn))
					{
						foreach (LogEntry entry in battle.Entries)
						{
							if (entry.Concerns(pawn))
							{
								if (showAll || entry.ShowInCompactView())
								{
									while (nonCombatIndex < nonCombatLines.Length && nonCombatLines[nonCombatIndex].Age < entry.Age)
									{
										if (currentBattle != null && currentBattle != battle)
										{
											yield return new ITab_Pawn_Log_Utility.LogLineDisplayableGap(ITab_Pawn_Log_Utility.BattleBottomPadding);
											currentBattle = null;
										}
										LogEntry[] array = nonCombatLines;
										int num = nonCombatIndex;
										nonCombatIndex = num + 1;
										yield return new ITab_Pawn_Log_Utility.LogLineDisplayableLog(array[num], pawn);
										atTop = false;
									}
									if (currentBattle != battle)
									{
										if (!atTop)
										{
											yield return new ITab_Pawn_Log_Utility.LogLineDisplayableGap(ITab_Pawn_Log_Utility.BattleBottomPadding);
										}
										yield return new ITab_Pawn_Log_Utility.LogLineDisplayableHeader(battle.GetName());
										currentBattle = battle;
										atTop = false;
									}
									yield return new ITab_Pawn_Log_Utility.LogLineDisplayableLog(entry, pawn);
									entry = null;
								}
							}
						}
						List<LogEntry>.Enumerator enumerator2 = default(List<LogEntry>.Enumerator);
						battle = null;
					}
				}
				List<Battle>.Enumerator enumerator = default(List<Battle>.Enumerator);
			}
			while (nonCombatIndex < nonCombatLines.Length)
			{
				if (currentBattle != null)
				{
					yield return new ITab_Pawn_Log_Utility.LogLineDisplayableGap(ITab_Pawn_Log_Utility.BattleBottomPadding);
					currentBattle = null;
				}
				LogEntry[] array2 = nonCombatLines;
				int num = nonCombatIndex;
				nonCombatIndex = num + 1;
				yield return new ITab_Pawn_Log_Utility.LogLineDisplayableLog(array2[num], pawn);
			}
			yield break;
			yield break;
		}

		// Token: 0x0400428A RID: 17034
		[TweakValue("Interface", 0f, 1f)]
		private static float AlternateAlpha = 0.03f;

		// Token: 0x0400428B RID: 17035
		[TweakValue("Interface", 0f, 1f)]
		private static float HighlightAlpha = 0.2f;

		// Token: 0x0400428C RID: 17036
		[TweakValue("Interface", 0f, 10f)]
		private static float HighlightDuration = 4f;

		// Token: 0x0400428D RID: 17037
		[TweakValue("Interface", 0f, 30f)]
		private static float BattleBottomPadding = 20f;

		// Token: 0x02002751 RID: 10065
		public class LogDrawData
		{
			// Token: 0x0600D931 RID: 55601 RVA: 0x00412440 File Offset: 0x00410640
			public void StartNewDraw()
			{
				this.alternatingBackground = false;
			}

			// Token: 0x040094EB RID: 38123
			public bool alternatingBackground;

			// Token: 0x040094EC RID: 38124
			public LogEntry highlightEntry;

			// Token: 0x040094ED RID: 38125
			public float highlightIntensity;
		}

		// Token: 0x02002752 RID: 10066
		public abstract class LogLineDisplayable
		{
			// Token: 0x0600D933 RID: 55603 RVA: 0x00412449 File Offset: 0x00410649
			public float GetHeight(float width)
			{
				if (this.cachedHeight == -1f)
				{
					this.cachedHeight = this.GetHeight_Worker(width);
				}
				return this.cachedHeight;
			}

			// Token: 0x0600D934 RID: 55604
			public abstract float GetHeight_Worker(float width);

			// Token: 0x0600D935 RID: 55605
			public abstract void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data);

			// Token: 0x0600D936 RID: 55606
			public abstract void AppendTo(StringBuilder sb);

			// Token: 0x0600D937 RID: 55607 RVA: 0x0001276E File Offset: 0x0001096E
			public virtual bool Matches(LogEntry log)
			{
				return false;
			}

			// Token: 0x040094EE RID: 38126
			private float cachedHeight = -1f;
		}

		// Token: 0x02002753 RID: 10067
		public class LogLineDisplayableHeader : ITab_Pawn_Log_Utility.LogLineDisplayable
		{
			// Token: 0x0600D939 RID: 55609 RVA: 0x0041247E File Offset: 0x0041067E
			public LogLineDisplayableHeader(TaggedString text)
			{
				this.text = text;
			}

			// Token: 0x0600D93A RID: 55610 RVA: 0x00412490 File Offset: 0x00410690
			public override float GetHeight_Worker(float width)
			{
				GameFont font = Text.Font;
				Text.Font = GameFont.Medium;
				float result = Text.CalcHeight(this.text, width);
				Text.Font = font;
				return result;
			}

			// Token: 0x0600D93B RID: 55611 RVA: 0x004124C0 File Offset: 0x004106C0
			public override void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data)
			{
				Text.Font = GameFont.Medium;
				Widgets.Label(new Rect(0f, position, width, base.GetHeight(width)), this.text);
				Text.Font = GameFont.Small;
			}

			// Token: 0x0600D93C RID: 55612 RVA: 0x004124EC File Offset: 0x004106EC
			public override void AppendTo(StringBuilder sb)
			{
				sb.AppendLine("--    " + this.text);
			}

			// Token: 0x040094EF RID: 38127
			private TaggedString text;
		}

		// Token: 0x02002754 RID: 10068
		public class LogLineDisplayableLog : ITab_Pawn_Log_Utility.LogLineDisplayable
		{
			// Token: 0x0600D93D RID: 55613 RVA: 0x0041250A File Offset: 0x0041070A
			public LogLineDisplayableLog(LogEntry log, Pawn pawn)
			{
				this.log = log;
				this.pawn = pawn;
			}

			// Token: 0x0600D93E RID: 55614 RVA: 0x00412520 File Offset: 0x00410720
			public override float GetHeight_Worker(float width)
			{
				float width2 = width - 29f;
				return Mathf.Max(26f, this.log.GetTextHeight(this.pawn, width2));
			}

			// Token: 0x0600D93F RID: 55615 RVA: 0x00412554 File Offset: 0x00410754
			public override void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data)
			{
				float height = base.GetHeight(width);
				float width2 = width - 29f;
				Rect rect = new Rect(0f, position, width, height);
				if (this.log == data.highlightEntry)
				{
					Widgets.DrawRectFast(rect, new Color(1f, 1f, 1f, ITab_Pawn_Log_Utility.HighlightAlpha * data.highlightIntensity), null);
					data.highlightIntensity = Mathf.Max(0f, data.highlightIntensity - Time.deltaTime / ITab_Pawn_Log_Utility.HighlightDuration);
				}
				else if (data.alternatingBackground)
				{
					Widgets.DrawRectFast(rect, new Color(1f, 1f, 1f, ITab_Pawn_Log_Utility.AlternateAlpha), null);
				}
				data.alternatingBackground = !data.alternatingBackground;
				TaggedString label = this.log.ToGameStringFromPOV(this.pawn, false);
				Widgets.Label(new Rect(29f, position, width2, height), label);
				Texture2D texture2D = this.log.IconFromPOV(this.pawn);
				if (texture2D != null)
				{
					GUI.color = (this.log.IconColorFromPOV(this.pawn) ?? Color.white);
					GUI.DrawTexture(new Rect(0f, position + (height - 26f) / 2f, 26f, 26f), texture2D);
					GUI.color = Color.white;
				}
				if (Mouse.IsOver(rect))
				{
					TooltipHandler.TipRegion(rect, () => this.log.GetTipString(), 613261 + this.log.LogID * 2063);
					Widgets.DrawHighlight(rect);
				}
				if (Widgets.ButtonInvisible(rect, this.log.CanBeClickedFromPOV(this.pawn)))
				{
					this.log.ClickedFromPOV(this.pawn);
				}
				if (DebugViewSettings.logCombatLogMouseover && Mouse.IsOver(rect))
				{
					this.log.ToGameStringFromPOV(this.pawn, true);
				}
			}

			// Token: 0x0600D940 RID: 55616 RVA: 0x0041273E File Offset: 0x0041093E
			public override void AppendTo(StringBuilder sb)
			{
				sb.AppendLine(this.log.ToGameStringFromPOV(this.pawn, false));
			}

			// Token: 0x0600D941 RID: 55617 RVA: 0x00412759 File Offset: 0x00410959
			public override bool Matches(LogEntry log)
			{
				return log == this.log;
			}

			// Token: 0x040094F0 RID: 38128
			private LogEntry log;

			// Token: 0x040094F1 RID: 38129
			private Pawn pawn;
		}

		// Token: 0x02002755 RID: 10069
		public class LogLineDisplayableGap : ITab_Pawn_Log_Utility.LogLineDisplayable
		{
			// Token: 0x0600D943 RID: 55619 RVA: 0x00412771 File Offset: 0x00410971
			public LogLineDisplayableGap(float height)
			{
				this.height = height;
			}

			// Token: 0x0600D944 RID: 55620 RVA: 0x00412780 File Offset: 0x00410980
			public override float GetHeight_Worker(float width)
			{
				return this.height;
			}

			// Token: 0x0600D945 RID: 55621 RVA: 0x0000313F File Offset: 0x0000133F
			public override void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data)
			{
			}

			// Token: 0x0600D946 RID: 55622 RVA: 0x00412788 File Offset: 0x00410988
			public override void AppendTo(StringBuilder sb)
			{
				sb.AppendLine();
			}

			// Token: 0x040094F2 RID: 38130
			private float height;
		}
	}
}
