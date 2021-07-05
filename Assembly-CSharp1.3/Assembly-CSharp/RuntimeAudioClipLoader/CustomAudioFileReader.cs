using System;
using System.IO;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NVorbis.NAudioSupport;
using UnityEngine;

namespace RuntimeAudioClipLoader
{
	// Token: 0x0200000F RID: 15
	internal class CustomAudioFileReader : WaveStream, ISampleProvider
	{
		// Token: 0x06000045 RID: 69 RVA: 0x00003CEC File Offset: 0x00001EEC
		public CustomAudioFileReader(Stream stream, AudioFormat format)
		{
			this.lockObject = new object();
			this.CreateReaderStream(stream, format);
			this.sourceBytesPerSample = this.readerStream.WaveFormat.BitsPerSample / 8 * this.readerStream.WaveFormat.Channels;
			this.sampleChannel = new SampleChannel(this.readerStream, false);
			this.destBytesPerSample = 4 * this.sampleChannel.WaveFormat.Channels;
			this.length = this.SourceToDest(this.readerStream.Length);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003D7C File Offset: 0x00001F7C
		private void CreateReaderStream(Stream stream, AudioFormat format)
		{
			if (format == AudioFormat.wav)
			{
				this.readerStream = new WaveFileReader(stream);
				if (this.readerStream.WaveFormat.Encoding != WaveFormatEncoding.Pcm && this.readerStream.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
				{
					this.readerStream = WaveFormatConversionStream.CreatePcmStream(this.readerStream);
					this.readerStream = new BlockAlignReductionStream(this.readerStream);
					return;
				}
			}
			else
			{
				if (format == AudioFormat.mp3)
				{
					this.readerStream = new Mp3FileReader(stream);
					return;
				}
				if (format == AudioFormat.aiff)
				{
					this.readerStream = new AiffFileReader(stream);
					return;
				}
				if (format == AudioFormat.ogg)
				{
					this.readerStream = new VorbisWaveReader(stream);
					return;
				}
				Debug.LogWarning("Audio format " + format + " is not supported");
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00003E31 File Offset: 0x00002031
		public override WaveFormat WaveFormat
		{
			get
			{
				return this.sampleChannel.WaveFormat;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00003E3E File Offset: 0x0000203E
		public override long Length
		{
			get
			{
				return this.length;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00003E46 File Offset: 0x00002046
		// (set) Token: 0x0600004A RID: 74 RVA: 0x00003E5C File Offset: 0x0000205C
		public override long Position
		{
			get
			{
				return this.SourceToDest(this.readerStream.Position);
			}
			set
			{
				object obj = this.lockObject;
				lock (obj)
				{
					this.readerStream.Position = this.DestToSource(value);
				}
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003EA8 File Offset: 0x000020A8
		public override int Read(byte[] buffer, int offset, int count)
		{
			WaveBuffer waveBuffer = new WaveBuffer(buffer);
			int count2 = count / 4;
			return this.Read(waveBuffer.FloatBuffer, offset / 4, count2) * 4;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003ED4 File Offset: 0x000020D4
		public int Read(float[] buffer, int offset, int count)
		{
			object obj = this.lockObject;
			int result;
			lock (obj)
			{
				result = this.sampleChannel.Read(buffer, offset, count);
			}
			return result;
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00003F20 File Offset: 0x00002120
		// (set) Token: 0x0600004E RID: 78 RVA: 0x00003F2D File Offset: 0x0000212D
		public float Volume
		{
			get
			{
				return this.sampleChannel.Volume;
			}
			set
			{
				this.sampleChannel.Volume = value;
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003F3B File Offset: 0x0000213B
		private long SourceToDest(long sourceBytes)
		{
			return (long)this.destBytesPerSample * (sourceBytes / (long)this.sourceBytesPerSample);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003F4E File Offset: 0x0000214E
		private long DestToSource(long destBytes)
		{
			return (long)this.sourceBytesPerSample * (destBytes / (long)this.destBytesPerSample);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003F61 File Offset: 0x00002161
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.readerStream != null)
			{
				this.readerStream.Dispose();
				this.readerStream = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x04000024 RID: 36
		private WaveStream readerStream;

		// Token: 0x04000025 RID: 37
		private readonly SampleChannel sampleChannel;

		// Token: 0x04000026 RID: 38
		private readonly int destBytesPerSample;

		// Token: 0x04000027 RID: 39
		private readonly int sourceBytesPerSample;

		// Token: 0x04000028 RID: 40
		private readonly long length;

		// Token: 0x04000029 RID: 41
		private readonly object lockObject;
	}
}
