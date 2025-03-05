using CourseProgram.Services;
using CourseProgram.ViewModels;
using System;
using System.IO;
using System.Threading.Tasks;
using Spire.Doc;
using Spire.Doc.Documents;
using System.Windows;

namespace CourseProgram.Commands
{
    public class CreateItineraryCommand : CommandBaseAsync
    {
        private readonly CreatingItineraryViewModel _viewModel; 

        public CreateItineraryCommand(CreatingItineraryViewModel viewModel)
        {
            _viewModel = viewModel;

            _viewModel.PropertyChanged += _viewModel_PropertyChanged;
        }

        private void _viewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.SelectedDriver))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) && _viewModel.SelectedDriver != null;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                IsExecuting = true;

                foreach (var item in _viewModel.Routes)
                {
                    var model = item.GetModel();
                    if (model.MachineID == null || model.DriverID == null || model.AddressStartID == null || model.AddressEndID == null)
                    {
                        MessageBox.Show($"Маршрут с номером {_viewModel.Routes.IndexOf(item) + 1}, не заполнен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "МаршрутныеЛисты");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string fileName = $"МаршрутныйЛист_{DateOnly.FromDateTime(DateTime.Now)}_{_viewModel.SelectedDriver.FIO.Replace(" ", "")}.docx";
                string filePath = Path.Combine(directoryPath, fileName);

                Document doc = new Document();
                Section section = doc.AddSection();

                Paragraph title = section.AddParagraph();
                title.AppendText("Маршрутный лист").CharacterFormat.Bold = true;
                title.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;

                Paragraph datePara = section.AddParagraph();
                datePara.AppendText($"Дата: {DateTime.Now:dd-MM-yyyy}");

                Paragraph driverPara = section.AddParagraph();
                driverPara.AppendText($"Водитель: {_viewModel.SelectedDriver.FIO}");

                Table table = section.AddTable(true);
                table.ResetCells(_viewModel.Routes.Count + 1, 4);

                TableRow header = table.Rows[0];
                header.Cells[0].AddParagraph().AppendText("Очередность");
                header.Cells[1].AddParagraph().AppendText("Техника");
                header.Cells[2].AddParagraph().AppendText("Пункт отправления");
                header.Cells[3].AddParagraph().AppendText("Пункт назначения");

                for (int i = 0; i < _viewModel.Routes.Count; ++i)
                {
                    var route = _viewModel.Routes[i];
                    TableRow dataRow = table.Rows[i + 1];
                    dataRow.Cells[0].AddParagraph().AppendText($"{i + 1}");
                    dataRow.Cells[1].AddParagraph().AppendText($"{route.MachineName}");
                    dataRow.Cells[2].AddParagraph().AppendText($"{route.AddressStart}");
                    dataRow.Cells[3].AddParagraph().AppendText($"{route.AddressEnd}");
                }

                doc.SaveToFile(filePath, FileFormat.Docx);

                MessageBox.Show("Файл создан!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                await LogManager.Instance.WriteLogAsync($"ERROR in {nameof(CreateItineraryCommand)}: {ex.Message}");
                MessageBox.Show("Неизвестная ошибка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsExecuting = false;
            }
        }
    }
}