using CSFExtractor;
using System.Data;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace CSFReader
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
        }

        private void Execute(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            var dialogResult = dialog.ShowDialog();
            var directory = "";

            if (dialogResult == DialogResult.OK)
            {
                directory = dialog.SelectedPath;
                var files = Directory.EnumerateFiles(directory, "*.pdf");
                List<CSF> data = new();
                var global = "";
                foreach (var file in files)
                {
                    var result = ShowContent(file);
                    data.Add(result);
                    global += result.GetText() + "\n\n";
                }

                var dataSource = ToDataTable(data);

                ResultsTable.DataSource = dataSource;
            }
        }

        private static CSF ShowContent(string file)
        {
            CSF archivo;
            try
            {
                archivo = CSF.Load(file);
            }
            catch (Exception ex)
            {
                archivo = new()
                {
                    FileName = Path.GetFileName(file),
                    RFC = ex.Message
                };
            }
            return archivo;
        }

        private void Main_SizeChanged(object sender, EventArgs e)
        {
            //ResultsTable.Width = Width - 5;
            //ResultsTable.Height = Height - ResultsTable.Top - 5;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null) ?? "";
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
}