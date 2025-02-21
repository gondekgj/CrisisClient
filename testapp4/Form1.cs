using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Windows.Forms;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography.X509Certificates;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

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
        public double cpuUsageProgram = 0;
        public long memoryUsageProgram = 0;
        public TimeSpan softwareUptime;
        public double timeElapsedProgram = 0;
        public TimeSpan uptimeProgram;
        public float cpuUsageSystem = 0;
        public float availableRamSystem = 0;
        public long systemElapsed = 0;

        public Form1()
        {
            InitializeComponent();

            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");

            //ip
            string ipPath = Path.Combine(Directory.GetCurrentDirectory(), "savedText.txt");
            string fileContent = ReadTextFromFile(ipPath);
            //port
            string portPath = Path.Combine(Directory.GetCurrentDirectory(), "savedText2.txt");
            string fileContent2 = ReadTextFromFile(portPath);
            //bring together
            label6.Text = fileContent + ":" + fileContent2;

        }
        //loads form aka timers
        private void Form1_Load(object sender, EventArgs e)
        {
            PopulateProcessComboBox();
            timer1.Interval = 1000;
            timer1.Enabled = true;
            timer1.Start();

            timer2.Interval = 60000;
            timer2.Tick += new EventHandler(LogProcessStatus);
            timer2.Start();
        }
        //big timer
        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan systemUptime = TimeSpan.FromMilliseconds(Environment.TickCount);
            label1.Text = $"System Uptime: {systemUptime.Days}d {systemUptime.Hours}h {systemUptime.Minutes}m {systemUptime.Seconds}s";
            systemElapsed = Environment.TickCount64;

            UpdateResourceUsage();

            if (!string.IsNullOrEmpty(selectedProcessName))
            {
                UpdateSoftwareUptimeAndUsage(selectedProcessName);
            }
        }
        //shows systems running
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
        //selecting software/service
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedProcessName = comboBox1.SelectedItem?.ToString();
            softwareStartTime = null;
            selectedProcess = null;
            lastTotalProcessorTime = TimeSpan.Zero;
            lastCpuMeasurementTime = DateTime.Now;
        }
        //updates usage pt1
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
                //force value on process
                softwareStartTime = null;
                selectedProcess = null;
                memoryUsageProgram = 0;
                cpuUsageProgram = 0;
                label2.Text = $"{processName}.exe is not running.";
                label4.Text = $"{processName}.exe | CPU Usage: 0.00% | App Memory Usage: 0 MB";
            }
        }
        //updates usage pt2
        public void UpdateProcessResourceUsage(Process process)
        {
            memoryUsageProgram = process.WorkingSet64 / (1024 * 1024); // Convert to MB

            TimeSpan currentTotalProcessorTime = process.TotalProcessorTime;
            DateTime currentCpuMeasurementTime = DateTime.Now;

            timeElapsedProgram = (currentCpuMeasurementTime - lastCpuMeasurementTime).TotalMilliseconds;

            if (timeElapsedProgram > 0)
            {
                cpuUsageProgram = (currentTotalProcessorTime - lastTotalProcessorTime).TotalMilliseconds / timeElapsedProgram * 100 / Environment.ProcessorCount;
            }
            else
            {
                cpuUsageProgram = 0;
            }

            lastTotalProcessorTime = currentTotalProcessorTime;
            lastCpuMeasurementTime = currentCpuMeasurementTime;

            label4.Text = $"{selectedProcessName}.exe | CPU Usage: {cpuUsageProgram:F2}% | App Memory Usage: {memoryUsageProgram} MB";
        }
        //log to file
        private async void LogProcessStatus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedProcessName))
            {
                Process[] processes = Process.GetProcessesByName(selectedProcessName);
                string logMessage;
                string jsonString;
                string state;
                string filePath2 = Path.Combine(Directory.GetCurrentDirectory(), "generatedData.json");
                string filePath3 = Path.Combine(Directory.GetCurrentDirectory(), "generatedDataMain.json");
                

                //logs json and text
                if (processes.Length > 0)
                {
                    logMessage = $"{DateTime.Now}: {selectedProcessName}.exe is running on {Environment.MachineName}.";
                    jsonString = $"{DateTime.Now}: {selectedProcessName}.exe is running on {Environment.MachineName}.";
                    state = "true";
                }
                else
                {
                    logMessage = $"{DateTime.Now}: {selectedProcessName}.exe is NOT running.";
                    jsonString = $"{DateTime.Now}: {selectedProcessName}.exe is NOT running.";
                    state = "false";
                }

                //test logging of text and json file
                LogToFile(logMessage);

                WriteJsonToFile(jsonString, filePath2);
                Console.WriteLine($"JSON file has been created at: {filePath2}");

                string softwareUptimeString = "N/A";
                if (softwareStartTime.HasValue)
                {
                    TimeSpan softwareUptime = DateTime.Now - softwareStartTime.Value;
                    softwareUptimeString = $"{softwareUptime.Days}d {softwareUptime.Hours}h {softwareUptime.Minutes}m {softwareUptime.Seconds}s";
                }
                else
                {
                    Console.WriteLine("Warning: softwareStartTime is null.");
                }
                TimeSpan systemUptime = TimeSpan.FromMilliseconds(Environment.TickCount64);

                // write formal json log
                var data = new
                {
                    Time = DateTime.Now,
                    Process = selectedProcessName,
                    Machine = Environment.MachineName,
                    Running = state,
                    SystemUptime = $"{systemUptime.Days}d {systemUptime.Hours}h {systemUptime.Minutes}m {systemUptime.Seconds}s",
                    SystemUsage = $"{cpuUsageSystem:F2}%",
                    SystemMemAvaliable = $"{availableRamSystem} MB",
                    ProcessUptime = softwareUptimeString,
                    ProcessUsage = $"{cpuUsageProgram:F2}%",
                    ProcessMemUsage = $"{memoryUsageProgram} MB"
                    
                };
                string jsonWrite = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath3, jsonWrite);

                PacketSending();

            }
        }
        //write to text
        private void LogToFile(string message)
        {
            string logFilePath = "ProcessStatusLog.txt";

            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine(message);
            }
        }
        //write json file
        public static void WriteJsonToFile(string jsonContent, string filePath2)
        {
            try
            {
                // Write the JSON content to the file
                File.WriteAllText(filePath2, jsonContent);
                Console.WriteLine("JSON file created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
        }
        //updating resource usage
        private void UpdateResourceUsage()
        {
            cpuUsageSystem = cpuCounter.NextValue();
            availableRamSystem = ramCounter.NextValue();

            label3.Text = $"CPU Usage: {cpuUsageSystem:F2}% | Available RAM: {availableRamSystem:F2} MB";
        }
        //open config menu
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 newForm = new Form2(this);
            newForm.Show();
        }
        //for reading from file
        private string ReadTextFromFile(string filePath)
        {
            try
            {
                return File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                return string.Empty;
            }
        }
        //active update of ip and port
        public void ExecuteAction()
        {
            string ipPath = Path.Combine(Directory.GetCurrentDirectory(), "savedText.txt");
            string fileContent = ReadTextFromFile(ipPath);
            //port
            string portPath = Path.Combine(Directory.GetCurrentDirectory(), "savedText2.txt");
            string fileContent2 = ReadTextFromFile(portPath);
            //bring together
            label6.Text = fileContent + ":" + fileContent2;
        }

        public async Task PacketSending()
        {
            //address
            string ipPath = Path.Combine(Directory.GetCurrentDirectory(), "savedText.txt");
            string fileContent = ReadTextFromFile(ipPath);
            //port
            string portPath = Path.Combine(Directory.GetCurrentDirectory(), "savedText2.txt");
            string fileContent2 = ReadTextFromFile(portPath);

            //Network URL for json sending
            //string url = "http://192.168.68.85:5000/clientUpdate"; PATH FOR SENDING HOME
            string url = "http://"+fileContent+":"+fileContent2+"/clientUpdate";

            //Path for json file
            //string filePath = @"C:\Users\jgond\Downloads\json\example.json"; // PATH FOR JSON FILE
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "generatedDataMain.json");

            try
            {
                if (File.Exists(filePath))
                {
                    //Reads JSON content from the file
                    string jsonContent = await File.ReadAllTextAsync(filePath);

                    //Creates the http client
                    using (HttpClient client = new HttpClient())
                    {
                        //Creates http packet with json info
                        HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                        //Sends post request
                        HttpResponseMessage response = await client.PostAsync(url, content);

                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine("File uploaded successfully!");
                        }
                        else
                        {
                            Console.WriteLine($"Failed to upload. Status Code: {response.StatusCode}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("File not found!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

    }
}
