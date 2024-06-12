/* ---------------------------------------------------------------------------
** The software supplied herewith by Engineering Acoustics, Inc.
** (the Company) for its Tactor Development Kit is intended and
** supplied to you, the Company's customer, for use solely and
** exclusively on Engineering Acoustics, Inc. products. The
** software is owned by the Company and/or its supplier, and is
** protected under applicable copyright laws. All rights are reserved.
** Any use in violation of the foregoing restrictions may subject the
** user to criminal sanctions under applicable laws, as well as to
** civil liability for the breach of the terms and conditions of this
** license.
**
** THIS SOFTWARE IS PROVIDED IN AN AS IS CONDITION. NO WARRANTIES,
** WHETHER EXPRESS, IMPLIED OR STATUTORY, INCLUDING, BUT NOT LIMITED
** TO, IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
** PARTICULAR PURPOSE APPLY TO THIS SOFTWARE. THE COMPANY SHALL NOT,
** IN ANY CIRCUMSTANCES, BE LIABLE FOR SPECIAL, INCIDENTAL OR
** CONSEQUENTIAL DAMAGES, FOR ANY REASON WHATSOEVER.
**
**   Copyright 2015(c) Engineering Acoustics Inc. All rights reserved.   *
** -------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Threading;

//-------------------------------------------------------------------------//
//TDK Windows C# Discover Controller Tutorial
//This project show how the discovery system work. When the user presses 
//the discover button it calls discover on the tdk.
//The combobox is then populated with the found devices.
//The user can select wich device to connect to in the combo box and press
//the connect button to connect to it.x
//The user is able to send a pulse command to the board by pressing the 
//pulse tactor 1 button
//-------------------------------------------------------------------------//

namespace DiscoverController
{
    public partial class DiscoverControllerForm : Form
    {
        private int ConnectedBoardID = -1;
        private CancellationTokenSource cancellationTokenSource;
        private bool mirroredHands;
        private bool jitterOn;
        //vCR Simulation Parameters
        private const int vCR_Cycle_Duration = 667; // ms
        private const int vCR_Duration = 100; // ms
        private const int vCR_Frequency = 2500; // Hz
        private const int num_fingers = 4;
        private const int vCR_Cycle_Duration_Single_Finger = vCR_Cycle_Duration / num_fingers;
        private const int inter_stimulus_interval = vCR_Cycle_Duration_Single_Finger - vCR_Duration;
        private const int jitter = (int)(inter_stimulus_interval * 0.235);
        // Setting what tactors to use on the basis of the hand
        private int[] fingersLeft = { 1, 2, 3, 4 };
        private int[] fingersRight = { 8, 7, 6, 5 };

        public DiscoverControllerForm()
        {
            InitializeComponent();
            //To initialize the TDKInterface we need to call InitializeTI before we use any
            //of its functionality
            WriteMessageToGUIConsole("InitializeTI\n");
            CheckTDKErrors(Tdk.TdkInterface.InitializeTI());
        }

        private void DiscoverButton_Click(object sender, EventArgs e)
        {
            WriteMessageToGUIConsole("Discover Started...\n");
            //Discovers all serial tactor devices and returns the amount found
            int ret = Tdk.TdkInterface.Discover((int)Tdk.TdkDefines.DeviceTypes.Serial);
            if (ret > 0)
            {
                WriteMessageToGUIConsole("Discover Found:\n");
                //populate combo box with discovered names
                for (int i = 0; i < ret; i++)
                {
                    //Gets the discovered device name at the index i
                    System.IntPtr discoveredNamePTR = Tdk.TdkInterface.GetDiscoveredDeviceName(i);
                    if (discoveredNamePTR != null)
                    {
                        string sComName = Marshal.PtrToStringAnsi(discoveredNamePTR);
                        WriteMessageToGUIConsole(sComName + "\n");
                        ComPortComboBox.Items.Add(sComName);
                    }
                    else
                        WriteMessageToGUIConsole(Tdk.TdkDefines.GetLastEAIErrorString());
                }
                ComPortComboBox.SelectedIndex = 0;
                DiscoverButton.Enabled = false;
                ConnectButton.Enabled = true;
            }
            else
            {
                WriteMessageToGUIConsole("Discover Failed:\n");
                WriteMessageToGUIConsole(Tdk.TdkDefines.GetLastEAIErrorString());
            }
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            string selectedComPort = ComPortComboBox.SelectedItem.ToString();
            WriteMessageToGUIConsole("\nConnecting to com port " + selectedComPort + "\n");
            //Connect connects to the tactor controller via serial with the given name
            //we should be hooking up a response callback but for simplicity of the 
            //tutorial we wont be. Reference the ResponseCallback tutorial for more information
            int ret = Tdk.TdkInterface.Connect(selectedComPort,
                                               (int)Tdk.TdkDefines.DeviceTypes.Serial,
                                                System.IntPtr.Zero);
            if (ret >= 0)
            {
                ConnectedBoardID = ret;
                DiscoverButton.Enabled = false;
                ConnectButton.Enabled = false;
                SimulationStartButton.Enabled = true;
                GainTrackBar.Enabled = true;
                RandomizGain.Enabled = true;
                vCRDuration.Enabled = true;
            }
            else
            {
                WriteMessageToGUIConsole(Tdk.TdkDefines.GetLastEAIErrorString());
            }
        }

        private void WriteMessageToGUIConsole(string msg)
        {
            ConsoleOutputRichTextBox.AppendText(msg);
        }

        private void DiscoverControllerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //closes up the connection to the tactor device with ConnectedBoardID
            CheckTDKErrors(Tdk.TdkInterface.Close(ConnectedBoardID));
            //cleans up everyting associated witht the TActionManager. Unloads any TActions loaded
            CheckTDKErrors(Tdk.TdkInterface.ShutdownTI());
        }
        private void CheckTDKErrors(int ret)
        {
            //if a tdk method returns less then zero then we should display the last error
            //in the tdk interface
            if (ret < 0)
            {
                //the GetLastEAIErrorString returns a string that represents the last error code
                //recorded in the tdk interface.
                WriteMessageToGUIConsole(Tdk.TdkDefines.GetLastEAIErrorString());
            }
        }

        private async void startButton_Click(object sender, EventArgs e)
        {
            disableAllInputs();

            int Total_Simulation_Duration = (int)(decimal.Parse(vCRDuration.Text) * 60 * 1000); // converts min to ms
            int Number_of_vCR_Cycles = Total_Simulation_Duration / vCR_Cycle_Duration;

            CheckTDKErrors(Tdk.TdkInterface.ChangeGain(ConnectedBoardID, 0, GainTrackBar.Value, 0));

            Random rand = new Random();
            int leftHandSeed = rand.Next();
            int rightHandSeed = this.mirroredHands ? leftHandSeed : rand.Next(); //Same seed is used for mirrored hands

            cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            progressBar.Maximum = Number_of_vCR_Cycles;
            progressBar.Value = 0;

            Task task = startFingerVibrationSimulation(Number_of_vCR_Cycles, cancellationToken, RandomizGain.Checked);
            await Task.WhenAll(task);

            enableAllInputs();
            ToggleGainInputs();
        }

        private void enableAllInputs()
        {
            SimulationStartButton.Enabled = true;
            stopButton.Enabled = false;
            GainTrackBar.Enabled = true;
            RandomizGain.Enabled = true;
            vCRDuration.Enabled = true;
        }

        private void disableAllInputs()
        {
            SimulationStartButton.Enabled = false;
            stopButton.Enabled = true;
            GainTrackBar.Enabled = false;
            RandomizGain.Enabled = false;
            vCRDuration.Enabled = false;
            GainMax.Enabled = false;
            GainMin.Enabled = false;
        }

        private async Task startFingerVibrationSimulation(int Number_of_vCR_Cycles, CancellationToken cancellationToken, bool randomizedGain = false)
        {
            System.Windows.Forms.Timer timer = getTimer();

            CheckTDKErrors(Tdk.TdkInterface.ChangeFreq(ConnectedBoardID, 0, vCR_Frequency, 0)); // setting the frequency

            Random randHands = new Random();
            int leftHandSeed = randHands.Next();
            int rightHandSeed = this.mirroredHands ? leftHandSeed : randHands.Next(); //Same seed is used for mirrored hands

            Random randLeft = new Random(leftHandSeed);
            Random randRight = new Random(rightHandSeed);
            Random randomJitter = new Random();
            // Each finger of the hand will get a random gain value

            int vCR_Burst_Start = 0;
            timer.Start();

            for (int vCR_Cycle = 0; vCR_Cycle < Number_of_vCR_Cycles; vCR_Cycle++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    // Handle the cancellation request here (e.g., clean up resources)
                    break;
                }

                if (vCR_Cycle % 5 < 3)
                {
                    int []fingersLeftThisCycle = fingersLeft.OrderBy(x => randLeft.Next()).ToArray(); //shuffling the order of the fingers to stimulate
                    int[] fingersRightThisCycle = fingersRight.OrderBy(x => randRight.Next()).ToArray(); //shuffling the order of the fingers to stimulate

                    for (int finger_idx = 0; finger_idx < num_fingers; finger_idx++)
                    {
                        Parallel.Invoke(
                           () => CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedBoardID, fingersLeftThisCycle[finger_idx], vCR_Duration, vCR_Burst_Start)),
                           () => CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedBoardID, fingersRightThisCycle[finger_idx], vCR_Duration, vCR_Burst_Start))
                           );
                        vCR_Burst_Start = jitterOn ? randomJitter.Next(0, 2 * jitter) : jitter;

                        if (randomizedGain)
                        {
                            CheckTDKErrors(Tdk.TdkInterface.ChangeGain(ConnectedBoardID, fingersLeftThisCycle[finger_idx], randLeft.Next(Int32.Parse(GainMin.Text), Int32.Parse(GainMax.Text)), 0));
                            CheckTDKErrors(Tdk.TdkInterface.ChangeGain(ConnectedBoardID, fingersRightThisCycle[finger_idx], randRight.Next(Int32.Parse(GainMin.Text), Int32.Parse(GainMax.Text)), 0));
                        }

                        if (vCR_Cycle == 0 && finger_idx == 0)
                            await Task.Delay(vCR_Cycle_Duration_Single_Finger - jitter);
                        else
                            await Task.Delay(vCR_Cycle_Duration_Single_Finger);
                    }
                }
                else
                {
                    await Task.Delay(vCR_Cycle_Duration);
                }
                progressBar.Value = vCR_Cycle + 1;
            }
            timer.Stop();
        }

        private System.Windows.Forms.Timer getTimer()
        {
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // 1000 ms = 1 second
            int elapsedTime = 0; // This will store the elapsed time in seconds

            timer.Tick += (sender, e) =>
            {
                elapsedTime++;

                int hours = elapsedTime / 3600;
                int minutes = (elapsedTime % 3600) / 60;
                int seconds = elapsedTime % 60;

                timeLabel.Text = $"Elapsed Time: {hours:D2}h {minutes:D2}m {seconds:D2}s";
            };
            return timer;
        }

        private void GainTrackBar_Scroll(object sender, EventArgs e)
        {
            GainValue.Text = GainTrackBar.Value.ToString();
        }

        private void RandomizGain_CheckedChanged(object sender, EventArgs e)
        {
            ToggleGainInputs();
        }

        private void ToggleGainInputs()
        {
            if (RandomizGain.Checked)
            {
                GainTrackBar.Enabled = false;
                GainMax.Enabled = true;
                GainMin.Enabled = true;
            }
            else
            {
                GainTrackBar.Enabled = true;
                GainMax.Enabled = false;
                GainMin.Enabled = false;
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
            }

        }

        private void simProtocols_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedSimProtocol = simProtocols.SelectedItem.ToString();
            switch (selectedSimProtocol)
            {
                case "A":
                    mirroredHands = true;
                    jitterOn = true;
                    break;
                case "B":
                    mirroredHands = false;
                    jitterOn = true;
                    break;
                case "C":
                    mirroredHands = true;
                    jitterOn = false;
                    break;
                case "D":
                    mirroredHands = false;
                    jitterOn = false;
                break;
            }
        }
    }
}
