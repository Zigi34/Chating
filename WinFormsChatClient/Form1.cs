using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace WinFormsChatClient
{
    public partial class Form1 : Form
    {
        ChatClient client;

        public void updateing()
        {
            // update
            while (true)
            {
                string[] messages;
                lock (client)
                {
                    messages = this.client.update();
                }
                this.listBox1.Items.Clear();
                foreach (string m in messages)
                {
                    listBox1.Items.Add(m);
                }
                Thread.Sleep(3000);
            }
        }

        public Form1()
        {
            InitializeComponent();
            Form.CheckForIllegalCrossThreadCalls = false;
            client = new ChatClient("127.0.0.1", 8080);
            client.connect();
            client.login("ja");
            Thread t = new Thread(new ThreadStart(updateing));
            t.IsBackground = true;
            t.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.client.disconnect();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            client.sendMessage(this.textBox1.Text);
            string[] messages;
            lock (this.client)
            {
                messages = this.client.update();
            }
            this.listBox1.Items.Clear();
            foreach (string m in messages)
            {
                this.listBox1.Items.Add(m);
            }
            this.textBox1.Text = "";
        }
    }
}
