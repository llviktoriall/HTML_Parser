using System;
using System.Windows.Forms;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ParseHTML
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //получение входной строки, передача её в класс ParseHTML и вывод результатов
        private void StartButton_Click(object sender, EventArgs e)
        {
            richTextBoxOutput.Text = "";
            string str = textBox_input.Text;//получение входной строки
            string path = textBox_save_place.Text;//получение пути сохранения файла
            //создаем объект класса ParseHTML
            ParseHTML prs = new ParseHTML(str, path);
            //получить результаты
            Dictionary<string, int> uniqueWords = prs.GetResult();
            //вывод результатов
            if (uniqueWords != null) OutPut(uniqueWords);
        }
        //вывод результатов
        private void OutPut(Dictionary<string, int> uniqueWords)
        {
            foreach (KeyValuePair<string, int> keyValue in uniqueWords)
            {
                richTextBoxOutput.Text += keyValue.Key + " - " + keyValue.Value + "\n";
            }
        }
    }
}
