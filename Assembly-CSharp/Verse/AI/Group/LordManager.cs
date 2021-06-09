using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02000ACD RID: 2765
	public sealed class LordManager : IExposable
	{
		// Token: 0x06004168 RID: 16744 RVA: 0x00030C67 File Offset: 0x0002EE67
		public LordManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004169 RID: 16745 RVA: 0x00187884 File Offset: 0x00185A84
		public void ExposeData()
		{
			Scribe_Collections.Look<Lord>(ref this.lords, "lords", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				for (int i = 0; i < this.lords.Count; i++)
				{
					this.lords[i].lordManager = this;
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int j = 0; j < this.lords.Count; j++)
				{
					Find.SignalManager.RegisterReceiver(this.lords[j]);
				}
			}
		}

		// Token: 0x0600416A RID: 16746 RVA: 0x0018790C File Offset: 0x00185B0C
		public void LordManagerTick()
		{
			for (int i = 0; i < this.lords.Count; i++)
			{
				try
				{
					this.lords[i].LordTick();
				}
				catch (Exception ex)
				{
					Lord lord = this.lords[i];
					Log.Error(string.Format("Exception while ticking lord with job {0}: \r\n{1}", (lord == null) ? "NULL" : lord.LordJob.ToString(), ex.ToString()), false);
				}
			}
			for (int j = this.lords.Count - 1; j >= 0; j--)
			{
				LordToil curLordToil = this.lords[j].CurLordToil;
				if (curLordToil == null || curLordToil.ShouldFail)
				{
					this.RemoveLord(this.lords[j]);
				}
			}
		}

		// Token: 0x0600416B RID: 16747 RVA: 0x001879D8 File Offset: 0x00185BD8
		public void LordManagerUpdate()
		{
			if (DebugViewSettings.drawLords)
			{
				for (int i = 0; i < this.lords.Count; i++)
				{
					this.lords[i].DebugDraw();
				}
			}
		}

		// Token: 0x0600416C RID: 16748 RVA: 0x00187A14 File Offset: 0x00185C14
		public void LordManagerOnGUI()
		{
			if (DebugViewSettings.drawLords)
			{
				for (int i = 0; i < this.lords.Count; i++)
				{
					this.lords[i].DebugOnGUI();
				}
			}
			if (DebugViewSettings.drawDuties)
			{
				Text.Anchor = TextAnchor.MiddleCenter;
				Text.Font = GameFont.Tiny;
				foreach (Pawn pawn in this.map.mapPawns.AllPawns)
				{
					if (pawn.Spawned)
					{
						string text = "";
						if (!pawn.Dead && pawn.mindState.duty != null)
						{
							text = pawn.mindState.duty.ToString();
						}
						if (pawn.InMentalState)
						{
							text = text + "\nMentalState=" + pawn.MentalState.ToString();
						}
						Vector2 vector = pawn.DrawPos.MapToUIPosition();
						Widgets.Label(new Rect(vector.x - 100f, vector.y - 100f, 200f, 200f), text);
					}
				}
				Text.Anchor = TextAnchor.UpperLeft;
			}
		}

		// Token: 0x0600416D RID: 16749 RVA: 0x00030C81 File Offset: 0x0002EE81
		public void AddLord(Lord newLord)
		{
			this.lords.Add(newLord);
			newLord.lordManager = this;
			Find.SignalManager.RegisterReceiver(newLord);
		}

		// Token: 0x0600416E RID: 16750 RVA: 0x00030CA1 File Offset: 0x0002EEA1
		public void RemoveLord(Lord oldLord)
		{
			this.lords.Remove(oldLord);
			Find.SignalManager.DeregisterReceiver(oldLord);
			oldLord.Cleanup();
		}

		// Token: 0x0600416F RID: 16751 RVA: 0x00187B4C File Offset: 0x00185D4C
		public Lord LordOf(Pawn p)
		{
			for (int i = 0; i < this.lords.Count; i++)
			{
				Lord lord = this.lords[i];
				for (int j = 0; j < lord.ownedPawns.Count; j++)
				{
					if (lord.ownedPawns[j] == p)
					{
						return lord;
					}
				}
			}
			return null;
		}

		// Token: 0x06004170 RID: 16752 RVA: 0x00187BA4 File Offset: 0x00185DA4
		public Lord LordOf(Building b)
		{
			for (int i = 0; i < this.lords.Count; i++)
			{
				Lord lord = this.lords[i];
				for (int j = 0; j < lord.ownedBuildings.Count; j++)
				{
					if (lord.ownedBuildings[j] == b)
					{
						return lord;
					}
				}
			}
			return null;
		}

		// Token: 0x06004171 RID: 16753 RVA: 0x00187BFC File Offset: 0x00185DFC
		public void LogLords()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("======= Lords =======");
			stringBuilder.AppendLine("Count: " + this.lords.Count);
			for (int i = 0; i < this.lords.Count; i++)
			{
				Lord lord = this.lords[i];
				stringBuilder.AppendLine();
				stringBuilder.Append("#" + (i + 1) + ": ");
				if (lord.LordJob == null)
				{
					stringBuilder.AppendLine("no-job");
				}
				else
				{
					stringBuilder.AppendLine(lord.LordJob.GetType().Name);
				}
				stringBuilder.Append("Current toil: ");
				if (lord.CurLordToil == null)
				{
					stringBuilder.AppendLine("null");
				}
				else
				{
					stringBuilder.AppendLine(lord.CurLordToil.GetType().Name);
				}
				stringBuilder.AppendLine("Members (count: " + lord.ownedPawns.Count + "):");
				for (int j = 0; j < lord.ownedPawns.Count; j++)
				{
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						"  ",
						lord.ownedPawns[j].LabelShort,
						" (",
						lord.ownedPawns[j].Faction,
						")"
					}));
				}
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x04002D23 RID: 11555
		public Map map;

		// Token: 0x04002D24 RID: 11556
		public List<Lord> lords = new List<Lord>();
	}
}
