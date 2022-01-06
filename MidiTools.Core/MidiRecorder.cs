using System;

namespace MidiTools.Core
{
	public class MidiRecorder : IDisposable
	{
		private bool disposed = false;

		private MidiListener midiListener;

		public MidiRecorder()
		{

		}

		#region interface IDisposable

		public void Dispose()
		{
			// Dispose of unmanaged resources.
			Dispose(true);
			// Suppress finalization.
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
			{
				return;
			}

			if (disposing)
			{
				// dispose managed state (managed objects)
				midiListener?.StopListening();
				midiListener?.Dispose();
				midiListener = null;
			}

			// free unmanaged resources (unmanaged objects) and override a finalizer below.
			// set large fields to null.

			disposed = true;
		}

		#endregion

		public void StartRecording()
		{

		}

		public void StopRecording()
		{

		}
	}
}
