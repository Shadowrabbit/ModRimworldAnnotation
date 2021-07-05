using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AF9 RID: 2809
	public class HistoryAutoRecorder : IExposable
	{
		// Token: 0x06004222 RID: 16930 RVA: 0x0016214C File Offset: 0x0016034C
		public void Tick()
		{
			if (Find.TickManager.TicksGame % this.def.recordTicksFrequency == 0 || !this.records.Any<float>())
			{
				float item = this.def.Worker.PullRecord();
				this.records.Add(item);
			}
		}

		// Token: 0x06004223 RID: 16931 RVA: 0x0016219C File Offset: 0x0016039C
		public void ExposeData()
		{
			Scribe_Defs.Look<HistoryAutoRecorderDef>(ref this.def, "def");
			byte[] recordsFromBytes = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				recordsFromBytes = this.RecordsToBytes();
			}
			DataExposeUtility.ByteArray(ref recordsFromBytes, "records");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.SetRecordsFromBytes(recordsFromBytes);
			}
		}

		// Token: 0x06004224 RID: 16932 RVA: 0x001621E8 File Offset: 0x001603E8
		private byte[] RecordsToBytes()
		{
			byte[] array = new byte[this.records.Count * 4];
			for (int i = 0; i < this.records.Count; i++)
			{
				byte[] bytes = BitConverter.GetBytes(this.records[i]);
				for (int j = 0; j < 4; j++)
				{
					array[i * 4 + j] = bytes[j];
				}
			}
			return array;
		}

		// Token: 0x06004225 RID: 16933 RVA: 0x00162248 File Offset: 0x00160448
		private void SetRecordsFromBytes(byte[] bytes)
		{
			int num = bytes.Length / 4;
			this.records.Clear();
			for (int i = 0; i < num; i++)
			{
				float item = BitConverter.ToSingle(bytes, i * 4);
				this.records.Add(item);
			}
		}

		// Token: 0x0400284C RID: 10316
		public HistoryAutoRecorderDef def;

		// Token: 0x0400284D RID: 10317
		public List<float> records = new List<float>();
	}
}
