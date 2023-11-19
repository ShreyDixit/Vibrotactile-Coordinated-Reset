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
                MirrorHands.Enabled = true;
                RandomizGain.Enabled = true;
                vCRDuration.Enabled = true;
                JitterCheckBox.Enabled = true;
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

            CheckTDKErrors(Tdk.TdkInterface.ChangeGain(ConnectedBoardID, 0, GainTrackBar.Value, 0));

            Random rand = new Random();
            int leftHandSeed = rand.Next();
            int rightHandSeed = MirrorHands.Checked ? leftHandSeed : rand.Next(); //Same seed is used for mirrored hands
            

            Task task = startFingerVibrationSimulation(Total_Simulation_Duration, RandomizGain.Checked, leftHandSeed, rightHandSeed);
            await Task.WhenAll(task);

            SimulationStartButton.Enabled = true;
            GainTrackBar.Enabled = true;
            MirrorHands.Enabled = true;
            RandomizGain.Enabled = true;
            vCRDuration.Enabled = true;
            JitterCheckBox.Enabled = true;
            ToggleGainInputs();
        }

        private void disableAllInputs()
        {
            SimulationStartButton.Enabled = false;
            GainTrackBar.Enabled = false;
            MirrorHands.Enabled = false;
            RandomizGain.Enabled = false;
            vCRDuration.Enabled = false;
            JitterCheckBox.Enabled = false;
            GainMax.Enabled = false;
            GainMin.Enabled = false;
        }

        private async Task startFingerVibrationSimulation(int Total_Simulation_Duration, bool randomizedGain = false, int leftHandSeed = 42, int rightHandSeed = 42)
        {
            //vCR Simulation Parameters
            const int vCR_Cycle_Duration = 667; // ms
            const int vCR_Duration = 100; // ms
            const int vCR_Frequency = 2500; // Hz
            const int num_fingers = 4;
            const int vCR_Cycle_Duration_Single_Finger = vCR_Cycle_Duration / num_fingers;
            const int inter_stimulus_interval = vCR_Cycle_Duration_Single_Finger - vCR_Duration;
            const int jitter = (int)(inter_stimulus_interval * 0.235);
            int Number_of_vCR_Cycles = Total_Simulation_Duration / vCR_Cycle_Duration;

            // Setting what tactors to use on the basis of the hand
            int[] fingersLeft = { 1, 2, 3, 4 };
            int[] fingersRight = {8, 7, 6, 5};

            CheckTDKErrors(Tdk.TdkInterface.ChangeFreq(ConnectedBoardID, 0, vCR_Frequency, 0)); // setting the frequency

            Random randLeft = new Random(leftHandSeed);
            Random randRight = new Random(rightHandSeed);
            Random randomJitter = new Random();
            // Each finger of the hand will get a random gain value

            int vCR_Burst_Start = 0;

            for (int vCR_Cycle = 0; vCR_Cycle < Number_of_vCR_Cycles; vCR_Cycle++)
            {
                if (vCR_Cycle % 5 < 3)
                {
                    fingersLeft = fingersLeft.OrderBy(x => randLeft.Next()).ToArray(); //shuffling the order of the fingers to stimulate
                    fingersRight = fingersRight.OrderBy(x => randRight.Next()).ToArray(); //shuffling the order of the fingers to stimulate

                    for (int finger_idx = 0; finger_idx < num_fingers; finger_idx++)
                    {
                        Parallel.Invoke(
                            () => CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedBoardID, fingersLeft[finger_idx], vCR_Duration, vCR_Burst_Start)),
                            () => CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedBoardID, fingersRight[finger_idx], vCR_Duration, vCR_Burst_Start))
                            );
                        vCR_Burst_Start = JitterCheckBox.Checked ? randomJitter.Next(0, 2 * jitter) : jitter;

                        if (randomizedGain)
                        {
                            CheckTDKErrors(Tdk.TdkInterface.ChangeGain(ConnectedBoardID, fingersLeft[finger_idx], randLeft.Next(Int32.Parse(GainMin.Text), Int32.Parse(GainMax.Text)), 0));
                            CheckTDKErrors(Tdk.TdkInterface.ChangeGain(ConnectedBoardID, fingersRight[finger_idx], randRight.Next(Int32.Parse(GainMin.Text), Int32.Parse(GainMax.Text)), 0));
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
            }
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
    }
}
