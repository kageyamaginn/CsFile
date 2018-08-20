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
using System.Text.RegularExpressions;

namespace DealWithText
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            sourceText = "";
            richTextBox1.DataBindings.Add("Text", sourceText, "");
            LoadControl("");

        }


        public void LoadControl(String description)
        {
            if (String.IsNullOrEmpty(description))
            {
                NonFile();
            }
        }
        private string _content;
        
       
        public String sourceText { get; set; }
        private String FileName { get; set; }
        private String FileType;
        String Content
        {
            get { return _content; }
            set {
                _content = value;
                LoadSourceView();
                richTextBox1.Text = _content;
            }
        }
        List<String> Lines = new List<string>();

        public void LoadSourceView()
        {
            if (_content == "")
            {
                MessageBox.Show("No file content");return;
            }
            FileType = IdentitySourceType();
            LoadFileTypeSource();

        }

        private string IdentitySourceType()
        {
            String fileTypeDicContent = System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dictionary", "FileTypeDic.txt"));
            String[] fileTypeLines = fileTypeDicContent.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<Tuple<String, String>> fileTypes = new List<Tuple<string, string>>();
            foreach (String line in fileTypeLines)
            {
                String[] fileTypePair = line.Split(new String[] {"---" },StringSplitOptions.RemoveEmptyEntries);
                fileTypes.Add(new Tuple<string, string>(fileTypePair[0], fileTypePair[1]));

            }
            Tuple<String, String> matchedItem = fileTypes.Find(f => f.Item1 == System.IO.Path.GetExtension(FileName));
            if (matchedItem != null)
            {
                return matchedItem.Item2;
            }
            else
            {

                return GetFileTypeBySource();
              
            }
        }

        private string GetFileTypeBySource()
        {
            return "Text File";
        }

        public void NonFile()
        {
            flowControls.Controls.Clear();
            flowControls.Controls.Add(new Button { Name = "btnOpenFile", Text = "Open File" });
            flowControls.Controls.Find("btnOpenFile", true).First().Click += (openfile, openfileEvent) => {
                OpenFileDialog openFileD = new OpenFileDialog();
                if (openFileD.ShowDialog() == DialogResult.OK)
                {
                    FileName = openFileD.FileName;
                    Content = System.IO.File.ReadAllText(openFileD.FileName);
                }
            };
        }

        public void LoadFileTypeSource()
        {
            if (FileType == "Text File")
            {
                flowControls.Controls.Clear();
                Label lbFileType = new Label() {};
                flowControls.Controls.Add(lbFileType);
                lbFileType.AutoSize = false;
                lbFileType.Width = flowControls.Width;
                lbFileType.Height = 50;
                lbFileType.Text = "File Type Name:(format: file extension---file type)";
                TextBox txfileType = new TextBox();
                flowControls.Controls.Add(txfileType);

                Button confirm = new Button();
                confirm.Text = "Confirm";
                flowControls.Controls.Add(confirm);
                confirm.Click += (confirmSender, confirmArgs) => 
                {
                    StreamWriter sw = new StreamWriter(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dictionary", "FileTypeDic.txt"), true);
                    sw.Write("\r\n" + txfileType.Text.Trim());
                    FileType = txfileType.Text.Split(new String[] { "---" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    sw.Dispose();
                    StreamWriter swFile= File.CreateText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dictionary", String.Format("ExpressionDic--{0}.txt",FileType)));
                    swFile.Dispose();
                    LoadFileTypeSource();
                };
                flowControls.Refresh();
                return;
               
            }
            
            flowControls.Controls.Clear();
            Label kn_lbFileType = new Label() {Text="File Type: "+FileType };
            flowControls.Controls.Add(kn_lbFileType);
            LoadExpression(FileType,false);
        }

        private void LoadExpression(string fileType, bool v)
        {
            flowControls.Controls.Clear();
            String fileTypeExpressionContent = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dictionary", String.Format("ExpressionDic--{0}.txt", FileType)));
            String[] componentDefine = fileTypeExpressionContent.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            bool pairStar = false;
           

            foreach (string commandLine in componentDefine)
            {
                Regex regComponentName = new Regex("\\[\\S*\\]");
                MatchCollection matchColl = regComponentName.Matches(commandLine);
                string componentName = matchColl[0].Value.TrimEnd(']').TrimStart('[');

                switch (componentName)
                {
                    case "Lb":
                        Label lb = new Label();
                        lb.Text = commandLine.Substring(commandLine.IndexOf(']') + 1);
                        if (pairStar)
                        { flowControls.Controls[flowControls.Controls.Count - 1].Controls[0].Controls.Add(lb); }
                        else
                        {
                            flowControls.Controls.Add(lb);
                        }
                        lb.Width = lb.Parent.Width- lb.Parent.Margin.Left*2;
                        // lb.Dock = DockStyle.Top;
                        break;
                    case "ExGroup":
                        GroupBox gb = new GroupBox();
                        gb.Text = commandLine.Substring(commandLine.IndexOf(']') + 1);
                        FlowLayoutPanel container = new FlowLayoutPanel();
                        container.FlowDirection = FlowDirection.TopDown;
                        gb.Controls.Add(container);
                        container.Dock = DockStyle.Fill;
                        flowControls.Controls.Add(gb);
                        pairStar = true;
                      //  gb.Dock = DockStyle.Top;
                        break;
                    case "ExGroup--End":
                        pairStar = false;
                        break;
                    case "Tx":
                        TextBox tx = new TextBox();
                        tx.Text = commandLine.Substring(commandLine.IndexOf(']') + 1);
                        if (pairStar)
                        { flowControls.Controls[flowControls.Controls.Count - 1].Controls[0].Controls.Add(tx); }
                        else
                        {
                            flowControls.Controls.Add(tx);
                        }
                        // tx.Dock = DockStyle.Top;
                        tx.Width = tx.Parent.Width - tx.Parent.Margin.Left * 2;
                        break;
                 
                }
            }
        }
    }
}
