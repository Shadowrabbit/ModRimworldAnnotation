using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B12 RID: 6930
	public static class ITab_Pawn_Log_Utility
	{
		// Token: 0x06009880 RID: 39040 RVA: 0x0006599A File Offset: 0x00063B9A
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

		// Token: 0x0400617E RID: 24958
		[TweakValue("Interface", 0f, 1f)]
		private static float AlternateAlpha = 0.03f;

		// Token: 0x0400617F RID: 24959
		[TweakValue("Interface", 0f, 1f)]
		private static float HighlightAlpha = 0.2f;

		// Token: 0x04006180 RID: 24960
		[TweakValue("Interface", 0f, 10f)]
		private static float HighlightDuration = 4f;

		// Token: 0x04006181 RID: 24961
		[TweakValue("Interface", 0f, 30f)]
		private static float BattleBottomPadding = 20f;

		// Token: 0x02001B13 RID: 6931
		public class LogDrawData
		{
			// Token: 0x06009882 RID: 39042 RVA: 0x000659E9 File Offset: 0x00063BE9
			public void StartNewDraw()
			{
				this.alternatingBackground = false;
			}

			// Token: 0x04006182 RID: 24962
			public bool alternatingBackground;

			// Token: 0x04006183 RID: 24963
			public LogEntry highlightEntry;

			// Token: 0x04006184 RID: 24964
			public float highlightIntensity;
		}

		// Token: 0x02001B14 RID: 6932
		public abstract class LogLineDisplayable
		{
			// Token: 0x06009884 RID: 39044 RVA: 0x000659F2 File Offset: 0x00063BF2
			public float GetHeight(float width)
			{
				if (this.cachedHeight == -1f)
				{
					this.cachedHeight = this.GetHeight_Worker(width);
				}
				return this.cachedHeight;
			}

			// Token: 0x06009885 RID: 39045
			public abstract float GetHeight_Worker(float width);

			// Token: 0x06009886 RID: 39046
			public abstract void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data);

			// Token: 0x06009887 RID: 39047
			public abstract void AppendTo(StringBuilder sb);

			// Token: 0x06009888 RID: 39048 RVA: 0x0000A2E4 File Offset: 0x000084E4
			public virtual bool Matches(LogEntry log)
			{
				return false;
			}

			// Token: 0x04006185 RID: 24965
			private float cachedHeight = -1f;
		}

		// Token: 0x02001B15 RID: 6933
		public class LogLineDisplayableHeader : ITab_Pawn_Log_Utility.LogLineDisplayable
		{
			// Token: 0x0600988A RID: 39050 RVA: 0x00065A27 File Offset: 0x00063C27
			public LogLineDisplayableHeader(TaggedString text)
			{
				this.text = text;
			}

			// Token: 0x0600988B RID: 39051 RVA: 0x002CDA40 File Offset: 0x002CBC40
			public override float GetHeight_Worker(float width)
			{
				GameFont font = Text.Font;
				Text.Font = GameFont.Medium;
				float result = Text.CalcHeight(this.text, width);
				Text.Font = font;
				return result;
			}

			// Token: 0x0600988C RID: 39052 RVA: 0x00065A36 File Offset: 0x00063C36
			public override void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data)
			{
				Text.Font = GameFont.Medium;
				Widgets.Label(new Rect(0f, position, width, base.GetHeight(width)), this.text);
				Text.Font = GameFont.Small;
			}

			// Token: 0x0600988D RID: 39053 RVA: 0x00065A62 File Offset: 0x00063C62
			public override void AppendTo(StringBuilder sb)
			{
				sb.AppendLine("--    " + this.text);
			}

			// Token: 0x04006186 RID: 24966
			private TaggedString text;
		}

		// Token: 0x02001B16 RID: 6934
		public class LogLineDisplayableLog : ITab_Pawn_Log_Utility.LogLineDisplayable
		{
			// Token: 0x0600988E RID: 39054 RVA: 0x00065A80 File Offset: 0x00063C80
			public LogLineDisplayableLog(LogEntry log, Pawn pawn)
			{
				this.log = log;
				this.pawn = pawn;
			}

			// Token: 0x0600988F RID: 39055 RVA: 0x002CDA70 File Offset: 0x002CBC70
			public override float GetHeight_Worker(float width)
			{
				float width2 = width - 29f;
				return Mathf.Max(26f, this.log.GetTextHeight(this.pawn, width2));
			}

			// Token: 0x06009890 RID: 39056 RVA: 0x002CDAA4 File Offset: 0x002CBCA4
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
					GUI.DrawTexture(new Rect(0f, position + (height - 26f) / 2f, 26f, 26f), texture2D);
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

			// Token: 0x06009891 RID: 39057 RVA: 0x00065A96 File Offset: 0x00063C96
			public override void AppendTo(StringBuilder sb)
			{
				sb.AppendLine(this.log.ToGameStringFromPOV(this.pawn, false));
			}

			// Token: 0x06009892 RID: 39058 RVA: 0x00065AB1 File Offset: 0x00063CB1
			public override bool Matches(LogEntry log)
			{
				return log == this.log;
			}

			// Token: 0x04006187 RID: 24967
			private LogEntry log;

			// Token: 0x04006188 RID: 24968
			private Pawn pawn;
		}

		// Token: 0x02001B17 RID: 6935
		public class LogLineDisplayableGap : ITab_Pawn_Log_Utility.LogLineDisplayable
		{
			// Token: 0x06009894 RID: 39060 RVA: 0x00065AC9 File Offset: 0x00063CC9
			public LogLineDisplayableGap(float height)
			{
				this.height = height;
			}

			// Token: 0x06009895 RID: 39061 RVA: 0x00065AD8 File Offset: 0x00063CD8
			public override float GetHeight_Worker(float width)
			{
				return this.height;
			}

			// Token: 0x06009896 RID: 39062 RVA: 0x00006A05 File Offset: 0x00004C05
			public override void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data)
			{
			}

			// Token: 0x06009897 RID: 39063 RVA: 0x00065AE0 File Offset: 0x00063CE0
			public override void AppendTo(StringBuilder sb)
			{
				sb.AppendLine();
			}

			// Token: 0x04006189 RID: 24969
			private float height;
		}
	}
}
