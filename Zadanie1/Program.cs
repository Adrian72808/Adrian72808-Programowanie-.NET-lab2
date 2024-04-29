using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace Dzielenie
{
    public partial class Form1 : Form
    {
        private TextBox txtDzielna;
        private TextBox txtDzielnik;
        private Button btnDziel;
        private TextBox txtWynik;

        public Form1()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.txtDzielna = new TextBox();
            this.txtDzielnik = new TextBox();
            this.btnDziel = new Button();
            this.txtWynik = new TextBox();
            this.SuspendLayout();
         

            this.txtDzielna.Location = new System.Drawing.Point(50, 50);
            this.txtDzielna.Name = "txtDzielna";
            this.txtDzielna.Size = new System.Drawing.Size(100, 20);
            this.txtDzielna.TabIndex = 0;
    

            this.txtDzielnik.Location = new System.Drawing.Point(50, 100);
            this.txtDzielnik.Name = "txtDzielnik";
            this.txtDzielnik.Size = new System.Drawing.Size(100, 20);
            this.txtDzielnik.TabIndex = 1;
  

            this.btnDziel.Location = new System.Drawing.Point(50, 150);
            this.btnDziel.Name = "btnDziel";
            this.btnDziel.Size = new System.Drawing.Size(100, 30);
            this.btnDziel.TabIndex = 2;
            this.btnDziel.Text = "Dziel";
            this.btnDziel.UseVisualStyleBackColor = true;
            this.btnDziel.Click += new System.EventHandler(this.btnDziel_Click);
     

            this.txtWynik.Location = new System.Drawing.Point(50, 200);
            this.txtWynik.Name = "txtWynik";
            this.txtWynik.ReadOnly = true;
            this.txtWynik.Size = new System.Drawing.Size(100, 20);
            this.txtWynik.TabIndex = 3;
   

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(200, 250);
            this.Controls.Add(this.txtWynik);
            this.Controls.Add(this.btnDziel);
            this.Controls.Add(this.txtDzielnik);
            this.Controls.Add(this.txtDzielna);
            this.Name = "Form1";
            this.Text = "Dzielenie";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void btnDziel_Click(object sender, EventArgs e)
        {
            // sprawdza zawartość czy nie jest pusta
            if (string.IsNullOrWhiteSpace(txtDzielna.Text) || string.IsNullOrWhiteSpace(txtDzielnik.Text))
            {
                LogEvent("Błąd: Nie wprowadzono wartości w obu polach.");
                MessageBox.Show("Proszę wprowadzić wartości w polach.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!double.TryParse(txtDzielna.Text, out double dzielna) || !double.TryParse(txtDzielnik.Text, out double dzielnik))
            {
                LogEvent("Błąd: Wprowadzono nieprawidłowe liczby.");
                MessageBox.Show("Proszę wprowadzić poprawne liczby.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // sprawdza czy dzielnik nie jest zerem
            if (dzielna == 0)
            {
                LogEvent("Błąd: Nie można dzielić przez zero.");
                MessageBox.Show("Nie można dzielić przez zero.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Dzielenie
            try
            {
                double wynik = dzielna / dzielnik;

                txtWynik.Text = wynik.ToString();
            }
            catch (Exception ex)
            {
                LogEvent("Błąd: Wystąpił nieoczekiwany błąd podczas wykonywania dzielenia. Szczegóły: " + ex.Message);
                MessageBox.Show("Wystąpił nieoczekiwany błąd podczas wykonywania dzielenia.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LogEvent(string message)
        {
            string eventLogName = "Application";

            if (!EventLog.SourceExists("Dzielenie"))
            {
                EventLog.CreateEventSource(new EventSourceCreationData("Dzielenie", eventLogName));
            }

            // Zapisuje błąd
            using (EventLog eventLog = new EventLog(eventLogName))
            {
                eventLog.Source = "Dzielenie";
                eventLog.WriteEntry(message, EventLogEntryType.Error);
            }
        }



        static class Program
        {

            [STAThread]
            static void Main()
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
        }
    }
}
