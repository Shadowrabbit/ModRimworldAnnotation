using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02000668 RID: 1640
	public sealed class LordManager : IExposable
	{
		// Token: 0x06002E85 RID: 11909 RVA: 0x0011651D File Offset: 0x0011471D
		public LordManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06002E86 RID: 11910 RVA: 0x00116544 File Offset: 0x00114744
		public void ExposeData()
		{
			Scribe_Collections.Look<Lord>(ref this.lords, "lords", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<StencilDrawerForCells>(ref this.stencilDrawers, "stencilDrawers", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				for (int i = 0; i < this.lords.Count; i++)
				{
					this.lords[i].lordManager = this;
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.stencilDrawers == null)
				{
					this.stencilDrawers = new List<StencilDrawerForCells>();
				}
				for (int j = 0; j < this.lords.Count; j++)
				{
					Find.SignalManager.RegisterReceiver(this.lords[j]);
				}
			}
		}

		// Token: 0x06002E87 RID: 11911 RVA: 0x001165F4 File Offset: 0x001147F4
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
					Log.Error(string.Format("Exception while ticking lord with job {0}: \r\n{1}", (lord == null) ? "NULL" : lord.LordJob.ToString(), ex.ToString()));
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
			for (int k = this.stencilDrawers.Count - 1; k >= 0; k--)
			{
				if (!this.lords.Contains(this.stencilDrawers[k].sourceLord) && this.stencilDrawers[k].ticksLeftWithoutLord <= 0)
				{
					this.stencilDrawers.RemoveAt(k);
				}
			}
		}

		// Token: 0x06002E88 RID: 11912 RVA: 0x0011671C File Offset: 0x0011491C
		public void LordManagerUpdate()
		{
			if (DebugViewSettings.drawLords)
			{
				for (int i = 0; i < this.lords.Count; i++)
				{
					this.lords[i].DebugDraw();
				}
			}
			foreach (StencilDrawerForCells stencilDrawerForCells in this.stencilDrawers)
			{
				stencilDrawerForCells.Draw();
				if (!this.lords.Contains(stencilDrawerForCells.sourceLord))
				{
					stencilDrawerForCells.ticksLeftWithoutLord--;
				}
			}
		}

		// Token: 0x06002E89 RID: 11913 RVA: 0x001167C0 File Offset: 0x001149C0
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

		// Token: 0x06002E8A RID: 11914 RVA: 0x001168F8 File Offset: 0x00114AF8
		public void AddLord(Lord newLord)
		{
			this.lords.Add(newLord);
			newLord.lordManager = this;
			Find.SignalManager.RegisterReceiver(newLord);
		}

		// Token: 0x06002E8B RID: 11915 RVA: 0x00116918 File Offset: 0x00114B18
		public void RemoveLord(Lord oldLord)
		{
			this.lords.Remove(oldLord);
			Find.SignalManager.DeregisterReceiver(oldLord);
			oldLord.Cleanup();
		}

		// Token: 0x06002E8C RID: 11916 RVA: 0x00116938 File Offset: 0x00114B38
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

		// Token: 0x06002E8D RID: 11917 RVA: 0x00116990 File Offset: 0x00114B90
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

		// Token: 0x06002E8E RID: 11918 RVA: 0x001169E8 File Offset: 0x00114BE8
		public void Notify_BuildingSpawned(Building b)
		{
			for (int i = 0; i < this.lords.Count; i++)
			{
				this.lords[i].Notify_BuildingSpawnedOnMap(b);
			}
			BreachingGridDebug.Notify_BuildingStateChanged(b);
		}

		// Token: 0x06002E8F RID: 11919 RVA: 0x00116A24 File Offset: 0x00114C24
		public void Notify_BuildingDespawned(Building b)
		{
			for (int i = 0; i < this.lords.Count; i++)
			{
				this.lords[i].Notify_BuildingDespawnedOnMap(b);
			}
			BreachingGridDebug.Notify_BuildingStateChanged(b);
		}

		// Token: 0x06002E90 RID: 11920 RVA: 0x00116A60 File Offset: 0x00114C60
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
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04001C93 RID: 7315
		public Map map;

		// Token: 0x04001C94 RID: 7316
		public List<Lord> lords = new List<Lord>();

		// Token: 0x04001C95 RID: 7317
		public List<StencilDrawerForCells> stencilDrawers = new List<StencilDrawerForCells>();
	}
}
