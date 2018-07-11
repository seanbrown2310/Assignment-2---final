using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace Assignment_2___final
{
    public partial class Form1 : Form
    {
        class row
        {
            public double time;
            public double altitude;
            public double velocity;
            public double acceleration;
        }

        List<row> table = new List<row>();

        public Form1()
        {
            InitializeComponent();
        }

        private void calculateVelocity()
        {
            // This subroutine calculates the velocity
            for (int i = 1; i < table.Count; i++)
            {
                double ds = table[i].altitude - table[i - 1].altitude;
                double dt = table[i].time - table[i - 1].time;
                table[i].velocity = ds / dt;
            }
        }

        private void calculateAcceleration()
        {
            // This subroutine calculates the acceleration
            for (int i = 1; i < table.Count; i++)
            {
                double dv = table[i].velocity - table[i - 1].velocity;
                double dt = table[i].time - table[i - 1].time;
                table[i].acceleration = dv / dt;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "CSV Files|*.csv";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
                    {
                        string line = sr.ReadLine();
                        while (!sr.EndOfStream)
                        {
                            table.Add(new row());
                            string[] l = sr.ReadLine().Split(',');
                            table.Last().time = double.Parse(l[0]);
                            table.Last().altitude = double.Parse(l[1]);

                        }
                        calculateVelocity();
                        calculateAcceleration();
                    }
                }
                catch (IOException)
                {
                    MessageBox.Show(openFileDialog1.FileName + " failed to open.");
                }
                catch (FormatException)
                {
                    MessageBox.Show(openFileDialog1.FileName + " is not in the required format.");
                }
                catch (IndexOutOfRangeException)
                {
                    MessageBox.Show(openFileDialog1.FileName + " is not in the required format");
                }
            }

        }

        private void velocityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.IsMarginVisible = false;
            Series series = new Series
            {
                Name = "Velocity",
                Color = Color.Blue,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline,
                BorderWidth = 2
            };
            chart1.Series.Add(series);
            foreach (row r in table.Skip(1))
            {
                series.Points.AddXY(r.time, r.velocity);
            }
            chart1.ChartAreas[0].AxisX.Title = "time (s)";
            chart1.ChartAreas[0].AxisY.Title = "velocity (m/s)";
            chart1.ChartAreas[0].RecalculateAxesScale();
        }

        private void accelerationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.IsMarginVisible = false;
            Series series = new Series
            {
                Name = "Acceleration",
                Color = Color.Blue,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline,
                BorderWidth = 2
            };
            chart1.Series.Add(series);
            foreach (row r in table.Skip(1))
            {
                series.Points.AddXY(r.time, r.acceleration);
            }
            chart1.ChartAreas[0].AxisX.Title = "time (s)";
            chart1.ChartAreas[0].AxisY.Title = "acceleration (m/s²)";
            chart1.ChartAreas[0].RecalculateAxesScale();
        }

        private void altitudeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.IsMarginVisible = false;
            Series series = new Series
            {
                Name = "Altitude",
                Color = Color.Blue,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline,
                BorderWidth = 2
            };
            chart1.Series.Add(series);
            foreach (row r in table.Skip(1))
            {
                series.Points.AddXY(r.time, r.altitude);
            }
            chart1.ChartAreas[0].AxisX.Title = "time (s)";
            chart1.ChartAreas[0].AxisY.Title = "altitude (m)";
            chart1.ChartAreas[0].RecalculateAxesScale();
        }

        private void saveCsvToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "";
            saveFileDialog1.Filter = "csv Files|*.csv";
            DialogResult results = saveFileDialog1.ShowDialog();
            if (results == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDialog1.FileName))
                    {
                        sw.WriteLine("Time (s), Velocity (m/s), Acceleration (m/s²), Altitude (m)");
                        foreach (row r in table)
                        {
                            sw.WriteLine(r.time + "," + r.velocity + "," + r.acceleration + "," + r.altitude);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show(saveFileDialog1.FileName + " failed to save.");
                }
            }
        }

        private void savePngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "";
            saveFileDialog1.Filter = "png Files|*.png";
            DialogResult results = saveFileDialog1.ShowDialog();
            if (results == DialogResult.OK)
            {
                try
                {
                    chart1.SaveImage(saveFileDialog1.FileName, ChartImageFormat.Png);
                }
                catch
                {
                    MessageBox.Show(saveFileDialog1.FileName + " failed to save.");
                }
            }
        }
    }
}
