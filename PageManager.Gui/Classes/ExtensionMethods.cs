using Newtonsoft.Json;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PageManager.Gui.Classes {
    public static class ExtensionMethods {
        public static void SaveToJson(this object self, string path) {
            var jsonString = JsonConvert.SerializeObject(self);
            File.WriteAllText(path, jsonString);
        }

        public static void AddColumn(this DataGrid self, string name) {
            var column = new DataGridTextColumn {
                Header = name,
                Binding = new Binding(name)
            };
            self.Columns.Add(column);
        }

        public static void AddColumns(this DataGrid self, string[] columnsNames) {
            foreach (var name in columnsNames) {
                self.AddColumn(name);
            }
        }

        public static void Clear(this DataGrid self) {
            self.ItemsSource = null;
            self.Items.Clear();
        }

        public static string GetValue(this DataGrid self, int rowIndex, int columnIndex) {
            DataGridCellInfo cellInfo = self.SelectedCells[columnIndex];
            var item = self.Items[rowIndex];
            var content = (cellInfo.Column.GetCellContent(item) as TextBlock)?.Text;
            return content;
        }

        public static string GetSelectedValue(this DataGrid self) {
            var columnIndex = self.Columns.IndexOf(self.CurrentColumn);
            DataGridCellInfo cellInfo = self.SelectedCells[columnIndex];
            var content = (cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock)?.Text;
            return content;
        }

        public static string GetSelectedValue(this DataGrid self, int columnIndex) {
            DataGridCellInfo cellInfo = self.SelectedCells[columnIndex];
            var content = (cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock)?.Text;
            return content;
        }

        public static void SetCurrentSizeToMin(this Window self) {
            self.MinWidth = self.Width;
            self.MinHeight = self.Height;
        }
    }
}
