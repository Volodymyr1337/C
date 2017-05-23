using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Collections.Generic;


/*  Рекурсивный перебор всех вложенных поддиректорий и файлов 
 *  для указанной директории с занесением результатов в файл формата XML.
 *  
 *  Предварительно создаем: 
 *  private System.Windows.Forms.TreeView treeView1;    - каталог-дерево
 *  private System.Windows.Forms.ImageList imageList1;  - изображения файлов и директорий
 *  private System.Windows.Forms.ListView listView1;    - окно со списоком файлов в выделенной директории
 *  private Button button1;                             - кнопка сохранения информации
 */

namespace xml_App
{
    public partial class Form1 : Form
    {
        long dirSize;       // размер директории

        TreeNode newSelected;
        XmlWriter writer;
        Thread demoThread;
        Stream myStream;

        //список с информацией о директориях и файлах
        List<Items> Elements = new List<Items>();   

        public Form1()
        {
            InitializeComponent();

            TreeNode nodeTree = new TreeNode(@"..\..");
            // Добавляем корневой узел к дереву просмотра
            treeView1.Nodes.Add(nodeTree);

        }

        // рекурсивный перебор директорий
        void AddDirectories(TreeNode node)
        {
            dirSize = 0;

            string strPath = node.FullPath;
            DirectoryInfo dir = new DirectoryInfo(strPath);
            // Объявляем ссылку на массив подкаталогов текущего каталога
            DirectoryInfo[] arrayDirInfo;

            node.Nodes.Clear();

            arrayDirInfo = dir.GetDirectories();
            // Добавляем прочитанные подкаталоги как узлы в дерево 
            // и записываем инфо о них в список
            foreach (DirectoryInfo dirInfo in arrayDirInfo)
            {
                // Создаем новый узел с именем подкаталога
                TreeNode nodeDir = new TreeNode(dirInfo.Name);
                // Добавляем его как дочерний к текущему узлу
                node.Nodes.Add(nodeDir);

                AddDirectories(nodeDir);            
            }
            string dirfsar = "";
            DirectorySecurity dirSecurity = dir.GetAccessControl();
            IdentityReference dirIdentityRef = dirSecurity.GetOwner(typeof(NTAccount));
            foreach (FileSystemAccessRule permission in dirSecurity.GetAccessRules(true, true, typeof(NTAccount)))
            {
                dirfsar += permission.FileSystemRights.ToString();   // AccessControlType ?
            }

            // записываем информацию о файлах в директории
            foreach (FileInfo file in dir.GetFiles())
            {
                FileSecurity fileSecurity = file.GetAccessControl();
                IdentityReference identityRef = fileSecurity.GetOwner(typeof(NTAccount));
                string fsar = "";

                foreach (FileSystemAccessRule permission in fileSecurity.GetAccessRules(true, true, typeof(NTAccount)))
                {
                    fsar += permission.FileSystemRights.ToString();
                }
                dirSize += file.Length;

                Elements.Add(new Items()
                {
                    Name = file.Name,
                    CreationTime = file.CreationTime.ToString(),
                    LastWriteTime = file.LastWriteTime.ToString(),
                    LastAccessTime = file.LastAccessTime.ToString(),
                    Attributes = file.Attributes.ToString(),
                    Size = file.Length.ToString(),
                    Owner = identityRef.ToString(),
                    Permission = fsar,
                });
            }
            // добавляем инфо о директории
            Elements.Add(new Items()
            {
                Name = dir.Name,
                CreationTime = dir.CreationTime.ToString(),
                LastWriteTime = dir.LastWriteTime.ToString(),
                LastAccessTime = dir.LastAccessTime.ToString(),
                Attributes = dir.Attributes.ToString(),
                Size = dirSize.ToString(),
                Owner = dirIdentityRef.ToString(),
                Permission = dirfsar,
            });
        }

        void MouseSelected()
        {
            Elements.Clear();

            AddDirectories(newSelected);
            ListVievWrite();

        }

        void ListVievWrite()
        {
                listView1.Items.Clear();


                DirectoryInfo nodeDirInfo = new DirectoryInfo(newSelected.FullPath);

                ListViewItem item = null;

                foreach (DirectoryInfo dir in nodeDirInfo.GetDirectories())
                {
                    
                    var itemInfo = Elements.Find(x => x.Name == dir.Name);
                    Console.WriteLine(itemInfo.Name);
                    item = new ListViewItem(itemInfo.Name, 0);
                    string mb = (Int64.Parse(itemInfo.Size) > 1048576) ? (Int64.Parse(itemInfo.Size) / 1048576).ToString() + "Mb" :
                        (Int64.Parse(itemInfo.Size) > 1024) ? (Int64.Parse(itemInfo.Size) / 1024).ToString() + "Kb" : itemInfo.Size + "b";

                    item.SubItems.Add(mb);
                    item.SubItems.Add(itemInfo.LastWriteTime);
                    listView1.Items.Add(item);
                }
                foreach (FileInfo file in nodeDirInfo.GetFiles())
                {
                    item = new ListViewItem(file.Name, 1);
                    string mb = (file.Length > 1048576) ? (file.Length / 1048576).ToString() + "Mb" :
                        (file.Length > 1024) ? (file.Length / 1024).ToString() + "Kb" : file.Length.ToString() + "byte";

                    item.SubItems.Add(mb);
                    item.SubItems.Add(file.LastWriteTime.ToString());
                    listView1.Items.Add(item);
                }

        }

        void SaveXml()
        {
            
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "XML files(.xml)|*.xml";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.Title = "Сохранение результатов";

            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if ((myStream = saveFileDialog1.OpenFile()) != null)
                    {
                        demoThread = new Thread(SaveInfo);
                        demoThread.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        void SaveInfo()
        {
            Console.WriteLine("Поток " + demoThread.GetHashCode() + " стартанул");

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineOnAttributes = true;
            writer = XmlWriter.Create(myStream, settings);

            writer.WriteStartElement("Info");

            foreach (Items el in Elements)
            {
                writer.WriteStartElement("Item");
                writer.WriteElementString("Name", el.Name);
                writer.WriteElementString("CreationTime", el.CreationTime);
                writer.WriteElementString("LastWriteTime", el.LastWriteTime);
                writer.WriteElementString("LastAccessTime", el.LastAccessTime);
                writer.WriteElementString("Attributes", el.Attributes);
                writer.WriteElementString("Size", el.Size);
                writer.WriteElementString("Owner", el.Owner);
                writer.WriteElementString("Permission", el.Permission);
                writer.WriteEndElement();

            }


            writer.WriteEndElement();

            writer.Close();
            myStream.Close();

            Console.WriteLine("Поток " + demoThread.GetHashCode() + " погиб");

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            newSelected = e.Node;

            MouseSelected();

            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveXml();
        }
    }
    public class Items
    {
        public string Name;
        public string CreationTime;
        public string LastWriteTime;
        public string LastAccessTime;
        public string Attributes;
        public string Size;
        public string Owner;
        public string Permission;
    }
}
