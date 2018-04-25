﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using NAudio;
using NAudio.Wave;

namespace Grabacion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WaveIn waveIn;
        WaveOut waveOut;
        WaveFormat formato;
        WaveFileWriter writer;
        AudioFileReader reader;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnIniciarGrabacion_Click(object sender, RoutedEventArgs e)
        {
            waveIn = new WaveIn();
            waveIn.WaveFormat = new WaveFormat(44100, 16, 1);
            formato = waveIn.WaveFormat;

            waveIn.DataAvailable += OnDataAvailable;
            waveIn.RecordingStopped += OnRecordingStopped;

            writer = 
                new WaveFileWriter("sonido.wav", formato);

            waveIn.StartRecording();

        }

        void OnRecordingStopped(object sender, StoppedEventArgs e)
        {
            writer.Dispose();
        }

        void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            byte[] buffer = e.Buffer;
            int bytesGrabados = e.BytesRecorded;

            for(int i = 0; i < bytesGrabados; i++)
            {
                lblMuestra.Text = buffer[i].ToString();
            }

            writer.Write(buffer, 0, bytesGrabados);
        }

        private void btnDetenerGrabacion_Click(object sender, RoutedEventArgs e)
        {
            waveIn.StopRecording();
        }

        private void btnReproducirGrabacion_Click(object sender, RoutedEventArgs e)
        {
            if (waveOut != null)
            {
                if (waveOut.PlaybackState == PlaybackState.Playing)
                {
                    waveOut.Stop();
                }
                waveOut.Init(reader);
                waveOut.Play();
            }
        }
    }
}
