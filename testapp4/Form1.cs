using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace testapp4
{
    public partial class Form1 : Form
    {
        private DateTime? softwareStartTime;
        private string selectedProcessName;
        private PerformanceCounter cpuCounter;
        private PerformanceCounter ramCounter;

        public Form1()
        {
            InitializeComponent();

            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PopulateProcessComboBox();
            // Set the timer interval to 1000 ms (1 second) and start the timer
            timer1.Interval = 1000;
            timer1.Enabled = true;
            timer1.Start();
        }

        // Event handler for the timer's Tick event
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Update system uptime
            TimeSpan systemUptime = TimeSpan.FromMilliseconds(Environment.TickCount);
            label1.Text = $"System Uptime: {systemUptime.Days}d {systemUptime.Hours}h {systemUptime.Minutes}m {systemUptime.Seconds}s";

            UpdateResourceUsage();

            // If a process is selected, update the uptime of the selected software
            if (!string.IsNullOrEmpty(selectedProcessName))
            {
                UpdateSoftwareUptime(selectedProcessName);
            }
        }

        private void PopulateProcessComboBox()
        {
            comboBox1.Items.Clear();

            // Get all running processes
            Process[] processes = Process.GetProcesses();

            // Add each process name to the ComboBox
            foreach (Process process in processes)
            {
                comboBox1.Items.Add(process.ProcessName);
            }

            // Set default selection to empty
            comboBox1.SelectedIndex = -1;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Set the selected process name
            selectedProcessName = comboBox1.SelectedItem?.ToString();

            // Reset the software start time so it recalculates for the new process
            softwareStartTime = null;
        }

        private void UpdateSoftwareUptime(string processName)
        {
            // Get all processes by the process name (e.g., "notepad")
            Process[] processes = Process.GetProcessesByName(processName);

            if (processes.Length > 0)
            {
                // Get the start time of the first instance of the process
                Process targetProcess = processes.FirstOrDefault();

                if (softwareStartTime == null || targetProcess.StartTime != softwareStartTime)
                {
                    softwareStartTime = targetProcess.StartTime;
                }

                // Calculate how long the software has been running
                TimeSpan softwareUptime = DateTime.Now - softwareStartTime.Value;

                // Display the software uptime in another label
                label2.Text = $"{processName}.exe Uptime: {softwareUptime.Days}d {softwareUptime.Hours}h {softwareUptime.Minutes}m {softwareUptime.Seconds}s";
            }
            else
            {
                // If the process is not running, reset the start time and show that the software is not running
                softwareStartTime = null;
                label2.Text = $"{processName}.exe is not running.";
            }
        }

            private void UpdateResourceUsage()
            {
                // Get current CPU usage
                float cpuUsage = cpuCounter.NextValue();
                // Get available RAM in MB
                float availableRam = ramCounter.NextValue();

                // Display the resource usage
                label3.Text = $"CPU Usage: {cpuUsage:F2}% | Available RAM: {availableRam:F2} MB";
            }
        }
    }
