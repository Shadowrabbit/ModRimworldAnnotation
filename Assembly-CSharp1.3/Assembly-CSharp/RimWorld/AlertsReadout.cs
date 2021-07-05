using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001292 RID: 4754
	public class AlertsReadout
	{
		// Token: 0x06007180 RID: 29056 RVA: 0x0025D89C File Offset: 0x0025BA9C
		public AlertsReadout()
		{
			this.AllAlerts.Clear();
			foreach (Type type in typeof(Alert).AllLeafSubclasses())
			{
				if (!typeof(Alert_Custom).IsAssignableFrom(type) && !typeof(Alert_CustomCritical).IsAssignableFrom(type))
				{
					this.AllAlerts.Add((Alert)Activator.CreateInstance(type));
				}
			}
			if (this.PriosInDrawOrder == null)
			{
				this.PriosInDrawOrder = new List<AlertPriority>();
				foreach (object obj in Enum.GetValues(typeof(AlertPriority)))
				{
					AlertPriority item = (AlertPriority)obj;
					this.PriosInDrawOrder.Add(item);
				}
				this.PriosInDrawOrder.Reverse();
			}
		}

		// Token: 0x06007181 RID: 29057 RVA: 0x0025D9E4 File Offset: 0x0025BBE4
		public void AlertsReadoutUpdate()
		{
			if (Mathf.Max(Find.TickManager.TicksGame, Find.TutorialState.endTick) < 600)
			{
				return;
			}
			if (Find.Storyteller.def.disableAlerts)
			{
				this.activeAlerts.Clear();
				return;
			}
			this.curAlertIndex++;
			if (this.curAlertIndex >= 24)
			{
				this.curAlertIndex = 0;
			}
			for (int i = this.curAlertIndex; i < this.AllAlerts.Count; i += 24)
			{
				this.CheckAddOrRemoveAlert(this.AllAlerts[i], false);
			}
			if (Time.frameCount % 20 == 0)
			{
				List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
				for (int j = 0; j < questsListForReading.Count; j++)
				{
					List<QuestPart> partsListForReading = questsListForReading[j].PartsListForReading;
					for (int k = 0; k < partsListForReading.Count; k++)
					{
						QuestPartActivable questPartActivable = partsListForReading[k] as QuestPartActivable;
						if (questPartActivable != null)
						{
							Alert cachedAlert = questPartActivable.CachedAlert;
							if (cachedAlert != null)
							{
								bool flag = questsListForReading[j].State != QuestState.Ongoing || questPartActivable.State != QuestPartState.Enabled;
								bool alertDirty = questPartActivable.AlertDirty;
								this.CheckAddOrRemoveAlert(cachedAlert, flag || alertDirty);
								if (alertDirty)
								{
									questPartActivable.ClearCachedAlert();
								}
							}
						}
					}
				}
			}
			if (ModsConfig.IdeologyActive && Time.frameCount % 20 == 0)
			{
				foreach (List<Alert> list in this.activePreceptAlerts.Values)
				{
					list.Clear();
				}
				foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
				{
					foreach (Precept precept in ideo.PreceptsListForReading)
					{
						List<Alert> list2;
						if (!this.activePreceptAlerts.TryGetValue(precept, out list2))
						{
							list2 = new List<Alert>();
							this.activePreceptAlerts[precept] = list2;
						}
						foreach (Alert alert in precept.GetAlerts())
						{
							this.CheckAddOrRemoveAlert(alert, false);
							list2.Add(alert);
						}
					}
				}
				for (int l = 0; l < this.activeAlerts.Count; l++)
				{
					Alert_Precept alert_Precept;
					if ((alert_Precept = (this.activeAlerts[l] as Alert_Precept)) != null)
					{
						Precept sourcePrecept = alert_Precept.sourcePrecept;
						this.CheckAddOrRemoveAlert(this.activeAlerts[l], sourcePrecept != null && (!this.activePreceptAlerts.ContainsKey(sourcePrecept) || !this.activePreceptAlerts[sourcePrecept].Contains(alert_Precept)));
					}
				}
			}
			if (Time.frameCount % 20 == 0)
			{
				this.activeSignalActionAlerts.Clear();
				List<Map> maps = Find.Maps;
				for (int m = 0; m < maps.Count; m++)
				{
					List<Thing> list3 = maps[m].listerThings.ThingsInGroup(ThingRequestGroup.ActionDelay);
					for (int n = 0; n < list3.Count; n++)
					{
						SignalAction_Delay signalAction_Delay = list3[n] as SignalAction_Delay;
						if (signalAction_Delay.Activated && signalAction_Delay.Alert != null)
						{
							this.activeSignalActionAlerts.Add(signalAction_Delay.Alert);
						}
					}
				}
				for (int num = 0; num < this.activeAlerts.Count; num++)
				{
					Alert_ActionDelay alert_ActionDelay;
					if ((alert_ActionDelay = (this.activeAlerts[num] as Alert_ActionDelay)) != null)
					{
						this.CheckAddOrRemoveAlert(alert_ActionDelay, !this.activeSignalActionAlerts.Contains(alert_ActionDelay));
					}
				}
				for (int num2 = 0; num2 < this.activeSignalActionAlerts.Count; num2++)
				{
					this.CheckAddOrRemoveAlert(this.activeSignalActionAlerts[num2], false);
				}
			}
			for (int num3 = this.activeAlerts.Count - 1; num3 >= 0; num3--)
			{
				Alert alert2 = this.activeAlerts[num3];
				try
				{
					this.activeAlerts[num3].AlertActiveUpdate();
				}
				catch (Exception ex)
				{
					Log.ErrorOnce("Exception updating alert " + alert2.ToString() + ": " + ex.ToString(), 743575);
					this.activeAlerts.RemoveAt(num3);
				}
			}
			if (this.mouseoverAlertIndex >= 0 && this.mouseoverAlertIndex < this.activeAlerts.Count)
			{
				IEnumerable<GlobalTargetInfo> allCulprits = this.activeAlerts[this.mouseoverAlertIndex].GetReport().AllCulprits;
				if (allCulprits != null)
				{
					foreach (GlobalTargetInfo target in allCulprits)
					{
						TargetHighlighter.Highlight(target, true, true, false);
					}
				}
			}
			this.mouseoverAlertIndex = -1;
		}

		// Token: 0x06007182 RID: 29058 RVA: 0x0025DF1C File Offset: 0x0025C11C
		private void CheckAddOrRemoveAlert(Alert alert, bool forceRemove = false)
		{
			try
			{
				alert.Recalculate();
				if (!forceRemove && alert.Active)
				{
					if (!this.activeAlerts.Contains(alert))
					{
						this.activeAlerts.Add(alert);
						alert.Notify_Started();
					}
				}
				else
				{
					this.activeAlerts.Remove(alert);
				}
			}
			catch (Exception ex)
			{
				Log.ErrorOnce("Exception processing alert " + alert.ToString() + ": " + ex.ToString(), 743575);
				this.activeAlerts.Remove(alert);
			}
		}

		// Token: 0x06007183 RID: 29059 RVA: 0x0025DFB0 File Offset: 0x0025C1B0
		public void AlertsReadoutOnGUI()
		{
			if (Event.current.type == EventType.Layout || Event.current.type == EventType.MouseDrag)
			{
				return;
			}
			if (this.activeAlerts.Count == 0)
			{
				return;
			}
			Alert alert = null;
			AlertPriority alertPriority = AlertPriority.Critical;
			bool flag = false;
			float num = 0f;
			for (int i = 0; i < this.activeAlerts.Count; i++)
			{
				num += this.activeAlerts[i].Height;
			}
			float num2 = Find.LetterStack.LastTopY - num;
			Rect rect = new Rect((float)UI.screenWidth - 154f, num2, 154f, this.lastFinalY - num2);
			float num3 = GenUI.BackgroundDarkAlphaForText();
			if (num3 > 0.001f)
			{
				GUI.color = new Color(1f, 1f, 1f, num3);
				Widgets.DrawShadowAround(rect);
				GUI.color = Color.white;
			}
			float num4 = num2;
			if (num4 < 0f)
			{
				num4 = 0f;
			}
			for (int j = 0; j < this.PriosInDrawOrder.Count; j++)
			{
				AlertPriority alertPriority2 = this.PriosInDrawOrder[j];
				for (int k = 0; k < this.activeAlerts.Count; k++)
				{
					Alert alert2 = this.activeAlerts[k];
					if (alert2.Priority == alertPriority2)
					{
						if (!flag)
						{
							alertPriority = alertPriority2;
							flag = true;
						}
						Rect rect2 = alert2.DrawAt(num4, alertPriority2 != alertPriority);
						if (Mouse.IsOver(rect2))
						{
							alert = alert2;
							this.mouseoverAlertIndex = k;
						}
						num4 += rect2.height;
					}
				}
			}
			this.lastFinalY = num4;
			UIHighlighter.HighlightOpportunity(rect, "Alerts");
			if (alert != null)
			{
				alert.DrawInfoPane();
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Alerts, KnowledgeAmount.FrameDisplayed);
				this.CheckAddOrRemoveAlert(alert, false);
			}
		}

		// Token: 0x04003E66 RID: 15974
		private List<Alert> activeAlerts = new List<Alert>(16);

		// Token: 0x04003E67 RID: 15975
		private int curAlertIndex;

		// Token: 0x04003E68 RID: 15976
		private float lastFinalY;

		// Token: 0x04003E69 RID: 15977
		private int mouseoverAlertIndex = -1;

		// Token: 0x04003E6A RID: 15978
		private readonly List<Alert> AllAlerts = new List<Alert>();

		// Token: 0x04003E6B RID: 15979
		private const int StartTickDelay = 600;

		// Token: 0x04003E6C RID: 15980
		public const float AlertListWidth = 164f;

		// Token: 0x04003E6D RID: 15981
		private const int AlertCycleLength = 24;

		// Token: 0x04003E6E RID: 15982
		private const int UpdateAlertsFromQuestsIntervalFrames = 20;

		// Token: 0x04003E6F RID: 15983
		private const int UpdateAlertsFromPreceptsIntervalFrames = 20;

		// Token: 0x04003E70 RID: 15984
		private const int UpdateAlertsFromSignalActionsIntervalFrames = 20;

		// Token: 0x04003E71 RID: 15985
		private readonly List<AlertPriority> PriosInDrawOrder;

		// Token: 0x04003E72 RID: 15986
		private Dictionary<Precept, List<Alert>> activePreceptAlerts = new Dictionary<Precept, List<Alert>>();

		// Token: 0x04003E73 RID: 15987
		private List<Alert> activeSignalActionAlerts = new List<Alert>();
	}
}
