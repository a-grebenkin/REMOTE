using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REMOTE
{
    public partial class RemoteForm : Form
    {
        private TextBox txtCommand;
        private Button btnSend;
        private TextBox txtOutput;
        private TextBox txtServer;
        private TextBox txtPort;

        public RemoteForm()
        {
            // Заголовок окна
            this.Text = "REMOTE - Удалённое выполнение команд";
            this.Width = 520;
            this.Height = 550;

            // Поле ввода IP-адреса
            Label lblServer = new Label { Text = "Сервер:", Left = 10, Top = 10, Width = 50 };
            txtServer = new TextBox { Left = 70, Top = 10, Width = 150, Text = "127.0.0.1" };

            // Поле ввода порта
            Label lblPort = new Label { Text = "Порт:", Left = 230, Top = 10, Width = 40 };
            txtPort = new TextBox { Left = 280, Top = 10, Width = 60, Text = "5000" };

            // Поле для ввода команды
            txtCommand = new TextBox { Left = 10, Top = 40, Width = 400 };
            btnSend = new Button { Text = "Выполнить", Left = 420, Top = 40, Width = 80 };
            btnSend.Click += new EventHandler(SendCommand);

            // Поле вывода результата
            txtOutput = new TextBox { Left = 10, Top = 80, Width = 480, Height = 400, Multiline = true, ScrollBars = ScrollBars.Vertical, ReadOnly = true };

            // Добавляем элементы в форму
            this.Controls.Add(lblServer);
            this.Controls.Add(txtServer);
            this.Controls.Add(lblPort);
            this.Controls.Add(txtPort);
            this.Controls.Add(txtCommand);
            this.Controls.Add(btnSend);
            this.Controls.Add(txtOutput);
        }

        private void SendCommand(object sender, EventArgs e)
        {
            string server = txtServer.Text;
            int port;

            if (!int.TryParse(txtPort.Text, out port))
            {
                MessageBox.Show("Ошибка: неверный порт.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string command = txtCommand.Text;
            if (string.IsNullOrWhiteSpace(command))
            {
                MessageBox.Show("Введите команду!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (TcpClient client = new TcpClient(server, port))
                using (NetworkStream stream = client.GetStream())
                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true })
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    writer.WriteLine(command);
                    txtOutput.Text = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка соединения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
