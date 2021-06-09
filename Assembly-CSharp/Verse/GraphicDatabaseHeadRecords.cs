using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004EC RID: 1260
	public static class GraphicDatabaseHeadRecords
	{
		// Token: 0x06001F70 RID: 8048 RVA: 0x0001BB11 File Offset: 0x00019D11
		public static void Reset()
		{
			GraphicDatabaseHeadRecords.heads.Clear();
			GraphicDatabaseHeadRecords.skull = null;
			GraphicDatabaseHeadRecords.stump = null;
		}

		// Token: 0x06001F71 RID: 8049 RVA: 0x000FFF1C File Offset: 0x000FE11C
		private static void BuildDatabaseIfNecessary()
		{
			if (GraphicDatabaseHeadRecords.heads.Count > 0 && GraphicDatabaseHeadRecords.skull != null && GraphicDatabaseHeadRecords.stump != null)
			{
				return;
			}
			GraphicDatabaseHeadRecords.heads.Clear();
			foreach (string text in GraphicDatabaseHeadRecords.HeadsFolderPaths)
			{
				foreach (string str in GraphicDatabaseUtility.GraphicNamesInFolder(text))
				{
					GraphicDatabaseHeadRecords.heads.Add(new GraphicDatabaseHeadRecords.HeadGraphicRecord(text + "/" + str));
				}
			}
			GraphicDatabaseHeadRecords.skull = new GraphicDatabaseHeadRecords.HeadGraphicRecord(GraphicDatabaseHeadRecords.SkullPath);
			GraphicDatabaseHeadRecords.stump = new GraphicDatabaseHeadRecords.HeadGraphicRecord(GraphicDatabaseHeadRecords.StumpPath);
		}

		// Token: 0x06001F72 RID: 8050 RVA: 0x000FFFDC File Offset: 0x000FE1DC
		public static Graphic_Multi GetHeadNamed(string graphicPath, Color skinColor)
		{
			GraphicDatabaseHeadRecords.BuildDatabaseIfNecessary();
			for (int i = 0; i < GraphicDatabaseHeadRecords.heads.Count; i++)
			{
				GraphicDatabaseHeadRecords.HeadGraphicRecord headGraphicRecord = GraphicDatabaseHeadRecords.heads[i];
				if (headGraphicRecord.graphicPath == graphicPath)
				{
					return headGraphicRecord.GetGraphic(skinColor, false);
				}
			}
			Log.Message("Tried to get pawn head at path " + graphicPath + " that was not found. Defaulting...", false);
			return GraphicDatabaseHeadRecords.heads.First<GraphicDatabaseHeadRecords.HeadGraphicRecord>().GetGraphic(skinColor, false);
		}

		// Token: 0x06001F73 RID: 8051 RVA: 0x0001BB29 File Offset: 0x00019D29
		public static Graphic_Multi GetSkull()
		{
			GraphicDatabaseHeadRecords.BuildDatabaseIfNecessary();
			return GraphicDatabaseHeadRecords.skull.GetGraphic(Color.white, true);
		}

		// Token: 0x06001F74 RID: 8052 RVA: 0x0001BB40 File Offset: 0x00019D40
		public static Graphic_Multi GetStump(Color skinColor)
		{
			GraphicDatabaseHeadRecords.BuildDatabaseIfNecessary();
			return GraphicDatabaseHeadRecords.stump.GetGraphic(skinColor, false);
		}

		// Token: 0x06001F75 RID: 8053 RVA: 0x00100050 File Offset: 0x000FE250
		public static Graphic_Multi GetHeadRandom(Gender gender, Color skinColor, CrownType crownType)
		{
			GraphicDatabaseHeadRecords.BuildDatabaseIfNecessary();
			Predicate<GraphicDatabaseHeadRecords.HeadGraphicRecord> predicate = (GraphicDatabaseHeadRecords.HeadGraphicRecord head) => head.crownType == crownType && head.gender == gender;
			int num = 0;
			GraphicDatabaseHeadRecords.HeadGraphicRecord headGraphicRecord;
			for (;;)
			{
				headGraphicRecord = GraphicDatabaseHeadRecords.heads.RandomElement<GraphicDatabaseHeadRecords.HeadGraphicRecord>();
				if (predicate(headGraphicRecord))
				{
					break;
				}
				num++;
				if (num > 40)
				{
					goto Block_2;
				}
			}
			return headGraphicRecord.GetGraphic(skinColor, false);
			Block_2:
			foreach (GraphicDatabaseHeadRecords.HeadGraphicRecord headGraphicRecord2 in GraphicDatabaseHeadRecords.heads.InRandomOrder(null))
			{
				if (predicate(headGraphicRecord2))
				{
					return headGraphicRecord2.GetGraphic(skinColor, false);
				}
			}
			Log.Error("Failed to find head for gender=" + gender + ". Defaulting...", false);
			return GraphicDatabaseHeadRecords.heads.First<GraphicDatabaseHeadRecords.HeadGraphicRecord>().GetGraphic(skinColor, false);
		}

		// Token: 0x04001610 RID: 5648
		private static List<GraphicDatabaseHeadRecords.HeadGraphicRecord> heads = new List<GraphicDatabaseHeadRecords.HeadGraphicRecord>();

		// Token: 0x04001611 RID: 5649
		private static GraphicDatabaseHeadRecords.HeadGraphicRecord skull;

		// Token: 0x04001612 RID: 5650
		private static GraphicDatabaseHeadRecords.HeadGraphicRecord stump;

		// Token: 0x04001613 RID: 5651
		private static readonly string[] HeadsFolderPaths = new string[]
		{
			"Things/Pawn/Humanlike/Heads/Male",
			"Things/Pawn/Humanlike/Heads/Female"
		};

		// Token: 0x04001614 RID: 5652
		private static readonly string SkullPath = "Things/Pawn/Humanlike/Heads/None_Average_Skull";

		// Token: 0x04001615 RID: 5653
		private static readonly string StumpPath = "Things/Pawn/Humanlike/Heads/None_Average_Stump";

		// Token: 0x020004ED RID: 1261
		private class HeadGraphicRecord
		{
			// Token: 0x06001F77 RID: 8055 RVA: 0x0010013C File Offset: 0x000FE33C
			public HeadGraphicRecord(string graphicPath)
			{
				this.graphicPath = graphicPath;
				string[] array = Path.GetFileNameWithoutExtension(graphicPath).Split(new char[]
				{
					'_'
				});
				try
				{
					this.crownType = ParseHelper.FromString<CrownType>(array[array.Length - 2]);
					this.gender = ParseHelper.FromString<Gender>(array[array.Length - 3]);
				}
				catch (Exception ex)
				{
					Log.Error("Parse error with head graphic at " + graphicPath + ": " + ex.Message, false);
					this.crownType = CrownType.Undefined;
					this.gender = Gender.None;
				}
			}

			// Token: 0x06001F78 RID: 8056 RVA: 0x001001DC File Offset: 0x000FE3DC
			public Graphic_Multi GetGraphic(Color color, bool dessicated = false)
			{
				Shader shader = (!dessicated) ? ShaderDatabase.CutoutSkin : ShaderDatabase.Cutout;
				for (int i = 0; i < this.graphics.Count; i++)
				{
					if (color.IndistinguishableFrom(this.graphics[i].Key) && this.graphics[i].Value.Shader == shader)
					{
						return this.graphics[i].Value;
					}
				}
				Graphic_Multi graphic_Multi = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>(this.graphicPath, shader, Vector2.one, color);
				this.graphics.Add(new KeyValuePair<Color, Graphic_Multi>(color, graphic_Multi));
				return graphic_Multi;
			}

			// Token: 0x04001616 RID: 5654
			public Gender gender;

			// Token: 0x04001617 RID: 5655
			public CrownType crownType;

			// Token: 0x04001618 RID: 5656
			public string graphicPath;

			// Token: 0x04001619 RID: 5657
			private List<KeyValuePair<Color, Graphic_Multi>> graphics = new List<KeyValuePair<Color, Graphic_Multi>>();
		}
	}
}
