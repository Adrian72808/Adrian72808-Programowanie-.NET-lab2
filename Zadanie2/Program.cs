using System;
using System.Diagnostics;
using System.Windows.Forms;
using NCalc;

namespace Kalkulator
{
    static class Program
    {
        private static Stopwatch initializationStopwatch = new Stopwatch();

        [STAThread]
        static void Main()
        {
            // Pomiar czasu
            initializationStopwatch.Start();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new KalkulatorForm());
            initializationStopwatch.Stop();

            // Sprawdzanie czasu 
            TimeSpan initializationTime = initializationStopwatch.Elapsed;
            TimeSpan threshold = TimeSpan.FromSeconds(0.2); // Ustawia próg czasu 
            if (initializationTime > threshold)
            {
                LogEvent("Czas inicjalizacji komponentów przekroczył próg: " + initializationTime.ToString());
            }
        }

        public static void LogEvent(string message)
        {
            string eventLogName = "Application";

            if (!EventLog.SourceExists("Kalkulator"))
            {
                EventLog.CreateEventSource(new EventSourceCreationData("Kalkulator", eventLogName));
            }

            // Zapis do dziennika 
            using (EventLog eventLog = new EventLog(eventLogName))
            {
                eventLog.Source = "Kalkulator";
                eventLog.WriteEntry(message, EventLogEntryType.Information);
            }
        }
    }

    public partial class KalkulatorForm : Form
    {
        private TextBox txtWynik;

        public KalkulatorForm()
        {
            InitializeComponent();
            InitializeTextBox();
            InitializeButtons();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "KalkulatorForm";
            this.ResumeLayout(false);
        }

        private void InitializeTextBox()
        {
            txtWynik = new TextBox();
            txtWynik.Location = new System.Drawing.Point(12, 12);
            txtWynik.Size = new System.Drawing.Size(260, 20);
            txtWynik.ReadOnly = true;
            this.Controls.Add(txtWynik);
        }

        private void InitializeButtons()
        {
            for (int i = 0; i <= 9; i++)
            {
                Button button = new Button();
                button.Text = i.ToString();
                button.Click += NumerButtonClick;
                this.Controls.Add(button);
                button.Location = new System.Drawing.Point(20 + (i % 3) * 50, 50 + (i / 3) * 50);
            }

            string[] operacje = { "+", "-", "*", "/", "=" };
            for (int i = 0; i < operacje.Length; i++)
            {
                Button button = new Button();
                button.Text = operacje[i];
                button.Click += OperacjaButtonClick;
                this.Controls.Add(button);
                button.Location = new System.Drawing.Point(20 + 3 * 50, 50 + (i * 50));
            }
        }

        private void NumerButtonClick(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            txtWynik.Text += button.Text;
        }

        private void OperacjaButtonClick(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.Text == "=")
            {
                ObliczWynik();
            }
            else
            {
                txtWynik.Text += button.Text;
            }
        }

        private void ObliczWynik()
        {
            try
            {
                string wyrazenie = txtWynik.Text;
                double wynik = Evaluate(wyrazenie);
                txtWynik.Text = wynik.ToString();
            }
            catch (Exception ex)
            {
                Program.LogEvent("Błąd: " + ex.Message);
                MessageBox.Show("Wystąpił błąd podczas obliczeń.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private double Evaluate(string expression)
        {
            Expression expr = new Expression(expression);
            object result = expr.Evaluate();

            if (result is int || result is double)
            {
                return Convert.ToDouble(result);
            }
            else
            {
                throw new ArgumentException("Wyrażenie jest niepoprawne.");
            }
        }
    }
}
