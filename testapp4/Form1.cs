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
        private Process selectedProcess;
        private TimeSpan lastTotalProcessorTime;
        private DateTime lastCpuMeasurementTime;

        public Form1()
        {
            InitializeComponent();

            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PopulateProcessComboBox();
            timer1.Interval = 1000;
            timer1.Enabled = true;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan systemUptime = TimeSpan.FromMilliseconds(Environment.TickCount);
            label1.Text = $"System Uptime: {systemUptime.Days}d {systemUptime.Hours}h {systemUptime.Minutes}m {systemUptime.Seconds}s";

            UpdateResourceUsage();

            if (!string.IsNullOrEmpty(selectedProcessName))
            {
                UpdateSoftwareUptimeAndUsage(selectedProcessName);
            }
        }

        private void PopulateProcessComboBox()
        {
            comboBox1.Items.Clear();

            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
                comboBox1.Items.Add(process.ProcessName);
            }

            comboBox1.SelectedIndex = -1;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedProcessName = comboBox1.SelectedItem?.ToString();

            softwareStartTime = null;
            selectedProcess = null;
            lastTotalProcessorTime = TimeSpan.Zero;
            lastCpuMeasurementTime = DateTime.Now;
        }

        private void UpdateSoftwareUptimeAndUsage(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);

            if (processes.Length > 0)
            {
                selectedProcess = processes.FirstOrDefault();

                if (softwareStartTime == null || selectedProcess.StartTime != softwareStartTime)
                {
                    softwareStartTime = selectedProcess.StartTime;
                }

                TimeSpan softwareUptime = DateTime.Now - softwareStartTime.Value;

                label2.Text = $"{processName}.exe | Uptime: {softwareUptime.Days}d {softwareUptime.Hours}h {softwareUptime.Minutes}m {softwareUptime.Seconds}s";

                UpdateProcessResourceUsage(selectedProcess);
            }
            else
            {
                softwareStartTime = null;
                label2.Text = $"{processName}.exe is not running.";
            }
        }
        private void UpdateProcessResourceUsage(Process process)
        {
            long memoryUsage = process.WorkingSet64 / (1024 * 1024); // Convert to MB

            TimeSpan currentTotalProcessorTime = process.TotalProcessorTime;
            DateTime currentCpuMeasurementTime = DateTime.Now;

            double cpuUsage = 0;
            double timeElapsed = (currentCpuMeasurementTime - lastCpuMeasurementTime).TotalMilliseconds;

            if (timeElapsed > 0)
            {
                cpuUsage = (currentTotalProcessorTime - lastTotalProcessorTime).TotalMilliseconds / timeElapsed * 100 / Environment.ProcessorCount;
            }

            lastTotalProcessorTime = currentTotalProcessorTime;
            lastCpuMeasurementTime = currentCpuMeasurementTime;

            label4.Text = $"{selectedProcessName}.exe | CPU Usage: {cpuUsage:F2}% | App Memory Usage: {memoryUsage} MB";
        }

        private void UpdateResourceUsage()
            {
                float cpuUsage = cpuCounter.NextValue();
                float availableRam = ramCounter.NextValue();

                label3.Text = $"CPU Usage: {cpuUsage:F2}% | Available RAM: {availableRam:F2} MB";
            }
        }
    }
