using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000348 RID: 840
	public static class GraphicDatabaseHeadRecords
	{
		// Token: 0x060017FE RID: 6142 RVA: 0x0008ECA0 File Offset: 0x0008CEA0
		public static void Reset()
		{
			GraphicDatabaseHeadRecords.heads.Clear();
			GraphicDatabaseHeadRecords.skull = null;
			GraphicDatabaseHeadRecords.stump = null;
		}

		// Token: 0x060017FF RID: 6143 RVA: 0x0008ECB8 File Offset: 0x0008CEB8
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

		// Token: 0x06001800 RID: 6144 RVA: 0x0008ED78 File Offset: 0x0008CF78
		public static Graphic_Multi GetHeadNamed(string graphicPath, Color skinColor, bool skinColorOverriden)
		{
			GraphicDatabaseHeadRecords.BuildDatabaseIfNecessary();
			for (int i = 0; i < GraphicDatabaseHeadRecords.heads.Count; i++)
			{
				GraphicDatabaseHeadRecords.HeadGraphicRecord headGraphicRecord = GraphicDatabaseHeadRecords.heads[i];
				if (headGraphicRecord.graphicPath == graphicPath)
				{
					return headGraphicRecord.GetGraphic(skinColor, false, skinColorOverriden);
				}
			}
			Log.Message("Tried to get pawn head at path " + graphicPath + " that was not found. Defaulting...");
			return GraphicDatabaseHeadRecords.heads.First<GraphicDatabaseHeadRecords.HeadGraphicRecord>().GetGraphic(skinColor, false, skinColorOverriden);
		}

		// Token: 0x06001801 RID: 6145 RVA: 0x0008EDEA File Offset: 0x0008CFEA
		public static Graphic_Multi GetSkull()
		{
			GraphicDatabaseHeadRecords.BuildDatabaseIfNecessary();
			return GraphicDatabaseHeadRecords.skull.GetGraphic(Color.white, true, false);
		}

		// Token: 0x06001802 RID: 6146 RVA: 0x0008EE02 File Offset: 0x0008D002
		public static Graphic_Multi GetStump(Color skinColor)
		{
			GraphicDatabaseHeadRecords.BuildDatabaseIfNecessary();
			return GraphicDatabaseHeadRecords.stump.GetGraphic(skinColor, false, false);
		}

		// Token: 0x06001803 RID: 6147 RVA: 0x0008EE18 File Offset: 0x0008D018
		public static Graphic_Multi GetHeadRandom(Gender gender, Color skinColor, CrownType crownType, bool skinColorOverriden)
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
			return headGraphicRecord.GetGraphic(skinColor, false, false);
			Block_2:
			foreach (GraphicDatabaseHeadRecords.HeadGraphicRecord headGraphicRecord2 in GraphicDatabaseHeadRecords.heads.InRandomOrder(null))
			{
				if (predicate(headGraphicRecord2))
				{
					return headGraphicRecord2.GetGraphic(skinColor, false, false);
				}
			}
			Log.Error("Failed to find head for gender=" + gender + ". Defaulting...");
			return GraphicDatabaseHeadRecords.heads.First<GraphicDatabaseHeadRecords.HeadGraphicRecord>().GetGraphic(skinColor, false, false);
		}

		// Token: 0x04001076 RID: 4214
		private static List<GraphicDatabaseHeadRecords.HeadGraphicRecord> heads = new List<GraphicDatabaseHeadRecords.HeadGraphicRecord>();

		// Token: 0x04001077 RID: 4215
		private static GraphicDatabaseHeadRecords.HeadGraphicRecord skull;

		// Token: 0x04001078 RID: 4216
		private static GraphicDatabaseHeadRecords.HeadGraphicRecord stump;

		// Token: 0x04001079 RID: 4217
		private static readonly string[] HeadsFolderPaths = new string[]
		{
			"Things/Pawn/Humanlike/Heads/Male",
			"Things/Pawn/Humanlike/Heads/Female"
		};

		// Token: 0x0400107A RID: 4218
		private static readonly string SkullPath = "Things/Pawn/Humanlike/Heads/None_Average_Skull";

		// Token: 0x0400107B RID: 4219
		private static readonly string StumpPath = "Things/Pawn/Humanlike/Heads/None_Average_Stump";

		// Token: 0x02001A68 RID: 6760
		private class HeadGraphicRecord
		{
			// Token: 0x06009CCD RID: 40141 RVA: 0x0036A87C File Offset: 0x00368A7C
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
					Log.Error("Parse error with head graphic at " + graphicPath + ": " + ex.Message);
					this.crownType = CrownType.Undefined;
					this.gender = Gender.None;
				}
			}

			// Token: 0x06009CCE RID: 40142 RVA: 0x0036A91C File Offset: 0x00368B1C
			public Graphic_Multi GetGraphic(Color color, bool dessicated = false, bool skinColorOverriden = false)
			{
				Shader shader = (!dessicated) ? ShaderUtility.GetSkinShader(skinColorOverriden) : ShaderDatabase.Cutout;
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

			// Token: 0x0400650D RID: 25869
			public Gender gender;

			// Token: 0x0400650E RID: 25870
			public CrownType crownType;

			// Token: 0x0400650F RID: 25871
			public string graphicPath;

			// Token: 0x04006510 RID: 25872
			private List<KeyValuePair<Color, Graphic_Multi>> graphics = new List<KeyValuePair<Color, Graphic_Multi>>();
		}
	}
}
