using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;

using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace ParseHTML
{
    class ParseHTML
    {
        private readonly string _strInput;
        private readonly string _filePath;
        private bool err;
        private Dictionary<string, int> uniqueWords;

        public ParseHTML(string _strInput, string _filePath)
        {
            this._strInput = _strInput;
            this._filePath = _filePath;
            err = false;
            uniqueWords = new Dictionary<string, int>();
        }
        //получение страницы целиком
        public string GetFullPage()
        {
            string fullPage = null;
            WebRequest request = WebRequest.Create(_strInput);
            WebResponse response = null;
            try
            {
                response = request.GetResponse();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                err = true;
                return null;
            }
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        fullPage += line;
                    }
                }
            }
            response.Close();
            return fullPage;
        }
        //сохранение файла на жесткий диск
        public void SavePageToFile(string fullPage)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(_filePath, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(fullPage);
                }
                MessageBox.Show("Запись в файл выполнена");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                err = true;
            }
        }
        //разбить страницу на слова
        private List<string> SplitPageIntoWords(string fullPage)
        {
            //разбить страницу на текстовые фрагменты
            List<string> listOfTexts = new List<string>();
            string fragmentOfText = "";
            string[] tags = { "<div", "<title", "<span", "<a", "<p", "<h1", "<h2", "<h3", "<h4", "<h5", "<br", "<input", "<button" };
            int lengthOfTag = 0;
            foreach (string tg in tags)
            {
                lengthOfTag = tg.Length;
                if (fullPage.Contains(tg))
                    for (int i = 0; i < fullPage.Length - lengthOfTag; i++)
                    {
                        if (fullPage.Substring(i, lengthOfTag) == tg)
                        {
                            fragmentOfText = "";
                            i += lengthOfTag;
                            while (fullPage.Substring(i, 1) != ">" && i < fullPage.Length - 1) i++;//пропуск внутренней части тега
                            i++;//пропуск закрывающей части тега >
                            while (fullPage.Substring(i, 1) != "<" && i < fullPage.Length - 1)
                            {
                                fragmentOfText += fullPage.Substring(i, 1);
                                i++;
                            }
                            listOfTexts.Add(fragmentOfText);
                            i--;
                        }
                    }
            }
            //разбить фрагменты на готовые слова
            List<string> listOfWords = new List<string>();
            for (int i = 0; i < listOfTexts.Count; i++)
            {
                string[] masOfAllWords = listOfTexts[i].Split('\'', '«', '»', '|', '/', '—', '©', '#', '&', '*', '%', '+', '=', '^',
                    '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', ' ', ',', '.', '!', '?', '"', ';', ':', '[', ']', '(', ')', '-');
                if (masOfAllWords != null)
                    foreach (string s in masOfAllWords)
                    {
                        string s_up = s.ToUpper();
                        if (s_up != "" && s_up != "PDF" && s_up != "PNG" && s_up != "JPG" && s_up != "DOC" && s_up != "XLS" &&
                            s_up != "DOCX" && s_up != "XLSX" && s_up != "JPEG")
                        {
                            listOfWords.Add(s_up);
                        }
                    }
            }
            return listOfWords;
        }
        //посчитать количество уникальных слов
        private void CountUniqueWords(List<string> listOfWords)
        {
            foreach (string word in listOfWords)
            {
                if (uniqueWords.ContainsKey(word)) uniqueWords[word]++;
                else uniqueWords.Add(word, 1);
            }
        }
        //метод для возврата результата, в нём запускаются остальные методы
        public Dictionary<string, int> GetResult()
        {
            string fullPage = GetFullPage();
            List<string> listOfWords = new List<string>();
            if (!err) SavePageToFile(fullPage);
            if (!err) listOfWords = SplitPageIntoWords(fullPage);
            if (!err) CountUniqueWords(listOfWords);
            return uniqueWords;
        }
    }
}