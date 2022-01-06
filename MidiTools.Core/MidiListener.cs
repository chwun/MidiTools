using System;
using System.Diagnostics;
using NAudio.Midi;

namespace MidiTools.Core
{
	public class MidiListener : IDisposable
	{
		private bool disposed = false;

		private int deviceIndex;
		private MidiIn midiInput;
		private Stopwatch stopwatch;
		private bool isListening;

		public event EventHandler<MidiNoteEventEventArgs> MidiNoteReceived;

		public MidiListener(int deviceIndex)
		{
			this.deviceIndex = deviceIndex;
			this.isListening = false;
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
				midiInput?.Stop();
				midiInput?.Dispose();
				midiInput = null;
			}

			// free unmanaged resources (unmanaged objects) and override a finalizer below.
			// set large fields to null.

			disposed = true;
		}

		#endregion

		public void StartListening()
		{
			if (isListening)
			{
				throw new InvalidOperationException("Listening already active");
			}

			isListening = true;
			midiInput = new MidiIn(deviceIndex);
			stopwatch = new Stopwatch();

			midiInput.MessageReceived += MessageReceived;

			stopwatch.Start();
			midiInput.Start();
		}

		public void StopListening()
		{
			if (!isListening)
			{
				throw new InvalidOperationException("Listening not active");
			}

			stopwatch?.Stop();
			midiInput?.Stop();
			midiInput?.Dispose();
			midiInput = null;
		}

		protected virtual void OnMidiNoteReceived(MidiNoteEventEventArgs e)
		{
			var handler = MidiNoteReceived;
			handler?.Invoke(this, e);
		}

		private void MessageReceived(object sender, MidiInMessageEventArgs e)
		{
			if ((e.MidiEvent.CommandCode == MidiCommandCode.NoteOn) && (e.MidiEvent is NoteOnEvent noteOnEvent))
			{
				long elapsedTime = stopwatch.ElapsedMilliseconds;
				OnMidiNoteReceived(new(MidiNoteEventType.NoteOn, noteOnEvent.NoteNumber, elapsedTime));
			}
			else if ((e.MidiEvent.CommandCode == MidiCommandCode.NoteOff) && (e.MidiEvent is NoteEvent noteOffEvent))
			{
				long elapsedTime = stopwatch.ElapsedMilliseconds;
				OnMidiNoteReceived(new(MidiNoteEventType.NoteOff, noteOffEvent.NoteNumber, elapsedTime));
			}
		}
	}
}